Imports System.Data
Imports System.Net
Imports System.IO
Imports ClosedXML.Excel
Partial Class MonthlyPayoutCommission

    Inherits System.Web.UI.Page
    'Dim obj As New DAL
    Dim Sql As String = ""
    'Dim scrname As String
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet


    Protected Sub DispatchData(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim GVRw As GridViewRow
            Dim lblslab As New Label
            Dim str As String = ""
            Dim i As Integer = 0
            Dim scrname As String

            Dim Date1 As New Label
            Dim lblSessid As New Label
            str = ""
            ' Dim Lblformno As New Labeli = objDAL.SaveData(str)

            GVRw = CType(sender.Parent.Parent, GridViewRow)
            'Date1.Text = DirectCast(GVRw.FindControl("lbluseddate"), Label).Text
            'lblslab.Text = DirectCast(GVRw.FindControl("txtslab"), TextBox).Text
            lblSessid.Text = DirectCast(GVRw.FindControl("lblSessid"), Label).Text
            'str = "Exec sp_DailySlab '" & Val(lblslab.Text) & "','" & Date1.Text & "'"
            str = "Update M_MonthSessnMaster Set OnwebSite='Y' where Sessid='" & Val(lblSessid.Text) & "';"
            i = objDAL.SaveData(str)
            If i > 0 Then

                scrname = "<SCRIPT language='javascript'>alert('Update Successfully  !! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Dispatched", scrname, False)
                Dim condition As String = ""
                FillDetail()
                'Response.Redirect("Bills.Aspx?IDNo=" & TxtIdNo.Text, False)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub DispatchPayoutData(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim GVRw As GridViewRow
            Dim lblslab As New Label
            Dim str As String = ""
            Dim i As Integer = 0
            Dim scrname As String
            Dim Date1 As New Label
     
            Dim lblSessid As New Label
            str = ""
            ' Dim Lblformno As New Labeli = objDAL.SaveData(str)

            GVRw = CType(sender.Parent.Parent, GridViewRow)
            'Date1.Text = DirectCast(GVRw.FindControl("lbluseddate"), Label).Text
            'lblslab.Text = DirectCast(GVRw.FindControl("txtslab"), TextBox).Text
            lblSessid.Text = DirectCast(GVRw.FindControl("lblSessid"), Label).Text
            objDAL = objDAL
            'str = "exec DailyPayout '" & Val(lblSessid.Text) & "';"
            str = "exec MonthlyPayout '" & Val(lblSessid.Text) & "';"
            i = objDAL.SaveData1(str)

            scrname = "<SCRIPT language='javascript'>alert(' Payout Calculate !! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Dispatched", scrname, False)
            Dim condition As String = ""
            FillDetail()
            'Response.Redirect("Bills.Aspx?IDNo=" & TxtIdNo.Text, False)
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub WebsiteData(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim GVRw As GridViewRow
            Dim lblslab As New Label
            Dim str As String = ""
            Dim i As Integer = 0
            Dim scrname As String
            Dim Date1 As New Label
            Dim lblSessid As New Label
            Dim ChkShow As New CheckBox
            str = ""
            ' Dim Lblformno As New Labeli = objDAL.SaveData(str)

            GVRw = CType(sender.Parent.Parent, GridViewRow)
            'Date1.Text = DirectCast(GVRw.FindControl("lbluseddate"), Label).Text
            'lblslab.Text = DirectCast(GVRw.FindControl("txtslab"), TextBox).Text
            lblSessid.Text = DirectCast(GVRw.FindControl("lblSessid"), Label).Text
            ChkShow = DirectCast(GVRw.FindControl("ChkShow"), CheckBox)
            objDAL = objDAL
            'str = "Update D_Sessnmaster Set OnwebSite= Case when '" & ChkShow.Checked & "'='True' then 'Y' else 'N' end where Sessid='" & Val(lblSessid.Text) & "';"
            str = "Update M_MonthSessnMaster Set OnwebSite= Case when '" & ChkShow.Checked & "'='True' then 'Y' else 'N' end where Sessid='" & Val(lblSessid.Text) & "';"
            i = objDAL.SaveData(str)

            scrname = "<SCRIPT language='javascript'>alert(' Payout Show On WebSite !! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Dispatched", scrname, False)
            Dim condition As String = ""
            FillDetail()
            'Response.Redirect("Bills.Aspx?IDNo=" & TxtIdNo.Text, False)
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then
                Session("PageName") = "Epin / Monthly payout"
                '    TxtDate.Text = Format(Date.Now, "dd-MMM-yyyy")
                FillDetail()


            End If
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub



    Private Sub FillDetail()
        Try
            Dim condition As String = ""
            'Dim Str As String = "select a.Sessid,Replace(Convert(Varchar,FrmDate,106),' ','-')as Date,Replace(Convert(Varchar,FrmDate,106),' ','-')as fromdate,isnull(Replace(Convert(Varchar,ToDate,106),' ','-'),'')as ToDate,TotalSale,Ach as [Pair Count],Fund ,Rate," & _
            '" Case when a.EndTime is Null then b.Slab else a.slab end as Commission," & _
            '                " Case when DATEADD(Day, -2, GetDate())< =Cast(FrmDate as date) then 'True' else 'False' end as Status " & _
            '                "   , Case when Cast(Frmdate as Date)=cast(Getdate()As Date) then 'False' when " & _
            '                " DATEADD(Day, -2, GetDate())< Cast(FrmDate as date) then 'True'   else 'False' end as PayoutStatus ,Case when OnwebSite='Y' then 'True' else 'False' end as CheckStatus " & _
            '                " from d_sessnmaster as a left join MstpairBudget as b on " & _
            '                " Cast(A.Frmdate as Date)=Cast(b.RectimeStamp as Date) and b.activeStatus='Y' Order by Sessid Desc"

            'Dim Str As String = "select a.Sessid,Replace(Convert(Varchar,FrmDate,106),' ','-')as Date,Replace(Convert(Varchar,FrmDate,106),' ','-')as fromdate,isnull(Replace(Convert(Varchar,ToDate,106),' ','-'),'')as ToDate," & _
            '                " Case when DATEADD(Month, -2, GetDate())< =Cast(FrmDate as date) then 'True' else 'False' end as Status, " & _
            '                "  Case when Cast(frmdate as Date)=cast(Getdate()As Date) then 'False' when  DATEADD(Month, -1, GetDate())< Cast(todate as date) then 'True' else 'False' end as PayoutStatus ,Case when OnwebSite='Y' then 'True' else 'False' end as CheckStatus " & _
            '                " from M_MonthSessnMaster as a left join MstpairBudget as b on " & _
            '                " Cast(A.Frmdate as Date)=Cast(b.RectimeStamp as Date) and b.activeStatus='Y' Order by Sessid Desc"

            Dim Str As String = "select a.Sessid,Replace(Convert(Varchar,FrmDate,106),' ','-')as Date,Replace(Convert(Varchar,FrmDate,106),' ','-')as fromdate,isnull(Replace(Convert(Varchar,ToDate,106),' ','-'),'')as ToDate," & _
                            " Case when DATEADD(Month, -2, GetDate())< =Cast(FrmDate as date) then 'True' else 'False' end as Status, " & _
                            "  Case when Cast(frmdate as Date)=cast(Getdate()As Date) then 'False' when  DATEADD(Month, -1, GetDate())< Cast(todate as date) then 'True' else 'False' end as PayoutStatus ,Case when OnwebSite='Y' then 'True' else 'False' end as CheckStatus " & _
                            ",Case when OnwebSite='Y' then 'False' when DATEADD(Month, -1, GetDate())= Cast(todate as date) then 'True' else 'False' end as CheckStatus1 from M_MonthSessnMaster as a  " & _
                            " Order by Sessid Desc"

            Dim Dt_ As New DataTable
            Dt_ = objDAL.GetData(Str)
            GvBatchMaster.DataSource = Dt_
            'GvBatchMaster.DataBind()
            GvBatchMaster.DataBind()
            Session("Stock") = Dt_

        Catch ex As Exception

        End Try
    End Sub







    Protected Sub GvBatchMaster_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvBatchMaster.PageIndexChanging
        GvBatchMaster.PageIndex = e.NewPageIndex
        GvBatchMaster.DataSource = Session("Stock")
        GvBatchMaster.DataBind()
    End Sub




    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GData1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "MonthlycommissionReport")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=MannualGPVReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub


End Class
