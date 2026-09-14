Imports System.Data
Imports System.Data.SqlClient
Partial Class DispatInvoice
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim CatIDQS As String
    Dim SType As String
    Dim objGen As clsGeneral = New clsGeneral
    'Private _isIntraState As Boolean

    'Private _totalTaxable As Decimal
    'Private _totalCgst As Decimal
    'Private _totalSgst As Decimal
    'Private _totalIgst As Decimal
    'Private _grandTotal As Decimal
    Private _isIntraState As Boolean = True

    Private _totalTaxable As Decimal = 0D
    Private _totalCgst As Decimal = 0D
    Private _totalSgst As Decimal = 0D
    Private _totalIgst As Decimal = 0D
    Private _grandTotal As Decimal = 0D
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim Ad As SqlDataAdapter



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        GetCompID()

        If Not IsPostBack Then
            LoadInvoice()
        End If
        If Session("compid").ToString() = "1105" Then
            GSTIN.Text = "08AAQCP5426J1ZW"
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
    Private Sub getData()
        Try
            Dim dbConnect As New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dbConnect.OpenConnection()
            Dim dRead As SqlDataReader
            Dim cmd As SqlCommand
            cmd = New SqlCommand("select * from M_CompanyMaster ", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                Session("CompState") = dRead("CompState")
            Else
                Session("CompName") = ""
                Session("CompAdd") = ""
                Session("CompWeb") = ""
                Session("Title") = "Welcome"
            End If
            dRead.Close()
        Catch
            Session("CompName") = ""
            Session("CompAdd") = ""
            Session("CompWeb") = ""
        End Try
    End Sub
    Private Sub LoadInvoice()

        ' 🔹 Base64 decode
        Dim encodedOrderNo As String = If(Request.QueryString("OrderNo"), "")
        Dim billNo As String = ""

        Try
            Dim data As Byte() = Convert.FromBase64String(encodedOrderNo)
            billNo = System.Text.Encoding.UTF8.GetString(data)
        Catch ex As Exception
            billNo = "0"
        End Try
        getData()
        ' 🔹 Session values
        Dim compState As String = If(Session("CompState") IsNot Nothing, Session("CompState").ToString().Trim(), "")

        Dim compID As String = Convert.ToString(Session("CompID"))
        Dim connStr As String = Convert.ToString(Session("MlmDatabase" & compID))

        ' ❌ अगर अभी भी blank है
        If String.IsNullOrEmpty(connStr) Then
            Throw New Exception("Connection string not found in Session.")
        End If

        Using con As New SqlConnection(connStr)
            con.Open()

            ' 🔹 Your existing methods
            LoadHeader(con, billNo, compState)

            Dim items = LoadItems(con, LblBillnumber.Text)
            Dim taxLines = ComputeTaxLines(items)

            RenderGstBadge()
            RenderTableHead()
            RenderTableBody(taxLines)
            RenderMobileCards(taxLines)
            RenderTotals()
        End Using

    End Sub
    Private Sub LoadHeader(ByVal con As SqlConnection, ByVal billNo As String, ByVal compState As String)

        Dim sql As String = "SELECT b.rid AS Id,b.BillNo,b.BillDate,m.mobl AS Mobile,c.id AS ReqID,m.idno AS MemberID,m.memfirstname AS MemberName,Statecodes AS CustStateCode,case when statename = '--Choose State Name--' then '' else statename end AS CustStateName," & _
            " CONCAT_WS(', ',NULLIF(UserAddress,''),NULLIF(c.City,''),NULLIF(c.District,''),NULLIF(case when statename = '--Choose State Name--' then '' else statename end,''),NULLIF(c.PinCode,'')) AS CustFullAddress " & _
            " From (SELECT *,ROW_NUMBER() OVER(PARTITION BY formno, kitid ORDER BY billdate DESC) rn FROM repurchincome) b " & _
            " JOIN(SELECT *,ROW_NUMBER() OVER(PARTITION BY formno, kitid ORDER BY reqdate DESC) rn FROM purchaseReq) c " & _
            "ON b.formno = c.formno AND b.kitid  = c.kitid AND b.rn     = c.rn JOIN m_membermaster m ON m.formno = b.formno WHERE b.BillNo = @ReqID"

        Using cmd As New SqlCommand(sql, con)

            ' 👉 yaha billNo ya ID jo use karna hai confirm karo
            cmd.Parameters.AddWithValue("@ReqID", billNo)

            Using dr As SqlDataReader = cmd.ExecuteReader()

                If Not dr.Read() Then Exit Sub

                Dim custStateCode As String = dr("CustStateCode").ToString().Trim()
                _isIntraState = compState.Equals(custStateCode, StringComparison.OrdinalIgnoreCase)

                LblBillno.Text = dr("BillNo").ToString()
                LblBillnumber.Text = dr("ReqID").ToString()

                LblBillDate.Text = Convert.ToDateTime(dr("BillDate")).ToString("dd MMM yyyy")
                LblMemberID.Text = dr("MemberID").ToString()
                LblMemberName.Text = dr("MemberName").ToString()
                LblName.Text = dr("MemberName").ToString()
                LblAddress.Text = dr("CustFullAddress").ToString()

                ' Mobile handling
                Dim mob As String = dr("Mobile").ToString().Trim()
                LblMobile.Text = mob
                LblMobile.Parent.Visible = Not String.IsNullOrEmpty(mob)

            End Using
        End Using

    End Sub
    Private Function LoadItems(ByVal con As SqlConnection, ByVal billNo As String) As List(Of InvoiceItem)

        Dim list As New List(Of InvoiceItem)()
        Dim sql As String = ""
        ' Session null check (IMPORTANT)
        If Session("compid") IsNot Nothing AndAlso Session("compid").ToString() = "1105" Then

            ' Hardcoded items
            list.Add(New InvoiceItem With {.ProductName = "Stem Life", .Description = "", .Qty = 1, .Rate = 3333.33D, .GstAmount = 166.66D, .GstRate = 5D, .Discount = 0D})

            list.Add(New InvoiceItem With {.ProductName = "Cordy Berry Gold", .Description = "", .Qty = 1, .Rate = 3333.33D, .GstAmount = 166.66D, .GstRate = 5D, .Discount = 0D})
        ElseIf Session("compid") IsNot Nothing AndAlso Session("compid").ToString() = "1103" Then
            sql = "SELECT " & _
                                    "'Gift Voucher' AS ProductName, " & _
                                    "'' AS Description, " & _
                                    "1 AS Qty, " & _
                                    "kitamount AS Rate, " & _
                                    "0 AS GstAmount, " & _
                                    "0 AS GstRate, " & _
                                    "0 AS Discount " & _
                                    "FROM M_kitmaster B " & _
                                    "JOIN purchaseReq c ON b.kitid = c.kitid " & _
                                    "WHERE c.id = @BillNo"
            Using cmd As New SqlCommand(sql, con)

                cmd.Parameters.Add("@BillNo", SqlDbType.VarChar).Value = billNo

                Using dr As SqlDataReader = cmd.ExecuteReader()

                    While dr.Read()

                        list.Add(New InvoiceItem With {.ProductName = dr("ProductName").ToString(), .Description = dr("Description").ToString(), .Qty = Convert.ToInt32(dr("Qty")), .Rate = Convert.ToDecimal(dr("Rate")), .GstAmount = Convert.ToDecimal(dr("GstAmount")), .GstRate = Convert.ToDecimal(dr("GstRate")), .Discount = Convert.ToDecimal(dr("Discount"))})

                    End While

                End Using

            End Using
        Else

            ' SQL Query
            sql = "SELECT " & _
                               "kitname AS ProductName, " & _
                               "'' AS Description, " & _
                               "1 AS Qty, " & _
                               "ROUND(kitamount / 1.05, 2) AS Rate, " & _
                               "ROUND((kitamount / 1.05) * 0.05, 2) AS GstAmount, " & _
                               "5 AS GstRate, " & _
                               "0 AS Discount " & _
                               "FROM M_kitmaster B " & _
                               "JOIN purchaseReq c ON b.kitid = c.kitid " & _
                               "WHERE c.id = @BillNo"

            Using cmd As New SqlCommand(sql, con)

                cmd.Parameters.Add("@BillNo", SqlDbType.VarChar).Value = billNo

                Using dr As SqlDataReader = cmd.ExecuteReader()

                    While dr.Read()

                        list.Add(New InvoiceItem With {.ProductName = dr("ProductName").ToString(), .Description = dr("Description").ToString(), .Qty = Convert.ToInt32(dr("Qty")), .Rate = Convert.ToDecimal(dr("Rate")), .GstAmount = Convert.ToDecimal(dr("GstAmount")), .GstRate = Convert.ToDecimal(dr("GstRate")), .Discount = Convert.ToDecimal(dr("Discount"))})

                    End While

                End Using

            End Using

        End If

        Return list

    End Function
    Private Function ComputeTaxLines(ByVal items As List(Of InvoiceItem)) As List(Of TaxLine)

        Dim lines As New List(Of TaxLine)()

        For Each item As InvoiceItem In items

            Dim taxable As Decimal = item.Qty * item.Rate

        Dim line As New TaxLine With {.ProductName = item.ProductName,.Description = item.Description,.Qty = item.Qty,.Rate = item.Rate,.Discount = item.Discount,.Taxable = taxable,.IsIntraState = _isIntraState}

            If _isIntraState Then

                Dim halfRate As Decimal = item.GstRate / 2D     ' 5 / 2 = 2.5
                Dim halfAmt As Decimal = Math.Round(item.GstAmount / 2D, 2)

                line.CgstPct = halfRate
                line.CgstAmt = halfAmt

                line.SgstPct = halfRate
                line.SgstAmt = halfAmt

                line.LineTotal = taxable + item.GstAmount

                _totalCgst += halfAmt
                _totalSgst += halfAmt

            Else

                line.IgstPct = item.GstRate
                line.IgstAmt = item.GstAmount

                line.LineTotal = taxable + item.GstAmount

                _totalIgst += item.GstAmount

            End If

            _totalTaxable += taxable

            lines.Add(line)

        Next

        _grandTotal = Math.Round(_totalTaxable + _totalCgst + _totalSgst + _totalIgst, 0, MidpointRounding.AwayFromZero)

        Return lines

    End Function
    Private Sub RenderGstBadge()

        If _isIntraState Then
            LitGstBadge.Text = "<span class='gst-type-badge intra'>CGST + SGST</span>"
        Else
            LitGstBadge.Text = "<span class='gst-type-badge inter'>IGST</span>"
        End If

    End Sub
    Private Sub RenderTableHead()

        Dim sb As New StringBuilder()

        sb.Append("<tr>")

        If Session("compid") IsNot Nothing AndAlso Session("compid").ToString() = "1103" Then
            sb.Append("<th>Voucher</th>")
        Else
            sb.Append("<th>Package</th>")
        End If
        sb.Append("<th>Qty</th>")
        sb.Append("<th>Rate</th>")
        sb.Append("<th>Taxable</th>")

        If _isIntraState Then
            sb.Append("<th class='cgst-col'>CGST %</th>")
            sb.Append("<th class='cgst-col'>CGST</th>")
            sb.Append("<th class='sgst-col'>SGST %</th>")
            sb.Append("<th class='sgst-col'>SGST</th>")
        Else
            sb.Append("<th class='igst-col'>IGST %</th>")
            sb.Append("<th class='igst-col'>IGST</th>")
        End If

        sb.Append("<th>Total</th>")
        sb.Append("</tr>")

        LitTableHead.Text = sb.ToString()

    End Sub
    Private Sub RenderTableBody(ByVal lines As List(Of TaxLine))

        Dim sb As New StringBuilder()

        For Each l As TaxLine In lines

            sb.Append("<tr>")
            sb.Append("<td>" & l.ProductName & "</td>")
            sb.Append("<td>" & l.Qty & "</td>")
            sb.Append("<td class='num'>" & l.Rate.ToString("N2") & "</td>")
            sb.Append("<td class='num'>" & l.Taxable.ToString("N2") & "</td>")

            If _isIntraState Then

                sb.Append("<td class='cgst-col'>" & l.CgstPct.ToString() & "%</td>")
                sb.Append("<td class='num cgst-col'>" & l.CgstAmt.ToString("N2") & "</td>")

                sb.Append("<td class='sgst-col'>" & l.SgstPct.ToString() & "%</td>")
                sb.Append("<td class='num sgst-col'>" & l.SgstAmt.ToString("N2") & "</td>")

            Else

                sb.Append("<td class='igst-col'>" & l.IgstPct.ToString() & "%</td>")
                sb.Append("<td class='num igst-col'>" & l.IgstAmt.ToString("N2") & "</td>")

            End If

            sb.Append("<td>" & Math.Round(l.LineTotal, 0, MidpointRounding.AwayFromZero).ToString("N0") & "</td>")
            sb.Append("</tr>")

        Next

        LitTableBody.Text = sb.ToString()

    End Sub
    Private Sub RenderMobileCards(ByVal lines As List(Of TaxLine))

        Dim sb As New StringBuilder()

        For Each l As TaxLine In lines

            sb.Append("<div class='m-item'>")

            sb.Append("<div class='m-item-header'>")
            sb.Append("<div class='m-item-name'>" & l.ProductName & "</div>")
            sb.Append("<div class='m-item-total'>&#8377;" & l.LineTotal.ToString("N2") & "</div>")
            sb.Append("</div>")

            sb.Append("<div class='m-item-desc'>Qty: " & l.Qty & _
                      " &middot; Rate: &#8377;" & l.Rate.ToString("N2") & _
                      "</div>")

            sb.Append("<div class='m-grid'>")

            sb.Append("<div class='gc'><label>Taxable</label><span>" & _
                      l.Taxable.ToString("N2") & _
                      "</span></div>")

            If l.IsIntraState Then

                sb.Append("<div class='gc'><label>GST Rate</label><span>" & _
                          (l.CgstPct + l.SgstPct).ToString() & _
                          "%</span></div>")

                sb.Append("<div class='gc cgst'><label>CGST " & _
                          l.CgstPct.ToString() & _
                          "%</label><span>" & _
                          l.CgstAmt.ToString("N2") & _
                          "</span></div>")

                sb.Append("<div class='gc sgst'><label>SGST " & _
                          l.SgstPct.ToString() & _
                          "%</label><span>" & _
                          l.SgstAmt.ToString("N2") & _
                          "</span></div>")

            Else

                sb.Append("<div class='gc'><label>GST Type</label><span>IGST</span></div>")

                sb.Append("<div class='gc igst'><label>IGST " & _
                          l.IgstPct.ToString() & _
                          "%</label><span>" & _
                          l.IgstAmt.ToString("N2") & _
                          "</span></div>")

            End If

            sb.Append("</div>") ' m-grid
            sb.Append("</div>") ' m-item

        Next

        LitMobileCards.Text = sb.ToString()

    End Sub
    Private Sub RenderTotals()

        Dim sb As New StringBuilder()

        sb.Append(Row("Taxable Amount", _totalTaxable))

        If _isIntraState Then

            sb.Append(Row("CGST", _totalCgst))
            sb.Append(Row("SGST", _totalSgst))

        Else

            sb.Append(Row("IGST", _totalIgst))

        End If


        ' Actual sum before rounding
        Dim actualTotal As Decimal = _totalTaxable + _totalCgst + _totalSgst + _totalIgst
        Dim roundOff As Decimal = _grandTotal - actualTotal
        sb.Append(Row("Grand Total", actualTotal))
        ' Round off row (only if non-zero)
        If roundOff <> 0D Then

            Dim sign As String = ""

            If roundOff > 0 Then
                sign = "+"
            End If

            sb.Append("<div class='totals-row'>")
            sb.Append("<span class='lbl'>Round Off</span>")
            sb.Append("<span class='val'>" & _
                      sign & _
                      roundOff.ToString("N2") & _
                      "</span>")
            sb.Append("</div>")

        End If


        ' Final TOTAL row
        sb.Append("<div class='totals-row grand'>")
        sb.Append("<span class='lbl'>TOTAL</span>")
        sb.Append("<span class='val'>&#8377;" & _
                  _grandTotal.ToString("N0") & _
                  "</span>")
        sb.Append("</div>")


        LitTotals.Text = sb.ToString()

    End Sub
    Private Function Row(ByVal label As String, ByVal value As Decimal) As String

        Return "<div class='totals-row'>" & _
           "<span class='lbl'>" & label & "</span>" & _
           "<span class='val'>" & value.ToString("N2") & "</span>" & _
           "</div>"

    End Function
    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As EventArgs)

        Response.Redirect("Dispatchproductreport.aspx")

    End Sub
