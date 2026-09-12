using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string BranchCode { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string FCode { get; set; }
        public int PartyId { get; set; }
        public int GroupId { get; set; }
        public int StateCode { get; set; }
        public string IsAdmin { get; set; }
        public string ParentPartyCode { get; set; }
        public string WBalance { get; set; }
        //public List<MenuMasterModel> objMenuList { get; set; }
        public bool IsSoldByHo { get; set; }
        public string IsActionName { get; set; }
        public string ActiveStatus { get; set; }
        public string Remarks { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string ISApprove { get; set; }
        public string Address1 { get; set; }
        public string PinCode { get; set; } 
        public string MobileNo { get; set; }    
        public string E_MailAdd  { get; set; }
        public decimal WalletBalance { get; set; }
        public string GroupPrefix { get; set; } 
        public FranchiseLimit franchiseLimit { get; set; }
		public List<Product> TopsellingProduct { get; set; }
        public List<Product> StockProduct { get; set; }
        public List<clientProduct> TopclientProduct { get; set; }
    }
    public class FranchiseLimit
    {
        public decimal PVLimit { get; set; }
        public decimal BVLImit { get; set; }
        public decimal PVBalance { get; set; }
        public decimal BVBalance { get; set; }

    }
  
}
