Imports System.Data

Partial Class App_UI_Application_Pages_Home
    Inherits System.Web.UI.Page
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                FillData()
                If Session("CompId") = "1083" Then
                    FillWalletSummary()
                End If
            End If
        End If
    End Sub
    Protected Sub FillData()
        Try
            Dim sql As String
            Dim dt1 As DataTable
            dt1 = New DataTable
            Dim obj As DAL
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            sql = " select * from V#Admin  "
            dt1 = New DataTable
            dt1 = obj.GetData(sql)
            If dt1.Rows.Count > 0 Then
                LblTodregister.Text = dt1.Rows(0)("TodayTotalJoining")
                LblTodActive.Text = dt1.Rows(0)("TodayTotalActivate")
                LblTotalRegister.Text = dt1.Rows(0)("TotalJoining")
                LblTotalActive.Text = dt1.Rows(0)("TotalActivate")
                LblTotalDeactive.Text = dt1.Rows(0)("TotalDeactivate")
                todaydeactive.Text = dt1.Rows(0)("TodayDeactivate")
                todayreamt.Text = dt1.Rows(0)("TodayAmount")
                TotalRecamt.Text = dt1.Rows(0)("TotalAmount")
                todayunit.Text = dt1.Rows(0)("Todayunit")
                totalunit.Text = dt1.Rows(0)("Totalunit")
            End If
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub FillWalletSummary()
        Try
            Dim sql As String
            Dim ds As DataSet = New DataSet()
            Dim dt1 As DataTable = New DataTable()
            Dim dt2 As DataTable = New DataTable()
            Dim dt3 As DataTable = New DataTable()
            sql = "  Exec Sp_WalletTotalSummary "
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql)
            dt1 = ds.Tables(0)
            Div5.visible = True
            Div6.visible = True
            Div7.visible = True
            Div8.visible = True
            Div9.visible = True
            Div17.visible = True
            Div18.visible = True
            Div19.visible = True
            Div20.Visible = True
            Div11.Visible = True
            Div12.Visible = True
            Div13.Visible = True
            If dt1.Rows.Count > 0 Then
                lblTotalCredit.Text = dt1.Rows(0)("Credit")
                lblTotalDebit.Text = dt1.Rows(0)("Debit")
                lblTotalBalance.Text = dt1.Rows(0)("Bal")
            End If
            dt2 = ds.Tables(1)
            If dt2.Rows.Count > 0 Then
                lblWithApprove.Text = dt2.Rows(0)("ApproveAmt")
                lblWithPending.Text = dt2.Rows(0)("PendingAmt")
            End If
            dt3 = ds.Tables(2)
            If dt3.Rows.Count > 0 Then
            End If
            Dim dt4 As DataTable = New DataTable()
            dt4 = ds.Tables(3)
            If dt4.Rows.Count > 0 Then
                lblCurrSessnBV.Text = dt4.Rows(0)("Total")
            Else
                lblCurrSessnBV.Text = "0"
            End If
            Dim dt5 As DataTable = New DataTable()
            dt5 = ds.Tables(4)
            If dt5.Rows.Count > 0 Then
                lblTotalSCredit.Text = dt5.Rows(0)("Credit")
                lblTotalSDebit.Text = dt5.Rows(0)("Debit")
                lblTotalSBalance.Text = dt5.Rows(0)("Bal")
            End If
            Dim dt6 As DataTable = New DataTable()
            dt6 = ds.Tables(5)
            If dt6.Rows.Count > 0 Then
                lblrepurchcr.Text = dt6.Rows(0)("Credit")
                lblrepurchdr.Text = dt6.Rows(0)("Debit")
                lblrepurchbal.Text = dt6.Rows(0)("Bal")
            End If
            If Session("compid") = "1083" Then
                Div14.Visible = True
                Dim dt7 As DataTable = New DataTable()
                dt7 = ds.Tables(6)
                If dt7.Rows.Count > 0 Then
                    LblRepurchase.Text = dt7.Rows(0)("Total")
                Else
                    LblRepurchase.Text = "0"
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub


    'Private Sub FIllBv()
    '    Try


    '        Dim sql As String
    '        Dim dt1 As DataTable
    '        dt1 = New DataTable
    '        Dim obj As  DAL

    '        sql = " Select Sum(Todayunit) as Todayunit,Sum(TotalUnit) as Totalunit from " & _
    '    "(select  Isnull(Sum(PvValue),0) as Todayunit,0 as TotalUnit from Repurchincome where" & _
    '        " Dsessid = Convert(Varchar, Getdate(), 112) and Billtype<>'R'" & _
    '        " Union All" & _
    '        " select  0 as Todayunit,Isnull(Sum(PvValue),0) as Totalunit  from Repurchincome where Billtype<>'R') as Temp"

    '        dt1 = New DataTable
    '        dt1 = obj.GetData(sql)
    '        If dt1.Rows.Count > 0 Then
    '            todayunit.Text = dt1.Rows(0)("Todayunit")
    '            totalunit.Text = dt1.Rows(0)("Totalunit")

    '            '  LblPendingEpin.Text = dt1.Rows(0)("pendingRequest")
    '            '  lblTodaysApprove.Text = dt1.Rows(0)("TodayApproveProductRequest")
    '            ' LblTotalApprove.Text = dt1.Rows(0)("TotalApproveProductRequest")
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Private Sub Fillkitamount()
    '    Try


    '        Dim sql As String
    '        Dim dt1 As DataTable
    '        dt1 = New DataTable
    '        Dim obj As  DAL

    '        sql = "Select Sum(TotalKitamount) as TotalAmount,Sum(TodayKitamount) as TodayAmount from " & _
    '        "( Select Sum(kitamount) as TotalKitamount,0 as TodayKitamount  from M_MemberMaster " & _
    '        " as a,M_KitMaster As b where a.kitid=b.kitid and b.ActiveStatus='Y'" & _
    '        " Union all" & _
    '         "  Select 0 as TotalkitAmount,Sum(kitamount) as TodayKitamount from M_MemberMaster as a," & _
    '        " M_KitMaster As b where" & _
    '        " a.kitid=b.kitid and b.ActiveStatus='Y' and " & _
    '        " Convert(Varchar,Upgradedate,106) =CONVERT(Varchar,GetDate(),106)) as Temp"

    '        dt1 = New DataTable
    '        dt1 = obj.GetData(sql)
    '        If dt1.Rows.Count > 0 Then
    '            todayreamt.Text = dt1.Rows(0)("TodayAmount")
    '            TotalRecamt.Text = dt1.Rows(0)("TotalAmount")

    '            '  LblPendingEpin.Text = dt1.Rows(0)("pendingRequest")
    '            '  lblTodaysApprove.Text = dt1.Rows(0)("TodayApproveProductRequest")
    '            ' LblTotalApprove.Text = dt1.Rows(0)("TotalApproveProductRequest")
    '        End If
    '    Catch ex As Exception

    '    End Try


    'End Sub


End Class
