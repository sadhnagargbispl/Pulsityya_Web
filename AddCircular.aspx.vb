Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddCircular
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim KitIdQS As String
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim Ad As SqlDataAdapter
    Dim tmptable As DataTable
    Dim objGen As clsGeneral = New clsGeneral
   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") <> "OK" Then
            Response.Redirect("Logout.aspx")
        End If
        If Not Page.IsPostBack Then

            BindData()
            'TxtAmount.Text = Amount()
        End If

      
    End Sub

    Private Sub FillCircularMaster()
        Dim strQuery As String
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        strQuery = "SELECT CircularCODE,CircularNAME as CircularName FROM M_CircularMaster WHERE ACTIVESTATUS='Y' ORDER BY CircularName"
        'dbConnect.OpenConnection()
        tmpTable = objDAL.GetData(strQuery)
        'dbConnect.Fill_Data_Tables(strQuery, Dt)
        With CmbCircular
            .DataSource = tmpTable
            .DataValueField = "CircularCODE"
            .DataTextField = "CircularName"
            .DataBind()
            '.SelectedIndex = 0
        End With
        Conn.Close()
    End Sub

    Private Sub BindData()
        Dim sql As String
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        sql = "select * ,'true' as Status from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster where ActiveStatus='Y' "
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        GvProd.DataSource = Dt
        GvProd.DataBind()
        Conn.Close()
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim chk As CheckBox
        Dim Lbl As Label
        Dim prodId As String = ""



        For Each Gvr As GridViewRow In GvProd.Rows

            chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            Lbl = DirectCast(Gvr.FindControl("LblProdCode"), Label)
            If chk.Checked Then
                If prodId = "" Then
                    prodId = Lbl.Text
                Else
                    prodId = prodId & "," & Lbl.Text
                End If
            End If
        Next
        Dim Sql As String
        Dim str As String = "select IsNull (Max(CircularCode+1),1) as CircularCode from M_CircularMaster"
        Dim dt As New DataTable
        Dim circularno As String
        dt = objDAL.GetData(str)
        If (dt.Rows.Count > 0) Then
            circularno = dt.Rows(0)("CircularCode")

        End If


        Sql = "Insert into M_Circularmaster(CircularCode,Circularname,ProdId,ActiveStatus,RecTimeStamp,FromDate,ToDate)Values('" & Val(circularno) & "','" & txtCircularName.Text & "','" & prodId & "','" & RbtStatus.SelectedValue & "',GetDate(),'" & txtStartDate.Text & "','" & txtEndDate.Text & "')"
        If objDAL.SaveData(Sql) > 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Successfully Saved!!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        End If

    End Sub


    Protected Sub GvProd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvProd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
    End Sub

    Protected Sub RbtCircular_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtCircular.SelectedIndexChanged
        'Dim Chk As CheckBox
        If RbtCircular.SelectedValue = "N" Then

            BtnSave.Visible = True
            PCircular.Visible = False
            ClearAll()
            'txtCircularName.Text = ""
            'txtStartDate.Text = ""
            'txtEndDate.Text = ""
            pCircularName.Visible = True
            PStartDate.Visible = True
            BtnModify.Visible = False
            GvProd.Visible = True
            ' PStatus.Visible = True
            'For Each Gvr As GridViewRow In GvProd.Rows
            '    'Lbl = DirectCast(Gvr.FindControl("LblProdCode"), Label)
            '    Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            '    Chk.Checked = False
            'Next

        Else
            pCircularName.Visible = False
            '  PStatus.Visible = False
            PCircular.Visible = True
            PStartDate.Visible = False
            GvProd.Visible = False
            BtnSave.Visible = False
            BtnModify.Visible = False
            ClearAll()

            FillCircularMaster()

        End If
    End Sub
    Private Sub BindDataCircular()
        'PPrize.Visible = True
        'PProduct.Visible = True
        PStartDate.Visible = True
        BtnModify.Visible = True
        pCircularName.Visible = True
        GvProd.Visible = True
        ' PStatus.Visible = True

        Dim sql As String
        Dim products As String
        Dim Lbl As Label
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        sql = "select 'False' as Status,ActiveStatus, CircularCode,CircularName,Replace(CONVERT(VARCHAR,FromDate,106),'','-') as FromDate,Replace(CONVERT(VARCHAR,ToDate,106),'','-') as ToDate, ProdId  from M_CircularMaster  where CircularName='" & CmbCircular.SelectedItem.Text & "'"
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        Dim Chk As New CheckBox
        If Dt.Rows.Count > 0 Then
            txtStartDate.Text = Dt.Rows(0)("FromDate")
            txtEndDate.Text = Dt.Rows(0)("ToDate")
            products = Dt.Rows(0)("ProdId")
            txtCircularName.Text = Dt.Rows(0)("CircularName")
            RbtStatus.SelectedValue = Dt.Rows(0)("ActiveStatus")
            Dim ProductArray() As String = products.Split(",")

            For i As Integer = 0 To ProductArray.Length - 1
                For Each Gvr As GridViewRow In GvProd.Rows
                    Lbl = DirectCast(Gvr.FindControl("LblProdCode"), Label)
                    Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                    If Lbl.Text = ProductArray(i) And Lbl.Text <> "" Then
                        Chk.Checked = True

                    End If
                Next
            Next


        End If
        Conn.Close()

    End Sub

    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        BindDataCircular()
    End Sub

  
    Protected Sub BtnModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnModify.Click
        Dim Sql, scrname As String
        Dim chk As CheckBox
        Dim Lbl As Label
        Dim prodId As String = ""
        For Each Gvr As GridViewRow In GvProd.Rows

            chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            Lbl = DirectCast(Gvr.FindControl("LblProdCode"), Label)
            If chk.Checked Then
                If prodId = "" Then
                    prodId = Lbl.Text
                Else
                    prodId = prodId & "," & Lbl.Text
                End If
            End If
        Next
        Sql = "Insert into TempCircularMaster select * ,'" & Session("UserID") & "',GetDate() from M_CircularMaster where circularCode='" & CmbCircular.SelectedValue & "'"

        Sql = Sql & "Update M_CircularMaster Set CircularName='" & txtCircularName.Text & "',FromDate='" & txtStartDate.Text & "',ToDate='" & txtEndDate.Text & "',ProdId='" & prodId & "',ActiveStatus='" & RbtStatus.SelectedValue & "' where CircularCode='" & CmbCircular.SelectedValue & "'"
        'objDAL.UpdateData(Sql)
        Dim updateEffect As Integer = 0
        updateEffect = objDAL.SaveData(Sql)

        If String.IsNullOrEmpty(Request("CircularCode")) = False And updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
        ElseIf updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        End If

        'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        FillCircularMaster()

    End Sub

    Protected Sub CmbCircular_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbCircular.SelectedIndexChanged

    End Sub

    Protected Sub txtEndDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEndDate.TextChanged

        Dim startDate, EndDate As DateTime
        EndDate = Convert.ToDateTime(txtEndDate.Text)
        startDate = Convert.ToDateTime(txtStartDate.Text)
        If startDate.Date > EndDate.Date Then
            LblError.Text = "Enter Greater Date from Start Date"
            LblError.Visible = True
        Else
            LblError.Visible = False

        End If

    End Sub
    Protected Sub ClearAll()
        txtCircularName.Text = ""
        txtStartDate.Text = ""
        txtEndDate.Text = ""
        Dim Chk As CheckBox
        For Each Gvr As GridViewRow In GvProd.Rows
            'Lbl = DirectCast(Gvr.FindControl("LblProdCode"), Label)
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            Chk.Checked = False
        Next

    End Sub
End Class
