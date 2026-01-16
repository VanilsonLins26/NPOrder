using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityServerAspNetIdentity.Services 
{
  
    public class NoOpEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
       
            return Task.CompletedTask;
        }
    }
}