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
        public string E_MailAdd { get; set; }
        public decimal WalletBalance { get; set; }
        public string GroupPrefix { get; set; }
        public FranchiseLimit franchiseLimit { get; set; }
        public List<Product> TopsellingProduct { get; set; }
        public List<Product> StockProduct { get; set; }
        public List<clientProduct> TopclientProduct { get; set; }
        public DashboardSummary dashboardSummary { get; set; }
    }

    /// <summary>
    /// Dashboard: Sale / Purchase (aaj aur total) aur Stock, har ek Activation/Repurchase me bata hua.
    /// </summary>
    public class DashboardSummary
    {
        public DashboardSplit TodaySale { get; set; } = new DashboardSplit();
        public DashboardSplit TotalSale { get; set; } = new DashboardSplit();
        public DashboardSplit TodayPurchase { get; set; } = new DashboardSplit();
        public DashboardSplit TotalPurchase { get; set; } = new DashboardSplit();
        public DashboardSplit Stock { get; set; } = new DashboardSplit();
    }

    /// <summary>
    /// Product Master ke Imported column se: J = Activation, R = Repurchase, baaki (Both) = Other.
    /// </summary>
    public class DashboardSplit
    {
        public decimal Activation { get; set; }
        public decimal Repurchase { get; set; }
        public decimal Other { get; set; }
        public decimal Total { get { return Activation + Repurchase + Other; } }

        public void Add(string productFor, decimal amount)
        {
            string code = (productFor ?? "").Trim().ToUpper();
            if (code == "J") { Activation += amount; }
            else if (code == "R") { Repurchase += amount; }
            else { Other += amount; }
        }
    }
    public class FranchiseLimit
    {
        public decimal PVLimit { get; set; }
        public decimal BVLImit { get; set; }
        public decimal PVBalance { get; set; }
        public decimal BVBalance { get; set; }

    }

}
