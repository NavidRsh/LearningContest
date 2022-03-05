using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Validation
{
    public interface IValidationHandler
    {
    }

    //هر هندلر مربوط به کامند یا کوئری بخواهد ولیدیشن داشته باشد باید
    //کلاسی برای آن بنویسیم که این اینترفیس را پیاده سازی کند.
    public interface IValidationHandler<T> : IValidationHandler
    {
        Task<ValidationResult> Validate(T Request, bool generateException = true);
    }


    public class ValidationResult
    {
        public bool IsSuccessful { get; set; } = true;
        public string Error { get; set; }
        public static ValidationResult Success => new ValidationResult();
        public static ValidationResult Fail(string error) => new ValidationResult { Error = error, IsSuccessful = false };

    }

}
