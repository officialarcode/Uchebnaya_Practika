using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_UP2.Models
{
    [Table("student_hostel")]
    public class StudentHostel
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("Student_ID")]
        public int StudentId { get; set; }

        [Column("Student_ID")]
        public Student Student { get; set; }

        [Column("Room")]
        public int Room { get; set; }

        [Column("Check_In_Date")]
        public DateTime CheckInDate { get; set; }

        [Column("Eviction_Date")]
        public DateTime EvictionDate { get; set; }

        [Column("Note")]
        public string Note { get; set; }

        [Column("FilePath")]
        public string FilePath { get; set; }
    }
}
