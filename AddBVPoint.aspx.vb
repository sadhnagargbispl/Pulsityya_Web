Imports System.Data
Imports System.Net
Imports System.IO

Partial Class AddBVPoint

    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim query As String = ""
            Dim formNo As String
            Dim voucherNo As String = ""
            Dim scrName As String
            formNo = TxtFormNo.Text

            lblError.Text = ""
            BtnFundTransfer.Enabled = False

            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub
            ElseIf Val(TxtFund.Text) <= "0" Then
                lblError.Text = "Enter BV Value."
                Exit Sub
            Else
            End If
            Dim Remark As String = ""
            Dim updateeffect As Integer
            Dim StrSql As String = "Insert into TrnBVadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
            updateeffect = objDAL.SaveData(StrSql)

            If updateeffect > 0 Then
           

                query = " Insert into TrnBV(Sessid,Formno,LegNo,BV,PV,Remark,RecTimeStamp,ActiveStatus,DSessid,BvType,Userid) Values " & _
                " ('" & Session("CurrentSessn") & "','" & Val(TxtFormNo.Text) & "','" & Val(RbtLeg.SelectedValue) & "'," & _
                " '" & Val(TxtFund.Text) & "','" & Val(txtPV.Text) & "','" & TxtRemarks.Text & "',Getdate(),'Y',Convert(Varchar(30),Getdate(),112),'S','" & Session("UserID") & "')"
                'objDAL.SaveData(Query)
                If objDAL.SaveData(query) <> 0 Then
                    ' SSendsms()

                    scrName = "<SCRIPT language='javascript'>alert('BV Point Added in " & RbtLeg.SelectedItem.Text & " Successfully!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                    'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                    TxtFund.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRemarks.Text = "" : LblMemName.Text = "" : LblAmount.Text = ""
                End If

                scrName = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrName, False)
            Else
                Response.Redirect("AddBVPoint.aspx")
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim str = "exec('Create table TrnBVadmin ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," & _
"ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[TrnBVadmin] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
            Dim i As Integer = 0
            i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)

        Catch ex As Exception

        End Try
        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then
                HdnCheckTrnns.Value = GenerateRandomStringAdmin(6)
            End If
        Else
            Response.Redirect("logout.aspx")
        End If
    End Sub
    Public Function GenerateRandomStringAdmin(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next
        Return sResult
    End Function


    Private Function Check_IdNo() As Boolean
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl  From " & objDAL.tblMemberMaster & " WHERE IDNO='" & Trim(TxtIDNo.Text) & "'"
        Dim Dt_ As New DataTable
        Dim dt1 As DataTable
        Dt_ = objDAL.GetData(Sql)
        If Dt_.Rows.Count = 0 Then
            LblMemName.Text = " Please enter correct Member ID."
            LblMemName.ForeColor = Drawing.Color.Red
            TxtIDNo.Text = ""
            BtnFundTransfer.Enabled = False
            Return False
        Else
            Sql = " Select * from TrnBV where Formno='" & Dt_.Rows(0)("Formno") & "' and Sessid='" & Session("CurrentSessn") & "' and ActiveStatus='Y'"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt1 = New DataTable
            dt1 = objDAL.GetData(Sql)
            If Session("CompId") = 1024 Then
                LblMemName.Text = Dt_.Rows(0)("MemName")
                LblMobl.Text = Dt_.Rows(0)("Mobl")
                LblMemName.ForeColor = Drawing.Color.Black
                TxtFormNo.Text = Dt_.Rows(0)("FormNo")
                BtnFundTransfer.Enabled = True
                Return True
            Else
                If dt1.Rows.Count > 0 Then
                    LblMemName.Text = " This Id Already Have BV In Current Session"
                    BtnFundTransfer.Enabled = False
                    Return False
                Else
                    LblMemName.Text = Dt_.Rows(0)("MemName")
                    LblMobl.Text = Dt_.Rows(0)("Mobl")
                    LblMemName.ForeColor = Drawing.Color.Black
                    TxtFormNo.Text = Dt_.Rows(0)("FormNo")
                    BtnFundTransfer.Enabled = True
                    Return True
                End If
            End If

        End If
    End Function
    

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo()

    End Sub


End Class
