Imports System.Data
Imports System.Data.SqlClient
Imports ClosedXML.Excel
Imports System.IO
Partial Class GroceryDetail_DAtewise


    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim conn As New SqlConnection
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("RewardList")
        GvData.DataBind()
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindData(1)
    End Sub
    Public Sub BindData(ByVal PageIndex As Integer)
        lblErr.Text = ""
        lblCount.Text = ""
        Dim scrName As String = ""
        If (ddlsearchtype.SelectedValue = "T") Then
            If txtMemId.Text = "" Then
                scrName = "<SCRIPT language='javascript'>alert('Please Enter Member ID!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                Exit Sub
            End If
        End If

        Try
            Dim Idno As String = "0"
            Dim rankid As String = "0"
            If txtMemId.Text <> "" Then
                Idno = GetFormNo()
            Else
                Idno = "0"
            End If

            If ddllist.SelectedValue <> 0 Then
                rankid = ddllist.SelectedValue
            Else
                rankid = 0
            End If

            Dim Ds As DataSet
            GvData.DataSource = Nothing
            GvData.DataBind()
            '' Exec sp_GroceryAChieverReport_DateWise  '0','S','','',0,1,50000,100
            'Dim prms As SqlParameter() = New SqlParameter(8) {}
            'prms(0) = New SqlParameter("@Formno", Convert.ToString(Idno).ToLower())
            'prms(1) = New SqlParameter("@SearchType", ddlsearchtype.SelectedValue)
            'prms(2) = New SqlParameter("@Fdate", txtFromDate.Text)
            'prms(3) = New SqlParameter("@Tdate", TxtToDate.Text)
            'prms(4) = New SqlParameter("@RankId", rankid)
            'prms(6) = New SqlParameter("@PageIndex", PageIndex)
            'prms(6) = New SqlParameter("@PageSize", Integer.Parse(50000))
            'prms(7) = New SqlParameter("@IsExport", "N")
            'prms(8) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            'Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GroceryAChieverReport_DateWise", prms)

            Dim str As String = "Exec sp_GroceryAChieverReport_DateWise  '" & Convert.ToString(Idno).ToLower() & "','" & ddlsearchtype.SelectedValue & "',"
            str &= " '" & txtFromDate.Text & "','" & TxtToDate.Text & "'," & rankid & "," & ddltype.SelectedValue & "," & PageIndex & ",10000000,'N',100"
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()
            Session("RewardList") = Ds.Tables(0)

        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try
    End Sub
    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMemId.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMemId.Text = ""
        End If
        Return formno
    End Function
    Private Sub FillReward()
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim S As String = ""
        S = "Select * From (Select 0 As Rewardid, '-- ALL --' As Reward Union ALL" & _
     " select rankid as Rewardid,Reward from M_GroceryMaster Where ActiveStatus='Y' ) as Temp "
        Dim dt As New DataTable
        dt = objDAL.GetData(S)
        ddllist.DataSource = dt
        ddllist.DataTextField = "Reward"
        ddllist.DataValueField = "rewardid"
        ddllist.DataBind()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                FillReward()
            Else
                Response.Redirect("logout.aspx")
            End If
        End If
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        lblErr.Text = ""
        lblCount.Text = ""
        Try
            Dim scrName As String = ""
            If (ddlsearchtype.SelectedValue = "T") Then
                If txtMemId.Text = "" Then
                    scrName = "<SCRIPT language='javascript'>alert('Please Enter Member ID!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                    Exit Sub
                End If
            End If

            Dim Idno As String = "0"

            Dim rankid As String = "0"
            If txtMemId.Text <> "" Then
                Idno = GetFormNo()
            Else
                Idno = "0"
            End If
            If ddllist.SelectedValue <> 0 Then
                rankid = ddllist.SelectedValue
            Else
                rankid = 0
            End If
            Dim Ds As DataSet
            'Dim prms As SqlParameter() = New SqlParameter(5) {}
            'prms(0) = New SqlParameter("@Idno", Convert.ToString(Idno).ToLower())
            'prms(1) = New SqlParameter("@RankId", rankid)
            'prms(2) = New SqlParameter("@PageIndex", 1)
            'prms(3) = New SqlParameter("@PageSize", Integer.Parse(50000))
            'prms(4) = New SqlParameter("@IsExport", "Y")
            'prms(5) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            'Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GroceryAChieverReport", prms)
            Dim str As String = "Exec sp_GroceryAChieverReport_DateWise  '" & Convert.ToString(Idno).ToLower() & "','" & ddlsearchtype.SelectedValue & "',"
            str &= " '" & txtFromDate.Text & "','" & TxtToDate.Text & "'," & rankid & "," & ddltype.SelectedValue & ",1,10000000,'Y',100"
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            Session("RewardList") = Ds.Tables(0)
            ExportExcel()
        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")

        End Try
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("RewardList")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "GroceryDetail")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=Grocery.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using
    End Sub

    Protected Sub ddlsearchtype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlsearchtype.TextChanged
        If Session("CompId") = "1010" Then
            If ddlsearchtype.SelectedValue = "S" Then
                Div2.Visible = False
            Else
                Div2.Visible = True
            End If

        End If
    End Sub
End Class

