using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UP2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_UP2.Context;

namespace API_UP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Users")]
    public class UsersController : ControllerBase
    {
        private readonly StudentManagementContext _context;

        public UsersController(StudentManagementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"Пользователь с ID {id} не найден" });
            }
            return user;
        }

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="user">Данные пользователя</param>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="loginData">Логин и пароль</param>
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest loginData)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == loginData.Username && u.PasswordHash == loginData.Password);

            if (user == null)
                return Unauthorized(new { message = "Неверный логин или пароль" });

            return Ok(new
            {
                userId = user.Id,
                username = user.Username,
                role = user.RoleId
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}