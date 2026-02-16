using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_UP2.Models
{
    [Table("student_SOP")]
    public class StudentSOP
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("Student_ID")]
        public int StudentId { get; set; }

        [Column("Student_ID")]
        public Student Student { get; set; }

        [Column("Type")]
        public int Type { get; set; }

        [Column("Date_Delivery")]
        public DateTime DateDelivery { get; set; }

        [Column("Date_De_Registration")]
        public DateTime DateDeRegistration { get; set; }

        [Column("Basis_Delivery")]
        public string BasisDelivery { get; set; }

        [Column("Basis_De_Registration")]
        public string BasisDeRegistration { get; set; }

        [Column("Reason_Delivery")]
        public string ReasonDelivery { get; set; }

        [Column("Reason_De_Registration")]
        public string ReasonDeRegistration { get; set; }

        [Column("Note")]
        public string Note { get; set; }

        [Column("FilePath")]
        public string FilePath { get; set; }
    }
}
