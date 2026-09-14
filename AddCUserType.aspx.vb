Imports System.Data
Imports System.Data.SqlClient
Partial Class AddCUserType
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim CTypeIdQS As String
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
        Try
            Dim str = "exec('Create table Trncomplain ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," & _
"ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trncomplain] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
            Dim i As Integer = 0
            i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
        Catch ex As Exception

        End Try
        If String.IsNullOrEmpty(Request("Type")) = False Then
            CTypeIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("Type")))
        End If
        If Not Page.IsPostBack Then
            HdnCheckTrnns.Value = GenerateRandomStringJoining(6)
            ClearAll()
            If Session("AStatus") = "OK" Then
                FillUser()
                If String.IsNullOrEmpty(Request("Type")) = False Then
                    BtnSave.Text = "Modify"
                    BindData()

                End If

            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub
    Public Function GenerateRandomStringJoining(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next
        Return sResult
    End Function

    Private Sub BindData()
        Dim sql As String = "Select a.*,isnull(b.Userid,0) as Cuserid From " + objDAL.tblCTypeMaster + " as a Left Join M_complaintusermaster as b " & _
        " on a.Ctypeid=b.ctypeid and b.activestatus='Y' and b.RowStatus='Y' where a.CTypeId='" & CTypeIdQS & "' AND a." + objDAL.activeCondition
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtCType.Text = Dt.Rows(0)("CType")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtCTypeID.Text = Dt.Rows(0)("CTypeId")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            TxtEmail.Text = Dt.Rows(0)("ToUserEmail")
            If Val(Dt.Rows(0)("cuserid")) <> "0" Then
                For i = 0 To Dt.Rows.Count - 1

                    For Each row As GridViewRow In GridView1.Rows

                        If Dt.Rows(i)("Cuserid") = DirectCast(row.FindControl("lblUserid"), Label).Text Then
                            TryCast(row.Cells(0).FindControl("chkRow"), CheckBox).Checked = True


                        End If

                                          Next

                Next
            End If
            'DDlUser.SelectedValue = Dt.Rows(0)("ToUserId")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub

    Private Sub FillUser()
        Dim strquery As String = ""
        Dim tmpTable As DataTable
        strquery = "SELECT UserId,Username FROM M_Usermaster WHERE ACTIVESTATUS='Y' And RowStatus='Y'  ORDER BY UserId"
        'dbConnect.OpenConnection()
        tmpTable = New DataTable
        tmpTable = objDAL.GetData(strquery)
        'dbConnect.Fill_Data_Tables(strQuery, tmpTable)
        GridView1.DataSource = tmpTable
        GridView1.DataBind()
    End Sub


    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Strquery = "Insert into Trncomplain (Transid) values(" & HdnCheckTrnns.Value & ")"
        Dim isOk1 As Integer = 0
        isOk1 = objDAL.SaveData(Strquery)
        If isOk1 > 0 Then



            Dim Sql As String = ""
            Dim str As String = ""
            If rdblist.SelectedIndex = 0 Then
                txtActiveStatus.Text = "Y"
            Else
                txtActiveStatus.Text = "N"
            End If

            For Each row As GridViewRow In GridView1.Rows
                Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkRow"), CheckBox)
                If chkRow.Checked Then
                    ' Dim name As String = row.Cells(1).Text
                    Dim Userid As String = DirectCast(row.FindControl("lblUserid"), Label).Text
                    ' TryCast(row.Cells(2).FindControl("lblUserid"), Label).Text
                    If String.IsNullOrEmpty(Request("Type")) = False Then
                        str = str & ";insert into M_ComplaintUsermaster(Uid,CtypeId,Userid,RectimeStamp,ActiveStatus,RowStatus)" & _
                    " select Isnull(Max(Uid),0)+1,'" & Val(CTypeIdQS) & "','" & Val(Userid) & "',GetDate(),'Y','Y' from M_ComplaintUsermaster ;"

                    Else
                        str = str & ";insert into M_ComplaintUsermaster(Uid,CtypeId,Userid,RectimeStamp,ActiveStatus,RowStatus)" & _
                    " select Isnull(Max(Uid),0)+1,(select isnull(Max(Ctypeid),1) from M_ComplaintTypeMaster),'" & Val(Userid) & "',GetDate(),'Y','Y' from M_ComplaintUsermaster ;"

                    End If

                End If
            Next

            If String.IsNullOrEmpty(Request("Type")) = False Then
                Sql = "Update " & objDAL.tblCTypeMaster & " SET RowStatus='N' Where CTypeId='" & CTypeIdQS & "';"
                Sql = Sql & " Insert into " & objDAL.tblCTypeMaster + "(CTypeId,CType,Remarks,ActiveStatus,LastModified,UserId,ToUserId,ToUserEmail) " & _
                " Values('" & Val(txtCTypeID.Text) & "','" & Trim(txtCType.Text) & "','" & Trim(txtRemarks.Text) & "','" & txtActiveStatus.Text & "'," & _
                " 'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Val(Session("UserID")) & "','" & Val(Session("UserID")) & "','" & Trim(TxtEmail.Text) & "')"
                Sql = Sql & ";Update M_ComplaintUserMaster Set ActiveStatus='N',LastModified=GetDate() where CtypeId='" & Val(CTypeIdQS) & "'"

            Else
                Sql = "Insert into " + objDAL.tblCTypeMaster + "(CTypeId,CType,Remarks,ActiveStatus,LastModified,UserId,RowStatus,ToUserId,ToUserEmail) " & _
                " Select Case When Max(CTypeId) Is Null Then '1' Else Max(CTypeId)+1 END as CTypeId,'" & Trim(txtCType.Text) & "'," & _
                " '" & Trim(txtRemarks.Text) & "','" & txtActiveStatus.Text & "','New by " & Session("UserName") & " at  " & _
                " " & DateTime.Now.ToString() & "','" & Val(Session("UserID")) & "','Y','" & Val(Session("UserID")) & "','" & Trim(TxtEmail.Text) & "' From " & objDAL.tblCTypeMaster
            End If
            'Dim Par As String = "@CType;@Remarks;@ToUserId;@ToUserEmail"
            'Dim ParVal As String = Trim(txtCType.Text) & ";" & Trim(txtRemarks.Text) & ";" & Val(0) & " ;" & Trim(TxtEmail.Text) & ""
            Dim updateEffect As Integer = 0
            Sql = Sql & str
            updateEffect = objDAL.UpdateData(Sql)

            If String.IsNullOrEmpty(Request("Type")) = False And updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
            ElseIf updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('This complaint already register.!');location.replace('UserComplaintType.aspx');", True)
            Exit Sub
        End If

        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub

    Private Sub ClearAll()
        txtCType.Text = ""
        txtCTypeID.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
        TxtEmail.Text = ""
    End Sub
End Class
