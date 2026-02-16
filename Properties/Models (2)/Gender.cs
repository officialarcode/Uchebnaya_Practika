using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_UP2.Models
{
    [Table("gender")]
    public class Gender
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("Name_Gender")]
        public string NameGender { get; set; }
    }
}
