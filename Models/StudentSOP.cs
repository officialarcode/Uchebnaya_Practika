namespace API_UP2.Models
{
    public class StudentSOP
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int Type { get; set; }
        public DateTime DateDelivery { get; set; }
        public DateTime DateDeRegistration { get; set; }
        public string BasisDelivery { get; set; }
        public string BasisDeRegistration { get; set; }
        public string ReasonDelivery { get; set; }
        public string ReasonDeRegistration { get; set; }
        public string Note { get; set; }
        public string FilePath { get; set; }
    }
}
