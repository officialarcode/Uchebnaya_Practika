using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("student")]
public class Student
{
    [Key]
    [Column("ID_Student")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("Lastname")]
    public string LastName { get; set; }

    [Column("Surname")]
    public string FirstName { get; set; }

    [Column("Date_Birth")]
    public DateTime DateBirth { get; set; }

    [Column("Gender")]
    public int Gender { get; set; }

    [Column("Phone")]
    public string Phone { get; set; }

    [Column("Education")]
    public string Education { get; set; }
    [Column("Departament")]
    public int DepartmentId { get; set; }
    [ForeignKey("DepartmentId")]
    public Departments Department { get; set; }

    [Column("Education_Group")]
    public string Education_Group { get; set; }

    [Column("Financy")]
    public string Financy { get; set; }

    [Column("Year_Entrance")]
    public int Year_Entrance { get; set; }

    [Column("Year_Ending")]
    public int Year_Ending { get; set; }

    [Column("Information_Deduction")]
    public string Information_Deduction { get; set; }

    [Column("Date_Deduction")]
    public DateTime Date_Deduction { get; set; }

    [Column("Note")]
    public string Note { get; set; }

    [Column("Information_About_Parents")]
    public string Information_About_Parents { get; set; }

    [Column("Penalties")]
    public string Penalties { get; set; }

    [Column("FilePath")]
    public string FilePath { get; set; }
}