Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class AddLevelIncome
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim Adp As SqlDataAdapter
    Dim dRead As SqlDataReader
    Dim Ds As New DataSet
    Dim dt As New DataTable
    Dim StrQuery As String
    Dim ScrName As String
    Dim objDAL As DAL
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Conn.Open()
            If Session("AStatus") = "OK" Then
                Session("PageName") = ""
                If Not Page.IsPostBack Then
                    BindData()
                End If
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Sub
  Private Function Check_IdNo() As Boolean
        Try
            Sql = "Select a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName " & _
                " from M_MemberMaster as a where  a.IDNo='" & TxtIDNo.Text & "' and a.IsBlock='N'"
            Dim Dt_ As New DataTable
            Dt_ = obj.GetData(Sql)
            If Dt_.Rows.Count = 0 Then
                lblError.Text = " Please enter correct Member ID."
                lblError.ForeColor = Drawing.Color.Red
                TxtIDNo.Text = ""
                LblMemName.Text = ""
                LblCondition.Text = ""
                LblNewKitid.Text = ""
                BtnUpgrade.Enabled = False
                Return False
            Else
                LblMemName.Text = Dt_.Rows(0)("MemName")
                LblFormno.Text = Dt_.Rows(0)("Formno")
                
                LblMemName.ForeColor = Drawing.Color.Black
                BtnUpgrade.Enabled = True
                Return True
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Function

    Private Function BindData() As Boolean
        Try
            Sql = "Select Idno,membername, Replace(Convert(Varchar,rectimestamp ,106),' ','-') " & _
                " +' '+CONVERT(varchar(15),CAST(rectimestamp AS TIME),100) as datetime from LevelIncomeMaster"
            Dim Dt_ As New DataTable
            Dt_ = obj.GetData(Sql)
            GrdDirects1s.DataSource = Dt_
            GrdDirects1s.DataBind()
            GrdDirects1s.Visible = True
         Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Function
    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Try
            Check_IdNo()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Sub
    Protected Function Updtmaster() As Boolean
        Try
            Dim strQry, Result As String
            Dim Dr As SqlDataReader
            Dim formno As String = ""
            Dim Partycode As String = ""
            Dim Address As String = ""
            Try
                Result = "Hello"
                Dim s As String = ""
                strQry = "Exec Sp_AddLevelIncome '" & Trim(TxtIDNo.Text) & "','" & LblFormno.Text & "','" & LblMemName.Text & "';"
                Comm = New SqlCommand(strQry, Conn)
                Dr = Comm.ExecuteReader()
                If Dr.Read = True Then
                    Result = Dr("Result")
                    Dr.Close()
                End If
                If Result Like "SUCCESS" Then
                    Updtmaster = True
                    BindData()
                Else
                    Updtmaster = False
                End If
            Catch ex As Exception
                Updtmaster = False
            End Try
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
             Response.Write("Try later.")
        End Try
    End Function
     Protected Sub BtnUpgrade_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpgrade.Click
        Dim scrname As String
      Try
            lblError.Text = ""

            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub

            Else

                If Check_IdNo() = False Then
                    lblError.Text = "Invalid Member ID."
                    Exit Sub
                End If
                If Trim(TxtIDNo.Text) <> "" Then
                    Dim s1 As String = "Select COUNT(idno) as cnt from LevelIncomeMaster where idno='" & Trim(TxtIDNo.Text) & "'"
                    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    Dim Dt1 As DataTable
                    Dt1 = New DataTable
                    Dt1 = objDAL.GetData(s1)
                    If Dt1.Rows(0)("cnt") > 0 Then

                        scrname = "<SCRIPT language='javascript'>alert('Already Add This ID.');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                        LblFormno.Text = ""
                        Exit Sub
                    End If
                End If


                If Updtmaster() Then
                    clear()
                    scrname = "<SCRIPT language='javascript'>alert('Successfully Sent');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
                Else
                    scrname = "<SCRIPT language='javascript'>alert('Unsuccessfully Sent');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
                End If
            End If
        Catch ex As Exception
            scrname = "<SCRIPT language='javascript'>alert('" & ex.Message & "');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
        End Try
    End Sub

    
    Protected Sub clear()
        TxtIDNo.Text = "" : LblMemName.Text = "" : lblError.Text = "" : BtnUpgrade.Enabled = False
        LblFormno.Text = "" : LblKitId.Text = ""
        LblNewKitid.Text = ""
    End Sub




    
End Class
