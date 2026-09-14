Imports System.Net
Imports System.Data
Imports System.IO
Imports System.Xml
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Globalization

Partial Class RejoiningProcess
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
        Me.UpdateTxnHash.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.UpdateTxnHash))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                Fun_Sp_GetCryptoAPIFor_FundWithdraw_Cpanel()
            End If
        End If
    End Sub
    Private Sub Fun_Sp_GetCryptoAPIFor_FundWithdraw_Cpanel()
        Try
            Dim sql As String = ""
            Dim dt_API_Master As New DataTable()
            sql = " Exec Sp_GetCryptoAPIFor_FundWithdraw_Cpanel "
            dt_API_Master = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).Tables(0)
            Session("CreateQrCode") = dt_API_Master.Rows(0)("APIURL")
            Session("BalanceCheckQrCode") = dt_API_Master.Rows(1)("APIURL")
            Session("TokenCheckQrCode") = dt_API_Master.Rows(2)("APIURL")
            Session("MMRATE") = dt_API_Master.Rows(3)("APIURL")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Private Function DisableTheButton(ByVal pge As Control, ByVal btn As Control) As String
        Dim sb As New System.Text.StringBuilder()
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ")
        sb.Append("this.value = 'Please Wait...';")
        sb.Append("this.disabled = true;")
        sb.Append(pge.Page.GetPostBackEventReference(btn))
        sb.Append(";")
        Return sb.ToString()
    End Function
    Protected Sub UpdateTxnHash_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateTxnHash.Click
        If TxtThxhash.Text = "" Then
            Dim scrname = "<SCRIPT language='javascript'>alert('Please Enter TxnHash.!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Please Enter TxnHash.!');", True)
        Else
            TokenHashCheck(TxtThxhash.Text.Trim())
        End If
    End Sub
    Public Function TokenHashCheck(ByVal txnhash As String) As String
        Try
            Dim scrname As String = ""
            Dim str As String = ""
            Dim dsLogin As DataSet = New DataSet()
            Dim completeUrl As String = "http://5.9.143.153:7017/status?transactionHash=" & txnhash.Trim() & ""
            Dim responseString As String = String.Empty
            Dim CustomerOTPmessage As String = ""
            Try
                System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType)
                Dim request1 As HttpWebRequest = TryCast(WebRequest.Create(completeUrl), HttpWebRequest)
                request1.ContentType = "application/json"
                request1.Method = "GET"
                Dim httpWebResponse As HttpWebResponse = CType(request1.GetResponse(), HttpWebResponse)
                Dim reader As StreamReader = New StreamReader(httpWebResponse.GetResponseStream())
                responseString = reader.ReadToEnd()
                Dim json As String = responseString
                Dim jss = New JavaScriptSerializer()
                Dim Data = jss.Deserialize(Of Object)(responseString)
                dsLogin = convertJsonStringToDataSet(responseString)
                str = responseString
                Dim auth As String = dsLogin.Tables(0).Rows(0)("status")
                Dim txn As String = dsLogin.Tables(0).Rows(0)("transactionHash")
                Dim amount As String = dsLogin.Tables(0).Rows(0)("amount")
                Dim add As String = dsLogin.Tables(0).Rows(0)("to")
                Dim sql1 As String = ""
                If auth.ToString().ToUpper() = "SUCCESS" Then
                    sql1 = "insert Into TrnHashDataAdmin (RectimeStamp,TxnHash,status,Response,Url,fromA,toA,gasUsed,amount,token)"
                    sql1 &= "Values(getdate(),'" & dsLogin.Tables(0).Rows(0)("transactionHash") & "','" & dsLogin.Tables(0).Rows(0)("status") & "','" & responseString & "',"
                    sql1 &= "'" & completeUrl & "','" & dsLogin.Tables(0).Rows(0)("from") & "','" & dsLogin.Tables(0).Rows(0)("to") & "' ,"
                    sql1 &= "'" & dsLogin.Tables(0).Rows(0)("gasUsed") & "','" & dsLogin.Tables(0).Rows(0)("amount") & "','" & dsLogin.Tables(0).Rows(0)("token") & "')"
                    Dim x As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql1)
                    Dim sql As String = ""
                    sql = " Exec Sp_GetWalletaddressUpdet '" & add.Trim() & "','" & txn.Trim() & "' "
                    Dim DtS As DataTable = New DataTable()
                    DtS = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).Tables(0)
                    If DtS.Rows.Count > 0 Then
                        Dim Response = DtS.Rows(0)("response")
                        If Response.ToString.ToUpper = "OK" Then
                            If (add.ToString().ToLower() = TxtWalletAddress.Text.ToString().ToLower()) Then
                                GetResponseCheckloop(add, amount, txn)
                            Else
                                scrname = "<SCRIPT language='javascript'>alert('Your Address Not Match.!');" & "</SCRIPT>"
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('" & Response & "');", True)
                                TxtThxhash.Text = ""
                            End If
                        Else
                            scrname = "<SCRIPT language='javascript'>alert('" & Response & "');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('" & Response & "');", True)
                            TxtThxhash.Text = ""

                        End If
                    Else
                    End If
                End If
            Catch ex As Exception
                Dim errorQry As String = ""
                errorQry = "insert Into TrnLogData(ErrorText,LogDate,Url,WalletAddress,SelectType)" & _
                        " values('" & ex.Message & "',getdate(),'" & completeUrl & "','" & responseString & "','Admin')"
                Dim x1 As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, errorQry)
                scrname = "<SCRIPT language='javascript'>alert('" & ex.Message & "');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('" & ex.Message & "');", True)
                TxtThxhash.Text = ""
            End Try
            Return str
        Catch ex As Exception
        End Try
    End Function
    Private Function GetResponseCheckloop(ByVal WalletAddress As String, ByVal amount As String, ByVal Txnhash As String) As String
        Dim sqlStr_ As String = ""
        Dim dtdataSql As DataTable = New DataTable()
        sqlStr_ = " Exec Sp_GetWalletAddress '" & WalletAddress.Trim() & "' "
        dtdataSql = objDAL.GetData(sqlStr_)
        If dtdataSql.Rows.Count > 0 Then
            OrderId = dtdataSql.Rows(0)("orderid")
            FromID = dtdataSql.Rows(0)("Formno")
            privatekey = dtdataSql.Rows(0)("PrivateKey")
        End If
        Dim result = ""
        Dim messgae = ""
        Dim Status = ""
        Dim txnin = ""
        Dim txnout = ""
        Dim scrname = ""
        Dim sql As String = ""
        Try
            Dim TransactionStatus As String = "1"
            Dim vi As String = WalletAddress
            Dim CallbackUrl = ""
            Dim message As String = ""
            If TransactionStatus <> "" Then
                Dim i As Integer = 0
                If TransactionStatus.ToString().Trim() = "1" Then
                    Dim Bal As String = Fund_Balance_Check(WalletAddress, FromID, OrderId, privatekey, Txnhash)
                    If Val(Bal) > 0 Then
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Function
    Public Function Fund_Balance_Check(ByVal walletAddress As String, ByVal formNoV As String, ByVal orderIV As String, ByVal privateKeyV As String, ByVal Txnhash As String) As String
        Dim URL As String = ""
        Dim sResult As String = ""
        Dim currentDatetime As String = DateTime.Now.ToString("yyyyMMddHHmmssfff")
        Dim randomNumber As Integer = New Random().Next(0, 999)
        Dim formattedDatetime As String = currentDatetime & randomNumber.ToString().PadLeft(3, "0"c)
        sResult = formattedDatetime
        Dim postData As String = ""
        Dim balance As String = ""
        Dim str As String = ""
        Dim statusApi As String = ""
        Dim resultRR As Integer = 0
        Dim dsLogin As New DataSet()
        Dim dsLoginToAddress As New DataTable()
        Dim responseString As String = String.Empty
        Dim toWalletAddress As String = ""
        Try
            Dim value As Decimal = 0
            Dim code As String = ""
            Dim ds As New DataSet()
            Dim data As New DataSet()
            Try
                ServicePointManager.Expect100Continue = True
                ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
                URL = Session("BalanceCheckQrCode").ToString()
                Dim tRequest As WebRequest = WebRequest.Create(URL)
                tRequest.Method = "POST"
                tRequest.ContentType = "application/json"
                tRequest.ContentLength = 0
                postData = "{""walletAddress"": """ & walletAddress.Trim() & """}"
                Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
                Dim sqlReq As String = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, ApiType)"
                sqlReq += " VALUES('" & sResult.Trim() & "','" & formNoV.Trim() & "','" & URL.Trim() & "','" & postData.Trim() & "','BALANCECHECK')"
                Dim xReq As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sqlReq))
                tRequest.ContentLength = byteArray.Length
                Dim dataStream As Stream = tRequest.GetRequestStream()
                dataStream.Write(byteArray, 0, byteArray.Length)
                dataStream.Close()
                Dim tResponse As WebResponse = tRequest.GetResponse()
                dataStream = tResponse.GetResponseStream()
                Dim tReader As New StreamReader(dataStream)
                str = tReader.ReadToEnd()
                Dim sqlRes As String = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" & str.Trim() & "' WHERE ReqID = '" & sResult.Trim() & "' AND ApiType = 'BALANCECHECK' "
                Dim xRes As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sqlRes))
                Dim json As String = str
                Dim strs As String = ""
                Dim strCheck As String = ""
                Dim tokenName As String = ""
                Dim tokenSymbol As String = ""
                Dim FinalAmount As Decimal
                Dim dtCheck As New DataTable()
                Try
                    data = convertJsonStringToDataSet(str)
                    Dim dv As New DataView(data.Tables(1))
                    dv.RowFilter = "To = '" & walletAddress.Trim() & "'"
                    dsLoginToAddress = dv.ToTable()
                    If Convert.ToDecimal(data.Tables(0).Rows(0)("Balance")) > 0 Then
                        If dsLoginToAddress.Rows.Count > 0 Then
                            For Each row As DataRow In dsLoginToAddress.Rows
                                toWalletAddress = row("to").ToString()
                                Dim fromWalletAddress As String = row("from").ToString()
                                Dim originalValue As Double = Convert.ToDouble(row("value"))
                                FinalAmount = CType(originalValue / Math.Pow(10, 18), Decimal)
                                Dim accurateTotalAmount As Decimal = Decimal.Parse(FinalAmount.ToString(), NumberStyles.Float)
                                Dim hash As String = row("hash").ToString()
                                Dim contractAddress As String = row("contractAddress").ToString().Trim()
                                tokenName = row("tokenName").ToString()
                                tokenSymbol = row("tokenSymbol").ToString()
                                If tokenName.ToUpper() = "MANGO MAN INTELLIGENT" Then
                                    If tokenSymbol.ToUpper() = "MMIT" Then

                                        If (hash.ToString().ToLower() = Txnhash.ToString().ToLower()) Then
                                            Dim txnInsert As String = "INSERT INTO TrnvoucherTxnHash(Formno, From_walletAddress, To_walletAddress, Amount, Txnhash, To_PrivateKey,ReqFrom)"
                                            txnInsert += " VALUES('" & formNoV & "','" & fromWalletAddress & "','" & toWalletAddress & "',"
                                            txnInsert += "'" & Decimal.Parse(FinalAmount.ToString(), NumberStyles.Float) & "','" & hash & "','" & privateKeyV & "','MMIT')"
                                            Dim xI As Integer = 0
                                            xI = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, txnInsert)
                                            If xI > 0 Then
                                                strCheck = " EXEC Sp_CheckTxnHAsh '" & hash.ToString() & "'"
                                                dtCheck = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strCheck).Tables(0)
                                                If dtCheck.Rows.Count <> 0 Then
                                                    If contractAddress = "0x9767c8e438aa18f550208e6d1fdf5f43541cc2c8" Then
                                                        strs = String.Empty
                                                        strs = "INSERT INTO ApiQrCodeReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, Response,"
                                                        strs &= "ApiStatus, RectimeStamp, ApiType, TxnHash, AMount, PostData, TypeB, FromID, ToID) "
                                                        strs &= "VALUES('" & formNoV & "','" & orderIV & "','" & walletAddress & "','" & privateKeyV & "',"
                                                        strs &= "'" & URL & "','" & str & "','" & statusApi & "',GETDATE(),'Re-Transcation','" & hash & "',"
                                                        strs &= "'" & Decimal.Parse(FinalAmount.ToString(), NumberStyles.Float) & "','','QrCode',"
                                                        strs &= "'" & toWalletAddress & "','" & fromWalletAddress & "');"
                                                        strs &= " EXEC sp_FundAddUpdate_Fund '" & formNoV & "','" & Decimal.Parse(FinalAmount.ToString(), NumberStyles.Float) & "',"
                                                        strs &= "'" & orderIV & "','" & walletAddress & "','" & hash & "','" & fromWalletAddress & "','" & toWalletAddress & "';"
                                                        Dim queryStr As String = ""
                                                        queryStr = " BEGIN TRY BEGIN TRANSACTION " & strs & " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH"
                                                        Dim x As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, queryStr)
                                                        If x > 0 Then
                                                            resultRR = 1
                                                            Dim sqlResToken As String = ""
                                                            Dim tokenResponse As String = Fund_Token_Send(toWalletAddress, privateKeyV, Decimal.Parse(FinalAmount.ToString(), NumberStyles.Float), formNoV)
                                                            If tokenResponse.ToUpper().Trim() = "SUCCESSFUL" Then
                                                                sqlResToken = "UPDATE TrnvoucherTxnHash SET Is_Pay = 'A', Update_DATe = GETDATE() WHERE To_walletAddress = '" & toWalletAddress.Trim() & "' AND Is_Pay = 'P'"
                                                                Dim xResToken As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sqlResToken)
                                                            End If
                                                        End If
                                                    Else
                                                        strs = String.Empty
                                                        strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" & fromWalletAddress & "',To_walletAddress =  '" & toWalletAddress & "'"
                                                        strs &= " WHERE Txnhash  = '" & hash & "' AND Formno =  '" & formNoV & "'"
                                                        Dim x As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strs)
                                                    End If
                                                Else
                                                    strs = String.Empty
                                                    strs = "UPDATE TrnvoucherTxnHash SET From_walletAddress =  '" & fromWalletAddress & "',To_walletAddress =  '" & toWalletAddress & "'"
                                                    strs &= " WHERE Txnhash  = '" & hash & "' AND Formno =  '" & formNoV & "'"
                                                    Dim x As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strs)
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                    If resultRR > 0 Then
                        Dim message As String = "Payment Successfully Added in Wallet.!"
                        Dim urlStr As String = "RejoiningProcess.aspx"
                        Dim script As String = "window.onload = function(){ alert('" & message & "'); window.location = '" & urlStr & "'; }"
                        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Redirect", script, True)
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('No transaction found. Please try again later');", True)
                    End If
                Catch ex As Exception
                    Dim errorQry As String = ""
                    Dim errorMsg As String = ""

                    Try
                        errorMsg = ex.Message
                    Catch eXX As Exception
                    Finally
                        If errorMsg <> "" Then
                        Else
                            errorMsg = ""
                        End If
                    End Try
                    errorQry = "INSERT INTO TrnLogData(ErrorText, LogDate, Url, WalletAddress, PostData, formno) "
                    errorQry += "VALUES('" & errorMsg & "', GETDATE(),'" & URL & "','" & toWalletAddress.Trim() & "','" & str & "','" & formNoV & "')"
                    Dim x1 As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, errorQry)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('No transaction found. Please try again later.');", True)
                End Try
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('" & ex.Message & ".');", True)
            End Try
        Catch ex As Exception
        End Try

        Return balance
    End Function
    Public Function Fund_Token_Send(ByVal senderAddress As String, ByVal senderPrivateKey As String, ByVal Balance As Decimal, ByVal Formno_V As String) As String
        Dim sResult As String = ""
        Dim current_datetime As String = DateTime.Now.ToString("yyyyMMddHHmmssfff")
        Dim random_number As Integer = New Random().Next(0, 999)
        Dim formatted_datetime As String = current_datetime & random_number.ToString().PadLeft(3, "0"c)
        sResult = formatted_datetime
        Dim URL As String = ""
        Dim postData As String = ""
        Dim str As String = String.Empty
        Dim value As Decimal = 0
        Dim Code As String = ""
        Dim ds As New DataSet()
        Dim data As New DataSet()
        Dim StatusApi As String = ""
        Dim RetunnStatus As String = ""

        Try
            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
            URL = Session("TokenCheckQrCode").ToString()
            Dim tRequest As WebRequest = WebRequest.Create(URL)
            tRequest.Method = "POST"
            tRequest.ContentType = "application/json"
            tRequest.ContentLength = 0
            postData = "{""senderAddress"":""" & senderAddress.Trim() & """,""senderPrivateKey"":""" & senderPrivateKey.Trim() & ""","
            postData &= """amount"":""" & Decimal.Parse(Balance.ToString(), NumberStyles.Float) & """}"
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            Dim sql_req As String = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,postdata, ApiType)"
            sql_req &= " VALUES('" & sResult.Trim() & "','" & Formno_V.Trim() & "','" & URL.Trim() & "','" & postData.Trim() & "','TOKENCHECKADMIN')"
            Dim x_Req As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql_req))
            tRequest.ContentLength = byteArray.Length
            Dim dataStream As Stream = tRequest.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim tResponse As WebResponse = tRequest.GetResponse()
            dataStream = tResponse.GetResponseStream()
            Dim tReader As New StreamReader(dataStream)
            str = tReader.ReadToEnd()
            Dim sql_res As String = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" & str.Trim() & "' WHERE ReqID = '" & sResult.Trim() & "' AND ApiType = 'TOKENCHECKADMIN'"
            Dim x_res As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql_res))
            Dim SqlUpdate As String = ""
            Dim json As String = str
            data = convertJsonStringToDataSet(str)
            StatusApi = data.Tables(0).Rows(0)("transactionstatus").ToString()
            Dim Query As String = ""
            Dim hash_ As String = ""
            If StatusApi.ToUpper().Trim() = "SUCCESSFUL" Then
                Try
                    hash_ = data.Tables(0).Rows(0)("transactionHash").ToString()
                Catch ex As Exception
                Finally
                    If hash_ = "" Then
                        hash_ = ""
                    End If
                End Try
            End If

            Dim strs As String = ""
            strs &= "INSERT INTO ApiQrCodeReqResponse(Formno,Orderid,WalletAddress,PrivateKey,Request,Response,ApiStatus,RectimeStamp,ApiType,TxnHash,AMount,PostData,TypeB) "
            strs &= "VALUES('" & Formno_V & "','" & senderPrivateKey & "','" & senderAddress & "','" & senderPrivateKey & "','" & URL & "','" & str & "','" & StatusApi & "',"
            strs &= "GETDATE(),'Token Payout','" & hash_ & "','" & Decimal.Parse(Balance.ToString(), NumberStyles.Float) & "','" & postData & "','QrCode');"

            Query = " BEGIN TRY BEGIN TRANSACTION " & strs & " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH"
            Dim x As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Query)

            If x > 0 Then
                RetunnStatus = StatusApi
            End If
        Catch ex As Exception
            Dim errorQry As String = ""
            Dim ErrorMsg As String = ""
            Try
                ErrorMsg = ex.Message
            Catch eXX As Exception
            Finally
                If ErrorMsg = "" Then
                    ErrorMsg = ""
                End If
            End Try
            Dim sql_res As String = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" & ErrorMsg & "' WHERE ReqID = '" & sResult.Trim() & "' AND ApiType = 'TOKENCHECKADMIN'"
            Dim x_res As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql_res))
            errorQry = "INSERT INTO TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,formno)VALUES('" & ErrorMsg & "',GETDATE(),'" & URL & "','" & senderAddress.Trim() & "','" & postData & "','" & Formno_V & "')"
            Dim x1 As Integer = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, errorQry)

            If x1 > 0 Then
                RetunnStatus = "failed"
            End If
        End Try

        Return RetunnStatus
    End Function
    Private Function Base64Decode(ByVal base64EncodedData As String) As String
        Dim base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData)
        Return System.Text.Encoding.UTF8.GetString(base64EncodedBytes)
    End Function
    Public Shared Function Base64Encode(ByVal plainText As String) As String
        Dim plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText)
        Return System.Convert.ToBase64String(plainTextBytes)
    End Function
    Public Function convertJsonStringToDataSet(ByVal jsonString As String) As DataSet
        Dim xd As XmlDocument = New XmlDocument()
        jsonString = "{ ""rootNode"": {" & jsonString.Trim().TrimStart("{"c).TrimEnd("}"c) & "} }"
        xd = CType(JsonConvert.DeserializeXmlNode(jsonString), XmlDocument)
        Dim ds As DataSet = New DataSet()
        ds.ReadXml(New XmlNodeReader(xd))
        Return ds
    End Function
    Private Function Check_IdNo() As Boolean
        Try
            Dim Sql As String = ""
            Sql = " Exec Sp_GetIDDetail '" & TxtMemberID.Text & "'"
            Dim Dt_ As New DataTable
            Dt_ = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Sql).Tables(0)
            If Dt_.Rows.Count > 0 Then
                lblError.Text = ""
                TxtMemberName.Text = Dt_.Rows(0)("MemName")
                LblFormno.Text = Dt_.Rows(0)("Formno")
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
            Sql = " Exec Sp_Getformno '" & LblFormno.Text & "'"
            Dim Dt_ As New DataTable
            Dt_ = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Sql).Tables(0)
            If Dt_.Rows.Count > 0 Then
                TxtWalletAddress.Text = Dt_.Rows(0)("Address")
            End If
        End If
    End Sub
End Class
