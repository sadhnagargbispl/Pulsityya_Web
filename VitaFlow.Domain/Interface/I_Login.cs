using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VitaFlow.Domain.Entities;


namespace VitaFlow.Domain.Interface
{
    public interface I_Login
    {
        Task<List<GroupModel>> GetGroupList();
        Task<List<StateModel>> GetstateList();
        Task<string> GetPartyCode(string SelectedParentPartyCode, string SelectedGroupId);
        Task<PartyModel> GetParentParty(string Partycode,string GroupId);
        Task<int> SavePartyDetails(PartyModel obj);
        Task<User> ValidateUser(Login model);
        Task<List<PartyModel>> GetParentPartyList(int Groupid, int stateCode, string District);
        Task<string> GetPincode(string Pincode);
        Task<List<PartyModel>> CheckExistsGroupid(int GroupId,int StateCode,string District,string Pincode);  
    }
}
