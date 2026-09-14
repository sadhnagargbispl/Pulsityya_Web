Imports System.Data
Partial Class SmartCardBV
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Session("AStatus") = "OK" Then
                    Session("PageName") = "Member / Add Smart Card BV"
                Else
                    Response.Redirect("Logout.aspx")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Try
            Get_SponsorDetail()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Get_SponsorDetail()
        Try
            Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,IDNo, FormNo From M_MemberMaster WHERE IDno = '" & TxtIDNo.Text.Trim() & "' And ActiveStatus = 'Y'"
            Dim Ds_ As New DataSet
            Ds_ = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Sql)
            If Ds_.Tables(0).Rows.Count > 0 Then

                LblMemName.Text = Ds_.Tables(0).Rows(0)("IDNo") & " [" & Ds_.Tables(0).Rows(0)("MemName") & "]"
                hdnFormno.Value = Ds_.Tables(0).Rows(0)("FormNo")
            Else
                TxtIDNo.Text = ""
                LblMemName.Text = ""
                hdnFormno.Value = ""
                scrname = "<SCRIPT language='javascript'>alert('Invaild Member ID OR Deactivated ID, Please Enter Vaild Member ID or Active Member ID.!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub txtAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAmount.TextChanged


    End Sub

    Protected Sub BtnLegShift_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLegShift.Click
        Try
            'Dim Totalamount As String
            'Totalamount = Val(txtBV.Text) + Val(TxtReward.Text) + Val(TxtRoyalty.Text)
            'If Totalamount <= Val(txtAmount.Text) Then
            Dim SmartCardBV As String = Val(TxtRoyalty.Text)
            'Dim STR As String = "sp_AddSmartCardBV '" & hdnFormno.Value & "', '" & Val(txtBV.Text) & "','" & Val(txtAmount.Text) & "','" & Val(TxtReward.Text) & "','" & Val(TxtRoyalty.Text) & "','" & SmartCardBV & "','" & txtRemark.Text & "'"
            Dim STR As String = "sp_AddSmartCardBV '" & hdnFormno.Value & "', '" & Val(txtBV.Text) & "','" & Val(txtAmount.Text) & "','" & Val(TxtReward.Text) & "','0','" & SmartCardBV & "','" & txtRemark.Text & "'"
            Dim i As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, STR)
            If (i > 0) Then
                Clear()
                scrname = "<SCRIPT language='javascript'>alert('Record Save Successfully .!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Else
                scrname = "<SCRIPT language='javascript'>alert('Invaild Data Please Try Again.!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
            'Else
            '    scrname = "<SCRIPT language='javascript'>alert('Invaild Data .Matching BV,Reward and Royalty cannot be greater than Totalamount. Please Try Again.!!');" & "</SCRIPT>"
            '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)

            'End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Clear()
        Try
            TxtIDNo.Text = ""
            LblMemName.Text = ""
            hdnFormno.Value = ""
            txtBV.Text = ""
            txtAmount.Text = ""
            txtRemark.Text = ""
            TXtmatchingperc.Text = ""
            TxtReward.Text = ""
            TxtRewardPerc.Text = ""
            TxtRoyalty.Text = ""
            TxtRoyaltyper.Text = ""
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TXtmatchingperc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TXtmatchingperc.TextChanged
        Try
            Dim decNum As Decimal = Val(txtAmount.Text)
            Dim strArr = decNum.ToString().Split("."c).ToArray()
            txtAmount.Text = strArr(0)
            If Val(TXtmatchingperc.Text) > 100 Then
                scrname = "<SCRIPT language='javascript'>alert('Matching Percentege can not be greater than 100%.!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                TXtmatchingperc.Text = ""
                txtBV.Text = ""
                Exit Sub
                'ElseIf (Val(TXtmatchingperc.Text) + Val(TxtRewardPerc.Text) + Val(TxtRoyaltyper.Text)) > Val(100) Then
                '    scrname = "<SCRIPT language='javascript'>alert('Invaild Data .Matching BV,Reward and Royalty cannot be greater than Totalamount. Please Try Again.!!');" & "</SCRIPT>"
                '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                '    TXtmatchingperc.Text = ""
                '    txtBV.Text = ""
                '    Exit Sub
            Else
                txtBV.Text = Val((Val(txtAmount.Text) * Val(TXtmatchingperc.Text)) / 100).ToString()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TxtRewardPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtRewardPerc.TextChanged
        Try
            Dim decNum As Decimal = Val(txtAmount.Text)
            Dim strArr = decNum.ToString().Split("."c).ToArray()
            txtAmount.Text = strArr(0)
            If Val(TxtRewardPerc.Text) > 100 Then
                scrname = "<SCRIPT language='javascript'>alert('Matching Percentege can not be greater than 100%.!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                TxtReward.Text = ""
                TxtRewardPerc.Text = ""
                Exit Sub
                'ElseIf (Val(TXtmatchingperc.Text) + Val(TxtRewardPerc.Text) + Val(TxtRoyaltyper.Text)) > Val(100) Then
                '    scrname = "<SCRIPT language='javascript'>alert('Invaild Data .Matching BV,Reward and Royalty cannot be greater than Totalamount. Please Try Again.!!');" & "</SCRIPT>"
                '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                '    TxtReward.Text = ""
                '    TxtRewardPerc.Text = ""
                '    Exit Sub
            Else
                TxtReward.Text = Val((Val(txtAmount.Text) * Val(TxtRewardPerc.Text)) / 100).ToString()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TxtRoyaltyper_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtRoyaltyper.TextChanged
        Try
            Dim decNum As Decimal = Val(txtAmount.Text)
            Dim strArr = decNum.ToString().Split("."c).ToArray()
            txtAmount.Text = strArr(0)
            If Val(TxtRoyaltyper.Text) > 100 Then
                scrname = "<SCRIPT language='javascript'>alert('Matching Percentege can not be greater than 100%.!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                TxtRoyalty.Text = ""
                TxtRoyaltyper.Text = ""
                Exit Sub
                'ElseIf (Val(TXtmatchingperc.Text) + Val(TxtRewardPerc.Text) + Val(TxtRoyaltyper.Text)) > Val(100) Then
                '    scrname = "<SCRIPT language='javascript'>alert('Invaild Data .Matching BV,Reward and Royalty cannot be greater than Totalamount. Please Try Again.!!');" & "</SCRIPT>"
                '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                '    TxtRoyalty.Text = ""
                '    TxtRoyaltyper.Text = ""
                '    Exit Sub
            Else
                TxtRoyalty.Text = Val((Val(txtAmount.Text) * Val(TxtRoyaltyper.Text)) / 100).ToString()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnLegShiftLess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLegShiftLess.Click
        Try
            'Dim Totalamount As String
            'Totalamount = Val(txtBV.Text) + Val(TxtReward.Text) + Val(TxtRoyalty.Text)
            'If Totalamount <= Val(txtAmount.Text) Then
            Dim SmartCardBV As String = Val(-TxtRoyalty.Text)
            'Dim STR As String = "sp_AddSmartCardBV '" & hdnFormno.Value & "', '" & Val(-txtBV.Text) & "','" & Val(-txtAmount.Text) & "','" & Val(-TxtReward.Text) & "','" & Val(-TxtRoyalty.Text) & "','" & SmartCardBV & "','" & txtRemark.Text & "'"
            Dim STR As String = "sp_AddSmartCardBV '" & hdnFormno.Value & "', '" & Val(-txtBV.Text) & "','" & Val(-txtAmount.Text) & "','" & Val(-TxtReward.Text) & "','0','" & SmartCardBV & "','" & txtRemark.Text & "'"
            Dim i As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, STR)
            If (i > 0) Then
                Clear()
                scrname = "<SCRIPT language='javascript'>alert('Record Save Successfully .!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Else
                scrname = "<SCRIPT language='javascript'>alert('Invaild Data Please Try Again.!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
            'Else
            '    scrname = "<SCRIPT language='javascript'>alert('Invaild Data .Matching BV,Reward and Royalty cannot be greater than Totalamount. Please Try Again.!!');" & "</SCRIPT>"
            '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)

            'End If


        Catch ex As Exception

        End Try
    End Sub
End Class
