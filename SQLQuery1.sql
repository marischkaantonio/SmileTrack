USE SmileTrackDB;
SELECT 
    p.PatientID AS [Patient ID], 
    p.FirstName AS [First Name], 
    p.LastName AS [Last Name], 
    p.BirthDate AS [Birth Date], 
    p.Age AS [Age], 
    p.Gender AS [Gender],
    p.ContactNo AS [Contact No],
    p.Email AS [Email],
    p.Address AS [Address],
    a.AppointmentDateTime AS [Last Appointment],
    a.Treatment AS [Treatment],
    a.Dentist AS [Dentist],
    a.Status AS [Status]
FROM Patients p
INNER JOIN Appointments a ON p.PatientID = a.PatientID
