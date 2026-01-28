using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UP2.Context;

namespace API_UP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly StudentManagementContext _context;

        public DepartmentsController(StudentManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все отделения
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departments>>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        /// <summary>
        /// Получить студентов отделения
        /// </summary>
        /// <param name="id">ID отделения</param>
        [HttpGet("{id}/students")]
        public async Task<ActionResult<IEnumerable<Student>>> GetDepartmentStudents(int id)
        {
            var students = await _context.Students
                .Where(s => s.DepartmentId == id.ToString())
                .Include(s => s.Department)
                .ToListAsync();

            return students;
        }
    }
}