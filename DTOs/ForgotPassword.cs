using System;

namespace API_UP2.DTOs
{
	public class ForgotPassword
	{
		public string Email { get; set; }

	}
	public class ResetPassword
	{
		public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
