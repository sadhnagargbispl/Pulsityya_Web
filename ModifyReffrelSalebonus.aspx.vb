Imports System.Data
Imports System.IO
Partial Class ModifyReffrelSalebonus
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim tmpTable As New Data.DataTable
    'Dim tmpTable As Data.DataTable
    Dim ProductCodeQS As String
    Private dbConnect As cls_DataAccess
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("m_id")) = False Then
            ProductCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("m_id")))
            Session("m_id") = ProductCodeQS
        End If
        If Not Page.IsPostBack Then
            ClearAll()
            If Session("AStatus") = "OK" Then
                 If String.IsNullOrEmpty(Request("m_id")) = False Then
                    BtnUpdate.Text = "Modify"
                    BindData()


                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        'txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
        'BindData()

    End Sub


    Protected Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click
        Dim Sql, PId As String
        Dim FlNm As String = ""
        Dim ImgFl1 As String = ""
        Dim ImgFl2 As String = ""
        Dim DocPath As String = ""
        Dim ImageType As String = ""
        Dim str As String = ""
        Dim dt As New DataTable
        

        If String.IsNullOrEmpty(Request("PId")) = True Then
            Sql = "Update M_ReferralSalesBonus set Remark='" & txtRemark.Text & "',callername='" & TxtCallerName.Text & "',status='" & rdblist.SelectedValue & "' where m_id='" & Val(Session("m_id")) & "'"
        End If
        Dim updateEffect As Integer = 0
        updateEffect = objDAL.SaveData(Sql)

        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
            'Exit Sub
           Else
         scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        End If
        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)

    End Sub

    Private Sub ClearAll()
        txtRemark.Text = ""
        TxtCallerName.Text = ""
    End Sub

    

    Private Sub BindData()

        '         select b.memfirstname as membername,b.idno as idno, 
        ' replace(convert(varchar,a.rectimestamp,106),' ','-') as Date1,
        ' c_name as cname, c_mobileno as mobile ,healthissue as hlissue,
        ' Case when Status='A' then 'Close' else 'Pending' end as Status,callername,remark
        'from M_ReferralSalesBonus as a ,
        ' m_membermaster as b Where  a.formno = b.formno 

        Dim sql As String = ""
        sql = "select b.memfirstname as membername,b.idno as idno,replace(convert(varchar,a.rectimestamp,106),' ','-') as Date1, " & _
        " c_name as cname, c_mobileno as mobile ,healthissue as hlissue, Case when Status='A' then 'Close' else 'Pending' end as Status,callername,remark " & _
        " from M_ReferralSalesBonus as a ,  m_membermaster as b Where  a.formno = b.formno AND m_id='" & ProductCodeQS & "'"

        'Dim sql As String = "Select * From M_ReferralSalesBonus Where m_id='" & ProductCodeQS & "'  "
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            
            'RbtEventType.SelectedValue = Dt.Rows(0)("EventType")
            membername.Text = Dt.Rows(0)("membername")
            idno.Text = Dt.Rows(0)("idno")
            Date1.Text = Dt.Rows(0)("Date1")
            cname.Text = Dt.Rows(0)("cname")
            mobile.Text = Dt.Rows(0)("mobile")
            hlissue.Text = Dt.Rows(0)("hlissue")
            Status.Text = Dt.Rows(0)("Status")
            TxtCallerName.Text = Dt.Rows(0)("callername")
            txtRemark.Text = Dt.Rows(0)("Remark")
            'BtnUpdate.Visible = False
        End If
    End Sub
End Class
