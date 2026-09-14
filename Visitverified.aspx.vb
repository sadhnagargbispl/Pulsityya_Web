Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_Visitverified
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral
    Dim VId As Integer

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Member / Visiting Verify"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                If Session("AStatus") = "OK" Then
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

    Public Sub BindData(Optional ByVal Condition As String = "")
        Try


            'Dim sql As String = ""
            'sql = " select a.VId,a.IDNo,a.Name,a.city,a.State,b.Id,"
            'sql &= "(select count(*)as cnt from TrnVisitingMaster b where a.Vid = b.Vid) as TotalMember,b.Name As MemName,b.Age,b.Pincode,b.CityName,b.stateName,Isnull(Convert(Varchar,b.VerifyDate),'') as VerifyDate ,Isnull(b.VerifyBy,'') as VerifyBy ,"
            'sql &= " (Case When b.ActiveStatus = 'N' Then 'Pending' "
            'sql &= " When b.ActiveStatus = 'R' Then 'Rejected' "
            'sql &= " When b.ActiveStatus = 'Y' Then 'Approved'  End ) As  Status,"
            'sql &= " Replace(Convert(Varchar,b.ReqRectimestamp,106),' ','-') As rectimestamp"
            'sql &= "  From M_VisitingMaster as a,TrnVisitingMaster as b  "
            'sql &= " Where a.VId = b.VId "

            Dim sql As String = "select a.VId,a.IDNo,a.Name,Replace(Convert(Varchar,a.ReqRectimestamp,106),' ','-') As rectimestamp,(select count(*)as cnt from TrnVisitingMaster as b where a.Vid = b.Vid) as TotalMember"
            sql &= " From M_VisitingMaster as a"
            
            If ChkMem.Checked = True Then

                If txtMemId.Text <> "" Then
                    sql &= " Where a.IDNo = '" & txtMemId.Text & "'"
                End If
            End If
            'If DDlVerify.SelectedValue <> "S" Then
            '    If (DDlVerify.SelectedValue <> "Y") Then
            '        sql &= " And b.ActiveStatus = '" & DDlVerify.SelectedValue & "'"
            '    Else
            '        sql &= " And b.ActiveStatus = '" & DDlVerify.SelectedValue & "'"
            '    End If
            'End If

            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            If dtData.Rows.Count > 0 Then
                BtnVerifiy.Enabled = True
                BTnUnVerification.Enabled = True
                'BtnExport.Enabled = True
            Else
                BtnVerifiy.Enabled = False
                BTnUnVerification.Enabled = False
                'BtnExport.Enabled = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub
    'Protected Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExport.Click
    '    Try
    '        Dim dtTemp As New DataTable

    '        Dim sql As String = ""
    '        sql = " select a.IDNo,a.Name,a.State,a.city,"
    '        sql &= "b.Name As [Member Name],b.Age,b.Pincode,b.CityName,b.stateName, Isnull(Convert(Varchar,b.VerifyDate),'') as VerifyDate ,"
    '        sql &= " (Case When b.ActiveStatus = 'N' Then 'Pending' "
    '        sql &= " When b.ActiveStatus = 'R' Then 'Rejected' "
    '        sql &= " When b.ActiveStatus = 'Y' Then 'Approved'  End ) As  Status"
    '        'sql &= " Replace(Convert(Varchar,b.ReqRectimestamp,106),' ','-') As rectimestamp"
    '        sql &= "  From M_VisitingMaster as a,trnvisitingMaster as b  "
    '        sql &= " Where a.Vid = b.Vid"
    '        If ChkMem.Checked = True Then

    '            If txtMemId.Text <> "" Then
    '                sql &= " And a.IDNo = '" & txtMemId.Text & "'"
    '            End If
    '        End If
    '        If DDlVerify.SelectedValue <> "S" Then
    '            sql &= " And b.ActiveStatus = '" & DDlVerify.SelectedValue & "'"
    '        End If




    '        Dim dg As New DataGrid
    '        dtTemp = New DataTable
    '        dtTemp = objDAL.GetData(sql)

    '        dg.DataSource = dtTemp
    '        dg.DataBind()

    '        ExportToExcel("VisitList.xls", dg)

    '    Catch ex As Exception
    '        Response.Write(ex.Message & "Error In Exporting File")
    '    End Try

    'End Sub
    'Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
    '    Dim sw As New System.IO.StringWriter
    '    Dim htw As System.Web.UI.HtmlTextWriter
    '    Response.Clear()
    '    Response.Buffer = True
    '    Response.ContentType = "application/vnd.xls"
    '    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
    '    Response.Charset = ""
    '    dg.EnableViewState = False
    '    htw = New HtmlTextWriter(sw)
    '    dg.RenderControl(htw)
    '    Response.Write(sw.ToString())
    '    Response.End()
    'End Sub
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
            'Dim LblIdVerify As Label
            Dim Chk As CheckBox
            Dim Remark As String = ""
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                LblIdNo = DirectCast(Gvr.FindControl("LblIdno"), Label)
                'LblIdVerify = DirectCast(Gvr.FindControl("LblIdVerify"), Label)
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

                    str = str & "Update trnvisitingMaster Set ActiveStatus = 'Y',IsApprove='Y', AdminRemark= '" & Remark & "',VerifyDate = getdate(),VerifyBy ='" & Val(Session("Userid")) & "' Where VId = '" & lbl.Text & "'"


                    'End If

                End If
            Next
            Dim i As Integer = 0
            If str <> "" Then
                i = objDAL.UpdateData(str)
            End If
            If i <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert(' Verified successfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            Else
                scrname = "<SCRIPT language='javascript'>alert(' Verified unsuccessfuly. ');" & "</SCRIPT>"
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

                    'If RbtVerifi.SelectedValue = "N" Then
                    Remark = "Forms UnVerify of IdNo:" & LblIdno.Text & ""
                    ''str = str & "; Update KycVerify Set IsAddrssverified='R', AddrssVerifyDate=getdate(),AddrssUserid='" & Val(Session("Userid")) & "', AddrssRemark='" & TxtARemark.Text & "',AddressRejectId='" & DDlREason.SelectedValue & "' " & _
                    '                " where IsAddrssverified<>'R' And  formno='" & lbl.Text & "'"
                    '                str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,ImgPath1,Userid,RectimeStamp,Status,Remark,RejectId)" & _
                    '"select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'A',b.AddrProof,b.BackAddressProof,'" & Val(Session("Userid")) & "',Getdate(),2,'" & TxtARemark.Text.Trim & "','" & DDlREason.SelectedValue & "' from M_MemberMaster as a,KycVerify as b" & _
                    '" where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "'"
                    '                str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "'"
                    '                str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                    '"('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','Address Proof UnVerify','" & Remark & "',Getdate(),'" & lbl.Text & "')"
                    str = str & " Update trnvisitingMaster Set ActiveStatus = 'R',IsApprove='Y', AdminRemark= '" & Remark & "',VerifyDate = getdate(),VerifyBy ='" & Val(Session("Userid")) & "' Where Vid = '" & lbl.Text & "'"

                    'End If
                End If
                'End If
            Next

            Dim i As Integer = 0
            If str <> "" Then
                i = objDAL.UpdateData(str)
            End If
            If i <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert(' Unapprove successfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            Else
                scrname = "<SCRIPT language='javascript'>alert(' Unapprove unsuccessfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)


            End If
            'DivRemark.Visible = False
            'TxtARemark.Text = ""
            BindData()
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub BTnUnVerification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTnUnVerification.Click
        'DivRemark.Visible = True
        DivRemark.Visible = False
        BtnVerifiy.Enabled = False
        BTnUnVerification.Enabled = False
        'FillDetail()
    End Sub
    'Protected Sub FillDetail()
    '    Try


    '        Dim s As String = ""
    '        s = "Select * from M_KYCReject where activeStatus='Y'"
    '        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '        Dim Dt As DataTable
    '        Dt = New DataTable
    '        Dt = objDAL.GetData(s)
    '        If Dt.Rows.Count > 0 Then
    '            DDlREason.DataValueField = "KId"
    '            DDlREason.DataTextField = "reason"
    '            DDlREason.DataSource = Dt
    '            DDlREason.DataBind()
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class




