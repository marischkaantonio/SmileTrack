using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmileTrack
{
    internal class Class1
    {
        // Static list of treatments
        public static List<string> GetTreatmentTypes()
        {
            return new List<string>
            {
                "Cleaning",
                "Extraction",
                "Root Canal Treatment",
                "Orthodontic Treatment",
                "Clear Aligners",
                "Complete Ortho Package",
                "Implant",
                "Bone Grafting",
                "Sinus Lifting"
            };
        }
    }
}

public class WalkInPatient
{
    public int No { get; set; }
    public string PatientName { get; set; }
    public DateTime TimeIn { get; set; }
}
public class User
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Status { get; set; }
}
public class AuditLog
{
    public DateTime Date { get; set; }
    public string User { get; set; }
    public string Action { get; set; }
    public string Details { get; set; }
}

public class Appointment
{
    public string Time { get; set; }
    public string PatientName { get; set; }
    public string Dentist { get; set; }
    public string Treatment { get; set; }
}




namespace SmileTrack
{
    public static class AppointmentGenerator
    {
        private static readonly Random random = new Random();

        // Sample patient names
        private static readonly List<string> patientNames = new List<string>
        {
            "Juan Dela Cruz",
            "Maria Santos",
            "Jose Ramirez",
            "Ana Mendoza",
            "Carlos Reyes",
            "Liza Fernandez",
            "Mark Villanueva",
            "Grace Lim",
            "Paolo Garcia",
            "Ella Cruz"
        };

        // Sample treatments
        private static readonly List<string> treatments = new List<string>
        {
            "Cleaning",
            "Filling",
            "Root Canal",
            "Extraction",
            "Orthodontic Treatment",
            "Teeth Whitening",
            "Crown",
            "Bridge",
            "Dentures",
            "Implant"
        };

        // Randomly pick a patient name
        public static string GetRandomPatientName()
        {
            int index = random.Next(patientNames.Count);
            return patientNames[index];
        }

        // Randomly pick a dentist (Rimrose or Marjie)
        public static string GetRandomDentist()
        {
            string[] dentists = { "Dr. Primrose", "Dr. Margie" };
            int index = random.Next(dentists.Length);
            return dentists[index];
        }

        // Randomly pick a treatment
        public static string GetRandomTreatment()
        {
            int index = random.Next(treatments.Count);
            return treatments[index];
        }
    }
}

