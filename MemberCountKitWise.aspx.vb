Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class MemberCountKitWise
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

                FillReport()
                FillTotal()
            Else

                lblTotalCount.Text = "0.00"
            End If
           

        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillCityPinDetail()
        Try

            Dim sql As String = String.Empty
            sql = " Select * from (   "
            sql &= " Select  KitID,KitName from M_KitMaster Where ActiveStatus = 'Y' And Rowstatus = 'Y'  And KitID  <> 1"
            sql &= " Union All"
            sql &= " Select 0,'---Select Kit---') As RR Order by KitID Asc"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            ddlstate.DataSource = dt
            ddlstate.DataTextField = "KitName"
            ddlstate.DataValueField = "KitID"
            ddlstate.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillReport()
        Try

            Dim sql As String = String.Empty


            'sql = " Select * from ("
            'sql &= " Select b.KitID,  b.KitNAme, a.Cnt As Cnt "
            'sql &= " from V#KitCount a  inner Join M_kitMAster as b on A.kitId  = b.KitID "
            'sql &= " And b.KitID <> 1  "
            'sql &= " Where  1= 1 "
            'If (txtStartDate.Text <> "" Or txtEndDate.Text <> "") Then
            '    sql &= "  And  a.BillDate  between  '" & txtStartDate.Text & "' And  '" & txtEndDate.Text & "' "
            'End If

            'If Val(ddlstate.SelectedValue) > 0 Then
            '    sql &= "  And   b.KitID   =  '" & ddlstate.SelectedValue & "'"
            'End If

            'sql &= " ) As  RR"
            'sql &= " order By KitID  Asc "

            sql = "Exec Sp_GetMemberKitWise '" & ddlstate.SelectedValue & "', '" & txtStartDate.Text & "', '" & txtEndDate.Text & "'"



            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
           
            GvData.DataSource = dt
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillReport()
        FillTotal()
    End Sub
    Private Sub FillTotal()
        Try
            Dim tmpval As Integer = 0
            For i As Integer = 0 To GvData.Rows.Count - 1
                Dim lblcnt As Label = DirectCast(GvData.Rows(i).FindControl("lblcnt"), Label)
                Dim LblTotalAmount As Label = DirectCast(GvData.Rows(i).FindControl("lblTotalCount"), Label)

                tmpval = tmpval + Integer.Parse(lblcnt.Text)
                lblTotalCount.Text = tmpval.ToString()
            Next
        Catch ex As Exception

        End Try
    End Sub
End Class
