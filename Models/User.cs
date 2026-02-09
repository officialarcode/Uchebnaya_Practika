using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class User
{
    [Key]
    [Column("ID_User")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("Lastname")]
    public string Lastname { get; set; }

    [Column("Surname")]
    public string Surname { get; set; }

    [Required]
    [Column("Username")]
    public string Username { get; set; }

    [Column("Password_Hash")]
    public string PasswordHash { get; set; }
}