Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class SeminarReport
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

  


    Private Sub FillCityPinDetail()
        Try

            Dim sql As String = String.Empty

            sql = " Select MeetingID,Program from M_MeetingMaster Where ActiveStatus = 'Y' Order By  Program "
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            ddlstate.DataSource = dt
            ddlstate.DataTextField = "Program"
            ddlstate.DataValueField = "MeetingID"
            ddlstate.DataBind()
            ddlstate.Items.Insert(0, "--Select Seminar--")
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
            sql = " select c.Idno As [Req. by  Idno],(c.MemFirstName+ ' '+c.MemlastName) as [Req. by  MemberName] ,Mobl as Mobileno,a.ReqNo,"
            sql &= " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,"
            sql &= " E.Program,isnull(f.groupname,'') as [Group Name],d.Idno As [Member Idno],d.NAme [Member Name],d.Mobile,d.Email,d.City,d.PassCode,Isnull(Gr.Rank,'') As Rank,ISnull(GS,0) As [Gross Income]"
            sql &= " from SeminarReq as a  with(nolock) left join M_SeminarGroupMaster as f on a.groupid=f.GroupId,"
            sql &= " M_MeetingMaster As E with(nolock),"
            sql &= " M_BankMaster as b  with(nolock), "
            sql &= " M_MemberMaster as c  with(nolock),"
            sql &= " TrnSemimarMember as d with(nolock)"
            sql &= " Left Join V#GetMaxRankSemiNAr As GR On REPLACE(d.idno,'VI','') = Convert(Varchar,GR.formno ) "
            sql &= " Left Join V#GetTotalGrossIncome As GI On REPLACE(d.idno,'VI','') = Convert(Varchar,GI.formno ) "
            sql &= " where a.Formno=c.Formno"
            sql &= " And  a.ReqNo=d.ReqNo"
            sql &= "  And E.MeetingID =  a.SemiNarID "
            sql &= " and  a.BankId=b.BankCode "
            sql &= " and b.RowStatus='Y'  And a.IsApprove = 'Y'   ANd E.ActiveStatus = 'Y' "




            Dim startDate As Date
            Dim endDate As Date

            If (ddlstate.SelectedIndex > 0) Then
                sql &= "      And  E.MeetingID = " & ddlstate.SelectedValue & " "
            End If
            If (txtMemberID.Text <> "") Then
                sql &= "     And  c.Idno = '" & txtMemberID.Text & "' "
            End If

            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                sql &= "     And  Cast(a.RectimeStamp As Date ) >= Cast('" & txtStartDate.Text & "' As Date ) "
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                sql &= "     And  Cast(a.RectimeStamp As Date ) <= Cast('" & txtEndDate.Text & "' As Date ) "
            End If
            sql &= " order by a.ReqNo "


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
            Response.AddHeader("content-disposition", "attachment;filename=SeminarReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub


End Class
