using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("student_disabled_people")]
public class StudentDisabledPeople
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Student_ID")]
    public int StudentId { get; set; }

    [Column("Student_ID")]
    public Student Student { get; set; }

    [Column("Status_Assignment_Order")]
    public string StatusAssignmentOrder { get; set; }

    [Column("Start_Status")]
    public DateTime StartStatus { get; set; }

    [Column("End_Status")]
    public DateTime EndStatus { get; set; }

    [Column("Note")]
    public string Note { get; set; }

    [Column("Type_of_Disability")]
    public int TypeOfDisability { get; set; }

    [Column("FilePath")]
    public string FilePath { get; set; }
}