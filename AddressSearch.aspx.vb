Imports System.Net
Imports System.Data
Imports System.IO
Imports System.Xml
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Globalization

Partial Class AddressSearch
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim Conn As SqlConnection
    Dim Conn1 As SqlConnection
    Dim Comm As SqlCommand
    Dim OrderId As String = ""
    Dim FromID As String = ""
    Dim privatekey As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
            End If
        End If
    End Sub
    Private Function Check_IdNo() As Boolean
        Try
            Dim Sql As String = ""
            Sql = " Exec Sp_GetIDDetail_formno '" & LblFormno.Text & "'"
            Dim Dt_ As New DataTable
            Dt_ = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Sql).Tables(0)
            If Dt_.Rows.Count > 0 Then
                TxtMemberName.Text = Dt_.Rows(0)("MemName")
                TxtMemberID.Text = Dt_.Rows(0)("Idno")
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Function
    Protected Sub TxtMemberID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtMemberID.TextChanged
        If Check_IdNo() = True Then
            Dim Sql As String = ""
            Sql = " Exec Sp_Getformno_WalletAddress '" & LblFormno.Text & "'"
            Dim Dt_ As New DataTable
            Dt_ = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Sql).Tables(0)
            If Dt_.Rows.Count > 0 Then
                TxtWalletAddress.Text = Dt_.Rows(0)("Address")
            End If
        End If
    End Sub

    Protected Sub TxtWalletAddress_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtWalletAddress.TextChanged
        Dim Sql As String = ""
        Sql = " Exec Sp_Getformno_WalletAddress '" & TxtWalletAddress.Text & "'"
        Dim Dt_ As New DataTable
        Dt_ = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Sql).Tables(0)
        If Dt_.Rows.Count > 0 Then
            LblFormno.Text = Dt_.Rows(0)("formno")
            Check_IdNo()
        Else
            Dim scrname = "<SCRIPT language='javascript'>alert('Wallet Address Not Found.!');;location.replace('AddressSearch.aspx');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
        End If
    End Sub
End Class
