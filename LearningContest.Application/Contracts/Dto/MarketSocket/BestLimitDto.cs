using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Application.Contracts.Dto.MarketSocket
{
    public class BestLimitDto
    {
        public string Type { get; set; }
        public long BuyVolume1 { get; set; }
        public decimal BuyPrice1 { get; set; }
        public long BuyAmount1 { get; set; }
        public TimeSpan Time1 { get; set; }
        public long SellVolume1 { get; set; }
        public decimal SellPrice1 { get; set; }
        public long SellAmount1 { get; set; }
        public TimeSpan Time2 { get; set; }
    }
}
