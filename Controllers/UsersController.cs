using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UP2.Models;
using API_UP2.DTOs;
using API_UP2.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

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
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Lastname = u.Lastname,
                Surname = u.Surname,
                Username = u.Username,
                RoleId = u.RoleId
            }).ToList();
        }

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"Пользователь с ID {id} не найден" });
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                Surname = user.Surname,
                Username = user.Username,
                RoleId = u.RoleId
            };
        }

        /// <summary>
        /// Зарегистрировать нового пользователя
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] RegisterUserDto registerDto)
        {
            if (registerDto == null)
                return BadRequest(new { message = "Данные пользователя не могут быть пустыми" });

            // Проверяем уникальность имени пользователя
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerDto.Username);

            if (existingUser != null)
                return Conflict(new { message = "Пользователь с таким именем уже существует" });

            // Проверяем, существует ли роль
            try
            {
                var roleExists = await _context.Database
                    .ExecuteSqlRawAsync("SELECT COUNT(*) FROM role WHERE ID_Role = {0}", registerDto.RoleId) > 0;

                if (!roleExists)
                {
                    return BadRequest(new { message = $"Роль с ID {registerDto.RoleId} не существует" });
                }
            }
            catch
            {
                // Если таблица role не существует, пропускаем проверку
            }

            // Хешируем пароль
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Создаем пользователя
            var user = new User
            {
                Name = registerDto.Name,
                Lastname = registerDto.Lastname,
                Surname = registerDto.Surname,
                Username = registerDto.Username,
                PasswordHash = passwordHash,
                RoleId = registerDto.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                Surname = user.Surname,
                Username = user.Username,
                RoleId = user.RoleId
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest(new { message = "Данные для входа не могут быть пустыми" });

            // Находим пользователя по имени
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null)
                return Unauthorized(new { message = "Неверный логин или пароль" });

            // Проверяем пароль с использованием BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (!isPasswordValid)
                return Unauthorized(new { message = "Неверный логин или пароль" });

            // Успешная аутентификация
            return Ok(new
            {
                userId = user.Id,
                username = user.Username,
                name = user.Name,
                lastname = user.Lastname,
                surname = user.Surname,
                role = user.RoleId,
                message = "Успешный вход"
            });
        }

        /// <summary>
        /// Создать пользователя (старый метод для совместимости)
        /// </summary>
        [HttpPost]
        [Obsolete("Используйте метод /api/users/register вместо этого")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { message = "Данные пользователя не могут быть пустыми" });

            // Проверяем, есть ли пароль для хеширования
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest(new { message = "Пароль не может быть пустым" });
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            if (existingUser != null)
                return Conflict(new { message = "Пользователь с таким именем уже существует" });

            // Проверяем, существует ли роль
            try
            {
                var roleExists = await _context.Database
                    .ExecuteSqlRawAsync("SELECT COUNT(*) FROM role WHERE ID_Role = {0}", user.RoleId) > 0;

                if (!roleExists)
                {
                    return BadRequest(new { message = $"Роль с ID {user.RoleId} не существует" });
                }
            }
            catch
            {
                // Если таблица role не существует, пропускаем проверку
            }

            // Хешируем пароль перед сохранением
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
    }
}