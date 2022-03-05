using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Domain.Common
{
    public class ApiCallResponseError
    {
        public string title { get; set; }
        public string description { get; set; }
        public string descriptionEn { get; set; }
        public string errorType { get; set; }
        public int errorCode { get; set; }
        public object payloads { get; set; }
        public string UUID { get; set; }
    }
}
