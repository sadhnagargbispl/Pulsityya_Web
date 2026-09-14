Imports System.Data
Partial Class App_UI_Application_Pages_IdUpgrade
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Member / Upgrade Package "
            'If Not Page.IsPostBack Then

            'End If
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Function Check_IdNo() As Boolean
        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq,a.Formno From " & obj.tblMemberMaster & " as a," & obj.tblKitMaster & " as b WHERE a.KitId=b.KitId And IDNO='" & Trim(TxtIDNo.Text) & "' and IsBlock='N'"
        Dim Dt_ As New DataTable
        Dt_ = obj.GetData(Sql)
        LblKitId.Text = Dt_.Rows(0)("KitId")
        If Dt_.Rows.Count = 0 Then
            LblMemName.Text = " Please enter correct Member ID."
            LblMemName.ForeColor = Drawing.Color.Red
            TxtIDNo.Text = ""
            BtnUpgrade.Enabled = False
            Return False
        Else
            If Session("MACAdrs") = "R" Then
                If Dt_.Rows(0)("IsTopup") = "Y" Then
                    If Session("TopUpSeq") > Dt_.Rows(0)("TopUpSeq") Then

                        LblMemName.Text = Dt_.Rows(0)("MemName")
                        LblFormno.Text = Dt_.Rows(0)("Formno")
                        '  TxtPinNo.Focus()
                        LblMemName.ForeColor = Drawing.Color.Black
                        BtnUpgrade.Enabled = True
                        Return True
                    Else
                        LblMemName.Text = " Member is  already upgraded."
                        LblMemName.ForeColor = Drawing.Color.Red
                        TxtIDNo.Text = ""
                        BtnUpgrade.Enabled = False
                        Return False
                    End If

                Else

                    LblMemName.Text = " Member is not activated. "
                    LblMemName.ForeColor = Drawing.Color.Red
                    TxtIDNo.Text = ""
                    BtnUpgrade.Enabled = False
                    Return False
                End If
            Else

                If Dt_.Rows(0)("IsTopup") = "N" Then
                    LblMemName.Text = Dt_.Rows(0)("MemName")
                    LblFormno.Text = Dt_.Rows(0)("Formno")
                    '  TxtPinNo.Focus()
                    LblMemName.ForeColor = Drawing.Color.Black
                    BtnUpgrade.Enabled = True
                    Return True
                Else

                    LblMemName.Text = " Member already activated. Please enter another Member ID."
                    LblMemName.ForeColor = Drawing.Color.Red
                    TxtIDNo.Text = ""
                    BtnUpgrade.Enabled = False
                    Return False
                End If
            End If

        End If
    End Function

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        'Check_IdNo()
    End Sub

    Private Function Check_PinNo() As Boolean
        Sql = "Select a.KitName,b.FormNo,b.ScratchNo,b.GeneratedBy,b.UsedBy,a.Allowtopup,b.ProdId,a.MACAdrs,a.TopUpSeq FROM M_KitMaster as a,M_FormGeneration as b WHERE a.KitID=b.ProdID AND b.FormNo='" & Trim(TxtPinNo.Text) & "'" '' AND a.Allowtopup='Y' AND b.Usedby='0' AND GeneratedBy='Y'
        Dim Dt_ As New DataTable
        Dt_ = obj.GetData(Sql)
   
        If Dt_.Rows.Count = 0 Then
            LblErr.Text = " Please enter correct Pin No."
            TxtPinNo.Text = ""
            BtnUpgrade.Enabled = False
            Return False
        Else
            Session("NewKitName") = Val(Dt_.Rows(0)("KitName"))
            Session("TopUpSeq") = Dt_.Rows(0)("TopUpSeq")
            Session("MACAdrs") = Dt_.Rows(0)("MACAdrs")
            If Dt_.Rows(0)("Allowtopup").ToString().ToUpper <> "Y" Then
                LblErr.Text = " You can't updgrade to kit " & Dt_.Rows(0)("KitName") & "." : TxtPinNo.Text = "" : BtnUpgrade.Enabled = False : Return False
            ElseIf Dt_.Rows(0)("GeneratedBy").ToString().ToUpper <> "Y" Then
                LblErr.Text = " This pin is not issued to any Member." : TxtPinNo.Text = "" : BtnUpgrade.Enabled = False : Return False
            ElseIf Dt_.Rows(0)("Usedby") > "0" Then
                LblErr.Text = " This is used pin." : TxtPinNo.Text = "" : BtnUpgrade.Enabled = False : Return False
            Else
                'LblMemName.Text = Dt_.Rows(0)("FormNo")
                LblErr.Text = ""
                LblScratchNo.Text = Dt_.Rows(0)("ScratchNo")
                BtnUpgrade.Enabled = True
                Return True
            End If
        End If

        'If LblKitId.Text < Session("KitId") And MacAdrs = "R" Then
        '    LblErr.Text = " This pin is not issued to any Member." : TxtPinNo.Text = "" : BtnUpgrade.Enabled = False : Return False
        '    Return True
        '    'If Session("KitId") Then
        'End If




    End Function

    Protected Sub TxtPinNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtPinNo.TextChanged
        If TxtPinNo.Text = "" Then
            Exit Sub
        Else
            Check_PinNo()
            Check_IdNo()
        End If

    End Sub

    Protected Sub BtnUpgrade_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpgrade.Click
        Dim scrname As String
        Dim Remark As String
        Remark = " Package Upgrade of Idno:" & TxtIDNo.Text & ""
        Try
            lblError.Text = ""

            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub
            ElseIf Val(TxtPinNo.Text) = 0 Then
                lblError.Text = "Enter Pin No."
                Exit Sub
            ElseIf Trim(TxtScratchNo.Text) = "" Then
                lblError.Text = "Enter Scratch No."
                Exit Sub
            Else
                If Check_PinNo() = False Then
                    lblError.Text = "Please Pin No." : Exit Sub
                End If
                If Check_IdNo() = False Then
                    lblError.Text = "Invalid Member ID."
                    Exit Sub
                End If

                If Trim(LblScratchNo.Text) <> Trim(TxtScratchNo.Text) Then
                    lblError.Text = "Incorrect Scratch No." : Exit Sub
                End If

                Sql = "Exec Sp_Activate '" & Trim(TxtIDNo.Text) & "'," & Val(TxtPinNo.Text) & ";"
                Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,memberId)Values" & _
     "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Upgrade Package ','Upgrade Package','" & Remark & "',Getdate(),'" & LblFormno.Text & "')"

                If obj.SaveData(Sql) <> 0 Then

                    scrname = "<SCRIPT language='javascript'>alert('ID upgraded Successfully!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                    lblError.Text = " ID upgraded successfully."
                    TxtIDNo.Text = "" : TxtPinNo.Text = "" : TxtScratchNo.Text = "" : LblMemName.Text = "" : LblErr.Text = "" : BtnUpgrade.Enabled = False
                    ' BtnExport.Visible = True
                End If
            End If
        Catch ex As Exception
            scrname = "<SCRIPT language='javascript'>alert('" & ex.Message & "');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
        End Try
    End Sub

End Class
