

Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class CommunityReEntry
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
                FillReport()
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


            'Dim startDate As Date
            'Dim endDate As Date

            If RbReqStatus.SelectedValue = "M" Then
                sql = " Select d.Idno as MemberIDNo,d.MemFirstName as MemberName,d.mobl,g.statename,count(a.Rn) as Count,Replace(convert(Varchar,Min(a.RectimeStamp),106),' ','-')  as Joindate   "
                sql &= " from PoolData as A Inner join M_memberMaster as d on a.formno = d.formno   "
                sql &= " left join  M_StateDivMaster as g on d.statecode=g.statecode "
                sql &= " Where 1=1 "
                GvData.Columns(5).Visible = True
            Else
                sql = " Select d.Idno as MemberIDNo,d.MemFirstName as MemberName,d.mobl,g.statename,count(1) as Count,Replace(convert(Varchar,Min(a.RectimeStamp),106),' ','-')  as Joindate   "
                sql &= " from repurchincomecommunity as A Inner join M_memberMaster as d on a.formno = d.formno   "
                sql &= " left join  M_StateDivMaster as g on d.statecode=g.statecode "
                sql &= " Where 1=1 "
                GvData.Columns(5).Visible = False
            End If


            'If txtStartDate.Text <> "" Then
            '    startDate = txtStartDate.Text
            'Else
            '    startDate = Format(Date.Now, "dd-MMM-yyyy")
            'End If
            'If txtEndDate.Text <> "" Then
            '    endDate = txtEndDate.Text
            'Else
            '    endDate = Format(Date.Now, "dd-MMM-yyyy")
            'End If
            'If txtMemberID.Text <> "" Then
            '    Idno = txtMemberID.Text.Trim
            'Else
            '    Idno = "0"
            'End If
            'sql = " EXEC Sp_GetCommunityReEntry '" & Idno & "','" & startDate & "','" & endDate & "' "

            'sql = " Select d.Idno as MemberIDNo,d.MemFirstName as MemberName,d.mobl,g.statename,count(a.Rn) as Count,Replace(convert(Varchar,Min(a.RectimeStamp),106),' ','-')  as Joindate   " & _
            '" from PoolData as A Inner join M_memberMaster as d on a.formno = d.formno   "
            'sql &= " left join  M_StateDivMaster as g on d.statecode=g.statecode "
            'sql &= " Where 1=1 "






            '            Where(1 = 1)
            'and d.Idno=Case when @IDNO='0' then d.Idno else @IDNO end          
            ' group by a.formno,d.Idno,d.MemFirstName,d.mobl,g.statename    
            '            Having()
            ' Cast(Convert(varchar,Min(a.RectimeStamp),106) as DateTime)>=Case when @StartDate='' then '12-Feb-2018'else @Startdate end  And                     
            ' Cast(Convert(varchar,Min(a.RectimeStamp),106) as DateTime)<=Case when @Todate='' then                         
            ' Cast(Replace(Convert(Varchar,GetDate(),106),'','-')as Date)else @Todate end                      
            ' ORDER BY CASE WHEN d.MemFirstName like '%DISCOUNT%' THEN 1      
            '           ELSE 4 END 

            Dim startDate As Date
            Dim endDate As Date

            sql &= " group by a.formno,d.Idno,d.MemFirstName,d.mobl,g.statename "


            sql &= " Having "
            If txtStartDate.Text <> "" Then
                sql &= "       Cast(Convert(varchar,Min(a.RectimeStamp),106) as DateTime) >= Cast('" & txtStartDate.Text & "' As Date ) "
            End If
            If txtEndDate.Text <> "" Then
                sql &= "     And  Cast(Convert(varchar,Min(a.RectimeStamp),106) as DateTime) <= Cast('" & txtEndDate.Text & "' As Date ) "
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
            Response.AddHeader("content-disposition", "attachment;filename=CommunityReEntryPayoutReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub


End Class
