using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Application.Contracts.Services
{
    public interface IFinancialFormulaService
    {
        decimal CalcApproxYieldToMaturity(decimal faceValue, decimal price, decimal daysToMaturity, decimal commission);
        decimal CalcPriceBasedOnYield(decimal faceValue, decimal daysToMaturity, decimal commission, decimal expectedYield); 

    }
    public class FinancialFormulaService:IFinancialFormulaService
    {
        public decimal CalcApproxYieldToMaturity(decimal faceValue, decimal price, decimal daysToMaturity, decimal commission)
        {
            var result = Math.Pow((double)(faceValue / (price * (1 + commission))), (double)((decimal)365 / daysToMaturity));
            return (decimal)result - 1;
        }

        public decimal CalcPriceBasedOnYield(decimal faceValue, decimal daysToMaturity, decimal commission, decimal expectedYield)
        {
            //var divisor = (decimal)Math.Pow((double)(1 + (expectedYield / (decimal)12)), (double)((decimal)12 * daysToMaturity / (decimal)365));
            //var divisor = (decimal)Math.Pow((double)(1 + (expectedYield / (decimal)12)), (double)(daysToMaturity / (decimal)365));
            var divisor = (decimal)Math.Pow((double)(1 + expectedYield), (double)(daysToMaturity / (decimal)365));
            var tempValue = faceValue / divisor;

            return (decimal)(tempValue / (decimal)(1 + commission));

        }
    }
}
