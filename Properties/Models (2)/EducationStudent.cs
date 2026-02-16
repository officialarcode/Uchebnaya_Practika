using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_UP2.Models
{
    [Table("education_student")]
    public class EducationStudent
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("Name_Education_Student")]
        public string NameEducationStudent { get; set; }
    }
}
