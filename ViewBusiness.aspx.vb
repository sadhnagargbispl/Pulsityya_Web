Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewBusiness
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim ReqNo As String
    Dim objGen As clsGeneral = New clsGeneral
    Dim Ds As DataSet

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
                If String.IsNullOrEmpty(Request("IdNo")) = False Then
                    LblNo.Text = " IdNo :" & Request("IdNo")
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
    Public Sub BindData()
        Try





            Dim startDate1 As String = Request("fromDate")
            Dim endDate1 As String = Request("Todate")
            Dim Idno As String = Request("Idno")
            Dim type As String = Request("mlevel")
            Dim Condition As String = ""
            Dim startDate As Date
            Dim endDate As Date
            If startDate1 = "" Then
                startDate = Session("CompDate")
            Else
                startDate = startDate1
            End If
            If endDate1 = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                endDate = endDate1
            End If

            Dim formno As String = GetFormNo()
            If type = "1" Then
                Condition = "And Mlevel=1"
            ElseIf type = "2" Then
                Condition = "and Mlevel>1"
            ElseIf type = "0" Then
                Condition = ""
            End If
            Dim sql As String = ""
            'If type = "N" Then
            sql = "select Row_Number() Over(Order by c.Idno  ) As SNo,c.Idno,C.Memfirstname+' '+c.MemlastName as MemberName," & _
            " d.TotalBusiness from M_MemberMaster as c,(select b.formno as Formno,sum(Kitamount)as TotalBusiness from R_MemtreeRelation " & _
            " as a,  Repurchincome as b,M_kitmaster as c    where  a.Formnodwn=b.Formno and b.Kitid=c.Kitid and c.RowStatus='Y' " & _
            " " & Condition & " And  CAST(b.Billdate AS DATE)>=Cast('" & Format(startDate, "dd-MMM-yyyy") & "' as date) and " & _
            " CAST(b.Billdate AS DATE)<=Cast('" & Format(endDate, "dd-MMM-yyyy") & "' as date) " & _
            " and a.formno='" & formno & "'  Group by b.formno   )  As d,R_Memtreerelation as e where  c.Formno=d.Formno    " & _
            "  " & Condition & " and c.formno=e.formnodwn and   e.Formno= " & formno & " Order by Idno   "
            '     Else
            '     sql = "select Row_Number() Over(Order by c.Idno  Desc) As SNo,c.Idno,C.Memfirstname+' '+c.MemlastName as MemberName,  " & _
            '    " d.TotalBusiness from M_MemberMaster as c,  " & _
            '    " (select a.formno,sum(Kitamount)as TotalBusiness from R_MemtreeRelation as a,Repurchincome as b,M_kitmaster as c  " & _
            '    " where  a.Formnodwn=b.Formno and b.Kitid=c.Kitid and c.RowStatus='Y' and a.Mlevel=1 Group by a.formno  " & _
            '    " )As d where c.Formno=d.formno and c.Idno=" & Idno & " and " & _
            '    " CAST(b.Billdate AS DATE)>='" & startDate & "' and CAST(b.Billdate AS DATE)<='" & endDate & "'  " & _
            '" Order by Idno  "
            '     End If
            '  Response.Write(sql)
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim dt As New DataTable
            dt = objDAL.GetData(sql)
            ' ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", "<SCRIPT language='javascript'>alert('" + dt + "');" & "</SCRIPT>", False)

            Session("GData") = dt
            GvData.DataSource = dt
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub



End Class
