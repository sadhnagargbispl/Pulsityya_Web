Imports System.Data
Imports System.Data.SqlClient
Partial Class UpdateRewardPaid
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim UserIdQS As String
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            Dim str = "exec('Create table TrnRewardUpdate ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,"
            str &= "PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,"
            str &= "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] "
            str &= "ALTER TABLE [dbo].[TrnRewardUpdate] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
            Dim i As Integer = 0
            i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
        Catch ex As Exception
        End Try
        If String.IsNullOrEmpty(Request("Rewardid")) = False Then
            UserIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("Rewardid")))
        End If
        If Not Page.IsPostBack Then
            HdnCheckTrnns.Value = GenerateRandomStringJoining(6)
            ClearAll()
            txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
            If String.IsNullOrEmpty(Request("Rewardid")) = False Then
                'BindData()
            End If
        End If
    End Sub
    Public Function GenerateRandomStringJoining(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "123456789".ToCharArray()
        Dim sResult As String = ""
        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next
        Return sResult
    End Function
    'Private Sub BindData()
    '    Dim sql As String = "Select * From " + objDAL.tblNewsMaster + " Where NewsId='" & UserIdQS & "' AND " + objDAL.activeCondition
    '    'Dim sql As String = ""
    '    Dt = New DataTable
    '    Dt = objDAL.GetData(sql)
    '    If Dt.Rows.Count > 0 Then
    '        'DDLType.SelectedValue = Dt.Rows(0)("NType")
    '        txtNewsID.Text = Dt.Rows(0)("NewsId")
    '        txtHeading.Text = Dt.Rows(0)("NewsHdr")
    '        txtDetail.Text = Dt.Rows(0)("NewsDtl")
    '        txtFrmDate.Text = Format(Dt.Rows(0)("FrmDate"), "dd-MMM-yyyy")
    '        txtToDate.Text = Format(Dt.Rows(0)("ToDate"), "dd-MMM-yyyy")
    '        txtRemarks.Text = Dt.Rows(0)("Remarks")
    '        txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
    '        If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
    '            rdblist.SelectedIndex = 0
    '        Else
    '            rdblist.SelectedIndex = 1
    '        End If
    '    End If
    'End Sub
    Private Sub ClearAll()
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
        txtFrmDate.Text = ""
    End Sub
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Strquery = "Insert into TrnRewardUpdate (Transid) values(" & HdnCheckTrnns.Value & ")"
        Dim isOk1 As Integer = 0
        isOk1 = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Strquery)
        If isOk1 > 0 Then
            Dim Sql As String
            Dim FrmDate As String
            FrmDate = txtFrmDate.Text
            Try
                Dim Dt As DateTime = FrmDate
            Catch ex As Exception
                scrname = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" & "</SCRIPT>"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", scrname, True)
                Exit Sub
            End Try
            If Session("CompID") = "1103" Then
                Sql = "Update MstRewardAchievers SET PaidStatus = 'Y',Remark = '" & txtRemarks.Text & "',PaidDate = '" & txtFrmDate.Text & "' Where Rewardid  = '" & Request("Rewardid") & "' AND formno = '" & Request("formno") & "';"

                Sql &= " INSERT INTO Tempm_MstRewardAchievers(SessId,FormNo,RewardId,Reward,WSessID,Remark,PaidDate,LastModified,Comm,Rid,Rp)"

                Sql &= " SELECT SessId,FormNo,RewardId,Reward,WSessID,'" & txtRemarks.Text & "','" & txtFrmDate.Text & "',"

                Sql &= " 'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "',"

                Sql &= " Comm,Rid,Rp FROM MstRewardAchievers "

                Sql &= " WHERE Rewardid = '" & Request("Rewardid") & "' AND formno = '" & Request("formno") & "'"

            Else

                Sql = "Update M_RewardFinal SET PaidStatus = 'Y',Remark = '" & txtRemarks.Text & "',PaidDate = '" & txtFrmDate.Text & "' Where Rewardid  = '" & Request("Rewardid") & "' AND formno = '" & Request("formno") & "';"
                Sql &= " insert into TempM_RewardFinal(SessId,FormNo,PwrLeg,WkrLeg,NewPair,RewardId,Reward,Amount,PanNo,TdsAmount,AdminCharge,ChqAmt,RecTimeStamp,IsTransfer,"
                Sql &= "WSessID,ISRedeem,RedeemDate,UpgradeDate,DayCnt,Remark,PaidDate,LastModified)"
                Sql &= " select SessId,FormNo,PwrLeg,WkrLeg,NewPair,RewardId,Reward,Amount,PanNo,TdsAmount,AdminCharge,ChqAmt,RecTimeStamp,IsTransfer,"
                Sql &= "WSessID,ISRedeem,RedeemDate,UpgradeDate,DayCnt,'" & txtRemarks.Text & "','" & txtFrmDate.Text & "',"
                Sql &= "'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "' from M_RewardFinal "
                Sql &= "where Rewardid  = '" & Request("Rewardid") & "' AND formno = '" & Request("formno") & "' "

            End If

            'Sql = "Update M_RewardFinal SET PaidStatus = 'Y',Remark = '" & txtRemarks.Text & "',PaidDate = '" & txtFrmDate.Text & "' Where Rewardid  = '" & Request("Rewardid") & "' AND formno = '" & Request("formno") & "';"
            'Sql &= " insert into TempM_RewardFinal(SessId,FormNo,PwrLeg,WkrLeg,NewPair,RewardId,Reward,Amount,PanNo,TdsAmount,AdminCharge,ChqAmt,RecTimeStamp,IsTransfer,"
            'Sql &= "WSessID,ISRedeem,RedeemDate,UpgradeDate,DayCnt,Remark,PaidDate,LastModified)"
            'Sql &= " select SessId,FormNo,PwrLeg,WkrLeg,NewPair,RewardId,Reward,Amount,PanNo,TdsAmount,AdminCharge,ChqAmt,RecTimeStamp,IsTransfer,"
            'Sql &= "WSessID,ISRedeem,RedeemDate,UpgradeDate,DayCnt,'" & txtRemarks.Text & "','" & txtFrmDate.Text & "',"
            'Sql &= "'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "' from M_RewardFinal "
            'Sql &= "where Rewardid  = '" & Request("Rewardid") & "' AND formno = '" & Request("formno") & "' "

            Dim updateEffect As Integer = 0
            updateEffect = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Sql)
            If (updateEffect > 0) Then
                scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');location.replace('UpdateRewardPaid.aspx');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');location.replace('UpdateRewardPaid.aspx');" & "</SCRIPT>"
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('This News already register.!');location.replace('Reward.aspx');", True)
            Exit Sub
        End If
        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        ClearAll()
    End Sub
End Class
