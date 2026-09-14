Imports System.Data.SqlClient
Imports System.Data
Imports ClosedXML.Excel
Imports System.IO

Partial Class amontRecivedsalecredit
    Inherits System.Web.UI.Page

    Dim strquery As String
    Dim dt As DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Dim formno As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then

            If Page.IsPostBack = False Then
                PaymentDetails()

            End If

        Else
            Response.Redirect("Logout.aspx")
            Response.End()
        End If
    End Sub
    Private Function GetFormNo(ByVal idno As String) As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Dim idNo As String
        Dim formno As String
        'idNo = Trim(txtMemberID.Text)
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idno & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMemberID.Text = ""
        End If
        Return formno
    End Function
    Private Sub PaymentDetails()
        Try

            Dim dt As New DataTable
            Dim col As String = ""
            Dim col2 As String = ""
            Dim col9 As String = ""
            Dim obj As DAL
            Dim Idno As String = "0"
            Dim startDate As Date
            Dim endDate As Date
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Condition As String = ""
            If txtMemberID.Text <> "" Then
                Idno = txtMemberID.Text.Trim
                'formno = GetFormNo(Idno)
            Else
                Idno = "0"
            End If
            If txtfrmdate.Text <> "" Then
                startDate = txtfrmdate.Text
            Else

            End If
            If txttodate.Text <> "" Then
                endDate = txttodate.Text
            End If
            strquery = " Sp_GetReceivedByCreditSaleReport '" & Idno & "','" & txtfrmdate.Text & "','" & txttodate.Text & "'"
            dt = obj.GetData(strquery)
            If dt.Rows.Count > 0 Then

                btnExport.Visible = True

            End If


            GvData.DataSource = dt
            GvData.DataBind()

            Session("Gvdata") = dt


        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try
    End Sub


    Protected Sub btnsubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        PaymentDetails()
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            If Session("CompID") = "1066" Then
                PaymentDetails()
                'Else

            End If
            ExportExcel()

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")

        End Try
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("Gvdata")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Report")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=amontRecivedbyincome.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Orderno, Idno, scrname As String
        'Dim msgRslt As MsgBoxResult = MsgBox("Jig tool succussfully registered. Do you want to continue register jigtool? .", MsgBoxStyle.YesNo)
        'If msgRslt = MsgBoxResult.Yes Then


        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)

        Orderno = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        Idno = DirectCast(GVRw.FindControl("lblidno"), Label).Text
        formno = GetFormNo(Idno)
        'Dim Sql As String = "exec sp_Iddeactive " & Val(Session("Formno")) & ",'" & Orderno & "' "
        Dim Sql As String = "exec sp_Iddeactive " & formno & ",'" & Orderno & "' "

        Dim obj As DAL
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        Dim updateEffect As Integer = 0
        'below commit 19 Jan 2023
        updateEffect = obj.UpdateData(Sql)

        'Dim updateEffect As Integer = obj.GetData(Sql)
        If updateEffect <> 0 Then
            'If dt.Rows.Count > 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Row Data! ');" & "</SCRIPT>"
        End If
        'ElseIf msgRslt = MsgBoxResult.No Then
        'scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group! ');" & "</SCRIPT>"
        'End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
        'BindData()
        PaymentDetails()
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("Gvdata")
        GvData.DataBind()
    End Sub
End Class