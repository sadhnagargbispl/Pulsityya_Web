Imports System.Data.SqlClient
Imports System.Data
Partial Class Downline1
    Inherits System.Web.UI.Page
    Dim dt As DataTable
    ' Private dbGeneral As New clsGeneral
    'Private dbConnect As cls_DataAccess
    Dim strquery As String
    Dim FrmCondition As String = ""
    Dim ACnt As Integer = 0
    Dim BCnt As Integer = 0
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim objGen As clsGeneral = New clsGeneral

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Member / Member Downline  Report"
            If Not Page.IsPostBack Then
                FillKit()
                BindSession()
                txtMemberId.Text = ""
            End If
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub

    Private Sub FillDownlineSumm(ByVal FormNo As String)
        Try
            Dim Dt As New DataTable
            Dim Condition2 As String = ""
            Dim Condition3 As String = ""
            If RbtSearch.SelectedValue = "D" Then
                If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then


                    Condition2 = "and Cast(Replace(CONVERT(Varchar,a.Billdate,106),' ','-')as date)>=Cast('" & txtStartDate.Text & "' as date)  and Cast(Replace(CONVERT(varchar,a.BillDate,106),' ','-') as Date)<=Cast(Replace(Convert(Varchar,'" & txtEndDate.Text & "',106),' ','-') as Date)"

                    Condition3 = "and Sessid=  (Select Max(a.Sessid) from M_SessWiseBvCF as a,M_SessnMaster as b where a.Sessid=b.Sessid" & _
                    " and  Cast(Replace(CONVERT(Varchar,b.Frmdate,106),' ','-')as date)>=Cast('" & txtStartDate.Text & "' as Date) and  Cast(Replace(CONVERT(Varchar,b.Todate,106),' ','-')as date)" & _
                    " <=Cast('" & txtEndDate.Text & "' as Date) and Formno='" & FormNo & "')"
                End If
            Else
                Condition2 = Condition2 & "and Cast(Replace(CONVERT(Varchar,a.BillDate,106),' ','-') as Date)>='" & LblSDate.Text & "' and cast(Replace(CONVERT(varchar,a.BillDate,106),' ','-') as Date)<='" & LblTDate.Text & "'"
                Condition3 = Condition3 & " and Sessid='" & Val(DDLSession.SelectedValue) & "'"
            End If
            If ChkKit.Checked Then
                Condition2 = Condition2 & " And KitId='" & CmbKit.SelectedValue & "' "
            End If
            If Session("CompId") = 1081 Then
                strquery = " select IsNull(Sum(temp.LeftBV),0) as LeftBV,Isnull(Sum(Temp.RightBV),0)as RightBV ,IsnUll(sum(Temp.LegXBVCf),0) as LegXBVCf,IsNull(sum(Temp.LegYBVCF),0) as LegYBVCf," & _
            " Isnull(sum(Temp.LegXBVPaid),0) as LegXBVPaid,IsnUll(sum(Temp.LegYBvPaid),0) as LegYBVPaid From(" & _
            " Select Isnull(sum(BV),0) as LeftBV,0 as RightBV,0 as LegXBVCF,0 as LegYBvCF,0 as LegXBVPaid,0 as LegYBVPaid FROM M_membermaster as a,M_MemTreeRelation as b  WHERE " & _
            " a.Formno=b.FormnoDwn and a.LegNo='1' and " & _
            " b.FormNo=" & FormNo & "" & Condition2 & " " & _
            " Union ALL" & _
             " Select 0 as LeftBV,Isnull(sum(BV),0) as RightBV,0 as LegXBVCF,0 as LegYBvCF,0 as LegXBVPaid,0 as LegYBVPaid FROM M_membermaster as a,M_MemTreeRelation as b  WHERE " & _
            " a.Formno=b.FormnoDwn and a.LegNo='2' and " & _
            " b.FormNo=" & FormNo & "" & Condition2 & "" & _
            " Union All " & _
           " select 0 as LeftBv,0 as RightBV,Isnull(LegXBvCF,0) as LegXBvCF,Isnull(LegYBVCF,0) as LegYBVCF,Isnull(LegXBVPaid,0) as LegXBVPaid,IsnUll(LegYBVPaid,0) as LegYBVPaid from M_SesswiseBvCF  " & _
           " where Formno='" & FormNo & "' " & Condition3 & "" & _
            ") as Temp"
            ElseIf Session("CompId") = 1084 Then
                strquery = " select IsNull(Sum(temp.LeftBV),0) as LeftBV,Isnull(Sum(Temp.RightBV),0)as RightBV ,IsnUll(sum(Temp.LegXBVCf),0) as LegXBVCf,IsNull(sum(Temp.LegYBVCF),0) as LegYBVCf," & _
            " Isnull(sum(Temp.LegXBVPaid),0) as LegXBVPaid,IsnUll(sum(Temp.LegYBvPaid),0) as LegYBVPaid From(" & _
            " Select Isnull(sum(BV),0) as LeftBV,0 as RightBV,0 as LegXBVCF,0 as LegYBvCF,0 as LegXBVPaid,0 as LegYBVPaid FROM M_membermaster as a,M_MemTreeRelation as b  WHERE " & _
            " a.Formno=b.FormnoDwn and B.LegNo='1' and " & _
            " b.FormNo=" & FormNo & "" & Condition2 & " " & _
            " Union ALL" & _
             " Select 0 as LeftBV,Isnull(sum(BV),0) as RightBV,0 as LegXBVCF,0 as LegYBvCF,0 as LegXBVPaid,0 as LegYBVPaid FROM M_membermaster as a,M_MemTreeRelation as b  WHERE " & _
            " a.Formno=b.FormnoDwn and B.LegNo='2' and " & _
            " b.FormNo=" & FormNo & "" & Condition2 & "" & _
            " Union All " & _
           " select 0 as LeftBv,0 as RightBV,Isnull(LegXBvCF,0) as LegXBvCF,Isnull(LegYBVCF,0) as LegYBVCF,Isnull(LegXBVPaid,0) as LegXBVPaid,IsnUll(LegYBVPaid,0) as LegYBVPaid from M_SesswiseBvCF  " & _
           " where Formno='" & FormNo & "' " & Condition3 & "" & _
            ") as Temp"
            Else
                strquery = " select IsNull(Sum(temp.LeftBV),0) as LeftBV,Isnull(Sum(Temp.RightBV),0)as RightBV ,IsnUll(sum(Temp.LegXBVCf),0) as LegXBVCf,IsNull(sum(Temp.LegYBVCF),0) as LegYBVCf," & _
            " Isnull(sum(Temp.LegXBVPaid),0) as LegXBVPaid,IsnUll(sum(Temp.LegYBvPaid),0) as LegYBVPaid From(" & _
            " Select Isnull(sum(PVValue),0) as LeftBV,0 as RightBV,0 as LegXBVCF,0 as LegYBvCF,0 as LegXBVPaid,0 as LegYBVPaid FROM RepurchIncome as a,M_MemTreeRelation as b  WHERE " & _
            " a.Formno=b.FormnoDwn and a.BillType<>'R' and b.LegNo='1' and " & _
            " b.FormNo=" & FormNo & "" & Condition2 & " " & _
            " Union ALL" & _
             " Select 0 as LeftBV,Isnull(sum(PvValue),0) as RightBV,0 as LegXBVCF,0 as LegYBvCF,0 as LegXBVPaid,0 as LegYBVPaid FROM RepurchIncome as a,M_MemTreeRelation as b  WHERE " & _
            " a.Formno=b.FormnoDwn and a.BillType<>'R' and b.LegNo='2' and " & _
            " b.FormNo=" & FormNo & "" & Condition2 & "" & _
            " Union All " & _
           " select 0 as LeftBv,0 as RightBV,Isnull(LegXBvCF,0) as LegXBvCF,Isnull(LegYBVCF,0) as LegYBVCF,Isnull(LegXBVPaid,0) as LegXBVPaid,IsnUll(LegYBVPaid,0) as LegYBVPaid from M_SesswiseBvCF  " & _
           " where Formno='" & FormNo & "' " & Condition3 & "" & _
            ") as Temp"
            End If
            

            Dt = objDAL.GetData(strquery)
            If Dt.Rows.Count > 0 Then
                LblLeftBV.InnerText = Dt.Rows(0)("LeftBV")
                LblRightBV.InnerText = Dt.Rows(0)("RightBV")
                LblMatchingBv.InnerText = Dt.Rows(0)("LegXBVPaid")
                LblCarryA.InnerText = Dt.Rows(0)("LegXBVCF")
                LblCarryB.InnerText = Dt.Rows(0)("LegYBVCF")
            End If
            RadioButton()
        Catch ex As Exception
        End Try
    End Sub


    Public Sub BindSession()
        Dim sql As String = "Select * From( select SessID,Cast(SessID as varchar) + ' [' + Replace(Convert(varchar,FrmDate,106),' ','-') + ' to ' " & _
        " + ISNULL(Replace(Convert(varchar,ToDate,106),' ','-'),'') + ']' As SessnName,FrmDate " & _
        "   as FromDate,ToDate as Todate from M_SessnMaster Where ToDate Is Not Null) As Temp order by SessID"
        'objModuleFun.FillCombo(sql, ddlSession, "SessnName", "SessID")
        dt = New DataTable
        dt = objDAL.GetData(sql)

        DDLSession.DataSource = dt
        DDLSession.DataTextField = "SessnName"
        DDLSession.DataValueField = "SessId"
        DDLSession.DataBind()
        If dt.Rows.Count > 0 Then
            LblSDate.Text = Format(dt.Rows(0)("FromDate"), "dd-MMM-yyyy")

            LblTDate.Text = Format(dt.Rows(0)("ToDate"), "dd-MMM-yyyy")
        Else
            LblSDate.Text = ""
            LblTDate.Text = ""

        End If
    End Sub

    Public Sub FillKit()
        Dim query As String = ""

        query = "select 0 as kitId,'--All--' as KitName union Select kitId,KitName From M_KitMaster where RowStatus='Y'  Order By kitId"

        dt = New DataTable
        dt = objDAL.GetData(query)

        CmbKit.DataSource = dt
        CmbKit.DataTextField = "KitName"
        CmbKit.DataValueField = "KitId"
        CmbKit.DataBind()
    End Sub
    Private Sub FillDownline(Optional ByVal Condition As String = "", Optional ByVal IsSideA As Boolean = False)
        Dim dt1 As DataTable
        Dim dt2 As DataTable
        Dim Condition2 As String = ""
        Dim condition3 As String = ""
        Try
            formNo = GetFormNo()
            If RbtSearch.SelectedValue = "D" Then
                If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then
                    If DDlDate.SelectedValue = "J" Then

                        Condition2 = "and Cast(Upgradedate1 as date)>=Cast('" & txtStartDate.Text & "' as Date) and Cast(UpgradeDate1 as Date)<=Cast('" & txtEndDate.Text & "' as date)"
                    Else
                        Condition2 = "and Cast(Upgradedate1 as date)>=Cast('" & txtStartDate.Text & "' as Date) and cast(UpgradeDate1 as Date)<=Cast('" & txtEndDate.Text & "' as date) And ActiveStatus='Y'"
                    End If


                End If
            Else
                Condition2 = Condition2 & "and Cast(Upgradedate1 as date)>='" & LblSDate.Text & "' and Cast(Upgradedate1 as date)<='" & LblTDate.Text & "'"
            End If
            If ChkKit.Checked Then
                Condition2 = Condition2 & " And KitId='" & CmbKit.SelectedValue & "' "
            End If
            If DDlDate.SelectedValue = "A" Then
                Condition2 = Condition2 & " And Activestatus='Y'"
            End If

            Dim Str As String = ""
            If Session("CompId") = 1081 Or Session("CompId") = 1084 Then
                Str = " Select Sum(lActive) as LeftActive,Sum(lDeactive) as LeftDeactive," & _
           " sUM(Ractive) AS RightActive,Sum(RDeactive) as RightDeactive From " & _
      " (select Count(A.FormNoDwn) as lActive,0 as lDeactive,0 as Ractive,0 as RDeactive From M_MemTreeRelation As A Inner Join M_MemberMaster As B On A.FormNoDwn=B.FormNo " & _
        " where A.formno=" & Val(formNo.ToString()) & "  and A.LegNo='1' and B.ActiveStatus='Y' " & Condition2 & "" & _
          " Union All" & _
       " select 0 as lActive,Count(A.FormNoDwn) as lDeactive,0 as Ractive,0 as RDeactive From M_MemTreeRelation As A Inner Join M_MemberMaster As B On A.FormNoDwn=B.FormNo " & _
        " where A.formno=" & Val(formNo.ToString()) & "   and A.LegNo='1' and B.ActiveStatus='N' " & Condition2 & "" & _
         " Union All" & _
       " select 0 as lActive,0 as lDeactive,Count(A.FormNoDwn) as Ractive,0 as RDeactive From M_MemTreeRelation As A Inner Join M_MemberMaster As B On A.FormNoDwn=B.FormNo" & _
      " where A.formno=" & Val(formNo.ToString()) & "  and A.LegNo='2' and B.ActiveStatus='Y' " & Condition2 & "" & _
           " union All" & _
        " select 0 as lActive,0 as lDeactive,0 as Ractive,Count(A.FormNoDwn) as RDeactive From M_MemTreeRelation As A Inner Join M_MemberMaster As B On A.FormNoDwn=B.FormNo " & _
