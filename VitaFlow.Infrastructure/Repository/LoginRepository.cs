using Dapper;
using System;
using System.Data;
using System.Text.RegularExpressions;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;
using VitaFlow.Infrastructure.DapperContext;


namespace VitaFlow.Infrastructure.Repository
{
    public class LoginRepository : I_Login
    {
        private readonly DapperDbContext _context;
        public LoginRepository(DapperDbContext dapperContext)
        {
            _context = dapperContext;
        }
        public async Task<User> ValidateUser(Login model)
        {
            User obj = new User();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string sqlQuery = @"
                                   SELECT 
                                   u.UserId,
                                   u.UserName,
                                   u.Passw AS Password,
                                   u.BranchCode,
                                   l.PartyCode,
                                   l.PartyName,
                                   l.GroupId,
                                   l.PartyCode AS FCode,
                                   l.StateCode,
                                   u.IsAdmin,
                                   l.ParentPartyCode,
                                   l.CityName,
                                   s.StateName,
                                   l.PartyName,
                                   l.ISApprove,
                                   u.ActiveStatus,
                                   l.Address1,
                                   l.PinCode,
                                   l.MobileNo,
                                   l.E_MailAdd,
                                   g.Prefix AS GroupPrefix
                                   FROM Inv_M_UserMaster u
                                   JOIN M_LedgerMaster l ON u.BranchCode = l.PartyCode
                                   JOIN M_GroupMaster g ON l.GroupId = g.GroupId
                                   inner join M_StateDivMaster as s on l.StateCode=s.StateCode
                                   WHERE 
                                   l.OnWebSite='Y' AND l.ActiveStatus='Y'
                                   AND u.UserName = @UserName  
                                   AND u.Passw = @Password
                                   ";
                    obj = (await connection.QueryAsync<User>(sqlQuery, new { UserName = model.Username, Password = model.Password })).FirstOrDefault();
                }
            }
            catch
            {

            }
            return obj;
        }

        public async Task<List<GroupModel>> GetGroupList()
        {
            List<GroupModel> obj = new List<GroupModel>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var sql = "Select GroupId,GroupName from M_GroupMaster where GroupId not in (0,5) and ActiveStatus='Y' and InvLogin='Y' order by GroupId";
                    obj = (await connection.QueryAsync<GroupModel>(sql, commandType: CommandType.Text)).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        public async Task<List<StateModel>> GetstateList()
        {
            List<StateModel> obj = new List<StateModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "Select * from M_StateDivMaster where RowStatus='Y' AND ActiveStatus='Y'";
                    obj = (await connection.QueryAsync<StateModel>(sql, commandType: CommandType.Text)).ToList();
                }
            }
            catch
            {

            }
            return obj;
        }
        public async Task<string> GetPartyCode(string SelectedParentPartyCode, string SelectedGroupId)
        {
            string PartyCode = "";
            decimal GroupId = 0;
            decimal maxPcode;
            string groupPrefix;
            try
            {
                if (!string.IsNullOrEmpty(SelectedGroupId))
                {
                    GroupId = decimal.Parse(SelectedGroupId);
                }
                if (SelectedGroupId == "" && SelectedParentPartyCode == "")
                {
                    using (var connection = _context.CreateLiveconnInv())
                    {
                        // Get MaxPcode
                        string maxPcodeQuery = @"
                               SELECT ISNULL(MAX(PCode), 0)
                               FROM M_LedgerMaster
                               WHERE GroupId = @GroupId";
                        maxPcode = (await connection.QueryAsync<decimal>(maxPcodeQuery, new { GroupId = 5 })).Single();
                        maxPcode += 1;
                    }
                    // Determine PartyCode
                    if (maxPcode.ToString().Length == 1)
                    {
                        PartyCode = "S" + "0" + maxPcode.ToString().ToUpper().Trim();
                    }
                    else
                    {
                        PartyCode = "S" + maxPcode.ToString().ToUpper().Trim();
                    }
                }
                else
                {
                    using (var connection = _context.CreateLiveconnInv())
                    {
                        string maxPcodeQuery = @"
                           SELECT ISNULL(MAX(PCode), 0)
                            FROM M_LedgerMaster
                            WHERE GroupId = @GroupId";
                        maxPcode = (await connection.QueryAsync<decimal>(maxPcodeQuery, new { GroupId })).Single();
                        maxPcode += 1;

                        // Get GroupPrefix
                        string groupPrefixQuery = @"
                                                  SELECT Prefix
                                                  FROM M_GroupMaster
                                                  WHERE ActiveStatus = 'Y' AND GroupId = @GroupId";
                        groupPrefix = (await connection.QueryAsync<string>(groupPrefixQuery, new { GroupId })).FirstOrDefault();
                    }
                    // Format StrPcode
                    string strPcode = maxPcode.ToString();
                    if (strPcode.Length < 2)
                    {
                        int toBeAddedDigits = 2 - strPcode.Length;
                        for (int j = 0; j < toBeAddedDigits; j++)
                        {
                            strPcode = "0" + strPcode;
                        }
                    }
                    // Combine to form PartyCode
                    //PartyCode = SelectedParentPartyCode + groupPrefix.ToUpper().Trim() + strPcode.ToUpper().Trim();
                    PartyCode =  groupPrefix.ToUpper().Trim() + strPcode.ToUpper().Trim(); //remove wr from part code 
                }

            }
            catch
            {

            }
            return PartyCode;
        }

        public async Task<PartyModel> GetParentParty(string PartyCode, string GroupId)
        {
            PartyModel obj = new PartyModel();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string sql = @"
                              SELECT *
                              FROM M_LedgerMaster
                               WHERE PartyCode=@PartyCode";
                    //and GroupId < @GroupId
                    var objres = (await connection.QueryAsync<PartyModel>(sql, new { PartyCode })).FirstOrDefault();
                    if (objres != null)
                    {
                        obj = objres;
                    }
                }
            }
            catch
            {

            }
            return obj;
        }
        public async Task<int> SavePartyDetails(PartyModel objPartyModel)
        {
            int Savechange = 0;
            try
            {
                M_LedgerMaster objDTLedger = new M_LedgerMaster();
                int maxUserId = 0;
                decimal maxPcode = 0;
                using (var connection = _context.CreateLiveconnInv())
                {
                    string sqlmaxUserId = @" 
                     SELECT ISNULL(MAX(UserId), 0)
                      FROM Inv_M_UserMaster";
                    // Execute the query and get the result
                    maxUserId = (await connection.QuerySingleOrDefaultAsync<int>(sqlmaxUserId));

                    objDTLedger.GroupId = objPartyModel.GroupId;
                    objDTLedger.PGroupId = objPartyModel.PGroupId;
                    objDTLedger.UserPartyCode = objPartyModel.PartyCode;
                    string sqlmaxPcode = @"
                      SELECT ISNULL(MAX(PCode), 0)
                      FROM M_LedgerMaster
                      WHERE GroupId = @GroupId";
                    // Execute the query and get the result
                    maxPcode = (await connection.QuerySingleOrDefaultAsync<decimal>(sqlmaxPcode, new { GroupId = objPartyModel.GroupId }));
                    maxPcode = maxPcode + 1;
                    objDTLedger.PCode = maxPcode;
                    objDTLedger.PartyCode = objPartyModel.PartyCode;
                    objDTLedger.PartyName = objPartyModel.PartyName;
                    objDTLedger.ParentPartyCode = objPartyModel.ParentPartyCode;
                    objDTLedger.Address1 = string.IsNullOrEmpty(objPartyModel.Address1) ? "" : objPartyModel.Address1;
                    objDTLedger.Address2 = string.IsNullOrEmpty(objPartyModel.Address2) ? "" : objPartyModel.Address2;
                    objDTLedger.StateCode = objPartyModel.StateCode;
                    objDTLedger.CityCode = objPartyModel.CityCode;
                    objDTLedger.CityName = objPartyModel.CityName;
                    objDTLedger.Tehsil = string.IsNullOrEmpty(objPartyModel.Tehsil) ? "" : objPartyModel.Tehsil;
                    objDTLedger.PinCode = objPartyModel.PinCode;
                    objDTLedger.PhoneNo = string.IsNullOrEmpty(objPartyModel.PhoneNo) ? "" : objPartyModel.PhoneNo;
                    objDTLedger.MobileNo = objPartyModel.MobileNo;
                    objDTLedger.FaxNo = string.IsNullOrEmpty(objPartyModel.FaxNo) ? "" : objPartyModel.FaxNo;
                    objDTLedger.PanNo = string.IsNullOrEmpty(objPartyModel.PanNo) ? "" : objPartyModel.PanNo;
                    objDTLedger.TinNo = string.IsNullOrEmpty(objPartyModel.GSTIN) ? "" : objPartyModel.GSTIN;
                    objDTLedger.STaxNo = string.IsNullOrEmpty(objPartyModel.STaxNo) ? "" : objPartyModel.STaxNo;
                    objDTLedger.CstNo = objPartyModel.CstNo;
                    objDTLedger.BankAcNo = string.IsNullOrEmpty(objPartyModel.BankAccNo) ? "" : objPartyModel.BankAccNo;
                    objDTLedger.BankCode = objPartyModel.BankCode;
                    objDTLedger.BankName = string.IsNullOrEmpty(objPartyModel.BankName) ? "" : objPartyModel.BankName;
                    objDTLedger.RequestTo = string.IsNullOrEmpty(objPartyModel.RequestTo) ? "" : objPartyModel.RequestTo;
                    objDTLedger.AccountVerify = string.IsNullOrEmpty(objPartyModel.AccountVerify) ? "" : objPartyModel.AccountVerify;
                    objDTLedger.RecommandBy = string.IsNullOrEmpty(objPartyModel.RecommandBy) ? "" : objPartyModel.RecommandBy;
                    objDTLedger.ContactPerson = string.IsNullOrEmpty(objPartyModel.ContactPerson) ? "" : objPartyModel.ContactPerson;
                    objDTLedger.E_MailAdd = string.IsNullOrEmpty(objPartyModel.EmailAddress) ? "" : objPartyModel.EmailAddress;
                    objDTLedger.ActiveStatus = objPartyModel.ActiveStatus;
                    objDTLedger.OnWebSite = "Y";
                    objDTLedger.CreditLimit = objPartyModel.CreditLimit;
                    objDTLedger.Remarks = string.IsNullOrEmpty(objPartyModel.Remarks) ? "" : objPartyModel.Remarks;
                    objDTLedger.NewFld1 = string.IsNullOrEmpty(objPartyModel.NewFId1) ? "" : objPartyModel.NewFId1;
                    //objDTLedger.NewFld2 = string.IsNullOrEmpty(objPartyModel.objUserDetails.UserName) ? "" : objPartyModel.objUserDetails.UserName;
                    objDTLedger.NewFld2 = "";
                    objDTLedger.NewFld3 = "";
                    objDTLedger.NewFld4 = "";
                    objDTLedger.LastModified = "";
                    objDTLedger.Company = "";
                    objDTLedger.RecvdCForm = "N";
                    objDTLedger.UserName = objPartyModel.PartyCode;
                    objDTLedger.TinNo = objPartyModel.CstNo;

                    //--------------------------------------------
                    var M_LedgerMastersql = @"
                       insert into M_LedgerMaster(GroupId,PGroupId,UserPartyCode,
                       PCode,PartyCode,ParentPartyCode,PartyName,Address1,Address2,StateCode,CityCode,
                       CityName,Tehsil,PinCode,PhoneNo,MobileNo,FaxNo,PanNo,TinNo,CstNo,STaxNo,BankAcNo,BankCode,
                       BankName,RequestTo,AccountVerify,RecommandBy,ContactPerson,E_MailAdd,ActiveStatus,OnWebSite,
                       CreditLimit,Remarks,RecTimeStamp,NewFld1,NewFld2,NewFld3,NewFld4,Company,UserId,
                       UserName,LastModified,RecvdCForm)
                       values(@GroupId,@PGroupId,@UserPartyCode,
                       @PCode,@PartyCode,@ParentPartyCode,@PartyName,@Address1,@Address1,@StateCode,@CityCode,
                       @CityName,@Tehsil,@PinCode,@PhoneNo,@MobileNo,@FaxNo,@PanNo,@TinNo,@CstNo,@STaxNo,@BankAcNo,@BankCode,
                       @BankName,@RequestTo,@AccountVerify,@RecommandBy,@ContactPerson,@E_MailAdd,@ActiveStatus,@OnWebSite,
                       @CreditLimit,@Remarks,Getdate(),@NewFld1,@NewFld2,@NewFld3,@NewFld4,@Company,@UserId,
                       @UserName,@LastModified,@RecvdCForm)";
                    //var anonymousCustomer = new { Name = "ZZZ Projects", Email = "zzzprojects@example.com" };
                    var rowsAffected = await connection.ExecuteAsync(M_LedgerMastersql, objDTLedger);
                    if (rowsAffected > 0)
                    {
                        // insert into Inv_M_UserMaster
                        Inv_M_UserMaster objDTUserMaster = new Inv_M_UserMaster();
                        objDTUserMaster.UserName = objPartyModel.PartyCode;
                        objDTUserMaster.Passw = objPartyModel.Password;
                        objDTUserMaster.BranchCode = objPartyModel.PartyCode;
                        objDTUserMaster.FCode = objPartyModel.PartyCode;
                        string versionsql = @"
                          SELECT TOP 1 VersionNo
                          FROM M_NewHOVersionInfo";
                        // Execute the query and get the result
                        string version = (await connection.QueryFirstOrDefaultAsync<string>(versionsql));
                        objDTUserMaster.Version = version;
                        maxUserId = maxUserId + 1;
                        objDTUserMaster.UserId = maxUserId;
                        objDTUserMaster.Remarks = "";
                        objDTUserMaster.LUserId = 88;
                        objDTUserMaster.LastModified = "";
                        objDTUserMaster.ActiveStatus = "Y";
                        objDTUserMaster.CreateBy = "Admin";
                        objDTUserMaster.GroupId = objPartyModel.GroupId;
                        objDTUserMaster.IsAdmin = "N";
                        objDTUserMaster.LastIP = "";
                        objDTUserMaster.LoginStatus = "N";
                        objDTUserMaster.Status = "Y";

                        var Inv_M_UserMastersql = @"
                             insert into Inv_M_UserMaster(
                             UserId,BranchCode,FCode,UserName,Passw,Remarks,Status,CreateDate,CreateBy,LastIP,
                             Version,LoginStatus,GroupId,ActiveStatus,RecTimeStamp,LUserId,LastModified,IsAdmin,
                             LastLogOutTime)
                             values(@UserId,@BranchCode,@FCode,@UserName,@Passw,@Remarks,@Status,Getdate(),@CreateBy,@IsAdmin,
                             @Version,@LoginStatus,@GroupId,@ActiveStatus,getdate(),@LUserId,@LastModified,@IsAdmin,
                             getdate())";
                        var DTUserMasterrowsAffected = await connection.ExecuteAsync(Inv_M_UserMastersql, objDTUserMaster);
                        if (rowsAffected > 0 && DTUserMasterrowsAffected > 0)
                        {
                            Savechange = 1;
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }
            return Savechange;
        }
        public async Task<List<PartyModel>> GetParentPartyList(int Groupid, int stateCode, string District)
        {
            List<PartyModel> obj = new List<PartyModel>();
            using (var connection = _context.CreateLiveconnInv())
            {
                try
                {
                    if (Groupid == 1 || Groupid == 2)
                    {
                        var sql = "select PartyCode,PartyName from M_LedgerMaster where GroupId<@GroupId and ISApprove='Y'";
                        obj = (await connection.QueryAsync<PartyModel>(sql, new { Groupid })).ToList();
                    }
                    else if (Groupid == 3)
                    {
                        var sql = "select PartyCode,PartyName from M_LedgerMaster where ( GroupId<@GroupId and StateCode=@stateCode and ISApprove='Y' ) or GroupId=0";
                        obj = (await connection.QueryAsync<PartyModel>(sql, new { Groupid, stateCode })).ToList();
                    }
                    else if (Groupid == 4)
                    {
                        var sql = "select PartyCode,PartyName from M_LedgerMaster where ( GroupId<@GroupId and StateCode=@stateCode and CityName=@District and ISApprove='Y') or  GroupId=0 ";
                        obj = (await connection.QueryAsync<PartyModel>(sql, new { Groupid, stateCode, District })).ToList();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return obj;
        }
        public async Task<string> GetPincode(string Pincode)
        {
            string rstr = string.Empty;
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.postalpincode.in/pincode/" + Pincode + "");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                rstr = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

            }
            return rstr;
        }

        public async Task<List<PartyModel>> CheckExistsGroupid(int GroupId, int StateCode, string District, string Pincode) 
        {
            List<PartyModel> obj = new List<PartyModel>();
            using (var connection = _context.CreateLiveconnInv()) 
            {
                try
                {
                    if (GroupId == 1)
                    {
                        var sql = "select PartyCode,PartyName from M_LedgerMaster where GroupId=@GroupId and StateCode=@stateCode and ActiveStatus='Y'";
                        obj = (await connection.QueryAsync<PartyModel>(sql, new { GroupId, StateCode })).ToList(); 
                    }
                    else if (GroupId == 2)
                    {
                        var sql = "select PartyCode,PartyName,CityName,CityCode from M_LedgerMaster where GroupId=@GroupId and UPPER(CityName)=UPPER('@District') and ActiveStatus='Y'";
                        obj = (await connection.QueryAsync<PartyModel>(sql, new { GroupId, StateCode, District })).ToList();
                    }
                    else if (GroupId == 3) 
                    {
                        var sql = "select PartyCode,PartyName,CityName,CityCode from M_LedgerMaster where GroupId=@GroupId and PinCode=@Pincode and ActiveStatus='Y'";
                        obj = (await connection.QueryAsync<PartyModel>(sql, new { GroupId,  Pincode })).ToList(); 
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return obj;
        }
    }
}
