Imports System.Data
Imports System.Net
Imports System.IO

Partial Class App_UI_Application_Pages_Add10Points

    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim Sql As String = ""
    Dim txformno As String = ""
    Dim txLoanNo As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click
        Try
            Dim query As String = ""
            Dim formNo As String
            Dim voucherNo As String = ""
            Dim scrName As String
            formNo = TxtFormNo.Text

            lblError.Text = ""
            If rdblist.SelectedIndex = 0 Then
                txtActiveStatus.Text = "Y"
            Else
                txtActiveStatus.Text = "N"
            End If

            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub
            Else
                'ExistsIdNo()
                lblError.Text = ""
            End If

           
            Dim Remark As String = ""
            Dim LoanNo As String = ""
            If String.IsNullOrEmpty(Request("TId")) = False Then

            Else

                query = "insert into M_Tenpoints(Idno,ActiveStatus,RecTimeStamp)values" & _
            "('" & TxtIDNo.Text & "','" & txtActiveStatus.Text & "',GetDate())"


                'objDAL.SaveData(Query)
                If objDAL.SaveData(query) <> 0 Then
                    scrName = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                    'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                    BtnFundTransfer.Enabled = True : TxtIDNo.Text = "" : TxtFormNo.Text = "" : LblMemName.Text = "" : LblAmount.Text = ""
                End If

            End If
            scrName = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrName, False)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then
                If Request("TId") IsNot Nothing Then

                    'txformno = Request("key")
                    'txLoanNo = Request("LoanNo")
                    'BtnFundTransfer.Text = "Update"
                    FillDetail()

                End If

            End If
            ExistsIdNo()
        Else
            Response.Redirect("logout.aspx")
        End If
    End Sub
    Protected Sub FillDetail()
        Dim Dt As DataTable
        Dt = New DataTable
        'Dim str As String = " select * from TrnLoan as a, M_MemberMaster as b where  a.Formno=b.Formno and a.Formno='" & txformno & "' and LoanNo='" & txLoanNo & "' "
        Dim str As String = " select * from M_Tenpoints where ActiveStatus='Y' "
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dt = objDAL.GetData(str)
        If Dt.Rows.Count > 0 Then
            TxtIDNo.Text = Dt.Rows(0)("Idno")
            'TxtFund.Text = Dt.Rows(0)("Amount")
            'Txtloan.Text = Dt.Rows(0)("LoanPercent")
            'TxtRemarks.Text = Dt.Rows(0)("Remark")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
            'Txtloan.ReadOnly = False
            TxtIDNo.ReadOnly = True
            'TxtFund.ReadOnly = True
            'TxtRemarks.ReadOnly = True
            'RntLoan.Enabled = False

        End If

    End Sub
    


    Private Function Check_IdNo() As Boolean
        Try
            Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl  From M_Membermaster WHERE IDNO='" & Trim(TxtIDNo.Text) & "' and IsBlock='N'"
            Dim Dt_ As New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            Dt_ = objDAL.GetData(Sql)
            If Dt_.Rows.Count = 0 Then
                LblMemName.Text = " Please enter correct Member ID."
                LblMemName.ForeColor = Drawing.Color.Red
                TxtIDNo.Text = ""
                BtnFundTransfer.Enabled = False
                Return False
            Else
                LblMemName.Text = Dt_.Rows(0)("MemName")
                LblMobl.Text = Dt_.Rows(0)("Mobl")
                LblMemName.ForeColor = Drawing.Color.Black
                TxtFormNo.Text = Dt_.Rows(0)("FormNo")
                lblError.Text = ""
                BtnFundTransfer.Enabled = True
                Return True
            End If

        Catch ex As Exception

        End Try

    End Function
    Private Function ExistsIdNo() As Boolean
        Try
            Dim scrName As String
            Sql = "Select Count(*) as cnt From M_Tenpoints WHERE IDNO='" & Trim(TxtIDNo.Text) & "'"
            Dim Dt As New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            Dt = objDAL.GetData(Sql)
            If Dt.Rows(0)("cnt") >= 1 Then
                'LblMemName.Text = "Already Generate 10 Points this Member ID ."
                'LblMemName.ForeColor = Drawing.Color.Red
                scrName = "<SCRIPT language='javascript'>alert('Already Generate 10 Points this Member ID!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "already", scrName, False)
                TxtIDNo.Text = ""
                LblMemName.Visible = False
                BtnFundTransfer.Enabled = False
                Return False
            Else
                'LblMemName.Text = Dt_.Rows(0)("MemName")
                'LblMobl.Text = Dt_.Rows(0)("Mobl")
                'LblMemName.ForeColor = Drawing.Color.Black
                'TxtFormNo.Text = Dt_.Rows(0)("FormNo")
                lblError.Text = ""
                BtnFundTransfer.Enabled = True
                Return True
            End If

        Catch ex As Exception

        End Try

    End Function
  
    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo()

    End Sub


    'Protected Sub RntLoan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RntLoan.SelectedIndexChanged
    '    If RntLoan.SelectedValue = "D" Then
    '        PLoanPercent.Visible = True
    '    Else
    '        PLoanPercent.Visible = False
    '    End If
    'End Sub
End Class
