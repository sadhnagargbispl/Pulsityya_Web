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



Partial Class onlinetransactionnew
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim dtData As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
            Else
                Response.Redirect("logout.aspx")
            End If

            If Not Page.IsPostBack Then
                Filldate()
                FillReport()
                btnExport.Visible = False

           
            End If



        Catch ex As Exception

        End Try
    End Sub


    Private Sub Filldate()
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
            dtData = New DataTable
            dtData = objDAL.GetData(Str)
            If dtData.Rows.Count > 0 Then
                txtStartDate.Text = dtData.Rows(0)("CurrentDate")
                txtEndDate.Text = dtData.Rows(0)("CurrentDate")
            End If
        Catch ex As Exception

        End Try
    End Sub
    

    Private Sub FillReport()
        Try

            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            Dim Idno As String = "0"

            Dim qry1 As String = ""
            qry1 = "select b.Idno,a.MemberName,a.Orderid,a.TotalAmount,a.TransactionCharge,a.Amount," & _
            "Replace(Convert(Varchar,A.RectimeStamp,106),' ','-')As OrderDate," & _
            "case when a.Status='ACTIVE' then 'Failed' else a.Status end as status,Case when Status='' then 'True' else 'False' end as VisibleStatus," & _
            "'https://discountmart.in/OnlinePymentApi/CheckPaymentStatus?Orderid='+Orderid as Clickurl,Acttype " & _
            "from onlinetransaction as a, M_memberMaster as b where a.formno=b.formno" & Condition & ""

            Dim startDate As Date
            Dim endDate As Date

            If ChkMember.Checked = True Then
                   qry1 &= " And  Idno = '" & txtMemberId.Text & "'"
            End If

            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                qry1 &= "     And  Cast(A.RectimeStamp As Date) >= '" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                qry1 &= "     And  Cast(A.RectimeStamp As Date) <= '" & txtEndDate.Text & "'"
            End If
            If CmbMessage.SelectedValue <> "All" Then
                If CmbMessage.SelectedValue = "Pending" Then
                    qry1 &= " And Status=' '"
                Else
                    qry1 &= " And Status='" & CmbMessage.SelectedValue & "'"
                End If

            End If

            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(qry1)
            GvData.DataSource = dt
            GvData.DataBind()
            If dt.Rows.Count > 0 Then
                btnExport.Visible = True
            Else
                btnExport.Visible = False

            End If
            ViewState("WithDrawDate") = "BankCode"
            ViewState("Sort_Order") = "ASC"
            Session("GDataNew") = dt



        Catch ex As Exception

        End Try
    End Sub
    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

        Dim orderid, scrname As String
            Dim GVRw As GridViewRow
            GVRw = CType(sender.Parent.Parent, GridViewRow)
            'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Texāst
            orderid = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text

            Dim apiUrl As String = "https://api.cashfree.com/pg/orders/ORDER_" & orderid & "/payments"
            Dim apiVersion As String = "2022-09-01"
            Dim clientId As String = "167039f0a3bf29bd3ad61dfdaa930761"
            Dim clientSecret As String = "7d80011f9b0902863ccf43ecc401f3904d889d5c"
            Dim responseJson As String = MakeApiRequest(apiUrl, apiVersion, clientId, clientSecret)
            Dim result As String = ""
            result = MakeApiRequest(apiUrl, apiVersion, clientId, clientSecret)
            If result <> "" Then
                Dim message As String = result
                Dim status As String = ""
                ' Dim json As String = result
                Dim jss = New JavaScriptSerializer()
                Dim data = jss.Deserialize(Of Object)(message)
                status = data(0)("payment_status")
                Dim i As Integer = 0
                Dim str As String = "exec sp_saveresponseStaus '" & message & "','" & orderid & "','" & status & "'; "
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                i = objDAL.SaveData(str)

                If status.ToString.ToUpper = "SUCCESS" Then
                    Dim strqry As String = ""
                    strqry = "select * from Onlinetransaction where OrderId='" & orderid & "'"
                    dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strqry).Tables(0)

                    If i > 0 And dt.Rows.Count > 0 Then
                        '   Dim sql As String = ""
                        If Val(dt.Rows(0)("Kitid")) = 0 And dt.Rows(0)("Acttype") = "" Then
                            strqry = "exec sp_addMoney '" & message & "','" & orderid & "','" & status & "';"
                        ElseIf Val(dt.Rows(0)("Kitid")) > 0 Then
                            strqry = "exec Sp_Activateidpay '" & dt.Rows(0)("toidno") & "'," & dt.Rows(0)("formno") & ",'" & orderid & "' "
                        ElseIf dt.Rows(0)("Acttype") = "ReGallery" Then

                            strqry = "exec Sp_Regallerypay " & dt.Rows(0)("toidno") & ",'" & orderid & "' "

                        ElseIf dt.Rows(0)("Acttype") = "Community" Then

                            strqry = "exec Sp_Communitypay " & dt.Rows(0)("toidno") & ",'" & orderid & "' "
                        Else
                            strqry = "exec Sp_shoppingpay " & dt.Rows(0)("toidno") & ",'" & orderid & "' "
                        End If

                        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                        i = objDAL.SaveData(strqry)
                        If i > 0 Then
                            scrname = "<SCRIPT language='javascript'>alert('Transaction Successfully Saved');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)

                        End If
                    End If

                Else
                    scrname = "<SCRIPT language='javascript'>alert('Transaction Failed');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)

                End If
            Else
                scrname = "<SCRIPT language='javascript'>alert('No Response From Payment Gateway');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)

            End If
        Catch ex As Exception

        End Try

        FillReport()
    End Sub

    Function MakeApiRequest(ByVal apiUrl As String, ByVal apiVersion As String, ByVal clientId As String, ByVal clientSecret As String) As String
        System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType)
        Dim request As HttpWebRequest = CType(WebRequest.Create(apiUrl), HttpWebRequest)
        request.Method = "GET"
        If Not request.Headers.AllKeys.Contains("x-api-version") Then
            request.Headers.Add("x-api-version", apiVersion)
        End If
        If Not request.Headers.AllKeys.Contains("x-client-id") Then
            request.Headers.Add("x-client-id", clientId)
        End If
        If Not request.Headers.AllKeys.Contains("x-client-secret") Then
            request.Headers.Add("x-client-secret", clientSecret)
        End If
        Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
        Try
            If response.StatusCode = HttpStatusCode.OK Then
                Using reader As New StreamReader(response.GetResponseStream())
                    Return reader.ReadToEnd()
                End Using
            Else
                Console.WriteLine("Error: {response.StatusCode} - {response.StatusDescription}")
                Return Nothing
            End If
        Finally
            response.Close()
        End Try
    End Function



    'Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try

    '        Dim orderid, scrname As String
    '        Dim GVRw As GridViewRow
    '        GVRw = CType(sender.Parent.Parent, GridViewRow)
    '        'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Texāst
    '        orderid = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text

    '        Dim reqJson As String = "{""Orderid"":""" & orderid & """}"


    '        Dim httpWebRequest = CType(WebRequest.Create("https://discountmart.in/OnlinePymentApi/CheckPaymentStatus"), HttpWebRequest)
    '        httpWebRequest.ContentType = "application/json"
    '        httpWebRequest.Method = "POST"
    '        Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
    '            streamWriter.Write(reqJson)
    '            streamWriter.Flush()
    '            streamWriter.Close()
    '        End Using
    '        Dim result As String = ""
    '        Dim httpResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
    '        Using streamReader = New StreamReader(httpResponse.GetResponseStream())
    '            result = streamReader.ReadToEnd()
    '        End Using





    '        Dim ds As New DataSet()
    '        ds = convertJsonStringToDataSet(result)
    '        If result <> "" Then


    '            Dim message As String = ds.Tables(0).Rows(0)("Resp_Value")
    '            Dim status As String = ""
    '            ' Dim json As String = result
    '            Dim jss = New JavaScriptSerializer()
    '            Dim data = jss.Deserialize(Of Object)(message)
    '            status = data("order_status")
    '            Dim i As Integer = 0
    '            Dim str As String = "exec sp_saveresponseStaus '" & message & "','" & orderid & "','" & status & "'; "
    '            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '            i = objDAL.SaveData(str)

    '            If status.ToString.ToUpper = "PAID" Then
    '                Dim strqry As String = ""
    '                strqry = "select * from Onlinetransaction where OrderId='" & orderid & "'"
    '                dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strqry).Tables(0)

    '                If i > 0 And dt.Rows.Count > 0 Then
    '                    '   Dim sql As String = ""
    '                    If Val(dt.Rows(0)("Kitid")) = 0 And dt.Rows(0)("Acttype") = "" Then
    '                        strqry = "exec sp_addMoney '" & message & "','" & orderid & "','" & status & "';"
    '                    ElseIf Val(dt.Rows(0)("Kitid")) > 0 Then
    '                        strqry = "exec Sp_Activateidpay '" & dt.Rows(0)("toidno") & "'," & dt.Rows(0)("formno") & ",'" & orderid & "' "
    '                    ElseIf dt.Rows(0)("Acttype") = "ReGallery" Then

    '                        strqry = "exec Sp_Regallerypay " & dt.Rows(0)("toidno") & ",'" & orderid & "' "

    '                    ElseIf dt.Rows(0)("Acttype") = "Community" Then

    '                        strqry = "exec Sp_Communitypay " & dt.Rows(0)("toidno") & ",'" & orderid & "' "
    '                    Else
    '                        strqry = "exec Sp_shoppingpay " & dt.Rows(0)("toidno") & ",'" & orderid & "' "
    '                    End If

    '                    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '                    i = objDAL.SaveData(strqry)
    '                    If i > 0 Then
    '                        scrname = "<SCRIPT language='javascript'>alert('Transaction Successfully Saved');" & "</SCRIPT>"
    '                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)

    '                    End If
    '                End If

    '            Else
    '                scrname = "<SCRIPT language='javascript'>alert('Transaction Failed');" & "</SCRIPT>"
    '                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)

    '            End If
    '        Else
    '            scrname = "<SCRIPT language='javascript'>alert('No Response From Payment Gateway');" & "</SCRIPT>"
    '            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)

    '        End If
    '    Catch ex As Exception

    '    End Try

    '    FillReport()
    'End Sub
    Public Function convertJsonStringToDataSet(ByVal jsonString As String) As DataSet
        Dim xd As XmlDocument = New XmlDocument()
        jsonString = "{ ""rootNode"": {" & jsonString.Trim().TrimStart("{"c).TrimEnd("}"c) & "} }"
        xd = CType(JsonConvert.DeserializeXmlNode(jsonString), XmlDocument)
        Dim ds As DataSet = New DataSet()
        ds.ReadXml(New XmlNodeReader(xd))
        Return ds
    End Function

    


    Private Sub FillReport_TeamBigway()
        Try

            'Dim sql As String = String.Empty
            'sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No.],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
            'sql &= " b.Rank,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
            'sql &= " from M_SessWiseBv  as A Inner join MstRanks As b on a.RankID = b.RankiD"
            'sql &= " left join M_SessnMAster as c on a.SessID = c.SessID "
            'sql &= " Left join M_memberMaster as d on a.formno = d.formno "
            'sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
            'If (ddlstate.SelectedIndex > 0) Then
            '    sql &= "      And  b.RankID = " & ddlstate.SelectedValue & " "
            'End If
            'If (txtMemberID.Text <> "") Then
            '    sql &= "     And  d.Idno = '" & txtMemberID.Text & "' "
            'End If



            'objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            'dt = objDAL.GetData(sql)
            'GvData.DataSource = dt
            'GvData.DataBind()

            'Session("GDataNew") = dt
        Catch ex As Exception

        End Try
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Session("CompID") = "1006" Then
        '    FillReport_TeamBigway()
        'Else
        FillReport()
        'End If

    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        'GvData.DataSource = Session("GDataNew")
        'GvData.DataBind()
        If Session("CompID") = "1006" Then
            FillReport_TeamBigway()
        Else
            FillReport()
        End If
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            'If Session("CompID") = "1006" Then
            '    FillReport_TeamBigway()
            'Else

            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            Dim Idno As String = "0"

            Dim qry1 As String = ""
            qry1 = "select b.Idno,a.MemberName,a.Orderid,a.TotalAmount,a.TransactionCharge,a.Amount," & _
            "Replace(Convert(Varchar,A.RectimeStamp,106),' ','-')As OrderDate," & _
            "case when a.Status='ACTIVE' then 'Failed' else a.Status end as Status " & _
            "from onlinetransaction as a, M_memberMaster as b where a.formno=b.formno" & Condition & ""

            Dim startDate As Date
            Dim endDate As Date

            If ChkMember.Checked = True Then
                qry1 &= " And  Idno = '" & txtMemberId.Text & "'"
            End If

            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                qry1 &= "     And  Cast(A.RectimeStamp As Date) >= '" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                qry1 &= "     And  Cast(A.RectimeStamp As Date) <= '" & txtEndDate.Text & "'"
            End If
            If CmbMessage.SelectedValue <> "All" Then
                If CmbMessage.SelectedValue = "Pending" Then
                    qry1 &= " And Status=' '"
                Else
                    qry1 &= " And Status='" & CmbMessage.SelectedValue & "'"
                End If

            End If


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(qry1)
            Session("GDataNew") = dt
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

End Class
