
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Partial Class KYCVERIFYWITHPDFUPDATE
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
            If txtMemId.Text <> "" Then
                Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
            End If
            If DDlVerify.SelectedValue <> "S" Then
                Condition = Condition & " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "'  "
            End If
            If RbtSearch.SelectedValue <> "A" Then
                Condition = Condition & " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'  "
            End If
            'If DDlDateselect.SelectedValue = "A" Then
            If txtStartDate.Text <> "" Then
                Condition = Condition & " And  Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime)>='" & txtStartDate.Text & "'  "
            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & " And  Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime)<='" & txtEndDate.Text & "'  "
            End If
            'End If

            ' ===================== COMPANY KYC columns (sirf 1108) =====================
            ' Per-type proof: Bill / GST / Other (alag columns). Empty pe blank.
            Dim compCols As String = ""
            If Session("compid") = "1108" Then
                compCols = " , Isnull(c.CompBillNo,'') as CompBillNo, Isnull(c.CompBillImg,'') as CompBillImg," & _
                    " Isnull(c.CompGstNo,'') as CompGstNo, Isnull(c.CompGstImg,'') as CompGstImg," & _
                    " Isnull(c.CompOtherNo,'') as CompOtherNo, Isnull(c.CompOtherImg,'') as CompOtherImg "
            Else
                compCols = " , '' as CompBillNo, '' as CompBillImg," & _
                    " '' as CompGstNo, '' as CompGstImg," & _
                    " '' as CompOtherNo, '' as CompOtherImg "
            End If
            ' ===========================================================================

            Dim sql As String = ""
            sql = " select Cast(a.FormNo as varchar) as FormNo,Replace(CONVERT(varchar,a.Doj,106),' ','-')as Doj," & _
                            " Case when a.Activestatus='Y' then Replace(CONVERT(varchar,a.UpgradeDate,106),' ','-') else '' End as ActivationDate," & _
                            " a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemName,a.Address1,a.City,a.Tehsil,a.District," & _
                            " b.Statename,g.Bankname,a.Acno,a.Branchname,a.Ifscode," & _
                            " a.Pincode,a.Fax,a.panno,cASE WHEN c.BackAddressProof<>'' then c.BackAddressProof else '" & Session("CompWeb") & "/Images/no_photo.jpg' end as BackAdressProof," & _
                            " Case when c.BackAddressProof<>'' then Replace(convert(varchar,BackAddressDate,106),' ','-')else '' end as BackAddressDate, " & _
                            " CASE WHEN c.IsAddrssverified='Y' THEN 'Verified' when c.IsAddrssverified='R' then 'Rejected' Else 'Verification Due' END AS AddrssVerf," & _
                            " case when c.AddrProof<>'' then Replace(CONVERT(varchar,c.AddrProofDate ,106),' ','-') else '' end  as AddressProofDate,d.IdType,c.IdProofNo, " & _
                            " CASE WHEN c.AddrProof <>'' Then 	c.AddrProof ELSE '" & Session("CompWeb") & "/Images/no_photo.jpg' END AddressproofStatus,Case when c.IsAddrssverified='Y' then 'False' else 'True' end as EnableStatus," & _
                            " Case when c.IsAddrssVerified='Y' then Replace(CONVERT(varchar,c.AddrssVerifyDate ,106),' ','-') else '' end  as AddressVerifyDate, Isnull(e.Username,'')VerifyBy," & _
                            " Case when c.isAddrssVerified='R' then c.AddrssRemark else '' end as RejectRemark,Isnull(f.Reason, '')As RejectReason," & _
                            " case when c.BankProof<>'' then Replace(CONVERT(varchar,c.BankProofDate ,106),' ','-') else '' end  as BankProofDate, " & _
                           " CASE WHEN c.BankProof <>'' Then c.BankProof ELSE '" & Session("CompWeb") & "/Images/no_photo.jpg' END AS BankProofStatus," & _
