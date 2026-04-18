# Developer Checklist - Login Event Logging Implementation

## Pre-Deployment Tasks

### 1. Code Review & Verification
- [ ] Review all created files for correctness
- [ ] Review all modified files for completeness
- [ ] Check that all imports are properly added
- [ ] Verify no syntax errors in any file

### 2. Project Build
```bash
cd C:\Users\admin\RiderProjects\OnlineStore
dotnet build
```
- [ ] Build completes without errors
- [ ] No compilation warnings (or acceptable warnings)
- [ ] All references resolved

### 3. Database Preparation
- [ ] Backup current database
- [ ] Verify database connection string is correct
- [ ] Check SQL Server is running and accessible
- [ ] Verify user has permissions to create tables/indexes

### 4. Migration Application
```bash
cd Server
dotnet ef database update
```
- [ ] Migration applies successfully
- [ ] No errors in migration output
- [ ] All changes visible in database

### 5. Database Verification
Execute these SQL queries to verify:

```sql
-- Verify LoginEvents table exists
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'LoginEvents'
```
- [ ] Table exists

```sql
-- Verify Users table columns
SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Users' 
AND COLUMN_NAME IN ('LastSuccessfulLoginAt', 'LastFailedLoginAt', 
                     'FailedLoginAttemptsSinceLastSuccess')
```
- [ ] LastSuccessfulLoginAt column exists
- [ ] LastFailedLoginAt column exists
- [ ] FailedLoginAttemptsSinceLastSuccess column exists

```sql
-- Verify indexes created
SELECT name FROM sys.indexes 
WHERE object_id = OBJECT_ID('LoginEvents')
```
- [ ] IX_LoginEvents_UserId exists
- [ ] IX_LoginEvents_CreatedDate exists
- [ ] IX_LoginEvents_UserId_CreatedDate exists

### 6. Application Build & Run
```bash
dotnet run
```
- [ ] Application starts without errors
- [ ] No missing dependencies
- [ ] Logger shows clean startup
- [ ] API is accessible at configured port

### 7. Swagger/API Verification
- [ ] Open Swagger UI (if available)
- [ ] Verify new endpoints appear:
  - [ ] POST /api/account/login
  - [ ] GET /api/account/login-summary
  - [ ] GET /api/account/login-events

## Testing Tasks

### Test 1: Successful Login
```bash
curl -X POST http://localhost:5000/api/account/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"password"}'
```
- [ ] Request succeeds (200 status)
- [ ] Response includes token
- [ ] Response includes lastSuccessfulLoginAt
- [ ] Response includes lastFailedLoginAt (can be null)
- [ ] Response includes failedLoginAttemptsSinceLastSuccess (should be 0)
- [ ] Save token for next tests

### Test 2: Failed Login Attempt
```bash
curl -X POST http://localhost:5000/api/account/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"wrongpassword"}'
```
- [ ] Request fails (appropriate error status)
- [ ] Error message is appropriate

### Test 3: Verify Failed Event Recorded
```sql
SELECT TOP 1 * FROM LoginEvents 
ORDER BY CreatedDate DESC
```
- [ ] New LoginEvent record exists
- [ ] IsSuccessful = 0
- [ ] FailureReason = 'Invalid password'
- [ ] IpAddress is populated
- [ ] UserAgent is populated

### Test 4: Login Summary Endpoint
```bash
curl -X GET http://localhost:5000/api/account/login-summary \
  -H "Authorization: Bearer {TOKEN}"
```
- [ ] Returns 200 status
- [ ] Response includes lastSuccessfulLoginAt
- [ ] Response includes lastFailedLoginAt
- [ ] Response includes failedLoginAttemptsSinceLastSuccess

### Test 5: Login History Endpoint
```bash
curl -X GET "http://localhost:5000/api/account/login-events?limit=10" \
  -H "Authorization: Bearer {TOKEN}"
```
- [ ] Returns 200 status
- [ ] Response includes loginEvents array
- [ ] Each event includes: id, eventDate, isSuccessful, failureReason, ipAddress, userAgent
- [ ] Response includes lastSuccessfulLoginAt
- [ ] Response includes lastFailedLoginAt
- [ ] Limit parameter works (test with different values)

### Test 6: Authentication Required
```bash
curl -X GET http://localhost:5000/api/account/login-summary
```
- [ ] Returns 401 Unauthorized
- [ ] No data leaked

### Test 7: Multiple Failed Attempts
Perform 3 consecutive failed logins:
```bash
# 3 times with wrong password
curl -X POST http://localhost:5000/api/account/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"wrong"}'
```

