Imports System.Data
Partial Class App_UI_Application_Pages_LegShiftNP
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub BtnLegShift_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLegShift.Click
        Try
            lblError.Text = ""
            BtnLegShift.Enabled = False

            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub

            ElseIf Trim(TxtSpIDNo.Text) = "" Then
                lblError.Text = "Enter Sponser ID."
                Exit Sub

            Else
                If Check_IdNo(TxtFormNo, LblMemName, TxtIDNo) = False Then
                    lblError.Text = "Invalid Member ID."
                    Exit Sub
                End If
                If Check_IdNo(TxtFormNos, LblSponserName, TxtSpIDNo) = False Then
                    lblError.Text = "Invalid Upliner ID."
                    Exit Sub
                End If
                Dim dtLeg = New DataTable()
                Dim LegNo As Integer
                Dim strLeg As String = " Exec  Sp_GetMaxLegno  '" & Val(TxtFormNos.Text) & "'"
                dtLeg = obj.GetData(strLeg)
                LegNo = Val(dtLeg.Rows(0)("Cnt"))
                Sql = " Select * From M_MemberMaster WHERE LegNo='" & LegNo & "' AND UplnFormNo='" & Val(TxtFormNos.Text) & "'"
                Dim Dt As New DataTable
                Dt = obj.GetData(Sql)
                If Dt.Rows.Count > 0 Then
                    lblError.Text = "Leg is not blank."
                    Exit Sub
                End If
                Sql = "Select RefFormNo FROM M_MemberMaster WHERE Formno='" & Val(TxtFormNo.Text) & "' AND RefFormNo='" & Val(TxtFormNos.Text) & "'"
                Dt = New DataTable
                Dt = obj.GetData(Sql)
                If Dt.Rows.Count > 0 Then
                    Leg_Shift(Val(TxtFormNo.Text), Val(TxtFormNos.Text), LegNo)
                Else
                    Sql = " Select * From M_MemTreeRelation WHERE FormNoDwn='" & Val(TxtFormNos.Text) & "' AND FormNo in (Select RefFormNo FROm M_MemberMaster WHERE Formno='" & Val(TxtFormNo.Text) & "')"
                    Dt = New DataTable
                    Dt = obj.GetData(Sql)
                    If Dt.Rows.Count > 0 Then
                        Leg_Shift(Val(TxtFormNo.Text), Val(TxtFormNos.Text), LegNo)
                    Else
                        lblError.Text = "Member Id does not exist in sponsor downline ."
                        Exit Sub
                    End If
                End If
            End If
            BtnLegShift.Enabled = True
            TxtIDNo.Text = ""
            TxtSpIDNo.Text = ""
            LblMemName.Text = ""
            LblSponserName.Text = ""

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Private Sub Leg_Shift(ByVal FormNo As Integer, ByVal FormNos As Integer, ByVal LegNo As Integer)
        Try
            Dim remark As String = ""
            remark = " Upliner Change Of Idno=" & TxtIDNo.Text & " "
            Sql = "Begin Transaction Exec LegShift_NJ '" & Val(FormNo) & "','" & Val(FormNos) & "','" & LegNo & "' Commit Transaction"

            Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
      "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Change Upliner ','Change Upliner','" & remark & "',Getdate(),'" & TxtFormNo.Text & "')"

            Dim i As Integer = obj.SaveData(Sql)
            If i <> 0 Then
                lblError.Text = "Id Shifted Successfully!!"
            Else
                lblError.Text = "Problem in leg shifting."
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
            Exit Sub
        End Try
    End Sub

    

    Private Function Check_IdNo(ByVal txtf As TextBox, ByVal lblm As Label, ByVal txtid As TextBox) As Boolean
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName, FormNo From M_MemberMaster WHERE IDNO='" & Trim(txtid.Text) & "'"
        Dim Dt_ As New DataTable
        Dt_ = obj.GetData(Sql)
        If Dt_.Rows.Count = 0 Then
            lblm.Text = " Please enter correct Member ID."
            lblm.ForeColor = Drawing.Color.Red
            txtid.Text = ""
            BtnLegShift.Enabled = False
            Return False
        Else
            txtf.Text = Dt_.Rows(0)("FormNo")
            lblm.Text = Dt_.Rows(0)("MemName")
            lblm.ForeColor = Drawing.Color.Black
            BtnLegShift.Enabled = True
            Return True
        End If
    End Function


    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo(TxtFormNo, LblMemName, TxtIDNo)
    End Sub

    '    Protected Sub TxtQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtQty.TextChanged
    '        Check_Stock()
    '    End Sub

    Protected Sub TxtSpIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtSpIDNo.TextChanged
        Check_IdNo(TxtFormNos, LblSponserName, TxtSpIDNo)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Member / Change Upliner"
            Else
                Response.Redirect("Logout.aspx")
            End If
        End If
    End Sub
End Class
