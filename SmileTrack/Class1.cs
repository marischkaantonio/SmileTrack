using System;
using System.Collections.Generic;
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