namespace API_UP2.Models
{
    public class StudentSPPP
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public DateTime DateSppp { get; set; }
        public string BasisChallenge { get; set; }
        public string StaffPresent { get; set; }
        public string ManagerPresent { get; set; }
        public string ReasonCalling { get; set; }
        public string Decision { get; set; }
        public string Note { get; set; }
        public string FilePath { get; set; }
    }
}
