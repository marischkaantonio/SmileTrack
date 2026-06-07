using System;
using System.Data;
using System.Data.SqlClient;

namespace SmileTrack
{
    public static class DatabaseHelper
    {
        public static readonly string ConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False";

       
        public static void EnsureDatabaseAndTables()
        {
           
            var builder = new SqlConnectionStringBuilder(ConnectionString) { InitialCatalog = "master" };
            using (var con = new SqlConnection(builder.ToString()))
            using (var cmd = con.CreateCommand())
            {
                con.Open();
                cmd.CommandText = "IF DB_ID('SmileTrackDB') IS NULL CREATE DATABASE [SmileTrackDB];";
                cmd.ExecuteNonQuery();
            }

            
            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = con.CreateCommand())
            {
                con.Open();

                cmd.CommandText = @"
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
";
                cmd.ExecuteNonQuery();

               
                cmd.CommandText = @"
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
";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
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
";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
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
";
                cmd.ExecuteNonQuery();
            }
        }

        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            var dt = new DataTable();
            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        
        public static DataTable GetAppointmentsToday()
        {
            string sql = @"
SELECT AppointmentDateTime, FirstName + ' ' + LastName AS PatientName, Dentist, Treatment
FROM Appointments a
INNER JOIN Patients p ON a.PatientID = p.PatientID
WHERE Status = @status AND CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)
ORDER BY AppointmentDateTime";
            return ExecuteQuery(sql, new SqlParameter("@status", "Scheduled"));
        }

        public static DataTable GetWalkInsToday()
        {
            string sql = @"
SELECT FirstName + ' ' + LastName AS PatientName, AppointmentDateTime
FROM Appointments a
INNER JOIN Patients p ON a.PatientID = p.PatientID
WHERE VisitType = @visit AND CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)
ORDER BY AppointmentDateTime";
            return ExecuteQuery(sql, new SqlParameter("@visit", "Walk-in"));
        }

        public static DataTable GetFutureReminders()
        {
            string sql = @"
SELECT AppointmentDateTime, FirstName + ' ' + LastName AS PatientName
FROM Appointments a
INNER JOIN Patients p ON a.PatientID = p.PatientID
WHERE Status = @status AND AppointmentDateTime > GETDATE()
ORDER BY AppointmentDateTime";
            return ExecuteQuery(sql, new SqlParameter("@status", "Scheduled"));
        }

        public static int AddPatient(string fname, string lname, DateTime? bdate, int age, string gender, string contact, string email, string address)
        {
            const string sql = @"
INSERT INTO Patients (FirstName, LastName, BirthDate, Age, Gender, ContactNo, Email, Address)
VALUES (@fname,@lname,@bdate,@age,@gender,@contact,@email,@address);
SELECT SCOPE_IDENTITY();";
            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@fname", fname ?? string.Empty);
                cmd.Parameters.AddWithValue("@lname", lname ?? string.Empty);
                cmd.Parameters.AddWithValue("@bdate", (object)bdate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@gender", gender ?? string.Empty);
                cmd.Parameters.AddWithValue("@contact", contact ?? string.Empty);
                cmd.Parameters.AddWithValue("@email", email ?? string.Empty);
                cmd.Parameters.AddWithValue("@address", address ?? string.Empty);

                con.Open();
                var id = cmd.ExecuteScalar();
                return Convert.ToInt32(id);
            }
        }

        public static bool DeletePatient(int patientId)
        {
            if (patientId <= 0)
                return false;

            using (var con = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                using (var cmd = con.CreateCommand())
                {
                    cmd.Transaction = tran;
                    try
                    {
                        // Delete invoice items for invoices that belong to this patient
                        cmd.CommandText = @"
DELETE ii
FROM InvoiceItems ii
WHERE ii.InvoiceID IN (SELECT InvoiceID FROM Invoices WHERE PatientID = @pid);";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@pid", patientId);
                        cmd.ExecuteNonQuery();

                        // Delete invoices for this patient
                        cmd.CommandText = "DELETE FROM Invoices WHERE PatientID = @pid;";
                        cmd.ExecuteNonQuery();

                        // Delete appointments for this patient
                        cmd.CommandText = "DELETE FROM Appointments WHERE PatientID = @pid;";
                        cmd.ExecuteNonQuery();

                        // Finally delete the patient
                        cmd.CommandText = "DELETE FROM Patients WHERE PatientID = @pid;";
                        var rows = cmd.ExecuteNonQuery();

                        tran.Commit();
                        return rows > 0;
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        public static bool UpdatePatient(int patientId, string fname, string lname, DateTime? bdate, int age, string gender, string contact, string email, string address)
        {
            if (patientId <= 0) return false;

            const string sql = @"
        UPDATE Patients
        SET FirstName = @fname,
            LastName = @lname,
            BirthDate = @bdate,
            Age = @age,
            Gender = @gender,
            ContactNo = @contact,
            Email = @email,
            Address = @address
        WHERE PatientID = @id;";

            using (var con = new System.Data.SqlClient.SqlConnection(ConnectionString))
            using (var cmd = new System.Data.SqlClient.SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@fname", fname ?? string.Empty);
                cmd.Parameters.AddWithValue("@lname", lname ?? string.Empty);
                cmd.Parameters.AddWithValue("@bdate", (object)bdate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@gender", gender ?? string.Empty);
                cmd.Parameters.AddWithValue("@contact", contact ?? string.Empty);
                cmd.Parameters.AddWithValue("@email", email ?? string.Empty);
                cmd.Parameters.AddWithValue("@address", address ?? string.Empty);
                cmd.Parameters.AddWithValue("@id", patientId);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}