Imports System.Data
Imports System.Data.SqlClient
Partial Class invoiceSolfit
    Inherits System.Web.UI.Page
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("AStatus") = "OK" Then
                If Not Page.IsPostBack Then
                    If (Request("orderno") IsNot Nothing) Then
                        Dim OrderNo As String = Request("orderno")
                        Dim FormNo As String = Request("FormNo")
                        Fill_Invoice(OrderNo, FormNo)
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Fill_Invoice(ByVal OrderNo As String, ByVal FormNo As String)
        Try
            Dim str As String = " Exec Sp_ShowInvoiceAdmin '" & FormNo & "','" & OrderNo & "'"
            Dim ds As DataSet = New DataSet()
            Dim dt As DataTable = New DataTable()
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            dt = ds.Tables(0)
            If (dt.Rows.Count > 0) Then
                LblDate.Text = dt.Rows(0)("orderdate")
                LblOderNo.Text = dt.Rows(0)("OrderNo")
                LblID.Text = dt.Rows(0)("idno")
                LblName.Text = dt.Rows(0)("Name")
                LblMobileNo.Text = dt.Rows(0)("mobl")
                LblAddress.Text = dt.Rows(0)("Addresss")
            End If
            Dim str_ As String = " Exec Sp_getProductGetAdmin '" & OrderNo & "'"
            Dim ds_ As DataSet = New DataSet()
            Dim dt_ As DataTable = New DataTable()
            ds_ = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str_)
            dt_ = ds_.Tables(0)
            If (dt_.Rows.Count > 0) Then
                gv.DataSource = dt_
                gv.DataBind()
                LblTOtalAmount.Text = ds_.Tables(1).Rows(0)("TotalAmount")
                LblTotalBv.Text = ds_.Tables(1).Rows(0)("TotalBV")
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Function Base64Decode(ByVal base64EncodedData As String) As String
        Dim base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData)
        Return System.Text.Encoding.UTF8.GetString(base64EncodedBytes)
    End Function
End Class
