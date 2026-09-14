Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class App_UI_Application_Pages_Complaints
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
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            'GvData.PageIndex = Session("index")
            txtSearch.Text = ""
            txtnewsearch.Text = ""
            btnShowRecord.Visible = False
            btnShowRecord1.Visible = False
            FillUser()
            FillUser1()
            If (Session("CompID") = 1056) Then
                uddanview.Visible = True
                BindData()
                Allview.Visible = False

            Else
                uddanview.Visible = False
                Allview.Visible = True
                BindData()


            End If
            If Session("AStatus") = "OK" Then
                BindData()
            End If
        End If
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")
        
        If (Session("CompID") = 1056) Then
            Dim Conditions1 As String = ""
            ' If Session("GroupId") <> "1" Then
            If DDlGroup1.SelectedValue > 0 Then
                Conditions1 = Conditions1 & "and U.UserId='" & DDlGroup1.SelectedValue & "'"

            End If

            'Else
            'Conditions = Conditions & "and U.GroupId='" & DDlGroup.SelectedValue & "'"
            'End If

            Dim sql1 As String = "  Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM" & _
    "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status " & _
    "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S" & _
    "  ON M.CID=S.CID,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' " & _
    " and c.RowStatus='Y' and c.ToUserId=U.Userid and U.ActiveStatus='Y' and u.RowStatus='Y' " & Conditions1 & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            dtData = New DataTable
            dtData = objDAL.GetData(sql1)
            GridView1.DataSource = dtData
            GridView1.DataBind()
            Session("GData") = dtData
            If dtData.Rows.Count > 0 Then
                btnExport1.Enabled = True
                btnPrintAll1.Enabled = True
                btnPrintCurrent1.Enabled = True
            Else
                btnExport1.Enabled = False
                btnPrintAll1.Enabled = False
                btnPrintCurrent1.Enabled = False
            End If
        ElseIf (Session("CompID") = 1057) Then
            Dim Conditions As String = ""
            ' If Session("GroupId") <> "1" Then
            If DDlGroup.SelectedValue > 0 Then
                Conditions = Conditions & "and U.UserId='" & DDlGroup.SelectedValue & "'"

            End If

            'Else
            'Conditions = Conditions & "and U.GroupId='" & DDlGroup.SelectedValue & "'"
            'End If

            Dim sql1 As String = "  Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,m.email,m.mobl,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM" & _
    "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,b.mobl,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status " & _
    "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S" & _
    "  ON M.CID=S.CID,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' " & _
    " and c.RowStatus='Y' and c.ToUserId=U.Userid and U.ActiveStatus='Y' and u.RowStatus='Y' " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            dtData = New DataTable
            dtData = objDAL.GetData(sql1)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            If dtData.Rows.Count > 0 Then
                btnExport1.Enabled = True
                btnPrintAll1.Enabled = True
                btnPrintCurrent1.Enabled = True
            Else
                btnExport1.Enabled = False
                btnPrintAll1.Enabled = False
                btnPrintCurrent1.Enabled = False
            End If
        ElseIf (Session("CompID") = 1068) Then
            Dim Conditions As String = ""
            ' If Session("GroupId") <> "1" Then
            If DDlGroup.SelectedValue > 0 Then
                Conditions = Conditions & "and U.UserId='" & DDlGroup.SelectedValue & "'"

            End If

            'Else
            'Conditions = Conditions & "and U.GroupId='" & DDlGroup.SelectedValue & "'"
            'End If

            Dim sql1 As String = "  Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,m.email,m.mobl,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM" & _
    "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,b.mobl,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status " & _
    "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S" & _
    "  ON M.CID=S.CID,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' " & _
    " and c.RowStatus='Y' and c.ToUserId=U.Userid and U.ActiveStatus='Y' and u.RowStatus='Y' " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            dtData = New DataTable
            dtData = objDAL.GetData(sql1)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            If dtData.Rows.Count > 0 Then
                btnExport1.Enabled = True
                btnPrintAll1.Enabled = True
                btnPrintCurrent1.Enabled = True
            Else
                btnExport1.Enabled = False
                btnPrintAll1.Enabled = False
                btnPrintCurrent1.Enabled = False
            End If
        Else
            Dim Conditions As String = ""
            ' If Session("GroupId") <> "1" Then
            If DDlGroup.SelectedValue > 0 Then
                Conditions = Conditions & "and U.UserId='" & DDlGroup.SelectedValue & "'"

            End If

            'Else
            'Conditions = Conditions & "and U.GroupId='" & DDlGroup.SelectedValue & "'"
            'End If
            Dim sql As String = ""
            If Session("compid") = 1072 Then
                sql = "  Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,m.mobl,m.email,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM" & _
  "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,b.mobl,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status " & _
  "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S" & _
  "  ON M.CID=S.CID,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' " & _
  " and c.RowStatus='Y' and c.Userid=U.Userid and U.ActiveStatus='Y' and u.RowStatus='Y' " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            ElseIf Session("compid") = 1073 Then
                sql = "  Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,m.mobl,m.email,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM" & _
  "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,b.mobl,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status " & _
  "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S" & _
  "  ON M.CID=S.CID,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' " & _
  " and c.RowStatus='Y' and c.CTypeID=U.Userid and U.ActiveStatus='Y' and u.RowStatus='Y' " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            ElseIf Session("compid") = 1077 Then
                sql = "  Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,m.mobl,m.email,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM" & _
  "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,b.mobl,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status " & _
  "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S" & _
  "  ON M.CID=S.CID,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' " & _
  " and c.RowStatus='Y' and c.Userid=U.Userid and U.ActiveStatus='Y' and u.RowStatus='Y' " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            ElseIf Session("compid") = 1078 Then
                sql = "  Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,m.mobl,m.email,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM" & _
  "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,b.mobl,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status " & _
  "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S" & _
  "  ON M.CID=S.CID,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' " & _
  " and c.RowStatus='Y' and c.Userid=U.Userid and U.ActiveStatus='Y' and u.RowStatus='Y' " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            ElseIf Session("compid") = 1093 Then
                sql = "  Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,m.mobl,m.email,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM" & _
  "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,b.mobl,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status " & _
  "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S" & _
  "  ON M.CID=S.CID,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' " & _
  " and c.RowStatus='Y' and c.Userid=U.Userid and U.ActiveStatus='Y' and u.RowStatus='Y' " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"

            Else
                sql = "  Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,m.mobl,m.email,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM" & _
    "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,b.mobl,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status " & _
    "  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN M_SolutionMaster as S" & _
    "  ON M.CID=S.CID,M_UserMaster as U,m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId and c.ActiveStatus='Y' " & _
    " and c.RowStatus='Y' and c.ToUserId=U.Userid and U.ActiveStatus='Y' and u.RowStatus='Y' " & Conditions & "   " & Condition & " ORDER BY M.RecTimeStamp DESC"
            End If
            
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            If dtData.Rows.Count > 0 Then
                btnExport.Enabled = True
                btnPrintAll.Enabled = True
                btnPrintCurrent.Enabled = True
            Else
                btnExport.Enabled = False
                btnPrintAll.Enabled = False
                btnPrintCurrent.Enabled = False
            End If
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
        ' Session("index") = GvCat.PageIndex
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataSource = Session("GData")
        GridView1.DataBind()
        ' Session("index") = GvCat.PageIndex
    End Sub

    'Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
    '    Dim Condition As String

    '    If String.IsNullOrEmpty(txtSearch.Text) Or ddlGroupFields.SelectedValue = "None" Then
    '    ElseIf String.Equals(ddlGroupFields.SelectedItem.Text.ToLower(), "showall") = True Then
    '        BindData()
    '    Else
    '        Condition = " AND " + ddlGroupFields.SelectedValue.ToString() + " like '%" + txtSearch.Text + "%'"
    '        BindData(Condition)
    '    End If
    'End Sub

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
    Protected Sub Gridview1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
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
    Protected Sub btnExport1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport1.Click
        Response.ClearContent()
        Response.Buffer = True
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "Complaints.xls"))
        Response.ContentType = "application/ms-excel"
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        GridView1.AllowPaging = False
        GridView1.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GridView1.DataSource = dtData
        GridView1.DataBind()
        'BindGridview()
        'Change the Header Row back to white color
        GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF")
        'Applying stlye to gridview header cells
        For i As Integer = 0 To GridView1.HeaderRow.Cells.Count - 1
            GridView1.HeaderRow.Cells(i).Style.Add("background-color", "#D8A38D")
        Next

        'Remove modify and Delete columns from grid
        GridView1.HeaderRow.Cells(GridView1.HeaderRow.Cells.Count - 1).Visible = False
        ' GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GridView1.Rows.Count - 1
            GridView1.Rows(i).Cells(GridView1.HeaderRow.Cells.Count - 1).Visible = False
            '   GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        Next

        For i As Integer = 0 To GridView1.Rows.Count - 1
            For j As Integer = 0 To GridView1.Rows(i).Cells.Count - 1
                GridView1.Rows(i).Cells(j).Style.Add("background-color", "#FFFFFF")
                GridView1.Rows(i).Cells(j).Style.Add("color", "#000000")
            Next
        Next
        GridView1.RenderControl(htw)
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
        If Session("GroupId") <> "1" Then
            DDlGroup.SelectedValue = Session("Userid")
            DDlGroup.Enabled = False
            DDlGroup.Visible = False
            LblGroup.Visible = False
        Else
            DDlGroup.Enabled = True
            DDlGroup.Visible = True
            LblGroup.Visible = True

        End If
    End Sub
    Private Sub FillUser1()
        ' Dim sql As String = "Select * from (select 0 as GroupId ,'All'  as GroupName Union All GroupId,GroupName From " + objDAL.tblUserGrpMaster + " Where " + objDAL.activeCondition + "  Order by GroupName"
        Dim sql As String = "Select * From (Select 0 As GroupId,'-- All  --' As GroupName Union ALL Select UserId,UserName From M_UserMaster Where  ActiveStatus='Y'  And RowStatus='Y' ) As Temp Order By GroupId"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        If dtData.Rows.Count > 0 Then
            DDlGroup1.DataSource = dtData
            DDlGroup1.DataTextField = "GroupName"
            DDlGroup1.DataValueField = "GroupID"
            DDlGroup1.DataBind()

        End If
        If Session("GroupId") <> "1" Then
            DDlGroup1.SelectedValue = Session("Userid")
            DDlGroup1.Enabled = False
            DDlGroup1.Visible = False
            LblGroup1.Visible = False
        Else
            DDlGroup1.Enabled = True
            DDlGroup1.Visible = True
            LblGroup1.Visible = True

        End If
    End Sub
    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        'gridview.BorderWidth = "2px"
        GvData.BorderStyle = BorderStyle.Solid
        GvData.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        Next

        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)

        GvData.RenderControl(hw)

        Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

        Dim sb As New StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload = new function(){")

        sb.Append("var printWin = window.open('', '', 'left=0")

        sb.Append(",top=0,width=1000,height=600,status=0');")

        sb.Append("printWin.document.write(""")

        sb.Append(gridHTML)

        sb.Append(""");")

        sb.Append("printWin.document.close();")

        sb.Append("printWin.focus();")

        sb.Append("printWin.print();")

        sb.Append("printWin.close();};")

        sb.Append("</script>")

        ClientScript.RegisterStartupScript(Me.GetType(), "GridPrint", sb.ToString())

        GvData.AllowPaging = True
        GvData.PagerSettings.Visible = True
        dtData = New DataTable
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub
    Protected Sub btnPrintCurrent1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent1.Click
        GridView1.AllowPaging = False
        GridView1.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GridView1.DataSource = dtData
        GridView1.DataBind()
        GridView1.PagerSettings.Visible = False
        'gridview.BorderWidth = "2px"
        GridView1.BorderStyle = BorderStyle.Solid
        GridView1.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        GridView1.HeaderRow.Cells(GridView1.HeaderRow.Cells.Count - 1).Visible = False
        GridView1.HeaderRow.Cells(GridView1.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GridView1.Rows.Count - 1
            GridView1.Rows(i).Cells(GridView1.HeaderRow.Cells.Count - 1).Visible = False
            GridView1.Rows(i).Cells(GridView1.HeaderRow.Cells.Count - 2).Visible = False
        Next

        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)

        GridView1.RenderControl(hw)

        Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

        Dim sb As New StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload = new function(){")

        sb.Append("var printWin = window.open('', '', 'left=0")

        sb.Append(",top=0,width=1000,height=600,status=0');")

        sb.Append("printWin.document.write(""")

        sb.Append(gridHTML)

        sb.Append(""");")

        sb.Append("printWin.document.close();")

        sb.Append("printWin.focus();")

        sb.Append("printWin.print();")

        sb.Append("printWin.close();};")

        sb.Append("</script>")

        ClientScript.RegisterStartupScript(Me.GetType(), "GridPrint", sb.ToString())

        GridView1.AllowPaging = True
        GridView1.PagerSettings.Visible = True
        dtData = New DataTable
        dtData = Session("GData")
        GridView1.DataSource = dtData
        GridView1.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        GvData.BorderStyle = BorderStyle.Solid
        ' gridview.BorderWidth = 
        GvData.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        Next

        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)

        GvData.RenderControl(hw)

        Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

        Dim sb As New StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload = new function(){")

        sb.Append("var printWin = window.open('', '', 'left=0")

        sb.Append(",top=0,width=1000,height=1000,status=0');")

        sb.Append("printWin.document.write(""")

        sb.Append(gridHTML)

        sb.Append(""");")

        sb.Append("printWin.document.close();")

        sb.Append("printWin.focus();")

        sb.Append("printWin.print();")

        sb.Append("printWin.close();};")

        sb.Append("</script>")

        ClientScript.RegisterStartupScript(Me.[GetType](), "GridPrint", sb.ToString())

        GvData.AllowPaging = True
        GvData.PagerSettings.Visible = True
        dtData = New DataTable
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub
    Protected Sub btnPrintAll1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll1.Click
        GridView1.AllowPaging = False
        GridView1.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GridView1.DataSource = dtData
        GridView1.DataBind()
        GridView1.PagerSettings.Visible = False
        GridView1.BorderStyle = BorderStyle.Solid
        ' gridview.BorderWidth = 
        GridView1.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        GridView1.HeaderRow.Cells(GridView1.HeaderRow.Cells.Count - 1).Visible = False
        GridView1.HeaderRow.Cells(GridView1.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GridView1.Rows.Count - 1
            GridView1.Rows(i).Cells(GridView1.HeaderRow.Cells.Count - 1).Visible = False
            GridView1.Rows(i).Cells(GridView1.HeaderRow.Cells.Count - 2).Visible = False
        Next

        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)

        GridView1.RenderControl(hw)

        Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

        Dim sb As New StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload = new function(){")

        sb.Append("var printWin = window.open('', '', 'left=0")

        sb.Append(",top=0,width=1000,height=1000,status=0');")

        sb.Append("printWin.document.write(""")

        sb.Append(gridHTML)

        sb.Append(""");")

        sb.Append("printWin.document.close();")

        sb.Append("printWin.focus();")

        sb.Append("printWin.print();")

        sb.Append("printWin.close();};")

        sb.Append("</script>")

        ClientScript.RegisterStartupScript(Me.[GetType](), "GridPrint", sb.ToString())

        GridView1.AllowPaging = True
        GridView1.PagerSettings.Visible = True
        dtData = New DataTable
        dtData = Session("GData")
        GridView1.DataSource = dtData
        GridView1.DataBind()
    End Sub

    Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
        BindData()
        btnShowRecord.Visible = False
        ddlGroupFields.SelectedIndex = 0
        txtSearch.Text = ""
    End Sub

    Protected Sub btnShowRecord1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord1.Click
        BindData()
        btnShowRecord1.Visible = False
        ddlGroupFields1.SelectedIndex = 0
        txtnewsearch.Text = ""
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
        If ChkDate.Checked = True Then
            If TxtFromDate.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Enter From Date  ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Else
                Condition_ = Condition_ & " AND CAST(Convert(varchar,M.RecTimeStamp,106) as DateTime)>='" & Trim(TxtFromDate.Text) & "'"

            End If
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

        '  If DDlGroup.SelectedValue <> 0 Then
        'Condition_ = Condition_ & "and U.GroupId='" & DDlGroup.SelectedValue & "'"
        'Else
        'Condition_ = Condition_
        'End If

        BindData(Condition_)
    End Sub
    Protected Sub BtnSubmit1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit1.Click
        Dim Condition1_ As String = ""
        If ddlGroupFields1.SelectedValue.ToString <> "None" Then
            If txtnewsearch.Text <> "" Then
                Condition1_ = " AND " + ddlGroupFields1.SelectedValue.ToString() + " like '%" + txtnewsearch.Text + "%'"

            End If

        End If

        If RbReplied1.SelectedValue <> "K" Then
            Condition1_ = Condition1_ & " AND M.IsReplied='" & Trim(RbReplied1.SelectedValue) & "' "
        Else
            Condition1_ = Condition1_
        End If
        Dim scrname As String = ""
        If ChkDate1.Checked = True Then
            If TxtFromDate1.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Enter From Date  ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Else
                Condition1_ = Condition1_ & " AND CAST(Convert(varchar,M.RecTimeStamp,106) as DateTime)>='" & Trim(TxtFromDate1.Text) & "'"

            End If
            If TxtToDate1.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Enter To Date  ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Else
                Condition1_ = Condition1_ & "AND CAST(Convert(varchar,M.RecTimeStamp,106) as DateTime)<='" & Trim(TxtToDate1.Text) & "'"
            End If

        End If
        If RbtStatus1.SelectedValue <> "K" Then
            Condition1_ = Condition1_ & "And M.ComplaintStatus='" & RbtStatus1.SelectedValue & "'"
        Else
            Condition1_ = Condition1_
        End If

        '  If DDlGroup.SelectedValue <> 0 Then
        'Condition_ = Condition_ & "and U.GroupId='" & DDlGroup.SelectedValue & "'"
        'Else
        'Condition_ = Condition_
        'End If

        BindData(Condition1_)
    End Sub
End Class
