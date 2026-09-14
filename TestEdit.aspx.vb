
Imports System.Data
Imports System.Data.SqlClient
Partial Class App_UI_Application_Pages_TestEdit
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    'Dim objDAL As New DAL
    'Dim objModuleFun As ModuleFunction
    Dim GroupIdQS As String

    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("~\Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim objDAL As DAL
        'Dim objGen As clsGeneral = New clsGeneral
        'objDAL = New DAL()
        'objModuleFun = New ModuleFunction()
        'If String.IsNullOrEmpty(Request("ProdId")) = False Then
        '    GroupIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("ProdId")))
        'End If
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                'If String.IsNullOrEmpty(Request("ProdId")) = False Then
                '    BtnSave.Text = "Modify"
                BindData()

            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()

    End Sub

  
    

    Private Sub BindData()
        Dim sql As String = " select a.AID,a.FormNo,a.Descriptions,b.IDNo,b.MemFirstname,a.ApprovedDate" & _
     " from M_TestmonialsMaster as a,M_MemberMaster as b where a.formNo=b.FormNo and Aid='" & Request("Aid") & "' "
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtdesc.Text = Dt.Rows(0)("Descriptions")
            'txtheading.Text = Dt.Rows(0)("Heading")
            txtmemid.Text = Dt.Rows(0)("IDNo")
            'img.ImageUrl = Dt.Rows(0)("ImageLnk")
        End If
          
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        Dim FlNm As String = ""
      
       
       
        Sql = "insert into TempTestmonials select * ,Getdate(),'" & Session("UserID") & "' from M_TestmonialsMaster where Aid='" & Request("Aid") & "'"
        Sql = Sql & " update M_TestmonialsMaster set Descriptions='" & txtdesc.Text & "' ,updatedate=Getdate(),UpdateUSerid='" & Session("UserID") & "'   where Aid='" & Request("Aid") & "' "
        If objDAL.SaveData(Sql) <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Testimonial Updated  Successfully.');" & "</SCRIPT>"

            BindData()
        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)

      

        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub

    'Private Sub ClearAll()
    '    txtProdName.Text = "" : TxtRate.Text = "0" : TxtTax.Text = "0" : TxtTaxAmt.Text = "0" : TxtQty.Text = "0" : TxtAmount.Text = "0"
    '    txtGrpID.Text = ""
    '    txtRemarks.Text = ""
    '    txtActiveStatus.Text = ""
    '    txtIPAdrs.Text = ""
    'End Sub

End Class
