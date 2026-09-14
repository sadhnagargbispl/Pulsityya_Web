Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class UserComplaints
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" And Session("UserPermission") = 1 Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            'GvData.PageIndex = Session("index")
            txtSearch.Text = ""
            btnShowRecord.Visible = False
            FillUser()

            If Session("compid") = "1007" Then
                Divstate.Visible = True
                FillState()
            Else
                Divstate.Visible = False
            End If

            If Session("AStatus") = "OK" Then
                BindData()
            End If
        End If
    End Sub
    Private Sub FillState()
        ' Dim sql As String = "Select * from (select 0 as GroupId ,'All'  as GroupName Union All GroupId,GroupName From " + objDAL.tblUserGrpMaster + " Where " + objDAL.activeCondition + "  Order by GroupName"
        Dim sql As String = "select distinct stateCode,StateName from M_StateDivMaster where activestatus = 'Y' and RowStatus='Y'"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        If dtData.Rows.Count > 0 Then
            DDlState.DataSource = dtData
            DDlState.DataTextField = "StateName"
            DDlState.DataValueField = "stateCode"
            DDlState.DataBind()
        End If
    End Sub
    Public Sub BindData(Optional ByVal Condition As String = "")
        Dim Conditions As String = ""
        If DDlGroup.SelectedValue > 0 Then
            Conditions = Conditions & "and U.UserId='" & DDlGroup.SelectedValue & "'"
        End If
        Dim sql As String
        If Session("CompID") = 1074 Then
            sql = "  Select '' as StateName,M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName, "
            sql &= " ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint , "
            sql &= " ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM"
            sql &= "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status "
            sql &= "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S"
            sql &= "  ON M.CID=S.CID,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' "
            sql &= " and c.RowStatus='Y' and U.ActiveStatus='Y' and u.RowStatus='Y' " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
        ElseIf Session("CompID") = 1007 Then
            sql = "  Select DS.StateName,M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName, "
            sql &= " ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint , "
            sql &= " ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM"
            sql &= "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status,b.StateCode "
            sql &= "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo ) as M LEFT JOIN M_SolutionMaster as S"
            sql &= "  ON M.CID=S.CID,M_ComplaintUserMaster as t,M_UserMaster as U,m_ComplaintTypeMaster as c,M_StateDivMaster as DS WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' "
            sql &= " and c.RowStatus='Y' and c.Ctypeid=t.CtypeId and t.ActiveStatus='Y' and t.RowStatus='Y' and t.UserId=U.Userid and U.ActiveStatus='Y' "
            sql &= "and u.RowStatus='Y' AND DS.activestatus = 'Y' and DS.RowStatus='Y' AND M.StateCode = DS.statecode " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
        Else
            sql = "  Select '' as StateName,M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName, "
            sql &= " ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint , "
            sql &= " ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM"
            sql &= "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status "
            sql &= "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S"
            sql &= "  ON M.CID=S.CID,M_ComplaintUserMaster as t,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' "
            sql &= " and c.RowStatus='Y' and c.Ctypeid=t.CtypeId and t.ActiveStatus='Y' and t.RowStatus='Y' and t.UserId=U.Userid and U.ActiveStatus='Y' "
            sql &= "and u.RowStatus='Y' " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
        End If
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
        ' Session("index") = GvCat.PageIndex
    End Sub
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then
            If String.Equals(e.Row.Cells(3).Text.ToLower(), "deactive") = True Then
                'e.Row.BackColor = Drawing.Color.Red
                e.Row.Style("background-image") = "../Resources/Images/redback2.jpg"
                For i = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style("color") = "whitesmoke"
                Next
            End If
        End If
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        
        Response.ClearContent()
        Response.Buffer = True
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "Complaints.xls"))
        Response.ContentType = "application/ms-excel"
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
        'BindGridview()
        'Change the Header Row back to white color
        GvData.HeaderRow.Style.Add("background-color", "#FFFFFF")
        'Applying stlye to gridview header cells
        For i As Integer = 0 To GvData.HeaderRow.Cells.Count - 1
            GvData.HeaderRow.Cells(i).Style.Add("background-color", "#D8A38D")
        Next

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        ' GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            '   GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        Next

        For i As Integer = 0 To GvData.Rows.Count - 1
            For j As Integer = 0 To GvData.Rows(i).Cells.Count - 1
                GvData.Rows(i).Cells(j).Style.Add("background-color", "#FFFFFF")
                GvData.Rows(i).Cells(j).Style.Add("color", "#000000")
            Next
        Next
        GvData.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.[End]()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
    Private Sub FillUser()
        ' Dim sql As String = "Select * from (select 0 as GroupId ,'All'  as GroupName Union All GroupId,GroupName From " + objDAL.tblUserGrpMaster + " Where " + objDAL.activeCondition + "  Order by GroupName"
        Dim sql As String = "Select * From (Select 0 As GroupId,'-- All  --' As GroupName Union ALL Select UserId,UserName From M_UserMaster Where  ActiveStatus='Y'  And RowStatus='Y' ) As Temp Order By GroupId"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        If dtData.Rows.Count > 0 Then
            DDlGroup.DataSource = dtData
            DDlGroup.DataTextField = "GroupName"
            DDlGroup.DataValueField = "GroupID"
            DDlGroup.DataBind()

        End If
        'If Session("GroupId") <> "1" Then
        DDlGroup.SelectedValue = Session("Userid")
        DDlGroup.Enabled = False
        DDlGroup.Visible = False
        LblGroup.Visible = False
        'Else
        ' DDlGroup.Enabled = True
        'DDlGroup.Visible = True
        'LblGroup.Visible = True

        'End If
    End Sub
    Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
        BindData()
        btnShowRecord.Visible = False
        ddlGroupFields.SelectedIndex = 0
        txtSearch.Text = ""
    End Sub
    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click
        Dim Condition_ As String = ""
        If ddlGroupFields.SelectedValue.ToString <> "None" Then
            If txtSearch.Text <> "" Then
                Condition_ = " AND " + ddlGroupFields.SelectedValue.ToString() + " like '%" + txtSearch.Text + "%'"
            End If
        End If
        If RbReplied.SelectedValue <> "K" Then
            Condition_ = Condition_ & " AND M.IsReplied='" & Trim(RbReplied.SelectedValue) & "' "
        Else
            Condition_ = Condition_
        End If
        Dim scrname As String = ""
        If TxtFromDate.Text <> "" Then
            If TxtFromDate.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Enter From Date  ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Else
                Condition_ = Condition_ & " AND CAST(Convert(varchar,M.RecTimeStamp,106) as DateTime)>='" & Trim(TxtFromDate.Text) & "'"

            End If
        End If
        If TxtToDate.Text <> "" Then
            If TxtToDate.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Enter To Date  ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Else
                Condition_ = Condition_ & "AND CAST(Convert(varchar,M.RecTimeStamp,106) as DateTime)<='" & Trim(TxtToDate.Text) & "'"
            End If
        End If
        


        If RbtStatus.SelectedValue <> "K" Then
            Condition_ = Condition_ & "And M.ComplaintStatus='" & RbtStatus.SelectedValue & "'"
        Else
            Condition_ = Condition_
        End If
        If Session("compid") = "1007" Then
            If DDlState.SelectedValue <> 0 Then
                Condition_ = Condition_ & "and DS.StateCode = '" & DDlState.SelectedValue & "'"
            Else
                Condition_ = Condition_
            End If
        End If
        BindData(Condition_)
    End Sub
    ' Add 16 feb 2022
    Protected Sub gvdatanew_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvdatanew.PageIndexChanging
        gvdatanew.PageIndex = e.NewPageIndex
        gvdatanew.DataSource = Session("GData")
        gvdatanew.DataBind()
        ' Session("index") = GvCat.PageIndex
    End Sub

    Protected Sub gvdatanew_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvdatanew.RowDataBound
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then
            If String.Equals(e.Row.Cells(3).Text.ToLower(), "deactive") = True Then
                'e.Row.BackColor = Drawing.Color.Red
                e.Row.Style("background-image") = "../Resources/Images/redback2.jpg"
                For i = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style("color") = "whitesmoke"
                Next
            End If
        End If
    End Sub
End Class
