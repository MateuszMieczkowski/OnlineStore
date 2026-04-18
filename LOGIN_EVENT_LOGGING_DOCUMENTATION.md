## Login Event Logging Implementation

### Overview
This implementation adds comprehensive login event logging to the OnlineStore application. It tracks:
1. **Last successful login date/time**
2. **Last failed login attempt date/time**
3. **Number of failed login attempts since last successful login**

### Components Implemented

#### 1. **Database Entity: LoginEvent** (`Server/Entities/LoginEvent.cs`)
- Stores detailed login event records
- Fields:
  - `Id`: Unique identifier
  - `UserId`: Reference to the user
  - `IsSuccessful`: Whether the login succeeded
  - `FailureReason`: Reason for failure (if applicable)
  - `IpAddress`: IP address of the login attempt
  - `UserAgent`: Browser/client information
  - `CreatedDate`: Timestamp of the event

#### 2. **User Entity Updates** (`Server/Entities/User.cs`)
Enhanced with login tracking properties:
- `LastSuccessfulLoginAt`: DateTime of last successful login
- `LastFailedLoginAt`: DateTime of last failed login attempt
- `FailedLoginAttemptsSinceLastSuccess`: Counter of failed attempts

New methods:
- `RecordSuccessfulLogin()`: Updates successful login timestamp and resets counter
- `RecordFailedLogin()`: Updates failed login timestamp and increments counter

#### 3. **Database Migration** 
- `20260315120000_AddLoginEventTracking.cs`: Creates LoginEvents table and adds columns to Users table
- `20260315120000_AddLoginEventTracking.Designer.cs`: Migration metadata

#### 4. **LoginEventService** (`Server/Features/Accounts/Services/LoginEventService.cs`)
Handles all login event operations:

**Interface Methods:**
- `RecordLoginEventAsync()`: Records a login attempt (successful or failed)
- `GetLastSuccessfulLoginAsync()`: Retrieves the last successful login details
- `GetLastFailedLoginAsync()`: Retrieves the last failed login attempt details
- `GetFailedLoginCountSinceLastSuccessAsync()`: Gets the count of failed attempts since last success
- `GetLoginHistoryAsync()`: Retrieves the last N login events for a user

#### 5. **Authentication Handler Updates** (`Server/Features/Accounts/AuthenticateUser/AuthenticateUserCommandHandler.cs`)
Enhanced to record login events:
- Records failed login attempts with IP address and User-Agent
- Records successful logins with IP address
- Includes login event data in the response

#### 6. **Data Transfer Objects (DTOs)**

**LoginEventDto** (`Shared/Accounts/LoginEventDto.cs`):
```csharp
public class LoginEventDto
{
    public DateTime? LastSuccessfulLoginAt { get; set; }
    public DateTime? LastFailedLoginAt { get; set; }
    public int FailedLoginAttemptsSinceLastSuccess { get; set; }
}
```

**LoginEventDetailDto** (`Shared/Accounts/LoginEventDetailDto.cs`):
```csharp
public record LoginEventDetailDto(
    int Id,
    DateTime EventDate,
    bool IsSuccessful,
    string? FailureReason,
    string? IpAddress,
    string? UserAgent);

public record LoginEventDetailsDto(
    List<LoginEventDetailDto> LoginEvents,
    DateTime? LastSuccessfulLoginAt,
    DateTime? LastFailedLoginAt);
```

**AuthResponse Enhancement** (`Shared/Models/AuthResponse.cs`):
Extended to include login event information returned after authentication

#### 7. **API Endpoints** (`Server/Controllers/AccountController.cs`)

**1. Login Endpoint** (Existing, Enhanced)
- `POST /api/account/login`
- Now returns login event data in the response

**2. Get Login Summary**
- `GET /api/account/login-summary`
- Returns: `LoginEventDto` with last success/failure dates and failed attempt count
- Requires authentication

**3. Get Login History**
- `GET /api/account/login-events?limit=10`
- Returns: `LoginEventDetailsDto` with detailed event history
- Parameters: `limit` (default: 10)
- Requires authentication

#### 8. **Entity Configuration** (`Server/Entities/Configurations/LoginEventConfiguration.cs`)
Configures:
- Primary key
- Foreign key relationship to Users
- Column constraints (MaxLength)
- Indexes for performance:
  - Index on UserId
  - Index on CreatedDate
  - Compound index on (UserId, CreatedDate)

#### 9. **Dependency Injection** (`Server/Program.cs`)
Registered:
```csharp
builder.Services.AddScoped<ILoginEventService, LoginEventService>();
```

