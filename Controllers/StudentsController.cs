using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UP2.Context;
using API_UP2.Models;
using API_UP2.Services;
using API_UP2.DTOs;

namespace API_UP2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Students")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentManagementContext _context;
        private readonly StudentService _studentService;

        public StudentsController(StudentManagementContext context, StudentService studentService)
        {
            _context = context;
            _studentService = studentService;
        }

        /// <summary>
        /// Получить всех студентов
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.Students
                .Include(s => s.Department)
                .ToListAsync();
            return Ok(students);
        }

        /// <summary>
        /// Получить студента по ID
        /// </summary>
        /// <param name="id">ID студента</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _context.Students
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound(new { message = "Студент не найден" });

            return Ok(student);
        }

        /// <summary>
        /// Создать нового студента
        /// </summary>
        /// <param name="student">Данные студента</param>
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            if (student == null)
                return BadRequest(new { message = "Данные студента не могут быть пустыми" });

            // Проверяем, существует ли факультет
            if (student.DepartmentId > 0)
            {
                var department = await _context.Departments.FindAsync(student.DepartmentId);
                if (department == null)
                    return BadRequest(new { message = "Указанный факультет не существует" });
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

        /// <summary>
        /// Обновить данные студента
        /// </summary>
        /// <param name="id">ID студента</param>
        /// <param name="student">Обновленные данные</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
                return BadRequest(new { message = "ID в пути и в теле запроса не совпадают" });

            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
                return NotFound(new { message = "Студент не найден" });

            // Проверяем, существует ли факультет
            if (student.DepartmentId > 0)
            {
                var department = await _context.Departments.FindAsync(student.DepartmentId);
                if (department == null)
                    return BadRequest(new { message = "Указанный факультет не существует" });
            }

            // Обновляем свойства
            _context.Entry(existingStudent).CurrentValues.SetValues(student);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StudentExists(id))
                    return NotFound(new { message = "Студент не найден" });
                throw;
            }

            return Ok(student);
        }

        /// <summary>
        /// Удалить студента
        /// </summary>
        /// <param name="id">ID студента</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound(new { message = "Студент не найден" });

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Получить полную информацию о студенте (все статусы)
        /// </summary>
        /// <param name="id">ID студента</param>
        [HttpGet("{id}/full-info")]
        public async Task<IActionResult> GetStudentFullInfo(int id)
        {
            var fullInfo = await _studentService.GetStudentFullInfoAsync(id);

            if (fullInfo == null)
                return NotFound(new { message = "Студент не найден" });

            return Ok(fullInfo);
        }

        /// <summary>
        /// Получить статусы студента (сирота, инвалид и т.д.)
        /// </summary>
        /// <param name="id">ID студента</param>
        [HttpGet("{id}/statuses")]
        public async Task<IActionResult> GetStudentStatuses(int id)
        {
            // Проверяем существование студента
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound(new { message = "Студент не найден" });

            var today = DateTime.Today;

            var statuses = new
            {
                Orphan = await _context.Orphans
                    .AnyAsync(o => o.StudentId == id &&
                                 o.StartStatus <= today &&
                                 (o.EndStatus == null || o.EndStatus > today)),

                Disabled = await _context.StudentDisabledPeople
                    .AnyAsync(d => d.StudentId == id &&
                                 d.StartStatus <= today &&
                                 (d.EndStatus == null || d.EndStatus > today)),

                OVZ = await _context.StudentOVZ
                    .AnyAsync(o => o.StudentId == id &&
                                 o.StartStatus <= today &&
                                 (o.EndStatus == null || o.EndStatus > today)),

                SVO = await _context.StudentSVO
                    .AnyAsync(s => s.StudentId == id &&
                                 s.StartStatus <= today &&
                                 (s.EndStatus == null || s.EndStatus > today)),

                SOP = await _context.StudentSOP
                    .AnyAsync(s => s.StudentId == id &&
                                 s.DateDelivery <= today &&
                                 (s.DateDeRegistration == null || s.DateDeRegistration > today)),

                Social = await _context.SocialPayout
                    .AnyAsync(sp => sp.StudentId == id &&
                                  sp.StartStatus <= today &&
                                  (sp.EndStatus == null || sp.EndStatus > today)),

                Hostel = await _context.StudentHostel
                    .AnyAsync(h => h.StudentId == id &&
                                 h.CheckInDate <= today &&
                                 (h.EvictionDate == null || h.EvictionDate > today))
            };

            return Ok(statuses);
        }

        /// <summary>
        /// Получить статистику по студентам
        /// </summary>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var statistics = await _studentService.GetStatisticsAsync();
            return Ok(statistics);
        }

        /// <summary>
        /// Поиск студентов с фильтрами
        /// </summary>

        private async Task<bool> StudentExists(int id)
        {
            return await _context.Students.AnyAsync(e => e.Id == id);
        }
    }
}