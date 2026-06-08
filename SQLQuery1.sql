SELECT AppointmentID,
       PatientID,
       VisitType,
       Status,
       AppointmentDateTime
FROM Appointments
ORDER BY AppointmentDateTime DESC;

SELECT DISTINCT VisitType
FROM Appointments;
