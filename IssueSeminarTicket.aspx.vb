Imports System.Data
Partial Class IssueSeminarTicket
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


    Private Function Check_IdNo() As Boolean
        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName , Mobl,Formno From " & obj.tblMemberMaster & " WHERE IDNO='" & Trim(TxtIDNo.Text) & "'"
        Dim Dt_ As New DataTable
        Dt_ = obj.GetData(Sql)
        If Dt_.Rows.Count = 0 Then
            LblMemName.Text = " Please enter correct Member ID."
            LblMemName.ForeColor = Drawing.Color.Red
            TxtIDNo.Text = ""
            BtnGenerate.Enabled = False
            LblFormno.Text = ""
            Return False
        Else
            LblMemName.Text = Dt_.Rows(0)("MemName")
            LblMemMobl.Text = Dt_.Rows(0)("Mobl")
            LblFormno.Text = Dt_.Rows(0)("Formno")

            LblMemName.ForeColor = Drawing.Color.Green
            BtnGenerate.Enabled = True
            Return True
        End If
    End Function

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Try
            Check_IdNo()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerate.Click

    End Sub
End Class
