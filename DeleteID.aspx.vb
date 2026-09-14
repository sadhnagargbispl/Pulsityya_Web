Imports System.Data
Partial Class App_UI_Application_Pages_DeleteID
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub BtnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDelete.Click
        Try
            Dim scrname As String
            lblError.Text = ""
            BtnDelete.Enabled = False

            If Trim(TxtIDNo.Text) = "" Then
                ''lblError.Text = "Enter Member ID."
                scrname = "<SCRIPT language='javascript'>alert('Enter Member ID.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Enter Member ID.');", True)
                Exit Sub
            Else
                If Check_IdNo() = False Then
                    ''lblError.Text = "Invalid Member ID."
                    scrname = "<SCRIPT language='javascript'>alert('Invalid Member ID.');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Invalid Member ID.');", True)
                    Exit Sub
                End If

                Sql = "Exec DelIDByRk '" & Trim(TxtFormNo.Text) & "';"
                If obj.SaveData(Sql) <> 0 Then
                    lblError.Text = TxtIDNo.Text & " Deleted Successfully."
                End If
            End If
            TxtIDNo.Text = ""
            LblMemName.Text = ""
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then
            End If
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        lblError.Text = ""
        BtnDelete.Enabled = True
    End Sub

    Private Function Check_IdNo() As Boolean
        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo From " & obj.tblMemberMaster & " WHERE IDNO='" & Trim(TxtIDNo.Text) & "'"
        Dim Dt_ As New DataTable
        Dt_ = obj.GetData(Sql)
        If Dt_.Rows.Count = 0 Then
            LblMemName.Text = " Please enter correct Member ID."
            LblMemName.ForeColor = Drawing.Color.Red
            TxtIDNo.Text = ""
            BtnDelete.Enabled = False
            Return False
        Else
            TxtFormNo.Text = Dt_.Rows(0)("FormNo")
            'Check It is upliner of someone..
            Sql = "Select Count(*) as Cnt From " & obj.tblMemberMaster & " WHERE UplnFormNo='" & Trim(TxtFormNo.Text) & "'"
            Dim Dt As New DataTable
            Dt = obj.GetData(Sql)
            If Dt.Rows.Count > 0 Then
                Dim cnt As Integer = Dt.Rows(0)("Cnt")
                If cnt >= 1 Then
                    LblMemName.Text = " Cannot delete this Member ID. Delete downline First."
                    LblMemName.ForeColor = Drawing.Color.Red
                    TxtIDNo.Text = ""
                    BtnDelete.Enabled = False
                    Return False
                Else
                    LblMemName.Text = Dt_.Rows(0)("MemName")
                    LblMemName.ForeColor = Drawing.Color.Black
                    BtnDelete.Enabled = True
                    Return True
                End If
            End If
            
            End If
    End Function

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo()
    End Sub

End Class
