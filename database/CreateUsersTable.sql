CREATE TABLE dbo.UsersSimple
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(200) NOT NULL, -- plain text (insecure)
    Role NVARCHAR(50) NOT NULL DEFAULT 'User',
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE INDEX IX_UsersSimple_UserName ON dbo.UsersSimple(UserName);