End Class
Public Class InvoiceItem

    Private _ProductName As String
    Public Property ProductName() As String
        Get
            Return _ProductName
        End Get
        Set(ByVal value As String)
            _ProductName = value
        End Set
    End Property


    Private _Description As String
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property


    Private _Qty As Integer
    Public Property Qty() As Integer
        Get
            Return _Qty
        End Get
        Set(ByVal value As Integer)
            _Qty = value
        End Set
    End Property


    Private _Rate As Decimal
    Public Property Rate() As Decimal
        Get
            Return _Rate
        End Get
        Set(ByVal value As Decimal)
            _Rate = value
        End Set
    End Property


    Private _GstAmount As Decimal
    Public Property GstAmount() As Decimal
        Get
            Return _GstAmount
        End Get
        Set(ByVal value As Decimal)
            _GstAmount = value
        End Set
    End Property


    Private _GstRate As Decimal
    Public Property GstRate() As Decimal
        Get
            Return _GstRate
        End Get
        Set(ByVal value As Decimal)
            _GstRate = value
        End Set
    End Property


    Private _Discount As Decimal
    Public Property Discount() As Decimal
        Get
            Return _Discount
        End Get
        Set(ByVal value As Decimal)
            _Discount = value
        End Set
    End Property

End Class
Public Class TaxLine


    Private _ProductName As String
    Public Property ProductName() As String
        Get
            Return _ProductName
        End Get
        Set(ByVal value As String)
            _ProductName = value
        End Set
    End Property


    Private _Description As String
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property


    Private _Qty As Integer
    Public Property Qty() As Integer
        Get
            Return _Qty
        End Get
        Set(ByVal value As Integer)
            _Qty = value
        End Set
    End Property


    Private _Rate As Decimal
    Public Property Rate() As Decimal
        Get
            Return _Rate
        End Get
        Set(ByVal value As Decimal)
            _Rate = value
        End Set
    End Property


    Private _Discount As Decimal
    Public Property Discount() As Decimal
        Get
            Return _Discount
        End Get
        Set(ByVal value As Decimal)
            _Discount = value
        End Set
    End Property


    Private _Taxable As Decimal
    Public Property Taxable() As Decimal
        Get
            Return _Taxable
        End Get
        Set(ByVal value As Decimal)
            _Taxable = value
        End Set
    End Property


    Private _IsIntraState As Boolean
    Public Property IsIntraState() As Boolean
        Get
            Return _IsIntraState
        End Get
        Set(ByVal value As Boolean)
            _IsIntraState = value
        End Set
    End Property


    Private _CgstPct As Decimal
    Public Property CgstPct() As Decimal
        Get
            Return _CgstPct
        End Get
        Set(ByVal value As Decimal)
            _CgstPct = value
        End Set
    End Property


    Private _CgstAmt As Decimal
    Public Property CgstAmt() As Decimal
        Get
            Return _CgstAmt
        End Get
        Set(ByVal value As Decimal)
            _CgstAmt = value
        End Set
    End Property


    Private _SgstPct As Decimal
    Public Property SgstPct() As Decimal
        Get
            Return _SgstPct
        End Get
        Set(ByVal value As Decimal)
            _SgstPct = value
        End Set
    End Property


    Private _SgstAmt As Decimal
    Public Property SgstAmt() As Decimal
        Get
            Return _SgstAmt
        End Get
        Set(ByVal value As Decimal)
            _SgstAmt = value
        End Set
    End Property


    Private _IgstPct As Decimal
    Public Property IgstPct() As Decimal
        Get
            Return _IgstPct
        End Get
        Set(ByVal value As Decimal)
            _IgstPct = value
        End Set
    End Property


    Private _IgstAmt As Decimal
    Public Property IgstAmt() As Decimal
        Get
            Return _IgstAmt
        End Get
        Set(ByVal value As Decimal)
            _IgstAmt = value
        End Set
    End Property


    Private _LineTotal As Decimal
    Public Property LineTotal() As Decimal
        Get
            Return _LineTotal
        End Get
        Set(ByVal value As Decimal)
            _LineTotal = value
        End Set
    End Property


