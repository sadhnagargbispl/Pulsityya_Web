using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class M_Franchiselist
    {
        public int GroupId { get; set; }
        public string PartyName { get; set; }
        public string Address1 { get; set; }
        public string CityName { get; set; }
        public string Tehsil { get; set; }
        public decimal PinCode { get; set; }
        public decimal MobileNo { get; set; }
        public decimal MCommission { get; set; }
        public decimal SponsorCommission { get; set; }
        public decimal DistrictCommission { get; set; }
        public decimal DepotCommission { get; set; }
        public string GroupName { get; set; }
        public string StateName { get; set; }
        public string PartyCode { get; set; }
    }
}