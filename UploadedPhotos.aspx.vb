Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_UploadedPhotos
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
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
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
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")
        'Dim sql As String = "Select Cast(FormNo as varchar) as FormNo,IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,CASE WHEN DtPhotoProof is NULL Then '' ELSE 'View' END AS PhotoStatus,Replace(CONVERT(varchar,DtPhotoProof,106),' ','-') as PhDate,Replace(CONVERT(varchar,DtIdentityProof,106),' ','-') as IdentityDate,CASE WHEN DtSignature is NULL Then '' ELSE 'View' END AS SignStatus,Replace(CONVERT(varchar,DtSignature,106),' ','-') as SignDate From M_MemberMaster Where (DTSignature IS NOT NULL OR DtPhotoProof is not null OR DtIdentityProof is not null) " & Condition & " Order by DtPhotoProof DESC,DTSignature DESC"
        Dim sql As String = "Select Cast(FormNo as varchar) as FormNo,Replace(CONVERT(varchar,Doj,106),' ','-')as Doj,Replace(CONVERT(varchar,UpgradeDate,106),' ','-') as UpgradeDate," & _
                           " IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName," & _
                          " CASE WHEN IsAddrssVerified='Y' THEN 'Verified' Else 'Not Verified' END AS AddrssVerf," & _
                        " CASE WHEN IsIdVerified='Y' THEN 'Verified' Else 'Not Verified' END AS IdVerf," & _
                         "  CASE WHEN IdProof<>'' THEN IdProof  ELSE '' END AS IdStatus," & _
                         " CASE WHEN AddrProof <>'' Then AddrProof ELSE '' END AS AddrssStatus," & _
                       " case when IdProof<>'' then Replace(CONVERT(varchar,IdProofDate ,106),' ','-') else '' end  as IDProofDate ," & _
                        " case when AddrProof <>'' then Replace(CONVERT(varchar,AddrProofDate,106),' ','-') else '' end  as AddrssDate,IdProofDate From M_MemberMaster Where " & _
                       " ( AddrProof <>'' OR IdProof <>'') " & Condition & " Order by AddrProofDate DESC"

        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Condition As String = ""
        If ChkImgType.Checked = True Then
            Condition = " AND " & CmbImgType.SelectedValue & "<>''"
        End If
        If ChkMem.Checked = True Then
            Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
        End If
        BindData(Condition)
    End Sub

    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
    End Sub

    Protected Sub BtnVerifiy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnVerifiy.Click
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
                If RbtVerifi.SelectedValue = "V" Then
                    Remark = " IDProof Verify of IdNo:" & LblIdNo.Text & ""
                    str = str & ";Update M_MemberMaster SET IsIdVerified='Y' where FormNo='" & lbl.Text & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
     "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','IDProof Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                ElseIf RbtVerifi.SelectedValue = "A" Then
                    Remark = " AddressProof Verify of IdNo:" & LblIdNo.Text & ""
                    str = str & "; Update M_MemberMaster Set IsAddrssVerified='Y' where formno='" & lbl.Text & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','AddressProof Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                End If
            End If
            'End If
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
    End Sub

    Protected Sub BtnUnVerify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUnVerify.Click
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
            LblIdNo = DirectCast(Gvr.FindControl("LblIdno"), Label)
            If Chk.Checked = True Then
                If RbtVerifi.SelectedValue = "U" Then
                    Remark = " IDProof UnVerify of IdNo:" & LblIdNo.Text & ""
                    str = str & ";Update M_MemberMaster SET IsIdVerified='N' where FormNo='" & lbl.Text & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','IDProof UnVerify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                ElseIf RbtVerifi.SelectedValue = "N" Then
                    Remark = " AddressProof UnVerify of IdNo:" & LblIdno.Text & ""
                    str = str & "; Update M_MemberMaster Set IsAddrssVerified='N' where formno='" & lbl.Text & "'"
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
  "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','AddressProof UnVerify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                End If
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
        BindData()
    End Sub

    Protected Sub ChkVerify_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkVerify.CheckedChanged
        If ChkVerify.Checked Then
            BtnUnVerify.Enabled = True
            BtnVerifiy.Enabled = True
            RbtVerifi.Enabled = True
        Else
            BtnUnVerify.Enabled = False
            BtnVerifiy.Enabled = False
            RbtVerifi.Enabled = False
        End If

    End Sub
End Class
