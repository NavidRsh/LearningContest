using LearningContest.Application.Contracts.Dto.MarketSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Services
{
    public interface IMarketConversionService
    {
        List<MarketWatchDto> ConvertMarketWatchResponse(string marketWathcMessage);
        List<MarketWatchDto> ConvertBestLimitResponse(string bestLimitMessage);
        List<InstrumentMessageDto> ConvertToInstrumentMessages(string marketMessage); 
    }
    public class MarketConversionService : IMarketConversionService
    {
        /// <summary>
        /// تبدیل به دی تی او
        /// </summary>
        /// <param name="marketWathcMessage"></param>
        /// <returns></returns>
        public List<MarketWatchDto> ConvertMarketWatchResponse(string marketWathcMessage)
        {
            List<MarketWatchDto> result = new List<MarketWatchDto>();
            var marketWatchLine = marketWathcMessage.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var priceLine in marketWatchLine)
            {
                var priceItems = priceLine.Split(',');
                var bestLimit1 = priceItems[20].Split(';');
                var bestLimit2 = priceItems[21].Split(';');
                var bestLimit3 = priceItems[22].Split(';');

                result.Add(new MarketWatchDto()
                {
                    MessageCode = priceItems[0],
                    Id = priceItems[1],
                    InsMaxLcode = priceItems[2],
                    InstrumentName = priceItems[3],
                    EndPrice = CommonInfrastructure.Helper.Conversions.ToDouble(priceItems[4]),
                    MinLot = CommonInfrastructure.Helper.Conversions.ToDouble(priceItems[5]),
                    MaxLog = CommonInfrastructure.Helper.Conversions.ToDouble(priceItems[6]),
                    GroupCode = priceItems[7],
                    GroupStatus = priceItems[8],
                    LastPrice = CommonInfrastructure.Helper.Conversions.ToDouble(priceItems[9]),
                    TotalNumberOfTradedShares = CommonInfrastructure.Helper.Conversions.ToDouble(priceItems[10]),
                    DayMaxPrice = CommonInfrastructure.Helper.Conversions.ToDouble(priceItems[11]),
                    DayMinPrice = CommonInfrastructure.Helper.Conversions.ToDouble(priceItems[12]),
                    YesterdayPrice = CommonInfrastructure.Helper.Conversions.ToDouble(priceItems[13]),
                    InstrumentState = priceItems[14],
                    InstruemntAbbreviation = priceItems[15],
                    BaseVolume = CommonInfrastructure.Helper.Conversions.ToInt(priceItems[16]),
                    MinQuantityTreshold = CommonInfrastructure.Helper.Conversions.ToInt(priceItems[17]),
                    MaxQuantityTreshold = CommonInfrastructure.Helper.Conversions.ToInt(priceItems[18]),
                    LastTradeTime = CommonInfrastructure.Helper.Conversions.ToLong(priceItems[19]),
                    BestLimit1 = (bestLimit1 != null && bestLimit1.Length >= 8) ? new BestLimitDto()
                    {
                        Type = "bl1",
                        BuyVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit1[1]),
                        BuyPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit1[2]),
                        BuyAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit1[3]),
                        //Time1 =  
                        SellVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit1[5]),
                        SellPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit1[6]),
                        SellAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit1[7])
                        //Time2 
                    } : null,
                    BestLimit2 = (bestLimit2 != null && bestLimit2.Length >= 8) ? new BestLimitDto()
                    {
                        Type = "bl2",
                        BuyVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit2[1]),
                        BuyPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit2[2]),
                        BuyAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit2[3]),
                        //Time1 =  
                        SellVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit2[5]),
                        SellPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit2[6]),
                        SellAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit2[7])
                        //Time2
                    } : null,
                    BestLimit3 = (bestLimit3 != null && bestLimit3.Length >= 8) ? new BestLimitDto()
                    {
                        Type = "bl3",
                        BuyVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit3[1]),
                        BuyPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit3[2]),
                        BuyAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit3[3]),
                        //Time1 =  
                        SellVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit3[5]),
                        SellPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit3[6]),
                        SellAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit3[7])
                        //Time2
                    } : null,
                    EnglishInstrumentName = priceItems[23],
                    EnglishInstrumentCode = priceItems[24],
                    //OpenPrice = 993978.0,
                    //RealBuyersAmount = 2,
                    //LegalBuyersAmount = 1,
                    //RealBuyingVolume = 422,
                    //LegalBuyingVolume = 18498,
                    //RealSellerAmount = 2,
                    //LegalSellerAmount = 1,
                    //RealSellingVolume = 6432,
                    //LegalSellingVolume = 12488,
                    //CoreType = "c",
                    //ApplyTime = "",
                    //ContractCoefficient = "",
                    //IpoStartTime = "",
                    //IpoEndTime = "",
                    //BestLimit4 = "",
                    //BestLimit5 = "",
                    //TheoryOpeningPrice = 0.0,
                    //TsetmcId = 5126621656875241,
                    //IndustryCode = 69,
                    //BaseOrderVolumeLog = 1,
                    //CompanyFarsiName = "اسنادخزانه-م22بودجه97-000428",
                    //CloseIndicator = 0,
                    //LastTradedPrice = 993500.0,
                    //TotalNumberOfTrades = 8,
                    //TotalNumberOfTradedShares = 18920.0,
                    //TotalTradeValue = 1.87957862E10,
                    //CompanyStatusId = "",
                    //CompanyStatusName = "",
                    //Reasons = "",
                    //LastStatusChange = "",
                    //Market = 2,
                    //GroupName = "بازار ابزارهاي نوين مالي فرابورس",
                    //LastYearMinTradePrice = 820030,
                    //LastYearMaxTradePrice = 994000,
                    //LastWeekMinTradePrice = 990000,
                    //LastWeekMaxTradePrice = 994000,
                    //LastMonthTurnover = 0.013719304883099089,
                    //LastThreeMonthesTurnover = 0.04356,
                    //NumberOfShares = 20151525,
                    //MarketOpenPositions = "",
                    //FirstTradeDate = "",
                    //LastTradeDate = "",
                    //PrioritySettlementDate = "",
                    //CashSettlementDate = "",
                    //BaseAssetCisinCode = "",
                    //InitalMarginRate = "",
                    //ReferencePrice = 991382.0,
                    //UnKnown1 = "",
                    //UnKnown2 = "",
                    //UnKnown3 = 3
                });
            }
            return result;
        }

        public List<MarketWatchDto> ConvertBestLimitResponse(string bestLimitMessage)
        {
            List<MarketWatchDto> result = new List<MarketWatchDto>();
            var marketWatchLine = bestLimitMessage.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var priceLine in marketWatchLine)
            {
                var priceItems = priceLine.Split(',');
                var bestLimit1 = priceItems.Length > 2 ? priceItems[2].Split(';') : null;
                var bestLimit2 = priceItems.Length > 3 ? priceItems[3].Split(';') : null;
                var bestLimit3 = priceItems.Length > 4 ? priceItems[4].Split(';') : null;

                result.Add(new MarketWatchDto()
                {
                    MessageCode = priceItems[0],
                    InsMaxLcode = priceItems[1],
                    BestLimit1 = (bestLimit1 != null && bestLimit1.Length > 7) ? new BestLimitDto()
                    {
                        Type = "bl1",
                        BuyVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit1[1]),
                        BuyPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit1[2]),
                        BuyAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit1[3]),
                        //Time1 =  
                        SellVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit1[5]),
                        SellPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit1[6]),
                        SellAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit1[7])
                        //Time2
                    } : null,
                    BestLimit2 = (bestLimit2 != null && bestLimit2.Length > 7) ? new BestLimitDto()
                    {
                        Type = "bl2",
                        BuyVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit2[1]),
                        BuyPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit2[2]),
                        BuyAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit2[3]),
                        //Time1 =  
                        SellVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit2[5]),
                        SellPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit2[6]),
                        SellAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit2[7])
                        //Time2
                    } : null,
                    BestLimit3 = (bestLimit3 != null && bestLimit3.Length > 7) ? new BestLimitDto()
                    {
                        Type = "bl3",
                        BuyVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit3[1]),
                        BuyPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit3[2]),
                        BuyAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit3[3]),
                        //Time1 =  
                        SellVolume1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit3[5]),
                        SellPrice1 = CommonInfrastructure.Helper.Conversions.ToDecimal(bestLimit3[6]),
                        SellAmount1 = CommonInfrastructure.Helper.Conversions.ToLong(bestLimit3[7])
                        //Time2
                    } : null,

                });
            }
            return result;
        }
        /// <summary>
        /// تبدیل به لیست پیام به تفکیک کد نماد ها
        /// </summary>
        /// <param name="marketMessage"></param>
        /// <returns></returns>
        public List<InstrumentMessageDto> ConvertToInstrumentMessages(string marketMessage)
        {
            if (string.IsNullOrEmpty(marketMessage))
            {
                return null; 
            }

            var result = new List<InstrumentMessageDto>(); 
            var messageLines = marketMessage.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var messageLine in messageLines)
            {
                var priceItems = messageLine.Split(',');
                if (priceItems.Length > 1)
                {
                    string prefix = priceItems[0];
                    string insMaxLCode = "";
                    if (prefix == CommonInfrastructure.Stock.PriceSocketMessageType.MarketWatch)
                    {
                        insMaxLCode = priceItems[2];
                    }
                    else if (prefix == CommonInfrastructure.Stock.PriceSocketMessageType.BestLimit)
                    {
                        insMaxLCode = priceItems[2];

                    }
                    else if (prefix == CommonInfrastructure.Stock.PriceSocketMessageType.Trade)
                    {
                        insMaxLCode = priceItems[1];
                    }

                    if (!string.IsNullOrEmpty(insMaxLCode))
                    {
                        result.Add(new InstrumentMessageDto() { 
                            InsMaxLCode = insMaxLCode,
                            MessagePrefix = prefix,
                            MessageText = messageLine
                        }); 
                    }
                }
            }
            return result; 

        }
    }
}