Check User entity:
```sql
SELECT FailedLoginAttemptsSinceLastSuccess 
FROM Users WHERE Email = 'admin@example.com'
```
- [ ] FailedLoginAttemptsSinceLastSuccess = 3

### Test 8: Reset Failed Attempts Counter
Login successfully:
```bash
curl -X POST http://localhost:5000/api/account/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"correct"}'
```

Check User entity again:
```sql
SELECT FailedLoginAttemptsSinceLastSuccess 
FROM Users WHERE Email = 'admin@example.com'
```
- [ ] FailedLoginAttemptsSinceLastSuccess = 0
- [ ] LastSuccessfulLoginAt is updated to current time

### Test 9: Login History Shows All Events
```sql
SELECT COUNT(*) FROM LoginEvents 
WHERE UserId = (SELECT Id FROM Users WHERE Email = 'admin@example.com')
```
- [ ] Count matches number of login attempts
- [ ] Get endpoint returns all events (up to limit)

### Test 10: IP Address & User-Agent Captured
```sql
SELECT IpAddress, UserAgent FROM LoginEvents 
WHERE UserId = (SELECT Id FROM Users WHERE Email = 'admin@example.com')
```
- [ ] IpAddress is populated (not null)
- [ ] UserAgent is populated (not null)

## Performance Testing

### Query Performance
```sql
-- Test indexed queries
SET STATISTICS TIME ON
SELECT * FROM LoginEvents WHERE UserId = 1 ORDER BY CreatedDate DESC
SELECT * FROM LoginEvents WHERE CreatedDate > DATEADD(day, -7, GETUTCDATE())
SET STATISTICS TIME OFF
```
- [ ] Queries complete quickly
- [ ] Indexes are being used

### Table Growth
```sql
SELECT 
    COUNT(*) AS TotalRecords,
    (SELECT SUM(DATALENGTH(IpAddress)) FROM LoginEvents) AS DataSize
FROM LoginEvents
```
- [ ] Reasonable data size for current usage

## Multi-User Testing

### User 1 Login
- [ ] Record events
- [ ] Get login summary
- [ ] Get login history

### User 2 Login
- [ ] Record events
- [ ] Get login summary
- [ ] Get login history
- [ ] Verify User 1's events are NOT visible to User 2
- [ ] Verify User 2's events are NOT visible to User 1

## Error Handling

### Test Invalid Token
```bash
curl -X GET http://localhost:5000/api/account/login-summary \
  -H "Authorization: Bearer invalid-token"
```
- [ ] Returns 401 Unauthorized

### Test Expired Token
- [ ] Configure token with short expiry
- [ ] Wait for expiration
- [ ] Test endpoint - should return 401

### Test Non-Existent User
```bash
curl -X POST http://localhost:5000/api/account/login \
  -H "Content-Type: application/json" \
  -d '{"email":"nonexistent@example.com","password":"password"}'
```
- [ ] Returns appropriate error
- [ ] No event recorded (user doesn't exist)

## Final Verification

- [ ] All files exist and are correct
- [ ] Database schema matches expectations
- [ ] All endpoints working correctly
- [ ] Authentication working correctly
- [ ] Data isolation working correctly
- [ ] Performance acceptable
- [ ] Error handling appropriate
- [ ] Documentation is accurate
- [ ] Ready for production deployment

## Post-Deployment

### Production Monitoring
- [ ] Monitor application logs for errors
- [ ] Check database growth rate
- [ ] Monitor API response times
- [ ] Verify data is persisting correctly

### Documentation Update
- [ ] Update team wiki/documentation
- [ ] Brief team on new feature
- [ ] Provide endpoint documentation to frontend team
- [ ] Update API documentation

### Backup & Recovery
- [ ] Ensure database backups include LoginEvents table
- [ ] Test backup/restore procedures
- [ ] Document recovery procedures

## Sign-Off

- [ ] All tests passed
- [ ] All verification complete
- [ ] Documentation reviewed
- [ ] Ready for production

---

## Quick Reference Commands

### Build
```bash
dotnet build
```

### Apply Migration
```bash
dotnet ef database update
```

### Run Tests
```bash
dotnet test
```

### Start Application
```bash
dotnet run
```

### View Database
```bash
sqlcmd -S . -U sa -P YourPassword
SELECT * FROM LoginEvents
SELECT * FROM Users WHERE Id = 1
```

---

**Checklist Version:** 1.0
**Last Updated:** 2026-03-15
**Status:** Ready for Use ✓

