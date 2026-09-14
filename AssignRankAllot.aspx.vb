Imports System.Data
Partial Class AssignRankAllot
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Session("AStatus") = "OK" Then
                    Session("PageName") = "Member / Assign Rank"
                    FillData()
                    FillCategoryData()
                    BtnLegShift.Enabled = False
                Else
                    Response.Redirect("Logout.aspx")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillData(Optional ByVal condition As String = "")
        Try
            Dim str As String = ""
            Dim dt As DataTable = New DataTable()
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            str = " Select RankId,Rankname as rank from m_rankmaster Where ActiveStatus = 'Y' And 1=1 " & condition & " order by RankId "
            dt = obj.GetData(str)
            If (dt.Rows.Count > 0) Then
                ddlRank.DataSource = dt
                ddlRank.DataTextField = "Rank"
                ddlRank.DataValueField = "RankId"
                ddlRank.DataBind()
                ddlRank.Items.Insert(0, "---Select Rank---")
                Session("MKit") = dt
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillCategoryData(Optional ByVal condition As String = "")
        Try
            Dim str As String = ""
            Dim dt As DataTable = New DataTable()
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            str = " SELECT 0 as CategoryID,'---Select Category---' as CategoryName Union all"
            str &= " SELECT CategoryID,upper(CategoryName) as CategoryName FROM ProductCategories where ACtivestatus = 'Y'"
            dt = obj.GetData(str)
            If (dt.Rows.Count > 0) Then
                DdlCategory.DataSource = dt
                DdlCategory.DataTextField = "CategoryName"
                DdlCategory.DataValueField = "CategoryID"
                DdlCategory.DataBind()
                Session("MCategory") = dt
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub RankAllot()
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = ""
            str = "exec Sp_IdWiseRank  '" & HdnFormno.Value & "','" & ddlRank.SelectedValue & "','" & Val(Session("userid")) & "','" & txtRemark.Text & "','" & DdlCategory.SelectedValue & "'"
            Dim i As Integer = 0
            i = obj.SaveData(str)
            If (i > 0) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record  Save successfully !!!');location.replace('AssignRankAllotReport.aspx');", True)
                BtnLegShift.Enabled = False
            Else
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record  Already Exists !!!');", True)
                BtnLegShift.Enabled = True
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub BtnLegShift_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLegShift.Click
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If ddlRank.SelectedValue = 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please select Rank.');", True)
                Exit Sub
            End If
            If DdlCategory.SelectedValue = 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please select Category.');", True)
                Exit Sub
            End If
            Dim dt1 As DataTable = New DataTable()
            Dim Str1 = " select * from Idwiserank Where formno = '" & HdnFormno.Value & "'"
            dt1 = obj.GetData(Str1)
            If (dt1.Rows.Count > 0) Then
                Dim category = dt1.Rows(0)("CategoryID")
                If DdlCategory.SelectedValue = category Then
                    BtnLegShift.Enabled = True
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('The Category of this ID does not match the selected Category.');", True)
                    BtnLegShift.Enabled = True
                    Exit Sub
                End If
            End If
            Dim str As String = ""
            Dim dt As DataTable = New DataTable()
            str = " select * from Idwiserank Where formno='" & HdnFormno.Value & "' and Rankid='" & ddlRank.SelectedValue & "' and ActiveStatus = 'Y' "
            dt = obj.GetData(str)
            If (dt.Rows.Count > 0) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Selected Rank Already Assign !!!');location.replace('AssignRankAllot.aspx');", True)
                BtnLegShift.Enabled = False
                Exit Sub
            End If
            Dim str11 As String = ""
            Dim dt11 As DataTable = New DataTable()
            str11 = "SELECT CASE WHEN (SELECT COUNT(*) FROM IdWiseRank WHERE RankId = '" & ddlRank.SelectedValue & "')< " & _
            "(SELECT TotalCnt FROM m_rankmaster WHERE RankId = '" & ddlRank.SelectedValue & "')THEN 'OK' ELSE 'LIMIT' END AS ResultMsg;"
            dt11 = obj.GetData(str11)
            Dim result As String = ""
            If (dt11.Rows.Count > 0) Then
                result = dt11.Rows(0)("ResultMsg")
                If result.ToString() = "OK" Then
                    RankAllot()
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Maximum limit for this Rank has been achieved.!');", True)
                    BtnLegShift.Enabled = False
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub
    Protected Sub txtMemberID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberID.TextChanged
        Try
            If DdlCategory.SelectedValue = 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please select Category.');", True)
                txtMemberID.Text = ""
                Exit Sub
            End If
            Dim dt1 As New DataTable
            dt1 = DirectCast(Session("MKit"), DataTable)
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl,ActiveStatus,planid as rankid  "
            Sql &= "From M_MemberMaster WHERE IDNO='" & Trim(txtMemberID.Text) & "'"
            Dim Dt_ As New DataTable
            Dt_ = obj.GetData(Sql)
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
                Dim Srt = "SELECT topupseq FROM m_rankmaster where rankid = '" & Dt_.Rows(0)("rankid") & "' "
                Dim Dt__ As New DataTable
                Dt__ = obj.GetData(Srt)
                If Dt__.Rows.Count > 0 Then
                    LblCondition.Text = "and TopupSeq>'" & Dt__.Rows(0)("TopupSeq") & "'"
                End If
                'FillData(LblCondition.Text.Trim)

                BtnLegShift.Enabled = True
            End If

        Catch ex As Exception
        End Try
    End Sub

    Protected Sub ddlRank_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRank.SelectedIndexChanged
        Dim dt As DataTable = New DataTable()
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim Str = " select * from Idwiserank Where formno = '" & HdnFormno.Value & "' AND RankID = '" & ddlRank.SelectedValue & "' "
        dt = obj.GetData(Str)
        If (dt.Rows.Count > 0) Then
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Already Exist This ID.');", True)
            BtnLegShift.Enabled = False
            Exit Sub
        Else
            BtnLegShift.Enabled = True
        End If
        BtnLegShift.Enabled = True
    End Sub
End Class
