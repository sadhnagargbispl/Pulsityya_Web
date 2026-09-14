
Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class DRankAchiverReport
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim dtData As New DataTable
    Dim objGen As clsGeneral = New clsGeneral
    Dim Ds As DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
            Else
                Response.Redirect("logout.aspx")
            End If

            If Not Page.IsPostBack Then
                Filldate()
                FillCityPinDetail()


            End If


        Catch ex As Exception

        End Try
    End Sub

    'Private Sub FillCityPinDetail()
    '    Try

    '        Dim sql As String = String.Empty
    '        sql = " Select SessID,     "
    '        sql &= " (Convert(Varchar,SessID) +'-- ' + Replace(Convert(Varchar,FrmDate,106),' ','-') +' - '+ Replace(Convert(Varchar,ToDate,106),' ','-')) as Session"
    '        sql &= " From M_SessnMaster Where todate is not null"
    '        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '        dt = objDAL.GetData(sql)
    '        ddlstate.DataSource = dt
    '        ddlstate.DataTextField = "Session"
    '        ddlstate.DataValueField = "SessID"
    '        ddlstate.DataBind()
    '        ddlstate.Items.Insert(0, "--Select Session--")
    '    Catch ex As Exception

    '    End Try
    'End Sub


    Private Sub FillCityPinDetail()
        Try

            Dim sql As String = String.Empty
            If Session("CompId") = "1049" Then
                sql = "Exec sp_rank"
            ElseIf Session("CompId") = "1056" Then
                sql = "Exec sp_Orbitrank"
            ElseIf Session("CompId") = "1055" Then
                sql = " Select RankId,Rank from MstClub Where ActiveStatus = 'Y' Order by Rankid"
            Else
                sql = " Select RankId,Rank from MstRanks Where ActiveStatus = 'Y' Order by Rankid"
            End If
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            ddlstate.DataSource = dt
            ddlstate.DataTextField = "Rank"
            ddlstate.DataValueField = "RankId"
            ddlstate.DataBind()
            ddlstate.Items.Insert(0, "--Select Rank--")
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Filldate()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetDailySessionDetails ")
            'DDlFromDate.DataSource = Ds.Tables(0)
            'DDlFromDate.DataValueField = "Fromdate"
            'DDlFromDate.DataTextField = "Fromdate"
            'DDlFromDate.DataBind()
            'DDltodate.DataSource = Ds.Tables(0)
            'DDltodate.DataValueField = "Todate"
            'DDltodate.DataTextField = "Todate"
            'DDltodate.DataBind()
        Catch ex As Exception
        End Try
        'Try


        '    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        '    Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
        '    dtData = New DataTable
        '    dtData = objDAL.GetData(Str)
        '    If dtData.Rows.Count > 0 Then
        '        txtStartDate.Text = dtData.Rows(0)("CurrentDate")
        '        txtEndDate.Text = dtData.Rows(0)("CurrentDate")
        '    End If
        'Catch ex As Exception

        'End Try
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
                sql = "exec sp_GetRankAchieverDetail '" & Idno & "','" & ddlstate.SelectedValue & "',1,50000,'Y',1"

            ElseIf Session("Compid") = "1056" Then
                If txtMemberID.Text <> "" Then
                    Idno = txtMemberID.Text.Trim
                Else
                    Idno = "0"
                End If
                sql = "exec sp_GetOrbitRankAchieverDetail '" & Idno & "','" & ddlstate.SelectedValue & "',1,50000,'Y',1"

            Else



                If (Session("CompID") = "1007") Then

                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No.],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.Reward,Case When (a.Comm = 0 And b.RankID = 1)  Then Replace(Convert(Varchar,c.FrmDate,106),' ','-')  "
                    sql &= " + ' (Without Double Matching Bonus)' Else"
                    sql &= " Replace(Convert(Varchar,c.FrmDate,106),' ','-') End  As Date  "
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "

                ElseIf (Session("CompID") = "1041") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No.],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD"
                    sql &= " left join M_MonthSessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "

                ElseIf (Session("CompID") = "1055") Then
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No.],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstClubAchievers as A left join MstClub As b on a.RankID = b.RankiD"
                    sql &= " left join M_MonthSessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                Else
                    sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No.],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
                    sql &= " b.Rank,b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD"
                    sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
                End If


                Dim startDate As Date
                Dim endDate As Date

                If (ddlstate.SelectedIndex > 0) Then
                    sql &= "      And  b.RankID = " & ddlstate.SelectedValue & " "
                End If
                If (txtMemberID.Text <> "") Then
                    sql &= "     And  d.Idno = '" & txtMemberID.Text & "' "
                End If

                'If DDlFromDate.SelectedValue = "" Then
                '    startDate = Session("CompDate")
                'Else
                '    sql &= "     And  Cast(FrmDate As Date ) >= Cast('" & DDlFromDate.SelectedValue & "' As Date ) "
                'End If
                'If DDltodate.SelectedValue = "" Then
                '    endDate = Format(Date.Now, "dd-MMM-yyyy")
                'Else
                '    sql &= "     And  Cast(FrmDate As Date ) <= Cast('" & DDltodate.SelectedValue & "' As Date ) "
                'End If

            End If
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            GvData.DataSource = dt
            GvData.DataBind()

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

            If Session("CompID") = "1006" Then
                FillReport_TeamBigway()
            Else
                FillReport()
            End If
            ExportExcel()

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")

        End Try
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


End Class
