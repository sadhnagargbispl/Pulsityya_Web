Imports System.Data
Imports System.Data.SqlClient
Partial Class RewardPayReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim conn As New SqlConnection
    Dim objGen As clsGeneral = New clsGeneral


   


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillData()
    End Sub
    Protected Sub FillData()
        conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        conn.Open()
        Dim formno As String = ""

        Dim condition1 As String = ""
        Dim condition2 As String = ""
        Dim condition As String = ""
        If Chkmemid.Checked Then
            formno = GetFormNo()
            condition = condition & " And b.Formno='" & Val(formno) & "'"
        End If
        If txtFromDate.Text <> "" Then
            condition1 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)>='" & txtFromDate.Text & "'"
        End If
        If TxtToDate.Text <> "" Then
            condition2 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)<='" & TxtToDate.Text & "'"
        End If
        Dim qry1 As String = ""
        qry1 = "select a.Idno,B.Formno,a.MemFirstname as MemberName,b.Reward as Rewardname,b.Rewardid,b.Istransfer," & _
         " Replace(Convert(Varchar,b.RectimeStamp,106),' ','-') as  AchiveDate,Case when b.Istransfer='Y'" & _
         " then 'False' else 'True'" & _
         " end as Transferstatus,Replace(Convert(Varchar,b.RedeemDate,106),' ','-') as RedeemDate  ,Case when isTransfer='Y'" & _
        " then 'Transfered' else '' end as status" & _
 " from M_Membermaster as a,M_RewardFinal as b where b.IsRedeem='Y' and  a.Formno=b.Formno " & condition & " " & _
" " & condition1 & " " & condition2 & " "
        dtData = New DataTable
        dtData = objDAL.GetData(qry1)

        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub
    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMemId.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMemId.Text = ""
        End If
        Return formno
    End Function


    Protected Sub Paybtn(ByVal sender As Object, ByVal e As EventArgs)
        conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        conn.Open()
        Dim formno As String = ""
        Dim GridvwRow As GridViewRow
        Dim R As String = ""
        Dim Rewardname As String = ""
        Dim Idno As String = ""
        GridvwRow = DirectCast(sender.parent.parent, GridViewRow)
        Dim lblReward As Label = DirectCast(GridvwRow.FindControl("lblreward"), Label)
        Dim lblformno As Label = DirectCast(GridvwRow.FindControl("lblform"), Label)
        Rewardname = DirectCast(GridvwRow.FindControl("LblRewardName"), Label).Text
        Idno = DirectCast(GridvwRow.FindControl("LblIdno"), Label).Text
        Dim Remark As String = ""
        Remark = Rewardname & " Transfer To Idno:" & Idno
        Dim dl As String = "Update M_RewardFinal set IsTransfer='Y',Transferdate=Getdate(),UserId='" & Val(Session("UserID")) & "'  where formno='" & Val(lblformno.Text) & "' and Rewardid='" & Val(lblReward.Text) & "'"
        dl = dl & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
        "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Reward Pay Report','Reward Transfer','" & Remark & "',Getdate(),'" & lblformno.Text & "')"
        Dim updateeffect As Integer = 0
        Dim scrname As String = ""
        Dim comm As New SqlCommand(dl, conn)
        updateeffect = comm.ExecuteNonQuery()
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Reward Pay Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Reward Pay Unsuccessful! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Reward Pay", scrname, False)
        FillData()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then

            Else
                Response.Redirect("logout.aspx")
            End If
        End If
    End Sub

   
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            '     Dim condition As String = ""
            Dim condition1 As String = ""
            Dim condition2 As String = ""
            If Chkmemid.Checked Then
                formno = GetFormNo()
                Condition = Condition & " And b.Formno='" & Val(formno) & "'"
            End If
            If txtFromDate.Text <> "" Then
                condition1 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)>='" & txtFromDate.Text & "'"
            End If
            If TxtToDate.Text <> "" Then
                condition2 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)<='" & TxtToDate.Text & "'"
            End If
            Dim qry1 As String = ""
            qry1 = "select a.Idno,B.Formno,a.MemFirstname as MemberName,b.Reward as Rewardname,b.Rewardid,b.Istransfer," & _
             " Replace(Convert(Varchar,b.RectimeStamp,106),' ','-') as  AchiveDate,Case when b.Istransfer='Y'" & _
             " then 'False' else 'True'" & _
             " end as Transferstatus,Replace(Convert(Varchar,b.RedeemDate,106),' ','-') as RedeemDate  ,Case when isTransfer='Y'" & _
            " then 'Transfered' else '' end as status" & _
     " from M_Membermaster as a,M_RewardFinal as b where b.IsRedeem='Y' and  a.Formno=b.Formno " & condition & " " & _
    " " & condition1 & " " & condition2 & " "
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()
            ExportToExcel("RewardPayReport.xls", dg)

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

   

End Class