" case when c.PanImg<>'' then Replace(CONVERT(varchar,c.PANImgDate ,106),' ','-') else '' end  as PanProofDate, CASE WHEN c.PanImg <>'' Then c.PanImg ELSE '' END PanproofStatus," & _
       " Case when c.IsPanverified='N' then '' else Replace(CONVERT(varchar,c.PanVerifyDate ,106),' ','-') end as PanVerifyDate,CASE WHEN ISNULL(FrontSideForm,'') <> '' THEN FrontSideForm ELSE '' END AS FrontSideForm " & compCols & _
           " From M_MemberMaster a INNER JOIN KycVerify c       ON a.Formno = c.Formno INNER JOIN M_StatedivMaster b ON a.statecode = b.statecode AND b.Rowstatus = 'Y' " & _
          " INNER JOIN M_BAnkMaster g     ON a.bankid = g.bankcode AND g.rowstatus = 'Y' INNER JOIN M_IdTypeMaster d   ON c.IdType = d.Id AND d.ActiveStatus = 'Y' " & _
                            " LEFT JOIN M_KycReject f ON c.AddressRejectId = f.Kid LEFT JOIN M_UserMaster e ON c.AddrssUserId = e.Userid AND e.Rowstatus = 'Y' " & _
                            " LEFT JOIN m_formupload p ON c.formno = p.formno " & _
                            " WHERE (ISNULL(c.BackAddressProof,'') <> '' OR ISNULL(c.AddrProof,'') <> '') " & Condition & " ORDER BY c.AddrProofDate DESC; "
            dtData = New DataTable
            dtData = objDAL.GetData(sql)

            ' ===================== COMPANY columns visibility (sirf 1108) =====================
            ' NOTE: visibility bind se PEHLE set karna zaroori, taaki non-1108 me Eval na chale
            ' Index 26..31 = Bill No, Bill Img, GST No, GST Img, Other No, Other Img
            Dim showComp As Boolean = (Session("compid") = "1108")
            GvData.Columns(26).Visible = showComp
            GvData.Columns(27).Visible = showComp
            GvData.Columns(28).Visible = showComp
            GvData.Columns(29).Visible = showComp
            GvData.Columns(30).Visible = showComp
            GvData.Columns(31).Visible = showComp
            ' =================================================================================

            GvData.DataSource = dtData
            GvData.DataBind()
            If Session("compid") = 1102 Then
                GvData.Columns(23).Visible = True
            Else
                GvData.Columns(23).Visible = False
            End If
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

            'If ChkMem.Checked = True Then
            If txtMemId.Text <> "" Then
                Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
            End If

            'End If
            If DDlVerify.SelectedValue <> "S" Then
                Condition = Condition & " And c.IsAddrssverified='" & DDlVerify.SelectedValue & "'  "
            End If
            If RbtSearch.SelectedValue <> "A" Then
                Condition = Condition & " And a.ActiveStatus='" & RbtSearch.SelectedValue & "'  "
            End If
            If txtStartDate.Text <> "" Then
                Condition = Condition & " And  Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime)>='" & txtStartDate.Text & "'  "
            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & " And  Cast(Convert(varchar,c.AddrssVerifyDate,106) as DateTime)<='" & txtEndDate.Text & "'  "
            End If
            Dim sql As String = ""
            If Session("Compid") = "1055" Then
                sql = " select a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemberName," & _
                " Replace(CONVERT(varchar,case when Cast(a.Doj as date)>='01-Jan-2022' then a.doj else '01-Jan-2022' end,106),' ','-')as Doj," & _
                               " Case when a.Activestatus='Y' then Replace(CONVERT(varchar,a.UpgradeDate,106),' ','-') else '' End as ActivationDate," & _
                               " a.Address1,a.City,a.Tehsil,a.District,b.Statename," & _
                               " a.Pincode,d.IdType,c.IdProofNo,g.Bankname,a.Acno,a.Branchname,a.Ifscode," & _
                               " a.Fax as AccountType,a.panno," & _
                               " CASE WHEN c.IsAddrssverified='Y' THEN 'Verified' when c.IsAddrssverified='R' then 'Rejected' Else 'Verification Due' END AS Verification," & _
                               " Case when c.IsAddrssVerified='Y' then Replace(CONVERT(varchar,c.AddrssVerifyDate ,106),' ','-') else '' end  as VerifyDate " & _
                               ", Isnull(e.Username,'')VerifyBy," & _
                               " Case when c.isAddrssVerified='R' then c.AddrssRemark else '' end as RejectRemark,Isnull(f.Reason, '')As RejectReason" & _
                               " From M_MemberMaster as a,M_StatedivMaster as b, M_BAnkMaster as g ,KycVerify as c Left Join  M_KycReject as f On c.AddressRejectId=f.Kid Left Join M_UserMaster as e On c.AddrssUserId=e.Userid and e.Rowstatus='Y' " & _
                               " ,M_IdTypeMaster as d where c.IdType=d.Id and d.ActiveStatus='Y' and a.Formno=c.Formno and  " & _
                               " a.statecode=b.statecode and b.Rowstatus='Y'and a.bankid=g.bankcode and g.rowstatus='Y' " & _
                               " " & Condition & " and( c.BackAddressProof<>'' Or c.AddrProof <>'') Order by ActivationDate DESC"


            Else
                sql = " select a.IDNo,RTRIM(a.MemFirstName +' ' +a.MemLastName) as MemberName,Replace(CONVERT(varchar,a.Doj,106),' ','-')as Doj," & _
                               " Case when a.Activestatus='Y' then Replace(CONVERT(varchar,a.UpgradeDate,106),' ','-') else '' End as ActivationDate," & _
                               " a.Address1,a.City,a.Tehsil,a.District,b.Statename," & _
                               " a.Pincode,d.IdType,''''+ Convert(Varchar,C.IdProofNo) As IdProofNo,g.Bankname,''''+ Convert(Varchar,a.AcNo) As Acno,a.Branchname,a.Ifscode," & _
                               " a.Fax as AccountType,a.panno," & _
                               " CASE WHEN c.IsAddrssverified='Y' THEN 'Verified' when c.IsAddrssverified='R' then 'Rejected' Else 'Verification Due' END AS Verification," & _
                               " Case when c.IsAddrssVerified='Y' then Replace(CONVERT(varchar,c.AddrssVerifyDate ,106),' ','-') else '' end  as VerifyDate " & _
                               ", Isnull(e.Username,'')VerifyBy," & _
                               " Case when c.isAddrssVerified='R' then c.AddrssRemark else '' end as RejectRemark,Isnull(f.Reason, '')As RejectReason" & _
                               " From M_MemberMaster as a,M_StatedivMaster as b, M_BAnkMaster as g ,KycVerify as c Left Join  M_KycReject as f On c.AddressRejectId=f.Kid Left Join M_UserMaster as e On c.AddrssUserId=e.Userid and e.Rowstatus='Y' " & _
                               " ,M_IdTypeMaster as d where c.IdType=d.Id and d.ActiveStatus='Y' and a.Formno=c.Formno and  " & _
                               " a.statecode=b.statecode and b.Rowstatus='Y'and a.bankid=g.bankcode and g.rowstatus='Y' " & _
                               " " & Condition & " and( c.BackAddressProof<>'' Or c.AddrProof <>'') Order by ActivationDate DESC"


            End If
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
            Dim approovecnt As Integer = 0
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
                    Dim strsql As String = ""
                    Dim dtcheck As DataTable = New DataTable()
                    strsql = " select Count(formno) as cnt from KycVerify where IsBankVerified in ('Y','R') and Formno='" & Val(lbl.Text) & "' "
                    dtcheck = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strsql).Tables(0)
                    ' If Convert.ToInt32(dtcheck.Rows(0)("Cnt") = 0) Then
                    Remark = "Address Proof Verify of IdNo:" & LblIdNo.Text & ""
                    If Session("compid") = "1102" Then
                        Dim strsql1 As String = ""
                        Dim dtcheck1 As DataTable = New DataTable()
                        strsql1 = " select ispancard,doj from m_membermaster where Formno='" & Val(lbl.Text) & "' "
                        dtcheck1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strsql1).Tables(0)
                        If dtcheck1.Rows.Count > 0 Then
                            Dim userDoj As DateTime = CDate(dtcheck1.Rows(0)("doj"))
                            Dim compareDate As DateTime = DateTime.ParseExact("23-01-2026", "dd-MM-yyyy", CultureInfo.InvariantCulture())
                            If Convert.ToString(dtcheck1.Rows(0)("ispancard")) = "Y" AndAlso userDoj.Date >= compareDate Then
                                str = str & "; Update KycVerify Set IsAddress = 'Y',IsAddrssverified='Y',AddrssVerifyDate=getdate(),AddrssUserId='" & Val(Session("Userid")) & "' where IsidVerified<>'Y' AND formno='" & lbl.Text & "';"
                                str = str & "; Update m_formupload Set IsForm = 'Y',ActiveStatus = 'Y' where formno='" & lbl.Text & "';"
                            Else
                                str = str & "; Update KycVerify Set IsAddress = 'Y',IsBank = 'Y',IsPan = 'Y',IsAddrssverified='Y',AddrssVerifyDate=getdate(),AddrssUserId='" & Val(Session("Userid")) & "',IsPanVerified='Y',PanVerifyDate=getdate(),PanUserid='" & Val(Session("Userid")) & "' " & _
                         " ,IsBankVerified='Y',BankverifyDate = getdate(),BankUserId='" & Val(Session("UserId")) & "' where  formno='" & lbl.Text & "';"
                                str = str & "; Update m_formupload Set IsForm = 'Y',ActiveStatus = 'Y' where formno = '" & lbl.Text & "';"
                            End If
                        End If
                    Else
                        str = str & "; Update KycVerify Set IsAddress = 'Y',IsBank = 'Y',IsPan = 'Y',IsAddrssverified='Y',AddrssVerifyDate=getdate(),AddrssUserId='" & Val(Session("Userid")) & "',IsPanVerified='Y',PanVerifyDate=getdate(),PanUserid='" & Val(Session("Userid")) & "' " & _
                        " ,IsBankVerified='Y',BankverifyDate = getdate(),BankUserId='" & Val(Session("UserId")) & "' where  formno='" & lbl.Text & "';"
                        If Session("compid") = "1105" Or Session("compid") = "1107" Or Session("compid") = "1108" Or Session("compid") = "1109" Then
                        Else
                            str = str & "; Update m_formupload Set IsForm = 'Y',ActiveStatus = 'Y' where formno = '" & lbl.Text & "';"
                        End If
                    End If

                    If Session("compid") = "1102" Then
                        str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "';"
                    End If
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,ImgPath1,Userid,RectimeStamp,Status)" & _
                        "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'A',b.AddrProof,b.BackAddressProof,'" & Val(Session("Userid")) & "',Getdate(),1 from M_MemberMaster as a,KycVerify as b" & _
                        " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "';"
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status)" & _
                     "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'P',b.PanImg,'" & Val(Session("Userid")) & "',Getdate(),1 from M_MemberMaster as a,KycVerify as b" & _
                     " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "';"
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status)" & _
                      "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'B',b.BankProof,'" & Val(Session("Userid")) & "',Getdate(),1 from M_MemberMaster as a,KycVerify as b" & _
                      " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "';"

                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','Address Proof Verify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                    approovecnt = approovecnt + 1
                End If

                'End If
            Next
            Dim i As Integer = 0
            If str <> "" Then
                i = objDAL.UpdateData(str)
            End If
            If i <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('" & approovecnt & " Verified successfully. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            Else
                scrname = "<SCRIPT language='javascript'>alert('" & approovecnt & " Verified successfully. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)


            End If
            BindData()

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub BtnUnVerify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUnVerify.Click
        Try
            If TxtARemark.Text = "" Then
                LblARemark.Visible = True
                LblARemark.Text = "Please Enter Remark"
                Exit Sub
            End If
            LblARemark.Visible = False

            Dim Rejectcnt As Integer = 0
            Dim str As String = ""
            Dim scrname As String
            Dim Condition As String = ""
            Dim lbl As Label
            Dim Chk As CheckBox
            Dim Remark As String = ""
            Dim LblIdno As Label
            If (TxtARemark.Text = "") Then

                scrname = "<SCRIPT language='javascript'>alert('Please Enter Remarks ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                Exit Sub
            End If
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                LblIdno = DirectCast(Gvr.FindControl("LblIdno"), Label)
                If Chk.Checked = True Then
                    Dim strsql As String = ""
                    Dim dtcheck As DataTable = New DataTable()
                    strsql = " select Count(formno) as cnt from KycVerify where IsBankVerified='R' and Formno='" & Val(lbl.Text) & "' "
                    dtcheck = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strsql).Tables(0)
                    'If Convert.ToInt32(dtcheck.Rows(0)("Cnt") = 0) Then
                    'If RbtVerifi.SelectedValue = "N" Then
                    Remark = "KYC  UnVerify of IdNo:" & LblIdno.Text & ""
                    If Session("CompId") = 1077 Then
                        str = str & "update m_membermaster set panno='' where formno='" & lbl.Text & "'"
                    End If
                    If Session("compid") = "1102" Then
                        Dim strsql1 As String = ""
                        Dim dtcheck1 As DataTable = New DataTable()
                        strsql1 = " select ispancard,doj from m_membermaster where Formno='" & Val(lbl.Text) & "' "
                        dtcheck1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strsql1).Tables(0)
                        If dtcheck1.Rows.Count > 0 Then
                            Dim userDoj As DateTime = CDate(dtcheck1.Rows(0)("doj"))
                            Dim compareDate As DateTime = DateTime.ParseExact("23-01-2026", "dd-MM-yyyy", CultureInfo.InvariantCulture())
                            If Convert.ToString(dtcheck1.Rows(0)("ispancard")) = "Y" AndAlso userDoj.Date >= compareDate Then
                                str = str & "; Update KycVerify Set IsAddress = 'R',IsAddrssverified='R',AddrssVerifyDate=getdate(),AddrssUserid='" & Val(Session("Userid")) & "', " & _
                      "AddrssRemark='" & TxtARemark.Text & "',AddressRejectId='" & DDlREason.SelectedValue & "' where IsAddrssverified<>'R' and formno='" & lbl.Text & "';"
                                str = str & "; Update m_formupload Set IsForm = 'R',ActiveStatus = 'R',FrontSideForm = '' where formno='" & lbl.Text & "';"
                            Else
                                str = str & "; Update KycVerify Set IsAddress = 'R',IsBank = 'R',IsPan = 'R',IsAddrssverified='R', AddrssVerifyDate=getdate(),AddrssUserid='" & Val(Session("Userid")) & "', AddrssRemark='" & TxtARemark.Text & "',AddressRejectId='" & DDlREason.SelectedValue & "' ," & _
                        "IsBankVerified='R' ,BankverifyDate = getdate(),BankUserid='" & Val(Session("Userid")) & "',BankProofRemark='" & TxtARemark.Text & "',BankRejectId='" & DDlREason.SelectedValue & "' " & _
    ",IsPanVerified='R',PanVerifyDate=getdate(),PanUserid='" & Val(Session("Userid")) & "',PanRemarks='" & TxtARemark.Text & "',PanRejectId='" & DDlREason.SelectedValue & "'" & _
     " where IsAddrssverified<>'R'  and  formno='" & lbl.Text & "';"
                                str = str & "; Update m_formupload Set IsForm = 'R',ActiveStatus = 'R',FrontSideForm = '' where formno='" & lbl.Text & "';"
                            End If
                        End If
                    Else
                        str = str & "; Update KycVerify Set IsAddress = 'R',IsBank = 'R',IsPan = 'R',IsAddrssverified='R', AddrssVerifyDate=getdate(),AddrssUserid='" & Val(Session("Userid")) & "', AddrssRemark='" & TxtARemark.Text & "',AddressRejectId='" & DDlREason.SelectedValue & "' ," & _
                        "IsBankVerified='R' ,BankverifyDate = getdate(),BankUserid='" & Val(Session("Userid")) & "',BankProofRemark='" & TxtARemark.Text & "',BankRejectId='" & DDlREason.SelectedValue & "' " & _
    ",IsPanVerified='R',PanVerifyDate=getdate(),PanUserid='" & Val(Session("Userid")) & "',PanRemarks='" & TxtARemark.Text & "',PanRejectId='" & DDlREason.SelectedValue & "'" & _
     " where IsAddrssverified<>'R'  and  formno='" & lbl.Text & "';"
                        If Session("compid") = "1105" Or Session("compid") = "1107" Or Session("compid") = "1108" Or Session("compid") = "1109" Then
                        Else
                            str = str & "; Update m_formupload Set IsForm = 'R',ActiveStatus = 'R',FrontSideForm = '' where formno='" & lbl.Text & "';"
                        End If

                    End If

                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,ImgPath1,Userid,RectimeStamp,Status,Remark,RejectId)" & _
    "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'A',b.AddrProof,b.BackAddressProof,'" & Val(Session("Userid")) & "',Getdate(),2,'" & TxtARemark.Text.Trim & "','" & DDlREason.SelectedValue & "' from M_MemberMaster as a,KycVerify as b" & _
    " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "';"
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status,Remark,RejectId)" & _
 "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'P',b.PanImg,'" & Val(Session("Userid")) & "',Getdate(),2,'" & TxtARemark.Text.Trim & "','" & DDlREason.SelectedValue & "' from M_MemberMaster as a,KycVerify as b" & _
 " where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "';"
                    str = str & "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status,Remark,RejectId)" & _
