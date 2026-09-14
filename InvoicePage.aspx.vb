Imports System.Data

Partial Public Class InvoicePage
    Inherits System.Web.UI.Page
    Private Obj As DAL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If Session("AStatus") IsNot Nothing AndAlso Session("AStatus").ToString() = "OK" Then

                If Not Page.IsPostBack Then
                    Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    If Request("orderno") IsNot Nothing Then
                        Dim OrderNo As String = Base64Decode(Request("orderno").ToString())
                        Fill_Invoice(OrderNo)
                    End If
                End If
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Sub Fill_Invoice(ByVal OrderNo As String)
        Try
            Dim str As String = " Exec Sp_ShowInvoice '" & Session("FormNo") & "','" & OrderNo & "'"
            Dim ds As DataSet = New DataSet()
            Dim dt As DataTable = New DataTable()
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            dt = ds.Tables(0)

            If dt.Rows.Count > 0 Then
                litIssueDate.Text = dt.Rows(0)("orderdate").ToString()
                LblOderNo.Text = dt.Rows(0)("OrderNo").ToString()
                litToName.Text = dt.Rows(0)("Name").ToString()
                litToAddress.Text = dt.Rows(0)("Addresss").ToString()
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Function Base64Decode(ByVal base64EncodedData As String) As String
        Dim base64EncodedBytes As Byte() = Convert.FromBase64String(base64EncodedData)
        Return Encoding.UTF8.GetString(base64EncodedBytes)
    End Function
End Class
