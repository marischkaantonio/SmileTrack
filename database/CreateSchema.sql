-- Creates SmileTrackDB and required tables. Run in master or SSMS.
IF DB_ID('SmileTrackDB') IS NULL
BEGIN
    CREATE DATABASE [SmileTrackDB];
END
GO

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
    PatientID INT NOT NULL REFERENCES dbo.Patients(PatientID),
    AppointmentDateTime DATETIME2 NOT NULL,
    Dentist NVARCHAR(200) NULL,
    Treatment NVARCHAR(500) NULL,
    Status NVARCHAR(50) NULL,
    VisitType NVARCHAR(50) NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME()
);
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Invoices]') AND type in (N'U'))
BEGIN
CREATE TABLE dbo.Invoices
(
    InvoiceID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceNo NVARCHAR(50) NOT NULL,
    PatientID INT NULL REFERENCES dbo.Patients(PatientID),
    InvoiceDate DATETIME2 NOT NULL,
    DueDate DATETIME2 NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    PaidAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    BalanceAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME()
);
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvoiceItems]') AND type in (N'U'))
BEGIN
CREATE TABLE dbo.InvoiceItems
(
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceID INT NOT NULL REFERENCES dbo.Invoices(InvoiceID),
    Treatment NVARCHAR(200),
    Description NVARCHAR(500),
    Qty INT,
    UnitPrice DECIMAL(18,2),
    Amount DECIMAL(18,2)
);
END
GO          