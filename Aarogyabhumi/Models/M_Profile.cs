using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class M_Profile
    {
        public Profileres profileres {  get; set; }
    }


    public class Profilereq
    {
        public string islogin { get; set; }
        public string memberid { get; set; }
        public string passwd { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
    }


    public class Profileres 
    {
        public string idno { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string relation { get; set; }
        public string fname { get; set; }
        public string dob { get; set; }
        public string mobile { get; set; }
        public string phoneno { get; set; }
        public string email { get; set; }
        public string nominee { get; set; }
        public string nomineerelation { get; set; }
        public string dateofjoining { get; set; }
        public string dateofactivation { get; set; }
        public string sponsorid { get; set; }
        public string nomprefix { get; set; }
        public string response { get; set; }
    }


}