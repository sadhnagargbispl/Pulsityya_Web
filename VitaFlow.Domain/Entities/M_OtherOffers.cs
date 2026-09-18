using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class M_OtherOffers
    {
        public int AID { get; set; }
        public int OfferID { get; set; }
        public string OfferName { get; set; }
        public decimal MinBV { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public int StartProduct { get; set; }
        public string ActiveStatus { get; set; }
        public System.DateTime RecTimeStamp { get; set; }
        public string ForFranchise { get; set; }
        public decimal OfferOnToBV { get; set; }
        public string Forbill { get; set; }
    }
}
