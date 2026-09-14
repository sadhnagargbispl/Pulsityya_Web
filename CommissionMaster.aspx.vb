Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class CommissionMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "User / Commission Master"

        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

       
            objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                lblMsg.Visible = False
               
                BindData()
               
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindData()
        Try

       
            'Dim strlist As New List(Of String)
            Dim qry1 As String = "Select ClubId,Club,L1,L2,L3,L4,L5 from M_ClubMaster Order by clubid"
            dtData = New DataTable
            dtData = objDAL.GetData(qry1)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
        Catch ex As Exception

        End Try
      
    End Sub

  

   
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
        Session("grdIndex") = GvData.PageIndex
        ' SetCheckBoxValue()
    End Sub

   
    


    Protected Sub UpdateCommission(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

      
            Dim ClubId, scrname, L1, L2, l3, l4, l5, Club As String
            Dim GVRw As GridViewRow
            GVRw = CType(sender.Parent.Parent, GridViewRow)
            'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
            ClubId = DirectCast(GVRw.FindControl("LblClubID"), Label).Text

            Club = DirectCast(GVRw.FindControl("LblClub"), Label).Text
            L1 = DirectCast(GVRw.FindControl("TxtL1"), TextBox).Text
            L2 = DirectCast(GVRw.FindControl("TxtL2"), TextBox).Text
            l3 = DirectCast(GVRw.FindControl("TxtL3"), TextBox).Text
            l4 = DirectCast(GVRw.FindControl("TxtL4"), TextBox).Text
            l5 = DirectCast(GVRw.FindControl("TxtL5"), TextBox).Text
            Dim Sql As String = "Insert Into TempClubMaster Select *,Getdate(),'" & Session("Userid") & "' From M_CLUBMaster Where ClubId ='" & Val(ClubId) & "'"
            Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
       "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Commission','Commision Update','Commission Update of " & Club & " ',Getdate(),'0')"

            Sql = Sql & "Update M_clubMaster SET L1='" & Val(L1) & "',L2='" & Val(L2) & "',L3='" & Val(l3) & "',l4='" & Val(l4) & "',l5='" & Val(l5) & "' where ClubId='" & Val(ClubId) & "'"
            Dim K As String = " Begin Try Begin Transaction " & Sql & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH      "
            Dim updateEffect As Integer = objDAL.UpdateData(K)
            If updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Commission Update Successfully!');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Not able to Update the Commission! ');" & "</SCRIPT>"
            End If
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Update Commision", scrname, False)
            BindData()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try

        Catch ex As Exception

        End Try
    End Sub
End Class
