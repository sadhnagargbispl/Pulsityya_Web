

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class InvoiceVerified
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
    Protected Sub ApproveData(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        Dim LblFormno As New Label
        Dim sql As String = ""
        LblFormno.Text = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        sql = "Select Cast(c.FormNo as varchar) as FormNo, " & _
                               " c.InvoiceUrl," & _
                               " case when c.Status='Y' then 'False' else 'True' end as EnableStatus," & _
                               " Case when c.Status='N' then '' else Replace(CONVERT(varchar,c.ApproveDate ,106),' ','-') end as PanVerifyDate, " & _
      " Case when c.Status='N' then '' else c.Remarks end as VerifyRemark " & _
     " From Invoice as c  " & _
     " Where 1=1   and c.Formno='" & LblFormno.Text & "' Order by c.Rectimestamp DESC"
        dtData = New DataTable
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dtData = objDAL.GetData(sql)
        If dtData.Rows.Count > 0 Then
            ImgPan.ImageUrl = dtData.Rows(0)("Invoiceurl")
        End If
        'lblID.Text = DirectCast(GVRw.FindControl("LblID"), Label).Text
        'TxtIDNo.Text = DirectCast(GVRw.FindControl("LblIDNo"), Label).Text
        'TxtName.Text = DirectCast(GVRw.FindControl("LblPayeeName"), Label).Text
        'TxtAmount.Text = DirectCast(GVRw.FindControl("LblAmount"), Label).Text

    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")
        Try


            'Dim sql As String = "Select Cast(FormNo as varchar) as FormNo,IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,CASE WHEN DtPhotoProof is NULL Then '' ELSE 'View' END AS PhotoStatus,Replace(CONVERT(varchar,DtPhotoProof,106),' ','-') as PhDate,Replace(CONVERT(varchar,DtIdentityProof,106),' ','-') as IdentityDate,CASE WHEN DtSignature is NULL Then '' ELSE 'View' END AS SignStatus,Replace(CONVERT(varchar,DtSignature,106),' ','-') as SignDate From M_MemberMaster Where (DTSignature IS NOT NULL OR DtPhotoProof is not null OR DtIdentityProof is not null) " & Condition & " Order by DtPhotoProof DESC,DTSignature DESC"
            If ChkMem.Checked = True Then
                If txtMemId.Text <> "" Then
                    Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
                End If

            End If
            If DDlVerify.SelectedValue <> "S" Then
                Condition = Condition & " And c.Status='" & DDlVerify.SelectedValue & "'  "
            End If
         
            Dim sql As String = "Select c.id,Cast(a.FormNo as varchar) as FormNo,Replace(CONVERT(varchar,Doj,106),' ','-')as Doj," & _
                                       " IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,Invoiceno,Amount," & _
                               " CASE WHEN c.Status='Y' THEN 'Verified' when c.Status='R' then 'Rejected' Else 'Verification Due' END AS PanVerf," & _
                              "  Replace(CONVERT(varchar,c.RectimeStamp ,106),' ','-')  as Date, " & _
                               " Invoiceurl," & _
                               "case when c.Status<>'N' then 'False' else 'True' end as EnableStatus," & _
                               " Case when c.Status='N' then '' else Replace(CONVERT(varchar,c.ApproveDate ,106),' ','-') end as PanVerifyDate, " & _
      " Case when c.Status='N' then '' else c.Remark end as VerifyRemark ,Isnull(e.UserName,' ')As VerifyBy" & _
     " From M_MemberMaster as a Inner Join Invoice as c On a.Formno=c.Formno  " & _
     " Left Join M_Usermaster as e On c.Userid=e.UserId and e.RowStatus='Y' " & _
     " Where 1=1    " & Condition & " Order by c.Rectimestamp DESC"

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

            If ChkMem.Checked = True Then
                If txtMemId.Text <> "" Then
                    Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
                End If

            End If
            If DDlVerify.SelectedValue <> "S" Then
                Condition = Condition & " And c.Status='" & DDlVerify.SelectedValue & "'  "
            End If
            'If RbtSearch.SelectedValue <> "A" Then
            '    Condition = Condition & " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'  "
            'End If
            Dim sql As String = "Select IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemberName,Replace(CONVERT(varchar,Doj,106),' ','-')as [Date of joining]," & _
                                       " Invoiceno,Amount," & _
                               " CASE WHEN c.Status='Y' THEN 'Verified' when c.Status='R' then 'Rejected' Else 'Verification Due' END AS Status," & _
                              "  Replace(CONVERT(varchar,c.RectimeStamp ,106),' ','-')  as Date, " & _
                               " Invoiceurl," & _
                               " Case when c.Status='N' then '' else Replace(CONVERT(varchar,c.ApproveDate ,106),' ','-') end as VerifyDate, " & _
      " Case when c.Status='N' then '' else c.Remark end as VerifyRemark ,Isnull(e.UserName,' ')As VerifyBy" & _
     " From M_MemberMaster as a Inner Join Invoice as c On a.Formno=c.Formno  " & _
     " Left Join M_Usermaster as e On c.Userid=e.UserId and e.RowStatus='Y' " & _
     " Where 1=1    " & Condition & " Order by c.Rectimestamp DESC"
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("Invoice.xls", dg)

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
            Dim id As Label
            Dim Chk As CheckBox
            Dim Remark As String = ""
            Dim amount As String
            Dim s As String = ""
            Dim dt As New DataTable
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                LblIdNo = DirectCast(Gvr.FindControl("LblIdno"), Label)
                id = DirectCast(Gvr.FindControl("Lblid"), Label)
                amount = Math.Round(DirectCast(Gvr.FindControl("Lblamount"), Label).Text / 2, 2)
                If Chk.Checked = True Then
                    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    'If RbtVerifi.SelectedValue = "Y" Then
                    Remark = "Invoice Verify of IdNo:" & LblIdNo.Text & ""
                    s = "select * from Invoice where id='" & Val(id.Text) & "' and Status='N'"
                    dt = objDAL.GetData(s)
                    If dt.Rows.Count > 0 Then


                        str = str & "; Update Invoice Set Status='Y',ApproveDate=getdate(),Userid='" & Val(Session("Userid")) & "' where formno='" & lbl.Text & "' and Id='" & Val(id.Text) & "' AND Status<>'Y'"

                        str = str & ";Insert Into TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,narration,refno,actype,Vtype,RectimeStamp,SessID,WSessID)" & _
                                    " Values ((select Max(Voucherno)+1 from TrnVoucher),dbo.FormatDate(GetDate(),'dd-MMM-yyyy'),'0',Cast('" & lbl.Text & "' As Varchar),'" & amount & "'," & _
                                    " 'Reward Wallet Credited Against Invoice Approved ' ,'Invoice/'+Cast('" & id.Text & "' as Varchar)+'/'+Cast('" & lbl.Text & "' As Varchar),'S','C',Getdate(),Convert(Varchar,GetDate(),112)" & _
                                    " ,(Select IsNULL(Max(SessID),1) From M_SessnMaster))  "

                        str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Invoice Verify ','Invoice Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                    End If


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
            BtnVerifiy.Enabled = True
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
            Dim id As Label
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                LblIdno = DirectCast(Gvr.FindControl("LblIdno"), Label)
                ID = DirectCast(Gvr.FindControl("Lblid"), Label)
                If Chk.Checked = True Then

                    ' If RbtVerifi.SelectedValue = "N" Then
                    Remark = "Invoice UnVerify of IdNo:" & LblIdno.Text & ""
                    str = str & "; Update Invoice Set Status='R',ApproveDate=getdate(),Userid='" & Val(Session("Userid")) & "',Remark='" & TxtARemark.Text & "' where formno='" & lbl.Text & "' and Id='" & id.Text & "' AND Status<>'R'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Invoice Verify ','Invoice UnVerify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                    'End If
                End If
                'End If
            Next

            Dim i As Integer = 0
            If str <> "" Then
                i = objDAL.UpdateData(str)
            End If
            If i <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert(' UnVerified successfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            Else
                scrname = "<SCRIPT language='javascript'>alert(' UnVerified unsuccessfuly. ');" & "</SCRIPT>"
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

    End Sub
   
End Class



