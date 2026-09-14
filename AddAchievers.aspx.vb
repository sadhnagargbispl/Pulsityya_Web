Imports System.Data

Partial Class App_UI_Application_Pages_AddAchievers
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim AcId As String

    ' Dim formNo As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        If String.IsNullOrEmpty(Request("AcId")) = False Then
            AcId = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("AcId")))
        End If
        If Not Page.IsPostBack Then

            ClearAll()
            Fillsession()
            If Session("AStatus") = "OK" Then

                If String.IsNullOrEmpty(Request("AcId")) = False Then
                    BtnSave.Text = "Modify"
                    BindData()
                End If
            End If
        Else

        End If
    End Sub

    Private Sub BindData()
        'Dim sessid As String
        'sessid = Session("CurrentSessn")
        Dim sql As String = "Select * From Achievers Where AcId= '" & AcId & "'"
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            TxtSession.Text = Dt.Rows(0)("SessId")
            txtMemberName.Text = Dt.Rows(0)("MemberName")
            TxtIdNo.Text = Dt.Rows(0)("IdNo")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub
    Protected Sub Fillsession()
        Dim sql As String
        sql = "select Top 1* from M_SessnMaster where ToDate is Not Null Order By SessId Desc;"
        Dt = objDAL.GetData(sql)
        If dt.Rows.Count > 0 Then
            TxtSession.Text = Dt.Rows(0)("SessId")
            TxtSession.Enabled = False

        Else
            TxtSession.Text = ""
            TxtSession.Enabled = True


        End If
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        Dim formno As String
        formNo = GetFormNo()
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If
        If String.IsNullOrEmpty(Request("AcId")) = False Then
            Sql = "Update Achievers set MemberName='" & txtMemberName.Text & "',FormNo='" & formno & "',IdNo='" & TxtIdNo.Text & "',ActiveStatus='" & txtActiveStatus.Text & "' Where AcId = '" & AcId & "' "
            ' Sql = Sql & " insert into " & objDAL.tblBankMaster & " (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) values('" & Val(txtBankCode.Text) & "','" & txtBankName.Text & "','" & txtAccountNo.Text & "','" & txtIFSCode.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y') "
        Else
            Sql = "insert into Achievers  (AcId,SessId,FormNo,IdNo,MemberName,ActiveStatus,RecTimeStamp )Select Case When Max(AcId) Is Null Then '1' Else Max(AcId)+1 END as AcId,'" & Val(TxtSession.Text) & "','" & formno & "','" & TxtIdNo.Text & "','" & txtMemberName.Text & "','" & txtActiveStatus.Text & "',Getdate() from Achievers"
        End If
        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("AcId")) = False And updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
        ElseIf updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        End If

        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub
    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        LblError.Visible = False
        Dim formno As String
        idNo = TxtIdNo.Text
        'gvContainer.Visible = False
        'divright.Visible = False
        Dim qry As String = "Select FormNo,MemFirstName from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
            ' txtMemberName.Text = dt.Rows(0)("MemFirstName")
        Else
            LblError.Text = "Member Id does not exist. Please check it once and then enter it again."
            LblError.Visible = True
            TxtIdNo.Text = ""
            'txtMemberName.Text = ""
        End If
        Return formno
    End Function
    Private Sub ClearAll()
        txtBankCode.Text = ""
        TxtIdNo.Text = ""
        txtMemberName.Text = ""
        TxtSession.Text = ""

        'txtBankName.Text = ""
        'txtAccountNo.Text = ""
        'txtIFSCode.Text = ""
        'txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
    End Sub

    Protected Sub TxtIdNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIdNo.TextChanged
        ' Dim form As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        LblError.Visible = False
        Dim formno As String
        idNo = TxtIdNo.Text
        'gvContainer.Visible = False
        'divright.Visible = False
        Dim qry As String = "Select FormNo,MemFirstName from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formNo = dt.Rows(0)("FormNo")
            txtMemberName.Text = dt.Rows(0)("MemFirstName")
        Else
            LblError.Text = "Member Id does not exist. Please check it once and then enter it again."
            LblError.Visible = True
            TxtIdNo.Text = ""
            'txtMemberName.Text = ""
        End If

    End Sub
End Class
