Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Data.SqlClient
Partial Class Bills
    Inherits System.Web.UI.Page
    '   Dim ChinarAPI As New ChinarWebRef.Service
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim Ad As SqlDataAdapter
    Dim dt As DataTable
    Private dbConnect As cls_DataAccess
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'GetBillData()
            Try
                ' dbConnect = New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Session("AStatus") = "OK" Then
                    Get_BillDetails()
                End If
            Catch ex As Exception

            End Try

        End If


    End Sub

    'Private Sub GetBillData()
    '    Dim Str As System.Xml.XmlElement
    '    '        Str = ChinarAPI.GetIDWiseSaleXML(Session("IdNo"))
    '    'Ds.ReadXmlSchema()
    '    Dim xmlDS As New DataSet()
    '    Dim stream As StringReader = Nothing
    '    Dim reader As XmlTextReader = Nothing
    '    stream = New StringReader(Str.InnerXml)
    '    ' Load the XmlTextReader from the stream
    '    reader = New XmlTextReader(stream)
    '    xmlDS.ReadXml(reader)
    '    If xmlDS.Tables(0).Rows.Count > 0 Then
    '        lblInvoiceNoTxt.Text = xmlDS.Tables(0).Rows(0)("BillNo")
    '        lblDistIdtxt.Text = xmlDS.Tables(0).Rows(0)("IDNo")
    '        lblDistNametxt.Text = xmlDS.Tables(0).Rows(0)("MemName")
    '        'lblDistAddresstxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        lblInvoiceDateText.Text = xmlDS.Tables(0).Rows(0)("BillDate")
    '        'lblCourierNametxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblCnNOtxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblCNDatetxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblRoundOfftxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        lblNetPayabletxt.Text = xmlDS.Tables(0).Rows(0)("NetPayable")
    '        'lbltaxpercentage.Text = xmlDS.Tables(0).Rows(0)("Tax")'
    '        'lblAmountValueTxt.Text = xmlDS.Tables(0).Rows(0)("DP")'
    '        'lblTaxAmountTxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblTotalAmountTxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblTaxAmountTotaltxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblTotalAmounttotaltxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblTinNotxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblCINNotxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblCSTNotxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblPANNotxt.Text = xmlDS.Tables(0).Rows(0)("")
    '        'lblCashier.Text = xmlDS.Tables(0).Rows(0)("")
    '    End If
    '    GridView1.DataSource = xmlDS.Tables(0)
    '    GridView1.DataBind()
    'End Sub

    Private Sub Get_BillDetails()
        Dim str As String
        'Conn = New SqlConnection("Server=5.230.129.156;UID=usrchinar;PWD=chin@rrgr0x21p;Database=ChinarrGroupInv;Pooling=False;Connect Timeout=100000000;")
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        'Comm = New SqlCommand("Select * from V#DailyPayoutDetail as a,D_SessNmaster as b where a.IDNO='" & Session("Formno") & "' and  Order By PayoutNo")
        Comm = New SqlCommand("select a.FCode,b.PartyName,b.UserBillNo,b.BillDate,a.ProductId,a.ProductName,a.Rate,Cast(a.Qty as int) as Qty,a.BV,a.DiscountPer,a.Discount,a.NetAmount,a.Tax,a.TaxAmount,b.NetPayable,b.RndOff,a.DP,b.CourierName,b.DocketNo,b.DocketDate from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as b JOIN " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillDetails as a ON a.BillNo=b.BillNo where b.FormNo = " & Request("Id") & " AND b.UserBillNo='" & Request("BillNo") & "' ")
        Comm.Connection = Conn
        Ad = New SqlDataAdapter(Comm)
        dt = New DataTable
        Ad.Fill(dt)
        If dt.Rows.Count > 0 Then
            lblOfficeName.Text = Session("CompName")
            lblAddressTop.Text = Session("CompAdd")
            lblRegdOffice.Text = "Regd Office : " & Session("CompName") & ", " & Session("CompAdd")
            lblInvoiceNoTxt.Text = dt.Rows(0)("UserBillNo")
            lblDistIdtxt.Text = dt.Rows(0)("FCode")
            lblDistNametxt.Text = dt.Rows(0)("PartyName")
            lblDistAddresstxt.Text = Session("Address")
            lblInvoiceDateText.Text = dt.Rows(0)("BillDate")
            lblCourierNameTxt.Text = dt.Rows(0)("CourierName")
            lblCnNoTxt.Text = dt.Rows(0)("DocketNo")
            lblCNMI.Text = "For " & Session("CompName")
            If IsDBNull(dt.Rows(0)("DocketDate")) Then
                lblCNDatetxt.Text = ""
            Else
                lblCNDatetxt.Text = dt.Rows(0)("DocketDate")
            End If
            lblRoundOfftxt.Text = dt.Rows(0)("RndOff")
            lblNetPayabletxt.Text = dt.Rows(0)("NetPayable")

            GridView1.DataSource = dt
            GridView1.DataBind()
            If Conn.State = ConnectionState.Closed Then
                Conn.Open()
            End If

            Comm = New SqlCommand("Select Tax,Sum(NetAmount) as Amount,Sum(TaxAmount) as TaxAmount,Round(Sum(NetAmount+TaxAmount),2) as NetAmount From " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillDetails Where Tax>0 AND BillNo='" & Request("BillNo") & "' Group By Tax")
            Comm.Connection = Conn
            Ad = New SqlDataAdapter(Comm)
            dt = New DataTable
            Ad.Fill(dt)
            'If dt.Rows.Count > 0 Then
            '    lbltaxpercentage.Text = dt.Rows(0)("Tax")
            '    lblAmountValueTxt.Text = dt.Rows(0)("Amount")
            '    lblTaxAmountTxt.Text = dt.Rows(0)("TaxAmount")
            '    lblTotalAmountTxt.Text = dt.Rows(0)("NetAmount")

            'End If
            RptTax.DataSource = dt
            RptTax.DataBind()
            'lblTaxAmountTotaltxt.Text = dt.Rows(0)("TaxAmount")
            'lblTotalAmounttotaltxt.Text = dt.Rows(0)("")
            lblTinNotxt.Text = Session("CompTinNo")
            ' lblCINNotxt.Text = xmlDS.Tables(0).Rows(0)("")
            lblCSTNotxt.Text = Session("CompCSTNo")
            lblPANNotxt.Text = Session("CompPANNo")
            ''lblCashier.Text = xmlDS.Tables(0).Rows(0)("")
        End If
        Session("DirectData1") = dt
        Conn.Close()
    End Sub

End Class
