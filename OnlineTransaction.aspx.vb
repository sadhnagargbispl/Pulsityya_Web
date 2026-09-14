Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Xml

Partial Class OnlineTransaction
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Master / Online Transaction Detail"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
        BindData()
        btnShowRecord.Visible = False
        lblView.Visible = False
        ' ddlSearchFields.SelectedIndex = 0
        'txtSearch.Text = ""
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                '  txtSearch.Text = ""
                btnShowRecord.Visible = False
                lblView.Visible = False
                If Session("AStatus") = "OK" Then
                    BindData()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindData(Optional ByVal con As String = "")
        Try


            Dim sql As String = ""

            sql = "select Idno,MemberName,Email,b.KitName,a.Kitamount,Replace(Convert(Varchar,a.RectimeStamp ,106),' ','-')+' '+CONVERT(varchar(15),CAST(a.RecTimeStamp AS TIME),100) As " & _
                " TransactionDate,TxnId as TransactionId,isnull(Replace(Convert(Varchar,TransSuccessDate ,106),' ','-')+' '+CONVERT(varchar(15),CAST(TransSuccessDate AS TIME),100) ,'Failed')as " & _
                " SuccessDate,Status,Case when Status='Complete' then 'False' else 'True' end as VisibleStatus," & _
 "'http://gohappy.gohappynetwork.com/Response?txnid='+TxnId  as Clickurl from onlinetransaction " & _
                " as a,M_Kitmaster as b where a.kitid=b.KItid and b.activeStatus='Y' and b.RowStatus='Y' " & con & " order by aid desc"


            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            ViewState("WithDrawDate") = "BankCode"
            ViewState("Sort_Order") = "ASC"
            If dtData.Rows.Count > 0 Then
                btnExport.Enabled = True
                btnPrintAll.Enabled = True
                btnPrintCurrent.Enabled = True
            Else
                btnExport.Enabled = False
                btnPrintAll.Enabled = False
                btnPrintCurrent.Enabled = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim BankCode, scrname As String
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        BankCode = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text


        ' Dim client As New WebClient
        ' Dim data As Stream

        Dim Sql As String = "http://gohappy.gohappynetwork.com/Response?txnid=" + BankCode.Trim + ""
        Dim sResponseFromServer As String = String.Empty
        'clsgen.ErrorLog(Server.MapPath("Logs/ErrorLog"), "Success-1")                    
        Dim tRequest As HttpWebRequest
        Dim dataStream As Stream
        tRequest = HttpWebRequest.Create(Sql)
        Dim tResponse As HttpWebResponse = tRequest.GetResponse()
        dataStream = tResponse.GetResponseStream()
        Dim tReader As StreamReader = New StreamReader(dataStream)
        sResponseFromServer = tReader.ReadToEnd()
        Dim ds As New DataSet()
        ds = convertJsonStringToDataSet(sResponseFromServer)
        scrname = "<SCRIPT language='javascript'>alert('" & ds.Tables(0).Rows(0)(0) & "');" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)

        'Dim DT As New DataSet
        'DT = JsonConvert.DeserializeObject(Of DataSet)(sResponseFromServer.Trim)
        'Dim varses() = Newtonsoft.Json.JsonConvert.DeserializeObject(OfString())(sResponseFromServer)

        ' clsGen.convertJsonStringToDataSet(sResponseFromServer)
        'data = client.OpenRead(Sql)
        'Dim reader As New StreamReader(data)
        'Dim s As String
        's = reader.ReadToEnd()
        'data.Close()
        'reader.Close()


        BindData()
    End Sub
    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        If e.SortExpression = ViewState("WithDrawDate").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/up.png", "~/Images/down.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = e.SortExpression Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/up.png", "~/Images/down.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            End If

        Else
            RebindData(e.SortExpression, "ASC")
        End If
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Dim dt As DataTable = CType(Session("GData"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("WithDrawDate") = sColimnName
        ViewState("Sort_Order") = sSortOrder
    End Sub








    'Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
    '    Dim i As Integer
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        If String.Equals(e.Row.Cells(5).Text.ToLower(), "deactive") = True Then
    '            'e.Row.BackColor = Drawing.Color.Red
    '            e.Row.Style("background-image") = "../Resources/Images/redback2.jpg"
    '            For i = 0 To e.Row.Cells.Count - 1
    '                e.Row.Cells(i).Style("color") = "whitesmoke"
    '            Next
    '        End If
    '    End If
    'End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.ClearContent()
        Response.Buffer = True
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "OnlineTransaction.xls"))
        Response.ContentType = "application/ms-excel"
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
        'BindGridview()
        'Change the Header Row back to white color
        GvData.HeaderRow.Style.Add("background-color", "#FFFFFF")
        'Applying stlye to gridview header cells
        For i As Integer = 0 To GvData.HeaderRow.Cells.Count - 1
            GvData.HeaderRow.Cells(i).Style.Add("background-color", "#D8A38D")
        Next

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        '  GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            '     GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        Next

        For i As Integer = 0 To GvData.Rows.Count - 1
            For j As Integer = 0 To GvData.Rows(i).Cells.Count - 1
                GvData.Rows(i).Cells(j).Style.Add("background-color", "#FFFFFF")
                GvData.Rows(i).Cells(j).Style.Add("color", "#000000")
            Next
        Next
        GvData.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.[End]()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub

    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        'gridview.BorderWidth = "2px"
        GvData.BorderStyle = BorderStyle.Solid
        GvData.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            '   GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
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
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        GvData.BorderStyle = BorderStyle.Solid
        ' gridview.BorderWidth = 
        GvData.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            '   GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
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
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    'Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
    '    GvData.HeaderRow.Cells(0).Visible = False
    '    'For i As Integer = 0 To GvData.Rows.Count - 1
    '    '    GvData.Rows(i).Cells(0).Visible = False

    '    'Next
    'End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim condition As String = ""
        If TxtMemID.Text <> "" Then
            condition = " And a.Idno='" & TxtMemID.Text.Trim & "'"
        End If
        If txtStartDate.Text <> "" Then
            condition = condition & " And Cast(a.Rectimestamp as Date)>='" & txtStartDate.Text & "'"
        End If

        If txtEndDate.Text <> "" Then
            condition = condition & " And Cast(a.Rectimestamp as Date)<='" & txtEndDate.Text & "'"
        End If
        If RbReqStatus.SelectedValue = "A" Then
            condition = condition
        ElseIf RbReqStatus.SelectedValue = "Complete" Then
            condition = condition & " and a.status='Complete'"
        Else
            condition = condition & " and a.status='' and TransSuccessDate is null"


        End If
        BindData(condition)
    End Sub
    Public Function convertJsonStringToDataSet(ByVal jsonString As String) As DataSet
        Dim xd As XmlDocument = New XmlDocument()
        jsonString = "{ ""rootNode"": {" & jsonString.Trim().TrimStart("{"c).TrimEnd("}"c) & "} }"
        xd = CType(JsonConvert.DeserializeXmlNode(jsonString), XmlDocument)
        Dim ds As DataSet = New DataSet()
        ds.ReadXml(New XmlNodeReader(xd))
        Return ds
    End Function
End Class
