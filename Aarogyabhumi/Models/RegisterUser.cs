using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class RegisterUser
    {
        public string reqtype { get; set; }
        [Required]
        public string referralid { get; set; }
        [Required]
        public string side { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string fname { get; set; }
        public string dob { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        [MaxLength(10)]
        public string mobl { get; set; }
        public string nominee { get; set; }
        [Required]
        public string relation { get; set; }
        [Required]
        public string branch { get; set; }
        public string bankcode { get; set; }
        public string ifsc { get; set; }
        public string accountno { get; set; }
        public string actype { get; set; }
        [Required]
        public string panno { get; set; }
        public string islogin { get; set; }
        [Required]
        public int statecode { get; set; }
        [Required]
        public string pincode { get; set; }
        [Required]
        public string district { get; set; }
        [Required]
        public string city { get; set; }
        public List<State> states { get; set; }
       
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }
        public string fortype { get; set; }
    }


    public class Satarereq
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string countrycode { get; set; }
    }

    public class Stateroot
    {
        public List<State> states { get; set; }
        public string response { get; set; }
        public string msg { get; set; }
    }

    public class State
    {
        public string statecode { get; set; }
        public string statename { get; set; }
    }

    public class Signupresponse
    {
        public string response { get; set; }
        public string msg { get; set; }
        public string idno { get; set; }
        public string password { get; set; }
        public string formno { get; set; }
    }


    public class sponsorResponse 
    {
        public string response { get; set; }
        public string sponsorname { get; set; }
        public string showupline { get; set; }
        public string msg { get; set; }
    }

}