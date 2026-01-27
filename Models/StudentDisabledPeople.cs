public class StudentDisabledPeople
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public string StatusAssignmentOrder { get; set; }
    public DateTime StartStatus { get; set; }
    public DateTime EndStatus { get; set; }
    public string Note { get; set; }
    public int TypeOfDisability { get; set; }
    public string FilePath { get; set; }
}