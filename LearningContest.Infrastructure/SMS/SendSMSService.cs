using LearningContest.Application.Contracts.Infrastructure;
using LearningContest.Application.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Infrastructure.SMS
{
    public class SendSMSService:ISendSMSService
    {
        private readonly ILogger _logger; 
        public SendSMSService(ILogger logger)
        {
            _logger = logger; 
        }
        public async Task<(bool result, long messageId, string message)> SendSMS(string mobilePhoneNumber, string apiKey, string message, string template)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
                //Kavenegar.Core.Models.SendResult result = await api.Send("10006703323323", "09125046704", "خدمات پیام کوتاه کاوه نگار");
                var result = await api.VerifyLookup(mobilePhoneNumber, message, template);

                return (result: true, messageId: result.Messageid, message: "");
            }
            catch (Kavenegar.Core.Exceptions.ApiException ex)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                _logger.Error("سرویس ارسال پیامک با خطا مواجه شده است - شماره تلفن : " + mobilePhoneNumber, ex);
                return (result: false, messageId: 0, message: "سرویس ارسال پیامک با خطا مواجه شده است");
            }
            catch (Kavenegar.Core.Exceptions.HttpException ex)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                _logger.Error("برقراری ارتباط با سرویس ارسال پیامک مقدور نیست - شماره تلفن : " + mobilePhoneNumber, ex);
                return (result: false, messageId: 0, message: "امکان برقراری ارتباط با سرویس ارسال پیامک فراهم نیست");
            }
            catch(Exception ex)
            {
                _logger.Error("خطا در ارسال پیامک - شماره تلفن : " + mobilePhoneNumber, ex);
                return (result: false, messageId: 0, message: "خطا در ارسال پیامک");
            }

        }
    }
}
