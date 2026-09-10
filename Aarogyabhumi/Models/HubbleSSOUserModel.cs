using System.Collections.Generic;

namespace Shopinv.Models
{
    public class HubbleSSOUserModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Cohorts { get; set; }

    }
    public class UserModel
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public decimal CoinBalance { get; set; }
    }
    public class CoinsDebitRequest
    {
        public string userId { get; set; }

        public decimal coins { get; set; }

        public string referenceId { get; set; }

        public string note { get; set; }
    }
    public class CoinsReverseRequest
    {
        public string userId { get; set; }

        public string referenceId { get; set; }

        public string note { get; set; }
    }
}