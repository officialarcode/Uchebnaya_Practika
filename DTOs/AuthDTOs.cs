using System.ComponentModel.DataAnnotations;

namespace API_UP2.DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        public string Lastname { get; set; }

        public string Surname { get; set; }

        [Required(ErrorMessage = "Логин обязателен")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        public string Password { get; set; }

        public int? RoleId { get; set; }
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Логин обязателен")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }

    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}