" where A.formno=" & Val(formNo.ToString()) & "  and A.LegNo='2' and B.ActiveStatus='N' " & Condition2 & ") as Temp"
            Else
                Str = " Select Sum(lActive) as LeftActive,Sum(lDeactive) as LeftDeactive," & _
           " sUM(Ractive) AS RightActive,Sum(RDeactive) as RightDeactive From " & _
      " (select Count(Formno) as lActive,0 as lDeactive,0 as Ractive,0 as RDeactive from V#Downline" & _
        " where formno=" & Val(formNo.ToString()) & "  and LegNo='1' and ActiveStatus='Y' " & Condition2 & "" & _
          " Union All" & _
       " select 0 as lActive,Count(Formno) as lDeactive,0 as Ractive,0 as RDeactive from V#Downline " & _
        " where formno=" & Val(formNo.ToString()) & "   and LegNo='1' and ActiveStatus='N' " & Condition2 & "" & _
         " Union All" & _
       " select 0 as lActive,0 as lDeactive,Count(Formno) as Ractive,0 as RDeactive from V#Downline" & _
      " where formno=" & Val(formNo.ToString()) & "  and LegNo='2' and ActiveStatus='Y' " & Condition2 & "" & _
           " union All" & _
        " select 0 as lActive,0 as lDeactive,0 as Ractive,Count(Formno) as RDeactive from V#Downline " & _
