using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class M_UserKYC
    {
        public Getkycres Userkycres { get; set; }
        public List<State> states { get; set; }
        public List<BankList> BankLists { get; set; }
        public List<KycTypeMaster> kycTypeMasters { get; set; }
    }

    public class BankList
    {
        public string BankName { get; set; }
        public decimal BankCode { get; set; }
    }

    public class KycTypeMaster
    {
        public decimal Id { get; set; }
        public string IdType { get; set; }
    }
    public class KycSavereq
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
        public string accounttype { get; set; }
        public string accountno { get; set; }
        public string bankcode { get; set; }
        public string bankname { get; set; }
        public string branchname { get; set; }
        public string ifsccode { get; set; }
        public string bankimage { get; set; }
        public string panno { get; set; }
        public string panimage { get; set; }
        public string formupload { get; set; }
    }

    public class KycSaveres
    {
        public string response { get; set; }
        public string msg { get; set; }
    }

    public class Getkycreq
    {
        public string islogin { get; set; }
        public string passwd { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
    }
    public class Getkycres
    {
        public string idno { get; set; }
        public string addrsverf { get; set; }
        public string idverf { get; set; }
        public string rejectreason { get; set; }
        public string rejectremark { get; set; }
        public string vaerifydate { get; set; }
        public string isBankverified { get; set; }
        public string BankVerf { get; set; }
        public string BankRejectReason { get; set; }
        public string BankRejectRemark { get; set; }
        public string BankProofDate { get; set; }
        public string IsPanVerified { get; set; }
        public string PanVerf { get; set; }
        public string PanRejectReason { get; set; }
        public string PanRejectRemark { get; set; }
        public string PanVerifyDate { get; set; }
        public string formuploadVerf { get; set; }
        public string formuploadVerifyDate { get; set; }
        public Addressdetail addressdetail { get; set; }
        public Bankdetail bankdetail { get; set; }
        public Pandetail pandetail { get; set; }
        public Formuploaddetail formuploaddetail { get; set; }
        public string response { get; set; }
        public string msg { get; set; }
        public string Isformupload { get; set; }
    }

    public class Formuploaddetail
    {
        public string formupload { get; set; }
    }

    public class Addressdetail
    {
        public string idproof { get; set; }
        public string address { get; set; }
        public string pincode { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string statecode { get; set; }
        public string statename { get; set; }
        public string addrproof { get; set; }
        public string IdproofNo { get; set; }
        public string backaddressproof { get; set; }
        public string backaddressdate { get; set; }
        public string idtype { get; set; }
        public string areacode { get; set; }
    }

    public class Bankdetail
    {
        public string bankid { get; set; }
        public string acno { get; set; }
        public string ifscode { get; set; }
        public string accounttype { get; set; }
        public string branchname { get; set; }
        public string bankproof { get; set; }
    }

    public class Pandetail
    {
        public string panno { get; set; }
        public string panimage { get; set; }
    }
}