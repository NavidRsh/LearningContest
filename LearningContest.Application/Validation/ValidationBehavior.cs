using LearningContest.Application.Exceptions;
using LearningContest.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


//این یک ولیدیشن جنریک هست که تمام ولیدیتور های کامندها و کوئری ها باید از این ارث بری کنند
// ما این ولیدیشن را رجیستر میکنیم.
//پس تمامی ولیدیتور ها قابل شناسایی هستند و لازم نیست هر بار هر کدام را در استارتاپ رچیستر کنیم
namespace LearningContest.Application.Validation
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TResponse : class, new()
    {
        private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> logger;
        private readonly IValidationHandler<TRequest> validationHandler;

        // Have 2 constructors incase the validator does not exist
        public ValidationBehaviour(ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public ValidationBehaviour(ILogger<ValidationBehaviour<TRequest, TResponse>> logger, IValidationHandler<TRequest> validationHandler)
        {
            this.logger = logger;
            this.validationHandler = validationHandler;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType();
            if (validationHandler == null)
            {
                //اگر هیچ ولیدیشنی برای یک هندلر تعریف نکرده باشیم
                logger.LogInformation("{Request} does not have a validation handler configured.", requestName);
                return await next();
            }


            //متد ولیدیشن میتواند هم تولید اکسپشن کند و هم هندلر ما را با یک بولین از نتیجه کار با خبر کند
            var result = await validationHandler.Validate(request);
            if (result.IsSuccessful)
            {

            }
            else
            {
                //اگر خطا داشته باشیم و اکسپشن تولید نکنیم اینجا می آید
                logger.LogWarning("Validation failed for {Request}. Error: {Error}", requestName, result.Error);

                var resultR = new TResponse();

                dynamic r = resultR;
                r.Message = "";
                r.Success = false;
                r.ValidationErrors = new List<string> { result.Error };
                // new { Message = "", Success = false, ValidationErrors = new List<string> { result.Error } };
                return r as TResponse;
                //return new TResponse { Message = "", Success = false, ValidationErrors = new List<string> { result.Error } };
                //میتوانیم خطا تولید کنیم که از برنامه خارج و با استتوس 400 آن را نمایش دهد
                //اینکار را اینجا نمیکنیم. در هر ولیدیتور مربوط به کامند و کوئری میکنیم
                //throw new LearningContestException(string.Join(",",result.Error));
            }

            logger.LogInformation("Validation successful for {Request}.", requestName);
            return await next();
        }
    }
}
