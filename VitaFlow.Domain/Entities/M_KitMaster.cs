using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class M_KitMaster
    {
        public decimal KId { get; set; }
        public decimal KitId { get; set; }
        public string KitName { get; set; }
        public decimal JoinAmount { get; set; }
        public decimal KitAmount { get; set; }
        public decimal KitUnit { get; set; }
        public string SerialStart { get; set; }
        public decimal RefIncome { get; set; }
        public decimal PoolIncome { get; set; }
        public decimal SpillIncome { get; set; }
        public decimal BinaryIncome { get; set; }
        public decimal BV { get; set; }
        public decimal PV { get; set; }
        public decimal RP { get; set; }
        public decimal Capping { get; set; }
        public string Remarks { get; set; }
        public char ActiveStatus { get; set; }
        public DateTime RecTimeStamp { get; set; }
        public string LastModified { get; set; }
        public string UserCode { get; set; }
        public decimal UserId { get; set; }
        public char JoinStatus { get; set; }
        public string JoinColor { get; set; }
        public char AllowTopUp { get; set; }
        public string Statement { get; set; }
        public char IsBill { get; set; }
        public decimal SP { get; set; }
        public char OnWebSite { get; set; }
        public char RowStatus { get; set; }
        public string IPAdrs { get; set; }
        public string MACAdrs { get; set; }
        public decimal Deduction { get; set; }
        public char IsSMS { get; set; }
        public decimal TopUpSeq { get; set; }
        public decimal ShopPoint { get; set; }
        public string PromoName { get; set; }
        public string PromoId { get; set; }
        public char IsFood { get; set; }
        public char IsBigBazzar { get; set; }
        public string PromoName1 { get; set; }
        public string PromoId1 { get; set; }
        public decimal ShopPoint1 { get; set; }
        public string OldKit { get; set; }
        public decimal ShoppingUnit { get; set; }
        public decimal ShoppingQty { get; set; }
        public decimal OtherCouponMRP { get; set; }
        public decimal OtherCouponQty { get; set; }
        public int NoOfIds { get; set; }
        public decimal Coupon { get; set; }
        public char IsHoliday { get; set; }
    }
}
