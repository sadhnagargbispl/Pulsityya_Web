Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports System.Data
Partial Class Reciept
    Inherits System.Web.UI.Page
    Dim strScript As String
    Dim dtData As New DataTable
    Dim DSet As New DataSet
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack And Request.QueryString.HasKeys Then
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Key As Integer = Val(Crypto.Decrypt(Replace(Request.QueryString("Key"), " ", "+")))
            LblDeviceId.Text = Key
            Dim RefNo As String = Crypto.Decrypt(Replace(Request.QueryString("RefNo"), " ", "+"))
            LblRefno.Text = RefNo
            Dim FormNo As Integer = Val(Crypto.Decrypt(Replace(Request.QueryString("FormNo"), " ", "+")))
            LblFormno.Text = FormNo
            'BindData(Key)
            Dim Emimonth As Integer = Val(Crypto.Decrypt(Replace(Request.QueryString("EmiMonth"), " ", "+")))
            LblEmimonth.Text = Emimonth
            BindDataNew(Key, RefNo, FormNo, Emimonth)
            If GetCompID() = "" Then
                Response.Write("Host not found.")
                Response.End()
            End If
        End If
    End Sub
    Public Function GetCompID() As String
        Dim url As String = String.Empty
        Dim conn As SqlConnection

        Try
            url = HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICADMIN.", "").Replace("ADMIN.", "")
            Dim str As String = String.Empty
            If url = "LOCALHOST" Then
                str = " Select ID,Logo from M_CompanyMasterNew Where IsActive = 1 And ID='" & ConfigurationManager.AppSettings("CompanyID") & "'"
            Else
                str = " Select ID,Logo from M_CompanyMasterNew Where IsActive = 1 And (Upper(URL) = '" & url.ToUpper().Trim() & "') "
            End If




            Dim dRead As SqlDataReader
            Dim cmd As SqlCommand
            conn = New SqlConnection(Application("sConnect"))
            conn.Open()

            cmd = New SqlCommand(str, conn)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                Session("CompID") = dRead("ID")
                'Session("Logo") = "http://superadmin.bisplindia.in/images/Logo/" & dRead("Logo")
                'imgLogo.Src = "http://superadmin.bisplindia.in/images/Logo/" & dRead("Logo")

                'Session("FormNO") = dRead("FormNo")
                Session("Logo") = dRead("Logo")
                imgLogo.Src = dRead("Logo")

            End If
            dRead.Close()
            conn.Close()
        Catch ex As Exception
            If Not conn Is Nothing Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
        End Try
        GetCompID = url
    End Function
    

    Public Sub BindDataNew(ByVal ReqNo As String, ByVal RefNo As String, ByVal FormNO As String, ByVal EmiMonth As Integer)
        Try
            Dim Condition As String = ""
            Dim scrName As String = ""
            Dim InstallmentNo As Integer = 0

            Dim qry1 As String = ""
            InstallmentNo = EmiMonth
            Dim sql As String = String.Empty

            sql = "Exec Printreciept " & ReqNo & ",'" & RefNo & "','" & FormNO & "','" & InstallmentNo & "'"
            DSet = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql)
            DSet = objDAL.ExecProcDataSet(sql)
            'Session("Print") = DSet.Tables(0)

            If (DSet.Tables(0).Rows.Count > 0) Then
                lblbuyername.Text = DSet.Tables(0).Rows(0)("IdName")
                lblmobileno.Text = DSet.Tables(0).Rows(0)("Mobl")
                lblbuyerAddress.Text = DSet.Tables(0).Rows(0)("Address1")

            End If
            If (DSet.Tables(1).Rows.Count > 0) Then
                lblrecieptNo.Text = DSet.Tables(1).Rows(0)("RecieptNo")
                lblinstallmentamt.Text = DSet.Tables(1).Rows(0)("PaidAmount")
                lblrecieptdate.Text = DSet.Tables(1).Rows(0)("ReqDate")
                'lblInstallmentmonth.Text = DSet.Tables(1).Rows(0)("Installment No")
                lblInstallmentmonth.Text = DSet.Tables(1).Rows(0)("ReqDate")
                lblinstallmentno.Text = DSet.Tables(1).Rows(0)("Installment No")
                lblduedate.Text = DSet.Tables(1).Rows(0)("NextDueDate")
            End If

            If (DSet.Tables(2).Rows.Count > 0) Then
                lbldueamount.Text = DSet.Tables(2).Rows(0)("DueLoanAmt")
                lbltotalinsAmount.Text = DSet.Tables(2).Rows(0)("LoanAmt")
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
