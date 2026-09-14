Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_Home1
    Inherits System.Web.UI.Page

    Dim objGen As clsGeneral = New clsGeneral



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'GvData.PageIndex = Session("index")
            If Session("AStatus") = "OK" Then

                FillData()

            End If
        End If
    End Sub



    Protected Sub FillData()
        Try


            'Dim sql As String
            'Dim dt1 As DataTable
            'dt1 = New DataTable
            'Dim obj As DAL
            'obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            'sql = " select * from V#Admin  "
            'dt1 = New DataTable
            'dt1 = obj.(sql)
            'If dt1.Rows.Count > 0 Then

            'End If

            Dim Ds As New DataSet
            Dim prms As SqlParameter() = New SqlParameter(0) {}
            prms(0) = New SqlParameter("@FormNo", 0)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "AdminHome", prms)

            'If Ds.Tables(0).Rows.Count > 0 Then
            '    TodayRegister.InnerHtml = Ds.Tables(0).Rows(0)("TodayJoin")
            '    TodayActive.InnerHtml = Ds.Tables(0).Rows(0)("TodayActive")
            '    TodayDeactive.InnerHtml = Ds.Tables(0).Rows(0)("TodayDeActive")
            'End If


            If Ds.Tables(0).Rows.Count > 0 Then
                TodayRegister.InnerHtml = Ds.Tables(0).Rows(0)("TodayJoin")
                EP30.InnerHtml = Ds.Tables(0).Rows(0)("EP30")
                EP50.InnerHtml = Ds.Tables(0).Rows(0)("EP50")
                EP120.InnerHtml = Ds.Tables(0).Rows(0)("EP120")
                EP1200.InnerHtml = Ds.Tables(0).Rows(0)("EP1200")
                TodayDeactive.InnerHtml = Ds.Tables(0).Rows(0)("TodayDeActive")
            End If



            'If Ds.Tables(1).Rows.Count > 0 Then
            '    TotalRegister.InnerHtml = Ds.Tables(1).Rows(0)("TotalJoin")
            '    TotalActive.InnerHtml = Ds.Tables(1).Rows(0)("TotalActive")
            '    TotalDeactive.InnerHtml = Ds.Tables(1).Rows(0)("TotalDeActive")
            'End If


            If Ds.Tables(1).Rows.Count > 0 Then
                TotalRegister.InnerHtml = Ds.Tables(1).Rows(0)("TotalJoin")
                TEP30.InnerHtml = Ds.Tables(1).Rows(0)("TEP30")
                TEP50.InnerHtml = Ds.Tables(1).Rows(0)("TEP50")
                TEP120.InnerHtml = Ds.Tables(1).Rows(0)("TEP120")
                TEP1200.InnerHtml = Ds.Tables(1).Rows(0)("TEP1200")
                TotalDeactive.InnerHtml = Ds.Tables(1).Rows(0)("TotalDeActive")
            End If



            If Ds.Tables(2).Rows.Count > 0 Then
                GeneratedEP.InnerHtml = Ds.Tables(2).Rows(0)("GENEP")
                ReceivedFromID.InnerHtml = Ds.Tables(2).Rows(0)("ReceivedEP")
                TotalEP.InnerHtml = Ds.Tables(2).Rows(0)("Total")
                DebitEP.InnerHtml = Ds.Tables(2).Rows(0)("DebitEP")
                BalanceEP.InnerHtml = Ds.Tables(2).Rows(0)("BalanceEP")
            End If


            If Ds.Tables(3).Rows.Count > 0 Then
                TotalEPR.InnerHtml = Ds.Tables(3).Rows(0)("EpCRR")
                UsedEPR.InnerHtml = Ds.Tables(3).Rows(0)("EpDRR")
                BalanceEPR.InnerHtml = Ds.Tables(3).Rows(0)("EpBalR")

            End If


            If Ds.Tables(4).Rows.Count > 0 Then
                TotalEPM.InnerHtml = Ds.Tables(4).Rows(0)("EpCRM")
                UsedEPM.InnerHtml = Ds.Tables(4).Rows(0)("EpDRM")
                BalanceEPM.InnerHtml = Ds.Tables(4).Rows(0)("EpBalM")
            End If



            If Ds.Tables(5).Rows.Count > 0 Then
                totalGstEP.InnerHtml = Ds.Tables(5).Rows(0)("totalGstEP")
                EP30Gst.InnerHtml = Ds.Tables(5).Rows(0)("EP30Gst")
                EP50Gst.InnerHtml = Ds.Tables(5).Rows(0)("EP50Gst")
                EP120Gst.InnerHtml = Ds.Tables(5).Rows(0)("EP120Gst")
                EP1200Gst.InnerHtml = Ds.Tables(5).Rows(0)("EP1200Gst")


            End If



            If Ds.Tables(6).Rows.Count > 0 Then
                Level1.InnerHtml = Ds.Tables(6).Rows(0)("EP")
                Level2.InnerHtml = Ds.Tables(6).Rows(1)("EP")
                Level3.InnerHtml = Ds.Tables(6).Rows(2)("EP")
                Level4.InnerHtml = Ds.Tables(6).Rows(3)("EP")
                Level5.InnerHtml = Ds.Tables(6).Rows(4)("EP")
                Level6.InnerHtml = Ds.Tables(6).Rows(5)("EP")
                Level7.InnerHtml = Ds.Tables(6).Rows(6)("EP")
                Level8.InnerHtml = Ds.Tables(6).Rows(7)("EP")
                Level9.InnerHtml = Ds.Tables(6).Rows(8)("EP")
                Extraep.InnerHtml = Ds.Tables(6).Rows(9)("EP")
                TotalEPLevel9.InnerHtml = Ds.Tables(6).Rows(10)("EP")
            End If



            If Ds.Tables(7).Rows.Count > 0 Then
                DLTotalEP.InnerHtml = Ds.Tables(7).Rows(0)("DLTotalEP")
                DLBStarEP.InnerHtml = Ds.Tables(7).Rows(0)("DLBStarEP")
                DLBBronzeEP.InnerHtml = Ds.Tables(7).Rows(0)("DLBBronzeEP")
                DLBSilverEP.InnerHtml = Ds.Tables(7).Rows(0)("DLBSilverEP")
                DLBGoldEP.InnerHtml = Ds.Tables(7).Rows(0)("DLBGoldEP")
                DLBExtraEP.InnerHtml = Ds.Tables(7).Rows(0)("DLBExtraEP")

            End If


            If Ds.Tables(8).Rows.Count > 0 Then
                totalOrderEP.InnerHtml = Ds.Tables(8).Rows(0)("totalOrderEP")
                EP30Order.InnerHtml = Ds.Tables(8).Rows(0)("EP30Order")
                EP50Order.InnerHtml = Ds.Tables(8).Rows(0)("EP50Order")
                EP120Order.InnerHtml = Ds.Tables(8).Rows(0)("EP120Order")
                EP1200Order.InnerHtml = Ds.Tables(8).Rows(0)("EP1200Order")


            End If


            If Ds.Tables(9).Rows.Count > 0 Then
                CBEPToatal.InnerHtml = Ds.Tables(9).Rows(0)("CBEPToatal")
                CBEP30.InnerHtml = Ds.Tables(9).Rows(0)("CBEP30")
                CBEP50.InnerHtml = Ds.Tables(9).Rows(0)("CBEP50")
                CBEP120.InnerHtml = Ds.Tables(9).Rows(0)("CBEP120")
                CBEP1200.InnerHtml = Ds.Tables(9).Rows(0)("CBEP1200")
            End If
            If Ds.Tables(10).Rows.Count > 0 Then
                withdrawlBankService.InnerHtml = Ds.Tables(10).Rows(0)("BankCharge")
                withdrawlTdsAmount.InnerHtml = Ds.Tables(10).Rows(0)("TDSAmount")
                Withdrawlsamount.InnerHtml = Ds.Tables(10).Rows(0)("Amount")
                withdrawlNetAmount.InnerHtml = Ds.Tables(10).Rows(0)("NetAmount")
            End If
        Catch ex As Exception

        End Try
    End Sub





End Class
