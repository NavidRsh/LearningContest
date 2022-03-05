using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Application.Contracts.Dto.MarketSocket
{
    public class MarketWatchDto
    {
        public string MessageCode { get; set; }
        public string Id { get; set; }
        public string InsMaxLcode { get; set; }
        public string InstrumentName { get; set; }
        public double EndPrice { get; set; }
        public double MinLot { get; set; }
        public double MaxLog { get; set; }
        public string GroupCode { get; set; }
        public string GroupStatus { get; set; }
        public double LastPrice { get; set; }
        public double TotalNumberOfTradedShares { get; set; }
        public double DayMaxPrice { get; set; }
        public double DayMinPrice { get; set; }
        public double YesterdayPrice { get; set; }
        public string InstrumentState { get; set; }
        public string InstruemntAbbreviation { get; set; }
        public int BaseVolume { get; set; }
        public int MinQuantityTreshold { get; set; }
        public int MaxQuantityTreshold { get; set; }
        public long LastTradeTime { get; set; }
        public BestLimitDto BestLimit1 { get; set; }
        public BestLimitDto BestLimit2 { get; set; }
        public BestLimitDto BestLimit3 { get; set; }
        public string EnglishInstrumentName { get; set; }
        public string EnglishInstrumentCode { get; set; }
        public double OpenPrice { get; set; }
        public int RealBuyersAmount { get; set; }
        public int LegalBuyersAmount { get; set; }
        public int RealBuyingVolume { get; set; }
        public int LegalBuyingVolume { get; set; }
        public int RealSellerAmount { get; set; }
        public int LegalSellerAmount { get; set; }
        public int RealSellingVolume { get; set; }
        public int LegalSellingVolume { get; set; }
        public string CoreType { get; set; }
        public string ApplyTime { get; set; }
        public string ContractCoefficient { get; set; }
        public string IpoStartTime { get; set; }
        public string IpoEndTime { get; set; }
        public BestLimitDto BestLimit4 { get; set; }
        public BestLimitDto BestLimit5 { get; set; }
        public double TheoryOpeningPrice { get; set; }
        public long TsetmcId { get; set; }
        public int IndustryCode { get; set; }
        public int BaseOrderVolumeLog { get; set; }
        public string CompanyFarsiName { get; set; }
        public int CloseIndicator { get; set; }
        public double LastTradedPrice { get; set; }
        public int TotalNumberOfTrades { get; set; }
        public double TotalTradeValue { get; set; }
        public string CompanyStatusId { get; set; }
        public string CompanyStatusName { get; set; }
        public string Reasons { get; set; }
        public string LastStatusChange { get; set; }
        public int Market { get; set; }
        public string GroupName { get; set; }
        public int LastYearMinTradePrice { get; set; }
        public int LastYearMaxTradePrice { get; set; }
        public int LastWeekMinTradePrice { get; set; }
        public int LastWeekMaxTradePrice { get; set; }
        public double LastMonthTurnover { get; set; }
        public double LastThreeMonthesTurnover { get; set; }
        public int NumberOfShares { get; set; }
        public string MarketOpenPositions { get; set; }
        public string FirstTradeDate { get; set; }
        public string LastTradeDate { get; set; }
        public string PrioritySettlementDate { get; set; }
        public string CashSettlementDate { get; set; }
        public string BaseAssetCisinCode { get; set; }
        public string InitalMarginRate { get; set; }
        public double ReferencePrice { get; set; }
        public string UnKnown1 { get; set; }
        public string UnKnown2 { get; set; }
        public int UnKnown3 { get; set; }
    }

}
