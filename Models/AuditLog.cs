namespace API_UP2.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public string Action { get; set; }
        public DateTime Date_Birth { get; set; }
        public int Gender { get; set; }
    }
}
