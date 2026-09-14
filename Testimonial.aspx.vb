Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization


Partial Class App_UI_Application_Pages_Testimonial
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    'Dim objDAL As New DAL
    'Dim objModuleFun As ModuleFunction
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Dim ProductCodeQS As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("~\Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'FillDetail()
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

    End Sub
    Private Sub FillDetail()
        Dim sql As String = ""
        Dim condition As String = ""
        Dim dtData As DataTable
        If RbtTest.SelectedValue = "P" Then
            condition = "And IsApproved='N'"
        ElseIf RbtTest.SelectedValue = "A" Then
            condition = "And IsApproved='A'"
        ElseIf RbtTest.SelectedValue = "R" Then
            condition = "And IsApproved='R'"
        End If

        sql = "select a.AID,a.FormNo,a.Descriptions,b.IDNo,b.MemFirstname,a.ApprovedDate," & _
       " Case When a.IsApproved='N'  Then 'True' Else 'False' end As IsVisible," & _
 " Case When a.IsApproved='A' then 'Approved'  when a.IsApproved='R' then 'Rejected' else 'Pending' end as status from M_TestmonialsMaster as a,M_MemberMaster as b where a.formNo=b.FormNo " & condition
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        GvData.Visible = True


    End Sub
    Protected Sub DeleteData(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRw As GridViewRow
        Dim LblId As New Label
        Dim LblFormno As New Label
        Dim LblAID As New Label
        Dim sql As String = ""
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        LblId.Text = DirectCast(GVRw.FindControl("LblID"), Label).Text
        LblFormno.Text = DirectCast(GVRw.FindControl("LblFormNo"), Label).Text
        LblAID.Text = DirectCast(GVRw.FindControl("LblAID"), Label).Text
        sql = "insert into TempTestmonials select * ,Getdate(),'" & Session("UserID") & "' from M_TestmonialsMaster where AID='" & LblAID.Text & "'"
        sql = sql & "Delete from M_TestmonialsMaster  Where AID=" & LblAID.Text & ";"
        If objDAL.SaveData(sql) <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Testimonial Deleted  Successfully.');" & "</SCRIPT>"
            'ClearALL()
            FillDetail()
        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)
    End Sub

    Protected Sub ApproveData(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRw As GridViewRow
        Dim LblId As New Label
        Dim LblFormno As New Label
        Dim LblAID As New Label
        Dim sql As String = ""
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        LblId.Text = DirectCast(GVRw.FindControl("LblID"), Label).Text
        LblFormno.Text = DirectCast(GVRw.FindControl("LblFormNo"), Label).Text
        LblAID.Text = DirectCast(GVRw.FindControl("LblAID"), Label).Text
        sql = "insert into TempTestmonials select * ,Getdate(),'" & Session("UserID") & "' from M_TestmonialsMaster where AID='" & LblAID.Text & "'"
        sql = sql & "Update M_TestmonialsMaster Set IsApproved='A',ApprovedDate=GETDATE(),ApproveUserId='" & Session("UserID") & "' Where AID=" & LblAID.Text & ";"
        If objDAL.SaveData(sql) <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Testimonial Approved Successfully.');" & "</SCRIPT>"
            'ClearALL()
            FillDetail()
        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)
    End Sub




    Protected Sub RejectData(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRw As GridViewRow
        Dim LblId As New Label
        Dim LblFormno As New Label
        Dim LblAID As New Label
        Dim sql As String
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        LblId.Text = DirectCast(GVRw.FindControl("LblID"), Label).Text
        LblFormno.Text = DirectCast(GVRw.FindControl("LblFormNo"), Label).Text
        LblAID.Text = DirectCast(GVRw.FindControl("LblAID"), Label).Text
        sql = "insert into TempTestmonials select * ,Getdate(),'" & Session("UserID") & "' from M_TestmonialsMaster where AID='" & LblAID.Text & "'"
        sql = sql & "Update M_TestmonialsMaster Set IsApproved='R',ApprovedDate=GETDATE(),ApproveUserId='" & Session("UserID") & "' Where AID=" & LblAID.Text & ";"
        If objDAL.SaveData(sql) <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Testimonial Rejected Successfully.');" & "</SCRIPT>"
            FillDetail()
        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)
    End Sub


    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub






    'Protected Sub GvData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvData.SelectedIndexChanged

    'End Sub

    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        FillDetail()
    End Sub

 
End Class
