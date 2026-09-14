
Imports System.Data
Imports System.Data.SqlClient

Partial Class AddSmartCard
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
        If String.IsNullOrEmpty(Request("CouponNo")) = False Then
            '   KitIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("KitId")))
            KitIdQS = Request("CouponNo")
        End If
        If Not Page.IsPostBack Then
            'Pages()
            'ClearAll()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("CouponNo")) = False Then
                    BtnSave.Text = "Use"
                    BindData()

                Else
                    '  Fill_SeriesStart()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        'txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub

    Private Sub Fill_SeriesStart()
        Dim Sql As String = "Select Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END as SrStart FROM M_KitMaster"
        Dim Dt As New DataTable
        Dt = objDAL.GetData(Sql)
        If Dt.Rows.Count > 0 Then
            'txtSerialStart.Text = Dt.Rows(0)("SrStart")
        End If
    End Sub

    Private Sub BindData()
        Dim sql As String = "select * from TrnSmartCoupon where Voucherno='" & KitIdQS & "' AND Isused='N'"
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        Dim Dat1 As String
        If Dt.Rows.Count > 0 Then
            ' TxtDate.Text = Dt.Rows(0)("UsedDate")
            Dat1 = Format(Date.Now, "dd-MMM-yyyy")
            
            ' rdblist.SelectedValue = Dt.Rows(0)("Isused")
        
        End If
    End Sub


   
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        Dim Str As String
        Dim KitId As String = ""
        Dim JoinColr As String = ""
        
       
        Sql = "Update TrnSmartCoupon set Isused='Y',UsedDate='" & TxtDate.Text & "' where Voucherno='" & Request("CouponNo") & "'"
       

            
        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("KitId")) = False And updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
        ElseIf updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        End If

        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub

    'Private Sub ClearAll()
    '    txtKitId.Text = ""
    '    txtkitName.Text = ""
    '    txtJoinAmt.Text = 0
    '    txtKitAmt.Text = 0
    '    txtKitUnit.Text = 0
    '    txtSerialStart.Text = ""
    '    txtRefIn.Text = 0
    '    txtPoolIn.Text = 0
    '    txtSpillIn.Text = 0
    '    txtBinaryIn.Text = 0
    '    txtBV.Text = 0
    '    txtPV.Text = 0
    '    txtRP.Text = 0
    '    txtCapping.Text = 0
    '    txtRemarks.Text = ""
    '    txtActiveStatus.Text = ""
    '    txtIPAdrs.Text = ""
    '    TxtTopUp.Text = 0
    'End Sub
End Class
