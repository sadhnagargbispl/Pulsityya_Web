Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class TopUpDetailVerify
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then
                txtMemId.Text = ""
                GvData.Visible = False
                btnExport.Enabled = False
                Session("PageName") = "Member / Product Request Approve"

            End If
        End If
    End Sub

   
    Private Sub FillDetail()
        If CheckBox1.Checked = True Then
            Condition = Condition & " And a.Fcode='" & txtMemId.Text.Trim & "'"
        End If
      
        sql = "select d.Formno,d.OrderNo,b.Idno,(b.MemFirstName+ ' '+b.MemLastname) as [MemName],ProductName as PackageName,PayMode,b.Address1 as DeliveryAddress, " & _
              " Case when b.PId<>1 then b.ChDDNo else '' end as [ChDDNo], Case when b.Pid<>1 then Replace(Convert(Varchar,b.ChDDDate,106),' ' ,'-') else '' end as  " & _
              " [ChDDDate], Case when b.Pid<>1 then b.ChDDBank else '' end as BankName,Case when b.Pid<>1 then b.ChDDbranch else '' end as BranchName  " & _
              " , Replace(Convert(Varchar,a.RecTimeStamp,106),' ','-') + ' '+  CONVERT(varchar(15), CAST(a.RecTimeStamp AS TIME),100) as DepositDate,d.Amount,b.PlanId, " & _
              " Case when d.IsApprove='A' then 'Approve' when IsApprove='N' then 'Pending' else 'Rejected' end as Status, " & _
              " Case When d.IsApprove='A' Or d.IsApprove='R' Then 'False' Else 'True' end As IsVisible,Case When d.IsApprove='A' Then 'True' Else 'False' end As IsShow," & _
              " Case When d.IsApprove='R' Then 'True' Else 'False' end As IsReject,x.KitAmount,a.Qty , " & _
              " ApproveRemark as Remark, Case when d.IsApprove='N' then '' else Replace(Convert(Varchar,d.ApproveDate,106),' ','-') + ' '+ CONVERT(varchar(15), CAST(d.ApproveDate AS TIME),100)" & _
              "  end as ApproveDate  from TrnkitProducts as a,M_MemberMaster as b  ,TrnTopUpPayMode as d ,M_KitMaster as x WHERE a.UserId=d.Formno and a.OrderNo=d.OrderNo " & _
             " and a.KitId=x.KitId and x.RowStatus='Y' and  a.Fcode=b.Idno  " & Condition & "   and Qty<>0  Order by a.RecTimeStamp Desc "
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData


        GvData.Visible = True
        If dtData.Rows.Count > 0 Then

            btnExport.Enabled = True
        Else
            btnExport.Enabled = False

        End If
    End Sub
    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click


        FillDetail()
    End Sub


    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid

            If CheckBox1.Checked = True Then
                Condition = Condition & " And a.FCode='" & txtMemId.Text.Trim & "'"
            End If

            sql = "select b.Idno,(b.MemFirstName+ ' '+b.MemLastname) as [Member Name],a.ProductName as PackageName,x.KitAmount as [Package Amount],PayMode as PaymentMode," & _
                " a.Qty as Quantity,d.Amount as TotalAmount," & _
      " Case when d.IsApprove='A' then 'Approve' when IsApprove='N' then 'Pending' else 'Rejected' end as Status," & _
              " ApproveRemark as Remark, Case when d.IsApprove='N' then '' else Replace(Convert(Varchar,d.ApproveDate,106),' ','-') + ' '+ CONVERT(varchar(15), CAST(d.ApproveDate AS TIME),100) end as ApproveDate" & _
            "   from TrnkitProducts as a,M_MemberMaster as b  ,TrnTopUpPayMode as d,M_KitMaster as x " & _
            " WHERE a.UserId=d.Formno and a.OrderNo=d.OrderNo  and  a.Fcode=b.Idno  and a.kitId=x.KitId and x.RowStatus='Y' and Qty>0 " & Condition & " Order by a.RecTimeStamp Desc"
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("TopUpDetailVerify.xls", dg)

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
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
    Protected Sub RejectData(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        TxtIdno.Text = DirectCast(GVRw.FindControl("LblID"), Label).Text
        LblOrderNo.Text = DirectCast(GVRw.FindControl("LblOrderNo"), Label).Text
        LblMemId.Text = DirectCast(GVRw.FindControl("LblFormNo"), Label).Text

        DivRemark.Visible = True
        btnApprove.Visible = False
        BtnReject.Visible = True


        '  Else
        ' scrname = "<SCRIPT language='javascript'>alert('Withdrawl Rejected UnSuccessfully.');" & "</SCRIPT>"
        ' Me.RegisterStartupScript("MyAlert", scrname)

        ' End If
    End Sub
    
    Protected Sub ApproveData(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRw As GridViewRow
        ' Dim Lblformno As New Label
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        TxtIdno.Text = DirectCast(GVRw.FindControl("LblID"), Label).Text
        LblOrderNo.Text = DirectCast(GVRw.FindControl("LblOrderNo"), Label).Text
        LblMemId.Text = DirectCast(GVRw.FindControl("LblFormNo"), Label).Text
        LblPlanId.Text = DirectCast(GVRw.FindControl("LblPlanId"), Label).Text

        DivRemark.Visible = True
        btnApprove.Visible = True
        BtnReject.Visible = False
    End Sub

    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
        
        Dim updateeffect As Integer
        Dim LblId As New Label
        Dim Sql As String = "Update TrnTopupPaymode Set IsApprove='A',ApproveDate=GETDATE(),ApproveRemark='" & TxtARemark.Text & "' Where Formno=" & Val(LblMemId.Text) & " and OrderNo='" & Val(LblOrderNo.Text) & "';"
        updateeffect = objDAL.SaveData(Sql)
        Dim strq As String = ""
        Dim i As Integer = 0
        strq = " Exec  Sp_ActivateMember " & (TxtIdno.Text).Trim & "," & Val(LblPlanId.Text) & "," & Val(LblOrderNo.Text) & " "
        i = objDAL.SaveData(strq)
        If updateeffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('TopUp Request Approved Successfully.');" & "</SCRIPT>"

        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)

        ClearAll()
        FillDetail()
        DivRemark.Visible = False
    End Sub
    Private Sub ClearAll()
        TxtARemark.Text = ""
        LblOrderNo.Text = ""

    End Sub

    Protected Sub BtnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReject.Click

        Dim Sql As String = "Update TrnTopUpPayMode Set IsApprove='R',ApproveDate=GETDATE(),ApproveRemark='" & TxtARemark.Text & "' Where Formno=" & Val(LblMemId.Text) & " and OrderNo=" & Val(LblOrderNo.Text) & ""
        'comment on 8Oct2016           
        'Dim sql As String = "Update FundWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='Rejected by Administrator' Where ReqID=" & Id
        If objDAL.SaveData(Sql) <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Topup Request Rejected Successfully.');" & "</SCRIPT>"
            ClearAll()
            FillDetail()
            DivRemark.Visible = False
        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)
    End Sub
End Class
