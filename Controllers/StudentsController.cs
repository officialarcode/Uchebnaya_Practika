using Microsoft.AspNetCore.Mvc;
namespace API_UP2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Students")]
    public class StudentsController : ControllerBase
    {
        /// <summary>
        /// Получить всех студентов
        /// </summary>
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            return Ok(new List<object>());
        }
        /// <summary>
        /// Получить студента по ID
        /// </summary>
        /// <param name="id">ID студента</param>
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            return Ok(new { Id = id, Name = "Иван Иванов" });
        }

        /// <summary>
        /// Создать нового студента
        /// </summary>
        /// <param name="student">Данные студента</param>
        [HttpPost]
        public IActionResult CreateStudent([FromBody] object student)
        {
            return CreatedAtAction(nameof(GetStudentById), new { id = 1 }, student);
        }

        /// <summary>
        /// Обновить данные студента
        /// </summary>
        /// <param name="id">ID студента</param>
        /// <param name="student">Обновленные данные</param>
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] object student)
        {
            return Ok(student);
        }

        /// <summary>
        /// Удалить студента
        /// </summary>
        /// <param name="id">ID студента</param>
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return NoContent();
        }

        /// <summary>
        /// Получить статусы студента (сирота, инвалид и т.д.)
        /// </summary>
        /// <param name="studentId">ID студента</param>
        [HttpGet("{studentId}/statuses")]
        public IActionResult GetStudentStatuses(int studentId)
        {
            return Ok(new
            {
                Orphan = true,
                Disabled = false,
                Dormitory = true
            });
        }
    }
}