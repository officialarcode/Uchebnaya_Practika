using System.Security.Cryptography;
using System.Text;
using API_UP2.Context;
using Microsoft.EntityFrameworkCore;

namespace API_UP2.Services
{
    public class PasswordResetService
    {
        private readonly StudentManagementContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly TimeSpan _tokenExpiry = TimeSpan.FromMinutes(15);

        public PasswordResetService(
            StudentManagementContext context,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public string GenerateCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public string HashPass(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Хешируем код вместе со временем истечения
        private string HashTokenWithExpiry(string code, DateTime expiryTime)
        {
            var tokenWithExpiry = $"{code}:{expiryTime.Ticks}";
            return HashPass(tokenWithExpiry);
        }

        // Извлекаем время истечения из токена
        private (string code, DateTime expiryTime)? DecodeToken(string tokenHash, string providedCode)
        {
            try
            {
                // Пробуем разные форматы токена
                var expiryTimes = new[]
                {
                    DateTime.UtcNow.AddMinutes(15),
                    DateTime.UtcNow.AddMinutes(30),
                    DateTime.UtcNow.AddMinutes(60)
                };

                foreach (var expiryTime in expiryTimes)
                {
                    var testToken = HashTokenWithExpiry(providedCode, expiryTime);
                    if (testToken == tokenHash)
                    {
                        return (providedCode, expiryTime);
                    }
                }

                // Если не нашли - проверяем старый формат (без времени)
                if (HashPass(providedCode) == tokenHash)
                {
                    return (providedCode, DateTime.UtcNow); // Истек немедленно
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<PasswordResetResult> SendResetCodeAsync(string email)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return new PasswordResetResult(true, "Если пользователь с таким Email существует, код был отправлен");
                }

                var resetCode = GenerateCode();
                var expiryTime = DateTime.UtcNow.Add(_tokenExpiry);
                var resetToken = HashTokenWithExpiry(resetCode, expiryTime);

                user.ResetPasswordToken = resetToken;
                await _context.SaveChangesAsync();

                var emailSent = await _emailService.SendPasswordResetEmailAsync(email, resetCode);

                if (emailSent)
                {
                    return new PasswordResetResult(true, "Код восстановления отправлен на email");
                }
                else
                {
                    return new PasswordResetResult(false, "Ошибка отправки email");
                }
            }
            catch (Exception ex)
            {
                return new PasswordResetResult(false, $"Ошибка: {ex.Message}");
            }
        }

        public async Task<PasswordResetResult> ResetPasswordAsync(
            string email,
            string code,
            string newPassword)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return new PasswordResetResult(false, "Пользователь не найден");
                }

                if (string.IsNullOrEmpty(user.ResetPasswordToken))
                {
                    return new PasswordResetResult(false, "Код истёк или не действителен");
                }

                // Декодируем токен
                var decoded = DecodeToken(user.ResetPasswordToken, code);

                if (decoded == null)
                {
                    return new PasswordResetResult(false, "Неверный код восстановления");
                }

                var (_, expiryTime) = decoded.Value;

                // Проверяем срок действия
                if (expiryTime < DateTime.UtcNow)
                {
                    return new PasswordResetResult(false, "Код истёк");
                }

                // Проверка, что новый пароль отличается от старого
                var hashedPass = HashPass(newPassword);
                if (user.PasswordHash == hashedPass)
                {
                    return new PasswordResetResult(false, "Новый пароль должен отличаться от старого");
                }

                user.PasswordHash = hashedPass;
                user.ResetPasswordToken = null;
                await _context.SaveChangesAsync();

                return new PasswordResetResult(true, "Пароль успешно изменён");
            }
            catch (Exception ex)
            {
                return new PasswordResetResult(false, $"Ошибка: {ex.Message}");
            }
        }

        public async Task<PasswordResetResult> ValidateResetCodeAsync(string email, string code)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return new PasswordResetResult(false, "Пользователь не найден");
                }

                if (string.IsNullOrEmpty(user.ResetPasswordToken))
                {
                    return new PasswordResetResult(false, "Код истёк или не действителен");
                }

                var decoded = DecodeToken(user.ResetPasswordToken, code);

                if (decoded == null)
                {
                    return new PasswordResetResult(false, "Неверный код восстановления");
                }

                var (_, expiryTime) = decoded.Value;

                if (expiryTime < DateTime.UtcNow)
                {
                    return new PasswordResetResult(false, "Код истёк");
                }

                return new PasswordResetResult(true, "Код верный");
            }
            catch (Exception ex)
            {
                return new PasswordResetResult(false, $"Ошибка: {ex.Message}");
            }
        }
    }

    public class PasswordResetResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? Token { get; set; }

        public PasswordResetResult(bool success, string message, string? token = null)
        {
            Success = success;
            Message = message;
            Token = token;
        }
    }
}