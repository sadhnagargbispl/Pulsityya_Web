Imports System.Data
Imports System.Data.SqlClient
Partial Class EMIDeposit
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim scrname As String = ""
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Player Master"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack And Request.QueryString.HasKeys Then
                Dim Key As Integer = Val(Crypto.Decrypt(Replace(Request.QueryString("key"), " ", "+")))

                LblDeviceId.Text = Key

                Dim FormNo As Integer = Val(Crypto.Decrypt(Replace(Request.QueryString("FormNo"), " ", "+")))
                LblFormNo.Text = FormNo
                Dim EmiMonth As Integer = Val(Crypto.Decrypt(Replace(Request.QueryString("EmiMonth"), " ", "+")))
                LblFormNo.Text = EmiMonth
                BindData(Key)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

      
            Dim StateCode, scrname As String
            Dim GVRw As GridViewRow
            GVRw = CType(sender.Parent.Parent, GridViewRow)
            'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
            StateCode = DirectCast(GVRw.FindControl("Reqno"), Label).Text
            Dim remark1 As String = DirectCast(GVRw.FindControl("TextBox1"), TextBox).Text
            Dim remark As String = DirectCast(GVRw.FindControl("TxtRemark"), TextBox).Text
            Dim formno As String = DirectCast(GVRw.FindControl("LblFormno"), Label).Text
            Dim amount As String = DirectCast(GVRw.FindControl("LblAmount"), Label).Text
            'Dim RecieptNo As String = DirectCast(GVRw.FindControl("LblRecieptNo"), Label).Text
            Dim RecieptNo As String = DirectCast(GVRw.FindControl("Reqno"), Label).Text
            Dim Sql As String = "Update TrnEMi SET ActiveStatus='Y',Remark='" & remark & "',Reqdate=Getdate(),RecieptNo='" & RecieptNo & "' WHERE Reqno='" & Val(StateCode.ToString()) & "' "
            Sql = Sql & " Insert into Repurchincome(SessId,FormNo,BillNo,BillDate,RepurchIncome,Imported,BillType," & _
            " SoldBy,Msessid,KitID,Remarks,DSessID,PVValue,RecTimeStamp,RpValue,RBV,Amount)" & _
            " Values('" & Session("CurrentSessn") & "','" & formno & "','EMI " & Val(LblDeviceId.Text) & "',Getdate()," & _
            " '" & amount & "','N','R','','" & Session("CurrentSessn") & "',0,'EMI Paid Against ReqNo " & Val(LblDeviceId.Text) & "'," & _
            " Convert(Varchar,Getdate(),112),0,getdate(),0,0,'" & amount & "');"
            Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','EMI Deposit ','EMI Deposit Against Reqno " & LblDeviceId.Text & " ','EMI Deposit Against Reqno " & LblDeviceId.Text & " ',Getdate(),'" & formno & "')"


            Dim updateEffect As Integer = objDAL.UpdateData(Sql)
            If updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('EMI Deposit Successfuly!');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Not able to deposit EMI! ');" & "</SCRIPT>"
            End If
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
            BindData(LblDeviceId.Text)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ViewDetails(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            BindDataNew(LblDeviceId.Text, LblRefno.Text, LblFormNo.Text, LblEmimonth.Text)
            
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindData(ByVal DeviceID As String)
        Try
            Dim _Condition As String = ""

            Dim Qry As String = "Select Drto as Formno ,Reqno,RefNo,RecieptNo,EmiMonth,Amount,Case when ActiveStatus='Y' and Drto<>0 then 'Paid' else 'Pending' end as Status, " & _
                        " Case when ActiveStatus='Y' and Drto<>0 then 'False' else 'True' end as VisibleStatus," & _
                        " Case when ActiveStatus='Y' and Drto<>0 then 'True' else 'False' end as NewStatus," & _
                        " Case when ActiveStatus='Y' then Remark else '' end as Remark,Case when ActiveStatus='Y' then " & _
                        " Replace(Convert(Varchar,ReqDate,106),' ','-') else '' end As PaidDate" & _
                        " from TrnEMI  where RefNo='EMI/" & DeviceID & "' order by EMIMonth"
            dtData = New DataTable
            dtData = objDAL.GetData(Qry)

            dtData.AcceptChanges()
            GvData.DataSource = dtData
            GvData.DataBind()
            

        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindDataNew(ByVal ReqNo As String, ByVal RefNo As String, ByVal FormNo As String, ByVal EmiMonth As Integer)
        Try
            Dim _Condition As String = ""

            Dim Qry As String = "Select Drto as Formno ,Reqno,RefNo,RecieptNo,EmiMonth,Amount,Case when ActiveStatus='Y' and Drto<>0 then 'Paid' else 'Pending' end as Status, " & _
                        " Case when ActiveStatus='Y' and Drto<>0 then 'False' else 'True' end as VisibleStatus," & _
                        " Case when ActiveStatus='Y' and Drto<>0 then 'True' else 'False' end as NewStatus," & _
                        " Case when ActiveStatus='Y' then Remark else '' end as Remark,Case when ActiveStatus='Y' then " & _
                        " Replace(Convert(Varchar,ReqDate,106),' ','-') else '' end As PaidDate" & _
                        " from TrnEMI  where RefNo='EMI/" & RefNo & "' order by EMIMonth"
            dtData = New DataTable
            dtData = objDAL.GetData(Qry)

            dtData.AcceptChanges()
            GvData.DataSource = dtData
            GvData.DataBind()

        Catch ex As Exception

        End Try
    End Sub

End Class