### Database Schema

**LoginEvents Table:**
```sql
CREATE TABLE LoginEvents (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    IsSuccessful BIT NOT NULL,
    FailureReason NVARCHAR(500),
    IpAddress NVARCHAR(50),
    UserAgent NVARCHAR(500),
    CreatedDate DATETIME2 NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
)

CREATE INDEX IX_LoginEvents_UserId ON LoginEvents(UserId)
CREATE INDEX IX_LoginEvents_CreatedDate ON LoginEvents(CreatedDate)
CREATE INDEX IX_LoginEvents_UserId_CreatedDate ON LoginEvents(UserId, CreatedDate)
```

**Users Table Updates:**
```sql
ALTER TABLE Users ADD
    LastSuccessfulLoginAt DATETIME2 NULL,
    LastFailedLoginAt DATETIME2 NULL,
    FailedLoginAttemptsSinceLastSuccess INT DEFAULT 0
```

### Usage Examples

#### 1. **Get Login Summary for Current User**
```csharp
GET /api/account/login-summary
Authorization: Bearer {token}

Response:
{
    "lastSuccessfulLoginAt": "2026-03-15T10:30:00",
    "lastFailedLoginAt": "2026-03-15T09:45:00",
    "failedLoginAttemptsSinceLastSuccess": 2
}
```

#### 2. **Get Login History**
```csharp
GET /api/account/login-events?limit=5
Authorization: Bearer {token}

Response:
{
    "loginEvents": [
        {
            "id": 1,
            "eventDate": "2026-03-15T10:30:00",
            "isSuccessful": true,
            "failureReason": null,
            "ipAddress": "192.168.1.100",
            "userAgent": "Mozilla/5.0..."
        },
        {
            "id": 2,
            "eventDate": "2026-03-15T09:45:00",
            "isSuccessful": false,
            "failureReason": "Invalid password",
            "ipAddress": "192.168.1.100",
            "userAgent": "Mozilla/5.0..."
        }
    ],
    "lastSuccessfulLoginAt": "2026-03-15T10:30:00",
    "lastFailedLoginAt": "2026-03-15T09:45:00"
}
```

#### 3. **Login Response with Event Data**
```csharp
POST /api/account/login

Response:
{
    "id": 1,
    "email": "user@example.com",
    "token": "eyJhbGc...",
    "preferences": {...},
    "lastSuccessfulLoginAt": "2026-03-15T10:30:00",
    "lastFailedLoginAt": "2026-03-15T09:45:00",
    "failedLoginAttemptsSinceLastSuccess": 0
}
```

### Migration Steps

1. **Apply the migration:**
   ```bash
   dotnet ef database update
   ```

2. **Verify the new tables and columns were created:**
   - `LoginEvents` table
   - `Users` table columns: `LastSuccessfulLoginAt`, `LastFailedLoginAt`, `FailedLoginAttemptsSinceLastSuccess`

3. **Test the endpoints:**
   - Login and verify the response includes login event data
   - Check login summary endpoint
   - Review login history

### Security Considerations

1. **IP Address Logging**: Useful for detecting suspicious activity
2. **User-Agent Logging**: Helps identify different devices/browsers
3. **Failed Attempt Tracking**: Can be used to implement account lockout policies
4. **Access Control**: Login event endpoints require authentication

### Future Enhancements

1. Implement account lockout after N failed attempts
2. Add login anomaly detection (unusual locations/devices)
3. Create audit trail visualization in UI
4. Implement email notifications for suspicious login attempts
5. Add geographic location information from IP
6. Create admin dashboard for monitoring login attempts across all users

### Files Created/Modified

**Created:**
- `Server/Entities/LoginEvent.cs`
- `Server/Entities/Configurations/LoginEventConfiguration.cs`
- `Server/Features/Accounts/Services/LoginEventService.cs`
- `Server/Migrations/20260315120000_AddLoginEventTracking.cs`
- `Server/Migrations/20260315120000_AddLoginEventTracking.Designer.cs`
- `Shared/Accounts/LoginEventDto.cs`
- `Shared/Accounts/LoginEventDetailDto.cs`

**Modified:**
- `Server/Entities/User.cs`
- `Server/OnlineStoreDbContext.cs`
- `Server/Program.cs`
- `Server/Features/Accounts/AuthenticateUser/AuthenticateUserCommandHandler.cs`
- `Server/Controllers/AccountController.cs`
- `Shared/Models/AuthResponse.cs`

