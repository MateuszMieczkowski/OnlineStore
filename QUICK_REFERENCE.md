# Login Event Logging - Quick Reference Guide

## 🎯 Feature Summary

Comprehensive login event logging system that tracks:
1. **Last successful login date/time**
2. **Last failed login attempt date/time**
3. **Number of failed attempts since last successful login**

## 📂 Key Files Location

| File | Location | Purpose |
|------|----------|---------|
| LoginEvent Entity | `Server/Entities/LoginEvent.cs` | Database entity for login events |
| User Entity (Updated) | `Server/Entities/User.cs` | Added login tracking properties |
| LoginEventService | `Server/Features/Accounts/Services/LoginEventService.cs` | Business logic for events |
| AccountController (Updated) | `Server/Controllers/AccountController.cs` | New API endpoints |
| Database Config | `Server/Entities/Configurations/LoginEventConfiguration.cs` | EF Core mapping |
| Migration | `Server/Migrations/20260315120000_AddLoginEventTracking.cs` | Database schema changes |
| Shared DTOs | `Shared/Accounts/LoginEvent*.cs` | Data transfer objects |

## 🔌 API Endpoints

### 1. Login (Enhanced)
```
POST /api/account/login
Authorization: None (AnonymousAllowed)

Request:
{
    "email": "user@example.com",
    "password": "password123"
}

Response:
{
    "id": 1,
    "email": "user@example.com",
    "token": "eyJhbGc...",
    "preferences": {...},
    "lastSuccessfulLoginAt": "2026-03-15T10:30:00Z",
    "lastFailedLoginAt": "2026-03-15T09:45:00Z",
    "failedLoginAttemptsSinceLastSuccess": 0
}
```

### 2. Login Summary
```
GET /api/account/login-summary
Authorization: Bearer {token}

Response:
{
    "lastSuccessfulLoginAt": "2026-03-15T10:30:00Z",
    "lastFailedLoginAt": "2026-03-15T09:45:00Z",
    "failedLoginAttemptsSinceLastSuccess": 2
}
```

### 3. Login History
```
GET /api/account/login-events?limit=10
Authorization: Bearer {token}

Response:
{
    "loginEvents": [
        {
            "id": 1,
            "eventDate": "2026-03-15T10:30:00Z",
            "isSuccessful": true,
            "failureReason": null,
            "ipAddress": "192.168.1.100",
            "userAgent": "Mozilla/5.0..."
        },
        {
            "id": 2,
            "eventDate": "2026-03-15T09:45:00Z",
            "isSuccessful": false,
            "failureReason": "Invalid password",
            "ipAddress": "192.168.1.100",
            "userAgent": "Mozilla/5.0..."
        }
    ],
    "lastSuccessfulLoginAt": "2026-03-15T10:30:00Z",
    "lastFailedLoginAt": "2026-03-15T09:45:00Z"
}
```

## 🗄️ Database Schema

### LoginEvents Table
```sql
CREATE TABLE LoginEvents (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    IsSuccessful BIT NOT NULL,
    FailureReason NVARCHAR(500) NULL,
    IpAddress NVARCHAR(50) NULL,
    UserAgent NVARCHAR(500) NULL,
    CreatedDate DATETIME2 NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
)

-- Indexes for performance
CREATE INDEX IX_LoginEvents_UserId ON LoginEvents(UserId)
CREATE INDEX IX_LoginEvents_CreatedDate ON LoginEvents(CreatedDate)
CREATE INDEX IX_LoginEvents_UserId_CreatedDate ON LoginEvents(UserId, CreatedDate)
```

### Users Table (New Columns)
```sql
ALTER TABLE Users ADD
    LastSuccessfulLoginAt DATETIME2 NULL,
    LastFailedLoginAt DATETIME2 NULL,
    FailedLoginAttemptsSinceLastSuccess INT DEFAULT 0
```

## 🛠️ Implementation Details

### Service Interface
```csharp
public interface ILoginEventService
{
    Task RecordLoginEventAsync(int userId, bool isSuccessful, 
        string? failureReason = null, string? ipAddress = null, 
        string? userAgent = null, CancellationToken cancellationToken = default);
    
    Task<LoginEventDto?> GetLastSuccessfulLoginAsync(int userId, 
        CancellationToken cancellationToken = default);
    
    Task<LoginEventDto?> GetLastFailedLoginAsync(int userId, 
        CancellationToken cancellationToken = default);
    
    Task<int> GetFailedLoginCountSinceLastSuccessAsync(int userId, 
        CancellationToken cancellationToken = default);
    
    Task<List<LoginEventDetailDto>> GetLoginHistoryAsync(int userId, int limit = 10, 
        CancellationToken cancellationToken = default);
}
```

