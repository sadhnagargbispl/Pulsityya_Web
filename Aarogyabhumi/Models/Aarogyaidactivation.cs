namespace Shopinv.Models
{
    public class Aarogyaidactivation
    {
        public string reqtype { get; set; }
        public string toidno { get; set; }
        public string passwd { get; set; }
        public string amount { get; set; }
        public string bv { get; set; }
        public string transpassword { get; set; }
        public string islogin { get; set; }
        public string billtype { get; set; }
    }

    public class AarogyaidactivationResponse
    {
        public string response { get; set; }
        public string msg { get; set; }
        public string billno { get; set; }
    }

    public class BsnIdactivation
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Action { get; set; }
        public string TxnData { get; set; }
        public string Amount { get; set; }
    }


    public class Bsnaddbresponse 
    {
        public string loginid { get; set; }
        public string msg { get; set; }
        public string status { get; set; }
        public string voucherno { get; set; }
    }


}