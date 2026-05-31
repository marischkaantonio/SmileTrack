CREATE DATABASE SmileTrackDB;

USE SmileTrackDB;

CREATE TABLE Patients (
    PatientID INT PRIMARY KEY IDENTITY(1,1),
    Firstname VARCHAR(50),
    Lastname VARCHAR(50),
    Birthdate DATE,
    Age INT,
    Gender VARCHAR(10),
    ContactNo VARCHAR(20),
    Email VARCHAR(100),
    Address VARCHAR(255)
);

CREATE TABLE Appointments (
    AppointmentID INT PRIMARY KEY IDENTITY(1,1),
    PatientID INT FOREIGN KEY REFERENCES Patients(PatientID),
    Dentist VARCHAR(100),
    TreatmentType VARCHAR(100),
    VisitType VARCHAR(20), -- Walk-in or Appointment
    Status VARCHAR(20),    -- Scheduled, Completed, Cancelled
    DateTime DATETIME,
    Notes VARCHAR(255)
);
