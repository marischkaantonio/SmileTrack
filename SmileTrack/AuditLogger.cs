using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmileTrack
{

    public static class AuditLogger
    {
        // ✅ Declare filePath once at the top of the class
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "auditlog.json");

        public static void SaveAuditLog(string user, string action, string details)
        {
            List<AuditLog> logs = new List<AuditLog>();
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                logs = JsonConvert.DeserializeObject<List<AuditLog>>(json) ?? new List<AuditLog>();
            }

            logs.Add(new AuditLog
            {
                Date = DateTime.Now,
                User = user,
                Action = action,
                Details = details
            });

            string newJson = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(filePath, newJson);
        }

        public static List<AuditLog> LoadAuditLogs()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<AuditLog>>(json) ?? new List<AuditLog>();
            }
            return new List<AuditLog>();
        }

        public static void ClearAuditLogs()
        {
            File.WriteAllText(filePath, "[]");
        }
    }
}