End Class
'Public Class InvoiceItem

'    Private _ProductName As String
'    Public Property ProductName() As String
'        Get
'            Return _ProductName
'        End Get
'        Set(ByVal value As String)
'            _ProductName = value
'        End Set
'    End Property

'    Private _Description As String
'    Public Property Description() As String
'        Get
'            Return _Description
'        End Get
'        Set(ByVal value As String)
'            _Description = value
'        End Set
'    End Property

'    Private _Qty As Integer
'    Public Property Qty() As Integer
'        Get
'            Return _Qty
'        End Get
'        Set(ByVal value As Integer)
'            _Qty = value
'        End Set
'    End Property

'    Private _Rate As Decimal
'    Public Property Rate() As Decimal
'        Get
'            Return _Rate
'        End Get
'        Set(ByVal value As Decimal)
'            _Rate = value
'        End Set
'    End Property

'    Private _GstAmount As Decimal
'    Public Property GstAmount() As Decimal
'        Get
'            Return _GstAmount
'        End Get
'        Set(ByVal value As Decimal)
'            _GstAmount = value
'        End Set
'    End Property

'    Private _Discount As Decimal
'    Public Property Discount() As Decimal
'        Get
'            Return _Discount
'        End Get
'        Set(ByVal value As Decimal)
'            _Discount = value
'        End Set
'    End Property

