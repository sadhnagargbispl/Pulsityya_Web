using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace Shopinv.Models
{
    public class SendboxAuthenticate
    {
        public int code { get; set; }
        public long timestamp { get; set; }
        public string access_token { get; set; }
        public TokenData data { get; set; }
        public string transaction_id { get; set; }
    }

    public class TokenData
    {
        public string access_token { get; set; }
    }


    public class PanVerifyreq 
    {
        [JsonProperty("@entity")]
        public string Entity { get; set; } = "in.co.sandbox.kyc.pan_verification.request";

        [JsonProperty("pan")]
        public string Pan { get; set; } = string.Empty;

        [JsonProperty("name_as_per_pan")]
        public string NameAsPerPan { get; set; } = string.Empty;

        [JsonProperty("date_of_birth")]
        public string DateOfBirth { get; set; } = string.Empty;  // "DD/MM/YYYY"

        [JsonProperty("consent")]
        public string Consent { get; set; } = "Y";               // usually "Y" / "N"

        [JsonProperty("reason")]
        public string Reason { get; set; } = "Y";
    }

    public class PanVerificationApiResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }     // usually unix ms timestamp

        [JsonProperty("data")]
        public PanVerificationData Data { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        // Optional: helper property to get DateTime from timestamp
        [JsonIgnore]
        public DateTime TimestampAsDateTime
        {
            get
            {
                // 1970-01-01 + milliseconds
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    .AddMilliseconds(Timestamp);
            }
        }
    }

    public class PanVerificationData
    {
        [JsonProperty("@entity")]
        public string Entity { get; set; }

        [JsonProperty("pan")]
        public string Pan { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }          // "valid", "invalid", etc.

        [JsonProperty("remarks")]
        public string Remarks { get; set; }         // can be null

        [JsonProperty("name_as_per_pan_match")]
        public bool NameAsPerPanMatch { get; set; }

        [JsonProperty("date_of_birth_match")]
        public bool DateOfBirthMatch { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }        // "individual", "company", ...

        [JsonProperty("aadhaar_seeding_status")]
        public string AadhaarSeedingStatus { get; set; }   // "y", "n", "NA", ...
    }

    public class PennyDropResponse
    {
        public int code { get; set; }
        public long timestamp { get; set; }
        public string transaction_id { get; set; }
        public PennyDropData data { get; set; }
    }

    public class PennyDropData
    {
        [Newtonsoft.Json.JsonProperty("@entity")]
        public string entity { get; set; }

        public string message { get; set; }
        public bool account_exists { get; set; }
        public string name_at_bank { get; set; }
        public string utr { get; set; }
        public string amount_deposited { get; set; }
    }

    public class KycBankSaveRequest
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string passwd { get; set; }
        public string accounttype { get; set; }
        public string accountno { get; set; }
        public string bankcode { get; set; }
        public string bankname { get; set; }
        public string branchname { get; set; }
        public string ifsccode { get; set; }
    }
    public class KycBankSaveResponse
    {
        public string response { get; set; }
        public string msg { get; set; }
    }

    public class KycAddressRequest
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string passwd { get; set; }

        public string address { get; set; }
        public string citycode { get; set; }
        public string cityname { get; set; }
        public string pincode { get; set; }
        public string statecode { get; set; }

        public string districtcode { get; set; }
        public string district { get; set; }

        public string areaname { get; set; }
        public string areacode { get; set; }

        public string idproofid { get; set; }
        public string idproofno { get; set; }

        public string frontaddressproof { get; set; }
        public string backaddressproof { get; set; }
    }

    public class KycCommonResponse
    {
        public string response { get; set; }
        public string msg { get; set; }
    }

    public class FormUploadRequest
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string passwd { get; set; }
        public string formupload { get; set; }
    }

    public class FormUploadResponse
    {
        public string response { get; set; }
        public string msg { get; set; }
    }

    public class KycPanSaveRequest
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string passwd { get; set; }
        public string panno { get; set; }
    }

    public class KycPanSaveResponse
    {
        public string response { get; set; }
        public string msg { get; set; }
    }


}