Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class FundLimit
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" And Session("UserPermission") = 1 Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

                BindData()
            End If
        End If
    End Sub

    Protected Sub cleAR()
        TxtWalletDeduct.Text = ""
        TxtWalletLimit.Text = ""
        TxtWithdraLimit.Text = ""
        TxtWithDrawDedution.Text = ""
        txtmaxWithdrawllimit.Text = ""
    End Sub
    Public Sub BindData()
        cleAR()
        ''Dim sql As String = "Select CTypeId,Cast(CTypeId as varchar) as VCTypeId,CType,a.Remarks,CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status,B.UserName,a.ToUserEmail as UserEmail From " + objDAL.tblCTypeMaster + " as a,M_UserMaster as b Where a.TouserId=b.UserId and b.ActiveStatus='Y' and b.RowStatus='Y' and a." + objDAL.activeCondition + " Order by CType"
        Dim sql As String = "select MinWithDrawl as [Minimum Withdrawl Limit],MaxWithdrawl as [Maximum Withdrawl Limit],WithDrawlDeduction as  [Withdrawl Deduction]" & _
                "  from FundLimit where activeStatus='Y' and RowStatus='Y'"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData

    End Sub


    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
        ' Session("index") = GvCat.PageIndex
    End Sub

    

    
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim sql As String = ""
            Dim scrname As String = ""
            sql = "exec sp_insertFundLimit 0, '" & Val(TxtWithdraLimit.Text) & "','" & Val(txtmaxWithdrawllimit.Text) & "','" & Val(TxtWithDrawDedution.Text) & "','" & Val(TxtWalletLimit.Text) & "','" & Val(TxtWalletDeduct.Text) & "','" & Val(Session("userid")) & "'"
            Dim updateEffect As Integer = 0
            updateEffect = objDAL.UpdateData(sql)
            If updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully!');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('UnSuccessfully! ');" & "</SCRIPT>"
            End If
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
            BindData()
        Catch ex As Exception

        End Try
    End Sub
End Class
