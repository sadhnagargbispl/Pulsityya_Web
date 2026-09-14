Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel

Partial Class App_UI_Application_Pages_UploadKYCReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim searchtext As String = Session("Search")
        If Not Page.IsPostBack Then
            GvData.Visible = False
            gvContainer.Visible = False
            Session("PairData") = Nothing

            ' FillKit()
        End If
    End Sub




    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = Trim(TxtMember.Text)
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            TxtMember.Text = ""
        End If
        Return formno
    End Function

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        Try
            GvData.PageIndex = e.NewPageIndex
            GvData.DataSource = Session("PairData")
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    'Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
    '    Dim i As Integer
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        If String.Equals(e.Row.Cells(30).Text.ToLower(), "deactive") = True Then
    '            'e.Row.BackColor = Drawing.Color.Red
    '            e.Row.Style("background-image") = "../Resources/Images/redback2.jpg"
    '            For i = 0 To e.Row.Cells.Count - 1
    '                e.Row.Cells(i).Style("color") = "whitesmoke"
    '            Next
    '        End If
    '    End If
    'End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Dim dt As DataTable = Session("PairData")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "KYCReport")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=KYCReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using


    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try


            lblErr.Text = ""
            lblCount.Text = ""
            Dim Condition As String = ""

            Dim Condition1 As String = ""
            Dim qry As String = ""
            Dim formno As String
            If RbtSearch.SelectedValue <> "A" Then
                Condition = Condition & " And e.ActiveStatus='" & RbtSearch.SelectedValue & "'  "
            End If

            If ChkMember.Checked Then
                formno = GetFormNo()
                Condition = " and a.Formno=" & formno & ""
            End If
            If RbtSummary.SelectedValue = "S" Then
                If DDlSerchBy.SelectedValue = "Y" Then
                    qry = "select   IdNo,(MemFirstName)as [Member Name],IsNull(Replace(Convert(Varchar,PanVerifyDate,106),' ','-')+ ' '+CONVERT(varchar(15),CAST(PanVerifyDate AS TIME),100),'') as [PAN Verified Date]," & _
                        " IsNull(b.UserName,'') as [Pan Verify By],IsNull(Replace(Convert(Varchar,AddrssVerifyDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(AddrssVerifyDate AS TIME),100),'') as [Address Proof Verified Date]," & _
                         " IsNull(C.UserName,'') as [AddressProof Verify By],IsNull(Replace(Convert(Varchar,BankVerifyDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(BankVerifyDate AS TIME),100),'') as [Bank Proof Verified Date]," & _
                      " IsNull(d.UserName,'') as [BankProof Verify By] ,e.Mobl as [Mobile No]  from M_memberMaster as e, KycVerify as a Left Join M_UserMaster as b on a.PanUserId=b.UserId and b.RowStatus='Y'" & _
                      " Left Join M_UserMaster as c on a.AddrssUserId=c.UserId and c.RowStatus='Y' Left Join M_UserMaster as d on a.BankUserId=d.UserId and d.RowStatus='Y'" & _
                       " where e.Formno=a.Formno and (IsPanVerified='Y' or IsAddrssVerified='Y' or IsBankVerified='Y') " & Condition & ""
                ElseIf DDlSerchBy.SelectedValue = "R" Then

                    qry = "select   IdNo,(MemFirstName)as [Member Name],IsNull(Replace(Convert(Varchar,PanVerifyDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(PanVerifyDate AS TIME),100),'') as [PanCard Rejected Date],a.PanRemarks as [PanCard Remark]," & _
                         " IsNull(b.UserName,'') as [PanCard Rejected By],IsNull(Replace(Convert(Varchar,AddrssVerifyDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(AddrssVerifyDate AS TIME),100),'') as [Address Proof Rejected Date],a.AddrssRemark as [Address Proof Remark]," & _
                          " IsNull(C.UserName,'') as [AddressProof Rejected By],IsNull(Replace(Convert(Varchar,BankVerifyDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(BankVerifyDate AS TIME),100),'') as [Bank Proof Rejected Date],a.BankProofRemark as [Bank Proof Remark]," & _
                       " IsNull(d.UserName,'') as [BankProof Rejected By],e.Mobl as [Mobile No]   from M_memberMaster as e,KycVerify as a Left Join M_UserMaster as b on a.PanUserId=b.UserId and b.RowStatus='Y'" & _
                       " Left Join M_UserMaster as c on a.AddrssUserId=c.UserId and c.RowStatus='Y' Left Join M_UserMaster as d on a.BankUserId=d.UserId and d.RowStatus='Y'" & _
                        " where e.Formno=a.Formno and (IsPanVerified='R' or IsAddrssVerified='R' or IsBankVerified='R')" & Condition & ""

                ElseIf DDlSerchBy.SelectedValue = "N" Then

                    qry = "select   IdNo,(MemFirstName)  " & _
                    " + ' :- ' +" & _
                    " CAse When (AddrProof = '' Or BackAddressProof = '') Then 'Add Proof' Else  '' End" & _
                    " +CAse When (BankProof = '') Then ',Bank Proof' Else  '' End " & _
                    " +CAse When (PanImg  ='' ) Then ',Pan Kyc' Else  '' End as [Member Name],e.Mobl as [Mobile No]" & _
                    " from M_memberMaster as e, KycVerify as a Left Join M_UserMaster as b on a.PanUserId=b.UserId and b.RowStatus='Y'" & _
                   " Left Join M_UserMaster as c on a.AddrssUserId=c.UserId and c.RowStatus='Y' Left Join M_UserMaster as d on a.BankUserId=d.UserId and d.RowStatus='Y'" & _
                    " where e.Formno=a.Formno and (AddrProof = '' Or BackAddressProof = '' Or   BankProof = '' or PanImg  ='') " & Condition & ""


                ElseIf DDlSerchBy.SelectedValue = "P" Then
                    qry = "select   IdNo,(MemFirstName)as [Member Name],IsNull(Replace(Convert(Varchar,PanVerifyDate,106),' ','-')+ ' '+CONVERT(varchar(15),CAST(PanVerifyDate AS TIME),100),'') as [PAN Verified Date]," & _
                      " IsNull(b.UserName,'') as [Pan Verify By],IsNull(Replace(Convert(Varchar,AddrssVerifyDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(AddrssVerifyDate AS TIME),100),'') as [Address Proof Verified Date]," & _
                       " IsNull(C.UserName,'') as [AddressProof Verify By],IsNull(Replace(Convert(Varchar,BankVerifyDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(BankVerifyDate AS TIME),100),'') as [Bank Proof Verified Date]," & _
                    " IsNull(d.UserName,'') as [BankProof Verify By] ,e.Mobl as [Mobile No]    from M_memberMaster as e, KycVerify as a Left Join M_UserMaster as b on a.PanUserId=b.UserId and b.RowStatus='Y'" & _
                    " Left Join M_UserMaster as c on a.AddrssUserId=c.UserId and c.RowStatus='Y' Left Join M_UserMaster as d on a.BankUserId=d.UserId and d.RowStatus='Y'" & _
                     " where e.Formno=a.Formno and (AddrProof <> '' Or BackAddressProof <> '' Or   BankProof <> '' or PanImg  <>'') And IsAddrssverified='N'" & Condition & ""
                End If


            Else
                If DDlSerchBy.SelectedValue = "Y" Then
                    Condition = Condition & " and a.Status=1"
                Else
                    Condition = Condition & " and a.Status=2"

                End If
                Condition1 = "and a.Status="
                qry = "select Idno,MemName as Name,Case when Type='A' then 'Address Proof' when Type='P' then" & _
                " 'PAN Card' when Type='B' then 'Bank Proof' end as Type," & _
                " ImgPath,Imgpath1,b.UserName,Case when Status=1 then 'Verify' else 'Rejected' end as Status," & _
                " isnull(c.Reason,'')As RejectReason,Remark as RejectRemark,IsNull(Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.RectimeStamp AS TIME),100),'') as Date from kycHistory as a Left Join M_UserMaster as b On " & _
                " a.Userid=b.Userid Left Join M_KycReject as c on a.Rejectid=c.Kid where 1=1 " & Condition & " Order by a.RectimeStamp Desc"


            End If




            dtData = New DataTable
            dtData = objDAL.GetData(qry)
            If dtData.Rows.Count > 0 Then
                If RbtSummary.SelectedValue = "S" Then
                    GvData.DataSource = dtData
                    GvData.DataBind()
                    GvData.Visible = True
                    GridView1.Visible = False
                Else
                    GridView1.DataSource = dtData
                    GridView1.DataBind()
                    GridView1.Visible = True
                    GvData.Visible = False
                End If

                Session("PairData") = dtData

                gvContainer.Visible = True
                lblCount.Text = "Total : " & dtData.Rows.Count
            Else
                gvContainer.Visible = False
                lblErr.Text = "No Record Found!!"
            End If
        Catch ex As Exception

        End Try

    End Sub

    'Protected Sub ddlSearchFields_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSearchFields.SelectedIndexChanged
    '    If String.Equals(ddlSearchFields.SelectedValue.ToLower(), "dateofjoining") = True Then
    '        lblStart.Text = "Enter Start Date : "
    '        lblStart.Visible = True
    '        lblEnd.Visible = True
    '        txtStart.Visible = True
    '        txtEnd.Visible = True
    '    Else
    '        lblStart.Text = "Enter value : "
    '        lblStart.Visible = False
    '        txtStart.Visible = True
    '    End If
    'End Sub

    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("PairData")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        'gridview.BorderWidth = "2px"
        GvData.BorderStyle = BorderStyle.Solid
        GvData.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        Next

        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)

        GvData.RenderControl(hw)

        Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

        Dim sb As New StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload = new function(){")

        sb.Append("var printWin = window.open('', '', 'left=0")

        sb.Append(",top=0,width=1000,height=600,status=0');")

        sb.Append("printWin.document.write(""")

        sb.Append(gridHTML)

        sb.Append(""");")

        sb.Append("printWin.document.close();")

        sb.Append("printWin.focus();")

        sb.Append("printWin.print();")

        sb.Append("printWin.close();};")

        sb.Append("</script>")

        ClientScript.RegisterStartupScript(Me.GetType(), "GridPrint", sb.ToString())

        GvData.AllowPaging = True
        GvData.PagerSettings.Visible = True
        dtData = New DataTable
        dtData = Session("PairData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click

        Try


            GvData.AllowPaging = False
            GvData.GridLines = GridLines.Both
            dtData = New DataTable
            dtData = Session("PairData")
            GvData.DataSource = dtData
            GvData.DataBind()
            GvData.PagerSettings.Visible = False
            GvData.BorderStyle = BorderStyle.Solid
            ' gridview.BorderWidth = 
            GvData.BorderColor = Drawing.Color.Black

            'Remove modify and Delete columns from grid
            GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
            For i As Integer = 0 To GvData.Rows.Count - 1
                GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
                GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
            Next

            Dim sw As New StringWriter()

            Dim hw As New HtmlTextWriter(sw)

            GvData.RenderControl(hw)

            Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

            Dim sb As New StringBuilder()

            sb.Append("<script type = 'text/javascript'>")

            sb.Append("window.onload = new function(){")

            sb.Append("var printWin = window.open('', '', 'left=0")

            sb.Append(",top=0,width=1000,height=1000,status=0');")

            sb.Append("printWin.document.write(""")

            sb.Append(gridHTML)

            sb.Append(""");")

            sb.Append("printWin.document.close();")

            sb.Append("printWin.focus();")

            sb.Append("printWin.print();")

            sb.Append("printWin.close();};")

            sb.Append("</script>")

            ClientScript.RegisterStartupScript(Me.[GetType](), "GridPrint", sb.ToString())

            GvData.AllowPaging = True
            GvData.PagerSettings.Visible = True
            dtData = New DataTable
            dtData = Session("PairData")
            GvData.DataSource = dtData
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub



    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            GridView1.PageIndex = e.NewPageIndex
            GridView1.DataSource = Session("PairData")
            GridView1.DataBind()
        Catch ex As Exception

        End Try
    End Sub
End Class
