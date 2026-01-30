using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_UP2.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public string Action { get; set; }

        // Для хранения в БД
        public string OldDataJson { get; set; }
        public string NewDataJson { get; set; }

        // Игнорируемые свойства для работы в коде
        [NotMapped]
        public JObject OldData
        {
            get => string.IsNullOrEmpty(OldDataJson) ? null : JObject.Parse(OldDataJson);
            set => OldDataJson = value?.ToString();
        }

        [NotMapped]
        public JObject NewData
        {
            get => string.IsNullOrEmpty(NewDataJson) ? null : JObject.Parse(NewDataJson);
            set => NewDataJson = value?.ToString();
        }

        public int ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}