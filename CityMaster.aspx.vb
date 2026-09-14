Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class CityMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

        
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Master / State Master"
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
        Try
            BindData()
            btnShowRecord.Visible = False
            lblView.Visible = False
            txtSearch.Text = ""
            ddlSearchFields.SelectedIndex = 0
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
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
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Try
            Dim dt As DataTable = CType(Session("GData"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GvData.DataSource = dt
            GvData.DataBind()

            ViewState("WithDrawDate") = sColimnName
            ViewState("Sort_Order") = sSortOrder
        Catch ex As Exception

        End Try
    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                'GvData.PageIndex = Session("index")
                txtSearch.Text = ""
                btnShowRecord.Visible = False
                lblView.Visible = False
                If Session("AStatus") = "OK" Then
                    BindData()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindData()
        Try
            Dim sql As String = " Exec sp_GetCityBindData "
            'Dim sql As String = " Select a.CityCode,a.CityName, a.DistrictCode,b.DistrictName ,"
            'sql &= "  CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status,"
            'sql &= "Case when a.ActiveStatus='N' then 'label label-danger' else 'label label-success' end as StatusClass"
            'sql &= "  From m_CityStateMaster as a ,M_DistrictMaster as b where a.DistrictCode=b.DistrictCode order by CityCode Desc "
            'sql &= "  Order by DistrictName"


            dtData = New DataTable
            dtData = objDAL.GetData(Sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            ViewState("DistrictCode") = "DistrictCode"
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
        Try
            GvData.PageIndex = e.NewPageIndex
            GvData.DataSource = Session("GData")
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim CityCode, scrname As String

            Dim GVRw As GridViewRow
            GVRw = CType(sender.Parent.Parent, GridViewRow)

            CityCode = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text

            Dim Sql As String = "Update " + objDAL.tblCityStateMaster + " SET ActiveStatus='N',LastModified='De-Activated by " & Session("CityCode") & " at " & DateTime.Now.ToString() & "' WHERE CityCode='" & Val(CityCode.ToString()) & "' "
            Dim updateEffect As Integer = objDAL.UpdateData(Sql)
            If updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group! ');" & "</SCRIPT>"
            End If
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
            BindData()
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        Try
            Dim sql As String
            Dim status As String
            If String.IsNullOrEmpty(txtSearch.Text) Then
            ElseIf String.Equals(ddlSearchFields.SelectedItem.Text.ToLower(), "showall") = True Then
                BindData()
            Else



                If String.Equals(ddlSearchFields.SelectedItem.Text.ToLower(), "status") = True Then
                    If String.IsNullOrEmpty(txtSearch.Text) = False Then
                        If txtSearch.Text.ToLower().Contains("deactive") = True Then
                            status = "N"
                        Else
                            status = "Y"
                        End If
                        'sql = " Exec sp_GetimgSearch "
                        sql = "Select a.CityCode,a.CityName, a.DistrictCode,b.DistrictCode,b.DistrictName," & _
                        " CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive'END AS Status,Case when a.ActiveStatus='N' then 'label label-danger' else 'label label-success' end as StatusClass" & _
                        " From m_CityStateMaster as a ,M_DistrictMaster as b" & _
                        " Where a.DistrictCode=b.DistrictCode  and a.ActiveStatus like '%" + status.ToString() + "%' "
                    End If
                Else
                    sql = "Select a.CityCode,a.CityName, a.DistrictCode,b.DistrictCode,b.DistrictName," & _
                    " CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive'END AS Status,Case when a.ActiveStatus='N' then 'label label-danger' else 'label label-success' end as StatusClass " & _
                        " From m_CityStateMaster as a,M_DistrictMaster as b" & _
                    " Where a.DistrictCode=b.DistrictCode and  " + " a.CityName  like '%" + txtSearch.Text + "%' "


                End If
                dtData = New DataTable
                dtData = objDAL.GetData(sql)
                GvData.DataSource = dtData
                GvData.DataBind()
                Session("GData") = dtData
                btnShowRecord.Visible = True
                lblView.Visible = True
                If dtData.Rows.Count > 0 Then
                    btnExport.Enabled = True
                    btnPrintAll.Enabled = True
                    btnPrintCurrent.Enabled = True
                Else
                    btnExport.Enabled = False
                    btnPrintAll.Enabled = False
                    btnPrintCurrent.Enabled = False
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub




    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub

End Class
