Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_IDBlockalka
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" And Session("UserPermission") = 1 Then
            Session("PageName") = "Home"

        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            'BindData()
            'BindDataSummary()

            BindData()
        End If
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")

        Dim url As String = String.Empty

        Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri



        url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")


        url = url.ToLower

        Dim sql As String = ""
       
        sql = "Select   A.IDNo,A.MemFirstName As MemName,'" & url & "' as Site," & _
   " IsNull(REPLACE(CONVERT(VARCHAR(11), a.blockdate , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.Mobl As MobileNo," & _
   "  BlockRemark	,BlockDate,A.passw,'' as LgnID,A.panno," & _
" a.ActiveStatus,a.IsBlock From M_MemberMaster As A  " & _
" where isblock='Y' and 1=1 " & Condition & " Order by A.Doj Desc"

        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        For Each Dr As DataRow In dtData.Rows
            Dr("LgnID") = Crypto.Encrypt("uid=" & Dr("IDNo") & "&pwd=" & Dr("Passw"))
        Next
        GvData.DataSource = dtData
        GvData.DataBind()

        Session("MemberData") = dtData
        ViewState("Idno") = "Idno"
        ViewState("Sort_Order") = "ASC"
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("MemberData")
        GvData.DataBind()
    End Sub
    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        If e.SortExpression = ViewState("Idno").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        'Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/Upaarrow.png", "~/Images/DownArrow.png")
                        'tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        'tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = e.SortExpression Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        'Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/Upaarrow.png", "~/Images/DownArrow.png")
                        'tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        'tableCell.Controls.Add(img)
                    End If
                Next
            End If

        Else
            RebindData(e.SortExpression, "ASC")
        End If
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Dim dt As DataTable = CType(Session("MemberData"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("Idno") = sColimnName
        ViewState("Sort_Order") = sSortOrder
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Condition As String = ""
       
        If txtfrmdate.Text <> "" And txttodate.Text <> "" Then
            Condition = "   And  Cast(Convert(Varchar,Cast(Cast(blockdate as Varchar) as DateTime),112) as DateTime) >= Cast('" & txtfrmdate.Text & "' As Date )  And  Cast(Convert(Varchar,Cast(Cast(blockdate as Varchar) as DateTime),112) as DateTime) <= Cast('" & txttodate.Text & "' As Date )"
        End If
        If txtMemberID.Text <> "" Then
            Condition = " and a.IDNO = '" & txtMemberID.Text & "' "

        End If
        BindData(Condition)
    End Sub

    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Me.BindData(1)
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

   
End Class
