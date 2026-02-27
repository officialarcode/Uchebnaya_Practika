using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace API_UP2.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("ID_User")]
        public int Id { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [Required]
        [Column("Lastname")]
        public string Lastname { get; set; }

        [Column("Surname")]
        public string Surname { get; set; }

        [Required]
        [Column("Username")]
        public string Username { get; set; }

        [Required]
        [Column("Password_Hash")]
        public string PasswordHash { get; set; }

        [Column("Role_ID")]
        public int RoleId { get; set; }
    }
}