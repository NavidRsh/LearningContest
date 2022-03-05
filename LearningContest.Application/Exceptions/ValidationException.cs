using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace LearningContest.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public List<string> ValdationErrors { get; set; }

        public ValidationException(ValidationResult validationResult)
        {
            ValdationErrors = new List<string>();

            foreach (var validationError in validationResult.Errors)
            {
                ValdationErrors.Add(validationError.ErrorMessage);
            }
        }
    }

    public class LearningContestException : System.Exception
    {
        string _message;
        public readonly string Code;
        public readonly string Stack;

        public LearningContestException(string message,string code="",string stack="")
        {
            HResult = 1;
            _message = message;
            Code = code;
            Stack = stack;
        }

        public override string Message => _message;

    }
}
