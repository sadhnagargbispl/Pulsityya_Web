
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class AddDirectIDMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    'Dim objDAL As New DAL
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
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
        'objDAL = New DAL()
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Me.BtnFundTransfer.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnFundTransfer))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                BindData()
            End If
        End If
    End Sub

    Public Sub BindData()
        Dim sql As String = "exec Sp_GetDirectIDS "
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData

    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim formno, scrname As String
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        formno = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        Dim Sql As String = "DELETE FROM M_DirectIDMaster where formno = '" & formno & "'"
        Dim updateEffect As Integer = objDAL.UpdateData(Sql)
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
        BindData()
    End Sub
    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click
        Try
            If Trim(txtoffer.Text) = "" Then
                Dim scrName1 = "<SCRIPT language='javascript'>alert('Please Enter Member ID.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrName1, False)
                Exit Sub
            End If
            lblError.Visible = False
            Dim query As String = ""
            Dim voucherNo As String = ""
            Dim scrName As String
            query = " insert Into m_DirectIDMaster (MemberName,Formno,ActiveStatus,RecTimeStamp,MemberID,LegNo)"
            query &= " values('" & LblMemName.Text & "','" & TxtFormNo.Text & "','Y',getdate(),'" & txtoffer.Text & "','" & rdblist.SelectedValue & "')"
            Dim K As String = " Begin Try Begin Transaction " & query & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH      "
            If objDAL.SaveData(K) <> 0 Then

                scrName = "<SCRIPT language='javascript'>alert('Successfully Saved!!.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                LblMemName.Text = ""
                TxtFormNo.Text = ""
                txtoffer.Text = ""
                BindData()

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function Check_IdNo() As Boolean
        Try
            Dim Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl  From " & objDAL.tblMemberMaster & " " & _
                       " WHERE IDNO='" & Trim(txtoffer.Text) & "'"
            Dim Dt_ As New DataTable
            Dt_ = objDAL.GetData(Sql)
            If Dt_.Rows.Count = 0 Then
                LblMemName.Text = " Please enter correct Member ID."
                LblMemName.ForeColor = Drawing.Color.Red
                txtoffer.Text = ""
                BtnFundTransfer.Enabled = False
                Return False
            Else
                If Session("compid") = "1091" Then
                    Dim Sql_ = "SELECT count(*) as Cnt FROM m_DirectIDMaster WHERE MemberID = '" & Trim(txtoffer.Text) & "' "
                    Dim Dt__ As New DataTable
                    Dt__ = objDAL.GetData(Sql_)
                    If Dt__.Rows.Count > 0 Then
                        If Dt__.Rows(0)("Cnt") > 0 Then
                            Dim scrName1 = "<SCRIPT language='javascript'>alert('Already Exist ID.!');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrName1, False)
                            txtoffer.Text = ""
                            Exit Function
                        Else
                            LblMemName.Text = Dt_.Rows(0)("MemName")
                            LblMobl.Text = Dt_.Rows(0)("Mobl")
                            LblMemName.ForeColor = Drawing.Color.Black
                            TxtFormNo.Text = Dt_.Rows(0)("FormNo")
                            BtnFundTransfer.Enabled = True
                            Return True
                        End If
                    End If
                Else
                    LblMemName.Text = Dt_.Rows(0)("MemName")
                    LblMobl.Text = Dt_.Rows(0)("Mobl")
                    LblMemName.ForeColor = Drawing.Color.Black
                    TxtFormNo.Text = Dt_.Rows(0)("FormNo")
                    BtnFundTransfer.Enabled = True
                    Return True
                End If
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try


    End Function
    Protected Sub txtoffer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtoffer.TextChanged
        Check_IdNo()
    End Sub
End Class
