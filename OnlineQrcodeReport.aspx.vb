Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Xml
Imports System.Web.Script.Serialization
Partial Class OnlineQrcodeReport
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim dtData As New DataTable
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.btnSearch.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.btnSearch))
        Try
            If Session("AStatus") = "OK" Then
            Else
                Response.Redirect("logout.aspx")
            End If
            If Not Page.IsPostBack Then
                FillReport()
                btnExport.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub FillReport()
        Dim TransactionID As String = ""
        Dim WalletAddress As String = ""
        Dim Hash As String = ""
        Dim startDate As String
        Dim endDate As String
        Dim currentDate As DateTime = DateTime.Now
        Dim formattedDate As String = currentDate.ToString("dd-MMM-yyyy")
        If txtMemberId.Text <> "" Then
            TransactionID = txtMemberId.Text.Trim
        Else
            TransactionID = ""
        End If
        If TxtWalletAddress.Text <> "" Then
            WalletAddress = TxtWalletAddress.Text
        Else
            WalletAddress = ""
        End If
        If TxtHash.Text <> "" Then
            Hash = TxtHash.Text
        Else
            Hash = ""
        End If
        If txtStartDate.Text = "" Then
            startDate = "12-oct-2017"
        Else
            startDate = txtStartDate.Text
        End If
        If txtEndDate.Text = "" Then
            endDate = formattedDate
        Else
            endDate = txtEndDate.Text
        End If
        Dim sql As String = "exec sp_GetOnlineTransactionDetailUpdate '" & TransactionID & "','" & WalletAddress & "',"
        sql &= "'" & CmbMessage.SelectedValue & "','" & startDate & "','" & endDate & "','" & Hash & "','N','" & DDlRequestType.SelectedValue & "'"
        dtData = New DataTable
        dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).tables(0)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        ViewState("WithDrawDate") = "BankCode"
        ViewState("Sort_Order") = "ASC"
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub
    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim SqlStr As String = ""
        Dim sql_res As String = ""
        Dim Hdnidno, Hdnprivatekey, HdnWalletAddress, HdnAmount, Hdnformno, HdnTxnhash, LblGrpID, scrname As String
        Try
            Dim GVRw As GridViewRow
            GVRw = CType(sender.Parent.Parent, GridViewRow)
            LblGrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
            Hdnidno = DirectCast(GVRw.FindControl("Hdnidno"), Label).Text
            HdnAmount = DirectCast(GVRw.FindControl("HdnAmount"), Label).Text
            HdnWalletAddress = DirectCast(GVRw.FindControl("HdnWalletAddress"), Label).Text
            Hdnprivatekey = DirectCast(GVRw.FindControl("Hdnprivatekey"), Label).Text
            Hdnformno = DirectCast(GVRw.FindControl("Hdnformno"), Label).Text
            HdnTxnhash = DirectCast(GVRw.FindControl("HdnTxnhash"), Label).Text
            Dim dt As DataTable = New DataTable()
            SqlStr = "select * from TrnvoucherTxnHash where Txnhash = '" & HdnTxnhash.Trim() & "' AND Is_Pay = 'P'"
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, SqlStr).tables(0)
            If (dt.Rows.Count > 0) Then
                Dim TokenResoponse As String = Fund_Token_Send(HdnWalletAddress, Hdnprivatekey, HdnAmount, Hdnformno)
                If TokenResoponse.ToString().ToUpper.Trim() = "SUCCESSFUL" Then
                    sql_res = "Update TrnvoucherTxnHash  Set Is_Pay = 'A',Update_DATe = getdate() Where To_walletAddress = '" & HdnWalletAddress.Trim() & "' AND "
                    sql_res &= " To_PrivateKey = '" & Hdnprivatekey.Trim() & "' AND formno = '" & Hdnformno.Trim() & "' AND Txnhash = '" & HdnTxnhash.Trim() & "'  "
                    Dim x_res As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql_res))
                    If (x_res > 0) Then
                        scrname = "<SCRIPT language='javascript'>alert('Payment SuccessFully Added.!');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
                        'Exit Sub
                    End If
                Else
                    scrname = "<SCRIPT language='javascript'>alert('Payment Failed.!');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
                    'Exit Sub
                End If
            Else
                scrname = "<SCRIPT language='javascript'>alert('Already Clear.!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
                'Exit Sub
            End If
        Catch ex As Exception
            Dim errorQry As String = ""
            errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,formno) "
            errorQry &= "values('" & ex.Message & "',getdate(),'','" & SqlStr.Trim() & "',,'" & sql_res & "',,'" & Hdnformno & "')"
            Dim x1 As Integer = 0
            x1 = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, errorQry)
        End Try
        FillReport()
    End Sub
    Public Function Fund_Token_Send(ByVal senderAddress As String, ByVal senderPrivateKey As String, ByVal Balance As String, ByVal Formno_V As String) As String
        Dim sResult As String = ""
        Dim current_datetime As String = Format(Now(), "yyyyMMddHHmmssfff")
        Dim random_number As Integer = New Random().Next(0, 999)
        Dim formatted_datetime As String = current_datetime & random_number.ToString().PadLeft(3, "0"c)
        sResult = formatted_datetime
        Dim URL As String = ""
        Dim postData As String = ""
        Dim str As String = String.Empty
        Dim value As Decimal = 0
        Dim Code As String = ""
        Dim ds As New DataSet
        Dim data As New DataSet
        Dim StatusApi As String = ""
        Dim RetunnStatus As String = ""
        Try
            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
            URL = "http://5.9.143.153:7017/TokenAdmin"
            Dim tRequest As WebRequest
            tRequest = WebRequest.Create(URL)
            tRequest.Method = "POST"
            tRequest.ContentType = "application/json"
            tRequest.ContentLength = 0
            postData = "{""senderAddress"":""" & senderAddress.Trim() & """,""senderPrivateKey"":""" & senderPrivateKey.Trim() & """,""amount"":""" & Val(Balance) & """}"
            Dim byteArray() As Byte = Encoding.UTF8.GetBytes(postData)
            Dim sql_req As String = "insert Into Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,postdata,ApiType)"
            sql_req &= "Values('" & sResult.Trim() & "','" & Formno_V.Trim() & "','" & URL.Trim() & "','" & postData.Trim() & "','TOKENADMIN')"
            Dim x_Req As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql_req))
            tRequest.ContentLength = byteArray.Length
            Dim dataStream As Stream = tRequest.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim tResponse As WebResponse = tRequest.GetResponse()
            dataStream = tResponse.GetResponseStream()
            Dim tReader As StreamReader = New StreamReader(dataStream)
            str = tReader.ReadToEnd()
            Dim sql_res As String = "Update Tbl_ApiRequest_ResponseQrCode  Set Response = '" & str.Trim() & "' Where ReqID = '" & sResult.Trim() & "' AND ApiType = 'TOKENADMIN' "
            Dim x_res As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql_res))
            Dim SqlUpdate As String = ""
            Dim json As String = str
            data = convertJsonStringToDataSet(str.ToString())
            StatusApi = data.Tables(0).Rows(0)("transactionstatus")
            Dim Query As String = ""
            Dim hash_ As String = ""
            If StatusApi.ToString().ToUpper.Trim() = "SUCCESSFUL" Then
                Try
                    hash_ = data.Tables(0).Rows(0)("transactionHash")
                Catch ex As Exception
                Finally
                    If hash_ <> "" Then
                    Else
                        hash_ = ""
                    End If
                End Try
            End If
            Dim strs As String = ""
            strs &= "insert into ApiReqResponseAdmin(Formno,Orderid,WalletAddress,PrivateKey,Request,"
            strs &= "Response,ApiStatus,RectimeStamp,ApiType,TxnHash,AMount,PostData,TypeB,Coinrate) "
            strs &= "Values('" & Formno_V & "','" & senderPrivateKey & "','" & senderAddress & "',"
            strs &= "'" & senderPrivateKey & "','" & URL & "','" & str & "','" & StatusApi & "',getdate(),'Token Payout','" & hash_ & "',"
            strs &= "'" & Val(Balance) & "','" & postData & "','QrCode','170');"
            Query = " Begin Try   Begin Transaction " & strs & " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH"
            Dim x As Integer = 0
            x = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Query)
            If (x > 0) Then
                RetunnStatus = StatusApi
            End If
        Catch ex As Exception
            Dim errorQry As String = ""
            errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,formno) "
            errorQry &= "values('" & ex.Message & "',getdate(),'" & URL & "','" & senderAddress.Trim() & "',,'" & postData & "',,'" & Formno_V & "')"
            Dim x1 As Integer = 0
            x1 = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, errorQry)
            If (x1 > 0) Then
                RetunnStatus = "failed"
            End If
        End Try
        Return RetunnStatus
    End Function
    Public Function convertJsonStringToDataSet(ByVal jsonString As String) As DataSet
        Dim xd As XmlDocument = New XmlDocument()
        jsonString = "{ ""rootNode"": {" & jsonString.Trim().TrimStart("{"c).TrimEnd("}"c) & "} }"
        xd = CType(JsonConvert.DeserializeXmlNode(jsonString), XmlDocument)
        Dim ds As DataSet = New DataSet()
        ds.ReadXml(New XmlNodeReader(xd))
        Return ds
    End Function
    Private Function DisableTheButton(ByVal pge As Control, ByVal btn As Control) As String
        Dim sb As New System.Text.StringBuilder()
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ")
        sb.Append("this.value = 'Please wait...';")
        sb.Append("this.disabled = true;")
        sb.Append(pge.Page.GetPostBackEventReference(btn))
        sb.Append(";")
        Return sb.ToString()
    End Function
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillReport()
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        FillReport()
    End Sub
    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        If e.SortExpression = ViewState("WithDrawDate").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/up.png", "~/Images/down.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = e.SortExpression Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/up.png", "~/Images/down.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            End If
        Else
            RebindData(e.SortExpression, "ASC")
        End If
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Dim dt As DataTable = CType(Session("GDataNew"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("WithDrawDate") = sColimnName
        ViewState("Sort_Order") = sSortOrder
    End Sub
    Protected Sub CmbMessage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbMessage.TextChanged
        FillReport()
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim TransactionID As String = ""
            Dim WalletAddress As String = ""
            Dim Hash As String = ""
            Dim startDate As String
            Dim endDate As String
            Dim currentDate As DateTime = DateTime.Now
            Dim formattedDate As String = currentDate.ToString("dd-MMM-yyyy")
            If txtMemberId.Text <> "" Then
                TransactionID = txtMemberId.Text.Trim
            Else
                TransactionID = ""
            End If
            If TxtWalletAddress.Text <> "" Then
                WalletAddress = TxtWalletAddress.Text
            Else
                WalletAddress = ""
            End If
            If TxtHash.Text <> "" Then
                Hash = TxtHash.Text
            Else
                Hash = ""
            End If
            If txtStartDate.Text = "" Then
                startDate = "12-oct-2017"
            Else
                startDate = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                endDate = formattedDate
            Else
                endDate = txtEndDate.Text
            End If
            Dim sql As String = "exec sp_GetOnlineTransactionDetailUpdate '" & TransactionID & "','" & WalletAddress & "',"
            sql &= "'" & CmbMessage.SelectedValue & "','" & startDate & "','" & endDate & "','" & Hash & "','Y','" & DDlRequestType.SelectedValue & "'"
            dtData = New DataTable
            dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).tables(0)
            Session("GDataNew") = dtData
            ExportExcel()
        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GDataNew")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Incentive")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=OnlineTransactionsReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub

    Protected Sub DDlRequestType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlRequestType.TextChanged
        FillReport()
    End Sub
End Class
