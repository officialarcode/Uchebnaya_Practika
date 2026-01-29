using System;
namespace API_UP2.Services 

{
	public class EmailService
	{
		private readonly IConfiguration _configuration;
		public EmailService(IConfiguration configuration)
		{
            _configuration = configuration;	
        }
		public async Task<bool> SendPasswordResetEmailAsync(string email, string resetCode)
		{
			
		}
}
