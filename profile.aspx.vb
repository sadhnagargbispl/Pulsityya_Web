Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class Profile
    Inherits System.Web.UI.Page
    Dim _dblAvailLeg As Double = 0
    Private cmd As New SqlCommand
    Private dRead As SqlDataReader
    Dim objDAL As DAL
    Private strQuery, strCaptcha As String
    Dim tmpTable As New Data.DataTable
    ' Dim QryCls As New AccClass.MyAccClass.NewClass
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
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Member / Update Member Profile"
                ' UserStatus.InnerHtml = "<p>Welcome " & Session("MemName") & "(" & Session("FormNo") & ") To" & Session("CompName") & " </p>"
            Else
                Response.Redirect("logout.aspx")
            End If

            If Not Page.IsPostBack Then

                If (Session("CompID") = "1007") Then
                    divGst.Visible = True
                Else
                    divGst.Visible = False
                End If

                If (Session("CompID") = "1026") Then
                    divCardNo.Visible = True
                Else
                    divCardNo.Visible = False
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
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff" & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
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

                MemfirstName.Text = dt.Rows(0)("MemName")
                txtFNm.Text = dt.Rows(0)("MemFname")
                txtAddLn1.Text = dt.Rows(0)("Address1")
                ddlDOBdt.SelectedValue = CType(dt.Rows(0)("MemDob"), Date).Day
                ddlDOBmnth.SelectedValue = CType(dt.Rows(0)("MemDob"), Date).Month
                ddlDOBYr.SelectedValue = CType(dt.Rows(0)("MemDob"), Date).Year
                ddlDistrict.Text = dt.Rows(0)("DistrictName")
                ddlTehsil.Text = dt.Rows(0)("CityName")
                txtPinCode.Text = dt.Rows(0)("Pincode")
                txtPinCode.Text = dt.Rows(0)("Pincode")
                FillCityPinDetail()

                DDLAddressProof.SelectedValue = dt.Rows(0)("Idtype")
                If dt.Rows(0)("AreaCode") <> 0 Then
                    DDlArea.SelectedValue = dt.Rows(0)("areacode")
                End If

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
                If dt.Rows(0)("Fax").ToString.ToUpper = "CHOOSE ACCOUNT TYPE" Then
                    DDLAccountType.SelectedValue = "0"
                Else
                    DDLAccountType.SelectedValue = dt.Rows(0)("Fax")
                End If

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
    Private Sub FillCityPinDetail()
        Try
            Dim sql As String = ""
            If Val(txtPinCode.Text) <> 0 Then
                sql = "select a.Statename,b.DistrictName,c.CityName,d.VillageName,d.Pincode,a.StateCode,b.DistrictCode" & _
                      " ,c.CityCode,d.VillageCode from M_STateDivMaster as a with( NoLock) Inner Join M_DistrictMaster as b " & _
                      " with( NoLock) On a.StateCode=b.StateCode and a.ActivEstatus='Y' and b.ActiveStatus='Y' " & _
                      " Inner Join  M_CityStatemaster as c with( NoLock)  On b.DistrictCode=c.DistrictCode and c.ActivEstatus='Y' " & _
                      " Inner Join M_VillageMaster as d with( NoLock) On c.CityCode=d.CityCode and d.ActiveStatus='Y'" & _
                      " where  d.Pincode='" & Val(txtPinCode.Text) & "'"

                Dim Dt As DataTable
                Dt = New DataTable
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                Dt = objDAL.GetData(sql)
                If Dt.Rows.Count > 0 Then
                    ddlTehsil.Text = Dt.Rows(0)("CityName")
                    ddlDistrict.Text = Dt.Rows(0)("DistrictName")
                    StateCode.Value = Dt.Rows(0)("Statecode")
                    HDistrictCode.Value = Dt.Rows(0)("Districtcode")
                    HCityCode.Value = Dt.Rows(0)("Citycode")
                    txtStateName.Text = Dt.Rows(0)("StateName")
                    Dim dr As DataRow
                    dr = Dt.NewRow
                    dr("Villagecode") = 381264
                    dr("Villagename") = "Others"
                    Dt.Rows.Add(dr)
                    With DDlArea
                        .DataSource = Dt
                        .DataValueField = "VillageCode"
                        .DataTextField = "VillageName"
                        .DataBind()
                        .SelectedIndex = 0
                    End With
                    'txtPinCode.Focus()
                Else
                    StateCode.Value = 0
                    HDistrictCode.Value = 0
                    HCityCode.Value = 0
                End If
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Sub
    Protected Sub txtPinCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPinCode.TextChanged
        Try
            Dim scrname As String = ""
            Dim sql As String = ""
            If Val(Txtpincode.Text) <> 0 Then
                sql = "select a.Statename,b.DistrictName,c.CityName,d.VillageName,d.Pincode,a.StateCode,b.DistrictCode" & _
                      " ,c.CityCode,d.VillageCode from M_STateDivMaster as a with( NoLock) Inner Join M_DistrictMaster as b " & _
                      " with( NoLock) On a.StateCode=b.StateCode and a.ActivEstatus='Y' and b.ActiveStatus='Y' " & _
                      " Inner Join  M_CityStatemaster as c with( NoLock)  On b.DistrictCode=c.DistrictCode and c.ActivEstatus='Y' " & _
                      " Inner Join M_VillageMaster as d with( NoLock) On c.CityCode=d.CityCode and d.ActiveStatus='Y'" & _
                      " where  d.Pincode='" & Val(txtPinCode.Text) & "'"

                Dim Dt As DataTable
                Dt = New DataTable
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                Dt = objDAL.GetData(sql)
                If Dt.Rows.Count > 0 Then
                    txtStateName.Text = Dt.Rows(0)("StateName")
                    StateCode.Value = Dt.Rows(0)("StateCode")
                    ddlDistrict.Text = Dt.Rows(0)("DistrictName")
                    HDistrictCode.Value = Dt.Rows(0)("DistrictCode")
                    ddlTehsil.Text = Dt.Rows(0)("CityName")
                    HCityCode.Value = Dt.Rows(0)("CityCode")
                    Dim dr As DataRow
                    dr = Dt.NewRow
                    dr("Villagecode") = 381264
                    dr("Villagename") = "Others"
                    Dt.Rows.Add(dr)
                    With DDlArea
                        .DataSource = Dt
                        .DataValueField = "VillageCode"
                        .DataTextField = "VillageName"
                        .DataBind()
                        .SelectedIndex = 0
                    End With
                    DDlArea.Focus()
                    If DDlArea.SelectedValue = 381264 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Javascript", "javascript:FnVillageChange(381264); ", True)
                    End If
                Else
                    txtPinCode.Focus()
                    txtStateName.Text = ""
                    StateCode.Value = 0
                    ddlDistrict.Text = ""
                    HCityCode.Value = 0
                    ddlTehsil.Text = ""
                    HDistrictCode.Value = 0
                    DDlArea.Items.Clear()

                    scrname = "<SCRIPT language='javascript'>alert(' Pincode Not exist.');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Pincode Not exist.');", True)

                End If
                'With DDlPostVillage
                '    .DataSource = Dt
                '    .DataValueField = "VillageCode"
                '    .DataTextField = "VillageName"
                '    .DataBind()
                '    .SelectedIndex = 0
                'End With



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
        For i As Integer = 1890 To 2031
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


        'If (Session("CompID") = "1026") Then
        '    If (GetCardNo() = False) Then

        '        scrname = "alert('Card No. Already Exists. Please try Again.!!');"
        '        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
        '        FillDetail()
        '        Exit Sub
        '    End If
        'End If


        Dim value As Boolean = True

        formNo = GetFormNo()

        Dim strQry, strFld, strFldVal As String
        Dim strDOB, strDOM As String

        Dim cMarried As Char
        cMarried = "N"

        'If (MaritalStatus.SelectedIndex = 0) Then
        '    cMarried = "Y"
        'Else
        '    cMarried = "N"
        'End If
        Dim remark As String = ""
        ' remark = "Update Profile" & Context.Request.UserHostAddress.ToString
        Try


           

            Dim str As String = ""
            Dim Dt1 As DataTable
            Dt1 = New DataTable
            Dim obj As DAL
            Dim Name As String = ""
            Dim mobile As String = ""
            Dim Password As String = ""
            Dim TransPassword As String = ""
            Dim AreaCode As Integer = 0
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            str = "select a.*,isnull(b.IdproofNo,'')as IdproofNo,isnull(b.Idtype,0) as Idtype from M_MemberMaster as a Left Join KycVerify as b on a.Formno=b.Formno where   a.Formno='" & Val(formNo) & "'"
            Dt1 = obj.GetData(str)
            If Dt1.Rows.Count > 0 Then
                strDOB = ddlDOBdt.Text & "-" & ConvertDateToString(Val(ddlDOBmnth.Text)) & "-" & ddlDOBYr.Text
                ' strDOM = ddlMarrDt.Text & "-" & ConvertDateToString(Val(ddlMarrMnth.Text)) & "-" & ddlMarrYr.Text
                'remark = "Update Profile - By " & Session("UserName") & " /"
                Name = Dt1.Rows(0)("MemFirstName")
                TransPassword = Dt1.Rows(0)("EPassw")
                Password = Dt1.Rows(0)("Passw")
                'If ClearInject(Dt1.Rows(0)("MemFirstName")) <> ClearInject(txtFrstNm.Text) Then
                '    Remark = Remark & " First Name, "

                'End If

                'Dim s1 As String = ""
                'If txtPanNo.Text <> "" Then
                '    s1 = "select Count(Panno) as PanNo from M_Membermaster where Panno='" & txtPanNo.Text.Trim & "'"
                '    obj = New DAL
                '    Dim Dt2 As DataTable
                '    Dt2 = New DataTable
                '    Dt2 = obj.GetData(s1)
                '    If Dt2.Rows(0)("Panno") >= 1 Then
                '        btnSubmit.Enabled = True
                '        value = False
                '        scrname = "alert('Your Pan card Number already registered on another Ids');"
                '        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)

                '        Exit Sub
                '    End If
                'End If




                Dim dt As DataTable
                Dim q As String = ""
                Dim i As Integer = 0
                'If DDlArea.SelectedItem.Text = "" Then
                '    AreaCode = 0
                'Else

                If txtPinCode.Text <> "" Then


                    If DDlArea.SelectedItem.Text.ToUpper = "OTHERS" Then
                        If TxtVillage.Text <> "" Then
                            q = "Select * from M_VillageMaster where VillageName='" & TxtVillage.Text.Trim & "' and Activestatus='Y' and Pincode='" & Val(txtPinCode.Text) & "' "
                            obj = New DAL((HttpContext.Current.Session("MlmDatabase" & Session("CompID"))))
                            dt = New DataTable
                            dt = obj.GetData(q)
                            If dt.Rows.Count = 0 Then
                                q = ""
                                q = "insert into M_VillageMaster (VillageName,CityCode,PinCode) " & _
        " Values('" & TxtVillage.Text.ToUpper & "','" & HCityCode.Value & "','" & txtPinCode.Text & "')"


                                i = obj.SaveData(q)
                                If i > 0 Then
                                    q = " select Max(VillageCode)as VillageCode from M_VillageMaster where ActiveStatus='Y'"
                                    dt = obj.GetData(q)
                                    If dt.Rows.Count > 0 Then
                                        AreaCode = dt.Rows(0)("VillageCode")
                                    End If

                                End If
                            Else
                                AreaCode = dt.Rows(0)("VillageCode")

                            End If

                        End If
                    Else
                        AreaCode = DDlArea.SelectedValue
                    End If
                Else
                    AreaCode = 0
                End If
                ' End If
                Dim MemName As String = ""
                MemName = Trim(MemfirstName.Text)
                MemName = MemName.Trim
                Name = Name.Trim
                If ClearInject(Name) <> ClearInject(MemName) Then
                    remark = remark & "MemberName Changed From " & ClearInject(Name) & " to " & ClearInject(MemName) & ","
                End If
                Dim MemFName As String = ""
                MemFName = Trim(txtFNm.Text)
                MemFName = MemFName.Trim
                If ClearInject(Dt1.Rows(0)("MemFName")) <> ClearInject(MemFName) Then
                    remark = remark & " FatherName Changed From " & ClearInject(Dt1.Rows(0)("MemFName")) & " to " & ClearInject(MemFName) & ","
                End If
                If (Dt1.Rows(0)("MemDob")) <> strDOB Then
                    remark = remark & "Dob Changed From " & ClearInject(Dt1.Rows(0)("MemDob")) & " to " & strDOB & ","
                End If
                If ClearInject(Dt1.Rows(0)("Address1")) <> ClearInject(txtAddLn1.Text) Then
                    remark = remark & " Address Changed From " & ClearInject(Dt1.Rows(0)("Address1")) & " to " & ClearInject(txtAddLn1.Text) & " ,"

                End If
                If ClearInject(Dt1.Rows(0)("PinCode")) <> ClearInject(txtPinCode.Text) Then
                    remark = remark & " PinCode Changed From " & ClearInject(Dt1.Rows(0)("PinCode")) & " to " & ClearInject(txtPinCode.Text) & ","
                End If
                If Val(Dt1.Rows(0)("Statecode")) <> Val(StateCode.Value) Then
                    remark = remark & " State Changed From " & Val(Dt1.Rows(0)("Statecode")) & " to " & Val(StateCode.Value) & " ,"
                End If
                If ClearInject((Dt1.Rows(0)("District"))) <> ClearInject(ddlDistrict.Text) Then
                    remark = remark & " District Changed From " & ClearInject((Dt1.Rows(0)("District"))) & " to " & ClearInject(ddlDistrict.Text) & ","
                End If
                If ClearInject(Dt1.Rows(0)("City")) <> ClearInject(ddlTehsil.Text) Then
                    remark = remark & " City Changed From " & ClearInject(Dt1.Rows(0)("City")) & " to " & ClearInject(ddlTehsil.Text) & ","

                End If
                If Val(Dt1.Rows(0)("Areacode")) <> Val(DDlArea.SelectedValue) Then
                    remark = remark & " Area Changed From " & Val(Dt1.Rows(0)("Areacode")) & " to " & Val(DDlArea.SelectedValue) & ","

                End If
                If Val(Dt1.Rows(0)("Idtype")) <> Val(DDLAddressProof.SelectedValue) Then
                    remark = remark & " AddressProof Changed From " & Val(Dt1.Rows(0)("Idtype")) & " to " & Val(DDLAddressProof.SelectedValue) & " ,"
                End If
                If (Dt1.Rows(0)("IdproofNo")) <> (TxtIdProofNo.Text) Then
                    remark = remark & " AddressProofno Changed From " & (Dt1.Rows(0)("IdproofNo")) & " to " & (TxtIdProofNo.Text).Trim & ","
                End If


                If ClearInject(Dt1.Rows(0)("Mobl")) <> ClearInject(txtMobileNo.Text) Then
                    remark = remark & " MobileNo Changed From " & ClearInject(Dt1.Rows(0)("Mobl")) & " to " & ClearInject(txtMobileNo.Text.Trim) & ","
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
                    remark = remark & " AccountType Changed From " & Dt1.Rows(0)("Fax") & " to " & ClearInject(DDLAccountType.SelectedValue) & ","
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






                'remark = remark & " Changed of Idno " & txtMemberId.Text

            End If







            Dim Qry As String = "Insert Into TempMemberMaster Select *,'Update Profile',GetDate(),'U' From M_MemberMaster Where FormNo='" & Val(formNo) & "'"
            Qry = Qry & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId,Hostip)Values" & _
       "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Profile ','Profile Update','" & remark & "',Getdate(),'" & Val(formNo) & "','" & Context.Request.UserHostAddress.ToString & "')"
            Qry = Qry & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(formNo) & "' From KycVerify Where FormNo='" & Val(formNo) & "'"
            objDAL.SaveData(Qry)
            Dim sql As String = "Update m_MemberMaster set Prefix='" & CmbPrefix.SelectedItem.Text & "',MemFirstName='" & MemfirstName.Text.Trim() & "'," & _
            " MemRelation='" & CmbType.SelectedItem.Text & "'," & _
            " MemFName='" & txtFNm.Text.Trim & "',MemDOb='" & strDOB & "',Address1='" & txtAddLn1.Text.ToUpper & "',Pincode='" & txtPinCode.Text & "'" & _
            " ,StateCode='" & Val(StateCode.Value) & "',DistrictCode='" & Val(HDistrictCode.Value) & "',District='" & ddlDistrict.Text.ToUpper & "'," & _
            " Tehsil='" & ddlTehsil.Text.ToUpper & "',City='" & ddlTehsil.Text.ToUpper & "'," & _
            " AreaCode='" & AreaCode & "',CityCode='" & Val(HCityCode.Value) & "'," & _
            " PHN1='" & txtPhNo.Text & "',Mobl='" & txtMobileNo.Text & "',Email='" & TxtEmailID.Text.Trim & "',NomineeName='" & lblNominee.Text & "'," & _
            " Relation='" & lblRelation.Text & "',Fax='" & DDLAccountType.SelectedItem.Text & "',Acno='" & TxtAccountNo.Text & "',BankId='" & CmbBank.SelectedValue & "'," & _
            " BranchName='" & TxtBranchName.Text & "',IFSCode='" & TxtIfsCode.Text & "',PANNo='" & txtPanNo.Text & "',AadharNo3='" & txtGstNo.Text & "',Passw='" & TxtPassword.Text & "',E_MainPassw='" & TxtPassword.Text & "',Epassw='" & TxtTransactionPassword.Text & "', " & _
            " Fld5='" & txtCardNo.Text & "' where Formno= '" & Val(formNo) & "'"
            sql = sql & "Update KycVerify Set Idtype='" & DDLAddressProof.SelectedValue & "',IdProofNo='" & TxtIdProofNo.Text.Trim.ToUpper & "'" & _
            "  where Formno= '" & Val(formNo) & "'"

            ' Dim Qry As String = sql & "Insert Into TempMemberMaster Select *,'Update Address Proof - " & Context.Request.UserHostAddress.ToString & "',GetDate(),'U' From M_MemberMaster Where FormNo='" & Val(Session("FormNo")) & "'"

            'strQry = "Select * From M_MemberMaster Where FormNo='" & Val(formNo) & "'"
            'strFld = "Prefix;MemRelation;MemFirstName;MemFName;MemDOB" & _
            '        ";Address1;Address2;Post" & _
            '        ";City;District;StateCode" & _
            '        ";PinCode;PhN1;Fax;Mobl;EMail" & _
            '        ";PanNo" & _
            '        ";BankId;BranchName;AcNo;IFSCode;NomineeName;Relation;Passw;E_MainPassw;EPassw"

            'strFldVal = CmbPrefix.SelectedItem.Text & ";" & CmbType.SelectedItem.Text & ";" & MemfirstName.Text & ";" & txtFNm.Text & ";" & strDOB & _
            '           ";" & txtAddLn1.Text & ";" & txtAddLn1.Text & ";" & txtAddLn1.Text & _
            '           ";" & ddlTehsil.Text & ";" & ddlDistrict.Text & ";" & 0 & _
            '           ";" & txtPinCode.Text & ";" & txtPhNo.Text & ";" & DDLAccountType.SelectedValue & ";" & Val(txtMobileNo.Text) & ";" & TxtEmailID.Text & _
            '           ";" & txtPanNo.Text & "" & _
            '           ";" & CmbBank.SelectedValue & ";" & TxtBranchName.Text & ";" & TxtAccountNo.Text & ";" & TxtIfsCode.Text & _
            '           ";" & lblNominee.Text & ";" & lblRelation.Text & ";" & TxtPassword.Text & ";" & TxtPassword.Text & ";" & TxtTransactionPassword.Text & ""
            'Dim whr As String = " Where FormNo='" & Val(formNo) & "'"
            Dim a As Integer = objDAL.SaveData(sql)
            If a <> 0 Then

                Dim Str2 As String = "Profile.Aspx"
                If Request("key") IsNot Nothing Then

                    scrname = "alert('Profile Successfully Updated ');window.location.replace('" & Str2 & "','_blank');"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                    FillDetail()
                    btnSubmit.Visible = False
                Else




                    ' sendSMS(MemfirstName.Text, txtMobileNo.Text, TxtPassword.Text, TransPassword, txtMemberId.Text)
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
            'errMsg.InnerText = "Error In Updation"
            'dbGeneral.myMsgBx(e.Message)
            Return
        End Try



    End Sub
    Private Function ClearInject(ByVal StrObj As String) As String
        StrObj = Replace(Replace(Replace(StrObj, ";", ""), "'", ""), "=", "")
        Return StrObj
    End Function


    'Protected Sub MaritalStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MaritalStatus.SelectedIndexChanged
    '    enblDsblCtrl("ms")
    'End Sub
    Private Sub sendSMS(ByVal Username As String, ByVal MobileNo As String, ByVal Password As String, ByVal TransPassword As String, ByVal IdNo As String)

        Dim client As New WebClient
        Dim baseurl As String
        Dim data As Stream
        Dim datet As DateTime = Now
        Dim sms As String = "Dear " & Username & ", Your login details are ID-" & IdNo & "/ Pwd-" & Password & "/Trans Code-" & TransPassword & ", pls visit " & Session("CompWeb") & " for more details."

        ' Dim sms As String = "Dear " & Username & ", OTP for login is " & Otp & " at " & datet & " " & Session("AdminWeb") & "."
        Try
            'baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & MobileNo & "&msg=" & sms & ""
            ' baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & MobileNo & "&SenderId=" & Session("ClientId") & ""
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
            If Trim(TxtAccountNo.Text) = "" Then
                scrname = "alert('Enter Account No.');"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                Exit Sub
            End If

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
            If DDLAccountType.SelectedValue = "0" Then
                scrname = "alert('Enter Account Type.');"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", scrname, True)
                Exit Sub
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
            ' btnShowMemDetail.Visible = True
            divDetailSection.Visible = False
            btnSubmit.Enabled = False
            Btncancel.Enabled = False
            txtMemberId.Enabled = True
        Else
            txtMemberId.Enabled = False
            divDetailSection.Visible = True
            ' btnShowMemDetail.Visible = False
            'divDetailSection.Visible = True
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

    'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btncancel.Click
    '    divDetailSection.Visible = False
    'End Sub

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

    'Protected Sub txtMemberId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberId.TextChanged
    '    FillDetail()
    'End Sub


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
