Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class FlatTypeMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Flat Type Master"
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            ' objModuleFun = New ModuleFunction()
            If Not Page.IsPostBack Then
                Session("Mode") = "Insert"
                Dim DepoID As Integer = 0
                If Request.QueryString.HasKeys And Not Request.QueryString("Key") Is Nothing Then
                    DepoID = Val(Crypto.Decrypt(Replace(Request.QueryString("key"), " ", "+")))
                    If DepoID <> 0 Then
                        FillDetail(DepoID)
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim scrname As String = ""
            If TxtFlatType.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Enter Flattype.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If
            ClearInject()
            Dim str As String = ""
            If Session("Mode") = "Insert" Then
                str = "Insert Into M_FlattypeMaster(Flattype,ActiveStatus)"
                str = str & " Values('" & TxtFlatType.Text & "','" & ddlStatus.SelectedValue & "')"

            Else

                str = "Insert Into TempFlatTypeMaster(Id,Flattype,ActiveStatus,TrectimeStamp,TUserid)select Id,Flattype,ActiveStatus,getdate(),'" & Val(Session("userid")) & "' from M_FlattypeMaster where id='" & LblId.Text & "'; "
                str = str & " Update M_FlattypeMaster  Set Flattype='" & TxtFlatType.Text & "',ActiveStatus='" & ddlStatus.SelectedValue & "' where id='" & Val(LblId.Text) & "'"

            End If
            Dim i As Integer = objDAL.SaveData(str)
            If (i > 0) Then
                If Session("Mode") = "Insert" Then
                    scrname = "<SCRIPT language='javascript'>alert('Save Successfully  !!');" & "</SCRIPT>"
                Else
                    scrname = "<SCRIPT language='javascript'>alert('Update Successfully  !!');" & "</SCRIPT>"
                End If

                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Else
                scrname = "<SCRIPT language='javascript'>alert('Try Again Later !!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ClearInject()
        TxtFlatType.Text = objDAL.ClearInject(TxtFlatType.Text)
        ' TxtDescription.Text = objDAL.ClearInject(TxtDescription.Text)
    End Sub
    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Response.Redirect("ListFlattypemaster.aspx")
    End Sub
    Public Sub FillDetail(ByVal DepoID As Integer)
        Try
            Dim Qry As String = "Select * From  V#FlattypeMaster Where ID=" & DepoID
            dtData = New DataTable
            dtData = objDAL.GetData(Qry)
            If dtData.Rows.Count > 0 Then
                LblID.Text = dtData.Rows(0)("ID")
                TxtFlatType.Text = dtData.Rows(0)("Flattype")



                ddlStatus.SelectedValue = dtData.Rows(0)("ActiveStatus")
                Session("Mode") = "Update"
                btnSave.Text = "Update"


            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
