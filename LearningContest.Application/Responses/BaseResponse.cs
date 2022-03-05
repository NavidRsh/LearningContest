using System;
using System.Collections.Generic;

namespace LearningContest.Application.Responses
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
            Success = true;
        }
        public BaseResponse(string message = null)
        {
            Success = true;
            Message = message;
        }

        public BaseResponse(string message, bool success)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> ValidationErrors { get; set; }
        public T Result { get; set; }

       
    }
}
