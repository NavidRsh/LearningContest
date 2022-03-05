using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Infrastructure
{
    public interface ISendSMSService
    {
        Task<(bool result, long messageId, string message)> SendSMS(string mobilePhoneNumber, string apiKey, string message, string template); 
    }
}
