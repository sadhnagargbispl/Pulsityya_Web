Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Partial Class App_UI_Application_Pages_DispatchProductMaster

    Inherits System.Web.UI.Page
    Dim Ds As New DataSet()
    Dim dtData As New DataTable()
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim searchtext As String = Session("Search")
        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then
                GvData.Visible = False
                Filldate()
                'BindData(1)
                'BindCourier()
                Session("MemberList") = Nothing
                If searchtext <> "" Then

                End If
                'FillKit()
            End If
        Else
            Response.Redirect("LogOut.aspx")
        End If
    End Sub
    '
    Public Sub BindData(ByVal PageIndex As Integer)
        ' lblErr.Text = ""

         Dim recordCount As Integer
        '       Dim Sql As String = "select d.formno,t.Fcode,(d.MemFirstName+' '+d.MemLastName)As MemberName,d.Mobl as MobilenO,t.OrderNo,t.OrderDate,b.ProductName," & _
        '                        " b.DP as Rate,t.Qty,(b.DP*t.Qty) as TotalAmount,t.KitId,isnull(c.Couriername,' ') as CourierName,isnull(c.Docketno,'')as DocketNo," & _
        '                  "Case when DocketDate is null then ' ' else Cast(Replace(Convert(Varchar,DocketDate ,106),' ','-')as Varchar) end as DocketDate," & _
        '                    " Case when IsDispatch='Y' then 'Dispatched' else 'Pending' end as Status," & _
        '                           " Case when IsDispatch='Y' then 'False' else 'True' end as DispatchStatus" & _
        '       " from (select distinct(KitId) as KitId,Fcode,OrderNo,Replace(convert(Varchar,RectimeStamp,106),' ','-') as OrderDate,qty,IsDispatch from TrnKitProducts )as t" & _
        '       " Left Join" & _
        '" Trnbillmain  as c on t.Fcode=c.Fcode and t.orderno=c.orderno,M_productMaster as b,M_MemberMaster as d" & _
        '" where  Qty<>0 and t.fcode=d.idno and t.KitId=b.ProductCode and b.activeStatus='Y' " & Condition & " group by t.Fcode,t.Orderno,t.OrderDate,t.KitId,t.qty," & _
        '" c.Couriername,c.Docketno,c.DocketDate,b.ProductName,b.DP,t.IsDispatch,d.formno,d.MemFirstName,d.MemLastName,d.Mobl Order by t.orderno desc "
        'dtData = New DataTable
        'dtData = objDAL.GetData(Sql)
        'GvData.DataSource = dtData
        Dim prms As SqlParameter() = New SqlParameter(7) {}
        prms(0) = New SqlParameter("@IdNo", TxtMemId.Text)
        prms(1) = New SqlParameter("@StartDate", txtStartDate.Text)
        prms(2) = New SqlParameter("@EndDate", txtEndDate.Text)
        prms(3) = New SqlParameter("@IsDispatch", RbtSearch.SelectedValue)
        prms(4) = New SqlParameter("@IsExport", 0)
        prms(5) = New SqlParameter("@PageIndex", PageIndex)
        prms(6) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
        prms(7) = New SqlParameter("@RecordCount", ParameterDirection.Output)
        Ds = New DataSet()
        Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetDispatchMember", prms)
        If Ds.Tables(0).Rows.Count > 0 Then
            GvData.Visible = True
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()
            Session("TopUpData") = Ds.Tables(0)
            recordCount = Ds.Tables(1).Rows(0)("RecordCount")
            btnExport.Enabled = True
            For i As Integer = 0 To GvData.Columns.Count - 1
                Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                Dim img As New Image()
                img.ImageUrl = "~/Images/Uparrow.png"
                tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                tableCell.Controls.Add(img)
            Next
        Else
            GvData.Visible = False
            btnExport.Enabled = False

        End If
        ViewState("WithDrawDate") = "Fcode"
        ViewState("Sort_Order") = "ASC"
        Me.PopulatePager(recordCount, PageIndex)
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("TopUpData")
        GvData.DataBind()
    End Sub

    'Protected Sub btnshowall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnshowall.Click
    '    BindData(1)
    'End Sub
    'Public Sub BindCourier()
    '    'Dim sql As String = "Select * From(Select 0 As SessID,'-- ALL --' As SessnName Union ALL select SessID,Cast(SessID as varchar) + ' [' + Replace(Convert(varchar,FrmDate,106),' ','-') + ' to ' + ISNULL(Replace(Convert(varchar,ToDate,106),' ','-'),'') + ']' As SessnName from D_SessnMaster Where ToDate Is Not Null) As Temp order by SessID"
    '    Dim sql As String = "select * from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_CourierMaster where ActiveStatus='Y'"

    '    objModuleFun.FillCombo(sql, DDlCourier, "CourierName", "CourierId")
    'End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim condition As String = ""
        'If ChkMember.Checked Then
        '    If Check_IdNo() Then
        '        condition = condition & " And d.IdNo='" & TxtMemId.Text & "'"
        '    Else
        '        Exit Sub
        '    End If

        'End If
        'If txtStartDate.Text <> "" Then
        '    condition = condition & " And Cast(Convert(varchar,t.OrderDate,106) as DateTime)>='" & txtStartDate.Text & "'"
        'End If
        'If txtEndDate.Text <> "" Then
        '    condition = condition & " And Cast(Convert(varchar,t.OrderDate,106) as DateTime)<='" & txtEndDate.Text & "'"
        'End If
        'If RbtSearch.SelectedValue <> "A" Then
        '    condition = condition & "And t.IsDispatch='" & RbtSearch.SelectedValue & "'"
        'End If
        BindData(1)
    End Sub
    Private Function Check_IdNo() As Boolean
        Dim sql As String = ""
        sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName , Mobl,Address1 From " & objDAL.tblMemberMaster & " WHERE IDNO='" & Trim(TxtMemId.Text) & "'"
        Ds = New DataSet()
        Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql)
        If Ds.Tables(0).Rows.Count = 0 Then
            LblMemName.Text = " Please enter correct Member ID."
            LblMemName.ForeColor = Drawing.Color.Red
            TxtMemId.Text = ""
            ' LblAddress.Text = ""
            'LblAddress.Visible = False
            LblMemName.Visible = True
            Return False
        Else
            LblMemName.Visible = False
        End If
        Return True
    End Function
    Protected Sub DispatchData(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim GVRw As GridViewRow
            Dim lblformno As New Label
            Dim str As String = ""
            Dim i As Integer = 0
            Dim scrname As String
            str = ""
            ' Dim Lblformno As New Label
            GVRw = CType(sender.Parent.Parent, GridViewRow)
            TxtIdNo.Text = DirectCast(GVRw.FindControl("LblIdNo"), Label).Text
            lblformno.Text = DirectCast(GVRw.FindControl("LblFormNo"), Label).Text
            LblOrderNo.Text = DirectCast(GVRw.FindControl("LblOrderNo"), Label).Text
            Dim pageurl As String = "GstBill.Aspx?IDNo=" & TxtIdNo.Text & "&OrderNo=" & LblOrderNo.Text & ""
            str = "select * from TrnKitProducts where OrderNo='" & Val(LblOrderNo.Text) & "' and IsDispatch='N'"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dtData = New DataTable
            dtData = objDAL.GetData(str)
            If dtData.Rows.Count > 0 Then
                str = "Exec GenBill '" & LblOrderNo.Text & "',0,'','0','','Order Dispatch By" & Session("Username") & "'"
                i = objDAL.SaveData(str)
            End If
            If i > 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Dispatched Successfully!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Dispatched", scrname, False)
                Dim condition As String = ""
                If ChkMember.Checked Then
                    If Check_IdNo() Then
                        condition = condition & " And IdNo='" & TxtMemId.Text & "'"
                    Else
                        Exit Sub
                    End If
                End If
                BindData(1)
                Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>")
                'Response.Redirect("Bills.Aspx?IDNo=" & TxtIdNo.Text, False)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            'Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim prms As SqlParameter() = New SqlParameter(7) {}
            prms(0) = New SqlParameter("@IdNo", TxtMemId.Text)
            prms(1) = New SqlParameter("@StartDate", txtStartDate.Text)
            prms(2) = New SqlParameter("@EndDate", txtEndDate.Text)
            prms(3) = New SqlParameter("@IsDispatch", RbtSearch.SelectedValue)
            prms(4) = New SqlParameter("@IsExport", 1)
            prms(5) = New SqlParameter("@PageIndex", 1)
            prms(6) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(7) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = New DataSet()
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetDispatchMember", prms)
            'If Ds.Tables(0).Rows.Count > 0 Then
            '    dg.DataSource = Ds.Tables(0)
            '    dg.DataBind()
            'End If
            Session("GData1") = Ds.Tables(0)
            ExportExcel()
            'ExportToExcel("DispatchProductList.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
        Dim sw As New System.IO.StringWriter
        Dim htw As System.Web.UI.HtmlTextWriter
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.xlsx"
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
        Response.Charset = ""
        dg.EnableViewState = False
        htw = New HtmlTextWriter(sw)
        dg.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.End()
    End Sub

    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try


            ' If e.SortExpression = ViewState("FromIdno").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)

                    End If
                Next
            End If

            'Else
            'RebindData(e.SortExpression, "ASC")
            'End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Try
            Dim dt As DataTable = CType(Session("TopUpData"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GvData.DataSource = dt
            GvData.DataBind()
            ViewState("WithDrawDate") = sColimnName
            ViewState("Sort_Order") = sSortOrder
        Catch ex As Exception
        End Try
    End Sub
    Private Sub PopulatePager(ByVal recordCount As Integer, ByVal currentPage As Integer)
        Dim pages As New List(Of ListItem)()
        Dim startIndex As Integer, endIndex As Integer
        Dim pagerSpan As Integer = 5

        'Calculate the Start and End Index of pages to be displayed.
        Dim dblPageCount As Double = CDbl(CDec(recordCount) / Convert.ToDecimal(ddlPageSize.SelectedValue))
        Dim pageCount As Integer = CInt(Math.Ceiling(dblPageCount))
        startIndex = If(currentPage > 1 AndAlso currentPage + pagerSpan - 1 < pagerSpan, currentPage, 1)
        endIndex = If(pageCount > pagerSpan, pagerSpan, pageCount)
        If currentPage > pagerSpan Mod 2 Then
            If currentPage = 2 Then
                endIndex = 5
            Else
                endIndex = currentPage + 2
            End If
        Else
            endIndex = (pagerSpan - currentPage) + 1
        End If

        If endIndex - (pagerSpan - 1) > startIndex Then
            startIndex = endIndex - (pagerSpan - 1)
        End If

        If endIndex > pageCount Then
            endIndex = pageCount
            startIndex = If(((endIndex - pagerSpan) + 1) > 0, (endIndex - pagerSpan) + 1, 1)
        End If

        'Add the First Page Button.
        If currentPage > 1 Then
            pages.Add(New ListItem("First", "1"))
        End If

        'Add the Previous Button.
        If currentPage > 1 Then
            pages.Add(New ListItem("<<", (currentPage - 1).ToString()))
        End If

        For i As Integer = startIndex To endIndex
            pages.Add(New ListItem(i.ToString(), i.ToString(), i <> currentPage))
        Next

        'Add the Next Button.
        If currentPage < pageCount Then
            pages.Add(New ListItem(">>", (currentPage + 1).ToString()))
        End If

        'Add the Last Button.
        If currentPage <> pageCount Then
            pages.Add(New ListItem("Last", pageCount.ToString()))
        End If
        rptPager.DataSource = pages
        rptPager.DataBind()
    End Sub
    Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
            Me.BindData(pageIndex)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.BindData(1)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GData1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "DispatchProduct")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=DispatchProductMaster.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub

    Private Sub Filldate()
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
        dtData = New DataTable
        dtData = objDAL.GetData(Str)
        If dtData.Rows.Count > 0 Then
            txtStartDate.Text = dtData.Rows(0)("CurrentDate")
            txtEndDate.Text = dtData.Rows(0)("CurrentDate")
        End If



    End Sub
    Protected Sub BtnDispatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDispatch.Click
        Try


            Dim str As String = ""
            Dim strq As String = ""
            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim scrname As String = ""
            Dim CourierId As String = ""
            Dim CourierName As String
            'TxtIdNo.Text = "KTS129595"
            Dim pageurl As String = "GstBill.Aspx?IDNo=" & TxtIdNo.Text & ""
            If DDlCourier.SelectedItem.Text = "Other" Then
                CourierId = 0
                CourierName = TxtCourier.Text
            Else
                CourierId = Val(DDlCourier.SelectedValue)
                CourierName = DDlCourier.SelectedItem.Text
            End If

            '  If i > 0 Then


            'commentOn  1-july-2017 for GST
            str = "Exec " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..GenBill " & Val(LblOrderNo.Text) & "," & Val(CourierId) & ",'" & DDlCourier.SelectedItem.Text & "' ,'" & TxtDocket.Text & "' ,'" & LblAddress.Text & "'"
            j = objDAL.SaveData(str)
            '  End If

            If j > 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Dispatched Successfully!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Dispatched", scrname, False)
                Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>")
                'Response.Redirect("Bills.Aspx?IDNo=" & TxtIdNo.Text, False)
            End If
            DivTopup.Visible = False
            BindData(1)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub DDlCourier_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlCourier.SelectedIndexChanged
        If DDlCourier.SelectedItem.Text = "Other" Then
            OtherCourier.Visible = True
        Else
            OtherCourier.Visible = False

        End If
    End Sub

    Protected Sub BtnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnImport.Click
        Fill_CSV_DATA()
    End Sub
    Protected Sub Fill_CSV_DATA()  'Add: 26Oct16,Wednesday
        Try
            Dim Sql As String = ""
            Dim scrname As String = ""
            Dim dtIDList As New DataTable
            Dim connString As String = ""
            Dim Idno As String = ""
            ' Dim amount As Double = "0.0"
            Dim n As Integer
            Dim a As Integer
            Dim count As Integer = 0

            If FileUpload1.HasFile = True Then
                'SavePath
                Dim Updteffect As Integer
                Dim savePath As String = Server.MapPath("images/UploadImage/")

                ' Get the name of the file to upload.
                Dim fileName As String = FileUpload1.FileName

                ' Create the path and file name to check for duplicates.
                Dim pathToCheck As String = savePath + fileName

                ' Create a temporary file name to use for checking duplicates.
                Dim tempfileName As String

                ' Check to see if a file already exists with the
                ' same name as the file to upload.        
                If (System.IO.File.Exists(pathToCheck)) Then
                    Dim counter As Integer = 2
                    While (System.IO.File.Exists(pathToCheck))
                        ' If a file with this name already exists,
                        ' prefix the filename with a number.
                        tempfileName = counter.ToString() + fileName
                        pathToCheck = savePath + tempfileName
                        counter = counter + 1
                    End While
                    fileName = tempfileName
                End If
                ' Append the name of the file to upload to the path.
                savePath += fileName
                ' Call the SaveAs method to save the uploaded
                ' file to the specified directory.
                FileUpload1.SaveAs(savePath)
                'fileUpload.SaveAs(Server.MapPath("~/App_UI/Resources/Templates/"))
                Dim strFileType As String = Path.GetExtension(FileUpload1.FileName).ToLower()
                Dim path__1 As String = FileUpload1.PostedFile.FileName

                If String.Equals(strFileType, ".xls") = True Or String.Equals(strFileType, ".xlsx") = True Then
                    'Connection String to Excel Workbook
                    If strFileType.Trim() = ".xls" Then
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & savePath & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=2"""
                    ElseIf strFileType.Trim() = ".xlsx" Then
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & savePath & ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=2"""
                    End If

                    Dim objConn As New OleDbConnection(connString)
                    If objConn.State = ConnectionState.Closed Then
                        objConn.Open()
                    End If
                    ' Get the data table containg the schema guid.
                    Dim dbSchema As DataTable = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                    Dim firstSheetName As String = dbSchema.Rows(0)("TABLE_NAME").ToString()
                    Dim objCommand As New OleDbCommand("SELECT * FROM [" + firstSheetName + "]", objConn)
                    'Dim query As String = "SELECT * FROM [MemberID$]"
                    'Dim cmd As New OleDbCommand(Query, conn)
                    Dim da As New OleDbDataAdapter(objCommand)
                    dtIDList = New DataTable
                    da.Fill(dtIDList)
                    da.Dispose()
                    objConn.Close()
                    objConn.Dispose()

                    Dim str1 As String = ""
                    Dim qr As String = ""
                    Dim qr1 As String = ""
                    Dim Count_ = 0, Cnt As Integer = 0
                    'Dim Formno As String = ""
                    Dim St As New DataTable
                    Dim str As String = ""
                    'str = "select formno,OrderNo from TrnBillmain"
                    'objDAL = New DAL
                    'Dim Dt As DataTable
                    'Dt = New DataTable
                    'Dt = objDAL.GetData(str)
                    'Session("MemOrder") = Dt
                    'St = DirectCast(Session("MemOrder"), DataTable)
                    For i As Integer = 0 To dtIDList.Rows.Count - 1
                        If dtIDList.Rows(i)(0).ToString() <> "" Then
                            'Dim service As String
                            ' Dim Idno As String
                            If IsDBNull(dtIDList.Rows(i)(0)) = True Then
                                Idno = ""
                            Else
                                Idno = dtIDList.Rows(i)(0)
                                'Dim Dr() As DataRow = St.Select("IdNo='" & Idno.Trim & "'")
                                'formno = Dr(0)("Formno")
                            End If
                            Dim OrderNo As String = ""
                            Dim CourierName As String = ""
                            Dim DocketDate As DateTime
                            Dim DocketNo As String
                            Dim Mobl As String = ""
                            If IsDBNull(dtIDList.Rows(i)(1)) = True Then
                                OrderNo = 0
                            Else
                                OrderNo = dtIDList.Rows(i)(1)
                            End If
                            If IsDBNull(dtIDList.Rows(i)(2)) = True Then
                                CourierName = ""
                            Else
                                CourierName = dtIDList.Rows(i)(2)

                            End If
                            If IsDBNull(dtIDList.Rows(i)(3)) = True Then
                                DocketNo = ""
                            Else
                                DocketNo = dtIDList.Rows(i)(3)

                            End If
                            If IsDBNull(dtIDList.Rows(i)(4)) = True Then
                                DocketDate = Date.Now

                                DocketDate = Format(DocketDate, "dd-MMM-yyyy")

                            Else
                                DocketDate = dtIDList.Rows(i)(4)
                                DocketDate = Format(DocketDate, "dd-MMM-yyyy")

                            End If
                            If IsDBNull(dtIDList.Rows(i)(5)) = True Then
                                Mobl = ""
                            Else

                                Mobl = dtIDList.Rows(i)(5)
                            End If

                            Dim Dt1 As New DataTable
                            Dim CompId As Integer = 0
                            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))



                            If Idno <> "" And Val(OrderNo) <> 0 Then
                                qr = qr & "Update TrnBillMain Set CourierName='" & CourierName & "',DocketNo='" & DocketNo & "',DocketDate='" & DocketDate & "',DelvStatus='Y',LocName='" & Mobl & "' where Fcode='" & Idno.Trim & "' and Orderno='" & OrderNo & "' and DelvStatus='P';"
                                count = count + 1
                                'Formno = 0
                                Idno = ""
                            End If

                        End If

                    Next
                    Dim str2 As String = ""
                    Dim k As Integer = 0
                    'str1 = str1.Remove(str1.Length - 1)
                    'qr = qr.Remove(qr.Length - 1)

                    If qr <> "" Then

                        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                        k = objDAL.SaveData1(qr)
                    End If


                    If k <> 0 And count <> 0 Then

                        scrname = "<SCRIPT language='javascript'>alert('" & k & "Record Successfuly Saved');" & "</SCRIPT>"
                        Me.RegisterStartupScript("MyAlert", scrname)
                        '  Load_Data()
                    Else

                    End If
                    BtnImport.Enabled = True
                End If
            End If

        Catch ex As Exception
        End Try

    End Sub

    Protected Sub Btnsummery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnsummery.Click
        Try
            Dim dtTemp1 As New DataTable
            Dim dg1 As New DataGrid
            Dim condition As String = ""

            Dim search As String = ""
            'Dim condition As String = ""
            search = Session("Search")

            'Dim sql As String = "select b.Idno,(b.MemFirstName+ ' '+b.MemLastname) as MemName,ProductName,Barcode,Qty as Quantity,Replace(Convert(Varchar,a.RecTimeStamp,106),' ','-') + ' '+ " & _
            '    " CONVERT(varchar(15), CAST(a.RecTimeStamp AS TIME),100) as Date from TrnkitProducts as a,M_MemberMaster as b " & _
            '     " where a.Fcode=b.Idno  and Istopup='Y' and Fld2='N'  and Qty=0 and b.IsCompId='N' " & condition & " order by Idno"
            Dim Qry As String = " select a.Fcode as [IdNo],(c.MemFirstName+' '+c.MemLastName)As [Member Name]," & _
                         " c.Mobl as [Mobile No],a.OrderNo,Replace(Convert(Varchar,a.OrderDate,106),' ','-')As [Order Date],b.ProductName,b.DP as Rate,a.Qty,(b.DP*a.Qty) as [Total Amount]" & _
         ", Case when IsDispatch='Y' then 'Dispatched' else 'Pending' end as Status " & _
         " from (select distinct(KitId) as KitId,Fcode,OrderNo,Cast( RectimeStamp as Date) as OrderDate,qty,IsDispatch,Cast(DispatchDate as Date)as Dispatchdate" & _
    " from TrnKitProducts where qty<>0 group by Fcode,Orderno,Cast(RecTimeStamp as Date),KitId,qty,IsDispatch,Cast(Dispatchdate as Date)) as a, M_productMaster as b ,M_MemberMaster as c" & _
" where a.Fcode=c.Idno and  a.KitId=b.ProductCode and b.activeStatus='Y'  Order by a.orderno desc"


            dtTemp1 = New DataTable
            dtTemp1 = objDAL.GetData(Qry)

            dg1.DataSource = dtTemp1
            dg1.DataBind()

            ExportToExcel("DispatchProductSummery.xls", dg1)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub
End Class
