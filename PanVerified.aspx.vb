

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class App_UI_Application_Pages_PanVerified
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
                    If (Session("CompID") = "1007") Then
                        panlink.Visible = True
                    End If
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
    Private Sub GetSmsTemplate()
        Try
            Dim dtaddressverify As DataTable = New DataTable
            Dim dtaddress As DataTable = New DataTable
            Dim ds As DataSet = New DataSet
            Dim str As String = " exec Sp_GetSmsTemplate '" & HttpContext.Current.Session("CompID") & "' "
            ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
            dtaddressverify = ds.Tables(7)
            If (dtaddressverify.Rows.Count > 0) Then
                Session("PANVerify") = dtaddressverify.Rows(0)("PANverify")
            Else
                Session("PANVerify") = ""
            End If
            dtaddress = ds.Tables(10)
            If (dtaddressverify.Rows.Count > 0) Then
                Session("PanRejected") = dtaddress.Rows(0)("Panreject")
            Else
                Session("PanRejected") = ""
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
    Protected Sub ApproveData(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        Dim LblFormno As New Label
        Dim sql As String = ""
        LblFormno.Text = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        sql = "Select Cast(c.FormNo as varchar) as FormNo, " & _
                               " CASE WHEN c.PanImg <>'' Then c.PanImg ELSE '' END PanproofStatus," & _
                               "case when c.IsPanVerified='Y' then 'False' else 'True' end as EnableStatus," & _
                               " Case when c.IsPanverified='N' then '' else Replace(CONVERT(varchar,c.PanVerifyDate ,106),' ','-') end as PanVerifyDate, " & _
      " Case when c.IsPanverified='N' then '' else c.PanRemarks end as VerifyRemark " & _
     " From KycVerify as c  " & _
     " Where 1=1 and c.PanImg<>''  and c.Formno='" & LblFormno.Text & "' Order by c.PANImgDate DESC"
        dtData = New DataTable
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dtData = objDAL.GetData(sql)
        If dtData.Rows.Count > 0 Then
            ImgPan.ImageUrl = dtData.Rows(0)("PanProofStatus")
        End If
        'lblID.Text = DirectCast(GVRw.FindControl("LblID"), Label).Text
        'TxtIDNo.Text = DirectCast(GVRw.FindControl("LblIDNo"), Label).Text
        'TxtName.Text = DirectCast(GVRw.FindControl("LblPayeeName"), Label).Text
        'TxtAmount.Text = DirectCast(GVRw.FindControl("LblAmount"), Label).Text

    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")
        Try
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

            ''Dim sql As String = "Select Cast(FormNo as varchar) as FormNo,IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,CASE WHEN DtPhotoProof is NULL Then '' ELSE 'View' END AS PhotoStatus,Replace(CONVERT(varchar,DtPhotoProof,106),' ','-') as PhDate,Replace(CONVERT(varchar,DtIdentityProof,106),' ','-') as IdentityDate,CASE WHEN DtSignature is NULL Then '' ELSE 'View' END AS SignStatus,Replace(CONVERT(varchar,DtSignature,106),' ','-') as SignDate From M_MemberMaster Where (DTSignature IS NOT NULL OR DtPhotoProof is not null OR DtIdentityProof is not null) " & Condition & " Order by DtPhotoProof DESC,DTSignature DESC"
            'If ChkMem.Checked = True Then
            '    If txtMemId.Text <> "" Then
            '        Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
            '    End If

            'End If
            'If DDlVerify.SelectedValue = "N" Then
            '    Condition = Condition & " And c.IsPanVerified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,A.Doj,106) as DateTime) >='" & txtStartDate.Text & "'  And Cast(Convert(varchar,A.Doj,106) as DateTime) <='" & txtEndDate.Text & "' "
            'ElseIf DDlVerify.SelectedValue <> "S" Then
            '    Condition = Condition & " And c.IsPanVerified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,c.PANImgDate,106) as DateTime) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,c.PANImgDate,106) as DateTime) <='" & txtEndDate.Text & "'   "
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
                    Condition &= " And c.IsPanVerified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,A.Doj,106) as DateTime) >='" & txtStartDate.Text & "' And Cast(Convert(varchar,A.Doj,106) as DateTime) <='" & txtEndDate.Text & "' "
                ElseIf DDlVerify.SelectedValue <> "S" Then
                    Condition &= " And c.IsPanVerified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,c.PANImgDate,106) as DateTime) >='" & txtStartDate.Text & "' And Cast(Convert(varchar,c.PANImgDate,106) as DateTime) <='" & txtEndDate.Text & "' "
                Else
                    Condition &= " And Cast(Convert(varchar,A.Doj,106) as Date) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,A.Doj,106) as Date)<= '" & txtEndDate.Text & "' "
                End If
            Else
                ' अगर दोनों date empty हैं और केवल verify filter लगाना है
                If DDlVerify.SelectedValue = "N" Or DDlVerify.SelectedValue <> "S" Then
                    Condition &= " And c.IsPanVerified='" & DDlVerify.SelectedValue & "' "
                End If
            End If
            If RbtSearch.SelectedValue <> "A" Then
                Condition = Condition & " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'  "
            End If
            If txtpanno.Text <> "" Then
                Condition = Condition & " And Panno like '%" & txtpanno.Text & "%'  "
            End If
            'Condition = Condition & " And Convert(date, c.PANImgDate) >= Convert(date, '" & startDate & "') And Convert(date, c.PANImgDate) <= Convert(date, '" & endDate & "') "

            'Dim sql As String = "Exec sp_GetBindData "

            Dim sql As String = "Select Cast(a.FormNo as varchar) as FormNo,Replace(CONVERT(varchar,Doj,106),' ','-')as Doj," & _
            " Case when A.Activestatus='Y' then Replace(CONVERT(varchar,UpgradeDate,106),' ','-') Else '' End as ActivationDate," & _
            " IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,Panno,a.Mobl,c.IdProofNo as AadharNo, " & _
            " CASE WHEN c.IsPanVerified='Y' THEN 'Verified' when c.IsPanVerified='R' then 'Rejected' Else 'Verification Due' END AS PanVerf," & _
            " case when c.PanImg<>'' then Replace(CONVERT(varchar,c.PANImgDate ,106),' ','-') else '' end  as PanProofDate, " & _
            " CASE WHEN c.PanImg <>'' Then c.PanImg ELSE '' END PanproofStatus," & _
            "case when c.IsPanVerified='Y' then 'False' else 'True' end as EnableStatus," & _
            " Case when c.IsPanverified='N' then '' else Replace(CONVERT(varchar,c.PanVerifyDate ,106),' ','-') end as PanVerifyDate, " & _
            " Case when c.IsPanverified='N' then '' else c.PanRemarks end as VerifyRemark ,Isnull(e.UserName,' ')As VerifyBy,Isnull(f.Reason, '')As RejectReason" & _
            " From M_MemberMaster as a Inner Join KycVerify as c On a.Formno=c.Formno  Left Join  M_KycReject as f On c.PanRejectId=f.Kid" & _
            " Left Join M_Usermaster as e On c.PanUserid=e.UserId and e.RowStatus='Y' " & _
            " Where 1=1 and c.PanImg<>''   " & Condition & " Order by c.PANImgDate DESC"

            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            If dtData.Rows.Count > 0 Then
                BTnUnVerification.Enabled = True
                BtnVerifiy.Enabled = True
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

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click


        BindData()
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
            '    Condition = Condition & " And c.IsPanVerified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,c.PANImgDate,106) as DateTime) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,c.PANImgDate,106) as DateTime) <='" & txtEndDate.Text & "'   "
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
                    Condition &= " And c.IsPanVerified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,A.Doj,106) as DateTime) >='" & txtStartDate.Text & "' And Cast(Convert(varchar,A.Doj,106) as DateTime) <='" & txtEndDate.Text & "' "
                ElseIf DDlVerify.SelectedValue <> "S" Then
                    Condition &= " And c.IsPanVerified='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,c.PANImgDate,106) as DateTime) >='" & txtStartDate.Text & "' And Cast(Convert(varchar,c.PANImgDate,106) as DateTime) <='" & txtEndDate.Text & "' "
                Else
                    Condition &= " And Cast(Convert(varchar,A.Doj,106) as Date) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,A.Doj,106) as Date)<= '" & txtEndDate.Text & "' "
                End If
            Else
                ' अगर दोनों date empty हैं और केवल verify filter लगाना है
                If DDlVerify.SelectedValue = "N" Or DDlVerify.SelectedValue <> "S" Then
                    Condition &= " And c.IsPanVerified='" & DDlVerify.SelectedValue & "' "
                End If
            End If
            If RbtSearch.SelectedValue <> "A" Then
                Condition = Condition & " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'  "
            End If
            'Condition = Condition & " And Convert(date, c.PANImgDate) >= Convert(date, '" & startDate & "') And Convert(date, c.PANImgDate) <= Convert(date, '" & endDate & "') "
            Dim sql As String = " Select a.IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemberName,Replace(CONVERT(varchar,Doj,106),' ','-')as Doj," & _
                                " Case when a.Activestatus='Y' then Replace(CONVERT(varchar,UpgradeDate,106),' ','-') Else '' End as ActivationDate," & _
                                " Panno,''''+c.IdProofNo as AadharNo," & _
                                " CASE WHEN c.IsPanVerified='Y' THEN 'Verified' when c.IsPanVerified='R' then 'Rejected' Else 'Verification Due' END AS PanVerf," & _
                                " case when c.PanImg<>'' then Replace(CONVERT(varchar,c.PANImgDate ,106),' ','-') else '' end  as PanProofDate, " & _
                                " Case when c.IsPanverified='N' then '' else Replace(CONVERT(varchar,c.PanVerifyDate ,106),' ','-') end as PanVerifyDate, " & _
                                " Case when c.IsPanverified='N' then '' else c.PanRemarks end as RejectRemark,Isnull(f.Reason, '')As RejectReason,Isnull(e.UserName,' ')As VerifyBy " & _
                                " From M_MemberMaster as a Inner Join KycVerify as c On a.Formno=c.Formno Left Join  M_KycReject as f On c.PanRejectId=f.Kid " & _
                                " Left Join M_Usermaster as e On c.PanUserid=e.UserId and e.RowStatus='Y' Where 1=1 and c.PanImg<>''  " & _
                                " " & Condition & " Order by c.PANImgDate DESC"
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("PanCardKYC.xls", dg)

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
            Dim lblpanno As Label

            Dim Chk As CheckBox
            Dim Remark As String = ""
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                LblIdNo = DirectCast(Gvr.FindControl("LblIdno"), Label)
                lblmobileNo = DirectCast(Gvr.FindControl("lblmobileno"), Label)
                lblpanno = DirectCast(Gvr.FindControl("lblpanno"), Label)

                If Chk.Checked = True Then

                    'If RbtVerifi.SelectedValue = "Y" Then
                    Remark = "PanCard Verify of IdNo:" & LblIdNo.Text & ""
                    str = str & "; Update KycVerify Set IsPanVerified='Y',PanVerifyDate=getdate(),PanRemarks='',PanUserid='" & Val(Session("Userid")) & "' where formno='" & lbl.Text & "' AND IsPanVerified<>'Y'"
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status)" & _
                      "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'P',b.PanImg,'" & Val(Session("Userid")) & "',Getdate(),1 from M_MemberMaster as a,KycVerify as b" & _
                      " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "'"
                    str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','Pan Card Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"
                    If (Session("CompID") = 1041) Then
                        Dim BaseUrl As String
                        'Dear {#var1#} your {#var2#} KYC is verified. Wish you all the best. Regards : Oloo Global 
                        ' sms = "Dear " & Val(lblmemfirstname.Text) & " your " & Val(Session("UserID")) & " KYC is verified. Wish you all the best. Regards : Oloo Global"
                        BaseUrl = Session("PANVerify").ToString().Replace("{#var1#}", lblmemfirstname.Text).Replace("$MOB$", lblmobileNo.Text)
                        SmsAPI("http://testapi.bisplindia.in/SMS.aspx?", BaseUrl.Replace("&", "^^"), "SMS")

                        'Else

                        '    sms = "Dear " & TxtMemberName.Text.Trim & ", Your id " & txtMemberId.Text.Trim.ToUpper & " is successfully topup by " & CmbKit.SelectedItem.Text & ". Best of luck, Regards: " & Session("CompName") & ""
                    End If


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
                lblmemfirstname = DirectCast(Gvr.FindControl("lblmemfirstname"), Label)

                If Chk.Checked = True Then

                    ' If RbtVerifi.SelectedValue = "N" Then
                    Remark = "Pancard UnVerify of IdNo:" & LblIdno.Text & ""
                    str = str & "; Update KycVerify Set IsPanVerified='R',PanVerifyDate=getdate(),PanUserid='" & Val(Session("Userid")) & "',PanRemarks=N'" & TxtARemark.Text & "',PanRejectId='" & DDlREason.SelectedValue & "' where formno='" & lbl.Text & "' AND IsPanVerified<>'R'"
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status,Remark,RejectId)" & _
   "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'P',b.PanImg,'" & Val(Session("Userid")) & "',Getdate(),2,'" & TxtARemark.Text.Trim & "','" & DDlREason.SelectedValue & "' from M_MemberMaster as a,KycVerify as b" & _
   " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "'"
                    str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','Pancard UnVerify','" & Remark & "',Getdate(),'" & lbl.Text & "')"
                    Dim BaseUrl As String = "" 'https://alerts.prioritysms.com/api/web2sms.php?workingkey=A0d0297a35a3bb97e8510520a91957689&sender=OlOOOO&to=$MOB$&entity_id=1201161710759538574&template_id=1207161794691905997&message=Dear {#var1#}, your Bank KYC is rejected. Login in your account to know more {#var2#}. Regards : Oloo Global
                    'Dear {#var1#}, your Address KYC is rejected. Login in your account to know more {#var2#}. Regards : Oloo Global
                    'Dear {#var1#} your {#var2#} KYC is verified. Wish you all the best. Regards : Oloo Global 
                    ' sms = "Dear " & Val(lblmemfirstname.Text) & " your " & Val(Session("UserID")) & " KYC is verified. Wish you all the best. Regards : Oloo Global"
                    BaseUrl = Session("PanRejected").ToString().Replace("{#var1#}", lblmemfirstname.Text).Replace("{#var2#}", Session("CompWeb")).Replace("$MOB$", lblmobileNo.Text)
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
End Class



