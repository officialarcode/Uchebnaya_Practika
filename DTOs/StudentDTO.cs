using System;

namespace API_UP2.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Education { get; set; }
        public string DepartmentName { get; set; }
        public string GroupName { get; set; }
        public string Funding { get; set; }
        public int AdmissionYear { get; set; }
        public bool IsExpelled { get; set; }
    }

    public class CreateStudentDTO
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Education { get; set; }
        public string DepartmentId { get; set; }
        public string GroupName { get; set; }
        public string Funding { get; set; }
        public int AdmissionYear { get; set; }
    }

    public class FilterDTO
    {
        public string? Search { get; set; }
        public string? DepartmentId { get; set; }
        public string? GroupName { get; set; }
        public string? Funding { get; set; }
        public bool? IsExpelled { get; set; }
        public string? StatusType { get; set; }
        public bool? ActiveOnly { get; set; }
        public DateTime? FilterDate { get; set; }
    }
}