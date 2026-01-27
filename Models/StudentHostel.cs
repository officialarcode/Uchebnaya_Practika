namespace API_UP2.Models
{
    public class StudentHostel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int Room { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime EvictionDate { get; set; }
        public string Note { get; set; }
        public string FilePath { get; set; }
    }
}
