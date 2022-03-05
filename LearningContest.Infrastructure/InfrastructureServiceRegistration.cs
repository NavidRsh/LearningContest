using LearningContest.Application.Contracts.Infrastructure;
using LearningContest.Application.Models.Mail;
using LearningContest.Infrastructure.Auth;
using LearningContest.Infrastructure.Mail;
using LearningContest.Infrastructure.SMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LearningContest.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {            
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddTransient<IEmailService, EmailService>();

            services.AddScoped<ISendSMSService, SendSMSService>();

            //Jwt
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
            services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();            

            return services;
        }

        
    }
}