### User Entity Methods
```csharp
public void RecordSuccessfulLogin()
{
    LastSuccessfulLoginAt = DateTime.UtcNow;
    FailedLoginAttemptsSinceLastSuccess = 0;
}

public void RecordFailedLogin()
{
    LastFailedLoginAt = DateTime.UtcNow;
    FailedLoginAttemptsSinceLastSuccess++;
}
```

## 🔐 Security Features

✅ **IP Address Logging** - Every login attempt includes the client IP
✅ **User-Agent Logging** - Browser/device information captured
✅ **Failed Attempt Tracking** - Enables brute-force protection
✅ **Event Persistence** - Complete audit trail maintained
✅ **Access Control** - Events require authentication to view
✅ **Immutable Events** - Login events cannot be modified

## 🚀 Deployment Guide

### Step 1: Apply Migration
```bash
cd Server
dotnet ef database update
```

### Step 2: Verify Database
```sql
-- Check tables exist
SELECT * FROM LoginEvents
SELECT LastSuccessfulLoginAt, LastFailedLoginAt, FailedLoginAttemptsSinceLastSuccess 
FROM Users

-- Check indexes
EXEC sp_helpindex 'LoginEvents'
```

### Step 3: Test Endpoints
```bash
# Login
POST http://localhost:5000/api/account/login
Content-Type: application/json

{
    "email": "admin@example.com",
    "password": "password123"
}

# Save token from response, then:

# Check summary
GET http://localhost:5000/api/account/login-summary
Authorization: Bearer {token}

# Check history
GET http://localhost:5000/api/account/login-events?limit=10
Authorization: Bearer {token}
```

## 🔧 Common Tasks

### Get a User's Last Login
```csharp
var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
var lastLoginDate = user.LastSuccessfulLoginAt;
```

### Count Failed Attempts Since Last Success
```csharp
var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
var failedAttempts = user.FailedLoginAttemptsSinceLastSuccess;
```

### Retrieve Login History
```csharp
var events = await loginEventService.GetLoginHistoryAsync(userId, limit: 20);
foreach(var evt in events)
{
    Console.WriteLine($"{evt.EventDate}: {(evt.IsSuccessful ? "SUCCESS" : "FAILED")}");
}
```

### Check if User Had Recent Failed Attempts
```csharp
var failureReason = await loginEventService.GetLastFailedLoginAsync(userId);
if(failureReason?.EventDate > DateTime.UtcNow.AddHours(-1))
{
    // Failed attempt within last hour
}
```

## 🐛 Troubleshooting

| Issue | Solution |
|-------|----------|
| Migration fails | Ensure SQL Server is running, check connection string |
| Events not recording | Verify migration was applied, check ILoginEventService injection |
| Endpoints return 401 | Ensure JWT token is valid and in Authorization header |
| IP address always null | Check IHttpContextAccessor is registered in DI |
| Performance issues | Verify indexes were created on LoginEvents table |

## 📊 Database Queries

### Get User's Login Stats
```sql
SELECT 
    u.Id, u.Email,
    u.LastSuccessfulLoginAt,
    u.LastFailedLoginAt,
    u.FailedLoginAttemptsSinceLastSuccess,
    (SELECT COUNT(*) FROM LoginEvents WHERE UserId = u.Id AND IsSuccessful = 1) AS TotalLogins,
    (SELECT COUNT(*) FROM LoginEvents WHERE UserId = u.Id AND IsSuccessful = 0) AS TotalFailures
FROM Users u
WHERE u.Id = @UserId
```

### Get Recent Login Activity
```sql
SELECT TOP 100
    UserId, EventDate = CreatedDate, IsSuccessful, FailureReason, IpAddress
FROM LoginEvents
ORDER BY CreatedDate DESC
```

### Get Users with Failed Attempts
```sql
SELECT u.Id, u.Email, u.FailedLoginAttemptsSinceLastSuccess
FROM Users u
WHERE u.FailedLoginAttemptsSinceLastSuccess > 0
ORDER BY u.FailedLoginAttemptsSinceLastSuccess DESC
```

## 📚 Related Documentation

- **LOGIN_EVENT_LOGGING_DOCUMENTATION.md** - Comprehensive feature documentation
- **IMPLEMENTATION_SUMMARY.md** - Implementation details with examples
- **FILE_INVENTORY.md** - Complete file listing and status

---

**Status:** ✅ Production Ready
**Last Updated:** 2026-03-15

