using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class M_Offers
    {
        public int AID { get; set; }
        public System.DateTime OfferFromDt { get; set; }
        public System.DateTime OfferToDt { get; set; }
        public string OfferDatePart { get; set; }
        public decimal OfferOnValue { get; set; }
        public decimal OfferOnBV { get; set; }
        public decimal TotalQty { get; set; }
        public string ActiveStatus { get; set; }
        public System.DateTime RecTimeStamp { get; set; }
        public string ForNewIds { get; set; }
        public Nullable<System.DateTime> IdDate { get; set; }
        public string IdStatus { get; set; }
        public decimal UserID { get; set; }
        public string ForBillType { get; set; }
        public Nullable<decimal> OfferType { get; set; }
        public string ForFranchise { get; set; }
        public string OfferName { get; set; }
        public decimal OfferOnToBV { get; set; }
        public string IsFixedQty { get; set; }
        public decimal FixedQty { get; set; }
    }
}
