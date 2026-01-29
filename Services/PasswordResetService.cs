using System;
using System.Linq;
namespace API_UP2.Services
{
    public class PasswordResetService
    {
        private readonly StudentManagementContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public PasswordResetService(
            StudentManagementContext context,
            IConfiguration configuration,
            EmailService emailService)
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
        public string GenerateResetToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
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
                    return PasswordResetResult.Success("Если пользователь с таким Email существует, код был отправлен");

                }
                var resetCode = GenerateCode();
                var resetToken = HashPass(resetCode);
                user.ResetPasswordToken = resetToken;

                await _context.SaveChangesAsync();
                var EmailSent = await _emailService.SendPasswordResetEmailAsync(email, resetCode);
                if (EmailSent)
                {
                    return PasswordResetResult.Success("Код восстановления отправлен на email");
                } else
                {
                    return PasswordResetResult.Failure("Ошибка отправки email");
                }
            }
            catch (Exception ex)
            {
                return PasswordResetResult.Failure($"Ошибка: {ex.Message}");
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
                    return PasswordResetResult.Failure("Пользователь не найден");
                }

                if (string.IsNullOrEmpty(user.ResetPasswordToken){
                    return PasswordResetResult.Failure("Код истёк или не действителен");
                }

                var hashedCode = HashPass(code);

                if (user.ResetPasswordToken != hashedCode)
                {
                    return PasswordResetResult.Failure("Неверный код восстановления");
                }

                var hashedPass = HashPass(newPassword);

                user.PasswordHash = hashedPass;
                user.ResetPasswordToken = null;
                await _context.SaveChangesAsync();
                return PasswordResetResult.Success("Пароль успешно изменён");
            }
            catch (Exception Ex)
            {
                return PasswordResetResult.Failure($"Ошибка: {ex.Message}");
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
                    return PasswordResetResult.Failure("Пользователь не найден");
                }

                if (string.IsNullOrEmpty(user.ResetPasswordToken){
                    return PasswordResetResult.Failure("Код истёк или не действителен");
                }

                var hashedCode = HashPass(code);

                if (user.ResetPasswordToken != hashedCode)
                {
                    return PasswordResetResult.Failure("Неверный код восстановления");
                }
                return PasswordResetResult.Success("Код верный");
            }
            catch (Exception Ex)
            {
                return PasswordResetResult.Failure($"Ошибка: {ex.Message}");
            }
        }
    }
    public class PasswordResetResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public static PasswordResetResult SuccessResult(string message)
        {
            return new PasswordResetResult { Success = true, Message = message };
        }
        public static PasswordResetResult FailureResult(string message)
        {
            return new PasswordResetResult { Success = false, Message = message };
        }
        public static PasswordResetResult Success(string message) = SuccessResult(message);
        public static PasswordResetResult Failure(string message) = FailureResult(message);
    }
}