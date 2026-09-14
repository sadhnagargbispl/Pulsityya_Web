Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class specialbenfitverified
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

    Public Sub BindData(Optional ByVal Condition As String = "")
        Try


            'Dim sql As String = "Select Cast(FormNo as varchar) as FormNo,IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,CASE WHEN DtPhotoProof is NULL Then '' ELSE 'View' END AS PhotoStatus,Replace(CONVERT(varchar,DtPhotoProof,106),' ','-') as PhDate,Replace(CONVERT(varchar,DtIdentityProof,106),' ','-') as IdentityDate,CASE WHEN DtSignature is NULL Then '' ELSE 'View' END AS SignStatus,Replace(CONVERT(varchar,DtSignature,106),' ','-') as SignDate From M_MemberMaster Where (DTSignature IS NOT NULL OR DtPhotoProof is not null OR DtIdentityProof is not null) " & Condition & " Order by DtPhotoProof DESC,DTSignature DESC"
            If ChkMem.Checked = True Then
                If txtMemId.Text <> "" Then
                    Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
                End If

            End If
            If DDlVerify.SelectedValue <> "S" Then
                Condition = Condition & " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "'  "
            End If
          
            Dim sql As String = ""

            sql = "select Cast(a.FormNo as varchar) as FormNo,a.memmname,a.memdname,a.memfname," & _
"Replace(CONVERT(varchar,a.Doj,106),' ','-')as Doj,Replace(CONVERT(varchar,a.memdob,106),' ','-')as memdob," & _
"Case when a.Activestatus='Y' then Replace(CONVERT(varchar,a.UpgradeDate,106),' ','-') else '' End as ActivationDate," & _
"a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemName,a.Address1,a.City," & _
"cASE WHEN c.BackAddressProof<>'' then c.BackAddressProof else '" & Session("CompWeb") & "/Images/no_photo.jpg' end as BackAdressProof, " & _
"Case when c.BackAddressProof<>'' then Replace(convert(varchar,BackAddressDate,106),' ','-')else '' end as BackAddressDate, " & _
"CASE WHEN c.IsAddrssverified='Y' THEN 'Verified' when c.IsAddrssverified='R' then 'Rejected' Else 'Verification Due' END AS AddrssVerf," & _
"case when c.AddrProof<>'' then Replace(CONVERT(varchar,c.AddrProofDate ,106),' ','-') else '' end  as AddressProofDate," & _
"d.IdType,c.IdProofNo, " & _
"CASE WHEN c.AddrProof <>'' Then 	c.AddrProof ELSE '" & Session("CompWeb") & "/Images/no_photo.jpg' END AddressproofStatus, " & _
"Case when c.IsAddrssverified='Y' then 'False' else 'True' end as EnableStatus, " & _
"Case when c.IsAddrssVerified='Y' then Replace(CONVERT(varchar,c.AddrssVerifyDate ,106),' ','-') else '' end  as AddressVerifyDate," & _
"Isnull(e.Username,'')VerifyBy, " & _
"Case when c.isAddrssVerified='R' then c.AddrssRemark else '' end as RejectRemark,Isnull(f.Reason, '')As RejectReason " & _
" From M_MemberMaster as a,M_StatedivMaster as b, M_BAnkMaster as g   ,SKycVerify as c " & _
"Left Join  M_SKycReject as f On c.AddressRejectId=f.Kid Left Join  M_UserMaster as e On c.AddrssUserId=e.Userid and e.Rowstatus='Y'" & _
",M_STypeMaster as d where  c.IdType=d.Id and d.ActiveStatus='Y' and a.bankid=g.bankcode and g.rowstatus='Y' " & _
"and a.Formno=c.Formno and a.statecode=b.statecode and b.Rowstatus='Y'  and( c.BackAddressProof<>'' Or c.AddrProof <>'')" & _
"   " & Condition & " Order by AddressProofdate DESC"



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

            If ChkMem.Checked = True Then
                If txtMemId.Text <> "" Then
                    Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
                End If

            End If
            If DDlVerify.SelectedValue <> "S" Then
                Condition = Condition & " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "'  "
            End If
           
            Dim sql As String = ""

            sql = "select Cast(a.FormNo as varchar) as FormNo,a.memmname,a.memdname,a.memfname," & _
