using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_RegisterUser
    {
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string AlternateMobileno { get; set; } 
        public string Fax { get; set; }
        public string FormNo { get; set; }
        public string ActiveStatus { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int CityCode { get; set; }
        public string District { get; set; }
        public int DistrictCode { get; set; }
        public int CountryId { get; set; }
        public decimal StateCode { get; set; }
        public string PinCode { get; set; }
        public string CountryName { get; set; }
        public string randomId { get; set; }
        public int Id { get; set; }
         public string PartyCode { get; set; }
         public string DeliveryAddress { get; set; }
        public decimal BillingStateCode { get; set; } 
    }

    public class StateList
    {
        public decimal StateCode { get; set; }
        public string State { get; set; }
    }

     public class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string Address6 { get; set; }
        public string Address7 { get; set; }
        public string Address8 { get; set; }
        public string Address9 { get; set; }
        public string Address10 { get; set; }
    }
    public class E_AadhaarOtpLog
    {
        public string AadhaarNo { get; set; }

        public string ReferenceId { get; set; }

        public string Status { get; set; }

        public string ApiResponse { get; set; }
    }
    public class E_AadhaarOtpVerifyLog
    {
        public string ReferenceId { get; set; }
        public string OTP { get; set; }

        public string Name { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }

        public string Address { get; set; }

        public string Status { get; set; }
        public string Message { get; set; }
        public string CareOf { get; set; }

        public string EmailHash { get; set; }
        public string MobileHash { get; set; }

        public string YearOfBirth { get; set; }
        public string ShareCode { get; set; }

        public string Country { get; set; }
        public string District { get; set; }
        public string House { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }
        public string PostOffice { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string Subdistrict { get; set; }
        public string VTC { get; set; }

        public string Photo { get; set; }

        public string ApiResponse { get; set; }

        public string TransactionId { get; set; }
    }
}