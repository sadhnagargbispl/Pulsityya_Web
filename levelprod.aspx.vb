Imports System.Data
Imports System.Data.SqlClient

Partial Class levelprod
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim Ds As DataSet
    Dim objModuleFun As ModuleFunction
    Dim KitIdQS As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Private Sub Fillproduct()

        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_Fillproduct")
            DDlprod.DataSource = Ds.Tables(0)
            DDlprod.DataValueField = "prodid"
            DDlprod.DataTextField = "productname"
            DDlprod.DataBind()
        Catch ex As Exception
        End Try
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("prodId")) = False Then
            '   KitIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("KitId")))
            KitIdQS = Request("prodId")
        End If
        If Not Page.IsPostBack Then

            ClearAll()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("prodId")) = False Then
                    BtnSave.Text = "Modify"
                    Fillproducta()
                    BindData()

                Else
                    Fillproduct()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub
    Private Sub Fillproducta()

        Try
            Dim sql As String = "select * from decebalinv..m_productmaster where prodid='" & KitIdQS & "'"
            Dim Dt As New DataTable
            Dt = objDAL.GetData(Sql)
            If Dt.Rows.Count > 0 Then
                DDlprod.DataSource = Dt
                DDlprod.DataValueField = "prodid"
                DDlprod.DataTextField = "productname"
                DDlprod.DataBind()
            End If
        Catch ex As Exception
        End Try
    End Sub
    

    Private Sub BindData()
        Dim sql As String = " select a.Rectimestamp,a.prodid,ProductName,level_1,level_2,level_3,level_4,level_5,level_6,level_7,level_8,level_9,level_10,b.ActiveStatus  from M_prodLevelMaster as b left join decebalinv..M_productMaster as a on a.prodid=b.prodid Where a.prodId='" & KitIdQS & "' "
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        Dim Dat1 As String
        If Dt.Rows.Count > 0 Then
            LblDate.Text = Format(Dt.Rows(0)("Rectimestamp"), "dd-MMM-yyyy")
            Dat1 = Format(Date.Now, "dd-MMM-yyyy")
            txtlevelone.Text = Dt.Rows(0)("level_1")
            txtleveltwo.Text = Dt.Rows(0)("level_2")
            txtlevelthr.Text = Dt.Rows(0)("level_3")
            txtlevelfour.Text = Dt.Rows(0)("level_4")
            txtlevelfiv.Text = Dt.Rows(0)("level_5")
            txtlevelsi.Text = Dt.Rows(0)("level_6")
            txtlevelsev.Text = Dt.Rows(0)("level_7")
            txtlevele.Text = Dt.Rows(0)("level_8")
            txtleveln.Text = Dt.Rows(0)("level_9")
            txtleveltn.Text = Dt.Rows(0)("level_10")
          
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub


    

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        Dim Str As String
        Dim KitId As String = ""
        Dim JoinColr As String = ""
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If
       
        If String.IsNullOrEmpty(Request("prodId")) = False Then
            Sql = Str & ";Update [M_prodLevelMaster] set level_1='" & txtlevelone.Text & "',level_2='" & Val(txtleveltwo.Text) & "',level_3='" & Val(txtlevelthr.Text) & "',level_4='" & Val(txtlevelfour.Text) & "',level_5='" & Val(txtlevelfiv.Text) & "',level_6='" & Val(txtlevelsi.Text) & "',level_7='" & Val(txtlevelsev.Text) & "',level_8='" & Val(txtlevele.Text) & "',level_9='" & Val(txtleveln.Text) & "',level_10='" & Val(txtleveltn.Text) & "',ActiveStatus='" & txtActiveStatus.Text & "'" & _
            " where prodId='" & KitIdQS & "'"

        Else
            Str = " select * from  decebalinv..M_productMaster as a  where prodid  in(select prodid from M_prodLevelMaster) and prodId='" & DDlprod.SelectedValue & "'   "
            Dt = New DataTable
            Dt = objDAL.GetData(Str)
            If Dt.Rows.Count > 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Already Inserted You Can Edit It!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            Else
                Sql = " insert into [M_prodLevelMaster] (prodid,level_1,level_2,level_3,level_4,level_5,level_6,level_7,level_8,level_9,level_10,ActiveStatus,RecTimeStamp)" & _
                " values('" & DDlprod.SelectedValue & "','" & Val(txtlevelone.Text) & "','" & Val(txtleveltwo.Text) & "','" & Val(txtlevelthr.Text) & "'," & _
                " '" & Val(txtlevelfour.Text) & "','" & Val(txtlevelfiv.Text) & "','" & Val(txtlevelsi.Text) & "','" & Val(txtlevelsev.Text) & "','" & Val(txtlevele.Text) & "','" & Val(txtleveln.Text) & "','" & Val(txtleveltn.Text) & "','Y',getdate());"
            End If

        End If
        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("prodId")) = False And updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
        ElseIf updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        End If

        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub

    Private Sub ClearAll()
        txtprodId.Text = ""
        txtlevelone.Text = ""
        txtleveltwo.Text = ""
        txtlevelthr.Text = ""
        txtlevelfour.Text = ""
        txtlevelfiv.Text = ""
        txtlevelsi.Text = ""
        txtlevelsev.Text = ""
        txtlevele.Text = ""
        txtleveln.Text = ""
        txtleveltn.Text = ""
      
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""

    End Sub
End Class
