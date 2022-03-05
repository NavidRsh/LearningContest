using LearningContest.Application.Models.Mail;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
