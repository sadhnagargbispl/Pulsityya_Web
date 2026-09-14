Imports System.Data.SqlClient
Imports System.Data
Partial Class M_groupdirectBusinessDatewise
    Inherits System.Web.UI.Page
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim Dt As DataTable
    Dim Ad As SqlDataAdapter
    Dim Obj As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then

                If Request.QueryString.HasKeys Then
                    If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                        txtMemberId.Text = Request.QueryString("key").ToString

                    End If
                End If
            End If

        Else
            Response.Redirect("logout.aspx")
        End If

    End Sub

    


    Private Function GetFormNo() As String
        Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMemberId.Text
        Dim qry As String = "Select FormNo from " & Obj.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = Obj.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblError.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblError.Visible = True
            txtMemberId.Text = ""
        End If
        Return formno
    End Function

    Private Sub Filltotal(ByVal Formno As String, ByVal dwnFormno As String)
        Try
            Dim Dt As New DataTable()
            Dim strSql As String = "Exec Sp_MydirectReportDate '" & Formno & "','" & dwnFormno & "','" & TxtFromDate.Text & "','" & TxtToDate.Text & "'"

            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")).ToString(), CommandType.Text, strSql).Tables(0)
            DivSideA.Visible = True
            Session("DirectDownline") = Dt
            Grdtotal.DataSource = Dt
            Grdtotal.DataBind()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub


    Private Sub LoadDownline(ByVal FormNo As String)
        Try
            Dim Dt1 As New DataTable()
            Dim strSql As String = "Exec Sp_MydirectReportNewDate '" & FormNo & "','" & TxtFromDate.Text & "','" & TxtToDate.Text & "'"

            Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")).ToString(), CommandType.Text, strSql).Tables(0)
            DivSideA.Visible = True
            Session("DirectDownline") = Dt1
            DLDirects.DataSource = Dt1
            DLDirects.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('" & ex.Message & "')", True)
        End Try
    End Sub


    Protected Sub PerformData(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim Di As RepeaterItem = CType(CType(sender, ImageButton).NamingContainer, RepeaterItem)
            Dim FormNo As String = CType(Di.FindControl("lblID"), Label).Text
            Dim Formno1 As String = ""
            Formno1 = GetFormNo()
            Filltotal(Formno1.ToString(), FormNo)
            LoadDownline(FormNo)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('" & ex.Message & "')", True)
        End Try
    End Sub
    
    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Formno As String = ""
        Formno = GetFormNo()
        Try
            Session("Data") = Formno
            Session("Data") = Formno ' Add on 30Oct2018

            Filltotal(Formno, Formno.ToString())
            'Filltotal(Session("Formno").ToString(), Session("dwnFormno").ToString())
            LoadDownline(Formno.ToString())

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('" & ex.Message & "')", True)
        End Try
    End Sub
End Class