'End Class
'Public Class TaxLine

'    Private _ProductName As String
'    Public Property ProductName() As String
'        Get
'            Return _ProductName
'        End Get
'        Set(ByVal value As String)
'            _ProductName = value
'        End Set
'    End Property

'    Private _Description As String
'    Public Property Description() As String
'        Get
'            Return _Description
'        End Get
'        Set(ByVal value As String)
'            _Description = value
'        End Set
'    End Property

'    Private _Qty As Integer
'    Public Property Qty() As Integer
'        Get
'            Return _Qty
'        End Get
'        Set(ByVal value As Integer)
'            _Qty = value
'        End Set
'    End Property

'    Private _Rate As Decimal
'    Public Property Rate() As Decimal
'        Get
'            Return _Rate
'        End Get
'        Set(ByVal value As Decimal)
'            _Rate = value
'        End Set
'    End Property

'    Private _Discount As Decimal
'    Public Property Discount() As Decimal
'        Get
'            Return _Discount
'        End Get
'        Set(ByVal value As Decimal)
'            _Discount = value
'        End Set
'    End Property

'    Private _Taxable As Decimal
'    Public Property Taxable() As Decimal
'        Get
'            Return _Taxable
'        End Get
'        Set(ByVal value As Decimal)
'            _Taxable = value
'        End Set
'    End Property

