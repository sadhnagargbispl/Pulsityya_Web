Imports System.Data
Partial Class App_UI_Application_Pages_ChangeRef
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub BtnLegShift_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLegShift.Click
        Try
            lblError.Text = ""
            BtnLegShift.Enabled = False

            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                BtnLegShift.Enabled = True
                scrname = "<SCRIPT language='javascript'>alert('" & lblError.Text & "');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)

                Exit Sub

            ElseIf Trim(TxtSpIDNo.Text) = "" Then
                lblError.Text = "Enter Sponser ID."
                BtnLegShift.Enabled = True
                scrname = "<SCRIPT language='javascript'>alert('" & lblError.Text & "');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)

                Exit Sub

            Else
                If Check_IdNo(TxtFormNo, LblMemName, TxtIDNo) = False Then
                    lblError.Text = "Invalid Member ID."
                    BtnLegShift.Enabled = True
                    scrname = "<SCRIPT language='javascript'>alert('" & lblError.Text & "');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)

                    Exit Sub
                End If
                If Check_IdNo(TxtFormNos, LblSponserName, TxtSpIDNo) = False Then
                    lblError.Text = "Invalid Sponser ID."
                    BtnLegShift.Enabled = True
                    scrname = "<SCRIPT language='javascript'>alert('" & lblError.Text & "');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    Exit Sub
                End If

                Sql = " Select * FROM R_MemTreeRelation WHERE FormNo='" & Val(TxtFormNo.Text) & "' AND FormNoDwn='" & Val(TxtFormNos.Text) & "'"
                Dim Dt As New DataTable
                Dt = obj.GetData(Sql)
                If Dt.Rows.Count > 0 Then
                    lblError.Text = "Sponsor couldn't be changed."
                    scrname = "<SCRIPT language='javascript'>alert('" & lblError.Text & "');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    BtnLegShift.Enabled = True

                    Exit Sub
                Else
                    Leg_Shift()
                End If
                BtnLegShift.Enabled = True
                TxtIDNo.Text = ""
                TxtSpIDNo.Text = ""
                LblMemName.Text = ""
                LblSponserName.Text = ""
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Private Sub Leg_Shift()
        Try
            Dim LegNo As Integer = 0
            Dim Remark As String = ""

            Remark = "Sponsor Change of Idno=" & TxtIDNo.Text & ""
            Sql = "Select CASE WHEN Max(LegNo) is NULL THEN '1' ELSE Max(LegNo)+1 END AS LegNo FROM R_MemTreeRelation WHERE FormNo='" & Val(TxtFormNos.Text) & "' AND MLevel=1"
            Dim Dt As New DataTable
            Dt = obj.GetData(Sql)
            If Dt.Rows.Count > 0 Then
                LegNo = Dt.Rows(0)("LegNo")
            End If
            Sql = " Begin Transaction Exec RefLegShift_NJ '" & Val(TxtFormNo.Text) & "','" & Val(TxtFormNos.Text) & "','" & LegNo & "'Commit Transaction"
            Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
     "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Changer Sponsor ','Change Sponsor','" & Remark & "',Getdate(),'" & TxtFormNo.Text & "')"

            Dim i As Integer = obj.SaveData1(Sql)
            If i <> 0 Then
                lblError.Text = "Sponsor Id Changed Successfully!!"
            Else
                lblError.Text = "Problem in Sponsor change."
            End If
            scrname = "<SCRIPT language='javascript'>alert('" & lblError.Text & "');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        Catch ex As Exception
            lblError.Text = ex.Message
            Exit Sub
        End Try
    End Sub

    '    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '        If Session("AStatus") = "OK" Then
    '            If Not Page.IsPostBack Then
    '                FillKit()
    '                getStock()
    '            End If
    '        Else
    '            Response.Redirect("~\Default.aspx")
    '        End If
    '    End Sub

    '    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
    '        lblError.Text = ""
    '        BtnGenerate.Enabled = True
    '        TxtQty.Text = 0
    '        CmbKit.SelectedIndex = 0
    '        GvBatchMaster.DataSource = Nothing
    '        GvBatchMaster.DataBind()
    '        BtnExport.Visible = False
    '    End Sub

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

    Private Sub Get_SponsorDetail()
        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,IDNo, FormNo From M_MemberMaster WHERE FormNo in (Select RefFormno FROM M_MemberMaster WHERE FormNo='" & Trim(TxtFormNo.Text) & "')"
        Dim Dt_ As New DataTable
        Dt_ = obj.GetData(Sql)
        If Dt_.Rows.Count > 0 Then
            LblOldSponsor.Text = Dt_.Rows(0)("IDNo") & " [" & Dt_.Rows(0)("MemName") & "]"
        End If
    End Sub

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo(TxtFormNo, LblMemName, TxtIDNo)
        Get_SponsorDetail()
    End Sub

    '    Protected Sub TxtQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtQty.TextChanged
    '        Check_Stock()
    '    End Sub

    Protected Sub TxtSpIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtSpIDNo.TextChanged
        Check_IdNo(TxtFormNos, LblSponserName, TxtSpIDNo)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

       
            If Not Page.IsPostBack Then
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Session("AStatus") = "OK" Then
                    Session("PageName") = "Member / Change Sponsor"
                Else
                    Response.Redirect("Logout.aspx")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class
