using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Domain
{
    public class PagerResponse<T>
    {
        public List<T> List { get; set; }
        public int Count { get; set; }
    }

}
