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




