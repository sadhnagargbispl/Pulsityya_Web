Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewIncomeDetail
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim ReqNo As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    'Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
    '    BindData()
    '    btnShowRecord.Visible = False
    '    lblView.Visible = False
    '    ddlSearchFields.SelectedIndex = 0
    '    txtSearch.Text = ""
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim scrname As String
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Dim ReqNo As String

        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("type")) = False Then
                    '   LblNo.Text = " IdNo :" & Request("IdNo")
                    BindData()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
    End Sub
    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = Request("Idno")
        Dim qry As String = "Select FormNo from M_MemberMaster where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            formno = 0
        End If
        Return formno
    End Function
    Public Sub BindData(Optional ByVal SrchCond As String = "")
        Try
            Dim cond As String = ""
            Dim formno As String = ""
            Dim s As String = Request("type")
            Dim s1() As String = s.Split("/")
            '  formno = GetFormNo()

            ' Dim sql As String = "select Formno,ReqNo,Case when Status='R' then RejectRemark else Remarks end as Remarks from TrnPinReqMain where ReqNo='" & ReqNo & "'" & SrchCond
            Dim sql As String = "Exec Sp_IncomeDetail '" & s1(0) & "','" & s1(1) & "','" & s1(3) & "','" & s1(2) & "'"
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            If s1(2) = "D" Then
                GvData.Columns(4).Visible = False
                GvData.Columns(5).Visible = False
                If s1(3) = "PairIncome" Then
                    GvData.Columns(2).Visible = True
                    GvData.Columns(3).Visible = False
                Else
                    GvData.Columns(3).Visible = True
                    GvData.Columns(2).Visible = False

                End If
            Else
                GvData.Columns(2).Visible = False
                GvData.Columns(3).Visible = False
                If s1(3) = "PairIncome" Then
                    GvData.Columns(4).Visible = True
                    GvData.Columns(5).Visible = False
                Else
                    GvData.Columns(5).Visible = True
                    GvData.Columns(4).Visible = False

                End If

            End If

            Session("GData") = dtData
            'If dtData.Rows.Count > 0 Then
            '    btnExport.Enabled = True
            '    btnPrintAll.Enabled = True
            '    btnPrintCurrent.Enabled = True
            'Else
            '    btnExport.Enabled = False
            '    btnPrintAll.Enabled = False
            '    btnPrintCurrent.Enabled = False
            'End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub



End Class
