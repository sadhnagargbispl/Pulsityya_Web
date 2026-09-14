Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Partial Class App_UI_Application_Pages_Dispatchproduct
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
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

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")
        Try


            'Dim sql As String = "Select Cast(FormNo as varchar) as FormNo,IDNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,CASE WHEN DtPhotoProof is NULL Then '' ELSE 'View' END AS PhotoStatus,Replace(CONVERT(varchar,DtPhotoProof,106),' ','-') as PhDate,Replace(CONVERT(varchar,DtIdentityProof,106),' ','-') as IdentityDate,CASE WHEN DtSignature is NULL Then '' ELSE 'View' END AS SignStatus,Replace(CONVERT(varchar,DtSignature,106),' ','-') as SignDate From M_MemberMaster Where (DTSignature IS NOT NULL OR DtPhotoProof is not null OR DtIdentityProof is not null) " & Condition & " Order by DtPhotoProof DESC,DTSignature DESC"



            If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then
                If ddllist.SelectedValue = "E" Then
                    Condition = Condition & " And  CAST(ElligDate As date ) >= '" + txtStartDate.Text + "'"
                    Condition = Condition & " And CAST(ElligDate As date ) <= '" + txtEndDate.Text + "'"
                ElseIf ddllist.SelectedValue = "A" Then
                    Condition = Condition & " And  CAST(ActivationDate As date ) >= '" + txtStartDate.Text + "'"
                    Condition = Condition & " And CAST(ActivationDate As date ) <= '" + txtEndDate.Text + "'"
                ElseIf ddllist.SelectedValue = "D" Then
                    Condition = Condition & " And  CAST(productgivendate As date ) >= '" + txtStartDate.Text + "'"
                    Condition = Condition & " And CAST(productgivendate As date ) <= '" + txtEndDate.Text + "'"
                End If
            Else


            End If
            If rbtnlist.SelectedValue = "Y" Then
                Condition = Condition & " And  Productgiven='Y'"
            Else

                Condition = Condition & " And  Productgiven='N'"
            End If
            If ChkMem.Checked = True Then
                Condition = Condition & " And  b.Idno = '" & txtMember.Text & "'"
                '' Condition = txtMember.Text
            Else
                txtMember.Text = ""
            End If


            Dim sql As String = "select Aid,idno,Memfirstname as membername,Replace(Convert(Varchar,ElligDate,106),'','-')  as Achievementdate," & _
" Replace(Convert(Varchar,ActivationDate,106),'','-') as Eligibledate,Case when ElligForProduct='Y'  then Remark else 'Two Direct not Completed' end as Remarks ," & _
" Case when ElligForProduct='Y' and Productgiven='N' then 'True' else 'False' end as enablestatus,Case when productgiven='Y' then replace(Convert(Varchar,productgivendate,106),'','-') else '' end  as DispatchDate" & _
  " from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..EligibleforProduct as a,M_MembeRMaster as b  where  a.Formno = B.Formno " & Condition & ""


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

            If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then
                If ddllist.SelectedValue = "E" Then
                    Condition = Condition & " And  CAST(ElligDate As date ) >= '" + txtStartDate.Text + "'"
                    Condition = Condition & " And CAST(ElligDate As date ) <= '" + txtEndDate.Text + "'"
                ElseIf ddllist.SelectedValue = "A" Then
                    Condition = Condition & " And  CAST(ActivationDate As date ) >= '" + txtStartDate.Text + "'"
                    Condition = Condition & " And CAST(ActivationDate As date ) <= '" + txtEndDate.Text + "'"
                ElseIf ddllist.SelectedValue = "D" Then
                    Condition = Condition & " And  CAST(productgivendate As date ) >= '" + txtStartDate.Text + "'"
                    Condition = Condition & " And CAST(productgivendate As date ) <= '" + txtEndDate.Text + "'"
                End If
            Else


            End If
            If rbtnlist.SelectedValue = "Y" Then
                Condition = Condition & " And  Productgiven='Y'"
            Else

                Condition = Condition & " And  Productgiven='N'"
            End If

            Dim sql As String = "select Aid,idno,Memfirstname as membername,Replace(Convert(Varchar,ElligDate,106),'','-')  as Eligibledate," & _
