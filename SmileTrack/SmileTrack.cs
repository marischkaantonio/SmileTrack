using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmileTrack
{
    public static class AppointmentUpdater
    {
        // Update appointment status by patient name
        public static void UpdateAppointmentStatus(string patientName, string newStatus)
        {
            var appt = ReceptionistDashboard.AppointmentManager.Appointments
                .FirstOrDefault(a => a.PatientName == patientName && a.Date.Date == DateTime.Today);

            if (appt != null)
            {
                appt.Status = newStatus;
            }
        }

        // Refresh today's appointments in the DataGridView
        public static void RefreshTodaysAppointments(DataGridView dgv)
        {
            dgv.Rows.Clear();

            if (dgv.Columns.Count == 0)
            {
                dgv.Columns.Add("Time", "Time");
                dgv.Columns.Add("PatientName", "Patient Name");
                dgv.Columns.Add("Dentist", "Dentist");
                dgv.Columns.Add("Treatment", "Treatment");
                dgv.Columns.Add("Status", "Status");
            }

            var todaysAppointments = ReceptionistDashboard.AppointmentManager.Appointments
                .Where(a => a.Date.Date == DateTime.Today)
                .ToList();

            foreach (var appt in todaysAppointments)
            {
                dgv.Rows.Add(
                    appt.Date.ToString("hh:mm tt"),
                    appt.PatientName,
                    appt.Dentist,
                    appt.Treatment,
                    appt.Status
                );
            }
        }

        // Add a new appointment
        public static void AddAppointment(DateTime date, string patientName, string dentist, string treatment, string status)
        {
            var newAppt = new ReceptionistDashboard.Appointment
            {
                Date = date,
                PatientName = patientName,
                Dentist = dentist,
                Treatment = treatment,
                Status = status
            };

            ReceptionistDashboard.AppointmentManager.Appointments.Add(newAppt);
        }

        public static void AddAppointmentFromPatient(string fullName, string dentist, string treatment)
        {
            var newAppt = new ReceptionistDashboard.Appointment
            {
                Date = DateTime.Now, // automatically sets to current date/time
                PatientName = fullName,
                Dentist = dentist,
                Treatment = treatment,
                Status = "Scheduled"
            };

            ReceptionistDashboard.AppointmentManager.Appointments.Add(newAppt);
        }

    }
}