"Replace(CONVERT(varchar,a.Doj,106),' ','-')as Doj,Replace(CONVERT(varchar,a.memdob,106),' ','-')as memdob," & _
"Case when a.Activestatus='Y' then Replace(CONVERT(varchar,a.UpgradeDate,106),' ','-') else '' End as ActivationDate," & _
"a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemName,a.Address1,a.City," & _
"cASE WHEN c.BackAddressProof<>'' then c.BackAddressProof else '" & Session("CompWeb") & "/Images/no_photo.jpg' end as BackAdressProof, " & _
"Case when c.BackAddressProof<>'' then Replace(convert(varchar,BackAddressDate,106),' ','-')else '' end as BackAddressDate, " & _
"CASE WHEN c.IsAddrssverified='Y' THEN 'Verified' when c.IsAddrssverified='R' then 'Rejected' Else 'Verification Due' END AS AddrssVerf," & _
"case when c.AddrProof<>'' then Replace(CONVERT(varchar,c.AddrProofDate ,106),' ','-') else '' end  as AddressProofDate," & _
"d.IdType,c.IdProofNo, " & _
"CASE WHEN c.AddrProof <>'' Then 	c.AddrProof ELSE '" & Session("CompWeb") & "/Images/no_photo.jpg' END AddressproofStatus, " & _
"Case when c.IsAddrssverified='Y' then 'False' else 'True' end as EnableStatus, " & _
"Case when c.IsAddrssVerified='Y' then Replace(CONVERT(varchar,c.AddrssVerifyDate ,106),' ','-') else '' end  as AddressVerifyDate," & _
"Isnull(e.Username,'')VerifyBy, " & _
"Case when c.isAddrssVerified='R' then c.AddrssRemark else '' end as RejectRemark,Isnull(f.Reason, '')As RejectReason " & _
" From M_MemberMaster as a,M_StatedivMaster as b, M_BAnkMaster as g   ,SKycVerify as c " & _
"Left Join  M_SKycReject as f On c.AddressRejectId=f.Kid Left Join  M_UserMaster as e On c.AddrssUserId=e.Userid and e.Rowstatus='Y'" & _
",M_STypeMaster as d where  c.IdType=d.Id and d.ActiveStatus='Y' and a.bankid=g.bankcode and g.rowstatus='Y' " & _
"and a.Formno=c.Formno and a.statecode=b.statecode and b.Rowstatus='Y'  and( c.BackAddressProof<>'' Or c.AddrProof <>'')" & _
"   " & Condition & " Order by AddressProofdate DESC"




            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("KYCDetail.xls", dg)

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
                    Remark = "Address Proof Verify of IdNo:" & LblIdNo.Text & ""
                    str = str & " Update SKycVerify Set IsAddrssverified='Y',AddrssVerifyDate=getdate(),AddrssUserId='" & Val(Session("Userid")) & "' ,sessid=(Select Max(SessID) from M_SessnMAster),msessid=(Select Max(SessID) from M_MonthSessnMaster) " & _
                    "  where IsidVerified<>'Y'  AND formno='" & lbl.Text & "';"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','SKYC Verify ','Address Proof Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"


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
                    Remark = "KYC  UnVerify of IdNo:" & LblIdno.Text & ""
                    str = str & " Update SKycVerify Set IsAddrssverified='R', AddrssVerifyDate=getdate(),AddrssUserid='" & Val(Session("Userid")) & "', AddrssRemark='" & TxtARemark.Text & "',AddressRejectId='" & DDlREason.SelectedValue & "' " & _
 " where IsAddrssverified<>'R'  and  formno='" & lbl.Text & "';"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','KYC UnVerify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

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




