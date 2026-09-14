

Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class MechingIncomePayout
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
        'Try

        '    Dim sql As String = String.Empty
        '    If Session("CompId") = "1049" Then
        '        sql = "Exec sp_rank"
        '    ElseIf Session("CompId") = "1056" Then
        '        sql = "Exec sp_Orbitrank"
        '    ElseIf Session("CompId") = "1057" Then
        '        sql = " select Rewardid as RankId,Designation+'/'+Reward as Rank from M_RewardNew where " & _
        '        " activeStatus='Y' Order by RankId"

        '    Else
        '        sql = " Select RankId,Rank from MstRanks Where ActiveStatus = 'Y' Order by Rankid"
        '    End If
        '    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        '    dt = objDAL.GetData(sql)
        '    ddlstate.DataSource = dt
        '    ddlstate.DataTextField = "Rank"
        '    ddlstate.DataValueField = "RankId"
        '    ddlstate.DataBind()
        '    ddlstate.Items.Insert(0, "--Select Rank--")
        'Catch ex As Exception

        'End Try
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

    Private Sub FillReport()
        Try

            Dim sql As String = String.Empty
            Dim Idno As String = "0"
            sql = sql & "   Select Cast(Convert(Varchar,Cast(Cast(f.sessid as Varchar) as DateTime),112) as DateTime) as Date, "
            sql = sql & "  case when cast([Income Date] as date)='30-aug-2022' then (DistributeIncome-netamount) else  TdsAmount end as TdsAmount, "
            sql = sql & "  DistributeIncome as DistributeIncome,   case when cast([Income Date] as date)='30-aug-2022' then netamount else  NetIncome end as [Gross Income],"
            sql = sql & "  f.PairIncome as  [Team Performance Incentive],f.RewardInc as  [RewardIncome] "
            sql = sql & " from MatchingIncome as A ,  "
            sql = sql & "  M_memberMaster as d,  "
            sql = sql & " D_MonthlyPayDetail as f,D_SessnMaster  as sn"
            sql = sql & "  Where(1 = 1 And a.username = d.idno And f.formno = d.formno AND f.sessid=sn.sessid)"
            Dim startDate As Date
            Dim endDate As Date

            If (txtMemberID.Text <> "") Then
                sql &= "     And  d.Idno = '" & txtMemberID.Text & "' "
            End If

            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                sql &= "     And  Cast(Convert(Varchar,Cast(Cast(f.sessid as Varchar) as DateTime),112) as DateTime) >= Cast('" & txtStartDate.Text & "' As Date ) "
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                sql &= "     And  Cast(Convert(Varchar,Cast(Cast(f.sessid as Varchar) as DateTime),112) as DateTime) <= Cast('" & txtEndDate.Text & "' As Date ) "
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
            'If (ddlstate.SelectedIndex > 0) Then
            '    sql &= "      And  b.RankID = " & ddlstate.SelectedValue & " "
            'End If
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
