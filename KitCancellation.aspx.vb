Imports System.Data
Partial Class App_UI_Application_Pages_KitCancellation
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub BtnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerate.Click
        Try
            lblError.Text = ""
            BtnGenerate.Enabled = False

            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub
            ElseIf CmbKit.SelectedValue = 0 Then
                lblError.Text = "Invalid Package."
                Exit Sub
           
            Else
                If Check_IdNo() = False Then
                    ' lblError.Text = "Invalid Member ID."
                    Exit Sub
                End If
               
                Sql = "Exec Sp_CancelActivateMembers '" & Trim(TxtIDNo.Text) & "';"
                If obj.SaveData(Sql) <> 0 Then
                    lblError.Text = "Kit activation cancelled successfully for " & TxtIDNo.Text & "."

                End If
            End If
            BtnGenerate.Enabled = True
            TxtIDNo.Text = ""

            LblMemName.Text = ""

            CmbKit.SelectedIndex = -1
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            CmbKit.Enabled = False
            If Not Page.IsPostBack Then
                FillKit()
            End If
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub

    Public Sub FillKit()
        Sql = "Select * From (Select 0 As KitID,'-- Select Package --' As KitName Union ALL Select kitId,KitName From M_KitMaster) As Temp Order By kitId"
        Dim Dt As New DataTable
        Dt = obj.GetData(Sql)
        CmbKit.DataSource = Dt
        CmbKit.DataTextField = "KitName"
        CmbKit.DataValueField = "KitId"
        CmbKit.DataBind()
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        lblError.Text = ""
        BtnGenerate.Enabled = True
        CmbKit.SelectedIndex = 0
    End Sub

    Private Function Check_IdNo() As Boolean
        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,KitID,IsTopup,UpgrdSessID From " & obj.tblMemberMaster & " WHERE IDNO='" & Trim(TxtIDNo.Text) & "'"
        Dim Dt_ As New DataTable
        Dt_ = obj.GetData(Sql)
        If Dt_.Rows.Count = 0 Then
            LblMemName.Text = " Please enter correct Member ID."
            LblMemName.ForeColor = Drawing.Color.Red
            TxtIDNo.Text = ""
            BtnGenerate.Enabled = False
            Return False
        Else
            If Dt_.Rows(0)("IsTopup") = "N" Then
                LblMemName.Text = " Member ID is not activated."
                LblMemName.ForeColor = Drawing.Color.Red
                TxtIDNo.Text = ""
                BtnGenerate.Enabled = False

                Return False
            ElseIf Val(Dt_.Rows(0)("UpgrdSessID")) <> Val(Session("CurrentSessn")) Then
                LblMemName.Text = " Member ID is not activated in Current Week."
                LblMemName.ForeColor = Drawing.Color.Red
                TxtIDNo.Text = ""
                BtnGenerate.Enabled = False
                Return False
            Else
                LblMemName.Text = Dt_.Rows(0)("MemName")
                LblMemName.ForeColor = Drawing.Color.Black
                BtnGenerate.Enabled = True
                CmbKit.SelectedValue = Dt_.Rows(0)("KitID")
                CmbKit.Enabled = True
                Return True
            End If
        End If
    End Function

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo()
    End Sub

End Class
