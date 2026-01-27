namespace API_UP2.Models
{
    public class Orphans
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StatusAssignmentOrder { get; set; }
        public DateTime StartStatus { get; set; }
        public DateTime EndStatus { get; set; }
        public string Note { get; set; }
        public string FilePath { get; set; }
    }
}
