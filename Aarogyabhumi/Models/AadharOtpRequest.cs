using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class AadharOtpRequest
    {
        [JsonProperty("@entity")]
        public string Entity { get; set; }

        [JsonProperty("aadhaar_number")]
        public string AadhaarNumber { get; set; }

        [JsonProperty("consent")]
        public string Consent { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
    public class AadhaarOtpLog
    {
        public string AadhaarNo { get; set; }

        public string ReferenceId { get; set; }

        public string Status { get; set; }

        public string ApiResponse { get; set; }
    }
    public class AadhaarOtpData
    {
        [JsonProperty("reference_id")]
        public long ReferenceId { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }
    }
    public class AadhaarOtpApiResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }

        [JsonProperty("data")]
        public AadhaarOtpData Data { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }
    }
    public class AadhaarVerifyData
    {
        [JsonProperty("@entity")]
        public string Entity { get; set; }

        [JsonProperty("reference_id")]
        public long? ReferenceId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("care_of")]
        public string CareOf { get; set; }

        [JsonProperty("full_address")]
        public string FullAddress { get; set; }

        [JsonProperty("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("email_hash")]
        public string EmailHash { get; set; }

        [JsonProperty("mobile_hash")]
        public string MobileHash { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("year_of_birth")]
        public string YearOfBirth { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("share_code")]
        public string ShareCode { get; set; }

        [JsonProperty("address")]
        public AadhaarAddress Address { get; set; }
    }
    public class AadhaarAddress
    {
        [JsonProperty("@entity")]
        public string Entity { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("house")]
        public string House { get; set; }

        [JsonProperty("landmark")]
        public string Landmark { get; set; }

        [JsonProperty("pincode")]
        public string Pincode { get; set; }

        [JsonProperty("post_office")]
        public string PostOffice { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("subdistrict")]
        public string Subdistrict { get; set; }

        [JsonProperty("vtc")]
        public string Vtc { get; set; }
    }
    public class AadharOtpVerifyRequest
    {
        [JsonProperty("@entity")]
        public string Entity { get; set; }

        [JsonProperty("reference_id")]
        public string reference_id { get; set; }

        [JsonProperty("otp")]
        public string otp { get; set; }
    }
    public class AadhaarOtpVerifyResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("data")]
        public AadhaarVerifyData Data { get; set; }
    }
}