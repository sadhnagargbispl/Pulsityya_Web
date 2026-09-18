using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VitaFlow.Application.IServices;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;

namespace VitaFlow.Application.Services
{
    public class LoginServices : I_Login_Service
    {
        private readonly I_Login i_loginRepository;
        public LoginServices(I_Login I_Login)
        {
            i_loginRepository = I_Login;
        }

        public Task<List<GroupModel>> GetGroupList()
        {
            return i_loginRepository.GetGroupList();
        }
        public Task<List<StateModel>> GetstateList()
        {
            return i_loginRepository.GetstateList();
        }
        public Task<string> GetPartyCode(string SelectedParentPartyCode, string SelectedGroupId)
        {
            return i_loginRepository.GetPartyCode(SelectedParentPartyCode, SelectedGroupId);
        }
        public Task<PartyModel> GetParentParty(string Partycode, string GroupId)
        {
            return i_loginRepository.GetParentParty(Partycode, GroupId);
        }
        public Task<int> SavePartyDetails(PartyModel obj)
        {
            return i_loginRepository.SavePartyDetails(obj);
        }
        public Task<User> ValidateUser(Login model)
        {
            return i_loginRepository.ValidateUser(model);
        }
        public Task<List<PartyModel>> GetParentPartyList(int Groupid, int stateCode, string District)
        {
            return i_loginRepository.GetParentPartyList(Groupid, stateCode, District);
        }
        public Task<string> GetPincode(string Pincode)
        {
            return i_loginRepository.GetPincode(Pincode);
        }
        public Task<List<PartyModel>> CheckExistsGroupid(int GroupId, int StateCode, string District, string Pincode)
        {
            return i_loginRepository.CheckExistsGroupid(GroupId, StateCode, District, Pincode);
        }
    }
}
