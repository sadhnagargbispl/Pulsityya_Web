Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_BecomeFranchise
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Dt As New DataTable
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Member / Become Franchisee "
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim searchtext As String = Session("Search")
        If Not Page.IsPostBack Then

            Session("FranchiseeData") = Nothing
            BindGroup()
            BindParentId()


        End If
    End Sub
    Public Sub BindGroup()
        Dim sql As String = "Select * FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_GRoupMAster WHERE ActiveStatus='Y' AND GROUPID in (1,2,3,7) "
        objModuleFun.FillCombo(sql, DDlGroup, "GroupName", "GroupId")

    End Sub
    Public Sub BindParentId()
        Dim sql As String = "Select Partycode+'  '+ '('+ PartyName +')' as Party,PartyCode FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & ".. M_LedgerMAster WHERE ActiveStatus='Y' AND GROUPID in (0,1,2,3) AND GroupID <'" & DDlGroup.SelectedValue & "'"
        objModuleFun.FillCombo(sql, DDlParentId, "Party", "PartyCode")

    End Sub

    Protected Sub clearAll()
        txtMemberId.Text = ""
        LblMemName.Text = ""
        LblFormno.Text = ""
        LblPassw.Text = ""
        TxtRemark.Text = ""


    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub





    Protected Sub txtMemberId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberId.TextChanged
        Check_IdNo()
    End Sub
    Private Function Check_IdNo() As Boolean
        Dim sql As String
        sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName , Mobl,Formno,Fld5,Passw From " & objDAL.tblMemberMaster & " WHERE IDNO='" & Trim(txtMemberId.Text) & "'"
        Dim Dt_ As New DataTable
        Dt_ = objDAL.GetData(sql)
        If Dt_.Rows.Count = 0 Then
            LblMemName.Text = " Please enter correct Member ID."
            LblMemName.ForeColor = Drawing.Color.Red
            LblFormno.Text = ""
            txtMemberId.Text = ""
            btnchngpswd.Enabled = False
            'LblError.Visible = False
            LblPassw.Text = ""
            Return False
        Else
            If Dt_.Rows(0)("Fld5") = "F" Then
                'LblError.Text = "Member Already Franchisee"
                'LblError.ForeColor = Drawing.Color.Red
                'LblError.Visible = True
                btnchngpswd.Enabled = False
                LblMemName.Text = Dt_.Rows(0)("MemName") & " Already Franchisee "
                LblMemName.ForeColor = Drawing.Color.Black
                LblPassw.Text = ""
                btnchngpswd.Enabled = False

            Else
                LblMemName.Text = Dt_.Rows(0)("MemName")
                LblMemName.ForeColor = Drawing.Color.Black
                LblFormno.Text = Dt_.Rows(0)("Formno")
                LblPassw.Text = Dt_.Rows(0)("Passw")
                btnchngpswd.Enabled = True
                'LblError.Visible = False
                Return True
            End If
        End If
    End Function

    Protected Sub btnchngpswd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnchngpswd.Click
        Dim str As String = ""
        Dim sql As String = ""
        Dim UserPartyCode As String = ""
        Dim PGroupId As String = ""
        Dim UsePcode As String = ""
        Dim PCode As String = ""
        Dim Pcode1 As String = ""
        Dim dt As DataTable
        Dim Prefix As String = ""
        Dim scrname As String = ""
        dt = New DataTable
        sql = "select b.Prefix,a.UserPartyCode,a.GroupID from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_LedgerMAster a, " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_GroupMaster as b where b.GroupId='" & DDlGroup.SelectedValue & "' AND  a.PartyCode='" & DDlParentId.SelectedItem.Value & "'"

        Dim qry As String = "select Case When Max(Pcode) Is Null Then '1' Else Max(PCode)+1 END as MaxPcode from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_LedgerMaster  where GroupId='" & DDlGroup.SelectedValue & "'  "
        dt = objDAL.GetData(sql)
        If dt.Rows.Count > 0 Then
            PGroupId = dt.Rows(0)("GroupId")
            UsePcode = dt.Rows(0)("UserPartyCode")
            Prefix = dt.Rows(0)("Prefix")

        End If
        dt = New DataTable
        dt = objDAL.GetData(qry)
        If dt.Rows.Count > 0 Then
            Pcode1 = Val(dt.Rows(0)("MaxPCode"))
            PCode = Val(dt.Rows(0)("MaxPCode"))
            If PCode.Length = 1 Then
                PCode = "00" & PCode
            ElseIf UsePcode.Length = 2 Then
                PCode = "0" & PCode
            End If
            UserPartyCode = UsePcode & Prefix & PCode
        End If
        txtMemberId.Text = txtMemberId.Text.ToUpper
        Dim Remark As String = ""
        Remark = " Become Franchisee To IdNo " & txtMemberId.Text & ""
        str = "Update M_MemberMaster Set Fld5='f' where formno='" & LblFormno.Text & "' "
        str = str & "Insert into " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_LedgerMaster(" & _
             " GroupId,PGroupId,UserPartyCode,PCode,PartyCode,ParentPartyCode,PartyName,Address1,Address2," & _
             " StateCode,CityCode,CityName,Tehsil,PinCode,PhoneNo,MobileNo,FaxNo,PanNo,TinNo,CstNo,STaxNo,BankAcNo," & _
              " BankCode,BankName,RequestTo,AccountVerify,RecommandBy,ContactPerson,E_MailAdd,ActiveStatus,OnWebSite," & _
            " CreditLimit,Remarks,RecTimeStamp,NewFld1,NewFld2,NewFld3,NewFld4,Company,UserId,UserName,LastModified, RecvdCForm)" & _
        "select " & DDlGroup.SelectedValue & "," & PGroupId & ",'" & UserPartyCode & "','" & Pcode1 & "','" & txtMemberId.Text & "','" & DDlParentId.SelectedValue & "'" & _
 " ,'" & LblMemName.Text & "',Address1,Address2,StateCode,CityCode,City,Tehsil,Cast(PinCode as Numeric),PHN1,Mobl,Fax,PanNo,0,0,0,a.AcNo,a.BankId,b.BankName," & _
 " '','','','" & LblMemName.Text & "',Email,'Y','N',0,'" & TxtRemark.Text & "',GetDate(),'','" & Val(LblFormno.Text) & "','','',''," & Session("UserId") & ",'" & Session("UserName") & "','','' " & _
 " from M_MemberMaster as a,M_bankMaster as b where a.BankId=b.BankCode and b.ActiveStatus='Y' and b.RowStatus='Y' and a.Formno=" & Val(LblFormno.Text) & ""
        str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
        "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Become Franchisee ','Become Franchisee','" & Remark & "',Getdate(),'" & Val(LblFormno.Text) & "');"
        str = str & "  insert into " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..Inv_M_UserMaster(" & _
                " UserId, BranchCode, FCode, UserName, Passw, Remarks, Status, CreateDate ,CreateBy,LastIP,LastLoginTime," & _
                " LastLogOutTime,Version,LoginStatus,GroupId,ActiveStatus,RecTimeStamp,LUserId,LastModified,IsAdmin)" & _
              " select Case When Max(UserId) Is Null Then '1' Else Max(UserId)+1 END as UserId,'" & txtMemberId.Text & "','" & txtMemberId.Text & "'," & _
              "'" & txtMemberId.Text & "','" & LblPassw.Text & "','" & TxtRemark.Text & "','Y',GetDate(),'" & Session("UserName") & "',' ',Getdate(), " & _
              " Getdate(),'','N','" & DDlGroup.SelectedValue & "','Y',GetDate()," & Session("UserId") & ",'','N' from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..Inv_M_UserMaster"
        '"& TxtMe&"'
        Dim a As Integer = objDAL.UpdateData(str)
        If a <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Member become franchisee Successfully!! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)

            clearAll()
        End If
    End Sub

    Protected Sub DDlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlGroup.SelectedIndexChanged
        BindParentId()
    End Sub
End Class
