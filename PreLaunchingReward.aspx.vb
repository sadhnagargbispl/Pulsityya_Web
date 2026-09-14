Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class PreLaunchingReward
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
            Else
                Response.Redirect("logout.aspx")
            End If

            If Not Page.IsPostBack Then
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

            sql = " Select RewardId As RankId, Reward As Rank from M_PreLaunchReward Where ActiveStatus = 'Y' Order by RewardId"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            ddlstate.DataSource = dt
            ddlstate.DataTextField = "Rank"
            ddlstate.DataValueField = "RankId"
            ddlstate.DataBind()
            ddlstate.Items.Insert(0, "--Select Reward--")
        Catch ex As Exception

        End Try
    End Sub
    'Private Sub FillReport()
    '    Try

    '        Dim sql As String = String.Empty


    '        sql = " Select d.Idno,d.MemFirstName,c.rank,Replace(Convert(Varchar,b.TOdate,106),' ','-') Date "
    '        sql &= " from MstRankAchievers a "
    '        sql &= " inner join M_SessnMaster b  on a.sessid = b.sessid and todate is not null"
    '        sql &= " inner join MstRanks c on  a.rankid = c.RankId"
    '        sql &= " inner join M_MemberMaster d on  a.formno = d.formno"
    '        sql &= "   Where 1= 1"
    '        If (txtMemberID.Text <> "") Then
    '            sql &= "     and  d.Idno = '" & txtMemberID.Text & "' "
    '        End If

    '        If (ddlstate.SelectedIndex > 0) Then
    '            sql &= "      And a.sessid = " & ddlstate.SelectedValue & " "
    '        End If
    '        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '        dt = objDAL.GetData(sql)
    '        GvData.DataSource = dt
    '        GvData.DataBind()
    '    Catch ex As Exception

    '    End Try
    'End Sub


    Private Sub FillReport()
        Try

            Dim sql As String = String.Empty



            sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No.],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
            sql &= " b.Reward As Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
            sql &= " from PreLaunchBv as A left join M_PreLaunchReward As b on a.RewardId = b.RewardId"
            sql &= " left join D_SessnMaster as c on a.SessID = c.SessID "
            sql &= " Left join M_memberMaster as d on a.formno = d.formno "
            sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1  And b.Reward is not null"




            If (ddlstate.SelectedIndex > 0) Then
                sql &= "      And  b.RewardId = " & ddlstate.SelectedValue & " "
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
            Response.AddHeader("content-disposition", "attachment;filename=IncentiveDetail.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub


End Class
