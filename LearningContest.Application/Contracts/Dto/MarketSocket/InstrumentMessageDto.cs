using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Application.Contracts.Dto.MarketSocket
{
    public class InstrumentMessageDto
    {
        public string InsMaxLCode { get; set; }
        public string MessagePrefix { get; set; }
        public string MessageText { get; set; }
    }
}
