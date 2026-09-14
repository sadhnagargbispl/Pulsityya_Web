Imports System.Data.SqlClient
Imports System.Data
Partial Class Mission365view
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim ReqNo As String
    Dim paysessn As String

    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim scrname As String
            objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            'Dim ReqNo As String
            If String.IsNullOrEmpty(Request("IdNo")) = False Then
                ReqNo = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("IdNo")))

            End If
            If String.IsNullOrEmpty(Request("payoutno")) = False Then
                paysessn = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("payoutno")))

            End If
            If Not Page.IsPostBack Then
                
                If Session("AStatus") = "OK" Then
                    If String.IsNullOrEmpty(Request("IdNo")) = False Then
                        'LblNo.Text = " Request No :" & ReqNo
                        BindData()
                    End If
                Else
                    scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                End If
            End If
        Catch ex As Exception

        End Try


    End Sub
    Public Sub BindData(Optional ByVal SrchCond As String = "")
        Try
            Dim cond As String = ""
            Dim formno As String = ""
            formno = GetFormNo()

            ' Dim sql As String = "select Formno,ReqNo,Case when Status='R' then RejectRemark else Remarks end as Remarks from TrnPinReqMain where ReqNo='" & ReqNo & "'" & SrchCond
            'Dim sql As String = " select d.Sessid,d.FromDate,d.ToDate, b.Idno,(b.MemFirstname+' '+b.MemlastName)as Memname,C.Legno,a.bonus " & _
            '   " from V#InfinityTeamId as a,M_MemberMaster as b,V#payoutDetail as d, M_memTreeRelation as c where " & _
            '   " a.Formnodwn=b.Formno  and a.Formno='" & Val(formno) & "' and d.Sessid='" & Request("Sessid") & "' and  c.Formno = a.Formno And c.FormnoDwn = a.FormNoDwn And " & _
            '   " a.FormnoDwn = d.Formno   order by d.Sessid,c.Legno"
            'Dim sql As String = "select a.*,b.idno from Mission365inc as a inner join M_membermaster as b on a.formno=b.formno where  b.idno=" & Request("idno") & " and a.comm='" & Request("comm") & "'"
            Dim sql As String = "select a.*,b.idno,(b.MemfirstName+''+b.MemLastname) as MemberName from Mission365inc as a inner join M_membermaster as b on a.formnoDwn=b.formno where  a.formno=" & formno & " and paysessn='" & Request("PayoutNO") & "' group by a.sessid,b.idno,a.formno,a.formnodwn,a.pairincome,a.slab,a.comm,a.paysessn,b.MemfirstName,b.MemLastname "
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

    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
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
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
End Class
