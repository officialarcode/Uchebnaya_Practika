using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("departament")]

public class Departments
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Name_Departament")]
    public string Name { get; set; }
}