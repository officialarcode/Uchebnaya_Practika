using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_UP2.Models
{
    [Table("student_SPPP")]
    public class StudentSPPP
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("Student_ID")]
        public int StudentId { get; set; }

        [Column("Student_ID")]
        public Student Student { get; set; }

        [Column("Date_Sppp")]
        public DateTime DateSppp { get; set; }

        [Column("Basis_Challenge")]
        public string BasisChallenge { get; set; }

        [Column("Staff_Present")]
        public string StaffPresent { get; set; }

        [Column("Manager_Present")]
        public string ManagerPresent { get; set; }

        [Column("Reason_Calling")]
        public string ReasonCalling { get; set; }

        [Column("Decision")]
        public string Decision { get; set; }

        [Column("Note")]
        public string Note { get; set; }

        [Column("FilePath")]
        public string FilePath { get; set; }

    }
}
