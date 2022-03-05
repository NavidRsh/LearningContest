using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Application.Exceptions
{
    public class LoginHandledException: ApplicationException
    {
        public LoginHandledException(string text)
           : base(text)
        {
        }
    }
}
