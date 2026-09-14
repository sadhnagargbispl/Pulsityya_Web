Imports System.Data
Partial Class GenerateSeminarTikit
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim quantity As String
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                Session("PageName") = "Epin / Generate Epin"
                FillProgram()
            End If
        End If


    End Sub


    Public Sub FillProgram()
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = String.Empty
            str = "Select * from ("
            str &= " Select MeetingID,Program from M_MeetingMaster Where ActiveStatus = 'Y' And Cast(MeetingTime as Date ) >=  Cast(Getdate() As date)"
            str &= " Union All "
            str &= " Select 0 As  MeetingID, '--Select Seminar--'"
            str &= " ) Temp Order by MeetingID"

            Dim Dt As New DataTable
            Dt = obj.GetData(str)
            ddlProgram.DataSource = Dt
            ddlProgram.DataTextField = "Program"
            ddlProgram.DataValueField = "MeetingID"
            ddlProgram.DataBind()
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub BtnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerate.Click
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim SqlStr As String = String.Empty
            SqlStr = "Exec Generate_SeminarTicket " & ddlProgram.SelectedValue & "," & txtqty.Text & ";"
            If obj.SaveData(SqlStr) <> 0 Then
                lblError.Text = txtqty.Text & " Seminar Ticket stock of " & ddlProgram.SelectedItem.Text & " have been sucessfully generated."
                ddlProgram.SelectedIndex = 0
                txtqty.Text = ""

            Else
                lblError.Text = txtqty.Text & " Seminar Ticket stock of " & ddlProgram.SelectedItem.Text & " have been Not generated."
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
