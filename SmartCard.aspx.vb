Imports System.Data
Imports System.Data.SqlClient
Partial Class SmartCard
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
        FillData()
    End Sub
    Protected Sub FillData()
        Try


            'conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            'conn.Open()
            Dim formno As String = "0"

            Dim Status As String = ""
            Dim coupon As String = "0"
            Dim condition As String = ""
         
            If txtMemId.Text <> "" Then
                formno = txtMemId.Text
            Else
                formno = "0"
            End If
            If TxtCoupon.Text <> "" Then
                coupon = TxtCoupon.Text
            Else
                coupon = "0"
            End If
            If ddllist.SelectedValue <> "A" Then
                Status = ddllist.SelectedValue
            Else
                Status = "A"
            End If
            
            Dim qry1 As String = ""
            qry1 = "Exec sp_GetsmartGouponAsmin '" & formno & "','" & coupon & "','" & Status & "' "



            dtData = New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dtData = objDAL.GetData(qry1)

            GvData.DataSource = dtData
            GvData.DataBind()
            Session("RewardList") = dtData
        Catch ex As Exception

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

    


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                ' FillReward()
            Else
                Response.Redirect("logout.aspx")
            End If
        End If
    End Sub


    'Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
    '    Try
    '        Dim dtTemp As New DataTable
    '        Dim dg As New DataGrid
    '        Dim Condition As String = ""
    '        Dim formno As String = ""
    '        Dim scrName As String = ""
    '        '     Dim condition As String = ""
    '        Dim condition1 As String = ""
    '        Dim condition2 As String = ""
    '        If Chkmemid.Checked Then
    '            formno = GetFormNo()
    '            Condition = Condition & " And b.Formno='" & Val(formno) & "'"
    '        End If
    '        If ddllist.SelectedValue <> 0 Then
    '            Condition = Condition & " And b.Rewardid='" & ddllist.SelectedValue & "'"
    '        End If
    '        If txtFromDate.Text <> "" Then
    '            condition1 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)>='" & txtFromDate.Text & "'"
    '        End If
    '        If TxtToDate.Text <> "" Then
    '            condition2 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)<='" & TxtToDate.Text & "'"
    '        End If
    '        Dim qry1 As String = ""
    '        qry1 = "select a.Idno,a.MemFirstname as MemberName,b.Reward as Rewardname," & _
    '       " Replace(Convert(Varchar,b.RectimeStamp,106),' ','-') as  AchieveDate , Case when c.Rdays>=b.Daycnt then 'Time Limit' Else 'No Time Limit' End as AchievedReward" & _
    '      " from M_Membermaster as a,M_RewardFinal as b,M_RewardMaster as c where c.Rewardid=b.Rewardid and   a.Formno=b.Formno " & Condition & " " & _
    '      " " & condition1 & " " & condition2 & " "
    '        dtTemp = New DataTable
    '        dtTemp = objDAL.GetData(qry1)

    '        dg.DataSource = dtTemp
    '        dg.DataBind()
    '        ExportToExcel("Reward.xls", dg)

    '    Catch ex As Exception
    '        Response.Write(ex.Message & "Error In Exporting File")

    '    End Try
    'End Sub
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
