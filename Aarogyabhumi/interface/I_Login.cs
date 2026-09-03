using Shopinv.Entity;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Interface
{
    public interface I_Login
    {
        IEnumerable<E_RegisterUser> SaveloginDetails(string UserName, string password, string FirstName, string LastName, string MobileNo, string FormNo, string ActiveStatus, string Fax, string Address, string City, string CityCode, string District, string DistrictCode, string StateCode, string PinCode, string CountryId, string CountryName, string RandomId,string Email);
        IEnumerable<E_RegisterUser> SrchUserDetail(string FormNo, string rnd);
        DataSet LoginApiUser(string Action, E_RegisterUser userDetail);
        IEnumerable<StateList> GetDDLState();
        DataSet GetStateFranchise(string StateCode);
        DataSet getRefdata(string refid);
        IEnumerable<E_RegisterUser> GetUserLoginDetail(string UserName, string Password, string id);
        IEnumerable<E_RegisterUser> GetUserotherLoginDetail(string UserName, string Password, string id);
        string UserPoints(string rndNo, string FormNo, string ProdId, string username, string UserPoints);
        IEnumerable<E_RegisterUser> SaveAddressDetail(string Action, string Id, string UserName, string Password, 
            string Email, string FirstName, string Lastname, string Mobile, string FormNo, string StateCode, 
            string District, string City, string Address, string PinCode, string AlternateMobileno, 
            string BillingAddress, string BillingCity,
            string BillingPinCode, string BillingStateCodebState);

        DataSet GetuserDetail(string userid, string IDno);
        E_RegisterUser Checklogin(E_RegisterUser userdetail);
        DataSet GetFranchiseProduct(string PartyCode);
        IEnumerable<StateList> GetDDLStateFranchise();

        DataSet GetMemberdetails(string dno);
        IEnumerable<E_Grivancelstres> GetComplaintType();
        DataSet GetSellerInformation(string Username);
    }
}
