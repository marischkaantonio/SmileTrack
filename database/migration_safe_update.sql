-- Migration-safe schema & data repair script for SmileTrackDB
-- Purpose: normalize existing data, remove orphans, add NOT NULL defaults, add/replace FK constraints with ON DELETE CASCADE, and create helpful indexes.
-- Run in context of SmileTrackDB (USE [SmileTrackDB])

SET XACT_ABORT ON;
GO

BEGIN TRY
	BEGIN TRANSACTION;

	-- 1) Normalize NULL or empty values so we can safely add NOT NULL constraints
	UPDATE dbo.Appointments SET Status = 'Scheduled' WHERE Status IS NULL OR LTRIM(RTRIM(Status)) = '';
	UPDATE dbo.Appointments SET VisitType = '' WHERE VisitType IS NULL;
	UPDATE dbo.Invoices SET Status = '' WHERE Status IS NULL;

	-- 2) Remove orphaned child rows that would violate new FKs
	-- Remove invoice items whose invoice no longer exists
	DELETE ii
	FROM dbo.InvoiceItems ii
	LEFT JOIN dbo.Invoices i ON ii.InvoiceID = i.InvoiceID
	WHERE i.InvoiceID IS NULL;

	-- Remove invoices that reference non-existing patients (safe: invoices without patient are removed)
	DELETE i
	FROM dbo.Invoices i
	LEFT JOIN dbo.Patients p ON i.PatientID = p.PatientID
	WHERE i.PatientID IS NOT NULL AND p.PatientID IS NULL;

	-- Remove appointments that reference non-existing patients
	DELETE a
	FROM dbo.Appointments a
	LEFT JOIN dbo.Patients p ON a.PatientID = p.PatientID
	WHERE p.PatientID IS NULL;

	-- 3) Ensure column definitions are non-nullable and add DEFAULT constraints (idempotent checks)
	IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Appointments') AND name = 'Status')
	BEGIN
		-- Make column NOT NULL (we already fixed NULLs above)
		ALTER TABLE dbo.Appointments ALTER COLUMN Status NVARCHAR(50) NOT NULL;
		-- Add default constraint if missing
		IF NOT EXISTS (
			SELECT 1 FROM sys.default_constraints dc
			JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
			WHERE OBJECT_NAME(dc.parent_object_id) = 'Appointments' AND c.name = 'Status')
		BEGIN
			ALTER TABLE dbo.Appointments ADD CONSTRAINT DF_Appointments_Status DEFAULT ('Scheduled') FOR Status;
		END
	END

	IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Appointments') AND name = 'VisitType')
	BEGIN
		ALTER TABLE dbo.Appointments ALTER COLUMN VisitType NVARCHAR(50) NOT NULL;
		IF NOT EXISTS (
			SELECT 1 FROM sys.default_constraints dc
			JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
			WHERE OBJECT_NAME(dc.parent_object_id) = 'Appointments' AND c.name = 'VisitType')
		BEGIN
			ALTER TABLE dbo.Appointments ADD CONSTRAINT DF_Appointments_VisitType DEFAULT ('') FOR VisitType;
		END
	END

	IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Invoices') AND name = 'Status')
	BEGIN
		ALTER TABLE dbo.Invoices ALTER COLUMN Status NVARCHAR(50) NOT NULL;
		IF NOT EXISTS (
			SELECT 1 FROM sys.default_constraints dc
			JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
			WHERE OBJECT_NAME(dc.parent_object_id) = 'Invoices' AND c.name = 'Status')
		BEGIN
			ALTER TABLE dbo.Invoices ADD CONSTRAINT DF_Invoices_Status DEFAULT ('') FOR Status;
		END
	END

	-- 4) (Re)create foreign key constraints with ON DELETE CASCADE. Drop existing variant first if needed.
	IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Appointments_Patients')
	BEGIN
		ALTER TABLE dbo.Appointments DROP CONSTRAINT FK_Appointments_Patients;
	END
	IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Appointments_Patients')
	BEGIN
		ALTER TABLE dbo.Appointments ADD CONSTRAINT FK_Appointments_Patients FOREIGN KEY (PatientID) REFERENCES dbo.Patients(PatientID) ON DELETE CASCADE;
	END

	IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Invoices_Patients')
	BEGIN
		ALTER TABLE dbo.Invoices DROP CONSTRAINT FK_Invoices_Patients;
	END
	IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Invoices_Patients')
	BEGIN
		ALTER TABLE dbo.Invoices ADD CONSTRAINT FK_Invoices_Patients FOREIGN KEY (PatientID) REFERENCES dbo.Patients(PatientID) ON DELETE CASCADE;
	END

	IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_InvoiceItems_Invoices')
	BEGIN
		ALTER TABLE dbo.InvoiceItems DROP CONSTRAINT FK_InvoiceItems_Invoices;
	END
	IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_InvoiceItems_Invoices')
	BEGIN
		ALTER TABLE dbo.InvoiceItems ADD CONSTRAINT FK_InvoiceItems_Invoices FOREIGN KEY (InvoiceID) REFERENCES dbo.Invoices(InvoiceID) ON DELETE CASCADE;
	END

	-- 5) Create helpful indexes if they don't already exist
	IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Appointments_AppointmentDateTime' AND object_id = OBJECT_ID('dbo.Appointments'))
		CREATE NONCLUSTERED INDEX IX_Appointments_AppointmentDateTime ON dbo.Appointments(AppointmentDateTime);

	IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Appointments_PatientID' AND object_id = OBJECT_ID('dbo.Appointments'))
		CREATE NONCLUSTERED INDEX IX_Appointments_PatientID ON dbo.Appointments(PatientID);

	IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Invoices_PatientID' AND object_id = OBJECT_ID('dbo.Invoices'))
		CREATE NONCLUSTERED INDEX IX_Invoices_PatientID ON dbo.Invoices(PatientID);

	IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_InvoiceItems_InvoiceID' AND object_id = OBJECT_ID('dbo.InvoiceItems'))
		CREATE NONCLUSTERED INDEX IX_InvoiceItems_InvoiceID ON dbo.InvoiceItems(InvoiceID);

	COMMIT TRANSACTION;
	PRINT 'Migration completed successfully.';
END TRY
BEGIN CATCH
	DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
	DECLARE @ErrNum INT = ERROR_NUMBER();
	ROLLBACK TRANSACTION;
	RAISERROR('Migration failed: %d - %s', 16, 1, @ErrNum, @ErrMsg);
END CATCH
GO

-- End of migration script
