Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_PayoutExportForTally
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral
    Dim objModuleFun As ModuleFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If Not Page.IsPostBack Then
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Session("AStatus") = "OK" Then


                    BindSession()
                    Session("PageName") = "Member / Payout Export For Tally"
                Else
                    Response.Redirect("Logout.aspx")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindSession()
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim sql As String = "Sp_FillSession"
        objModuleFun.FillCombo(sql, ddlsession, "SessnName", "SessID")
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click


        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim dt As DataTable = New DataTable()
        Dim str As String = " Exec Sp_TallyData '" & ddlsession.SelectedValue & "'"
        dt = obj.GetData(str)
        If (dt.Rows.Count > 0) Then
            Session("RewardList") = dt
            Response.ClearContent()
            Response.Buffer = True
            Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "Journal.xls"))
            Response.ContentType = "application/ms-excel"

            Dim sw As New StringWriter()
            Dim htw As New HtmlTextWriter(sw)
            GvData.AllowPaging = False
            GvData.GridLines = GridLines.Both
            Dim dtData As DataTable
            dtData = New DataTable
            dtData = Session("RewardList")
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
            'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
            'For i As Integer = 0 To GvData.Rows.Count - 1
            '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
            'Next

            For i As Integer = 0 To GvData.Rows.Count - 1
                For j As Integer = 0 To GvData.Rows(i).Cells.Count - 1
                    GvData.Rows(i).Cells(j).Style.Add("background-color", "#FFFFFF")
                    GvData.Rows(i).Cells(j).Style.Add("color", "#000000")
                Next
            Next
            GvData.RenderControl(htw)
            Response.Write(sw.ToString())
            Response.[End]()
        End If

       



    End Sub



    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
End Class
