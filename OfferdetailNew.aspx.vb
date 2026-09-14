Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class OfferdetailNew
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    'Dim objDAL As New DAL
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
        BindData()
        btnShowRecord.Visible = False
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            btnShowRecord.Visible = False
            If Session("AStatus") = "OK" Then
                BindData()
            End If
        End If
    End Sub
    Public Sub BindData()
        Dim sql As String = "select OfferID,offername,selfbv,DirectBv,replace(convert(varchar,startdate,106),' ','-') as StARTDATE,"
        sql &= " replace(Convert(varchar,enddate,106),' ','-') as EndDate, Case When ActiveStatus='Y' then 'Active' Else 'Deactive' End As Status"
        sql &= " from M_MemberOfferNew order by id desc "
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        If Session("compid") = "1010" Or Session("compid") = "1103" Or Session("compid") = "1108" Then
            GridView1.DataSource = dtData
            GridView1.DataBind()
            DivWellValueUser.Visible = True
            DivAllUser.Visible = False
        Else
            GvData.DataSource = dtData
            GvData.DataBind()
            DivWellValueUser.Visible = False
            DivAllUser.Visible = True
        End If
        Session("GData") = dtData
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataSource = Session("GData")
        GridView1.DataBind()
    End Sub
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim OfferID As String = GvData.DataKeys(e.Row.RowIndex).Value.ToString()
            Dim GrpId As LinkButton
            Dim LblDeacTive As Label
            If OfferID <> "" Then
                GrpId = DirectCast(e.Row.Cells(7).FindControl("LBDelete"), LinkButton)
                If String.Equals(e.Row.Cells(5).Text.ToLower(), "deactive") = True Then
                    GrpId.Visible = False
                Else
                    GrpId.Visible = True
                End If

                '((LinkButton)e.Row.Cells[15].FindControl("lnkbtnresend")).Enabled = false;
                'e.Row.Cells(8).Visible = True
            Else
                GrpId = DirectCast(e.Row.Cells(8).FindControl("LBDelete"), LinkButton)
                GrpId.Visible = False
                '  e.Row.Cells(8).Visible = False
            End If
        End If

    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim OfferID As String = GridView1.DataKeys(e.Row.RowIndex).Value.ToString()
            Dim GrpId As LinkButton
            Dim LblDeacTive As Label
            If OfferID <> "" Then
                GrpId = DirectCast(e.Row.Cells(7).FindControl("LBDelete"), LinkButton)
                If String.Equals(e.Row.Cells(5).Text.ToLower(), "deactive") = True Then
                    GrpId.Visible = False
                Else
                    GrpId.Visible = True
                End If

                '((LinkButton)e.Row.Cells[15].FindControl("lnkbtnresend")).Enabled = false;
                'e.Row.Cells(8).Visible = True
            Else
                GrpId = DirectCast(e.Row.Cells(8).FindControl("LBDelete"), LinkButton)
                GrpId.Visible = False
                '  e.Row.Cells(8).Visible = False
            End If
        End If

    End Sub
    Protected Sub DeleteGroup1(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim OfferID, scrname As String
        Dim dt As DataTable
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        OfferID = DirectCast(GVRw.FindControl("Lbloffer"), Label).Text
        Dim str As String = "Select * from M_MemberOfferNew where OfferID='" & Val(OfferID.ToString()) & "' "
        dt = New DataTable
        dt = objDAL.GetData(str)
        If dt.Rows.Count > 0 Then
            Dim Sql As String = "Update M_MemberOfferNew SET ActiveStatus='N' WHERE OfferID='" & Val(OfferID.ToString()) & "'"

            'objDAL = New DAL
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim updateEffect As Integer = objDAL.UpdateData(Sql)
            If updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Deactive Successfully!');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Not able to Deactivate the selected Offer! ');" & "</SCRIPT>"
            End If
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
            BindData()
        Else
            scrname = "<SCRIPT language='javascript'>alert('This Record Can Not Be Deleted!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
        End If
    End Sub
    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim OfferID, scrname As String
        Dim dt As DataTable
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        OfferID = DirectCast(GVRw.FindControl("Lbloffer"), Label).Text
        Dim str As String = "Select * from M_MemberOfferNew where OfferID='" & Val(OfferID.ToString()) & "' "
        dt = New DataTable
        dt = objDAL.GetData(str)
        If dt.Rows.Count > 0 Then
            Dim Sql As String = "Update M_MemberOfferNew SET ActiveStatus='N' WHERE OfferID='" & Val(OfferID.ToString()) & "'"

            'objDAL = New DAL
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim updateEffect As Integer = objDAL.UpdateData(Sql)
            If updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Deactive Successfully!');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Not able to Deactivate the selected Offer! ');" & "</SCRIPT>"
            End If
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
            BindData()
        Else
            scrname = "<SCRIPT language='javascript'>alert('This Record Can Not Be Deleted!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
        End If
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.ClearContent()
        Response.Buffer = True
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "VirtualBVDetails.xls"))
        Response.ContentType = "application/ms-excel"
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        If Session("compid") = "1010" Or Session("compid") = "1103" Then
            GridView1.AllowPaging = False
            GridView1.GridLines = GridLines.Both
        Else
            GvData.AllowPaging = False
            GvData.GridLines = GridLines.Both
        End If
        dtData = New DataTable
        dtData = Session("GData")

        'If Session("compid") = "1010" Or Session("compid") = "1091" Then
        If Session("compid") = "1010" Or Session("compid") = "1103" Then
            GridView1.DataSource = dtData
            GridView1.DataBind()
        Else
            GvData.DataSource = dtData
            GvData.DataBind()
        End If

        'BindGridview()
        If Session("compid") = "1010" Or Session("compid") = "1103" Then
            'Change the Header Row back to white color
            GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF")
            'Applying stlye to gridview header cells
            For i As Integer = 0 To GridView1.HeaderRow.Cells.Count - 1
                GridView1.HeaderRow.Cells(i).Style.Add("background-color", "#D8A38D")
            Next

            'Remove modify and Delete columns from grid
            GridView1.HeaderRow.Cells(GridView1.HeaderRow.Cells.Count - 1).Visible = False
            ' GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
            For i As Integer = 0 To GridView1.Rows.Count - 1
                GridView1.Rows(i).Cells(GridView1.HeaderRow.Cells.Count - 1).Visible = False
                '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
            Next

            For i As Integer = 0 To GridView1.Rows.Count - 1
                For j As Integer = 0 To GridView1.Rows(i).Cells.Count - 1
                    GridView1.Rows(i).Cells(j).Style.Add("background-color", "#FFFFFF")
                    GridView1.Rows(i).Cells(j).Style.Add("color", "#000000")
                Next
            Next
            GridView1.RenderControl(htw)
            Response.Write(sw.ToString())
            Response.[End]()
        Else
            'Change the Header Row back to white color
            GvData.HeaderRow.Style.Add("background-color", "#FFFFFF")
            'Applying stlye to gridview header cells
            For i As Integer = 0 To GvData.HeaderRow.Cells.Count - 1
                GvData.HeaderRow.Cells(i).Style.Add("background-color", "#D8A38D")
            Next

            'Remove modify and Delete columns from grid
            GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            ' GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
            For i As Integer = 0 To GvData.Rows.Count - 1
                GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
                '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
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
        End If
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
End Class
