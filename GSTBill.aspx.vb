Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Data.SqlClient
Partial Class GSTBill
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



    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Dim i As Integer
        'For Each Gvr As GridViewRow In GridView1.Rows
        If e.Row.RowType = DataControlRowType.DataRow Then
            If DirectCast(e.Row.FindControl("LblQty"), Label).Visible = True Then
                e.Row.Style("font-size") = "smaller"
            Else
                e.Row.Style("font-size") = "small"
            End If

        End If

    End Sub
    Public Function AmountInWords(ByVal MyNumber) As String 'Linking With Lacs,Crores & Billion...OnDated: 14.Jan.2013,Monday
        ' This Function To Convert Number to Word By GR

        Dim Temp, Temp_2, Temp_Num
        Dim Rupees, Ps
        Dim DecimalPlace, Count
        Dim Place() As String
        ReDim Place(9)
        Temp = "" : Ps = "" : Rupees = ""
        Temp_2 = ""
        Temp_Num = ""
        Place(2) = " Thousand "
        Place(3) = " Lac " ' add: new
        Place(4) = " Crore " 'Million
        Place(5) = " Billion " '4
        Place(6) = " Trillion " '5

        ' Convert MyNumber to a string, trimming extra spaces.
        'MsgBox(Str(CDbl(MyNumber)))
        MyNumber = Str(CDbl(MyNumber)).Trim
        ' MsgBox(MyNumber)
        ' Find decimal place.
        DecimalPlace = InStr(MyNumber, ".")

        ' If we find decimal place...
        If DecimalPlace > 0 Then
            ' Convert Ps
            Temp = Microsoft.VisualBasic.Left(Mid(MyNumber, DecimalPlace + 1) & "00", 2)
            Ps = ConvertTens(Temp)
            ' Strip off Ps from remainder to convert.
            MyNumber = Microsoft.VisualBasic.Left(MyNumber, DecimalPlace - 1).Trim
        End If

        Count = 1
        '*** New Loop For Lacs & Crore *****
        Do While MyNumber <> ""
            ' Convert last 3 digits of MyNumber to English Rupees.
            If Count = 1 And Len(MyNumber) = 2 Or Count = 1 Then
                Temp = ConvertHundreds(Microsoft.VisualBasic.Right(MyNumber, 3))

                '***** Lac Series >> 12Jan13
                'ElseIf (Len(MyNumber) = 3 Or Len(MyNumber) = 4) And Count = 2 Then


            ElseIf Len(MyNumber) >= 1 And Count >= 2 Then
                'MyNumber = "115121"
                If Len(MyNumber) = 1 Then
                    Temp_Num = Microsoft.VisualBasic.Left(MyNumber, Len(MyNumber) - 1)
                Else
                    Temp_Num = Microsoft.VisualBasic.Left(MyNumber, Len(MyNumber) - 2)
                End If

                MyNumber = Microsoft.VisualBasic.Right(MyNumber, 2)
                'MyNumber = Microsoft.VisualBasic.Mid(MyNumber, Len(MyNumber) - 2, 2)
                If Len(MyNumber) = 1 Then
                    Temp = ConvertDigit(Microsoft.VisualBasic.Left(MyNumber, 1))
                ElseIf Len(MyNumber) = 2 Then
                    Temp = ConvertTens(Microsoft.VisualBasic.Left(MyNumber, 2))
                End If
                MyNumber = Temp_Num
            Else
                Temp = "" : MyNumber = ""
            End If


            If Temp <> "" Then 'Temp <> "" And Count < 7 Then
                Rupees = Temp & Place(Count) & Rupees
            End If
            'If Temp <> ""  Then Rupees = Temp & Place(Count) & Rupees
            If Len(MyNumber) <= 2 And Count = 1 Then
                MyNumber = ""
            ElseIf Len(MyNumber) >= 3 And Count = 1 Then
                ' Remove last 3 converted digits from MyNumber.
                MyNumber = Microsoft.VisualBasic.Left(MyNumber, Len(MyNumber) - 3)
            End If
            Count = Count + 1
        Loop

        ' Clean up Rupees.
        Select Case Rupees
            Case ""
                Rupees = "No Rupees"
            Case "One"
                Rupees = "Rupee One" '"One "
            Case Else
                Rupees = Rupees & " Rupees "
        End Select

        ' Clean up Ps.
        Select Case Ps
            Case ""
                Ps = ""
            Case "One"
                Ps = " And One Paise"
            Case Else
                Ps = " And " & Ps & " Paise"
        End Select
        AmountInWords = Rupees & Ps & " Only"
    End Function

    Public Function ConvertTens(ByVal MyTens)
        Dim Result As String

        ' Is value between 10 and 19?
        If Val(Microsoft.VisualBasic.Left(MyTens, 1)) = 1 Then
            Select Case Val(MyTens)
                Case 10 : Result = "Ten"
                Case 11 : Result = "Eleven"
                Case 12 : Result = "Twelve"
                Case 13 : Result = "Thirteen"
                Case 14 : Result = "Fourteen"
                Case 15 : Result = "Fifteen"
                Case 16 : Result = "Sixteen"
                Case 17 : Result = "Seventeen"
                Case 18 : Result = "Eighteen"
                Case 19 : Result = "Nineteen"
                Case Else
            End Select
        Else
            ' .. otherwise it's between 20 and 99.
            Select Case Val(Microsoft.VisualBasic.Left(MyTens, 1))
                Case 2 : Result = "Twenty "
                Case 3 : Result = "Thirty "
                Case 4 : Result = "Forty "
                Case 5 : Result = "Fifty "
                Case 6 : Result = "Sixty "
                Case 7 : Result = "Seventy "
                Case 8 : Result = "Eighty "
                Case 9 : Result = "Ninety "
                Case Else
            End Select

            ' Convert ones place digit.
            Result = Result & ConvertDigit(Microsoft.VisualBasic.Right(MyTens, 1))
        End If

        ConvertTens = Result
    End Function

    Public Function ConvertHundreds(ByVal MyNumber As String)
        Dim Result As String

        ' Exit if there is nothing to convert.
        If Val(MyNumber) = 0 Then Exit Function

        ' Append leading zeros to number.
        MyNumber = Microsoft.VisualBasic.Right("000" & MyNumber, 3)

        ' Do we have a hundreds place digit to convert?
        If Microsoft.VisualBasic.Left(MyNumber, 1) <> "0" Then
            Result = ConvertDigit(Microsoft.VisualBasic.Left(MyNumber, 1)) & " Hundred "
        End If

        ' Do we have a tens place digit to convert?
        If Mid(MyNumber, 2, 1) <> "0" Then
            Result = Result & ConvertTens(Mid(MyNumber, 2))
        Else
            ' If not, then convert the ones place digit.
            Result = Result & ConvertDigit(Mid(MyNumber, 3))
        End If

        ConvertHundreds = Result.Trim
    End Function

    Public Function ConvertDigit(ByVal MyDigit)
        Select Case Val(MyDigit)
            Case 1 : ConvertDigit = "One"
            Case 2 : ConvertDigit = "Two"
            Case 3 : ConvertDigit = "Three"
            Case 4 : ConvertDigit = "Four"
            Case 5 : ConvertDigit = "Five"
            Case 6 : ConvertDigit = "Six"
            Case 7 : ConvertDigit = "Seven"
            Case 8 : ConvertDigit = "Eight"
            Case 9 : ConvertDigit = "Nine"
            Case Else : ConvertDigit = ""
        End Select
    End Function
    Private Sub Get_BillDetails()
        Dim str As String
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        Dim Orderno As String = ""
        Dim condition As String = ""
        If Request("BillType") = "T" Then
            condition = "and b.RefNo='Topup'"
        Else
            condition = "and b.RefNo<>'Topup'"
        End If
        Dim id As String = ""
        If Not Request("IdNo") Is Nothing Then
            id = Request("Idno")
        Else
            id = Session("IDNo")
        End If
        If Not Request("OrderNo") Is Nothing Then
            Orderno = Request("OrderNo")
        End If
        Dim TaxType As String = ""
        'Comm = New SqlCommand("select  Case when a.Qty=0 then 'False' else 'True'  end as IsVisible,a.FCode,b.PartyName,b.UserBillNo,b.BillDate," & _
        '                      " a.ProductId,a.ProductName,a.Rate,Cast(a.Qty as int) as Qty,Cast(a.Qty as int)*a.Rate as Amount,a.bvvalue as BV," & _
        '                      " a.DiscountPer,a.Discount,a.NetAmount,a.Tax,a.TaxAmount,(a.TaxAmount+a.NetAmount) as TotalAmount,b.NetPayable,b.RndOff," & _
        '                      " a.DP,b.CourierName,b.LR,b.LRDate,b.DocketNo,b.DocketDate,b.BillNo from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as b " & _
        '                      " JOIN " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillDetails as a ON a.BillNo=b.BillNo where b.FCode='" & Session("IDNo") & "' " & _
        '                      " and b.UserBillNo='" & Request("BillNo") & "'  " & condition & " Order by Qty Asc ,b.BillId Desc  ")

        Comm = New SqlCommand("select  Case when a.Qty=0 then 'False' else 'True'  end as IsVisible,Case when a.BvValue=0 then 'False' else 'True' end as IsBvVisible, a.FCode,b.PartyName,b.UserBillNo,b.BillDate, " & _
                              " a.ProductId,a.ProductName,a.Mrp,a.Rate,Cast(a.Qty as int) as Qty,Cast(a.Qty as int)*a.Rate as Amount, a.Cgst,a.CgstAmt,a.SGSTAmt," & _
                              " a.SGST,isnull(a.bvvalue,0) as BV,a.DiscountPer,a.Discount,a.NetAmount,a.Tax,Case when a.TaxType='S' then b.STaxAmount else a.TaxAmount  end as TaxAmount," & _
                              "  Case when a.taxType='S' then 'True' else 'False' end as GSTVisible, case when a.TaxType<>'S' then 'True' else 'False' end as TaxVisible," & _
                               "  (a.NetAmount+a.CGSTAmt+a.TaxAmount+a.SGSTAmt) as TotalAmount,Case when Convert(Varchar,b.BillDate,112)<20170701 then 'Tax(%)' when  " & _
                              "   Convert(Varchar,b.BillDate,112)>=20170701 and a.TaxType='S' then '' else 'IGST(%)' end as TaxCaption,  " & _
                              " b.NetPayable,b.RndOff,a.DP,b.CourierName,b.LR, b.LRDate,b.DocketNo,b.DocketDate,b.BillNo,b.BuyerTin,b.Paymode,a.TaxType  " & _
                              "from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain " & _
                              " as b JOIN " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillDetails as a ON a.BillNo=b.BillNo  where b.FCode='" & id & "' and b.Orderno='" & Orderno & "' " & _
                              "  Order by Qty Asc ,b.BillId Desc ")
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
            lblInvoiceDateText.Text = Format(dt.Rows(0)("BillDate"), "dd-MMM-yyyy")
            If dt.Rows(0)("BuyerTin") <> "" Then
                LblBuyer.Text = "Bill BY:" & dt.Rows(0)("BuyerTIN")
            Else
                LblBuyer.Text = ""
            End If
            If dt.Rows(0)("TaxCaption") = "Tax(%)" Then
                lblVatTax.Text = "Tax Summary"
                LblGstIN.Text = ""
            ElseIf dt.Rows(0)("TaxCaption") = "IGST(%)" Then
                lblVatTax.Text = "IGST Summary"
                LblGstIN.Text = "GSTIN" & Session("CompCST")
            Else
                lblVatTax.Text = "GST Tax Summary"
                LblGstIN.Text = "GSTIN" & Session("CompCST")
            End If

            Session("TaxType") = dt.Rows(0)("TaxType").ToString.ToUpper.Trim
            'LblPaymentMode.Text = dt.Rows(0)("PayMode")
            'lblCourierNameTxt.Text = dt.Rows(0)("CourierName")
            ' lblCnNoTxt.Text = dt.Rows(0)("LR")
            ' lblCNMI.Text = "For " & Session("CompName")
            LblBill.Text = dt.Rows(0)("BillNo")
            'If IsDBNull(dt.Rows(0)("LRDate")) Then
            '    lblCNDatetxt.Text = ""
            'Else
            '    lblCNDatetxt.Text = Format(dt.Rows(0)("LRDate"), "dd-MMM-yyyy")
            'End If

            lblRoundOfftxt.Text = dt.Rows(0)("RndOff")
            lblNetPayabletxt.Text = dt.Rows(0)("NetPayable")
            lblinword.Text = AmountInWords(dt.Rows(0)("Netpayable"))
            Dim aDr1 As DataRow = dt.NewRow
            aDr1("Qty") = dt.Compute("Sum(Qty)", "")
            aDr1("BV") = dt.Compute("Sum(BV)", "")
            aDr1("Amount") = dt.Compute("Sum(Amount)", "")
            'aDr1("Amount") = dt.Compute("Sum(NetAmount)", "")
            aDr1("TaxAmount") = dt.Compute("Sum(TaxAmount)", "")
            aDr1("TotalAmount") = dt.Compute("Sum(TotalAmount)", "")
            aDr1("Discount") = dt.Compute("Sum(Discount)", "")
            aDr1("IsVisible") = "true"
            aDr1("IsBVVisible") = "true"
            If Session("TaxType") = "S" Then
                For Each dcfColumn As DataControlField In GridView1.Columns
                    If dcfColumn.HeaderText = "Tax(%)" Or dcfColumn.HeaderText = "IGST(%)" Then
                        dcfColumn.Visible = False
                        dcfColumn.HeaderText = dt.Rows(0)("TaxCaption")
                    End If
                    If dcfColumn.HeaderText = "TaxAmount" Then
                        dcfColumn.Visible = False
                    End If
                    If dcfColumn.HeaderText = "CGST" Then
                        dcfColumn.Visible = True
                    End If
                    If dcfColumn.HeaderText = "SGST" Then
                        dcfColumn.Visible = True
                    End If
                    If dcfColumn.HeaderText = "CGST Amount" Then
                        dcfColumn.Visible = True
                    End If
                    If dcfColumn.HeaderText = "SGST Amount" Then
                        dcfColumn.Visible = True
                    End If
                Next
            Else
                For Each dcfColumn As DataControlField In GridView1.Columns
                    If dcfColumn.HeaderText = "Tax(%)" Or dcfColumn.HeaderText = "IGST(%)" Then
                        dcfColumn.Visible = True
                        dcfColumn.HeaderText = dt.Rows(0)("TaxCaption")
                    End If
                    If dcfColumn.HeaderText = "TaxAmount" Then
                        dcfColumn.Visible = True
                    End If
                    If dcfColumn.HeaderText = "CGST" Then
                        dcfColumn.Visible = False
                    End If
                    If dcfColumn.HeaderText = "SGST" Then
                        dcfColumn.Visible = False
                    End If
                    If dcfColumn.HeaderText = "CGST Amount" Then
                        dcfColumn.Visible = False
                    End If
                    If dcfColumn.HeaderText = "SGST Amount" Then
                        dcfColumn.Visible = False
                    End If
                Next


            End If


            'aDr1("SNo") = 0

            dt.Rows.Add(aDr1)
            dt.AcceptChanges()
            GridView1.DataSource = dt
            GridView1.DataBind()

            If Conn.State = ConnectionState.Closed Then
                Conn.Open()
            End If
            If Session("TaxType") <> "S" Then


                Comm = New SqlCommand("Select Tax,Sum(NetAmount) as Amount,Sum(TaxAmount) as TaxAmount,0 as CGSTAmount,0as SGstAmount,Round(Sum(NetAmount+TaxAmount),2) as NetAmount From " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillDetails Where Tax>0 AND BillNo='" & LblBill.Text & "' Group By Tax")
                Comm.Connection = Conn
                Ad = New SqlDataAdapter(Comm)
                dt = New DataTable
                Ad.Fill(dt)


                RptTax1.DataSource = dt
                RptTax1.DataBind()
                TrTax.Visible = True
                TrCGST.Visible = False
                RptTax1.Visible = True
                RptTax.Visible = False
            Else
                Comm = New SqlCommand("Select CGST,Sum(NetAmount) as Amount,Sum(CGSTAmt) as CGSTAmount,sum(SGSTAmt) as SGSTAmount," & _
                                      " Round(Sum(NetAmount+CGSTAmt+SGSTAmt),2) as NetAmount From TrnBillDetails Where TaxType='S'" & _
                                      " AND BillNo='" & LblBill.Text & "' and Prodtype='P' Group By CGST")
                Comm.Connection = Conn
                Ad = New SqlDataAdapter(Comm)
                dt = New DataTable
                Ad.Fill(dt)


                RptTax.DataSource = dt
                RptTax.DataBind()
                TrTax.Visible = False
                TrCGST.Visible = True
                RptTax.Visible = True
                RptTax1.Visible = False
            End If


        End If
        Session("DirectData1") = dt
        Conn.Close()
    End Sub

End Class
