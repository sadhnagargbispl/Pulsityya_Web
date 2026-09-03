using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_Grivance
    {
        public string islogin { get; set; }
        public string passwd { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string name { get; set; }
        public string idno { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }
        public string complaintid { get; set; }
        public string subject { get; set; }
        public string description { get; set; }

    }
    public class E_Grivancetype
    {
        public string islogin { get; set; }
        public string passwd { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
    }

    public class E_Grivancetypecomplain
    {
        public string islogin { get; set; }
        public string passwd { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string complaintid { get; set; }
    }

    public class E_Grivancelst
    {
        public string  response { get; set; }
        public string msg { get; set; }

    }

      public class E_Grivancelstres {
        public decimal complaintid { get; set; }
        public string complaintname { get; set; }
    }

     public class E_Grivancetypecomplainres
    {
         public List<complaintreplydetail> complaintreplydetail { get; set; }
        public string complainttype { get; set; }
        public string  complaint { get; set; }
        public string  response { get; set; }
        public string msg { get; set; }

    }

     public class complaintreplydetail
    {
        public string  replydate { get; set; }
         public string reply { get; set; }
    }

}