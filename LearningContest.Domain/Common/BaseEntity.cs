using System;

namespace LearningContest.Domain.Common
{
    public class BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

    public enum ConnectionType
    {
        Alborz = 1,
        LearningContest = 2
    }
}
