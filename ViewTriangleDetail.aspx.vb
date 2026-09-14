Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewTriangleDetail
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim Idno As String
    Dim PanNo As String
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
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim scrname As String
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Dim ReqNo As String
        If String.IsNullOrEmpty(Request("Idno")) = False And String.IsNullOrEmpty(Request("PanNo")) = False Then
            Idno = Request("IdNo")
            PanNo = Request("PanNo")

        End If
        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("IdNo")) = False And String.IsNullOrEmpty(Request("PanNo")) = False Then
                    'LblNo.Text = " Request No :" & ReqNo
                    BindData()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
    End Sub
    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim MemberId As String
        Dim formno As String = ""
        MemberId = Idno
        MemberId = MemberId.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & MemberId & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formNo = dt.Rows(0)("FormNo")

        End If
        Return formNo
    End Function

    Public Sub BindData(Optional ByVal SrchCond As String = "")
        Try

            Dim Formno As String = ""
            Formno = GetFormNo()
            ' Dim sql As String = "select Formno,ReqNo,Case when Status='R' then RejectRemark else Remarks end as Remarks from TrnPinReqMain where ReqNo='" & ReqNo & "'" & SrchCond
            Dim sql As String = " select c.Idno as SponsorId,(c.MemFirstName+ ''+c.MemLastName) as SponsorName,a.Idno as DirectId," & _
            " (a.MemFirstname+' '+a.MemLastname) as DirectName,b.Kitname as PackageName,b.Kitamount as PackageAmount,b.Bv as PackageBv,Case when a.LegNo='1' then 'Left' else 'Right' end As LegNo," & _
            " Replace(Convert(varchar,a.Doj,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.Doj AS TIME),100) as DateOfJoining" & _
            " from M_Membermaster as a,m_kitmaster as b,M_MemberMaster as c where a.KitId=b.Kitid and  a.RefFormno=" & Formno & " and " & _
            "   a.RefFormno=c.Formno and c.ActiveStatus='Y' " & _
            "  and Upper( RTrim(LTrim(a.PanNo)))=Upper('" & PanNo & "') and b.RowStatus='Y' and a.ActiveStatus='Y' "
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
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
   
    Protected Sub BtnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcel.Click
        Try
            Dim dtTemp As New DataTable


            Dim Formno As String = ""
            Formno = GetFormNo()
            ' Dim sql As String = "select Formno,ReqNo,Case when Status='R' then RejectRemark else Remarks end as Remarks from TrnPinReqMain where ReqNo='" & ReqNo & "'" & SrchCond
            Dim str As String = " select c.Idno as SponsorId,(c.MemFirstName+ ''+c.MemLastName) as SponsorName,a.Idno as DirectId," & _
            " (a.MemFirstname+' '+a.MemLastname) as DirectName,b.Kitname as PackageName,b.Kitamount as PackageAmount,b.Bv as PackageBv,Case when a.LegNo='1' then 'Left' else 'Right' end As LegNo," & _
            " Replace(Convert(varchar,a.Doj,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.Doj AS TIME),100) as DateOfJoining" & _
            " from M_Membermaster as a,m_kitmaster as b,M_MemberMaster as c where a.KitId=b.Kitid and  a.RefFormno=" & Formno & " and " & _
            "   a.RefFormno=c.Formno and c.ActiveStatus='Y' " & _
            "  and Upper( RTrim(LTrim(a.PanNo)))=Upper('" & PanNo & "') and b.RowStatus='Y' and a.ActiveStatus='Y' "
            Dim dg As New DataGrid
            Dim Condition As String = ""
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(Str)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("TriangleReportwithDirectId.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub
End Class
