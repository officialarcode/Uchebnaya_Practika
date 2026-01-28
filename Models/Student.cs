public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime DateBirth { get; set; }
    public int Gender { get; set; } 
    public string Phone { get; set; }
    public string Education { get; set; }
    public string DepartmentId { get; set; }
    public Departments Department { get; set; }
    public string Education_Group { get; set; }
    public string Financy { get; set; }
    public int Year_Entrance { get; set; }
    public int Year_Ending { get; set; }
    public string Information_Deduction { get; set; }
    public DateTime Date_Deduction { get; set; }
    public string Note { get; set; }
    public string Information_About_Parents { get; set; }
    public string Penalties { get; set; }
    public string FilePath { get; set; }
}