using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_UP2.Models
{

    [Table("financy")]
    public class Financy
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("Name_Financy")]
        public string NameFinancy { get; set; }
    }
}
