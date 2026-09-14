Imports System.Data
Partial Class AssignRank
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub BtnLegShift_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLegShift.Click
        Try
            Leg_Shift()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Private Sub FillData()
        Try
            Dim str As String = ""
            Dim dt As DataTable = New DataTable()
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Session("Compid") = "1078" Then
                str = " Select RankId,Rankname+'(%)' as rank from m_rankmaster Where  ActiveStatus = 'Y' and Rankid<=(Select a.Planid from M_memberMaster  as a,M_memberMaster as b where a.formno=b.Refformno and b.formno='" & HdnFormno.Value & "')"
            ElseIf Session("Compid") = "1093" Then
                str = " Select RankId,Rankname as rank from m_rankmaster Where  ActiveStatus = 'Y' And Rankid=1 "
            Else
                str = " Select RankId,Rank from MstRanks Where  ActiveStatus = 'Y'"
            End If
            dt = obj.GetData(str)
            If (dt.Rows.Count > 0) Then
                ddlRank.DataSource = dt
                ddlRank.DataTextField = "Rank"
                ddlRank.DataValueField = "RankId"
                ddlRank.DataBind()
                ddlRank.Items.Insert(0, "---Select Rank---")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Leg_Shift()
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = ""
            If ddlRank.SelectedValue = 0 Then
                lblMemberName.Text = "Please select rank."
                lblMemberName.ForeColor = Drawing.Color.Red
                txtMemberID.Text = ""
                BtnLegShift.Enabled = False
                HdnFormno.Value = 0
            End If

            If Session("Compid") = "1078" Then
                str = "exec Sp_IdWiseRank  '" & HdnFormno.Value & "','" & ddlRank.SelectedValue & "','" & Val(Session("userid")) & "','" & txtRemark.Text & "'"

            ElseIf Session("CompID") = 1093 Then
                Dim dt As DataTable = New DataTable()
                str = " select * from Idwiserank Where formno='" & HdnFormno.Value & "' and Rankid='" & ddlRank.SelectedValue & "' and ActiveStatus = 'Y' "
                dt = obj.GetData(str)
                If (dt.Rows.Count > 0) Then
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Selected Rank Already Assign !!!');location.replace('AssignRank.aspx');", True)
                    BtnLegShift.Enabled = False
                    Exit Sub
                End If
                str = "exec Sp_IdWiseRank  '" & HdnFormno.Value & "','" & ddlRank.SelectedValue & "','" & Val(Session("userid")) & "','" & txtRemark.Text & "'"
            ElseIf Session("CompID") = 1106 Then
                Dim dt As DataTable = New DataTable()
                str = " select * from MstRankAchievers Where formno = '" & HdnFormno.Value & "' and Rankid = '" & ddlRank.SelectedValue & "' "
                dt = obj.GetData(str)
                If (dt.Rows.Count > 0) Then
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Selected Rank Already Assign.!');location.replace('AssignRank.aspx');", True)
                    BtnLegShift.Enabled = False
                    Exit Sub
                End If
                str = "exec Sp_IdWiseRank  '" & HdnFormno.Value & "','" & ddlRank.SelectedValue & "','" & Val(Session("userid")) & "','" & txtRemark.Text & "'"
            Else
                str = " Exec Sp_IdWiseRank 0,'" & txtMemberID.Text & "','" & ddlRank.SelectedValue & "','" & txtRemark.Text & "' "
            End If

            Dim i As Integer = 0
            i = obj.SaveData(str)
            If (i > 0) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record  Save successfully !!!');location.replace('ListAssignRank.aspx');", True)
                BtnLegShift.Enabled = False
            Else
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record  Already Exists !!!');", True)
                BtnLegShift.Enabled = True
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function DisableTheButton(ByVal pge As Control, ByVal btn As Control) As String
        Dim sb As New System.Text.StringBuilder()
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ")
        sb.Append("this.value = 'Please Wait...';")
        sb.Append("this.disabled = true;")
        sb.Append(pge.Page.GetPostBackEventReference(btn))
        sb.Append(";")
        Return sb.ToString()
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.BtnLegShift.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnLegShift))
            If Not Page.IsPostBack Then
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Session("AStatus") = "OK" Then
                    Session("PageName") = "Member / Assign Rank"
                    FillData()
                    BtnLegShift.Enabled = False
                Else
                    Response.Redirect("Logout.aspx")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub txtMemberID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberID.TextChanged
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Session("CompiD") = "1078" Or Session("CompiD") = "1093" Or Session("CompiD") = "1106" Then
                Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl,ActiveStatus  From M_MemberMaster WHERE IDNO='" & Trim(txtMemberID.Text) & "'"
            Else
                Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl,ActiveStatus  From M_MemberMaster WHERE IDNO='" & Trim(txtMemberID.Text) & "' And ActiveStatus = 'Y'"
            End If
            Dim Dt_ As New DataTable
            Dt_ = obj.GetData(Sql)
            If Session("CompiD") = "1106" Then
                If Dt_.Rows.Count = 0 Then
                    lblMemberName.Text = " Please enter correct Member ID."
                    lblMemberName.ForeColor = Drawing.Color.Red
                    txtMemberID.Text = ""
                    BtnLegShift.Enabled = False
                    HdnFormno.Value = 0
                Else
                    lblMemberName.Text = Dt_.Rows(0)("MemName")
                    lblMemberName.ForeColor = Drawing.Color.Red
                    HdnFormno.Value = Dt_.Rows(0)("formno")
                    BtnLegShift.Enabled = True
                End If
                Sql = "SELECT * FROM MstRanks WHERE RankID > ISNULL((SELECT MAX(RankID) FROM MstRankAchievers WHERE FormNo = '" & HdnFormno.Value & "'),0)ORDER BY RankID ASC"
                Dim Dt As New DataTable
                Dt = obj.GetData(Sql)
                ddlRank.DataSource = Dt
                ddlRank.DataTextField = "Rank"
                ddlRank.DataValueField = "RankId"
                ddlRank.DataBind()
                ddlRank.Items.Insert(0, "---Select Rank---")
            Else
                If Dt_.Rows.Count = 0 Then
                    lblMemberName.Text = " Please enter correct Member ID."
                    lblMemberName.ForeColor = Drawing.Color.Red
                    txtMemberID.Text = ""
                    BtnLegShift.Enabled = False
                    HdnFormno.Value = 0

                ElseIf Dt_.Rows(0)("Activestatus") = "N" Then
                    lblMemberName.Text = " Please Activate Member ID first after allot rank."
                    lblMemberName.ForeColor = Drawing.Color.Red
                    txtMemberID.Text = ""
                    BtnLegShift.Enabled = False
                    HdnFormno.Value = 0
                Else
                    lblMemberName.Text = Dt_.Rows(0)("MemName")
                    lblMemberName.ForeColor = Drawing.Color.Red
                    HdnFormno.Value = Dt_.Rows(0)("formno")
                    BtnLegShift.Enabled = True
                End If
               FillData()
            End If
            
        Catch ex As Exception

        End Try
    End Sub


    'Private Function CheckMember() As Boolean
    '    Dim Result As Boolean = False
    '    Try


    '        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl  From M_MemberMaster WHERE IDNO='" & Trim(txtMemberID.Text) & "' And ActiveStatus = 'Y'"
    '        Dim Dt_ As New DataTable
    '        Dt_ = obj.GetData(Sql)
    '        If Dt_.Rows.Count = 0 Then
    '            lblMemberName.Text = " Please enter correct Member ID."
    '            lblMemberName.ForeColor = Drawing.Color.Red
    '            txtMemberID.Text = ""
    '            Result = False
    '        Else
    '            lblMemberName.Text = Dt_.Rows(0)("MemName")
    '            lblMemberName.ForeColor = Drawing.Color.Red
    '            Result = True
    '        End If
    '    Catch ex As Exception

    '    End Try
    '    Return Result
    'End Function
End Class
