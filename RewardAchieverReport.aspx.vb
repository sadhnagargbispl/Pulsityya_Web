Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class App_UI_Application_Pages_RewardAchieverReport
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
                If RbReqStatus.SelectedValue = "A" Then
                    sessiddate.Visible = False
                    noramldate.Visible = True
                Else
                    sessiddate.Visible = True
                    noramldate.Visible = False
                End If
                If Session("compid") = "1055" Then
                    RbReqStatusid.Visible = True

                Else
                    RbReqStatusid.Visible = False

                End If
                Filldate()
                BindSession()
                FillCityPinDetail()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub FillCityPinDetail()
        Try
            Dim sql As String = String.Empty
            If Session("CompId") = "1049" Then
                sql = "Exec sp_rank"
            ElseIf Session("CompId") = "1056" Then
                sql = "Exec sp_Orbitrank"
            ElseIf Session("CompId") = "1057" Then
                sql = " select Rewardid as RankId,Designation+'/'+Reward as Rank from M_RewardNew where " & _
                " activeStatus='Y' Order by RankId"
            ElseIf Session("CompId") = "1038" Then
                sql = " Select RankId,Rank from MstLevelRanks Where ActiveStatus = 'Y' Order by Rankid"
            ElseIf Session("CompId") = "1083" Then
                'sql = " Select RankId,Rankname as Rank from MstRanksNew Where ActiveStatus = 'Y' Order by Rankid"
                sql = " Exec Sp_DDLRankNAme"
            ElseIf Session("CompId") = "1084" Then
                sql = "    select * from V#RewardRank Order by Rankid"
            ElseIf Session("CompId") = "1095" Then
                sql = " select Rewardid as RankId,Rank from M_RewardMaster where " & _
                " activeStatus='Y' Order by RewardId"
            ElseIf Session("CompId") = "1105" Then
                sql = " select Rewardid as RankId,Rank from M_RewardMaster where " & _
                " activeStatus='Y' Order by RewardId"
            ElseIf Session("CompId") = "1107" Then
                sql = " select Rewardid as RankId,Rank from M_RewardMaster where " & _
                " activeStatus='Y' Order by RewardId"
            ElseIf Session("CompId") = "1103" Then
                sql = "select Rewardid as RankId,Rank+' - '+reward as Rank from MstRewards where  activeStatus='Y' Order by RewardId"
            Else
                sql = " Select RankId,Rank from MstRanks Where ActiveStatus = 'Y' Order by Rankid"
            End If
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            ddlstate.DataSource = dt
            ddlstate.DataTextField = "Rank"
            ddlstate.DataValueField = "RankId"
            ddlstate.DataBind()
            ddlstate.Items.Insert(0, "--Select Rank\Reward--")
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
    Public Sub BindSession()
        Try
            Dim Ds As DataSet
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetRankWeeklySession")
            ddlSession.DataSource = Ds.Tables(0)
            ddlSession.DataValueField = "SessID"
            ddlSession.DataTextField = "SessnName"
            ddlSession.DataBind()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillReport()
        Try

            Dim sql As String = String.Empty
            Dim Idno As String = "0"
            If Session("CompId") = "1049" Then
                If txtMemberID.Text <> "" Then
                    Idno = txtMemberID.Text.Trim
                Else
                    Idno = "0"
                End If
                sql = "exec sp_GetRankAchieverDetail '" & Idno & "','" & ddlstate.SelectedValue & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "',1,50000,'Y',1"
            ElseIf Session("CompId") = "1057" Then
                Dim Rankid As Integer = 0
                If txtMemberID.Text <> "" Then
                    Idno = txtMemberID.Text.Trim
                Else
                    Idno = "0"
                End If
                If ddlstate.SelectedValue = "--Select Rank--" Then
                    Rankid = 0
                Else
                    Rankid = ddlstate.SelectedValue
                End If
                sql = "exec sp_GetRankAchieverDetail '" & Idno & "','" & Rankid & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "',1,50000,'Y',1"


            ElseIf Session("CompId") = "1055" Then
                Dim Rankid As Integer = 0
                If txtMemberID.Text <> "" Then
                    Idno = txtMemberID.Text.Trim
                Else
                    Idno = "0"
                End If
                If (ddlstate.SelectedIndex > 0) Then
                    Rankid = ddlstate.SelectedValue
                Else
                    Rankid = 0
                End If
                If RbReqStatus.SelectedValue = "A" Then
                    sql = "exec sp_GetRankAchieverDetailNew '" & Idno & "','" & Rankid & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "',1,50000,'N',10,'A','0'"
                Else
                    sql = "exec sp_GetRankAchieverDetailNew '" & Idno & "','" & Rankid & "','','',1,50000,'N',10,'N'," & ddlSession.SelectedValue & ""
                End If




            ElseIf Session("Compid") = "1056" Then
                If txtMemberID.Text <> "" Then
                    Idno = txtMemberID.Text.Trim
                Else
                    Idno = "0"
                End If
                sql = "exec sp_GetOrbitRankAchieverDetail '" & Idno & "','" & ddlstate.SelectedValue & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "',1,50000,'Y',1"
            Else
                If (Session("CompID") = "1007") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.Reward,Case When (a.Comm = 0 And b.RankID = 1)  Then Replace(Convert(Varchar,c.FrmDate,106),' ','-')  "
                    sql &= " + ' (Without Double Matching Bonus)' Else"
                    sql &= " Replace(Convert(Varchar,c.FrmDate,106),' ','-') End  As Date  "
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                ElseIf (Session("CompID") = "1041") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD"
                    sql &= " left join M_MonthSessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                ElseIf (Session("CompID") = "1062") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.FTReward as Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1"
                ElseIf (Session("CompID") = "1059") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.ftReward as [Reward],Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                ElseIf (Session("CompID") = "1038") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstLevelAchievers as A left join MstLevelRanks As b on a.RankID = b.RankiD"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                ElseIf (Session("CompID") = "1066") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],isnull(e.Idno,'') [Sponser IDNo],isnull(e.MemFirstName,'') as [Sponser Name],"
                    sql &= " b.Rank,b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1"
                ElseIf (Session("CompID") = "1083") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],isnull(e.Idno,'') [Sponser IDNo],isnull(e.MemFirstName,'') as [Sponser Name],"
                    sql &= " b.RankName as Rank,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from V#RankAchAdmin As b"
                    sql &= " left join D_SessnMaster as c on b.DSessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on b.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1"
                ElseIf (Session("CompID") = "1084") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " Designation as Rank,b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from DailyRankAchievers as A left join M_RewardNew As b on a.RewardID = b.RewardID "
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                ElseIf (Session("CompID") = "1095") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstRankAchievers as A left join M_RewardMaster As b on a.RankID = b.RewardID"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                ElseIf (Session("CompID") = "1074") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,f.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD  left join MstReward As f on a.RankID = f.Rewardid"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                ElseIf (Session("CompID") = "1105") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from Mstrewardachievers as A left join M_RewardMaster As b on b.RewardID = a.RewardID"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                ElseIf (Session("CompID") = "1107") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from Mstrewardachievers as A left join M_RewardMaster As b on b.RewardID = a.RewardID"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                ElseIf (Session("CompID") = "1106") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.Reward, Replace(Convert(Varchar,Isnull(a.rectimestamp,Getdate()),106),' ','-') as Date"
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD "
                    sql &= "  Left join M_memberMaster as d "
                    sql &= "  on a.formno = d.formno  Left Join M_memberMaster as e on e.formno = d.refformno  Where 1=1 "
                ElseIf (Session("CompID") = "1103") Then
                    sql = "  Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name], "
                    sql &= " b.Rank,b.Reward,FORMAT(CAST(LEFT(a.sessid,8) AS DATE), 'dd-MMM-yyyy') + '-' + RIGHT(a.sessid,2) as Date  from MstRewardAchievers as A  "
                    sql &= " left join MstRewards As b on a.RewardId = b.RewardId left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                Else
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                End If
                Dim startDate As Date
                Dim endDate As Date
                If (ddlstate.SelectedIndex > 0) Then
                    If Session("CompID") = "1095" Or (Session("CompID") = "1105") Or (Session("CompID") = "1107") Or (Session("CompID") = "1103") Then
                        sql &= "      And  b.RewardID = " & ddlstate.SelectedValue & " "
                    Else
                        sql &= "      And  b.RankID = " & ddlstate.SelectedValue & " "
                    End If
                End If

                If (txtMemberID.Text <> "") Then
                    sql &= "     And  d.Idno = '" & txtMemberID.Text & "' "
                End If

                If Session("CompID") = "1106" Then
                    If txtStartDate.Text = "" Then
                        startDate = Session("CompDate")
                    Else
                        sql &= "     And  Cast(a.rectimestamp As Date ) >= Cast('" & txtStartDate.Text & "' As Date ) "
                    End If
                    If txtEndDate.Text = "" Then
                        endDate = Format(Date.Now, "dd-MMM-yyyy")
                    Else
                        sql &= "   And  Cast(a.rectimestamp As Date ) <= Cast('" & txtEndDate.Text & "' As Date ) order by a.Sessid desc"
                    End If


                Else

                    If txtStartDate.Text = "" Then
                        startDate = Session("CompDate")
                    Else
                        sql &= "     And  Cast(FrmDate As Date ) >= Cast('" & txtStartDate.Text & "' As Date ) "
                    End If
                    If txtEndDate.Text = "" Then
                        endDate = Format(Date.Now, "dd-MMM-yyyy")
                    Else
                        sql &= "     And  Cast(FrmDate As Date ) <= Cast('" & txtEndDate.Text & "' As Date ) order by c.Sessid desc"
                    End If


                End If




            End If
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            'If Session("CompID") = 1038 Or Session("CompID") = 1066 Then
            If Session("CompID") = 1038 Then
                GridView1.DataSource = dt
                GridView1.DataBind()
                GvData.Visible = False
            ElseIf Session("CompID") = 1057 Then
                GridView2.DataSource = dt
                GridView2.DataBind()
            ElseIf Session("CompID") = 1055 Then
                GridView3.DataSource = dt
                GridView3.DataBind()
                GvData.Visible = False
            ElseIf Session("CompID") = 1083 Then
                GridView3.DataSource = dt
                GridView3.DataBind()
                GvData.Visible = False
            Else
                GvData.DataSource = dt
                GvData.DataBind()
            End If

            Session("GData") = dt
        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillReport_TeamBigway()
        Try
            Dim sql As String = String.Empty
            sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No.],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
            sql &= " b.Rank,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
            sql &= " from M_SessWiseBv  as A Inner join MstRanks As b on a.RankID = b.RankiD"
            sql &= " left join M_SessnMAster as c on a.SessID = c.SessID "
            sql &= " Left join M_memberMaster as d on a.formno = d.formno "
            sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
            If (ddlstate.SelectedIndex > 0) Then
                sql &= "      And  b.RankID = " & ddlstate.SelectedValue & " "
            End If
            If (txtMemberID.Text <> "") Then
                sql &= "     And  d.Idno = '" & txtMemberID.Text & "' "
            End If
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            GvData.DataSource = dt
            GvData.DataBind()
            Session("GData") = dt
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Session("CompID") = "1006" Then
            FillReport_TeamBigway()
        Else
            FillReport()
        End If
    End Sub
    Protected Sub GridView2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView2.PageIndexChanging
        GridView2.PageIndex = e.NewPageIndex
        If Session("CompID") = "1006" Then
            FillReport_TeamBigway()
        Else
            FillReport()
        End If
    End Sub
    Protected Sub GridView3_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView3.PageIndexChanging
        GridView3.PageIndex = e.NewPageIndex

        FillReport()

    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        If Session("CompID") = "1006" Then
            FillReport_TeamBigway()
        Else
            FillReport()
        End If
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dg As New DataGrid
            If Session("CompID") = "1006" Then
                FillReport_TeamBigway()
            Else
                FillReport()
            End If
            'ExportExcel()
            dg.DataSource = Session("GData")
            dg.DataBind()

            ExportToExcel("RankAchieverReport.xls", dg)
        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
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
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GData")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Incentive")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=RankAchieverReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using
    End Sub
    Protected Sub RbReqStatus_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbReqStatus.TextChanged
        If RbReqStatus.SelectedValue = "A" Then
            sessiddate.Visible = False
            noramldate.Visible = True
        Else
            sessiddate.Visible = True
            noramldate.Visible = False
        End If
        FillReport()
    End Sub
End Class
