using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_UP2.Models
{
    [Table("audit_log")]
    public class AuditLog
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("Table_Name")]
        public string TableName { get; set; }
        [Column("Record_ID")]
        public int RecordId { get; set; }
        [Column("Action")]
        public string Action { get; set; }
        [Column("Old_Data")]
        public string OldDataJson { get; set; }
        [Column("New_Data")]
        public string NewDataJson { get; set; }

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
        [Column("Changed_by")]
        public int ChangedBy { get; set; }
        [Column("Changed_at")]
        public DateTime ChangedAt { get; set; }
    }
}