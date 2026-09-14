Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class AddLevelNew
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral
    Dim obj As DAL
    Dim Sql As String = ""
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim Adp As SqlDataAdapter
    Dim dRead As SqlDataReader
    Dim Ds As New DataSet
    Dim dt As New DataTable
    Dim StrQuery As String
    Dim ScrName As String
    ' Dim objDAL As DAL

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Conn.Open()
            If Not Page.IsPostBack Then
                lblMsg.Visible = False
                'Fill Group drop down list
                'Dim qry1 As String = "select * from discmartinv..M_ProductMaster  where ActiveStatus='Y' and onwebsite='Y' "
                'objModuleFun.FillCombo(qry1, ddlGroup, "ProductName", "ProductCode")
                FillKit()
                'Fill grid with data
                ' BindData()
                'Session("ProductCode") = ddlGroup.SelectedValue.ToString()
                'Session("ProductName") = ddlGroup.SelectedItem.Text
                'Session("grpID") = ddlGroup.SelectedValue.ToString()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub FillKit()
        Try

            Dim qry1 As String = "select * from M_ProductMaster  where ActiveStatus='Y' and onwebsite='Y' "

            'Comm = New SqlCommand(qry1, Conn)
            'Dim Dr As SqlDataReader
            'Dr = Comm.ExecuteReader()
            Dim Dt As New DataTable
            Dt = objDAL.GetData(qry1)
            ddlGroup.DataSource = Dt
            ddlGroup.DataTextField = "ProductName"
            ddlGroup.DataValueField = "ProductCode"
            ddlGroup.DataBind()
            Session("MKit") = Dt
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            ''obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Sub

    Public Sub BindData()
        'Dim strlist As New List(Of String)
        Try

            Dim dTbar As DataTable
            dTbar = New DataTable
            Dim q As String = "select  a.Barcode,ProductName,b.Prodid,a.BId,a.ExpDate from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_BarCodeMaster as a," & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster as b" & _
                  " where a.ProdId=b.ProdId And a. ActiveStatus='Y' and b.Brandcode=0 And B.ActiveStatus='Y'  Order by ProdId"
            dTbar = objDAL.GetData(q)
            Session("DtBarCode") = dTbar

            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim qry1 As String = "select 'false' as status ,0 as Qty ,0 as DiscAmt,0 as Tax,* from " & _
          "" & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster  where ActiveStatus='Y'    order by ProdId"

            dtData = New DataTable
            dtData = objDAL.GetData(qry1)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData

            SetCheckBoxValue()

        Catch ex As Exception

        End Try
    End Sub



    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim qry1 As String = "Update M_KitProductDetail set RowStatus='N',LastModified='Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "',UserId='" & Val(Session("UserId")) & "' where KitId='" & Val(ddlGroup.SelectedValue.ToString()) & "';"
        Dim scrname As String
        Dim Chk As CheckBox
        Dim LblProd As Label
        Dim MRP As String
        Dim Barcode As String
        Dim Qty, Tax, Discount As String

        If GvData.Rows.Count > 0 Then
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkProd"), CheckBox)
                LblProd = DirectCast(Gvr.FindControl("lblProdId"), Label)
                Barcode = DirectCast(Gvr.FindControl("DDlBarcode"), DropDownList).SelectedValue
                Qty = DirectCast(Gvr.FindControl("TxtQty"), TextBox).Text
                MRP = DirectCast(Gvr.FindControl("LblRate"), Label).Text
                Tax = DirectCast(Gvr.FindControl("Tax"), TextBox).Text
                Discount = DirectCast(Gvr.FindControl("TxtDisc"), TextBox).Text

                If Chk.Checked = True Then
                    If Qty = 0 Then

                        scrname = "<SCRIPT language='javascript'>alert('Selected product quantity can not be Zero.!! ');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                        Exit Sub
                    ElseIf MRP = "" Then
                        scrname = "<SCRIPT language='javascript'>alert('Selected product MRP can not be blank.!! ');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                        Exit Sub
                        'ElseIf Discount = "" Then
                        '    scrname = "<SCRIPT language='javascript'>alert('Selected product Discount can not be blank.!! ');" & "</SCRIPT>"
                        '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                        '    Exit Sub
                        'ElseIf Tax = "" Then
                        '    scrname = "<SCRIPT language='javascript'>alert('Selected product Tax can not be blank.!! ');" & "</SCRIPT>"
                        '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                        '    Exit Sub
                    Else
                        qry1 = qry1 & " insert into M_KitProductDetail (KPId,KitId,ProdId,ActiveStatus,RectimeStatus,RowStatus,LastModified,UserId,Qty,BarCode,MRP,DiscAmt,Tax)" & _
                        "Select Case When Max(KPId) Is Null Then '1' Else Max(KPId)+1 END as KPId ,'" & Val(ddlGroup.SelectedValue.ToString()) & "','" & Val(LblProd.Text) & "','Y',GetDate(),'Y','','" & Session("UserId") & "','" & Qty & "','" & Barcode & "','" & MRP & "','" & Discount & "','0' from M_KitProductDetail;"

                    End If
                End If
            Next

            Dim a As Integer = objDAL.UpdateData(qry1)
            'If a <> 0 Then
            '    lblMsg.Text = "Product set for the selected Kit successfully."
            '    lblMsg.Visible = True
            '    lblMsg.ForeColor = Drawing.Color.Green
            'Else
            '    lblMsg.Text = "Product not set for the selected Kit"
            '    lblMsg.Visible = True
            '    lblMsg.ForeColor = Drawing.Color.Red
            'End If
            If a <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Product set for the selected Kit successfully.!! ');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Product not selected for the selected Kit!! ');" & "</SCRIPT>"
            End If

            ' scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        End If

        BindData()
        '
        'FillBarcode()
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
        Session("grdIndex") = GvData.PageIndex
        SetCheckBoxValue()
    End Sub

    Private Sub SetCheckBoxValue()
        Dim strlist As New DataTable()
        Dim dtPermission As New DataTable
        ' Dim i As String
        Dim qry2 As String = "Select 'false' as status, b.ProdId as ProdId,b.Qty,b.Barcode,b.MRP as Mrp,(b.MRP*b.Qty) as TotalAmt,"
        qry2 &= " b.DiscAmt,b.Tax "
        qry2 &= " from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster as a,"
        qry2 &= " M_KitProductDetail as b where  a.ProdId=b.ProdId and a.activeStatus='Y' and b.activeStatus='Y' and b.RowStatus='Y' and  a.Brandcode=0 and b.KitId='" & ddlGroup.SelectedValue & "'"
        dtPermission = objDAL.GetData(qry2)

        ' dtPermission.Rows.Count - 1


        'Setup checkbox
        For Each row As DataRow In dtPermission.Rows
            Dim Chk As New CheckBox
            Dim Lbl As New Label
            Dim TxtQty As New TextBox
            Dim DDlBarCode As New DropDownList
            Dim j As Integer = 0
            Dim i As Integer = dtPermission.Rows.Count
            LblTotalAmount1.Text = Val(dtPermission.Compute("sum(TotalAmt)", " ")) - Val(dtPermission.Compute("sum(DiscAmt)", " "))
            If GvData.Rows.Count > 0 Then

                For Each Gvr As GridViewRow In GvData.Rows
                    ' For j As Integer = 0 To i
                    If j < i Then
                        Chk = DirectCast(Gvr.FindControl("chkProd"), CheckBox)
                        Lbl = DirectCast(Gvr.FindControl("lblProdId"), Label)

                        If Lbl.Text = dtPermission.Rows(j)("ProdId").ToString Then
                            DirectCast(Gvr.FindControl("chkProd"), CheckBox).Checked = True
                            DirectCast(Gvr.FindControl("DDlBarcode"), DropDownList).SelectedValue = dtPermission(j)("BarCode").ToString
                            DirectCast(Gvr.FindControl("TxtQty"), TextBox).Text = dtPermission(j)("Qty").ToString
                            DirectCast(Gvr.FindControl("LblRate"), Label).Text = dtPermission(j)("MRP").ToString
                            DirectCast(Gvr.FindControl("LblAmt"), Label).Text = dtPermission(j)("TotalAmt").ToString
                            ''DirectCast(Gvr.FindControl("Tax"), TextBox).Text = dtPermission(j)("Tax").ToString
                            DirectCast(Gvr.FindControl("TxtDisc"), TextBox).Text = dtPermission(j)("DiscAmt").ToString

                            j += 1

                        Else
                            'DirectCast(Gvr.FindControl("chkProd"), CheckBox).Checked = False
                            'DirectCast(Gvr.FindControl("TxtQty"), TextBox).Text = 0

                        End If
                    Else
                        DirectCast(Gvr.FindControl("chkProd"), CheckBox).Checked = False
                        DirectCast(Gvr.FindControl("TxtQty"), TextBox).Text = 0


                    End If

                Next
            End If
        Next



    End Sub



    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
        Dim i As Integer
        Dim dtBarCode As New DataTable
        If e.Row.RowType = DataControlRowType.DataRow Then


            dtBarCode = Session("DtBarCode")

            Dim Dv As DataView

            Dim LblProdId As New Label
            LblProdId = DirectCast(e.Row.FindControl("lblProdId"), Label)
            Dv = New DataView(dtBarCode, "prodID='" & LblProdId.Text & "' ", "ExpDate,BID", DataViewRowState.CurrentRows)
            Dim ddlBarcode As New DropDownList

            ddlBarcode = DirectCast(e.Row.FindControl("DDlBarcode"), DropDownList)
            ddlBarcode.DataSource = Dv
            ' CmbKit.DataSource = Dt
            ddlBarcode.DataTextField = "Barcode"
            ddlBarcode.DataValueField = "BarCode"
            ddlBarcode.DataBind()
        End If
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        BindData()
        lblMsg.Visible = False
    End Sub


    'Protected Sub chkProd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim Chk As CheckBox
    '    For Each Gvr As GridViewRow In GvData.Rows
    '        Chk = DirectCast(Gvr.FindControl("chkProd"), CheckBox)
    '        If Chk.Checked Then
    '            DirectCast(Gvr.FindControl("TxtQty"), TextBox).Text = 1
    '        End If
    '    Next

    'End Sub

End Class
