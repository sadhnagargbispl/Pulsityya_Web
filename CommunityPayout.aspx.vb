
Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class CommunityPayout
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
            End If


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



            If txtMemberID.Text <> "" Then
                Idno = txtMemberID.Text.Trim
            Else
                Idno = "0"
            End If

               If Session("compid") = "1068" Then
                sql = " Select IsNull(REPLACE(CONVERT(VARCHAR(11), PayDt , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,PayDt ,100 ) ,7), 6, 0, ' '),'') as [Payout Date], d.Idno as [Member IDNo],d.MemFirstName [Member Name],"
                sql &= "a.comm as [Commission.], TdsAmount,AdminCharge,NetIncome "
                sql &= " from MstPoolIncomeNew as A  "
                sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                sql &= " Where 1=1 "
            Else
                sql = " Select IsNull(REPLACE(CONVERT(VARCHAR(11), PayDt , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,PayDt ,100 ) ,7), 6, 0, ' '),'') as [Payout Date], d.Idno as [Member IDNo],d.MemFirstName [Member Name],"
                sql &= "a.comm as [Commission.], TdsAmount,AdminCharge,NetIncome "
                sql &= " from MstPoolIncome as A  "
                sql &= " Left join M_memberMaster as d on a.formno = d.formno "
                sql &= " Where 1=1 "
            End If

            Dim startDate As Date
            Dim endDate As Date


            If (txtMemberID.Text <> "") Then
                sql &= "     And  d.Idno = '" & txtMemberID.Text & "' "
            End If

            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                sql &= "     And  Cast(PayDt As Date ) >= Cast('" & txtStartDate.Text & "' As Date ) "
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                sql &= "     And  Cast(PayDt As Date ) <= Cast('" & txtEndDate.Text & "' As Date ) "
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
        
        FillReport()
         End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
       
        FillReport()

    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            FillReport()
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
            Response.AddHeader("content-disposition", "attachment;filename=CommunityPayoutReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub


End Class