" Replace(Convert(Varchar,ActivationDate,106),'','-') as Achievedate,Case when ElligForProduct='Y'  then Remark else 'Two Direct not Completed' end as Remarks ," & _
" Case when productgiven='Y' then replace(Convert(Varchar,productgivendate,106),'','-') else '' end  as DispatchDate" & _
  " from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..EligibleforProduct as a,M_MembeRMaster as b  where a.Formno = B.Formno " & Condition & ""



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
            Dim cnt As Integer
            Dim updateeffect As Integer
            Dim LblIdNo As Label
            Dim lblmemberid As Label
            Dim Chk As CheckBox
            Dim Remark As String = ""
            Dim i As Integer = 0
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                LblIdNo = DirectCast(Gvr.FindControl("LblIdno"), Label)
                lblmemberid = DirectCast(Gvr.FindControl("LblmemberIdno"), Label)
                If Chk.Checked = True Then
                    Dim Sql As String = ""
                    'If RbtVerifi.SelectedValue = "Y" Then
                    ''Remark = "Address Proof Verify of IdNo:" & LblIdNo.Text & ""
                    '                str = str & "; Update KycVerify Set IsAddrssverified='Y',AddrssVerifyDate=getdate(),AddrssUserId='" & Val(Session("Userid")) & "' where IsidVerified<>'Y' and  formno='" & lbl.Text & "'"
                    '                str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "'"
                    '                str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,ImgPath1,Userid,RectimeStamp,Status)" & _
                    '                    "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'A',b.AddrProof,b.BackAddressProof,'" & Val(Session("Userid")) & "',Getdate(),1 from M_MemberMaster as a,KycVerify as b" & _
                    '                    " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "'"
                    '                str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                    '"('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','Address Proof Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"
                    'End If
                    Sql = ""
                    Sql &= ""
                    '       Sql &= "Update " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..EligibleforProduct Set Remark='Product Dispatch',Productgiven='Y',Productgivendate=GETDATE(),UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' ,prodid=1001 Where AID=" & LblIdNo.Text & ";" & _
                    '      " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                    '"('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Dispatch Product','Dispatch Product','Product Dispatch',Getdate(),'" & 0 & "')"
                    '       Sql = Sql & "exec " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..ActivationBill '" & lblmemberid.Text & "','" & LblIdNo.Text & "'  "

                    Sql = "exec " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..ActivationBill '" & lblmemberid.Text & "','" & LblIdNo.Text & "'  "



                    updateeffect = objDAL.SaveData(Sql)
                    If updateeffect > 0 Then
                        Sql = "Update " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..EligibleforProduct Set Remark='Product Dispatch',Productgiven='Y',Productgivendate=GETDATE(),UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' ,prodid=1001 Where AID=" & LblIdNo.Text & ";" & _
                       " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                 "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Dispatch Product','Dispatch Product','Product Dispatch',Getdate(),'" & 0 & "')"
                        updateeffect = objDAL.SaveData(Sql)
                        cnt = cnt + 1

                    End If
                    'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                    

                End If
            Next


            If cnt <> 0 Then




                scrname = "<SCRIPT language='javascript'>alert('" & cnt & "'' Dispatch successfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            Else
                scrname = "<SCRIPT language='javascript'>alert('Dispatch unsuccessfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)


            End If
            'Me.RegisterStartupScript("MyAlert", scrname)
            BindData()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub sendSMS(ByVal Idno As String, ByVal MemberPass1 As String, ByVal Mobile As String)
        Try


            'dbConnect.OpenConnection()
            Dim MemberPass As String = ""
            Dim MemberTransPassw As String = ""
            MemberPass = MemberPass1
            ''MemberTransPassw = Session("SMSTransPassw")
            MemberPass = MemberPass.Replace("%", "%25")
            MemberPass = MemberPass.Replace("&", "%26")
            MemberPass = MemberPass.Replace("#", "%23")
            MemberPass = MemberPass.Replace("'", "%22")
            MemberPass = MemberPass.Replace(",", "%2C")
            MemberPass = MemberPass.Replace("(", "%28")
            MemberPass = MemberPass.Replace(")", "%29")
            MemberPass = MemberPass.Replace("*", "%2A")
            MemberPass = MemberPass.Replace("!", "%21")
            MemberPass = MemberPass.Replace("/", "%2F")
            MemberPass = MemberPass.Replace("@", "%40")
            MemberTransPassw = MemberTransPassw.Replace("%", "%25")
            MemberTransPassw = MemberTransPassw.Replace("&", "%26")
            MemberTransPassw = MemberTransPassw.Replace("#", "%23")
            MemberTransPassw = MemberTransPassw.Replace("'", "%22")
            MemberTransPassw = MemberTransPassw.Replace(",", "%2C")
            MemberTransPassw = MemberTransPassw.Replace("(", "%28")
            MemberTransPassw = MemberTransPassw.Replace(")", "%29")
            MemberTransPassw = MemberTransPassw.Replace("*", "%2A")
            MemberTransPassw = MemberTransPassw.Replace("!", "%21")
            MemberTransPassw = MemberTransPassw.Replace("/", "%2F")
            MemberTransPassw = MemberTransPassw.Replace("@", "%40")
            If Len(Mobile) >= 10 And IsNumeric(Mobile) = True Then
                Dim client As New WebClient
                Dim baseurl As String
                Dim data As Stream
                Dim sms As String = "Welcome To " & Session("CompName") & ", Thank You For Registration.Your ID Is " & Idno & " and Password is " & MemberPass1 & ". Visit " & "http://6sence.in/" & "  Best of luck."
                Try
                    baseurl = "http://www.apiconnecto.com/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & Mobile & "&SenderId=" & Session("ClientId") & ""

                    data = client.OpenRead(baseurl)
                    Dim reader As New StreamReader(data)
                    Dim s As String
                    s = reader.ReadToEnd()
                    data.Close()
                    reader.Close()
                Catch ex As Exception
                    'MsgBox(ex.Message)
                End Try

            End If

        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Sub




    Protected Sub BTnUnVerification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTnUnVerification.Click
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

                    str = str & " Update m_memberMaster_B Set ApproveStatus ='R',ApproveBy = '" & Session("UserName") & "',"
                    str = str & " ApproveDate =GeTdate() Where transno = " & LblIdno.Text & ""
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

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class




