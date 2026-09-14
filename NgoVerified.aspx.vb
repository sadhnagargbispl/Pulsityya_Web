

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class NgoVerified
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Member / Ngo Verify"
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
                               " CASE WHEN c.NgoImage <>'' Then c.NgoImage ELSE '' END NgoProofStatus," & _
                                      " cASE WHEN c.NgoImage LIKE '%.pdf' Or b.NgoImage like '%.PDF' THEN b.Ngoimage else '' end as PDFLink1 ," & _
            " Case when c.NgoImage like '%.pdf' Or c.NgoImage like '%.PDF' then '~\images\pdf.png' else b.NgoImage end as NgoImage" & _
                               "case when c.IsNgoVerified='Y' then 'False' else 'True' end as EnableStatus," & _
                               " Case when c.IsNgoVerified='N' then '' else Replace(CONVERT(varchar,c.NgoVerifiedDate ,106),' ','-') end as NgoVerifiedDate, " & _
      " Case when c.IsNgoVerified='N' then '' else c.NgoRemark end as VerifyRemark " & _
     " From KycVerify as c  " & _
     " Where 1=1 and c.NgoImage<>''  and c.Formno='" & LblFormno.Text & "' Order by c.NgoImageDate DESC"
        dtData = New DataTable
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dtData = objDAL.GetData(sql)
        If dtData.Rows.Count > 0 Then
            ImgPan.ImageUrl = dtData.Rows(0)("NgoProofStatus")
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
                Condition = Condition & " And c.IsNgoVerified='" & DDlVerify.SelectedValue & "'  "
            End If
            If RbtSearch.SelectedValue <> "A" Then
                Condition = Condition & " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'  "
            End If
            Dim sql As String = "Select Cast(a.FormNo as varchar) as FormNo,Replace(CONVERT(varchar,Doj,106),' ','-')as Doj," & _
            " Case when A.Activestatus='Y' then Replace(CONVERT(varchar,UpgradeDate,106),' ','-') Else '' End as ActivationDate," & _
                                " IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,AAdharno3 as NgoNo, " & _
                               " CASE WHEN c.IsNgoVerified='Y' THEN 'Verified' when c.IsNgoVerified='R' then 'Rejected' Else 'Verification Due' END AS NgoVerf," & _
                              " case when c.NgoImage<>'' then Replace(CONVERT(varchar,c.NgoImageDate ,106),' ','-') else '' end  as NgoProofDate, " & _
                               " CASE WHEN c.NgoImage <>'' Then c.NgoImage ELSE '' END NgoProofStatus,'' as Link," & _
                               " case when c.IsNgoVerified='Y' then 'False' else 'True' end as EnableStatus,case when NgoImage like '%.pdf' or NgoImage like '%.PDF' then '' else NgoImage end as PDfLink," & _
                               " Case when c.IsNgoVerified='N' then '' else Replace(CONVERT(varchar,c.NgoVerifiedDate ,106),' ','-') end as NgoVerifiedDate, " & _
      " Case when c.IsNgoVerified='N' then '' else c.NgoRemark end as VerifyRemark ,Isnull(e.UserName,' ')As VerifyBy,Isnull(f.Reason, '')As RejectReason" & _
     " From M_MemberMaster as a Inner Join KycVerify as c On a.Formno=c.Formno  Left Join  M_KycReject as f On c.NgoRejectId=f.Kid" & _
     " Left Join M_Usermaster as e On c.NgoUserid=e.UserId and e.RowStatus='Y' " & _
     " Where 1=1 and c.NgoImage<>''   " & Condition & " Order by c.NgoImageDate DESC"

            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            If dtData.Rows.Count > 0 Then
                For Each Dr As DataRow In dtData.Rows
                    If Dr("PDFLink") <> "" Then

                        Dr("Link") = "<a style='font-weight:bold;color:Blue;' href='Img.aspx?ID=" & Dr("FormNo") & "&Type=Ngo' onclick=""return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )""> View </a>"
                        '" <img src='" & Dr("PDFLink") & "' Width=""50px"" Height=""50px""  /> 
                    Else

                        Dr("Link") = "<a style='font-weight:bold;color:Blue;' href='" & Dr("NgoproofStatus") & "'  target=""_blank""> <img src='images/pdf.png' /> </a>"

                    End If
                Next
            End If
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
                Condition = Condition & " And c.IsNgoVerified='" & DDlVerify.SelectedValue & "'  "
            End If
            If RbtSearch.SelectedValue <> "A" Then
                Condition = Condition & " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'  "
            End If
            Dim sql As String = " Select a.IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemberName,Replace(CONVERT(varchar,Doj,106),' ','-')as Doj," & _
                                " Case when a.Activestatus='Y' then Replace(CONVERT(varchar,UpgradeDate,106),' ','-') Else '' End as ActivationDate," & _
                                " Aadharno3 as GSTNo," & _
                                " CASE WHEN c.IsNgoVerified='Y' THEN 'Verified' when c.IsNgoVerified='R' then 'Rejected' Else 'Verification Due' END AS NgoVerify," & _
                                " case when c.NgoImage<>'' then Replace(CONVERT(varchar,c.NgoImageDate ,106),' ','-') else '' end  as NgoProofDate, " & _
                                " Case when c.IsNgoVerified='N' then '' else Replace(CONVERT(varchar,c.NgoVerifiedDate ,106),' ','-') end as NgoVerifiedDate, " & _
                                " Case when c.IsNgoVerified='N' then '' else c.NgoRemark end as RejectRemark,Isnull(f.Reason, '')As RejectReason,Isnull(e.UserName,' ')As VerifyBy " & _
                                " From M_MemberMaster as a Inner Join KycVerify as c On a.Formno=c.Formno Left Join  M_KycReject as f On c.NgoRejectId=f.Kid " & _
                                " Left Join M_Usermaster as e On c.NgoUserid=e.UserId and e.RowStatus='Y' Where 1=1 and c.NgoImage<>''  " & _
                                " " & Condition & " Order by c.NgoImageDate DESC"
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("NgoDetail.xls", dg)

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
            Dim Chk As CheckBox
            Dim Remark As String = ""
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                LblIdNo = DirectCast(Gvr.FindControl("LblIdno"), Label)
                If Chk.Checked = True Then

                    'If RbtVerifi.SelectedValue = "Y" Then
                    Remark = "Ngo Verify of IdNo:" & LblIdNo.Text & ""
                    str = str & "; Update KycVerify Set IsNgoVerified='Y',NgoVerifiedDate=getdate(),NgoUserid='" & Val(Session("Userid")) & "',NgoRemark='',NgoRejectId='0' where formno='" & lbl.Text & "' AND IsNgoVerified<>'Y'"
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status)" & _
                      "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'G',b.NgoImage,'" & Val(Session("Userid")) & "',Getdate(),1 from M_MemberMaster as a,KycVerify as b" & _
                      " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "'"
                    str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Ngo Verify ','Ngo Certificate Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"


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

                    ' If RbtVerifi.SelectedValue = "N" Then
                    Remark = "Ngo UnVerify of IdNo:" & LblIdno.Text & ""
                    str = str & "; Update KycVerify Set IsNgoVerified='R',NgoVerifiedDate=getdate(),NgoUserid='" & Val(Session("Userid")) & "',NgoRemark='" & TxtARemark.Text & "',NgoRejectId='" & DDlREason.SelectedValue & "' where formno='" & lbl.Text & "' AND IsNgoVerified<>'R'"
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status,Remark,RejectId)" & _
   "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'P',b.NgoImage,'" & Val(Session("Userid")) & "',Getdate(),2,'" & TxtARemark.Text.Trim & "','" & DDlREason.SelectedValue & "' from M_MemberMaster as a,KycVerify as b" & _
   " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "'"
                    str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Ngo Verify ','Ngo Certificate UnVerify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

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



