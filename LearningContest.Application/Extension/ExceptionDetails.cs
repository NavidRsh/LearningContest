using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearningContest.Application.Extension
{
    public static class DetailException
    {
        public static string GetExceptionDetails(this Exception exception)
        {
            var properties = exception.GetType()
                .GetProperties();
            var fields = properties
                .Select(property => new
                {
                    Name = property.Name,
                    Value = property.GetValue(exception, null)
                })
                .Select(x => String.Format(
                    "{0} = {1}",
                    x.Name,
                    x.Value != null ? x.Value.ToString() : String.Empty
                ));

            return String.Join("\n", fields);
        }
    }
}
