using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_UP2.Models
{
    public class RoomsHostel
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("Name_Room")]
        public string NameRoom { get; set; }

        [Column("Count_Room")]
        public int CountRoom { get; set; }
    }
}
