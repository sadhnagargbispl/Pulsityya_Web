Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Services
Partial Class App_UI_Application_Pages_viewmember
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral
    Dim VId As Integer

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Member / Visiting Verify"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'objDAL = New DAL()
            'objModuleFun = New ModuleFunction()
            If Not Page.IsPostBack Then

                If String.IsNullOrEmpty(Request("VId")) = False Then
                    'VId = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("VId")))
                    VId = Request("VId")

                    BindData()
                    DataGrid()
                End If

            Else

            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindData()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim sql As String = "select b.Name As MemName,b.Age,b.Pincode,b.CityName,b.stateName,"
            sql &= "Isnull(Convert(Varchar,b.VerifyDate),'') as VerifyDate ,Isnull(b.VerifyBy,'') as VerifyBy ,"
            sql &= " (Case When b.ActiveStatus = 'N' Then 'Pending' "
            sql &= " When b.ActiveStatus = 'R' Then 'Rejected' "
            sql &= " When b.ActiveStatus = 'Y' Then 'Approved'  End ) As  Status,"
            sql &= " Replace(Convert(Varchar,b.VisitingDate,106),' ','-') As rectimestamp"
            sql &= "  From M_VisitingMaster as a,TrnVisitingMaster as b  "
            sql &= " Where a.VId=b.Vid and b.VId='" & VId & "' "

            Dim Dt1 = New DataTable
            Dt1 = objDAL.GetData(sql)
            GvData.DataSource = Dt1
            'If Dt1.Rows.Count > 0 Then
            '    Lblmemname.Text = Dt1.Rows(0)("InstituteName")
            '    LblCityname.Text = Dt1.Rows(0)("CityName")
            '    'lblcourseName.Text = Dt1.Rows(0)("CourseName")
            '    lblId.Text = Dt1.Rows(0)("Id")
            'End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub DataGrid()
        Try
            Dim sql As String = "select b.VId,b.Name As MemName,b.Age,b.Pincode,b.CityName,b.stateName,"
            sql &= "Isnull(Convert(Varchar,b.VerifyDate),'') as VerifyDate ,Isnull(b.VerifyBy,'') as VerifyBy ,"
            sql &= " (Case When b.ActiveStatus = 'N' Then 'Pending' "
            sql &= " When b.ActiveStatus = 'R' Then 'Rejected' "
            sql &= " When b.ActiveStatus = 'Y' Then 'Approved'  End ) As  Status,"
            sql &= " Replace(Convert(Varchar,b.VisitingDate,106),' ','-') As rectimestamp,VisitingDate"
            sql &= "  From M_VisitingMaster as a,TrnVisitingMaster as b  "
            sql &= " Where a.VId=b.Vid and b.VId='" & VId & "' "

            Dim Dt1 = New DataTable
            Dt1 = objDAL.GetData(sql)

            GvData.DataSource = Dt1
            GvData.DataBind()
            Session("GData") = Dt1
            If Dt1.Rows.Count > 0 Then

            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class
