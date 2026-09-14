Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Globalization
Partial Class App_UI_Application_Pages_GenerateEP
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim quantity As String
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Generate EP"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then

            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Not Page.IsPostBack Then

                FillEP()
            End If
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub

    Public Sub FillEP()
        Sql = "  Select Credit,Debit,Balance from M_GenerateEP"

        Dim Dt As New DataTable
        Dt = obj.GetData(Sql)
        gv.DataSource = Dt
        gv.DataBind()
    End Sub




    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try


            Dim sql1 As String = "Select Balance from M_GenerateEP"
            Dim dt1 As New DataTable
            dt1 = obj.GetData(sql1)
            If (dt1.Rows.Count > 0) Then
                Session("EPBalance") = dt1.Rows(0)("Balance")
            End If


            Dim sqlstr As String = String.Empty
            Sql = " Exec Sp_GenerateEP 'Credit', '" & TxtQty.Text & "'"
            Sql &= "Insert into M_GenerateEPHistory (Formno,EP,VcType,Remark,Balance) Values"
            Sql &= "(0,'" & TxtQty.Text & "','C','" & txtRemark.Text & "', '" & (Val(Session("EPBalance")) + Val(TxtQty.Text)) & "')"
            sqlstr = " Begin Try Begin Transaction " & Sql & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH      "

            If obj.SaveData(sqlstr) <> 0 Then
                lblError.Text = TxtQty.Text & " EP stock of " & TxtQty.Text & " have been sucessfully generated."
                FillEP()
                Clear()
            Else
                lblError.Text = " EP Not generated. Please Try Again"
            End If


        Catch ex As Exception

        End Try
    End Sub

    Public Sub Clear()
        TxtQty.Text = ""
        txtRemark.Text = ""
    End Sub


End Class
