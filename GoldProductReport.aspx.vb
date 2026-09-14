Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class GoldProductReport
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
                'FillReport()

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
            Else
                sql = "  Select Gid,GoldName from M_GoldEnterMaster Where ActiveStatus = 'Y' Order by Gid"
            End If
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            ddlstate.DataSource = dt
            ddlstate.DataTextField = "GoldName"
            ddlstate.DataValueField = "Gid"
            ddlstate.DataBind()
            ddlstate.Items.Insert(0, "--Select Gold Product--")
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

            ElseIf Session("Compid") = "1056" Then
                If txtMemberID.Text <> "" Then
                    Idno = txtMemberID.Text.Trim
                Else
                    Idno = "0"
                End If
                sql = "exec sp_GetOrbitRankAchieverDetail '" & Idno & "','" & ddlstate.SelectedValue & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "',1,50000,'Y',1"

            Else



                If (Session("CompID") = "1007") Then

                    sql = " Select a.FormNo,d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No.],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
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
                    sql &= " from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD"
                    sql &= " left join M_MonthSessnMaster as c on a.SessID = c.SessID "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "

                ElseIf (Session("CompID") = "1059") Then
                    sql = " Select a.Formno,a.Gid,GoldIdno, GoldMemberName ,d.MemFirstName ,d.idno ,GoldName, "
                    sql &= " b.KitName, PurchaseDate ,PaidDate, "
                    sql &= " a.Remarks,a.IsPaid  from M_GoldEnterMaster as A"
                    sql &= " left join M_kitmaster As b on a.PackageName = b.kitid  "
                    sql &= " Left join M_memberMaster as d on a.formno = d.formno  "
                    sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1  "
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
                If RbReqStatus.SelectedValue <> "A" Then
                    sql &= " And a.IsPaid='" & RbReqStatus.SelectedValue & "'"

                End If
                'If (ddlstate.SelectedIndex > 0) Then
                '    sql &= "      And  a.Gid = " & ddlstate.SelectedValue & " "
                'End If
                If (txtMemberID.Text <> "") Then
                    sql &= "     And  GoldIdno = '" & txtMemberID.Text & "' "
                End If

                If txtStartDate.Text = "" Then
                    startDate = Session("CompDate")
                Else
                    sql &= "     And  Cast(PurchaseDate As Date ) >= Cast('" & txtStartDate.Text & "' As Date ) "
                End If
                If txtEndDate.Text = "" Then
                    endDate = Format(Date.Now, "dd-MMM-yyyy")
                Else
                    sql &= "     And  Cast(PurchaseDate As Date ) <= Cast('" & txtEndDate.Text & "' As Date ) "
                End If

            End If
            If (Session("CompID") = "1059") Then
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = objDAL.GetData(sql)
                GvData.DataSource = dt
                GvData.DataBind()
                btnPaid.Visible = True
                Session("GData") = dt
            Else
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = objDAL.GetData(sql)
                GvData.DataSource = dt
                GvData.DataBind()

                Session("GData") = dt
            End If

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
            Response.AddHeader("content-disposition", "attachment;filename=GoldProductReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub


    Protected Sub btnPaid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPaid.Click
        DivRemark.Visible = True
        btnPaids.Visible = True
    End Sub

    Protected Sub btnPaids_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPaids.Click
        AprvAction("Y", "A")
    End Sub
    Private Sub AprvAction(ByVal AprvType As String, ByVal ApprvStatus As String)
        Dim sms As String = ""
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim sql As String = ""
        ' Dim Chk As CheckBox
        Dim lbl As Label
        Dim lblReqno As Label
        Dim lblIdno As Label
        'Dim LblAmount As Label
        'Dim lblPaymode As Label
        'Dim LblMobileno As Label
        Dim Cnt As Integer = 0
        Dim Remark As String = ""
        Dim voucherno As String = ""
        Dim dt As New DataTable
        Dim Dt1 As New DataTable
        For Each Gvr As GridViewRow In GvData.Rows

            lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
            lblReqno = DirectCast(Gvr.FindControl("LblReqNo"), Label)
            lblIdno = DirectCast(Gvr.FindControl("LblIdNo"), Label)
 
            'If Chk.Checked = True And Chk.Enabled = True Then

            Dim Dr_ As DataRow
            Dim strSql As String = "Select Count(*) As Cnt  from M_GoldEnterMaster Where Gid = '" & lblReqno.Text & "' and IsPaid <> 'N'"
            Dt1 = New DataTable
            Dt1 = objDAL.GetData(strSql)
            If (Val(Dt1.Rows(0)("Cnt")) = 0) Then
                sql = sql & ";Update M_GoldEnterMaster SET  IsPaid = '" & AprvType & "',PaidDate=GEtdate(),GoldDesign='" & Val(Session("UserID")) & "',Remarks='" & TxtARemark.Text & "' where FormNo='" & lbl.Text & "' And Gid='" & lblReqno.Text & "' "
                Cnt = Cnt + 1

            End If

            'End If
        Next
        Dim a As Integer
        If sql <> "" Then
            a = objDAL.UpdateData(sql)
        End If

        Dim MsgTxt As String = ""
        If AprvType = "Y" Then
            MsgTxt = "Paid"
        Else : MsgTxt = "Rejected"
        End If
        If a <> 0 And Cnt > 0 Then
          
            lblMsg.Text = "" & Cnt & " Requests " & MsgTxt & " Successfully."
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Green
            TxtARemark.Text = ""
            DivRemark.Visible = False
            btnPaid.Enabled = False
            FillReport()
        Else
            lblMsg.Text = " Request Already  " & MsgTxt
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Red
        End If
    End Sub
End Class
