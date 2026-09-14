Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class MemberCountFranchiseWise
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
                'Session("PageName") = "Member / Update Member Profile"
                '' UserStatus.InnerHtml = "<p>Welcome " & Session("MemName") & "(" & Session("FormNo") & ") To" & Session("CompName") & " </p>"

            Else
                Response.Redirect("logout.aspx")
            End If

            If Not Page.IsPostBack Then
                FillCityPinDetail()

                ''FillReport()
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillCityPinDetail()
        Try

            Dim sql As String = String.Empty
            sql = " Select StateCode,StateNAme from M_StateDivMaster   "
            sql &= " Where ActiveStatus = 'Y'  Order By StateNAme"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            ddlstate.DataSource = dt
            ddlstate.DataTextField = "StateNAme"
            ddlstate.DataValueField = "StateCode"
            ddlstate.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillReport()
        Try

            Dim sql As String = String.Empty
            'sql = " Select a.IDno,b.MemFirstName,a.stateName,replace(Convert(varchar,ractimestamp,106),' ','-') as Date   "
            'sql &= " from M_Makefranchise a With(nolock)"
            'sql &= " Left Join M_MemberMaster  b With(nolock) on a.formno = b.formno"


            'sql = " Select Count(*) cnt ,c.IDno,b.StateNAme,b.StateCode from   "
            'sql &= " M_MemberMaster a inner join M_STateDivMaster  b  on a.stateCode = b.stateCode And b.Activestatus = 'Y'"
            'sql &= " inner join M_Makefranchise c on  c.StateID  = b.stateCode and c.StateID  = a.stateCode"
            'sql &= " Where 1 = 1 "
            'If Val(ddlstate.SelectedValue) > 0 Then
            '    sql &= "  And   c.StateID  =  '" & ddlstate.SelectedValue & "'"
            'End If
            'If (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
            '    sql &= "  And  Cast(a.doj as  date) between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'"
            'End If
            'sql &= "  Group by c.IDno,b.StateNAme,b.StateCode"


            sql = "  Select Sum(cntActive) cnta, Sum(CntD) as CntD,IDno,StateNAme,StateCode From ("
            sql &= " Select Count(*) cntActive  ,0 as CntD, c.IDno,b.StateNAme,b.StateCode "
            sql &= " from    M_MemberMaster a inner join M_STateDivMaster  b  on a.stateCode = b.stateCode "
            sql &= " And b.Activestatus = 'Y' inner join M_Makefranchise c on  c.StateID  = b.stateCode "
            sql &= " and c.StateID  = a.stateCode Where 1 = 1 And A.activeStatus ='Y'  "

            If Val(ddlstate.SelectedValue) > 0 Then
                sql &= "  And   c.StateID  =  '" & ddlstate.SelectedValue & "'"
            End If

            If (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
                sql &= "  And  Cast(a.doj as  date) between '" & txtStartDate.Text & "' and '" & txtEndDate.Text & "'"
            End If
            sql &= " Group by c.IDno,b.StateNAme,b.StateCode"
            sql &= " union All"
            sql &= " Select 0 as  cntActive,Count(*) CntD  ,c.IDno,b.StateNAme,b.StateCode "
            sql &= " from    M_MemberMaster a inner join M_STateDivMaster  b  on a.stateCode = b.stateCode "
            sql &= "  And b.Activestatus = 'Y' inner join M_Makefranchise c on  c.StateID  = b.stateCode "
            sql &= " and c.StateID  = a.stateCode Where 1 = 1  And A.activeStatus ='N'  "
            If Val(ddlstate.SelectedValue) > 0 Then
                sql &= "  And   c.StateID  =  '" & ddlstate.SelectedValue & "'"
            End If

            If (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
                sql &= " And b.KitID  = 2"
            End If
            sql &= " Group by c.IDno,b.StateNAme,b.StateCode) As tt"
            sql &= " Group by IDno,StateNAme,StateCode"



            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            GvData.DataSource = dt
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillReport()
    End Sub
End Class
