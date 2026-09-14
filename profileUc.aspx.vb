Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class profileUc
    Inherits System.Web.UI.Page
    Dim _dblAvailLeg As Double = 0
    Private cmd As New SqlCommand
    Private dRead As SqlDataReader
    Dim objDAL As DAL
    Private strQuery, strCaptcha As String
    Dim tmpTable As New Data.DataTable
    Dim minSpnsrNoLen, minScrtchLen As Integer
    Dim Upln, dblSpons, dblTehsil, dblDistrict, dblIdNo As Double
    Dim CurrDt As DateTime
    Dim montharray() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
    Dim LastInsertID As Integer = 0
    Dim scrname As String
    Public formNo As String
    Dim dt As New DataTable
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Member / Update Member Profile"
            Else
                Response.Redirect("logout.aspx")
            End If
            If Not Page.IsPostBack Then
                FillState()
                If (Session("CompID") = "1007") Then
                    divGst.Visible = True
                Else
                    divGst.Visible = False
                End If
                If Session("CompId") = 1082 Or Session("CompId") = 1092 Then
                    accounttype.Visible = False
                Else
                    accounttype.Visible = True
                End If
                If (Session("CompID") = "1026") Then
                    divCardNo.Visible = True
                Else
                    divCardNo.Visible = False
                End If
                If (Session("CompID") = "1066") Then
                    divdetail.Visible = True
                    divpresenter.Visible = True
                Else
                    divGst.Visible = False
                    divdetail.Visible = False
                    divpresenter.Visible = False
                End If
                divDetailSection.Visible = False
                FillDate()
                FillBankMaster()
                FindSession()
                FillIdtypeMaster()
                If Request("key") IsNot Nothing Then
                    FillDetail()
                    txtMemberId.Enabled = False
                End If
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub FillState()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim dt As DataTable = New DataTable()
            Dim str As String = " Select StateCode,StateName from M_StateDivMaster Where ActiveStatus =  'Y' And  RowStatus = 'Y' Order by StateName"
            dt = objDAL.GetData(str)
            ddlState.DataSource = dt
            ddlState.DataValueField = "StateCode"
            ddlState.DataTextField = "StateName"
            ddlState.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillDetail()
        Try
            divDetailSection.Visible = True
            Dim strqry As String = "" '"exec sp_MemDtl ' and mMst.Formno=" & Val(formNo) & "'"
            strqry = "exec sp_MemDtl ' and mMst.IDno=''" & txtMemberId.Text & "'''"
            If Request("key") IsNot Nothing Then
                strqry = "exec sp_MemDtl ' and mMst.IDno=''" & Request("key") & "'''"
                txtMemberId.Text = Request("key")
                txtMemberId.Enabled = False
            End If
            dt = New DataTable

            dt = objDAL.GenerateTreeProc(strqry)

            If dt.Rows.Count > 0 Then

                lblUplinerId.Text = dt.Rows(0)("UpLnIdNo") & ""
                lblUplnrNm.Text = dt.Rows(0)("UpLnName") & ""
                lblRefralId.Text = dt.Rows(0)("RefIdNo") & ""
                lblRefralNm.Text = dt.Rows(0)("RefName") & ""

                If dt.Rows(0)("MemRelation") = "" Then
                    CmbType.SelectedValue = "S/O"
                Else

                    CmbType.SelectedValue = (dt.Rows(0)("MemRelation")) & ""
                End If

                'MemfirstName.Text = dt.Rows(0)("MemName")
                MemfirstName.Text = dt.Rows(0)("MemFirstName")
                txtmemlastname.Text = dt.Rows(0)("MemlastName")
                txtFNm.Text = dt.Rows(0)("MemFname")
                txtAddLn1.Text = dt.Rows(0)("Address1")
                ddlDOBdt.SelectedValue = CType(dt.Rows(0)("MemDob"), Date).Day
                ddlDOBmnth.SelectedValue = CType(dt.Rows(0)("MemDob"), Date).Month
                ddlDOBYr.SelectedValue = CType(dt.Rows(0)("MemDob"), Date).Year
                ddlState.SelectedValue = dt.Rows(0)("StateCode")
                ddlDistrict.Text = dt.Rows(0)("DistrictName")
                ddlTehsil.Text = dt.Rows(0)("CityName")
                txtPinCode.Text = dt.Rows(0)("Pincode")

                If Session("CompID") = "1066" Then
                    txtpresenterid.Text = dt.Rows(0)("PresentId")
                    txtpresentername.Text = dt.Rows(0)("presenterName")

                End If


                DDLAddressProof.SelectedValue = dt.Rows(0)("Idtype")
                

                TxtIdProofNo.Text = dt.Rows(0)("IdProofNo")
                txtPhNo.Text = dt.Rows(0)("PhN1")
                txtMobileNo.Text = dt.Rows(0)("Mobl")
                TxtEmailID.Text = dt.Rows(0)("EMail")
                txtPanNo.Text = dt.Rows(0)("Panno")
                txtGstNo.Text = dt.Rows(0)("AadharNo3")

                If IsDBNull(dt.Rows(0)("BankId")) Then
                    CmbBank.SelectedValue = 0
                Else
                    CmbBank.SelectedValue = dt.Rows(0)("BankId")

                End If

                If IsDBNull(dt.Rows(0)("BranchName")) Then
                    TxtBranchName.Text = ""

                Else
                    TxtBranchName.Text = dt.Rows(0)("BranchName")
                End If
                If (Session("CompID") = "1078") Or (Session("CompID") = "1083") Or (Session("CompID") = "1084") Or (Session("CompID") = "1082") Or (Session("CompID") = "1089") Or (Session("CompID") = "1091") Or (Session("CompID") = "1092") Or (Session("CompID") = "1093") Then
                    If dt.Rows(0)("Fax").ToString.ToUpper = "CHOOSE ACCOUNT TYPE" Then
                        DDLAccountType.SelectedValue = "0"
                    Else
                        DDLAccountType.SelectedValue = dt.Rows(0)("Fax")
                    End If
                End If
                'End If
                'If dt.Rows(0)("Fax").ToString.ToUpper = "CHOOSE ACCOUNT TYPE" Then
                '    DDLAccountType.SelectedValue = "0"
                'Else
                '    DDLAccountType.SelectedValue = dt.Rows(0)("Fax")
                'End If

                'TxtBranchName.Text = dt.Rows(0)("BranchName")
                TxtAccountNo.Text = dt.Rows(0)("Acno")
                TxtIfsCode.Text = dt.Rows(0)("IFSCode")
                lblNominee.Text = dt.Rows(0)("NomineeName")
                lblRelation.Text = dt.Rows(0)("Relation")
                ' RbtPinPoint.SelectedValue = dt.Rows(0)("IsPinPoint")
                TxtTransactionPassword.Text = dt.Rows(0)("EPassw")
                TxtPassword.Text = dt.Rows(0)("Passw")

                If (Session("CompID") = "1026") Then
                    If (dt.Rows(0)("ActiveStatus") = "Y") Then
                        txtCardNo.Text = dt.Rows(0)("Fld5")
                    Else
                        txtCardNo.Enabled = False
                    End If

                End If
                Try
                    If dt.Rows(0)("ActiveStatus") = "N" Then
                        TxtStatus.Text = "Deactive"
                    Else
                        TxtStatus.Text = "Active"
                    End If
                    TxtPackage.Text = dt.Rows(0)("Category")
                Catch ex As Exception

                End Try


                btnShowMemDetail.Visible = False
                divDetailSection.Visible = True
                btnSubmit.Enabled = True
                btnSubmit.Visible = True
                Btncancel.Enabled = True
            Else
                btnShowMemDetail.Visible = True
                divDetailSection.Visible = False
                btnSubmit.Enabled = False
                Btncancel.Enabled = False
                btnSubmit.Visible = False


            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub FillDate()
        For i As Integer = 1 To 31
            ddlDOBdt.Items.Add(i)
            'ddlMarrDt.Items.Add(i)
        Next
        For i As Integer = 1 To 12
            ddlDOBmnth.Items.Add(i)
            ' ddlMarrMnth.Items.Add(i)
        Next
        For i As Integer = 1900 To 2031
            ddlDOBYr.Items.Add(i)
            'ddlMarrYr.Items.Add(i)
        Next
    End Sub
    Private Function ConvertDateToString(ByVal Month As String) As String
        Select Case Month
            Case 1
                Return "JAN"
            Case 2
                Return "FEB"
            Case 3
                Return "Mar"
            Case 4
                Return "Apr"
            Case 5
                Return "May"
            Case 6
                Return "Jun"
            Case 7
                Return "Jul"
            Case 8
                Return "Aug"
            Case 9
                Return "Sep"
            Case 10
                Return "Oct"
            Case 11
                Return "Nov"
            Case 12
                Return "Dec"
            Case Else
                Return ""
        End Select
    End Function
    Private Sub FillBankMaster()
        strQuery = "SELECT BankCode as BankCode,BANKNAME as Bank FROM " & objDAL.tblBankMaster & " WHERE  RowStatus='Y'  ORDER BY BANKNAME"
        'dbConnect.OpenConnection()
        tmpTable = New DataTable
        tmpTable = objDAL.GetData(strQuery)
        'dbConnect.Fill_Data_Tables(strQuery, tmpTable)
        With CmbBank
            .DataSource = tmpTable
            .DataValueField = "BankCode"
            .DataTextField = "Bank"
            .DataBind()
            .SelectedIndex = 0
        End With
    End Sub
    Private Sub FindSession()
        strQuery = "Select Top 1 SessID,ToDate,FrmDate from M_SessnMaster order by SessID desc"
        tmpTable = New DataTable
        tmpTable = objDAL.GetData(strQuery)
        If tmpTable.Rows.Count > 0 Then
            Session("SessID") = tmpTable.Rows(0)("SessID").ToString()
        End If
    End Sub
    Private Function GetCardNo() As Boolean
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim dt1 As DataTable = New DataTable()
            Dim Result As Boolean = False
            Dim str As String = "Select Count(*) As Cnt from M_MemberMAster where  ActiveStatus = 'Y' And  Fld5 <> '' And Fld5 = '" & txtCardNo.Text.Trim() & "' And Formno <> '" & hdnFormno.Value & "'  "
            dt1 = objDAL.GetData(str)
            If (Val(dt1.Rows(0)("Cnt")) = 0) Then
                Result = True
            Else
                Result = False
            End If
            Return Result
        Catch ex As Exception

        End Try
    End Function
    Private Sub UpdateDb()
        Dim value As Boolean = True
        formNo = GetFormNo()
        Dim strQry, strFld, strFldVal As String
        Dim strDOB, strDOM As String
        Dim cMarried As Char
        cMarried = "N"
        Dim remark As String = ""
        Try
            Dim str As String = ""
            Dim Dt1 As DataTable = New DataTable()
            Dim obj As DAL
            Dim Name As String = ""
            Dim mobile As String = ""
            Dim Password As String = ""
            Dim TransPassword As String = ""
            Dim AreaCode As Integer = 0
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            str = "select a.*,b.IdproofNo,b.Idtype from M_MemberMaster as a,KycVerify as b where a.Formno=b.Formno and a.Formno='" & Val(formNo) & "'"
            Dt1 = obj.GetData(str)
            If Dt1.Rows.Count > 0 Then
                strDOB = ddlDOBdt.Text & "-" & ConvertDateToString(Val(ddlDOBmnth.Text)) & "-" & ddlDOBYr.Text
                Name = Dt1.Rows(0)("MemFirstName")
                TransPassword = Dt1.Rows(0)("EPassw")
                Password = Dt1.Rows(0)("Passw")
                Dim dt As DataTable = New DataTable()
                Dim q As String = ""
                Dim i As Integer = 0
                Dim MemName As String = ""
                MemName = Trim(MemfirstName.Text)
                MemName = MemName.Trim
                Name = Name.Trim
                If ClearInject(Name) <> ClearInject(MemName) Then
                    remark = remark & "Name Changed From " & ClearInject(Name) & " to " & ClearInject(MemName) & ","
                End If
                Dim MemFName As String = ""
                MemFName = Trim(txtFNm.Text)
                MemFName = MemFName.Trim
                If ClearInject(Dt1.Rows(0)("MemFName")) <> ClearInject(MemFName) Then
                    remark = remark & " FatherName Changed From " & ClearInject(Dt1.Rows(0)("MemFName")) & " to " & ClearInject(MemFName) & ","
                End If
                If (Dt1.Rows(0)("MemDob")) <> strDOB Then
                    remark = remark & "Dob Changed From " & (Dt1.Rows(0)("MemDob")) & " to " & strDOB & ","
                End If
                If ClearInject(Dt1.Rows(0)("Address1")) <> ClearInject(txtAddLn1.Text) Then
                    remark = remark & " Address Changed From " & ClearInject(Dt1.Rows(0)("Address1")) & " to " & ClearInject(txtAddLn1.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("PinCode")) <> ClearInject(txtPinCode.Text) Then
                    remark = remark & " PinCode Changed From " & ClearInject(Dt1.Rows(0)("PinCode")) & " to " & ClearInject(txtPinCode.Text) & ","
                End If
                If Val(Dt1.Rows(0)("Statecode")) <> Val(ddlState.SelectedValue) Then
                    remark = remark & " State Changed From " & Val(Dt1.Rows(0)("Statecode")) & " to " & Val(ddlState.SelectedValue) & " ,"
                End If
                If ClearInject((Dt1.Rows(0)("District"))) <> ClearInject(ddlDistrict.Text) Then
                    remark = remark & " District Changed From " & ClearInject((Dt1.Rows(0)("District"))) & " to " & ClearInject(ddlDistrict.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("City")) <> ClearInject(ddlTehsil.Text) Then
                    remark = remark & " City Changed From " & ClearInject(Dt1.Rows(0)("City")) & " to " & ClearInject(ddlTehsil.Text) & ","
                End If
                If Val(Dt1.Rows(0)("Idtype")) <> Val(DDLAddressProof.SelectedValue) Then
                    remark = remark & " AddressProof Changed From " & Val(Dt1.Rows(0)("Idtype")) & " to " & Val(DDLAddressProof.SelectedValue) & ","
                End If
                If (Dt1.Rows(0)("IdproofNo")) <> (TxtIdProofNo.Text) Then
                    remark = remark & " AddressProofno Changed From " & (Dt1.Rows(0)("IdproofNo")) & " to " & (TxtIdProofNo.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("Mobl")) <> ClearInject(txtMobileNo.Text) Then
                    remark = remark & " MobileNo Changed From " & ClearInject(Dt1.Rows(0)("Mobl")) & " to " & ClearInject(txtMobileNo.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("PhN1")) <> ClearInject(txtPhNo.Text) Then
                    remark = remark & " PhoneNo Changed From " & ClearInject(Dt1.Rows(0)("PhN1")) & " to " & ClearInject(txtPhNo.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("Email")) <> ClearInject(TxtEmailID.Text) Then
                    remark = remark & " Email Changed From " & ClearInject(Dt1.Rows(0)("Email")) & " to " & ClearInject(TxtEmailID.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("NomineeName")) <> ClearInject(lblNominee.Text) Then
                    remark = remark & " NomineeName Changed From " & ClearInject(Dt1.Rows(0)("NomineeName")) & " to " & ClearInject(lblNominee.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("Relation")) <> ClearInject(lblRelation.Text) Then
                    remark = remark & " Relation Changed From " & ClearInject(Dt1.Rows(0)("Relation")) & " to " & ClearInject(lblRelation.Text) & ","
                End If
                If Dt1.Rows(0)("Fax") <> ClearInject(DDLAccountType.SelectedValue) Then
                    remark = remark & " AccountType Changed From " & Dt1.Rows(0)("Fax").ToString.Trim & " to " & ClearInject(DDLAccountType.SelectedValue) & ","
                End If
                If ClearInject(Dt1.Rows(0)("AcNo")) <> ClearInject(TxtAccountNo.Text) Then
                    remark = remark & " AccountNo Changed From " & ClearInject(Dt1.Rows(0)("AcNo")) & " to " & ClearInject(TxtAccountNo.Text) & ","
                End If
                If Val(Dt1.Rows(0)("BankId")) <> CmbBank.SelectedValue Then
                    remark = remark & " Bank Changed From " & Val(Dt1.Rows(0)("BankId")) & " to " & CmbBank.SelectedValue & ","
                End If
                If ClearInject(Dt1.Rows(0)("BranchName")) <> ClearInject(TxtBranchName.Text) Then
                    remark = remark & " BranchName Changed From " & ClearInject(Dt1.Rows(0)("BranchName")) & " to " & ClearInject(TxtBranchName.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("IFSCode")) <> ClearInject(TxtIfsCode.Text) Then
                    remark = remark & " IFSCCode Changed From " & ClearInject(Dt1.Rows(0)("IFSCode")) & " to " & ClearInject(TxtIfsCode.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("Passw")) <> ClearInject(TxtPassword.Text) Then
                    remark = remark & " Password Changed From " & ClearInject(Dt1.Rows(0)("Passw")) & " to " & ClearInject(TxtPassword.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("Panno")) <> ClearInject(txtPanNo.Text) Then
                    remark = remark & " PANNo Changed From " & ClearInject(Dt1.Rows(0)("Panno")) & " to " & ClearInject(txtPanNo.Text) & ","
                End If
            End If
            Dim Qry As String = "Insert Into TempMemberMaster Select *,'Update Profile',GetDate(),'U' From M_MemberMaster Where FormNo='" & Val(formNo) & "'"
            Qry = Qry & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId,HostIP)Values" & _
       "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Profile ','Profile Update','" & remark & "',Getdate(),'" & Val(formNo) & "','" & Context.Request.UserHostAddress.ToString & "')"
            Qry = Qry & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(formNo) & "' From KycVerify Where FormNo='" & Val(formNo) & "'"
            objDAL.SaveData(Qry)
            Dim sql As String = "Update m_MemberMaster set Prefix='" & CmbPrefix.SelectedItem.Text & "',MemFirstName='" & MemfirstName.Text.Trim() & "',MemlastName='" & txtmemlastname.Text.Trim() & "'," & _
            " MemRelation='" & CmbType.SelectedItem.Text & "'," & _
            " MemFName='" & txtFNm.Text.Trim & "',MemDOb='" & strDOB & "',Address1='" & txtAddLn1.Text.ToUpper & "',Pincode='" & txtPinCode.Text & "'" & _
            " ,StateCode='" & Val(ddlState.SelectedValue) & "',DistrictCode='0',District='" & ddlDistrict.Text.ToUpper & "'," & _
            " Tehsil='" & ddlTehsil.Text.ToUpper & "',City='" & ddlTehsil.Text.ToUpper & "'," & _
            " AreaCode='" & AreaCode & "',CityCode='0'," & _
            " PHN1='" & txtPhNo.Text & "',Mobl='" & txtMobileNo.Text & "',Email='" & TxtEmailID.Text.Trim & "',NomineeName='" & lblNominee.Text & "'," & _
            " Relation='" & lblRelation.Text & "',Fax='" & DDLAccountType.SelectedItem.Text & "',Acno='" & TxtAccountNo.Text & "',BankId='" & CmbBank.SelectedValue & "'," & _
            " BranchName='" & TxtBranchName.Text & "',IFSCode='" & TxtIfsCode.Text & "',PANNo='" & txtPanNo.Text & "',AadharNo3='" & txtGstNo.Text & "',Passw='" & TxtPassword.Text & "',E_MainPassw='" & TxtPassword.Text & "',Epassw='" & TxtTransactionPassword.Text & "', " & _
            " Fld5='" & txtCardNo.Text & "' where Formno= '" & Val(formNo) & "'"
            sql = sql & "Update KycVerify Set Idtype='" & DDLAddressProof.SelectedValue & "',IdProofNo='" & TxtIdProofNo.Text.Trim.ToUpper & "'" & _
            "  where Formno= '" & Val(formNo) & "'"
            Dim a As Integer = objDAL.SaveData(sql)
            If a <> 0 Then
                Dim Str2 As String = "ProfileUc.Aspx"
                If Request("key") IsNot Nothing Then
                    scrname = "alert('Profile Successfully Updated ');window.location.replace('" & Str2 & "','_blank');"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                    FillDetail()
                    btnSubmit.Visible = False
                Else
                    value = True
                    scrname = "alert('Profile Successfully Updated');"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                    FillDetail()
                    btnSubmit.Visible = False
                    btnShowMemDetail.Visible = True
                    Exit Sub
                End If
            End If
        Catch e As Exception
            value = False
            scrname = "<SCRIPT language='javascript'>alert('" & e.Message & "');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            Return
        End Try
    End Sub
    Private Function ClearInject(ByVal StrObj As String) As String
        StrObj = Replace(Replace(Replace(StrObj, ";", ""), "'", ""), "=", "")
        Return StrObj
    End Function
    Private Sub sendSMS(ByVal Username As String, ByVal MobileNo As String, ByVal Password As String, ByVal TransPassword As String, ByVal IdNo As String)
        Dim client As New WebClient
        Dim baseurl As String
        Dim data As Stream
        Dim datet As DateTime = Now
        Dim sms As String = "Dear " & Username & ", Your login details are ID-" & IdNo & "/ Pwd-" & Password & "/Trans Code-" & TransPassword & ", pls visit " & Session("CompWeb") & " for more details."
        Try
           baseurl = Session("SmsAPI") & "username=" & Session("SmsId") & "&password=" & Session("SmsPass") & "&Sender=" & Session("ClientId") & "&to=" & MobileNo & "&message=" & sms
            data = client.OpenRead(baseurl)
            Dim reader As New StreamReader(data)
            Dim s As String
            s = reader.ReadToEnd()
            data.Close()
            reader.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Trim(TxtAccountNo.Text) <> "" Or Trim(TxtIfsCode.Text) <> "" Or Trim(TxtBranchName.Text) <> "" Then
           
            If CmbBank.SelectedValue = 0 Then
                scrname = "alert('Choose Bank Name');"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                Exit Sub
            End If
            If TxtBranchName.Text = "" Then
                scrname = "alert('Enter Branch Name.');"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                Exit Sub
            End If
            If Session("CompId") = 1082 Or Session("CompId") = 1092 Then
            Else
                If DDLAccountType.SelectedValue = "0" Then
                    scrname = "alert('Enter Account Type.');"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                    Exit Sub
                End If
                If Trim(TxtAccountNo.Text) = "" Then
                    scrname = "alert('Enter Account No.');"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                    Exit Sub
                End If
            End If
           
            If TxtIfsCode.Text = "" Then
                scrname = "alert('Enter IFSC Code.');"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                Exit Sub
            End If
        End If
        txtMemberId.Enabled = True
        UpdateDb()
        divDetailSection.Visible = False
        txtMemberId.Text = ""
    End Sub
    Protected Sub btnShowMemDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowMemDetail.Click
        formNo = GetFormNo()
        If formNo = "" Then
            divDetailSection.Visible = False
            btnSubmit.Enabled = False
            Btncancel.Enabled = False
            txtMemberId.Enabled = True
        Else
            txtMemberId.Enabled = False
            divDetailSection.Visible = True
            btnSubmit.Enabled = True
            Btncancel.Enabled = True
            FillDetail()
            If Session("IsGetExtreme") = "N" Then
                tblSpnsr.Visible = False
            Else
                tblSpnsr.Visible = False
            End If
        End If
    End Sub
    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        'Dim formno As String
        idNo = txtMemberId.Text
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formNo = dt.Rows(0)("FormNo")
            hdnFormno.Value = dt.Rows(0)("FormNo")
            lblError.Visible = False

        Else
            lblError.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblError.Visible = True
            txtMemberId.Text = ""
        End If
        Return formNo
    End Function
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btncancel.Click
        Clear()
    End Sub
    Protected Sub Clear()
        txtMemberId.Enabled = True
        txtMemberId.Text = ""
        lblUplinerId.Text = ""
        lblUplnrNm.Text = ""
        lblRefralId.Text = ""
        lblRefralNm.Text = ""
        MemfirstName.Text = ""
        txtMobileNo.Text = ""
        TxtIfsCode.Text = ""
        txtPanNo.Text = ""
        txtPhNo.Text = ""
        txtFNm.Text = ""
        lblError.Visible = False
        TxtTransactionPassword.Text = ""
        ddlDistrict.Text = ""

        'ddlDOBdt.SelectedValue = ""
        'ddlDOBmnth.SelectedValue = ""
        'ddlDOBYr.SelectedValue = ""
        'ddlDOBdt.Text = ""
        'ddlDOBmnth.Text = ""
        'ddlDOBYr.Text = ""
        'DdlPaymode.Text = ""
        TxtAccountNo.Text = ""
        TxtBranchName.Text = ""
        TxtEmailID.Text = ""

        ddlTehsil.Text = ""
        txtAddLn1.Text = ""
        '  TxtAddLn2.Text = ""
        txtPinCode.Text = ""
        TxtPassword.Text = ""

        btnShowMemDetail.Visible = True
        divDetailSection.Visible = False
        btnSubmit.Enabled = False
        Btncancel.Enabled = False
        txtMemberId.Focus()
    End Sub
    Private Sub FillIdtypeMaster()
        Try
            Dim strQuery As String = ""
            strQuery = "SELECT Id,IdType  FROM M_IdTypeMaster WHERE ACTIVESTATUS='Y' "
            tmpTable = objDAL.GetData(strQuery)
            'DbConnect.Fill_Data_Tables(strQuery, tmpTable)
            With DDLAddressProof
                .DataSource = tmpTable
                .DataValueField = "Id"
                .DataTextField = "IdType"
                .DataBind()
                .SelectedIndex = 0
            End With
        Catch e As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & e.Message)
            Response.Write("Try later.")



        End Try

    End Sub
    Protected Sub txtCardNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCardNo.TextChanged
        Try
            If (Session("CompID") = "1026") Then
                If (GetCardNo() = False) Then
                    txtCardNo.Text = ""
                    scrname = "alert('Card No. Already Exists. Please try Again.!!');"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
