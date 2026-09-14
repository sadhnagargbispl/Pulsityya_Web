Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class App_UI_Application_Pages_Idverified
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Member / Kyc Verify"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

        
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                If Session("AStatus") = "OK" Then
                    GetSmsTemplate()
                    Filldate()
                    If Request.QueryString.HasKeys Then
                        If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                            txtMemId.Text = Request.QueryString("key")
                            ChkMem.Checked = True
                            BindData(" AND IDNo='" & Request.QueryString("key") & "'")
                        End If
                    Else
                        BindData()

                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Filldate()
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
            dtData = New DataTable
            dtData = objDAL.GetData(Str)
            If dtData.Rows.Count > 0 Then
                txtStartDate.Text = dtData.Rows(0)("CurrentDate")
                txtEndDate.Text = dtData.Rows(0)("CurrentDate")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")
        Try


            'Dim sql As String = "Select Cast(FormNo as varchar) as FormNo,IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,CASE WHEN DtPhotoProof is NULL Then '' ELSE 'View' END AS PhotoStatus,Replace(CONVERT(varchar,DtPhotoProof,106),' ','-') as PhDate,Replace(CONVERT(varchar,DtIdentityProof,106),' ','-') as IdentityDate,CASE WHEN DtSignature is NULL Then '' ELSE 'View' END AS SignStatus,Replace(CONVERT(varchar,DtSignature,106),' ','-') as SignDate From M_MemberMaster Where (DTSignature IS NOT NULL OR DtPhotoProof is not null OR DtIdentityProof is not null) " & Condition & " Order by DtPhotoProof DESC,DTSignature DESC"
            Dim startDate As Date
            Dim endDate As Date
            'If txtStartDate.Text = "" Then
            '    startDate = Session("CompDate")
            'Else
            '    startDate = txtStartDate.Text
            'End If
            'If txtEndDate.Text = "" Then
            '    endDate = Format(Date.Now, "dd-MMM-yyyy")
            'Else
            '    endDate = txtEndDate.Text
            'End If

            'If ChkMem.Checked = True Then
            '    If txtMemId.Text <> "" Then
            '        Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
            '    End If

            'End If

            'If DDlVerify.SelectedValue = "N" And (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
            '    Condition = Condition & " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,A.Doj,106) as DateTime) >='" & txtStartDate.Text & "'  And Cast(Convert(varchar,A.Doj,106) as DateTime) <='" & txtEndDate.Text & "' "
            'ElseIf DDlVerify.SelectedValue <> "S" Then
            '    Condition = Condition & " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime) >='" & txtStartDate.Text & "'  And Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime) <='" & txtEndDate.Text & "' "
            'Else
            '    Condition = Condition & " And Cast(Convert(varchar,A.Doj,106) as Date) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,A.Doj,106) as Date)<= '" & txtEndDate.Text & "' "
            'End If
            'If RbtSearch.SelectedValue <> "A" Then
            '    Condition = Condition & " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'  "
            'End If
            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                startDate = txtStartDate.Text
            End If

            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                endDate = txtEndDate.Text
            End If

            If ChkMem.Checked = True AndAlso txtMemId.Text <> "" Then
                Condition &= " AND IDNo='" & Trim(txtMemId.Text) & "'"
            End If

            If (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
                If DDlVerify.SelectedValue = "N" Then
                    Condition &= " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,A.Doj,106) as DateTime) >='" & txtStartDate.Text & "' And Cast(Convert(varchar,A.Doj,106) as DateTime) <='" & txtEndDate.Text & "' "
                ElseIf DDlVerify.SelectedValue <> "S" Then
                    Condition &= " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime) >='" & txtStartDate.Text & "' And Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime) <='" & txtEndDate.Text & "' "
                Else
                    Condition &= " And Cast(Convert(varchar,A.Doj,106) as Date) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,A.Doj,106) as Date)<= '" & txtEndDate.Text & "' "
                End If
            Else
                ' अगर दोनों date empty हैं और केवल verify filter लगाना है
                If DDlVerify.SelectedValue = "N" Or DDlVerify.SelectedValue <> "S" Then
                    Condition &= " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "' "
                End If
            End If

            If RbtSearch.SelectedValue <> "A" Then
                Condition &= " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'"
            End If
            If txtaadharno.Text <> "" Then
                Condition = Condition & " And c.IdProofNo like '%" & txtaadharno.Text & "%'  "
            End If
            Dim sql As String = " select Cast(a.FormNo as varchar) as FormNo,Replace(CONVERT(varchar,a.Doj,106),' ','-')as Doj," & _
                                " Case when a.Activestatus='Y' then Replace(CONVERT(varchar,a.UpgradeDate,106),' ','-') else '' End as ActivationDate," & _
                                " a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemName,a.mobl,a.Address1,a.City,a.Tehsil,a.District,b.Statename," & _
                                " a.Pincode,a.Fax,cASE WHEN c.BackAddressProof<>'' then c.BackAddressProof else '" & Session("CompWeb") & "/Images/no_photo.jpg' end as BackAdressProof," & _
                                " Case when c.BackAddressProof<>'' then Replace(convert(varchar,BackAddressDate,106),' ','-')else '' end as BackAddressDate, " & _
                                " CASE WHEN c.IsAddrssverified='Y' THEN 'Verified' when c.IsAddrssverified='R' then 'Rejected' Else 'Verification Due' END AS AddrssVerf," & _
                                " case when c.AddrProof<>'' then Replace(CONVERT(varchar,c.AddrProofDate ,106),' ','-') else '' end  as AddressProofDate,d.IdType,c.IdProofNo, " & _
                                " CASE WHEN c.AddrProof <>'' Then 	c.AddrProof ELSE '" & Session("CompWeb") & "/Images/no_photo.jpg' END AddressproofStatus,Case when c.IsAddrssverified='Y' then 'False' else 'True' end as EnableStatus," & _
                                " Case when c.IsAddrssVerified='Y' then Replace(CONVERT(varchar,c.AddrssVerifyDate ,106),' ','-') else '' end  as AddressVerifyDate, Isnull(e.Username,'')VerifyBy," & _
                                " Case when c.isAddrssVerified='R' then c.AddrssRemark else '' end as RejectRemark,Isnull(f.Reason, '')As RejectReason" & _
                                " From M_MemberMaster as a,M_StatedivMaster as b,KycVerify as c Left Join  M_KycReject as f On c.AddressRejectId=f.Kid Left Join M_UserMaster as e On c.AddrssUserId=e.Userid and e.Rowstatus='Y' ,M_IdTypeMaster as d where c.IdType=d.Id and d.ActiveStatus='Y' " & _
                                " and a.Formno=c.Formno and a.statecode=b.statecode and b.Rowstatus='Y' and( c.BackAddressProof<>'' Or c.AddrProof <>'')" & _
                                " " & Condition & " Order by AddressProofdate DESC"

            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            If dtData.Rows.Count > 0 Then
                BtnVerifiy.Enabled = True
                BTnUnVerification.Enabled = True
                BtnExport.Enabled = True
            Else
                BtnVerifiy.Enabled = False
                BTnUnVerification.Enabled = False
                BtnExport.Enabled = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub
    Protected Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim startDate As Date
            Dim endDate As Date
            'If txtStartDate.Text = "" Then
            '    startDate = Session("CompDate")
            'Else
            '    startDate = txtStartDate.Text
            'End If
            'If txtEndDate.Text = "" Then
            '    endDate = Format(Date.Now, "dd-MMM-yyyy")
            'Else
            '    endDate = txtEndDate.Text
            'End If
            'If ChkMem.Checked = True Then
            '    If txtMemId.Text <> "" Then
            '        Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
            '    End If

            'End If
            'If DDlVerify.SelectedValue <> "S" Then
            '    Condition = Condition & " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime) >='" & txtStartDate.Text & "'  And Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime) <='" & txtEndDate.Text & "' "
            'Else
            '    Condition = Condition & " And Cast(Convert(varchar,A.Doj,106) as DateTime) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,A.Doj,106) as DateTime)<= '" & txtEndDate.Text & "' "
            'End If
            'If RbtSearch.SelectedValue <> "A" Then
            '    Condition = Condition & " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'  "
            'End If
            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                startDate = txtStartDate.Text
            End If

            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                endDate = txtEndDate.Text
            End If

            If ChkMem.Checked = True AndAlso txtMemId.Text <> "" Then
                Condition &= " AND IDNo='" & Trim(txtMemId.Text) & "'"
            End If

            If (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
                If DDlVerify.SelectedValue = "N" Then
                    Condition &= " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,A.Doj,106) as DateTime) >='" & txtStartDate.Text & "' And Cast(Convert(varchar,A.Doj,106) as DateTime) <='" & txtEndDate.Text & "' "
                ElseIf DDlVerify.SelectedValue <> "S" Then
                    Condition &= " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime) >='" & txtStartDate.Text & "' And Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime) <='" & txtEndDate.Text & "' "
                Else
                    Condition &= " And Cast(Convert(varchar,A.Doj,106) as Date) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,A.Doj,106) as Date)<= '" & txtEndDate.Text & "' "
                End If
            Else
                ' अगर दोनों date empty हैं और केवल verify filter लगाना है
                If DDlVerify.SelectedValue = "N" Or DDlVerify.SelectedValue <> "S" Then
                    Condition &= " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "' "
                End If
            End If

            If RbtSearch.SelectedValue <> "A" Then
                Condition &= " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'"
            End If
            'Condition = Condition & " And Convert(date, c.AddrProofDate) >= Convert(date, '" & startDate & "') And Convert(date, c.AddrProofDate) <= Convert(date, '" & endDate & "') "
            Dim sql As String = " select a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemberName,Replace(CONVERT(varchar,a.Doj,106),' ','-')as Doj," & _
                                " Case when a.Activestatus='Y' then Replace(CONVERT(varchar,a.UpgradeDate,106),' ','-') else '' End as ActivationDate," & _
                                " a.Address1,a.City,a.Tehsil,a.District,b.Statename," & _
                                " a.Pincode,d.IdType,''''+c.IdProofNo as IdProofNo," & _
                                 " case when c.AddrProof<>'' then Replace(CONVERT(varchar,c.AddrProofDate ,106),' ','-') else '' end  as AddressProofDate, " & _
                                "Case when c.BackAddressProof<>'' then Replace(convert(varchar,BackAddressDate,106),' ','-')else '' end as BackAddressProofDate, " & _
                                " CASE WHEN c.IsAddrssverified='Y' THEN 'Verified' when c.IsAddrssverified='R' then 'Rejected' Else 'Verification Due' END AS AddressVerification," & _
                                " Case when c.IsAddrssVerified='Y' then Replace(CONVERT(varchar,c.AddrssVerifyDate ,106),' ','-') else '' end  as AddressVerifyDate " & _
                                ", Isnull(e.Username,'')VerifyBy," & _
                                " Case when c.isAddrssVerified='R' then c.AddrssRemark else '' end as RejectRemark,Isnull(f.Reason, '')As RejectReason" & _
                                " From M_MemberMaster as a,M_StatedivMaster as b,KycVerify as c Left Join  M_KycReject as f On c.AddressRejectId=f.Kid Left Join M_UserMaster as e On c.AddrssUserId=e.Userid and e.Rowstatus='Y' " & _
                                " ,M_IdTypeMaster as d where c.IdType=d.Id and d.ActiveStatus='Y' and a.Formno=c.Formno and a.statecode=b.statecode and b.Rowstatus='Y' " & _
                                " " & Condition & " and( c.BackAddressProof<>'' Or c.AddrProof <>'') Order by AddressProofdate DESC"

            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("AddressProofKYC.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try

    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
        Dim sw As New System.IO.StringWriter
        Dim htw As System.Web.UI.HtmlTextWriter
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.xls"
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
        Response.Charset = ""
        dg.EnableViewState = False
        htw = New HtmlTextWriter(sw)
        dg.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.End()
    End Sub
    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click


        BindData()
    End Sub

    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
    End Sub

    Protected Sub BtnVerifiy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnVerifiy.Click
        Try

            Dim str As String = ""
            Dim scrname As String
            Dim Condition As String = ""
            Dim lbl As Label
            Dim LblIdNo As Label
            Dim lblmobileNo As Label
            Dim lblmemfirstname As Label
            Dim Chk As CheckBox
            Dim Remark As String = ""
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                LblIdNo = DirectCast(Gvr.FindControl("LblIdno"), Label)
                lblmobileNo = DirectCast(Gvr.FindControl("lblmobileno"), Label)
                lblmemfirstname = DirectCast(Gvr.FindControl("lblmemfirstname"), Label)
                If Chk.Checked = True Then

                    'If RbtVerifi.SelectedValue = "Y" Then
                    Remark = "Address Proof Verify of IdNo:" & LblIdNo.Text & ""
                    str = str & "; Update KycVerify Set IsAddrssverified='Y',AddrssVerifyDate=getdate(),AddrssUserId='" & Val(Session("Userid")) & "',AddrssRemark='' where IsidVerified<>'Y' and  formno='" & lbl.Text & "'"
                    str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "'"
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,ImgPath1,Userid,RectimeStamp,Status)" & _
                        "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'A',b.AddrProof,b.BackAddressProof,'" & Val(Session("Userid")) & "',Getdate(),1 from M_MemberMaster as a,KycVerify as b" & _
                        " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','Address Proof Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                    Dim sms As String = ""
                    If (Session("CompID") = 1041) Then
                        Dim BaseUrl As String
                        'Dear {#var1#} your {#var2#} KYC is verified. Wish you all the best. Regards : Oloo Global 
                        ' sms = "Dear " & Val(lblmemfirstname.Text) & " your " & Val(Session("UserID")) & " KYC is verified. Wish you all the best. Regards : Oloo Global"
                        BaseUrl = Session("AddressVerify").ToString().Replace("{#var1#}", lblmemfirstname.Text).Replace("$MOB$", lblmobileNo.Text)
                        SmsAPI("http://testapi.bisplindia.in/SMS.aspx?", baseurl.Replace("&", "^^"), "SMS")

                        'Else

                        '    sms = "Dear " & TxtMemberName.Text.Trim & ", Your id " & txtMemberId.Text.Trim.ToUpper & " is successfully topup by " & CmbKit.SelectedItem.Text & ". Best of luck, Regards: " & Session("CompName") & ""
                    End If

                    ' sendSMS(sms, lblmobileNo.Text)

                    'End If

                End If
            Next
            Dim i As Integer = 0
            If str <> "" Then
                i = objDAL.UpdateData(str)
            End If
            If i <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert(' Verified successfully. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            Else
                scrname = "<SCRIPT language='javascript'>alert(' Verified unsuccessfully. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)


            End If
            BindData()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnUnVerify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUnVerify.Click
        Try


            Dim str As String = ""
            Dim scrname As String
            Dim Condition As String = ""
            Dim lbl As Label
            Dim Chk As CheckBox
            Dim Remark As String = ""
            Dim LblIdno As Label
            Dim lblmobileNo As Label
            Dim lblmemfirstname As Label

            If TxtARemark.Text = "" Then
                LblARemark.Visible = True
                LblARemark.Text = "Please Enter Remark"
                Exit Sub
            End If

            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                LblIdno = DirectCast(Gvr.FindControl("LblIdno"), Label)
                lblmobileNo = DirectCast(Gvr.FindControl("lblmobileno"), Label)
                lblmemfirstname = DirectCast(Gvr.FindControl("lblmemfirstname"), Label)
                If Chk.Checked = True Then

                    'If RbtVerifi.SelectedValue = "N" Then
                    Remark = "Address Proof UnVerify of IdNo:" & LblIdno.Text & ""
                    str = str & "; Update KycVerify Set IsAddrssverified='R', AddrssVerifyDate=getdate(),AddrssUserid='" & Val(Session("Userid")) & "', AddrssRemark=N'" & TxtARemark.Text & "',AddressRejectId='" & DDlREason.SelectedValue & "' " & _
                    " where IsAddrssverified<>'R' And  formno='" & lbl.Text & "'"
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,ImgPath1,Userid,RectimeStamp,Status,Remark,RejectId)" & _
    "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'A',b.AddrProof,b.BackAddressProof,'" & Val(Session("Userid")) & "',Getdate(),2,'" & TxtARemark.Text.Trim & "','" & DDlREason.SelectedValue & "' from M_MemberMaster as a,KycVerify as b" & _
    " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "'"
                    str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','Address Proof UnVerify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                    Dim BaseUrl As String
                    'Dear {#var1#}, your Address KYC is rejected. Login in your account to know more {#var2#}. Regards : Oloo Global
                    'Dear {#var1#} your {#var2#} KYC is verified. Wish you all the best. Regards : Oloo Global 
                    ' sms = "Dear " & Val(lblmemfirstname.Text) & " your " & Val(Session("UserID")) & " KYC is verified. Wish you all the best. Regards : Oloo Global"
                    BaseUrl = Session("AddressRejected").ToString().Replace("{#var1#}", lblmemfirstname.Text).Replace("{#var2#}", Session("CompWeb")).Replace("$MOB$", lblmobileNo.Text)
                    SmsAPI("http://testapi.bisplindia.in/SMS.aspx?", BaseUrl.Replace("&", "^^"), "SMS")


                    'End If
                End If
                'End If
            Next

            Dim i As Integer = 0
            If str <> "" Then
                i = objDAL.UpdateData(str)
            End If
            If i <> 0 Then
                'Dear {#var1#}, your {#var2#} KYC is rejected. Login in your account to know more {#var3#}. Regards : Oloo Global
             
                'sendSMS(sms, Val(lblmobileNo.text))
                scrname = "<SCRIPT language='javascript'>alert(' UnVerified successfully. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            Else
                scrname = "<SCRIPT language='javascript'>alert(' UnVerified unsuccessfully. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)


            End If
            DivRemark.Visible = False
            TxtARemark.Text = ""
            BindData()
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub BTnUnVerification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTnUnVerification.Click
        DivRemark.Visible = True
        BtnVerifiy.Enabled = False
        BTnUnVerification.Enabled = False
        FillDetail()
    End Sub
    Protected Sub FillDetail()
        Try


            Dim s As String = ""
            s = "Select * from M_KycReject where activeStatus='Y'"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Dt As DataTable
            Dt = New DataTable
            Dt = objDAL.GetData(s)
            If Dt.Rows.Count > 0 Then
                DDlREason.DataValueField = "kId"
                DDlREason.DataTextField = "reason"
                DDlREason.DataSource = Dt
                DDlREason.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    
    Private Function SmsAPI(ByVal apiURL As String, ByVal SMSURL As String, ByVal action As String)


        Dim fUrl As String = apiURL & "url=" & SMSURL & "&action=" & action
        Dim sResponseFromServer As String = String.Empty
        Try
            Dim tRequest As WebRequest
            Dim dataStream As Stream
            tRequest = WebRequest.Create(fUrl)
            Dim tResponse As WebResponse = tRequest.GetResponse()
            dataStream = tResponse.GetResponseStream()
            Dim tReader As StreamReader = New StreamReader(dataStream)
            sResponseFromServer = tReader.ReadToEnd()

        Catch Ex As Exception
        End Try
    End Function
    Private Sub GetSmsTemplate()
        Try
            Dim dtaddressverify As DataTable = New DataTable
            Dim dtaddress As DataTable = New DataTable
            Dim ds As DataSet = New DataSet
            Dim str As String = " exec Sp_GetSmsTemplate '" & HttpContext.Current.Session("CompID") & "' "
            ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
            dtaddressverify = ds.Tables(5)
            If (dtaddressverify.Rows.Count > 0) Then
                Session("AddressVerify") = dtaddressverify.Rows(0)("Addressverify")
            Else
                Session("AddressVerify") = ""
            End If
            dtaddress = ds.Tables(8)
            If (dtaddressverify.Rows.Count > 0) Then
                Session("AddressRejected") = dtaddress.Rows(0)("Addressreject")
            Else
                Session("AddressRejected") = ""
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class




