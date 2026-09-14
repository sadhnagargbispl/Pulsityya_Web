Imports System.Data
Imports System.IO

Partial Class OfferImageMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Master / Gallery Master"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
        BindData()
        btnShowRecord.Visible = False
        ' lblView.Visible = False
        'txtSearch.Text = ""
        'ddlSearchFields.SelectedIndex = 0
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            'txtSearch.Text = ""
            btnShowRecord.Visible = False
            'lblView.Visible = False
            If Session("AStatus") = "OK" Then
                BindData()
            End If
        End If
    End Sub

    Protected Sub BindData()
        Dim LblFileType As New Label
        Dim LblVideo As New Label
        Dim i As Integer = 0
        Dim ImgImage As New Image
        Dim Sql As String = "  SElect id,ImgPath as Imagepath,Case when activeStatus='Y' then 'Active' else 'Deactive' end as Status" & _
"  from M_OfferImage "


        dtData = objDAL.GetData(Sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData

    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        If Page.IsPostBack Then
            Dim PId, scrname As String
            Dim GVRw As GridViewRow
            GVRw = CType(sender.Parent.Parent, GridViewRow)
            'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
            PId = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
            Dim Sql As String = "Delete from  M_OfferImage   WHERE Id='" & Val(PId.ToString()) & "' "
            Dim updateEffect As Integer = objDAL.UpdateData(Sql)
            If updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group! ');" & "</SCRIPT>"
            End If
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
            BindData()
            scrname = ""
        End If
    End Sub
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim stts As String = DirectCast(e.Row.FindControl("lblStatus"), Label).Text
            If String.Equals(stts.ToLower(), "deactive") = True Then
                'e.Row.BackColor = Drawing.Color.Red
                'e.Row.Style("background-image") = "images/redback2.jpg"
                For i = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style("color") = "red"
                Next
            End If
        End If
    End Sub


End Class
