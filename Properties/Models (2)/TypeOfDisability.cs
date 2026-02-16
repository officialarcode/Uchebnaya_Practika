using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_UP2.Models
{
    [Table("type_of_disability")]
    public class TypeOfDisability
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("Name_Type")]
        public string NameType { get; set; }
    }
}
