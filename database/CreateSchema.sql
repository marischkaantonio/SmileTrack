-- Creates SmileTrackDB and required tables. Run in master or SSMS.
IF DB_ID('SmileTrackDB') IS NULL
BEGIN
    CREATE DATABASE [SmileTrackDB];
END
GO

-- Add foreign keys with cascade deletes for referential integrity (safe idempotent operations)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Appointments_Patients')
BEGIN
    ALTER TABLE dbo.Appointments DROP CONSTRAINT IF EXISTS FK_Appointments_Patients;
    ALTER TABLE dbo.Appointments ADD CONSTRAINT FK_Appointments_Patients FOREIGN KEY (PatientID) REFERENCES dbo.Patients(PatientID) ON DELETE CASCADE;
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Invoices_Patients')
BEGIN
    ALTER TABLE dbo.Invoices DROP CONSTRAINT IF EXISTS FK_Invoices_Patients;
    ALTER TABLE dbo.Invoices ADD CONSTRAINT FK_Invoices_Patients FOREIGN KEY (PatientID) REFERENCES dbo.Patients(PatientID) ON DELETE CASCADE;
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_InvoiceItems_Invoices')
BEGIN
    ALTER TABLE dbo.InvoiceItems DROP CONSTRAINT IF EXISTS FK_InvoiceItems_Invoices;
    ALTER TABLE dbo.InvoiceItems ADD CONSTRAINT FK_InvoiceItems_Invoices FOREIGN KEY (InvoiceID) REFERENCES dbo.Invoices(InvoiceID) ON DELETE CASCADE;
END

-- Indexes to help queries common in the app
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Appointments_AppointmentDateTime')
    CREATE NONCLUSTERED INDEX IX_Appointments_AppointmentDateTime ON dbo.Appointments(AppointmentDateTime);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Appointments_PatientID')
    CREATE NONCLUSTERED INDEX IX_Appointments_PatientID ON dbo.Appointments(PatientID);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Invoices_PatientID')
    CREATE NONCLUSTERED INDEX IX_Invoices_PatientID ON dbo.Invoices(PatientID);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_InvoiceItems_InvoiceID')
    CREATE NONCLUSTERED INDEX IX_InvoiceItems_InvoiceID ON dbo.InvoiceItems(InvoiceID);

USE [SmileTrackDB];
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Patients]') AND type in (N'U'))
BEGIN
CREATE TABLE dbo.Patients
(
    PatientID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    BirthDate DATE NULL,
    Age INT NULL,
    Gender NVARCHAR(10) NULL,
    ContactNo NVARCHAR(50) NULL,
    Email NVARCHAR(200) NULL,
    Address NVARCHAR(500) NULL,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME()
);
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Appointments]') AND type in (N'U'))
BEGIN
CREATE TABLE dbo.Appointments
(
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    PatientID INT NOT NULL,
    AppointmentDateTime DATETIME2 NOT NULL,
    Dentist NVARCHAR(200) NULL,
    Treatment NVARCHAR(500) NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Scheduled',
    VisitType NVARCHAR(50) NOT NULL DEFAULT '',
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Appointments_Patients FOREIGN KEY (PatientID) REFERENCES dbo.Patients(PatientID) ON DELETE CASCADE
);
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Invoices]') AND type in (N'U'))
BEGIN
CREATE TABLE dbo.Invoices
(
    InvoiceID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceNo NVARCHAR(50) NOT NULL,
    PatientID INT NULL,
    InvoiceDate DATETIME2 NOT NULL,
    DueDate DATETIME2 NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    PaidAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    BalanceAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT '',
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Invoices_Patients FOREIGN KEY (PatientID) REFERENCES dbo.Patients(PatientID) ON DELETE CASCADE
);
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvoiceItems]') AND type in (N'U'))
BEGIN
CREATE TABLE dbo.InvoiceItems
(
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceID INT NOT NULL,
    Treatment NVARCHAR(200),
    Description NVARCHAR(500),
    Qty INT,
    UnitPrice DECIMAL(18,2),
    Amount DECIMAL(18,2),
    CONSTRAINT FK_InvoiceItems_Invoices FOREIGN KEY (InvoiceID) REFERENCES dbo.Invoices(InvoiceID) ON DELETE CASCADE
);
END
GO          