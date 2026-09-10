using Shopinv.Entity;
using Shopinv.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Shopinv.Interface
{
    public interface I_Login
    {
        IEnumerable<E_RegisterUser> SaveloginDetails(string UserName, string password, string FirstName, string LastName, string MobileNo, string FormNo, string ActiveStatus, string Fax, string Address, string City, string CityCode, string District, string DistrictCode, string StateCode, string PinCode, string CountryId, string CountryName, string RandomId, string Email);
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
        string SendWhatsappMessage(string formNo, string name,string mobileNo, string message);

        IEnumerable<E_AadhaarOtpLog> SaveAadhaarOtpLog(string AadhaarNo, string ReferenceId, string Status, string ApiResponse, string TransactionId);
        IEnumerable<E_AadhaarOtpVerifyLog> SaveAadhaarOtpVerifyLog(
string ReferenceId,
string OTP,
string Name,
string DOB,
string Gender,
string Address,
string Status,
string Message,
string CareOf,
string EmailHash,
string MobileHash,
string YearOfBirth,
string ShareCode,
string Country,
string District,
string House,
string Landmark,
string Pincode,
string PostOffice,
string State,
string Street,
string Subdistrict,
string VTC,
string Photo,
string ApiResponse,
string TransactionId
);
    }
}
