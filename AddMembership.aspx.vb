Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddMembership
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim KitIdQS As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("Id")) = False Then
            '   KitIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("KitId")))
            KitIdQS = Request("Id")
        End If
        If Not Page.IsPostBack Then
            'Pages()
            FillpersonDDL()
            FillyearDDL()
            ClearAll()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("Id")) = False Then
                    BtnSave.Text = "Modify"
                    BindData()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
    End Sub
#Region "Fill personcount"
    Private Sub FillpersonDDL()
        Dim qry1 As String = "Select * from MembershipNumbers"
        objModuleFun.FillCombo(qry1, ddlPersoncount, "Value", "Id")
    End Sub
    Private Sub FillpersonselectDDL()
        Dim qry1 As String = "Select * from MembershipNumbers  where Value=" & ddlPersoncount.SelectedItem.Text & ""
        objModuleFun.FillCombo(qry1, ddlPersoncount, "Value", "Id")
    End Sub
#End Region
#Region "Fill yearcount"
    Private Sub FillyearDDL()
        Dim qry1 As String = "Select * from Numbers10Years"
        objModuleFun.FillCombo(qry1, ddlyear, "Value", "Id")
    End Sub
    Private Sub FillyearselectDDL()
        Dim qry1 As String = "Select * from Numbers10Years where Value=" & ddlyear.SelectedItem.Text & ""
        objModuleFun.FillCombo(qry1, ddlyear, "Value", "Id")
    End Sub
#End Region
    Private Sub BindData()
        Dim sql As String = "Select * From MembershipPlans Where Id='" & KitIdQS & "' "
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        Dim Dat1 As String
        If Dt.Rows.Count > 0 Then
            txtKitId.Text = Dt.Rows(0)("Id")

            ddlPersoncount.SelectedItem.Text = Dt.Rows(0)("PersonCount")
            ddlPersoncount.Enabled = False
            FillpersonselectDDL()
            ddlyear.SelectedItem.Text = Dt.Rows(0)("YearDuration")
            ddlyear.Enabled = False
            FillyearselectDDL()
            txtmrp.Text = Dt.Rows(0)("MRP")
            txtdiscount.Text = Dt.Rows(0)("DiscountPercent")
            txtdiscountamt.Text = Dt.Rows(0)("DiscountAmount")
            txtofferprice.Text = Dt.Rows(0)("OfferPrice")
          
        End If
    End Sub


    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        Dim Str As String
        Dim KitId As String = ""
        Dim JoinColr As String = ""
       
        If String.IsNullOrEmpty(Request("Id")) = False Then
            FillpersonselectDDL()
            FillyearselectDDL()
            Sql = "Update MembershipPlans set PersonCount='" & Val(ddlPersoncount.SelectedItem.Text) & "',YearDuration='" & Val(ddlyear.SelectedItem.Text) & "',MRP='" & Val(txtmrp.Text) & "',DiscountPercent='" & Val(txtdiscount.Text) & "',DiscountAmount='" & Val(txtdiscountamt.Text) & "',OfferPrice='" & Val(txtofferprice.Text) & "'" & _
            ",rectimestamp=getdate() where Id='" & Val(txtKitId.Text) & "'"
        Else
            Sql = " insert into MembershipPlans(PersonCount,YearDuration,MRP,DiscountPercent,DiscountAmount,OfferPrice,CashbackPercent,NetworkPercent,KejoksPercent)" & _
            " values('" & Val(ddlPersoncount.SelectedItem.Text) & "','" & Val(ddlyear.SelectedItem.Text) & "','" & Val(txtmrp.Text) & "','" & Val(txtdiscount.Text) & "','" & Val(txtdiscountamt.Text) & "','" & Val(txtofferprice.Text) & "',20.00,0.50,20.00)"

        End If


        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("Id")) = False And updateEffect <> 0 Then
            'scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
            scrname = "<script type='text/javascript'>alert('Successfully Updated!!');if (parent && parent.hs) {parent.location.reload(); parent.hs.close();}</script>"
        ElseIf updateEffect <> 0 Then
            'scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
            scrname = "<script type='text/javascript'>alert('Save Successfully!!');if (parent && parent.hs) {parent.location.reload(); parent.hs.close();}</script>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "ClosePopup", scrname, False)
        'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub

    Private Sub ClearAll()
        txtdiscountamt.Text = ""
        txtmrp.Text = ""
        txtdiscount.Text = ""
        txtofferprice.Text = ""
    End Sub

    Protected Sub txtdiscount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtdiscount.TextChanged
        If Not String.IsNullOrEmpty(txtmrp.Text) AndAlso Not String.IsNullOrEmpty(txtdiscount.Text) Then
            Dim mrp As Decimal = Convert.ToDecimal(txtmrp.Text)
            Dim discountPercent As Decimal = Convert.ToDecimal(txtdiscount.Text)

            ' Discount Amount निकालना
            Dim discountAmt As Decimal = (mrp * discountPercent) / 100
            txtdiscountamt.Text = discountAmt.ToString("0.00")

            ' Offer Price निकालना
            Dim offerPrice As Decimal = mrp - discountAmt
            txtofferprice.Text = offerPrice.ToString("0.00")
        End If
    End Sub
End Class
