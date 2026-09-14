Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Partial Class TransferAmount
    Inherits System.Web.UI.Page
    'Dim _dblAvailLeg As Double = 0
    'Private cmd As New SqlCommand
    'Private dRead As SqlDataReader
    Dim objDAL As DAL
    'Private strQuery, strCaptcha As String
    'Dim tmpTable As New Data.DataTable
    '' Dim QryCls As New AccClass.MyAccClass.NewClass
    'Dim minSpnsrNoLen, minScrtchLen As Integer

    'Dim Upln, dblSpons, dblTehsil, dblDistrict, dblIdNo As Double
    'Dim CurrDt As DateTime
    'Dim montharray() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
    'Dim LastInsertID As Integer = 0
    Dim scrname As String
    Public formNo As String
    Dim dt As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Session("AStatus") = "OK" Then
            ' UserStatus.InnerHtml = "<p>Welcome " & Session("MemName") & "(" & Session("FormNo") & ") To" & Session("CompName") & " </p>"
        Else
            Response.Redirect("logout.aspx")
        End If

        If Not Page.IsPostBack Then

        End If

    End Sub
    Private Sub GetMemberName()
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim MemberName As String
        idNo = TextMemberId.Text
        Dim qry As String = "Select MemFirstName  from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            TextMemberName.Text = dt.Rows(0)("MemFirstName")
            'formNo = dt.Rows(0)("formNo")
            MemberName = TextMemberName.Text
        Else
            LblError.Text = "Member Id does not exist. Please check it once and then enter it again."
            LblError.Visible = True
            TextMemberId.Text = ""
        End If

    End Sub
    'Private Sub GetMembeName()
    '     objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '    Dim idNo As String
    '    Dim MemberName As String
    '    idNo = TextMemberId.Text
    '    Dim qry As String = "Select MemFirstName  from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
    '    Dim dt As New DataTable
    '    dt = objDAL.GetData(qry)
    '    If (dt.Rows.Count > 0) Then
    '        TextMemberName.Text = dt.Rows(0)("MemFirstName")
    '        'formNo = dt.Rows(0)("formNo")
    '        MemberName = TextMemberName.Text
    '        'Else
    '        '    lblError.Text = "Member Id does not exist. Please check it once and then enter it again."
    '        '    lblError.Visible = True
    '        '    txtMemberId.Text = ""
    '    End If

    'End Sub
    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        'Dim formno As String
        idNo = TextMemberId.Text
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formNo = dt.Rows(0)("FormNo")
        Else
            lblError.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblError.Visible = True
            TextMemberId.Text = ""
        End If
        Return formNo
    End Function

    Protected Sub TextMemberId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextMemberId.TextChanged

        GetMemberName()
    End Sub

    Protected Sub BtnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnTransfer.Click
        Dim query As String
        Dim formNo As String
        Dim voucherNo As String = ""
        formNo = GetFormNo()

        Dim sql As String = "select IsNull (Max(VoucherNo+1),1) as VoucherNo from TrnVoucher"
        Dim dt As New DataTable
        dt = objDAL.GetData(sql)
        If (dt.Rows.Count > 0) Then
            voucherNo = dt.Rows(0)("VoucherNo")

        End If
        Dim amoutNar As String = "Amount Transfer from " & Session("UserName") & " "
        query = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID)values" & _
"('" & voucherNo & "',Getdate(),0,'" & formNo & "', '" & TextAmount.Text & "',  '" & amoutNar & "',0,'P',GetDate(),'C',Convert(Varchar,GetDate(),112), " & Session("CurrentSessn") & ")"

        If objDAL.SaveData(query) <> 0 Then

            scrname = "<SCRIPT language='javascript'>alert('Amount Transfer Successfully!! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
            LblError.Text = "Amount Transfer Successfully!!"
        End If
    End Sub
End Class

