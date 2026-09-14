Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Imports ClosedXML.Excel

Imports System.Configuration

Partial Class MISReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                BindSession()


                '                IncentiveDetail()
                '  ChkRadioMember.SelectedValue = "N" : IncentiveDetail()

            End If
        End If

    End Sub

    Public Sub BindSession()
        Dim sql As String = " select SessID,Cast(SessID as varchar) + ' [' + Replace(Convert(varchar,FrmDate,106),' ','-') + ' to ' + ISNULL(Replace(Convert(varchar,ToDate,106),' ','-'),'') + ']' As SessnName from M_SessnMaster Where ToDate Is Not Null order by SessID"
        objModuleFun.FillCombo(sql, ddlSession, "SessnName", "SessID")
    End Sub

    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click


       
        'sql = "Select b.KitName,b.KitAmount,b.BFund as [Binary Fund],Count(*) as TotalSale, b.PV*Count(*) as [TotalBV], b.KitAmount*Count(*) as [FundCollection],b.BFund*Count(*) as [Binary Fund Collection],b.CFund*Count(*) as [C FundCollection] FROM RepurchIncome a, M_KitMAster b" & _
        '"  WHERE a.KitID = b.KitID And a.SessID=" & ddlSession.SelectedValue & " GROUP BY b.KitName,b.KitAmount,b.BFund,b.PV,b.CFund"
        sql = "Exec KitFundDetail " & ddlSession.SelectedValue & ""
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvActivation.DataSource = dtData
        GvActivation.DataBind()
        Session("ActivationData") = dtData
        GvActivation.Visible = True


        sql = "Select Sum(PairIncome)as [Sales Incentive],Sum(PairIncentive) as [Team Building],Sum(MagicIncome) as [Team Infinity],SUM(NetIncome) as [Gross Income],SUM(TDSAmount) as [TDS]," & _
        " SUM(CouponsAmt) as [Repurchase],SUm(ChqAmt) as [Net Income],SUM(ClsBal) as [Closing] FROM M_MonthlyPayDetail WHERE SessID='" & ddlSession.SelectedValue & "'"

        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GrdPayout.DataSource = dtData
        GrdPayout.DataBind()
        Session("PayoutData") = dtData
        GrdPayout.Visible = True

        'sql = "Select a.ProductName,a.MRP,a.DP,b.PurchaseRate,b.BV,b.FundPoint as [Binary Fund],b.CV as [CIncome],a.Tax, b.PV as [TP]," & _
        '" b.Val1 as [TP Fund],b.RP,b.Val2 as [RP Fund],b.SubQty [DevFund],b.FlexiQty [Profit],Sum(a.Qty) as Qty,b.BV *Sum(a.Qty) as [BV Value]," & _
        '" b.FundPoint*Sum(a.Qty) as [Binary Fund Value],b.CV *Sum(a.Qty)as [CIncome Value],SUm(a.TaxAmount) as TaxAmout, b.PV*Sum(a.Qty) as " & _
        '" [TP Value],b.Val1*Sum(a.Qty) as [TP Fund Value],b.RP*Sum(a.Qty) as [RP Value],b.Val2*Sum(a.Qty) as [RP Fund Value]," & _
        '" b.SubQty*Sum(a.Qty) [DevFund Value],b.FlexiQty*Sum(a.Qty) [Total Profit]FROM " & Application("InvDB") & "..TrnBillDetails a, " & _
        '" " & Application("InvDB") & "..M_ProductMaster b,SJLInv..TrnBillMain c WHERE a.ProductID=b.ProdID AND a.BillNo=c.BillNo AND " & _
        '" c.BillType='R' AND a.SessID='" & ddlSession.SelectedValue & "' GROUP BY a.ProductName,a.MRP,a.DP,b.PurchaseRate,b.BV,b.FundPoint," & _
        '" b.CV ,a.Tax, b.PV,b.Val1,b.RP,b.Val2,b.SubQty ,b.FlexiQty"
        sql = "Exec GetMis " & ddlSession.SelectedValue & ""

        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GrdProduct.DataSource = dtData
        GrdProduct.DataBind()
        Session("ProductData") = dtData
        GrdProduct.Visible = True

        sql = " Select Sessid, PrevBal as [Previous Balance Of Binary Fund],KitBFund as [Current Kit Binary Fund Collection]," & _
                " RepurchBFund as [Current Repurchase Binary Fund],PairIncome as [Sales Incentive],CF as [Carry Forwrd Of Binary Fund]," & _
                " CFundPrev,KitCFund as [Current Kit C Fund Collection],RerpurchCFund as [Current Repurchase C Fund Collection]," & _
                " InfinityBulding as [Current C Income],CFCFund as [Carry Forward C Fund] FROM BinaryFundDetails WHERE SessID='" & ddlSession.SelectedValue & "'"

        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GrdSummary.DataSource = dtData
        GrdSummary.DataBind()
        Session("SummaryData") = dtData
        GrdSummary.Visible = True
        If dtData.Rows.Count > 0 Then
            
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If


    End Sub

    Protected Sub GvActivation_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvActivation.PageIndexChanging
        GvActivation.PageIndex = e.NewPageIndex
        GvActivation.DataSource = Session("ActivationData")
        GvActivation.DataBind()
    End Sub
    Protected Sub GrdPayout_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdPayout.PageIndexChanging
        GrdPayout.PageIndex = e.NewPageIndex
        GrdPayout.DataSource = Session("PayoutData")
        GrdPayout.DataBind()
    End Sub
    Protected Sub GrdProduct_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdProduct.PageIndexChanging
        GrdProduct.PageIndex = e.NewPageIndex
        GrdProduct.DataSource = Session("ProductData")
        GrdProduct.DataBind()
    End Sub
    Protected Sub GrdSummary_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdSummary.PageIndexChanging
        GrdSummary.PageIndex = e.NewPageIndex
        GrdSummary.DataSource = Session("SummaryData")
        GrdSummary.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        ''   Try

        ''   Catch ex As Exception

        ''   End Try
        ''   Try
        ''       Dim dtTemp As New DataTable
        ''       Dim dg As New DataGrid
        ''       objDAL = New DAL

        ''       sql = "Select b.KitName,b.KitAmount,b.BFund,Count(*) as TotalSale, b.PV*Count(*) as [TotalBV], b.KitAmount*Count(*) as [FundCollection],b.BFund*Count(*) as [BFundCollec],b.CFund*Count(*) as [CFundCollec] FROM RepurchIncome a, M_KitMAster b" & _
        ''  "  WHERE a.KitID = b.KitID And a.SessID=" & ddlSession.SelectedValue & " GROUP BY b.KitName,b.KitAmount,b.BFund,b.PV,b.CFund"

        ''       dtTemp = New DataTable
        ''       objDAL = New DAL
        ''       dtTemp = objDAL.GetData(sql)

        ''       dg.DataSource = dtTemp
        ''       dg.DataBind()

        ''       ExportToExcel("ActivationMIS.xls", dg)
        ''       Dim dg1 As New DataGrid
        ''       sql = "Select Sum(PairIncome)as [Sales Incentive],Sum(PairIncentive) as [Team Building],Sum(MagicIncome) as [Team Infinity],SUM(NetIncome) as [Gross Income],SUM(TDSAmount) as [TDS]," & _
        ''" SUM(CouponsAmt) as [Repurchase],SUm(ChqAmt) as [Net Income],SUM(ClsBal) as [Closing] FROM M_MonthlyPayDetail WHERE SessID='" & ddlSession.SelectedValue & "'"
        ''       dtTemp = New DataTable
        ''       objDAL = New DAL
        ''       dtTemp = objDAL.GetData(sql)

        ''       dg1.DataSource = dtTemp
        ''       dg1.DataBind()

        ''       ExportToExcel("PayoutMIS.xls", dg1)
        ''       Dim dg2 As New DataGrid
        ''       sql = "Select a.ProductName,a.MRP,a.DP,b.PurchaseRate,b.BV,b.FundPoint as [Binary Fund],b.CV as [CIncome],a.Tax, b.PV as [TP]," & _
        ''" b.Val1 as [TP Fund],b.RP,b.Val2 as [RP Fund],b.SubQty [DevFund],b.FlexiQty [Profit],Sum(a.Qty) as Qty,b.BV *Sum(a.Qty) as [BV Value]," & _
        ''" b.FundPoint*Sum(a.Qty) as [Binary Fund Value],b.CV *Sum(a.Qty)as [CIncome Value],SUm(a.TaxAmount) as TaxAmout, b.PV*Sum(a.Qty) as " & _
        ''" [TP Value],b.Val1*Sum(a.Qty) as [TP Fund Value],b.RP*Sum(a.Qty) as [RP Value],b.Val2*Sum(a.Qty) as [RP Fund Value]," & _
        ''" b.SubQty*Sum(a.Qty) [DevFund Value],b.FlexiQty*Sum(a.Qty) [Total Profit]FROM " & Application("InvDB") & "..TrnBillDetails a, " & _
        ''" " & Application("InvDB") & "..M_ProductMaster b," & Application("InvDB") & "..TrnBillMain c WHERE a.ProductID=b.ProdID AND a.BillNo=c.BillNo AND " & _
        ''" c.BillType='R' AND a.SessID='" & ddlSession.SelectedValue & "' GROUP BY a.ProductName,a.MRP,a.DP,b.PurchaseRate,b.BV,b.FundPoint," & _
        ''" b.CV ,a.Tax, b.PV,b.Val1,b.RP,b.Val2,b.SubQty ,b.FlexiQty"
        ''       dtTemp = New DataTable
        ''       objDAL = New DAL
        ''       dtTemp = objDAL.GetData(sql)

        ''       dg2.DataSource = dtTemp
        ''       dg2.DataBind()

        ''       ExportToExcel("ProductMIS.xls", dg2)
        ''       Dim dg3 As New DataGrid
        ''       sql = "Select * FROM BinaryFundDetails WHERE SessID='" & ddlSession.SelectedValue & "'"
        ''       dtTemp = New DataTable
        ''       objDAL = New DAL
        ''       dtTemp = objDAL.GetData(sql)

        ''       dg3.DataSource = dtTemp
        ''       dg3.DataBind()

        ''       ExportToExcel("SummaryMIS.xls", dg3)

        ''   Catch ex As Exception
        ''       Response.Write(ex.Message & "Error In Exporting File")
        ''   End Try
        Dim constr As String = Application("Connect")
        'Dim query As String = "Select b.KitName,b.KitAmount,b.BFund as [Binary Fund],Count(*) as TotalSale, b.PV*Count(*) as [TotalBV], b.KitAmount*Count(*) as [FundCollection],b.BFund*Count(*) as [Binary Fund Collection],b.CFund*Count(*) as [C FundCollection] FROM RepurchIncome a, M_KitMAster b" & _
        '"  WHERE a.KitID = b.KitID And a.SessID=" & ddlSession.SelectedValue & " GROUP BY b.KitName,b.KitAmount,b.BFund,b.PV,b.CFund;"
        Dim query As String = "Exec KitFundDetail " & ddlSession.SelectedValue & ";"
        query &= "Select Sum(PairIncome)as [Sales Incentive],Sum(PairIncentive) as [Team Building],Sum(MagicIncome) as [Team Infinity],SUM(NetIncome) as [Gross Income],SUM(TDSAmount) as [TDS]," & _
      " SUM(CouponsAmt) as [Repurchase],SUm(ChqAmt) as [Net Income],SUM(ClsBal) as [Closing] FROM M_MonthlyPayDetail WHERE SessID='" & ddlSession.SelectedValue & "'"
        query &= ";Exec GetMis " & ddlSession.SelectedValue & ";"
        'query &= ";Select a.ProductName,a.MRP,a.DP,b.PurchaseRate,b.BV,b.FundPoint as [Binary Fund],b.CV as [CIncome],a.Tax, b.PV as [TP]," & _
        '        " b.Val1 as [TP Fund],b.RP,b.Val2 as [RP Fund],b.SubQty [DevFund],b.FlexiQty [Profit],Sum(a.Qty) as Qty,b.BV *Sum(a.Qty) as [BV Value]," & _
        '        " b.FundPoint*Sum(a.Qty) as [Binary Fund Value],b.CV *Sum(a.Qty)as [CIncome Value],SUm(a.TaxAmount) as TaxAmout, b.PV*Sum(a.Qty) as " & _
        '        " [TP Value],b.Val1*Sum(a.Qty) as [TP Fund Value],b.RP*Sum(a.Qty) as [RP Value],b.Val2*Sum(a.Qty) as [RP Fund Value]," & _
        '        " b.SubQty*Sum(a.Qty) [DevFund Value],b.FlexiQty*Sum(a.Qty) [Total Profit]FROM " & Application("InvDB") & "..TrnBillDetails a, " & _
        '        " " & Application("InvDB") & "..M_ProductMaster b,SJLInv..TrnBillMain c WHERE a.ProductID=b.ProdID AND a.BillNo=c.BillNo AND " & _
        '        " c.BillType='R' AND a.SessID='" & ddlSession.SelectedValue & "' GROUP BY a.ProductName,a.MRP,a.DP,b.PurchaseRate,b.BV,b.FundPoint," & _
        '        " b.CV ,a.Tax, b.PV,b.Val1,b.RP,b.Val2,b.SubQty ,b.FlexiQty"
        query &= ";Select  Sessid, PrevBal as [Previous Balance Of Binary Fund],KitBFund as [Current Kit Binary Fund Collection]," & _
               " RepurchBFund as [Current Repurchase Binary Fund],PairIncome as [Sales Incentive],CF as [Carry Forwrd Of Binary Fund]," & _
               " CFundPrev,KitCFund as [Current Kit C Fund Collection],RerpurchCFund as [Current Repurchase C Fund Collection]," & _
               " InfinityBulding as [Current C Income],CFCFund as [Carry Forward C Fund] FROM BinaryFundDetails WHERE SessID='" & ddlSession.SelectedValue & "'"
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(query)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using ds As New DataSet()
                        sda.Fill(ds)

                        'Set Name of DataTables.
                        ds.Tables(0).TableName = "Activation"
                        ds.Tables(1).TableName = "Payout"
                        ds.Tables(2).TableName = "Product"
                        ds.Tables(3).TableName = "Summary"

                        Using wb As New XLWorkbook()
                            For Each dt As DataTable In ds.Tables
                                'Add DataTable as Worksheet.
                                wb.Worksheets.Add(dt)
                            Next

                            'Export the Excel file.
                            Response.Clear()
                            Response.Buffer = True
                            Response.Charset = ""
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                            Response.AddHeader("content-disposition", "attachment;filename=MIS.xlsx")
                            Using MyMemoryStream As New MemoryStream()
                                wb.SaveAs(MyMemoryStream)
                                MyMemoryStream.WriteTo(Response.OutputStream)
                                Response.Flush()
                                Response.End()
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
        Dim sw As New System.IO.StringWriter
        Dim htw As System.Web.UI.HtmlTextWriter
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.xls"
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
        Response.Charset = ""
        dg.EnableViewState = False
        htw = New HtmlTextWriter(sw)
        dg.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.End()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub

    Protected Sub GvActivation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvActivation.SelectedIndexChanged

    End Sub




End Class
