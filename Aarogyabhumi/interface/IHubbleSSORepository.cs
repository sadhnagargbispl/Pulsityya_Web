using Shopinv.Models;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Interface
{
    public interface IHubbleSSORepository
    {
        void SaveToken(string token, string userId);
        HubbleSSOUserModel GetUserByToken(string token);
        void MarkTokenUsed(string token);
        UserModel GetUserByUserId(string userId); // 👈 add this
        DataTable DebitCoins(string userId,decimal coins, string referenceId, string note);
        DataTable ReverseCoins(string userId,string referenceId,string note);
    }
}