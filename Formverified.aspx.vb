Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_Formverified
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

            Dim startDate As Date
            Dim endDate As Date
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
            Dim sql As String = ""
            If DDlVerify.SelectedValue = "N" Then
                sql = " select a.Formno,a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemName,a.mobl,FrontSideForm,BackSideForm,"
                sql &= " Isnull(Convert(Varchar,b.VerifyDate),'') as VerifyDate ,' ' As VerifyBy,"
                sql &= " (Case When b.ActiveStatus = 'N' Then 'Verification Due' "
                sql &= " When b.ActiveStatus = 'R' Then 'Rejected' "
                sql &= " When b.ActiveStatus = 'A' Then 'Approved'  End ) As  Status,Case when B.activestatus='N' then '' else B.Remark end as VerifyRemark,Isnull(b.AdminRemark, '')As RejectReason ,"
                sql &= " Replace(Convert(Varchar,b.rectimestamp,106),' ','-') As rectimestamp"
                sql &= "  From M_MemberMaster a,M_FormUpload b "
                sql &= " Where a.Formno = b.Formno "
                If ChkMem.Checked = True Then

                    If txtMemId.Text <> "" Then
                        sql &= " And a.IDNo = '" & txtMemId.Text & "'"
                    End If
                End If
                If (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
                    If DDlVerify.SelectedValue = "N" Then
                        sql &= " And  b.ActiveStatus='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,b.rectimestamp,106) as DateTime) >='" & txtStartDate.Text & "'  And Cast(Convert(varchar,b.rectimestamp,106) as DateTime) <='" & txtEndDate.Text & "' "
                    ElseIf DDlVerify.SelectedValue <> "S" Then
                        sql &= " And b.ActiveStatus = '" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,b.VerifyDate,106) as Date) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar, b.VerifyDate,106) as Date) <='" & txtEndDate.Text & "'"
                    Else
                        sql &= " And Cast(Convert(varchar,rectimestamp,106) as DateTime) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,rectimestamp,106) as DateTime)<= '" & txtEndDate.Text & "' "
                    End If
                Else
                    If DDlVerify.SelectedValue = "N" Or DDlVerify.SelectedValue <> "S" Then
                        sql &= " And b.ActiveStatus='" & DDlVerify.SelectedValue & "' "
                    End If
                End If
                
                
                'sql &= " And Convert(date, b.VerifyDate) >= Convert(date, '" & startDate & "') And Convert(date, b.VerifyDate) <= Convert(date, '" & endDate & "') "

            Else
                sql = " select * from ("
                sql &= " select a.Formno,a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemName,a.mobl,FrontSideForm,BackSideForm,"
                sql &= " Isnull(Convert(Varchar,b.VerifyDate),'') as VerifyDate ,' ' As VerifyBy,"
                sql &= " (Case When b.ActiveStatus = 'N' Then 'Verification Due' "
                sql &= " When b.ActiveStatus = 'R' Then 'Rejected' "
                sql &= " When b.ActiveStatus = 'A' Then 'Approved'  End ) As  Status,Case when B.activestatus='N' then '' else B.Remark end as VerifyRemark,Isnull(b.AdminRemark, '')As RejectReason ,"
                sql &= " Replace(Convert(Varchar,b.rectimestamp,106),' ','-') As rectimestamp"
                sql &= "  From M_MemberMaster a,M_FormUpload b "
                sql &= " Where a.Formno = b.Formno and b.ActiveStatus ='N'"
                sql &= " Union All "
                sql &= " select a.Formno,a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemName,a.mobl,FrontSideForm,BackSideForm,"
                sql &= " Isnull(Convert(Varchar,b.VerifyDate),'') as VerifyDate ,Isnull(e.UserName,' ')As VerifyBy,"
                sql &= " (Case When b.ActiveStatus = 'N' Then 'Pending' "
                sql &= " When b.ActiveStatus = 'R' Then 'Rejected' "
                sql &= " When b.ActiveStatus = 'A' Then 'Approved'  End ) As  Status,Case when B.activestatus='N' then '' else B.Remark end as VerifyRemark,Isnull(b.AdminRemark, '')As RejectReason ,"
                sql &= " Replace(Convert(Varchar,b.rectimestamp,106),' ','-') As rectimestamp"
                sql &= "  From M_MemberMaster a,M_FormUpload b ,M_Usermaster as e "
                sql &= " Where a.Formno = b.Formno AND b.VerifyBy=e.UserId and e.RowStatus='Y' "
                sql &= " ) as Temp "
                If ChkMem.Checked = True Then

                    If txtMemId.Text <> "" Then
                        'sql &= " And a.IDNo = '" & txtMemId.Text & "'"
                        sql &= " Where IDNo = '" & txtMemId.Text & "' "
                    End If
                    If DDlVerify.SelectedValue <> "S" Then
                        If DDlVerify.SelectedValue = "A" Then
                            sql &= " And Status = 'Approved'"
                        End If
                        If DDlVerify.SelectedValue = "N" Then
                            sql &= " And Status = 'Verification Due'"
                        End If
                        If DDlVerify.SelectedValue = "R" Then
                            sql &= " And Status = 'Rejected'"
                        End If
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
                        If (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
                            sql &= " And Cast(Convert(varchar,VerifyDate,106) as Date) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar, VerifyDate,106) as Date) <='" & txtEndDate.Text & "' "
                        End If
                        
                        'ElseIf DDlVerify.SelectedValue = "S" Then
                        '    ''sql &= " And Status = 'Pending' And VerifyBy=''"
                    Else
                        'sql &= " And VerifyBy=''"
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
                        If (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
                            sql &= " And Cast(Convert(varchar,rectimestamp,106) as DateTime) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,rectimestamp,106) as DateTime)<= '" & txtEndDate.Text & "'  "

                        End If
                       '  sql &= " And VerifyBy<>''"
                    End If
                Else
                    If DDlVerify.SelectedValue <> "S" Then
                        If DDlVerify.SelectedValue = "A" Then
                            sql &= " Where Status = 'Approved'"
                        End If
                        If DDlVerify.SelectedValue = "N" Then
                            sql &= " Where Status = 'Verification Due'"
                        End If
                        If DDlVerify.SelectedValue = "R" Then
                            sql &= " Where Status = 'Rejected'"
                        End If
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
                        'sql &= " And Cast(Convert(varchar,VerifyDate,106) as DateTime) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar, VerifyDate,106) as DateTime) <='" & txtEndDate.Text & "' "
                        sql &= " And Cast(Convert(varchar,VerifyDate,106) as Date) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar, VerifyDate,106) as Date) <='" & txtEndDate.Text & "' "

                        'ElseIf DDlVerify.SelectedValue = "S" Then
                        '    ''sql &= " And Status = 'Pending' And VerifyBy=''"
                    Else
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
                        sql &= " where Cast(Convert(varchar,VerifyDate,106) as Date) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar, VerifyDate,106) as Date) <='" & txtEndDate.Text & "' "

                    End If
                    
                End If

            End If

            'Isnull(b.VerifyBy,'') as VerifyBy ,


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
            Dim sql As String = ""
            Dim startDate As Date
            Dim endDate As Date
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
           

            sql = " select a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemName,a.mobl,FrontSideForm,BackSideForm,"
            sql &= " Isnull(Convert(Varchar,b.VerifyDate),'') as VerifyDate ,Isnull(b.VerifyBy,'') as VerifyBy ,"
            sql &= " (Case When b.ActiveStatus = 'N' Then 'Pending' "
            sql &= " When b.ActiveStatus = 'R' Then 'Rejected' "
            sql &= " When b.ActiveStatus = 'A' Then 'Approved'  End ) As  Status,Case when B.activestatus='N' then '' else B.Remark end as VerifyRemark,Isnull(b.AdminRemark, '')As RejectReason ,"
            sql &= " Replace(Convert(Varchar,b.rectimestamp,106),' ','-') As rectimestamp"
            sql &= "  From M_MemberMaster a,M_FormUpload b  "
            sql &= " Where a.Formno = b.Formno"
            If ChkMem.Checked = True Then

                If txtMemId.Text <> "" Then
                    sql &= " And a.IDNo = '" & txtMemId.Text & "'"
                End If
            End If
            If (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
                If DDlVerify.SelectedValue = "N" Then
                    sql &= " And  b.ActiveStatus='" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,b.rectimestamp,106) as DateTime) >='" & txtStartDate.Text & "'  And Cast(Convert(varchar,b.rectimestamp,106) as DateTime) <='" & txtEndDate.Text & "' "
                ElseIf DDlVerify.SelectedValue <> "S" Then
                    sql &= " And b.ActiveStatus = '" & DDlVerify.SelectedValue & "' And Cast(Convert(varchar,b.VerifyDate,106) as Date) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar, b.VerifyDate,106) as Date) <='" & txtEndDate.Text & "'"
                Else
                    sql &= " And Cast(Convert(varchar,rectimestamp,106) as DateTime) >= '" & txtStartDate.Text & "' And Cast(Convert(varchar,rectimestamp,106) as DateTime)<= '" & txtEndDate.Text & "' "
                End If
            Else
                If DDlVerify.SelectedValue = "N" Or DDlVerify.SelectedValue <> "S" Then
                    sql &= " And b.ActiveStatus='" & DDlVerify.SelectedValue & "' "
                End If
            End If



            Dim dg As New DataGrid
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("Forms.xls", dg)

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
            Dim Chk As CheckBox
            Dim Remark As String = ""
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                LblIdNo = DirectCast(Gvr.FindControl("LblIdno"), Label)
                If Chk.Checked = True Then

                    'If RbtVerifi.SelectedValue = "Y" Then
                    Remark = "Forms Verify of IdNo:" & LblIdNo.Text & ""
                    '                str = str & "; Update KycVerify Set IsAddrssverified='Y',AddrssVerifyDate=getdate(),AddrssUserId='" & Val(Session("Userid")) & "' where IsidVerified<>'Y' and  formno='" & lbl.Text & "'"
                    '                str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "'"
                    '                str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,ImgPath1,Userid,RectimeStamp,Status)" & _
                    '                    "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'A',b.AddrProof,b.BackAddressProof,'" & Val(Session("Userid")) & "',Getdate(),1 from M_MemberMaster as a,KycVerify as b" & _
                    '                    " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "'"
                    '                str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                    '"('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','Address Proof Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                    str = str & ";  Update M_FormUpload Set ActiveStatus = 'A', AdminRemark= '" & Remark & "',VerifyDate = getdate(),VerifyBy ='" & Val(Session("Userid")) & "' Where Formno = '" & lbl.Text & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
   "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Form Verify ','Form Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

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


            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                LblIdno = DirectCast(Gvr.FindControl("LblIdno"), Label)
                If Chk.Checked = True Then

                    Remark = "Forms UnVerify of IdNo:" & LblIdno.Text & ""
                    str = str & ";  Update M_FormUpload Set ActiveStatus = 'R', AdminRemark= N'" & TxtARemark.Text & "',VerifyDate = getdate(),VerifyBy ='" & Val(Session("Userid")) & "' Where Formno = '" & lbl.Text & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
   "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Form Verify ','Form UnVerify','" & Remark & "',Getdate(),'" & lbl.Text & "')"
                End If
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