'    Private _IsIntraState As Boolean
'    Public Property IsIntraState() As Boolean
'        Get
'            Return _IsIntraState
'        End Get
'        Set(ByVal value As Boolean)
'            _IsIntraState = value
'        End Set
'    End Property

'    Private _CgstPct As Decimal
'    Public Property CgstPct() As Decimal
'        Get
'            Return _CgstPct
'        End Get
'        Set(ByVal value As Decimal)
'            _CgstPct = value
'        End Set
'    End Property

'    Private _CgstAmt As Decimal
'    Public Property CgstAmt() As Decimal
'        Get
'            Return _CgstAmt
'        End Get
'        Set(ByVal value As Decimal)
'            _CgstAmt = value
'        End Set
'    End Property

'    Private _SgstPct As Decimal
'    Public Property SgstPct() As Decimal
'        Get
'            Return _SgstPct
'        End Get
'        Set(ByVal value As Decimal)
'            _SgstPct = value
'        End Set
'    End Property

'    Private _SgstAmt As Decimal
'    Public Property SgstAmt() As Decimal
'        Get
'            Return _SgstAmt
'        End Get
'        Set(ByVal value As Decimal)
'            _SgstAmt = value
'        End Set
'    End Property

'    Private _IgstPct As Decimal
'    Public Property IgstPct() As Decimal
'        Get
'            Return _IgstPct
'        End Get
'        Set(ByVal value As Decimal)
'            _IgstPct = value
'        End Set
'    End Property

'    Private _IgstAmt As Decimal
'    Public Property IgstAmt() As Decimal
'        Get
'            Return _IgstAmt
'        End Get
'        Set(ByVal value As Decimal)
'            _IgstAmt = value
'        End Set
'    End Property

'    Private _LineTotal As Decimal
'    Public Property LineTotal() As Decimal
'        Get
'            Return _LineTotal
'        End Get
'        Set(ByVal value As Decimal)
'            _LineTotal = value
'        End Set
'    End Property

'End Class