"select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'B',b.BankProof,'" & Val(Session("Userid")) & "',Getdate(),2,'" & TxtARemark.Text.Trim & "','" & DDlREason.SelectedValue & "' from M_MemberMaster as a,KycVerify as b" & _
" where a.Formno=b.Formno and a.Formno='" & Val(lbl.Text) & "';"
                    If Session("compid") = "1102" Then
                        str = str & "Insert Into TempKycVerify Select *,GetDate(),'" & Val(Session("Userid")) & "' From KycVerify Where FormNo='" & Val(lbl.Text) & "'"
                    End If
                    str = str & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','KYC Verify ','KYC UnVerify','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                    'End If
                    Rejectcnt = Rejectcnt + 1
                    'End If
                End If
                'End If
            Next

            Dim i As Integer = 0
            If str <> "" Then
                i = objDAL.UpdateData(str)
            End If
            If i <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('" & Rejectcnt & " Rejected successfully. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            Else
                scrname = "<SCRIPT language='javascript'>alert('" & Rejectcnt & " UnVerified unsuccessfully. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)


            End If
            DivRemark.Visible = False
            TxtARemark.Text = ""
            BindData()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub BTnUnVerification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTnUnVerification.Click
        Dim scrname As String = ""
        DivRemark.Visible = True
        BtnVerifiy.Enabled = False
        BTnUnVerification.Enabled = False
        'TxtARemark.Attributes.Add("CssClass", "form-control validate[required]")

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