using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class M_Complaint
    {
        public List<Complainttype> complainttype { get; set; }
        public List<Complaintdetail> complaintdetail { get; set; }
    }

    public class Complainttypelist
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string passwd { get; set; }
    }
    public class complainttyperes
    {
        public List<Complainttype> complainttype { get; set; }
        public string response { get; set; }
        public string msg { get; set; }
    }
    public class Complainttype
    {
        public string complaintid { get; set; }
        public string complaintname { get; set; }
    }
    public class Compaintreq
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string passwd { get; set; }
        public string complaintid { get; set; }
        public string idno { get; set; }
        public string name { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
    }
    public class Compaintres
    {
        public string response { get; set; }
        public string msg { get; set; }
    }
    public class ComplaintDetailsreq
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string passwd { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }
    public class ComplaintDetailsRes
    {
        public List<Complaintdetail> complaintdetail { get; set; }
        public string recordcount { get; set; }
        public string response { get; set; }
        public string msg { get; set; }
    }

    public class Complaintdetail
    {
        public string complaintid { get; set; }
        public string complaintdate { get; set; }
        public string complaint { get; set; }
        public string replydate { get; set; }
        public string reply { get; set; }
    }


    public class ComplaintReplyDetailsReq
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string passwd { get; set; }
        public string complaintid { get; set; }
    }


    public class ComplaintReplyDetailsRes
    {
        public List<Complaintreplydetail> complaintreplydetail { get; set; }
        public string complainttype { get; set; }
        public string complaint { get; set; }
        public string response { get; set; }
        public string msg { get; set; }
    }

    public class Complaintreplydetail
    {
        public string replydate { get; set; }
        public string reply { get; set; }
    }


    public class Forgotreq
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
    }


    public class Forgotres
    {
        public string response { get; set; }
        public string msg { get; set; }
        public string isuser { get; set; }
        public string ismailsent { get; set; }
        public string issmssent { get; set; }
    }
}