" where formno=" & Val(formNo.ToString()) & "  and LegNo='2' and ActiveStatus='N' " & Condition2 & ") as Temp"
            End If
           
            dt = New DataTable
            dt = objDAL.GetData(Str)
            If dt.Rows.Count > 0 Then
                Leftactive.Text = "Left Active: " & dt.Rows(0)("LeftActive")
                LeftDeactive.Text = "Left Deactive: " & dt.Rows(0)("LeftDeactive")
                RActive.Text = "Right Active: " & dt.Rows(0)("RightActive")
                RDeactive.Text = "Right Deactive: " & dt.Rows(0)("RightDeactive")
            End If
            If Session("compid") = "1075" Then
                GrdDirects1.Columns(8).Visible = True
                GrdDirects1.Columns(9).Visible = True

            Else
                GrdDirects1.Columns(1).Visible = True
                GrdDirects1.Columns(2).Visible = True
                GrdDirects1.Columns(3).Visible = True
                GrdDirects1.Columns(4).Visible = True
                GrdDirects1.Columns(5).Visible = True
                GrdDirects1.Columns(6).Visible = True
                GrdDirects1.Columns(7).Visible = True
                GrdDirects1.Columns(8).Visible = False
                GrdDirects1.Columns(9).Visible = False
                GrdDirects1.Columns(10).Visible = True
                GrdDirects1.Columns(11).Visible = True
            End If
            If Session("compid") = "1075" Then
                GrdDirects2.Columns(8).Visible = True
                GrdDirects2.Columns(8).Visible = True

            Else
                GrdDirects2.Columns(1).Visible = True
                GrdDirects2.Columns(2).Visible = True
                GrdDirects2.Columns(3).Visible = True
                GrdDirects2.Columns(4).Visible = True
                GrdDirects2.Columns(5).Visible = True
                GrdDirects2.Columns(6).Visible = True
                GrdDirects2.Columns(7).Visible = True
                GrdDirects2.Columns(8).Visible = False
                GrdDirects2.Columns(9).Visible = False
                GrdDirects2.Columns(10).Visible = True
                GrdDirects2.Columns(11).Visible = True
            End If

            'dbConnect.OpenConnection()
            ' strquery = "select * from V#Downline " & Val(formNo.ToString()) & "," & IIf(IsSideA, 1, 2) & "," & IIf(Condition2 = "", "", " " & Condition2) & "," & IIf(condition3 = "", "", " " & condition3)

            'Comm = New SqlCommand(strquery, dbConnect.cnnObject)
            'Adp = New SqlDataAdapter(Comm)
            'ds = objDAL.GenerateTreeProc(strquery)
            If Session("CompID") = "1038" Then
                If rbleg.SelectedValue = "0" Or rbleg.SelectedValue = "1" Then
                    ' Adp.Fill(ds, "Directs1")
                    strquery = "select '' as address,'' as Mobl,*,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='1' " & Condition2 & "  Order by JoinDate"

                    dt1 = New DataTable
                    dt1 = objDAL.GenerateTreeProc(strquery)
                    Session("DirectData1") = dt1
                    GrdDirects1.DataSource = dt1
                    GrdDirects1.DataBind()
                    DivSideA.Style("display") = "block"
                    
                End If
                If rbleg.SelectedValue = "0" Or rbleg.SelectedValue = "2" Then
                    strquery = "select '' as address,'' as Mobl,*,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='2' " & Condition2 & " order by JoinDate"


                    ' Adp.Fill(ds, "Directs2")
                    dt2 = New DataTable
                    dt2 = objDAL.GenerateTreeProc(strquery)
                    Session("DirectData2") = dt2
                    GrdDirects2.DataSource = dt2
                    GrdDirects2.DataBind()
                    DivSideB.Style("display") = "block"
                    
                End If
               
            Else
                If Session("CompID") = "1075" Then
                    If rbleg.SelectedValue = "0" Or rbleg.SelectedValue = "1" Then
                        ' Adp.Fill(ds, "Directs1")
                        strquery = "select *,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='1' " & Condition2 & "  Order by JoinDate"

                        dt1 = New DataTable
                        dt1 = objDAL.GenerateTreeProc(strquery)
                        Session("DirectData1") = dt1
                        GrdDirects1.DataSource = dt1
                        GrdDirects1.DataBind()
                        DivSideA.Style("display") = "block"
                    End If
                    If rbleg.SelectedValue = "0" Or rbleg.SelectedValue = "2" Then
                        strquery = "select *,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='2' " & Condition2 & " order by JoinDate"


                        ' Adp.Fill(ds, "Directs2")
                        dt2 = New DataTable
                        dt2 = objDAL.GenerateTreeProc(strquery)
                        Session("DirectData2") = dt2
                        GrdDirects2.DataSource = dt2
                        GrdDirects2.DataBind()
                        DivSideB.Style("display") = "block"

                    End If
                ElseIf Session("CompID") = "1081" Or Session("CompId") = 1084 Then
                    If rbleg1.SelectedValue = "0" Or rbleg1.SelectedValue = "1" Then
                        ' Adp.Fill(ds, "Directs1")
                        strquery = "select '' as address,'' as Mobl,*,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='1' " & Condition2 & "  Order by JoinDate"

                        dt1 = New DataTable
                        dt1 = objDAL.GenerateTreeProc(strquery)
                        Session("DirectData1") = dt1
                        If dt1.Rows.Count > 0 Then
                            GrdDirects1.DataSource = dt1
                            GrdDirects1.DataBind()
                            GrdDirects1.Visible = True
                            DivSideA.Style("display") = "block"
                        Else
                            GrdDirects1.Visible = False
                        End If


                    End If
                    If rbleg1.SelectedValue = "0" Or rbleg1.SelectedValue = "2" Then
                        strquery = "select '' as address,'' as Mobl,*,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='2' " & Condition2 & " order by JoinDate"



                        ' Adp.Fill(ds, "Directs2")
                        dt2 = New DataTable
                        dt2 = objDAL.GenerateTreeProc(strquery)
                        Session("DirectData2") = dt2
                        If dt2.Rows.Count > 0 Then
                            GrdDirects2.DataSource = dt2
                            GrdDirects2.DataBind()

                            GrdDirects2.Visible = True
                            DivSideB.Style("display") = "block"
                        Else
                            GrdDirects2.Visible = False
                        End If



                    End If
                Else

                    If rbleg.SelectedValue = "0" Or rbleg.SelectedValue = "1" Then
                        ' Adp.Fill(ds, "Directs1")
                        strquery = "select '' as address,'' as Mobl,*,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='1' " & Condition2 & "  Order by JoinDate"

                        dt1 = New DataTable
                        dt1 = objDAL.GenerateTreeProc(strquery)
                        Session("DirectData1") = dt1
                        If dt1.Rows.Count > 0 Then
                            GrdDirects1.DataSource = dt1
                            GrdDirects1.DataBind()
                            GrdDirects1.Visible = True
                            DivSideA.Style("display") = "block"
                        Else
                            GrdDirects1.Visible = False
                        End If
                        

                    End If
                    If rbleg.SelectedValue = "0" Or rbleg.SelectedValue = "2" Then
                        strquery = "select '' as address,'' as Mobl,*,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='2' " & Condition2 & " order by JoinDate"



                        ' Adp.Fill(ds, "Directs2")
                        dt2 = New DataTable
                        dt2 = objDAL.GenerateTreeProc(strquery)
                        Session("DirectData2") = dt2
                        If dt2.Rows.Count > 0 Then
                            GrdDirects2.DataSource = dt2
                            GrdDirects2.DataBind()
                            Session("DirectData2") = dt2
                            DivSideB.Style("display") = "block"
                            GrdDirects2.Visible = True
                        Else
                            GrdDirects2.Visible = False
                        End If
                        

                    End If
                End If
            End If



            'Comm.Cancel()
            'ds.Dispose()
            RadioButton()





        Catch ex As Exception
            If IsSideA Then
                Response.Write(ex.Message & "SideA")
            Else
                Response.Write(ex.Message & "SideB")
            End If
        End Try
    End Sub
    Private Sub ExportDownline(ByVal Str As String, Optional ByVal Condition As String = "")
        Try
            Dim dtTemp As New DataTable



            Dim dg As New DataGrid
            'Adp.Fill(ds, "ExportToExcel")
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(Str)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("Downline.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
        'Comm.Cancel()
        'ds.Dispose()

    End Sub

    Protected Sub rbleg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbleg.SelectedIndexChanged
        RadioButton()
    End Sub
    Protected Sub rbleg1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbleg1.SelectedIndexChanged
        RadioButton()
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
    Public Shared Sub ExportToSpreadsheet(ByVal table As DataTable, ByVal name As String)
        Dim context As HttpContext = HttpContext.Current
        context.Response.Clear()

        For Each column As DataColumn In table.Columns
            context.Response.Write(column.ColumnName + ";")
        Next

        context.Response.Write(Environment.NewLine)

        For Each row As DataRow In table.Rows
            For i As Integer = 0 To table.Columns.Count - 1
                context.Response.Write(row(i).ToString().Replace(";", String.Empty) & ";")
            Next
            context.Response.Write(Environment.NewLine)
        Next

        context.Response.ContentType = "text/csv"
        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" & name & ".csv")
        context.Response.[End]()
    End Sub

    Private Sub RadioButton()
        If Session("CompId") = "1081" Or Session("CompId") = 1084 Then
            If (rbleg1.SelectedIndex = 1) Then
                DivSideA.Style("display") = "block"
                DivSideB.Style("display") = "none"
                trRightHeading.Visible = False
                TrLeftHeading.Visible = True
                BtnExportA.Visible = True
                BtnExportB.Visible = False
            ElseIf (rbleg1.SelectedIndex = 2) Then
                DivSideA.Style("display") = "none"
                DivSideB.Style("display") = "block"
                trRightHeading.Visible = True
                TrLeftHeading.Visible = False
                BtnExportA.Visible = False
                BtnExportB.Visible = True
            Else
                DivSideA.Style("display") = "block"
                DivSideB.Style("display") = "block"
                trRightHeading.Visible = True
                TrLeftHeading.Visible = True
                BtnExportB.Visible = True
                BtnExportB.Visible = True
            End If
        Else
            If (rbleg.SelectedIndex = 1) Then
                DivSideA.Style("display") = "block"
                DivSideB.Style("display") = "none"
                trRightHeading.Visible = False
                TrLeftHeading.Visible = True
                BtnExportA.Visible = True
                BtnExportB.Visible = False
            ElseIf (rbleg.SelectedIndex = 2) Then
                DivSideA.Style("display") = "none"
                DivSideB.Style("display") = "block"
                trRightHeading.Visible = True
                TrLeftHeading.Visible = False
                BtnExportA.Visible = False
                BtnExportB.Visible = True
            Else
                DivSideA.Style("display") = "block"
                DivSideB.Style("display") = "block"
                trRightHeading.Visible = True
                TrLeftHeading.Visible = True
                BtnExportB.Visible = True
                BtnExportB.Visible = True
            End If
        End If
        
    End Sub


    Protected Sub BtnExportB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportB.Click
        Dim cond As String = ""
        formNo = GetFormNo()
        If RbtSearch.SelectedValue = "D" Then
            If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then
                If DDlDate.SelectedValue = "J" Then
                    cond = "and Cast(UpgradeDate1 as date)>=Cast('" & txtStartDate.Text & "' as Date) and Cast(UpgradeDate1 as date)<=Cast('" & txtEndDate.Text & "' as Date)"
                Else
                    cond = "and Cast(UpgradeDate1 as date)>=Cast('" & txtStartDate.Text & "' as Date) and Cast(UpgradeDate1 as date)<=cast('" & txtEndDate.Text & "' as date) And ActiveStatus='Y'"
                End If
            End If
        Else
            cond = "and Cast(UpgradeDate1 as date)>='" & LblSDate.Text & "'  and Cast(UpgradeDate1 as date)<='" & LblTDate.Text & "'"
        End If
        If ChkKit.Checked Then
            cond = cond & " And KitId='" & CmbKit.SelectedValue & "' "
        End If
        If DDlDate.SelectedValue = "A" Then
            cond = cond & " And Activestatus='Y' "
        End If
        ''Sponsor as UplinerId,SponsorName as UplinerName,
        Dim str As String = ""
        If Session("CompId") = "1038" Then
            str = "select IDNO,MemName as MemberName,RefFormno as SponsorId,ReferalName as SponsorName,doj as Doj,JoiningTime,TopUpDate ,TopUpTime, KitName as Package,KitAmount as PackageAmount,Bv  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='2' " & cond & " order by JoinDate"
        
        End If
        If Session("CompId") = "1075" Then
            str = "select  IDNO,MemName as MemberName,RefFormno as SponsorId,ReferalName as SponsorName,Mobl as Mobile,Address,doj as Doj,JoiningTime,TopUpDate ,TopUpTime, KitName as Package,KitAmount as PackageAmount,Bv as Unit  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='2' " & cond & " order by JoinDate"
        Else

            str = "select top 100 IDNO,MemName as MemberName,RefFormno as SponsorId,ReferalName as SponsorName,doj as Doj,JoiningTime,TopUpDate ,TopUpTime, KitName as Package,KitAmount as PackageAmount,Bv  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='2' " & cond & " order by JoinDate"
        End If
      


        ExportDownline(str, "")
    End Sub

    Protected Sub BtnExportA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportA.Click
        Dim cond As String = ""
        formNo = GetFormNo()
        If RbtSearch.SelectedValue = "D" Then
            If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then
                If DDlDate.SelectedValue = "J" Then
                    cond = "and Cast(UpgradeDate1 as date)>=Cast('" & txtStartDate.Text & "' as date) and Cast(UpgradeDate1 as date)<=Cast('" & txtEndDate.Text & "' as Date)"
                Else
                    cond = "and Cast(UpgradeDate1 as date)>=Cast('" & txtStartDate.Text & "' as Date) and Cast(UpgradeDate1 as date)<=Cast('" & txtEndDate.Text & "' as Date) And ActiveStatus='Y'"
                End If
            End If
        Else
            cond = "and Cast(UpgradeDate1 as date)>='" & LblSDate.Text & "' and Cast(UpgradeDate1 as date)<='" & LblTDate.Text & "' "

        End If
        If ChkKit.Checked Then
            cond = cond & " And KitId='" & CmbKit.SelectedValue & "' "
        End If

        If DDlDate.SelectedValue = "A" Then
            cond = cond & " And Activestatus='Y' "
        End If
        ''Sponsor as UplinerId,SponsorName as UplinerName,
        Dim str As String = ""
        If Session("CompId") = "1038" Then
            str = "select IDNO,MemName as MemberName,RefFormno as SponsorId,ReferalName as SponsorName,doj as Doj,JoiningTime,TopUpDate ,TopUpTime, KitName as Package,KitAmount as PackageAmount,Bv  from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='1' " & cond & " order by JoinDate"
        Else
            If Session("CompId") = "1075" Then
                str = "select  IDNO,MemName as MemberName,RefFormno as SponsorId,ReferalName as SponsorName,doj as Doj,JoiningTime,TopUpDate ,TopUpTime, KitName as Package,Mobl as Mobile,Address,KitAmount as PackageAmount,Bv as Unit    from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='1' " & cond & " order by JoinDate"
            Else
                str = "select top 100 IDNO,MemName as MemberName,RefFormno as SponsorId,ReferalName as SponsorName,doj as Doj,JoiningTime,TopUpDate ,TopUpTime, KitName as Package,KitAmount as PackageAmount,Bv    from V#Downline where formno=" & Val(formNo.ToString()) & " and LegNo='1' " & cond & " order by JoinDate"
            End If
           End If
        ExportDownline(str, "")
    End Sub

    Protected Sub GrdDirects1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles GrdDirects1.PageIndexChanged
        GrdDirects1.CurrentPageIndex = e.NewPageIndex
        GrdDirects1.DataSource = Session("DirectData1")
        GrdDirects1.DataBind()
       
    End Sub

    Protected Sub GrdDirects2_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles GrdDirects2.PageIndexChanged
        GrdDirects2.CurrentPageIndex = e.NewPageIndex
        GrdDirects2.DataSource = Session("DirectData2")
        GrdDirects2.DataBind()
        
    End Sub

    Protected Sub btnShowDownline_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowDownline.Click
        formNo = GetFormNo()
        If formNo = "" Then
        Else
            lblError.Text = ""
            FillDownlineSumm(formNo)
            divMemDownline.Visible = True
            ' filldetail()
            FillDownline()
            FillDownline(, False)
        End If
    End Sub

    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        'Dim formno As String
        idNo = txtMemberId.Text
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formNo = dt.Rows(0)("FormNo")
        Else
            lblError.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblError.Visible = True
            txtMemberId.Text = ""
        End If
        Return formNo
    End Function
   
    Protected Sub RbtSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtSearch.SelectedIndexChanged
        If RbtSearch.SelectedValue = "S" Then
            DDLSession.Visible = True
            txtEndDate.Visible = False
            txtStartDate.Visible = False
            lblEndDate.Visible = False
            lblStartDate.Visible = False
            DDlDate.Visible = False
        Else
            DDLSession.Visible = False
            txtEndDate.Visible = True
            txtStartDate.Visible = True
            lblEndDate.Visible = True
            lblStartDate.Visible = True
            DDlDate.Visible = True
        End If
    End Sub

    Protected Sub DDLSession_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLSession.SelectedIndexChanged
        Dim sql As String = "select Cast(Replace(Convert(Varchar,FrmDate,106),' ','-')as DateTime) as FromDate,Cast(Replace(Convert(Varchar,ToDate,106),' ','-') as dateTime) as ToDate" & _
        ",FrmDate  as FDate,ToDate  as TDate from M_SessnMaster where Sessid='" & Val(DDLSession.SelectedValue) & "' "
        dt = objDAL.GetData(sql)
        If dt.Rows.Count > 0 Then
            LblSDate.Text = Format(dt.Rows(0)("FDate"), "dd-MMM-yyyy")
            LblTDate.Text = Format(dt.Rows(0)("TDate"), "dd-MMM-yyyy")
          
        Else
            LblTDate.Text = ""
            LblSDate.Text = ""
            LblSTDate.Text = ""
            LblToDate.Text = ""
        End If
    End Sub
End Class
