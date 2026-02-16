using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_UP2.Models
{
    [Table("student")]
    public class SocialPayout
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

        [Column("FilePath")]
        public string FilePath { get; set; }
    }
}
