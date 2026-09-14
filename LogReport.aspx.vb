Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_LogReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "User / Log Report"

        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim searchtext As String = Session("Search")
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            Dim qry1 As String = "Select * From(Select 0 As UserId,'-- ALL --' As UserName Union ALL select UserID,UserName As UserName from M_UserMaster Where ActiveStatus='Y' and RowStatus='Y') As Temp order by UserID "
            objModuleFun.FillCombo(qry1, DDlMember, "UserName", "UserId")
            If RbtUser.SelectedValue = "A" Then



                DDlMember.Visible = True
                txtMember.Visible = False
            Else
                DDlMember.Visible = False
                txtMember.Visible = True
            End If

            GvData.Visible = False
            gvContainer.Visible = False
            Session("LogReport") = Nothing
            If searchtext <> "" Then

            End If

        End If
    End Sub





    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("LogReport")
        GvData.DataBind()
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""

            If ChkMem.Checked Then
                If RbtUser.SelectedValue = "M" Then


                    formno = GetFormNo()
                    Condition = Condition & " And Temp.Idno='" & txtMember.Text & "'"
                Else
                    If DDlMember.SelectedValue <> 0 Then
                        Condition = Condition & " And Temp.UserId='" & DDlMember.SelectedValue & "'"

                    End If

                End If

            End If
            If txtStartDate.Text <> "" Then
                Condition = Condition & " And  Cast(Convert(Varchar,Temp.ChangeDate,106) as Date)>='" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(Varchar,Temp.ChangeDate,106)as Date)<='" & txtEndDate.Text & "'"
            End If
            Dim qry1 As String = ""

            'If RbtUser.SelectedValue = "M" Then
            If Session("CompId") = "1011" Then
                qry1 = "select Temp.UserName as SubAdmin,Temp.Idno,Temp.MemName as MemberName,Temp.Changedate,Activity,ModifiedFlds,Pagename  from(" & _
              " select a.Userid,a.AId,c.Idno,b.UserName,(c.MemFirstName+' '+c.MemLastName) as Memname,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15), " & _
        " CAST(a.RecTimeStamp AS TIME),100) as ChangeDate,Activity,ModifiedFlds,Pagename from  M_UserMaster as b,UserHistory as a left Join M_MemberMaster as c on a.MemberId=c.Formno " & _
            " where a.UserId=b.Userid and b.RowStatus='Y' and   a.Userid<>0 " & _
          " Union " & _
     " select a.Userid,a.AId,b.Idno,'' as UserName,(b.MemFirstName+' '+b.MemLastName)as Memname,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15), " & _
        " CAST(a.RecTimeStamp AS TIME),100) as ChangeDate,Activity,ModifiedFlds,Pagename from UserHistory as a, M_MemberMaster as b " & _
        " where a.MemberId=b.Formno   and a.Userid=0  )as Temp  where 1=1 " & Condition & "  order by AId"

            Else
                qry1 = "select Temp.UserName as SubAdmin,Temp.Idno,Temp.MemName as MemberName,Temp.Changedate,Activity,ModifiedFlds,Pagename ,IpAdrs as IPAddress from(" & _
                              " select a.Userid,a.AId,c.Idno,b.UserName,(c.MemFirstName+' '+c.MemLastName) as Memname,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15), " & _
                        " CAST(a.RecTimeStamp AS TIME),100) as ChangeDate,Activity,ModifiedFlds,Pagename,C.Hostip as IPAdrs from  M_UserMaster as b,UserHistory as a left Join M_MemberMaster as c on a.MemberId=c.Formno " & _
                            " where a.UserId=b.Userid and b.RowStatus='Y' and   a.Userid<>0 " & _
                          " Union " & _
                     " select a.Userid,a.AId,b.Idno,'' as UserName,(b.MemFirstName+' '+b.MemLastName)as Memname,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15), " & _
                        " CAST(a.RecTimeStamp AS TIME),100) as ChangeDate,Activity,ModifiedFlds,Pagename,b.Hostip as IPAdrs from UserHistory as a, M_MemberMaster as b " & _
                        " where a.MemberId=b.Formno   and a.Userid=0  )as Temp  where 1=1 " & Condition & "  order by AId"

            End If
           
            'qry1 = " select b.Idno,(MemFirstName+' '+MemLastName) as MemberName,PageName,Activity,ModifiedFlds ,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(a.RecTimeStamp AS TIME),100) as ChangeDate from UserHistory  as a,M_Membermaster as b where a.UserId=b.Formno " & Condition & " order by AId Desc "
            'Else
            'qry1 = "select b.UserName as MemberName,PageName,Activity,ModifiedFlds,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(a.RecTimeStamp AS TIME),100) as ChangeDate from UserHistory  as a,M_Usermaster as b where a.UserId=b.UserId and b.ActiveStatus='Y' and b.RowStatus='Y' " & Condition & " order by AId Desc "
            'End If
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("LogReport.xls", dg)

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

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try

            lblErr.Text = ""
            lblCount.Text = ""
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""

            If ChkMem.Checked Then
                If RbtUser.SelectedValue = "M" Then

                    If txtMember.Text <> "" Then

                        Condition = Condition & " And Temp.Idno='" & txtMember.Text & "'"
                    End If
                Else
                    If DDlMember.SelectedValue <> 0 Then
                        Condition = Condition & " And Temp.UserId='" & DDlMember.SelectedValue & "'"

                    End If
                End If

            End If
            If txtStartDate.Text <> "" Then
                Condition = Condition & " And  Cast(Convert(Varchar,Temp.ChangeDate,106) as Date)>='" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(Varchar,Temp.ChangeDate,106)as Date)<='" & txtEndDate.Text & "'"
            End If
            Dim qry1 As String = ""

            'If RbtUser.SelectedValue = "M" Then
            If Session("Compid") = "1011" Then
                qry1 = "select Temp.UserName as SubAdmin,Temp.Idno,Temp.MemName as MemberName,Temp.Changedate,Activity,ModifiedFlds,Pagename from(" & _
                   " select a.Userid,a.AId,c.Idno,b.UserName,(c.MemFirstName+' '+c.MemLastName) as Memname,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15), " & _
             " CAST(a.RecTimeStamp AS TIME),100) as ChangeDate,Activity,ModifiedFlds,Pagename from  M_UserMaster as b,UserHistory as a left Join M_MemberMaster as c on a.MemberId=c.Formno " & _
                 " where a.UserId=b.Userid and b.RowStatus='Y' and   a.Userid<>0 " & _
               " Union " & _
          " select a.Userid,a.AId,b.Idno,'' as UserName,(b.MemFirstName+' '+b.MemLastName)as Memname,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15), " & _
             " CAST(a.RecTimeStamp AS TIME),100) as ChangeDate,Activity,ModifiedFlds,Pagename from UserHistory as a, M_MemberMaster as b " & _
             " where a.MemberId=b.Formno   and a.Userid=0  )as Temp  where 1=1 " & Condition & "  order by AId"

            Else
                qry1 = "select Temp.UserName as SubAdmin,Temp.Idno,Temp.MemName as MemberName,Temp.Changedate,Activity,ModifiedFlds,Pagename,HostIP as [IP Address] from(" & _
                   " select a.Userid,a.AId,c.Idno,b.UserName,(c.MemFirstName+' '+c.MemLastName) as Memname,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15), " & _
             " CAST(a.RecTimeStamp AS TIME),100) as ChangeDate,Activity,ModifiedFlds,Pagename,c.HostIP from  M_UserMaster as b,UserHistory as a left Join M_MemberMaster as c on a.MemberId=c.Formno " & _
                 " where a.UserId=b.Userid and b.RowStatus='Y' and   a.Userid<>0 " & _
               " Union " & _
          " select a.Userid,a.AId,b.Idno,'' as UserName,(b.MemFirstName+' '+b.MemLastName)as Memname,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15), " & _
             " CAST(a.RecTimeStamp AS TIME),100) as ChangeDate,Activity,ModifiedFlds,Pagename,b.HostIP from UserHistory as a, M_MemberMaster as b " & _
             " where a.MemberId=b.Formno   and a.Userid=0  )as Temp  where 1=1 " & Condition & "  order by AId"

            End If
            'qry1 = " select IsNull(c.UserName,'') as UserName,IsNull(b.Idno,' ') as Idno,IsNull((b.MemFirstName+' '+b.MemLastName),'') as [Member Name],Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(a.RecTimeStamp AS TIME),100) as ChangeDate,Activity,ModifiedFlds,Pagename from UserHistory  as a Right Join  M_MemberMaster as b on a.MemberId=b.Formno Right Join M_Usermaster as c on" & _
            '" a.UserId=c.Userid and c.RowStatus='Y' and (c.Activestatus='Y' Or a.UserId=4) where 1 =1 " & Condition & " order by AId Desc "
            ' qry1 = " select b.Idno,(MemFirstName+' '+MemLastname) as MemberName,PageName,Activity,ModifiedFlds,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(a.RecTimeStamp AS TIME),100) as ChangeDate from UserHistory  as a,M_Membermaster as b where a.UserId=b.Formno " & Condition & " order by AId Desc "
            'Else
            'qry1 = "select b.UserName as MemberName,PageName,Activity,ModifiedFlds,Replace(convert(Varchar,a.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(a.RecTimeStamp AS TIME),100) as ChangeDate from UserHistory  as a,M_Usermaster as b where a.UserId=b.UserId and b.ActiveStatus='Y' and b.RowStatus='Y' " & Condition & " Order by AId Desc "
            'End If
            dtData = New DataTable
            dtData = objDAL.GetData(qry1)
            If dtData.Rows.Count > 0 Then
                GvData.DataSource = dtData
                GvData.DataBind()
                Session("LogReport") = dtData
                GvData.Visible = True
                gvContainer.Visible = True
                lblCount.Text = "Total : " & dtData.Rows.Count
            Else
                GvData.Visible = False
                gvContainer.Visible = False
                lblErr.Text = "No Record Found!!"
            End If

        Catch ex As Exception

        End Try

    End Sub
    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMember.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMember.Text = ""
        End If
        Return formno
    End Function
    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("LogReport")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        'gridview.BorderWidth = "2px"
        GvData.BorderStyle = BorderStyle.Solid
        GvData.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        'For i As Integer = 0 To GvData.Rows.Count - 1
        '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        'Next

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
        dtData = Session("LogReport")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("LogReport")
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
        dtData = Session("LogReport")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub


    Protected Sub RbtUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtUser.SelectedIndexChanged
        If RbtUser.SelectedValue = "A" Then
            DDlMember.Visible = True
            txtMember.Visible = False
        Else
            DDlMember.Visible = False
            txtMember.Visible = True
        End If
    End Sub
End Class
