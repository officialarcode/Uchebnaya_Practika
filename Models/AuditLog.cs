using Newtonsoft.Json.Linq;

namespace API_UP2.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public string Action { get; set; }
        public JObject OldData { get; set; }
        public JObject NewData { get; set; }
        public int ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}