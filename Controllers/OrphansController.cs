using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_UP2.Context;

namespace API_UP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Orphans")]
    public class OrphansController : ControllerBase
    {
        private readonly StudentManagementContext _context;

        public OrphansController(StudentManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить всех студентов-сирот с информацией о студенте
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOrphans()
        {
            var orphans = await _context.Orphans.ToListAsync();
            var result = new List<object>();

            foreach (var orphan in orphans)
            {
                var student = await _context.Students
                    .Include(s => s.Department)
                    .FirstOrDefaultAsync(s => s.Id == orphan.StudentId);

                result.Add(new
                {
                    orphan.Id,
                    orphan.StudentId,
                    orphan.StatusAssignmentOrder,
                    orphan.StartStatus,
                    orphan.EndStatus,
                    orphan.Note,
                    orphan.FilePath,
                    Student = student != null ? new
                    {
                        student.Id,
                        student.LastName,
                        student.FirstName,
                        student.Name,
                        student.DateBirth,
                        student.Education_Group,
                        Department = student.Department?.Name
                    } : null
                });
            }

            return Ok(result);
        }

        /// <summary>
        /// Добавить статус сироты студенту
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Orphans>> CreateOrphanStatus([FromBody] Orphans orphan)
        {
            // Проверка существования студента
            var studentExists = await _context.Students.AnyAsync(s => s.Id == orphan.StudentId);
            if (!studentExists)
            {
                return BadRequest(new { message = $"Студент с ID {orphan.StudentId} не найден" });
            }

            _context.Orphans.Add(orphan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrphanById), new { id = orphan.Id }, orphan);
        }

        /// <summary>
        /// Получить статус сироты по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetOrphanById(int id)
        {
            var orphan = await _context.Orphans.FindAsync(id);
            if (orphan == null)
            {
                return NotFound(new { message = $"Запись о сироте с ID {id} не найдена" });
            }

            var student = await _context.Students
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == orphan.StudentId);

            return Ok(new
            {
                orphan.Id,
                orphan.StudentId,
                orphan.StatusAssignmentOrder,
                orphan.StartStatus,
                orphan.EndStatus,
                orphan.Note,
                orphan.FilePath,
                Student = student != null ? new
                {
                    student.Id,
                    student.LastName,
                    student.FirstName,
                    student.Name,
                    student.DateBirth,
                    student.Education_Group,
                    Department = student.Department?.Name
                } : null
            });
        }

        /// <summary>
        /// Получить сирот по статусу (действующие/все)
        /// </summary>
        /// <param name="status">Действующие (active) или все (all)</param>
        [HttpGet("filter/{status}")]
        public async Task<ActionResult<IEnumerable<object>>> GetOrphansByStatus(string status)
        {
            var query = _context.Orphans.AsQueryable();

            if (status.ToLower() == "active")
            {
                query = query.Where(o => o.EndStatus == null || o.EndStatus > DateTime.Now);
            }

            var orphans = await query.ToListAsync();
            var result = new List<object>();

            foreach (var orphan in orphans)
            {
                var student = await _context.Students
                    .Include(s => s.Department)
                    .FirstOrDefaultAsync(s => s.Id == orphan.StudentId);

                result.Add(new
                {
                    orphan.Id,
                    orphan.StudentId,
                    orphan.StatusAssignmentOrder,
                    orphan.StartStatus,
                    orphan.EndStatus,
                    orphan.Note,
                    orphan.FilePath,
                    Student = student != null ? new
                    {
                        student.Id,
                        student.LastName,
                        student.FirstName,
                        student.Name,
                        student.DateBirth,
                        student.Education_Group,
                        Department = student.Department?.Name
                    } : null
                });
            }

            return Ok(result);
        }

        /// <summary>
        /// Получить статусы сирот для конкретного студента
        /// </summary>
        /// <param name="studentId">ID студента</param>
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<Orphans>>> GetOrphansByStudentId(int studentId)
        {
            var orphans = await _context.Orphans
                .Where(o => o.StudentId == studentId)
                .ToListAsync();

            if (!orphans.Any())
            {
                return NotFound(new { message = $"Записи о сиротах для студента ID {studentId} не найдены" });
            }

            return Ok(orphans);
        }

        /// <summary>
        /// Обновить статус сироты
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrphanStatus(int id, [FromBody] Orphans updatedOrphan)
        {
            if (id != updatedOrphan.Id)
            {
                return BadRequest(new { message = "ID в URL не совпадает с ID в теле запроса" });
            }

            var existingOrphan = await _context.Orphans.FindAsync(id);
            if (existingOrphan == null)
            {
                return NotFound(new { message = $"Запись о сироте с ID {id} не найдена" });
            }

            // Проверка существования студента
            var studentExists = await _context.Students.AnyAsync(s => s.Id == updatedOrphan.StudentId);
            if (!studentExists)
            {
                return BadRequest(new { message = $"Студент с ID {updatedOrphan.StudentId} не найден" });
            }

            // Обновление полей
            existingOrphan.StudentId = updatedOrphan.StudentId;
            existingOrphan.StatusAssignmentOrder = updatedOrphan.StatusAssignmentOrder;
            existingOrphan.StartStatus = updatedOrphan.StartStatus;
            existingOrphan.EndStatus = updatedOrphan.EndStatus;
            existingOrphan.Note = updatedOrphan.Note;
            existingOrphan.FilePath = updatedOrphan.FilePath;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrphanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Удалить статус сироты
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrphanStatus(int id)
        {
            var orphan = await _context.Orphans.FindAsync(id);
            if (orphan == null)
            {
                return NotFound(new { message = $"Запись о сироте с ID {id} не найдена" });
            }

            _context.Orphans.Remove(orphan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrphanExists(int id)
        {
            return _context.Orphans.Any(e => e.Id == id);
        }
    }
}