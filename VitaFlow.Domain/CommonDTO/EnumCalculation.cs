using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.CommonDTO
{
    public class EnumCalculation
    {
        public class Enums
        {
            public enum CalculationConditionalVar
            {
                IsCommissonAdd = 0,//set 0 if don't want in calculation
                IsDiscountAdd = 0,//set 0 if don't want in calculation
                IsCommissonAddOnPartyBill = 1,
                IsDiscountAddOnPartyBill = 0,
            };

        }
    }
}
