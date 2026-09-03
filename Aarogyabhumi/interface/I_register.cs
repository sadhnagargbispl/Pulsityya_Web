using Shopinv.Entity;
using Shopinv.Models;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Interface
{
    interface I_register
    {
        IEnumerable<M_Register> Register(string Action, string firstName, string LastName, string E_Mail, string Password);
        DataSet SaveRegistration(E_sregistration obj);
        DataSet SaveOTP(string OTP ,string Email);
        DataSet GetOTP(string Email);
        DataSet getemail(string Email);
        DataSet Getmobileno(string mobileno);
    }
}
