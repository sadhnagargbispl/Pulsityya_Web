Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_MemberDownlinePurchase
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim dt As DataTable
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try


            If Session("AStatus") = "OK" Then
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Not Page.IsPostBack Then
                FillLevel()
                GvData.Visible = False
                gvContainer.Visible = False
                Session("RewardList") = Nothing
                'FillTotal()

            End If
        Catch ex As Exception

        End Try
    End Sub





    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        Try


            GvData.PageIndex = e.NewPageIndex
            GvData.DataSource = Session("RewardList")
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            If txtMember.Text <> "" Then
                Condition = Condition & " d.Formno='" & Val(LblFormno.Text) & "'"
            Else
                scrName = "<SCRIPT language='javascript'>alert('Member Id does not exist .. ');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrName)
                Exit Sub
            End If

            If RbtProduct.SelectedValue = "R" Then
                If DdlLevel.SelectedValue > 0 Then
                    Condition = Condition & "AND d.MLevel='" & DdlLevel.SelectedValue & "'"
                End If
            Else
                If RbtLegNo.SelectedValue <> "0" Then
                    Condition = Condition & " and d.LegNo='" & Val(RbtLegNo.SelectedValue) & "'"
                End If
            End If


            Dim FrmDate As String = txtStartDate.Text
            Dim ToDate As String = txtEndDate.Text
            If FrmDate <> "" Then


                Try
                    Dim Dt As DateTime = FrmDate
                Catch ex As Exception
                    scrName = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" & "</SCRIPT>"
                    Me.RegisterStartupScript("MyAlert", scrName)
                    Exit Sub
                End Try
            End If
            If ToDate <> "" Then
                Try
                    Dim Dt As DateTime = ToDate
                Catch ex As Exception
                    scrName = "<SCRIPT language='javascript'>alert('Check End Date.. ');" & "</SCRIPT>"
                    Me.RegisterStartupScript("MyAlert", scrName)
                    Exit Sub
                End Try
            End If
            If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then


                Condition = Condition & " And  Cast(Convert(Varchar,b.BillDate,106)as DateTime)>='" & FrmDate & "' And  Cast(Convert(Varchar,b.BillDate,106)as DateTime)<='" & ToDate & "'"
            End If
            Dim qry1 As String = ""
            If RbtProduct.SelectedValue = "R" Then
                If (Session("CompID") = "1010") Then
                    '   qry1 = " select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno,(a.MemFirstName+ ' '+a.MemLastName)As " & _
                    '  " [Member Name], d.Mlevel as [Level],Isnull(c.OrderAmt,s.NetPayable)as [Bill Amount],b.RepurchIncome as [BV] " & _
                    '  " from M_MemberMaster as a with(nolock) inner Join  R_MemTreeRelation as d with(nolock)" & _
                    '  " on a.Formno=d.FormnoDwn Inner Join RepurchIncome as b with(nolock) on  a.Formno=b.Formno " & _
                    '  " Left Join TrnOrder as c with(nolock)  On   b.Formno=c.formno" & _
                    '  " Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo  where  1=1 " & Condition & " and " & _
                    '" a.Formno=b.Formno and b.BillType='R'  And c.ActiveStatus <> 'D'   Order by Year(b.BillDate) Desc,Month(b.BillDate)Desc,Day(b.BillDate)Desc,Mlevel"
                    qry1 = " exec Sp_DowmLineRepurchaseReport '" & LblFormno.Text & "','" & FrmDate & "','" & ToDate & "' "
                    '        qry1 = "  select a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," & _
                    '" Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " & _
                    '" Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Bill Amount],k.KitName as [Package Name],Repurchincome as BV   from " & _
                    '" M_MemberMaster as a with(nolock) Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " & _
                    '" inner Join M_KitMaster as k with(nolock)on  k.RowStatus='Y'," & _
                    '" RepurchIncome as b with(nolock) Left Join TrnOrder as c with(nolock)   On   " & _
                    '" c.Formno=b.Formno   Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " & _
                    '" and s.BillNo=b.BillNo  where  b.Kitid=k.KitId and " & Condition & " and " & _
                    '" a.Formno=b.Formno and b.BillType='R' And c.ActiveStatus <> 'D' "

                    '        qry1 &= " union All "
                    '        qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    '        qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    '        qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    '        qry1 &= " Isnull(b.Amount,0) as [Bill Amount],'Real State BV'as [Package Name],Repurchincome as BV  "
                    '        qry1 &= " from  M_MemberMaster as a with(nolock) "
                    '        qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    '        qry1 &= " Inner join RepurchIncome b  on d.FormnoDwn  = b.Formno And RBV = 'N' and b.BillType='R'"
                    '        qry1 &= " Where 1 =1 and " & Condition & " "

                Else
                    qry1 = "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as BillDate ,a.Idno,(a.MemFirstName+ ' '+a.MemLastName)As MemberName," & _
              " b.RepurchIncome as RepurchaseBv ,d.Mlevel, " & _
              " Isnull(c.OrderAmt,s.NetPayable)as BillAmount from M_MemberMaster as a,R_MemTreeRelation as d," & _
              " RepurchIncome as b Left Join TrnOrder as c   On  ('Order '+ Cast( OrderNo as Varchar))=b.BillNo and  " & _
              " c.Formno=b.Formno   Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s On  b.Formno=s.Formno " & _
              " and s.BillNo=b.BillNo  where a.Formno=d.FormnoDwn and " & Condition & " and " & _
              " a.Formno=b.Formno and b.BillType='R'    Order by Year(b.BillDate) Desc,Month(b.BillDate)Desc,Day(b.BillDate)Desc,Mlevel"
                End If


            Else



                If (Session("CompID") = "1010") Then
                    '       qry1 = "  select a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," & _
                    '" Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " & _
                    '" Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name],Repurchincome as BV,B.PVValue as PV ,Case when Repurchincome>0 then IsNull( K.PoolIncome,0) else isNull((k.PoolIncome *-1 ),0) end as  [Royalty BV]  from " & _
                    '" M_MemberMaster as a with(nolock) Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " & _
                    '" inner Join M_KitMaster as k with(nolock)on  k.RowStatus='Y'," & _
                    '" RepurchIncome as b with(nolock) Left Join TrnOrder as c with(nolock)   On   " & _
                    '" c.Formno=b.Formno   Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " & _
                    '" and s.BillNo=b.BillNo  where  b.Kitid=k.KitId   " & Condition & " and " & _
                    '" a.Formno=b.Formno and b.BillType<>'R' And c.ActiveStatus <> 'D'"

                    '       qry1 &= " union All "
                    '       qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    '       qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    '       qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    '       qry1 &= " Isnull(b.Amount,0) as [Amount],'Real State BV'as [Package Name],Repurchincome as BV,B.PVValue as PV,B.Royalty as [Royalty BV]  "
                    '       qry1 &= " from  M_MemberMaster as a with(nolock) "
                    '       qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    '       qry1 &= " Inner join RepurchIncome b  on d.FormnoDwn  = b.Formno And RBV = 'Y' and b.BillType<>'R'"
                    '       qry1 &= " Where  1=1  " & Condition & " "
                    '        qry1 = "  select a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," & _
                    '" Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " & _
                    '" Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name],Repurchincome as [Topup BV],B.PVValue as PV ,Case when Repurchincome>0 then IsNull( K.PoolIncome,0) else isNull((k.PoolIncome *-1 ),0) end as  [Royalty BV]  from " & _
                    '" M_MemberMaster as a with(nolock) Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " & _
                    '" inner Join M_KitMaster as k with(nolock)on  k.RowStatus='Y'," & _
                    '" RepurchIncome as b with(nolock) Left Join TrnOrder as c with(nolock)   On   " & _
                    '" c.Formno=b.Formno and Replace(B.BillNo,'Order ','')=Cast(C.OrderNo as Varchar) Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " & _
                    '" and s.BillNo=b.BillNo  where  b.Kitid=k.KitId   " & Condition & " and " & _
                    '" a.Formno=b.Formno and b.BillType<>'R' And c.ActiveStatus <> 'D' and b.BillNo<>'0' and b.RBV='N'"

                    '        qry1 &= " union All "
                    '        qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    '        qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    '        qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    '        qry1 &= " Isnull(k.KitAmount,0) as [Amount],k.KitName as [Package Name],Repurchincome as [Repurchase BV],B.PVValue as PV,  "
                    '        qry1 &= " Case when Repurchincome>0 then IsNull( K.PoolIncome,0) else isNull((k.PoolIncome *-1 ),0) end as  [Royalty BV]"
                    '        qry1 &= " from  M_MemberMaster as a with(nolock) "
                    '        qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    '        qry1 &= " ,RepurchIncome as b with(nolock) inner Join M_KitMaster as k with(nolock)on b.Kitid=k.KitId  and b.RBV='N' and b.BillNo='0'   And  k.RowStatus='Y'"
                    '        qry1 &= " Where 1=1 and a.formno=b.Formno " & Condition & " "
                    '        qry1 &= " union All "
                    '        qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    '        qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    '        qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    '        qry1 &= " Isnull(b.Amount,0) as [Amount],'Real State BV'as [Package Name],Repurchincome as BV,B.PVValue as PV,B.Royalty as [Royalty BV]  "
                    '        qry1 &= " from  M_MemberMaster as a with(nolock) "
                    '        qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    '        qry1 &= " Inner join RepurchIncome b  on d.FormnoDwn  = b.Formno And RBV = 'Y' and b.BillType<>'R'"
                    '        qry1 &= " Where 1=1   " & Condition & " "
                    '                qry1 = "  select a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," & _
                    '" Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " & _
                    '" Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name],Repurchincome as [BV],B.PVValue as PV ,Case when Repurchincome>0 then IsNull( K.PoolIncome,0) else isNull((k.PoolIncome *-1 ),0) end as  [Royalty BV],isnull(b.smartcardbv,0.00) as [Smartcard BV]   from  " & _
                    '"  M_MemberMaster as a with(nolock) Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn, " & _
                    '" RepurchIncome as b with(nolock) Left Join TrnOrder as c with(nolock) On " & _
                    '" c.Formno=b.Formno  and Cast(c.OrderNo as Varchar)=Replace(b.BillNo,'Order ','') Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " & _
                    '" and s.BillNo=b.BillNo  inner Join M_KitMaster as k with(nolock)on   b.Kitid=k.KitId   And k.RowStatus='Y' where 1=1 " & Condition & " and " & _
                    '" a.Formno=b.Formno and b.BillType<>'R' And c.ActiveStatus <> 'D' and b.BillNo<>'0' and  B.rbv='N'   "

                    '                qry1 &= " union All "
                    '                qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    '                qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    '                qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    '                qry1 &= " Isnull(k.KitAmount,0) as [Amount],k.KitName as [Package Name],Repurchincome as [BV],B.PVValue as PV,Case when Repurchincome>0 then IsNull( K.PoolIncome,0) else isNull((k.PoolIncome *-1 ),0) end as  [Royalty BV],isnull(b.smartcardbv,0.00) as [Smartcard BV]  "
                    '                qry1 &= " from M_MemberMaster as a with(nolock)  Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    '                qry1 &= " ,RepurchIncome as b with(nolock) "
                    '                qry1 &= " inner Join M_KitMaster as k with(nolock)on b.Kitid=k.KitId   And  k.RowStatus='Y'"
                    '                qry1 &= " Where 1=1 And  a.formno=b.formno and b.BillNo='0' and B.RBV='N' " & Condition & " "
                    '                qry1 &= " union All "
                    '                qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    '                qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    '                qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    '                qry1 &= " Isnull(b.Amount,0) as [Amount],'Smart Card BV'as [Package Name],Repurchincome as BV,B.PVValue as PV,B.Royalty as [Royalty BV],isnull(b.smartcardbv,0.00) as [Smartcard BV]  "
                    '                qry1 &= " from  M_MemberMaster as a with(nolock) "
                    '                qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    '                qry1 &= " Inner join RepurchIncome b  on d.FormnoDwn  = b.Formno And RBV = 'Y' and b.BillType<>'R'"
                    '                qry1 &= " Where  1=1  " & Condition & " "
                    qry1 = "  select a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," & _
    " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " & _
    " Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name],Repurchincome as [BV],Royalty as [Royalty BV],0.00 as [Smartcard BV]   from  " & _
    "  M_MemberMaster as a with(nolock) Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn, " & _
    " RepurchIncome as b with(nolock) Left Join TrnOrder as c with(nolock) On " & _
    " c.Formno=b.Formno  and Cast(c.OrderNo as Varchar)=Replace(b.BillNo,'Order ','') Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " & _
    " and s.BillNo=b.BillNo  inner Join M_KitMaster as k with(nolock)on   b.Kitid=k.KitId   And k.RowStatus='Y' where 1=1 and " & Condition & " and " & _
    " a.Formno=b.Formno and b.BillType<>'R' And c.ActiveStatus <> 'D' and b.BillNo<>'0' and  B.rbv='N'   "

                    'qry1 &= " union All "
                    'qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    'qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    'qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    'qry1 &= "  Isnull(k.KitAmount,0) as [Amount],k.KitName as [Package Name],Repurchincome as [BV],b.Royalty as  [Royalty BV],0.00 as [Smartcard BV]  "
                    'qry1 &= " from M_MemberMaster as a with(nolock)  Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    'qry1 &= " Inner Join RepurchIncome as b with(nolock) on  "
                    'qry1 &= "  a.formno=b.formno inner Join M_KitMaster as k with(nolock)on b.Kitid=k.KitId   And  k.RowStatus='Y' and b.BillType<>'R'  and B.RBV='N' and " & Condition & " "
                    qry1 &= " union All "
                    qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    qry1 &= " Isnull(b.Amount,0) as [Amount],Case when b.smartcardbv>0 then 'Smart Card BV' else 'Royalty BV' end as  [Package Name],Repurchincome as BV,0.00 as [Royalty BV],isnull(b.smartcardbv,0.00) as [Smartcard BV]  "
                    qry1 &= " from  M_MemberMaster as a with(nolock) "
                    qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    qry1 &= " Inner join RepurchIncome b  on d.FormnoDwn  = b.Formno And RBV = 'Y' and b.BillType<>'R' and b.Amount>0"
                    qry1 &= " Where  " & Condition & " "

                    qry1 &= " union All "
                    qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    qry1 &= " Isnull(b.Amount,0) as [Amount],Case when b.smartcardbv<0 then 'Smart Card BV' else 'Royalty BV' end as  [Package Name],Repurchincome as BV,0.00 as [Royalty BV],isnull(b.smartcardbv,0.00) as [Smartcard BV]  "
                    qry1 &= " from  M_MemberMaster as a with(nolock) "
                    qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    qry1 &= " Inner join RepurchIncome b  on d.FormnoDwn  = b.Formno And RBV = 'Y' and b.BillType<>'R' and b.Amount<0"
                    qry1 &= " Where  " & Condition & " "

                Else
                    qry1 = "  select a.Idno,(a.MemFirstName+ ' '+a.MemLastName)As MemberName," & _
                         " Case when d.LegNo=1 then 'Left' else 'Right' end as GroupName,Replace(Convert(Varchar,b.BillDate,106),' ','-') as BillDate, " & _
                         " Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as BillAmount,k.KitName as PackageName,b.RepurchIncome as BV,B.PVValue as PV " & _
                         " from M_MemberMaster as a,M_MemTreeRelation as d,M_KitMaster as k," & _
                         " RepurchIncome as b Left Join TrnOrder as c   On  ('Order '+ Cast( OrderNo as Varchar))=b.BillNo and  " & _
                         " c.Formno=b.Formno   Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s On  b.Formno=s.Formno " & _
                         " and s.BillNo=b.BillNo  where b.Kitid=k.KitId and k.RowStatus='Y' and a.Formno=d.FormnoDwn and " & Condition & " and " & _
                         " a.Formno=b.Formno and b.BillType<>'R'   Order by Year(b.BillDate) Desc,Month(b.BillDate)Desc,Day(b.BillDate)Desc"
                End If



            End If

            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("DownlinePurchase.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub
    Protected Sub FillLevel(Optional ByVal Condition As String = "")
        Try


            Dim str As String
            dtData = New DataTable
            'str = "Select MLevel(Select 0 As MLevel,'-- ALL --' As Mlevel Union ALL select Mlevel from R_MemTreeRelation where FormNo='" & Session("FormNo") & "'order by MLevel"
            str = "Select distinct * from (Select 0 As MLevel,'-- ALL --' As LevelName Union ALL " & _
     " select MLevel,'Level :'+ convert (varchar,MLevel) as LevelName from R_MemTreeRelation where 1=1 " & Condition & " ) as Temp order by MLevel"

            'str = "select distinct MLevel from R_MemTreeRelation where FormNo='" & Session("FormNo") & "'"
            dtData = objDAL.GetData(str)
            DdlLevel.DataSource = dtData
            DdlLevel.DataTextField = "LevelName"
            DdlLevel.DataValueField = "MLevel"
            DdlLevel.DataBind()
            'Conn.Close()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
        Dim sw As New System.IO.StringWriter
        Dim htw As System.Web.UI.HtmlTextWriter
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.xls"
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
        Response.Charset = ""
        dg.EnableViewState = False
        htw = New HtmlTextWriter(sw)
        dg.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.End()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            lblErr.Text = ""
            lblCount.Text = ""
            lblbv.Text = ""
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            If txtMember.Text <> "" Then
                'Condition = "And d.Formno='" & LblFormno.Text & "'"
                Condition = " d.Formno='" & LblFormno.Text & "'"
            Else
                scrName = "<SCRIPT language='javascript'>alert('Member Id Does Not Exist.. ');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrName)
                Exit Sub
            End If
            If RbtProduct.SelectedValue = "R" Then
                If RbtLegNo.SelectedValue <> "0" Then
                    Condition = Condition & " and d.LegNo='" & Val(RbtLegNo.SelectedValue) & "'"
                End If
                'If DdlLevel.SelectedValue > 0 Then
                '    Condition = Condition & "AND d.MLevel='" & DdlLevel.SelectedValue & "'"
                'End If
            Else
                If RbtLegNo.SelectedValue <> "0" Then
                    Condition = Condition & " and d.LegNo='" & Val(RbtLegNo.SelectedValue) & "'"
                End If
            End If

            '  Dim scrname As String = ""
            Dim FrmDate As String = txtStartDate.Text
            Dim ToDate As String = txtEndDate.Text
            If FrmDate <> "" Then


                Try
                    Dim Dt As DateTime = FrmDate
                Catch ex As Exception
                    scrName = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" & "</SCRIPT>"
                    Me.RegisterStartupScript("MyAlert", scrName)
                    Exit Sub
                End Try
            End If
            If ToDate <> "" Then


                Try
                    Dim Dt As DateTime = ToDate
                Catch ex As Exception
                    scrName = "<SCRIPT language='javascript'>alert('Check End Date.. ');" & "</SCRIPT>"
                    Me.RegisterStartupScript("MyAlert", scrName)
                    Exit Sub
                End Try
            End If
            If FrmDate <> "" And ToDate <> "" Then


                Condition = Condition & " And Cast(Convert(Varchar,b.BillDate,106)as DateTime)>='" & FrmDate & "' And Cast(Convert(Varchar,b.BillDate,106)as DateTime)<='" & ToDate & "'"
            End If
            Dim qry1 As String = ""
            '        qry1 = "Select Replace(Convert(Varchar,a.BillDate,106),' ','-') as BillDate,a.FCode as IdNo,a.PartyName as MemberName ,c.RepurchIncome as RepurchaseBv," & _
            '        " b.MLevel as Level,a.NetPayable as BillAmount FROM " & _
            '"" & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as a,R_MemTreeRelation as b, RepurchIncome as c WHERE b.FormNoDwn = a.FormNo  " & _
            '" And c.BillType='R'  And (a.BillNo =c.BillNo Or a.RefNo=c.BillNo) " & Condition & "   Group by a.BillDate, a.FCode,a.PartyName,b.MLevel ,c.RepurchIncome,a.NetPayable Order By Year(a.BillDate) Desc,Month(a.BillDate)Desc,Day(a.BillDate)Desc,Mlevel "
            If RbtProduct.SelectedValue = "R" Then
                If (Session("CompID") = "1010") Then
                    'qry1 = "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno,(a.MemFirstName+ ' '+a.MemLastName)As  "
                    'qry1 &= "  [Member Name],  Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    'qry1 &= "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.RepurchIncome as [Repurchase BV]  "
                    'qry1 &= "  from M_MemberMaster as a with(nolock) inner Join  M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    'qry1 &= "  Inner Join RepurchIncome as b with(nolock) on  a.Formno=b.Formno  "
                    'qry1 &= " Left Join TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo "
                    'qry1 &= "  Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo"
                    'qry1 &= "  Left join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo"
                    'qry1 &= "  where  1 =1 and "
                    'qry1 &= " " & Condition & " and  a.Formno=b.Formno and b.BillType in ('R','O')"
                    ''qry1 &= " Order by b.rectimestamp "
                    qry1 = " exec Sp_DowmLineRepurchaseReport '" & LblFormno.Text & "','" & FrmDate & "','" & ToDate & "' "
                    '        qry1 = "  select a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," & _
                    '" Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " & _
                    '" Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Bill Amount],k.KitName as [Package Name],Repurchincome as BV   from " & _
                    '" M_MemberMaster as a with(nolock) Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " & _
                    '" inner Join M_KitMaster as k with(nolock)on  k.RowStatus='Y'," & _
                    '" RepurchIncome as b with(nolock) Left Join TrnOrder as c with(nolock)   On   " & _
                    '" c.Formno=b.Formno   Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " & _
                    '" and s.BillNo=b.BillNo  where  b.Kitid=k.KitId and " & Condition & " and " & _
                    '" a.Formno=b.Formno and b.BillType='R' And c.ActiveStatus <> 'D' "

                    '        qry1 &= " union All "
                    '        qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    '        qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    '        qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    '        qry1 &= " Isnull(b.Amount,0) as [Bill Amount],'Real State BV'as [Package Name],Repurchincome as BV  "
                    '        qry1 &= " from  M_MemberMaster as a with(nolock) "
                    '        qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    '        qry1 &= " Inner join RepurchIncome b  on d.FormnoDwn  = b.Formno And RBV = 'N' and b.BillType='R'"
                    '        qry1 &= " Where 1 =1 and  " & Condition & " "
                    
                ElseIf (Session("CompID") = "1007") Then
                    qry1 = "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as BillDate ,a.Idno,(a.MemFirstName+ ' '+a.MemLastName)As MemberName," & _
          "Case when d.LegNo=1 then 'Left' else 'Right' end as GroupName,b.PvValue as [Repurchase BV],d.Mlevel, " & _
          " Isnull(c.OrderAmt,s.NetPayable)as BillAmount from M_MemberMaster as a,M_MemTreeRelation as d," & _
          " RepurchIncome as b Left Join TrnOrder as c   On  ('Order '+ Cast( OrderNo as Varchar))=b.BillNo and  " & _
          " c.Formno=b.Formno   Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s On  b.Formno=s.Formno " & _
          " and s.BillNo=b.BillNo  where a.Formno=d.FormnoDwn " & Condition & " and " & _
          " a.Formno=b.Formno and b.BillType='R'    Order by Year(b.BillDate) Desc,Month(b.BillDate)Desc,Day(b.BillDate)Desc,Mlevel"

                Else
                    qry1 = "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as BillDate ,a.Idno,(a.MemFirstName+ ' '+a.MemLastName)As MemberName," & _
              " b.RepurchIncome as [Repurchase BV] ,d.Mlevel, " & _
              " Isnull(c.OrderAmt,s.NetPayable)as BillAmount from M_MemberMaster as a,R_MemTreeRelation as d," & _
              " RepurchIncome as b Left Join TrnOrder as c   On  ('Order '+ Cast( OrderNo as Varchar))=b.BillNo and  " & _
              " c.Formno=b.Formno   Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s On  b.Formno=s.Formno " & _
              " and s.BillNo=b.BillNo  where a.Formno=d.FormnoDwn " & Condition & " And  a.formno=b.formno and " & _
              " a.Formno=b.Formno and b.BillType='R'    Order by Year(b.BillDate) Desc,Month(b.BillDate)Desc,Day(b.BillDate)Desc,Mlevel"

                End If


            Else



                If (Session("CompID") = "1010") Then
                    '       qry1 = "  select a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," & _
                    '" Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " & _
                    '" Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name],Repurchincome as [Repurchase BV],B.PVValue as PV ,Case when Repurchincome>0 then IsNull( K.PoolIncome,0) else isNull((k.PoolIncome *-1 ),0) end as  [Royalty BV]  from " & _
                    '" M_MemberMaster as a with(nolock) Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " & _
                    '" inner Join M_KitMaster as k with(nolock)on  k.RowStatus='Y'," & _
                    '" RepurchIncome as b with(nolock) Left Join TrnOrder as c with(nolock)   On   " & _
                    '" c.Formno=b.Formno   Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " & _
                    '" and s.BillNo=b.BillNo  where  b.Kitid=k.KitId   " & Condition & " and " & _
                    '" a.Formno=b.Formno and b.BillType<>'R' And c.ActiveStatus <> 'D' and b.BillNo<>'0'"

                    '       qry1 &= " union All "
                    '       qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    '       qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    '       qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    '       qry1 &= " Isnull(k.KitAmount,0) as [Amount],k.KitName as [Package Name],Repurchincome as [Repurchase BV],B.PVValue as PV,  "
                    '       qry1 &= " Case when Repurchincome>0 then IsNull( K.PoolIncome,0) else isNull((k.PoolIncome *-1 ),0) end as  [Royalty BV]"
                    '       qry1 &= " from  M_MemberMaster as a with(nolock) "
                    '       qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    '       qry1 &= " ,RepurchIncome as b with(nolock) inner Join M_KitMaster as k with(nolock)on b.Kitid=k.KitId   And  k.RowStatus='Y'"
                    '       qry1 &= " Where 1=1 and " & Condition & " "

                    ' qry1 = "  select a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," & _
                    '" Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " & _
                    '" Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name],Repurchincome as [Topup BV],B.PVValue as PV ,Case when Repurchincome>0 then IsNull( K.PoolIncome,0) else isNull((k.PoolIncome *-1 ),0) end as  [Royalty BV],b.smartcardbv as [Smartcard BV]  from  " & _
                    '"  M_MemberMaster as a with(nolock) Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn, " & _
                    '" RepurchIncome as b with(nolock) Left Join TrnOrder as c with(nolock) On " & _
                    '" c.Formno=b.Formno  and Cast(c.OrderNo as Varchar)=Replace(b.BillNo,'Order ','') Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " & _
                    '" and s.BillNo=b.BillNo  inner Join M_KitMaster as k with(nolock)on   b.Kitid=k.KitId   And k.RowStatus='Y' where " & Condition & " and " & _
                    '" a.Formno=b.Formno and b.BillType<>'R' And c.ActiveStatus <> 'D' and b.BillNo<>'0' and  B.rbv='N'   "

                    '       qry1 &= " union All "
                    '       qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    '       qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    '       qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    '       qry1 &= " Isnull(k.KitAmount,0) as [Amount],k.KitName as [Package Name],Repurchincome as [Repurchase BV],B.PVValue as PV,Case when Repurchincome>0 then IsNull( K.PoolIncome,0) else isNull((k.PoolIncome *-1 ),0) end as  [Royalty BV],b.smartcardbv as [Smartcard BV] "
                    '       qry1 &= " from M_MemberMaster as a with(nolock)  Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    '       qry1 &= " ,RepurchIncome as b with(nolock) "
                    '       qry1 &= " inner Join M_KitMaster as k with(nolock)on b.Kitid=k.KitId   And  k.RowStatus='Y'"
                    '       qry1 &= " Where 1=1 And  a.formno=b.formno and b.BillNo='0' and B.RBV='N' And " & Condition & " "
                    '       qry1 &= " union All "
                    '       qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    '       qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    '       qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    '       qry1 &= " Isnull(b.Amount,0) as [Amount],'Real State BV'as [Package Name],Repurchincome as BV,B.PVValue as PV,B.Royalty as [Royalty BV]  "
                    '       qry1 &= " from  M_MemberMaster as a with(nolock) "
                    '       qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    '       qry1 &= " Inner join RepurchIncome b  on d.FormnoDwn  = b.Formno And RBV = 'Y' and b.BillType<>'R'"
                    '       qry1 &= " Where 1=1  and " & Condition & " "

                    qry1 = " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], " & _
                     " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name]," & _
                     " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date]," & _
                     " Case when b.kitid=0 then RoyaltyNmWise  else kitname end  as  [Package Name]," & _
                      "Case when b.kitid=0 then b.amount  else isnull(kitamount,0) end as Amount,Repurchincome as BV,isnull(royalty,0) as [Royalty BV]  " & _
                     " from  M_MemberMaster as a with(nolock) " & _
                    " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " & _
                     " Inner join RepurchIncome b  on d.FormnoDwn  = b.Formno And RBV in ('Y','N') and b.BillType<>'R' " & _
                    "left Join M_KitMaster as k with(nolock)on b.Kitid=k.KitId  And k.RowStatus='Y'" & _
                    " Where  " & Condition & " "
                    '" union All "
                    'qry1 &= "  select a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name],"
                    'qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], "
                    'qry1 &= " Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name],Repurchincome as [BV],Royalty as [Royalty BV],0.00 as [Smartcard BV]   from  "
                    'qry1 &= "  M_MemberMaster as a with(nolock) Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn, "
                    'qry1 &= " RepurchIncome as b with(nolock) Left Join TrnOrder as c with(nolock) On "
                    'qry1 &= " c.Formno=b.Formno  and Cast(c.OrderNo as Varchar)=Replace(b.BillNo,'Order ','') Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno "
                    'qry1 &= " and s.BillNo=b.BillNo  inner Join M_KitMaster as k with(nolock)on   b.Kitid=k.KitId   And k.RowStatus='Y' where 1=1 and " & Condition & " and "
                    'qry1 &= " a.Formno=b.Formno and b.BillType<>'R' And c.ActiveStatus <> 'D'  and  B.rbv='Y'   "

                    'qry1 &= " union All "
                    'qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    'qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    'qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    'qry1 &= "  Isnull(k.KitAmount,0) as [Amount],k.KitName as [Package Name],Repurchincome as [BV],b.Royalty as  [Royalty BV],0.00 as [Smartcard BV]  "
                    'qry1 &= " from M_MemberMaster as a with(nolock)  Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    'qry1 &= " Inner Join RepurchIncome as b with(nolock) on  "
                    'qry1 &= "  a.formno=b.formno inner Join M_KitMaster as k with(nolock)on b.Kitid=k.KitId   And  k.RowStatus='Y' and b.BillType<>'R' and B.RBV='N' and " & Condition & " "
                    'qry1 &= " union All "
                    'qry1 &= " Select  a.Idno as [Id No],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], "
                    'qry1 &= " Case when d.LegNo=1 then 'Group A' else 'Group B' end as [Group Name],"
                    'qry1 &= " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],"
                    'qry1 &= " Case when b.smartcardbv<0 then RoyaltyNmWise else kitname end as  [Package Name],Repurchincome as BV,isnull(royalty,0) as [Royalty BV]  "
                    'qry1 &= " from  M_MemberMaster as a with(nolock) "
                    'qry1 &= " Inner Join M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn "
                    'qry1 &= " Inner join RepurchIncome b  on d.FormnoDwn  = b.Formno And RBV = 'Y' and b.BillType<>'R' and b.Amount<0"
                    'qry1 &= "left Join M_KitMaster as k with(nolock)on b.Kitid=k.KitId   And  k.RowStatus='Y'"
                    'qry1 &= " Where  " & Condition & " "

                ElseIf (Session("CompID") = "1007") Then
                    qry1 = "  select a.Idno,(a.MemFirstName+ ' '+a.MemLastName)As MemberName," & _
                    " Case when d.LegNo=1 then 'Left' else 'Right' end as GroupName,Replace(Convert(Varchar,b.BillDate,106),' ','-') as BillDate, " & _
                    " Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as BillAmount,k.KitName as PackageName,B.PVValue as PV" & _
                    " from M_MemberMaster as a,M_MemTreeRelation as d,M_KitMaster as k," & _
                    " RepurchIncome as b Left Join TrnOrder as c   On  ('Order '+ Cast( OrderNo as Varchar))=b.BillNo and  " & _
                    " c.Formno=b.Formno   Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s On  b.Formno=s.Formno " & _
                    " and s.BillNo=b.BillNo  where b.Kitid=k.KitId and k.RowStatus='Y' and a.Formno=d.FormnoDwn " & Condition & " and " & _
                    " a.Formno=b.Formno and b.BillType<>'R'   Order by Year(b.BillDate) Desc,Month(b.BillDate)Desc,Day(b.BillDate)Desc"
                Else

                    qry1 = "  select a.Idno,(a.MemFirstName+ ' '+a.MemLastName)As MemberName," & _
                         " Case when d.LegNo=1 then 'Left' else 'Right' end as GroupName,Replace(Convert(Varchar,b.BillDate,106),' ','-') as BillDate, " & _
                         " Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as BillAmount,k.KitName as PackageName,b.RepurchIncome as [Repurchase BV],B.PVValue as PV " & _
                         " from M_MemberMaster as a," & _
                         " M_MemTreeRelation as d,M_KitMaster as k," & _
                         " RepurchIncome as b Left Join TrnOrder as c   On  ('Order '+ Cast( OrderNo as Varchar))=b.BillNo and  " & _
                         " c.Formno=b.Formno   Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as s On  b.Formno=s.Formno " & _
                         " and s.BillNo=b.BillNo  where b.Kitid=k.KitId and k.RowStatus='Y' and a.Formno=d.FormnoDwn " & Condition & " and " & _
                         " a.Formno=b.Formno and b.BillType<>'R'   Order by Year(b.BillDate) Desc,Month(b.BillDate)Desc,Day(b.BillDate)Desc"

                End If



            End If
            dtData = New DataTable
            dtData = objDAL.GetData(qry1)
            If dtData.Rows.Count > 0 Then
                GvData.DataSource = dtData
                GvData.DataBind()
                Session("RewardList") = dtData
                GvData.Visible = True
                gvContainer.Visible = True
                lblCount.Text = "Total : " & dtData.Rows.Count
                'If RbtProduct.SelectedValue = "R" Then

                ' End If

                ' FillRepurchaseTotal1()
            Else
                GvData.Visible = False
                gvContainer.Visible = False
            End If
            If (RbtProduct.SelectedValue = "R") Then
                If Session("CompId") = "1007" Or Session("CompId") = "1010" Then
                    FillRepurchaseTotal()
                End If
                
            Else

                If (RbtProduct.SelectedValue = "T") Then
                    'divR.Visible = False
                    'divT.Visible = True
                    lblleftbv.Visible = True
                    lblbv.Visible = True
                    lblroyaltileftbv.Visible = True
                    lblroyaltirightbv.Visible = True
                    lblrleftbv.Visible = False


                    Dim aDr1 As DataRow = dtData.NewRow
                    
                    If RbtLegNo.SelectedValue = 1 Then

                        Dim dt1 As DataTable = New DataTable
                        dt1 = dtData
                       

                        Dim dv As DataView = New DataView(dt1)
                        'dv.RowFilter = "[Group Name] = 'Left'"
                        dv.RowFilter = "[Group Name] = 'Group A'"
                        dt1 = dv.ToTable()
                        If (dt1.Rows.Count > 0) Then
                            Dim aDr2 As DataRow = dt1.NewRow
                            lblrleftbv.Text = dt1.Compute("Sum(Bv)", "")
                            lblleftbv.Text = "Left BV :" & dt1.Compute("Sum(BV)", "")
                            'lblleftbv.Text = dt1.Compute("Sum(BV)", "")
                            lblbv.Text = "Right BV:" & "0.00"
                            lblroyaltirightbv.Text = "Right Royalty BV :" & "0.00"
                        Else
                            lblrleftbv.Text = "Left BV :" & "0.00"
                            'lblleftbv.Text = "Left BV :" & "0.00"
                            lblbv.Text = "Right BV :" & "0.00"
                            lblroyaltirightbv.Text = "Right Royalty BV :" & "0.00"
                            lblCount.Text = "Total :" & "0.00"
                        End If
                        If IsDBNull(dtData.Compute("Sum([Royalty BV])", "")) Then
                            lblroyaltileftbv.Text = "Left Royalty BV :" & "0.00"

                        Else
                            lblroyaltileftbv.Text = "Left Royalty BV :" & dtData.Compute("Sum([Royalty BV])", "")
                        End If
                        If RbtProduct.SelectedValue = "T" Then
                            Try
                                If dtData.Rows.Count > 0 Then
                                    lblTotalBV.Text = "Total BV :" & dtData.Compute("Sum([BV])", "")
                                    lblTotalBV.Visible = True
                                Else
                                    lblTotalBV.Text = "Total BV :" & "0.00"
                                    lblTotalBV.Visible = True
                                End If
                                
                            Catch ex As Exception

                            End Try
                        Else
                            lblTotalBV.Visible = False
                        End If
                    End If
                    If RbtLegNo.SelectedValue = 2 Then
                        Dim dt1 As DataTable = New DataTable
                        dt1 = dtData
                        If Session("CompId") = "1010" Then
                            Dim dv As DataView = New DataView(dt1)
                            'dv.RowFilter = "[Group Name] = 'Left'"
                            dv.RowFilter = "[Group Name] = 'Group B'"
                            dt1 = dv.ToTable()
                            If (dt1.Rows.Count > 0) Then
                                Dim aDr3 As DataRow = dt1.NewRow
                                lblbv.Text = "Right BV:" & dt1.Compute("Sum([BV])", "")
                                'lblleftbv.Visible = False
                                'lblrleftbv.Text = "Left BV :" & "0.00"
                                lblroyaltileftbv.Text = "Left Royalty BV :" & "0.00"
                                lblleftbv.Text = "Left BV :" & "0.00"
                            Else
                                lblbv.Text = "Right BV:" & "0.00"
                                lblleftbv.Visible = False
                                lblrleftbv.Text = "Left BV :" & "0.00"
                                lblroyaltileftbv.Text = "Left Royalty BV :" & "0.00"
                                lblCount.Text = "Total :" & "0.00"
                            End If

                            If IsDBNull(dtData.Compute("Sum([Royalty BV])", "")) Then
                                lblroyaltirightbv.Text = "Right Royalty BV :" & "0.00"
                            Else
                                lblroyaltirightbv.Text = "Right Royalty BV :" & dtData.Compute("Sum([Royalty BV])", "")

                            End If

                            'LblRightRoyalty.Text = dt.Compute("Sum([Royalty BV])", "")
                        Else
                            lblbv.Text = dtData.Compute("Sum([Repurchase BV])", "")
                            lblbv.Text = "0"
                            lblroyaltirightbv.Text = "Right Royalty BV :" & "0.00"
                        End If
                        If RbtProduct.SelectedValue = "T" Then
                            Try
                                If dtData.Rows.Count > 0 Then
                                    lblTotalBV.Text = "Total BV :" & dtData.Compute("Sum([BV])", "")
                                    lblTotalBV.Visible = True
                                Else
                                    lblTotalBV.Text = "Total BV :" & "0.00"
                                    lblTotalBV.Visible = True
                                End If
                                
                            Catch ex As Exception

                            End Try
                        Else
                            lblTotalBV.Visible = False
                        End If
                    End If

                    If RbtLegNo.SelectedValue = 0 Then
                        Dim dt1 As DataTable = New DataTable
                        dt1 = dtData
                        If (dtData.Rows.Count > 0) Then
                            'below commit 22 March 2022
                            If RbtProduct.SelectedValue = "T" Then
                                Try
                                    lblTotalBV.Text = "Total BV :" & dtData.Compute("Sum([BV])", "")
                                    lblTotalBV.Visible = True
                                Catch ex As Exception

                                End Try
                            Else
                                lblTotalBV.Visible = False
                            End If
                            'lblTotalBV.Text = dt.Compute("Sum(BV)", "")
                        Else
                            lblTotalBV.Text = "0.00"
                            lblTotalBV.Visible = True
                        End If
                        Dim dv As DataView = New DataView(dt1)
                        dv.RowFilter = "[Group Name] = 'Group A'"
                        dt1 = dv.ToTable()
                        If Session("CompId") <> "1010" Then

                            If (dt1.Rows.Count > 0) Then
                                Dim aDr2 As DataRow = dt1.NewRow
                                lblleftbv.Text = "Left BV :" & dt1.Compute("Sum([Repurchase BV])", "")

                            Else
                                lblleftbv.Text = "Left BV :" & "0.00"

                            End If
                        
                        End If
                        If Session("CompId") = "1010" Then
                            'If RbtProduct.SelectedValue = "T" Then
                            If RbtProduct.SelectedValue = "T" Then
                                If (dt1.Rows.Count > 0) Then
                                    Dim aDr2 As DataRow = dt1.NewRow
                                    lblleftbv.Text = "Left BV :" & dt1.Compute("Sum([BV])", "")
                                    lblleftbv.Visible = True
                                Else
                                    lblleftbv.Text = "Left BV :" & "0.00"
                                    lblleftbv.Visible = True
                                    lblrleftbv.Visible = False
                                End If
                                'End If
                            Else
                                lblleftbv.Visible = False
                            End If
                            'Else
                            '    lblleftbv.Text = "Left BV :" & "0.00"
                        End If
                        If Session("CompId") = "1010" Then
                            'If RbtProduct.SelectedValue = "T" Then
                            If RbtProduct.SelectedValue = "T" Then
                                If IsDBNull(dt1.Compute("Sum([Royalty BV])", "")) Then
                                    lblroyaltileftbv.Text = "Left Royalty BV:" & "0.00"
                                Else
                                    lblroyaltileftbv.Text = "Left Royalty BV :" & dt1.Compute("Sum([Royalty BV])", "")
                                End If
                                'If IsDBNull(dtData.Compute("Sum([Smartcard BV])", "")) Then
                                '    'LblLeftRoyalty.Text = "0.00"
                                '    LeftSmartcardbv.Text = "Left Well Smart BV:" & "0.00"
                                'Else
                                '    'LblLeftRoyalty.Text = dtData.Compute("Sum([Smartcard BV])", "")
                                '    LeftSmartcardbv.Text = "Left Well Smart BV :" & dtData.Compute("Sum([Smartcard BV])", "")
                                'End If
                            End If
                        Else
                            lblroyaltileftbv.Text = "Left Royalty BV :" & "0.00"
                        End If

                        If Session("CompId") = "1010" Then
                            'If RbtProduct.SelectedValue = "T" Then
                            '    If RbtProduct.SelectedValue = "T" Then

                            '        If IsDBNull(dt1.Compute("Sum([Smartcard BV])", "")) Then
                            '            'LblLeftRoyalty.Text = "0.00"
                            '            LeftSmartcardbv.Text = "Left Well Smart BV:" & "0.00"
                            '        Else
                            '            'LblLeftRoyalty.Text = dtData.Compute("Sum([Smartcard BV])", "")
                            '            LeftSmartcardbv.Text = "Left Well Smart BV :" & dt1.Compute("Sum([Smartcard BV])", "")
                            '            LeftSmartcardbv.Visible = True
                            '        End If


                            '    End If
                            'Else
                            '    LeftSmartcardbv.Text = "Left Well Smart BV :" & "0.00"
                            LeftSmartcardbv.Visible = False
                        End If

                        dt1 = dtData
                        Dim dv1 As DataView = New DataView(dt1)
                        dv1.RowFilter = "[Group Name] = 'Group B'"
                        dt1 = dv1.ToTable()
                        If Session("CompId") <> "1010" Then
                            If (dt1.Rows.Count > 0) Then
                                Dim aDr3 As DataRow = dt1.NewRow
                                lblbv.Text = "Right BV:" & dt1.Compute("Sum([Repurchase BV])", "")
                            Else
                                lblbv.Text = "Right BV:" & "0.00"
                            End If

                        End If
                        If Session("CompId") = "1010" Then
                            If RbtProduct.SelectedValue = "T" Then
                                If (dt1.Rows.Count > 0) Then
                                    Dim aDr3 As DataRow = dt1.NewRow
                                    lblbv.Text = "Right BV:" & dt1.Compute("Sum([BV])", "")
                                Else
                                    lblbv.Text = "Right BV:" & "0.00"
                                End If

                                If IsDBNull(dt1.Compute("Sum([Royalty BV])", "")) Then
                                    lblroyaltirightbv.Text = "Right Royalty BV :" & "0.00"
                                Else
                                    lblroyaltirightbv.Text = "Right Royalty BV :" & dt1.Compute("Sum([Royalty BV])", "")
                                End If


                            End If

                            lblrrightbv.Visible = False

                            ' LblRightRoyalty.Text = dt1.Compute("Sum([Royalty BV])", "")
                        Else
                            lblroyaltirightbv.Text = "Right Royalty BV :" & "0.00"
                        End If
                        If Session("CompId") = "1010" Then
                            If RbtProduct.SelectedValue = "T" Then
                                'If IsDBNull(dt1.Compute("Sum([Smartcard BV])", "")) Then
                                '    'LblRightRoyalty.Text = "0.00"
                                '    rightSmartcardbv.Text = "Right Well Smart BV :" & "0.00"
                                'Else
                                '    'LblRightRoyalty.Text = dtData.Compute("Sum([Smartcard BV])", "")
                                '    rightSmartcardbv.Text = "Right Well Smart  BV :" & dt1.Compute("Sum([Smartcard BV])", "")
                                '    'LblLeftRoyalty.Visible = True
                                '    'LblRightRoyalty.Visible = True
                                '    'lblroyaltileftbv.Visible = True
                                '    'lblroyaltirightbv.Visible = True
                                'End If
                                rightSmartcardbv.Visible = False
                            End If
                        End If

                        ' LblRightRoyalty.Text = dt1.Compute("Sum([Royalty BV])", "")
                    Else
                        ' rightSmartcardbv.Text = "Right Well Smart  BV :" & "0.00"
                        rightSmartcardbv.Visible = False
                    End If

                Else
                    GvData.Visible = False
                    gvContainer.Visible = False
                    lblErr.Text = "No Record Found!!"
            End If
                End If
                'endif
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetFormNo() As String
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim idNo As String
            Dim formno As String
            idNo = txtMember.Text
            idNo = idNo.Trim
            Dim qry As String = "Select FormNo,(MemFirstName+''+MemLastName) as MemberName from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
            Dim dt As New DataTable
            dt = objDAL.GetData(qry)
            If (dt.Rows.Count > 0) Then
                formno = dt.Rows(0)("FormNo")
                LblName.Text = dt.Rows(0)("MemberName")
                lblErr.Text = ""
            Else
                lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
                lblErr.Visible = True
                txtMember.Text = ""
                LblName.Text = ""
                LblFormno.Text = ""
            End If
            Return formno
        Catch ex As Exception

        End Try
    End Function
    'Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
    '    GvData.AllowPaging = False
    '    GvData.GridLines = GridLines.Both
    '    dtData = New DataTable
    '    dtData = Session("RewardList")
    '    GvData.DataSource = dtData
    '    GvData.DataBind()
    '    GvData.PagerSettings.Visible = False
    '    'gridview.BorderWidth = "2px"
    '    GvData.BorderStyle = BorderStyle.Solid
    '    GvData.BorderColor = Drawing.Color.Black

    '    'Remove modify and Delete columns from grid
    '    'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
    '    'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
    '    'For i As Integer = 0 To GvData.Rows.Count - 1
    '    '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
    '    '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
    '    'Next

    '    Dim sw As New StringWriter()

    '    Dim hw As New HtmlTextWriter(sw)

    '    GvData.RenderControl(hw)

    '    Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

    '    Dim sb As New StringBuilder()

    '    sb.Append("<script type = 'text/javascript'>")

    '    sb.Append("window.onload = new function(){")

    '    sb.Append("var printWin = window.open('', '', 'left=0")

    '    sb.Append(",top=0,width=1000,height=600,status=0');")

    '    sb.Append("printWin.document.write(""")

    '    sb.Append(gridHTML)

    '    sb.Append(""");")

    '    sb.Append("printWin.document.close();")

    '    sb.Append("printWin.focus();")

    '    sb.Append("printWin.print();")

    '    sb.Append("printWin.close();};")

    '    sb.Append("</script>")

    '    ClientScript.RegisterStartupScript(Me.GetType(), "GridPrint", sb.ToString())

    '    GvData.AllowPaging = True
    '    GvData.PagerSettings.Visible = True
    '    dtData = New DataTable
    '    dtData = Session("RewardList")
    '    GvData.DataSource = dtData
    '    GvData.DataBind()
    'End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("RewardList")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        GvData.BorderStyle = BorderStyle.Solid
        ' gridview.BorderWidth = 
        GvData.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        Next

        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)

        GvData.RenderControl(hw)

        Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

        Dim sb As New StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload = new function(){")

        sb.Append("var printWin = window.open('', '', 'left=0")

        sb.Append(",top=0,width=1000,height=1000,status=0');")

        sb.Append("printWin.document.write(""")

        sb.Append(gridHTML)

        sb.Append(""");")

        sb.Append("printWin.document.close();")

        sb.Append("printWin.focus();")

        sb.Append("printWin.print();")

        sb.Append("printWin.close();};")

        sb.Append("</script>")

        ClientScript.RegisterStartupScript(Me.[GetType](), "GridPrint", sb.ToString())

        GvData.AllowPaging = True
        GvData.PagerSettings.Visible = True
        dtData = New DataTable
        dtData = Session("RewardList")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub


    Protected Sub txtMember_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMember.TextChanged
        LblFormno.Text = GetFormNo()
        FillLevel("And FormNo='" & Val(LblFormno.Text) & "'")
        'FillTotal()
    End Sub

    Protected Sub RbtProduct_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtProduct.SelectedIndexChanged
        Try


            If RbtProduct.SelectedValue = "R" Then
                LblLevel.Text = " Group Wise"
                DdlLevel.Visible = False
                RbtLegNo.Visible = True
            Else
                LblLevel.Text = "Group Wise"
                DdlLevel.Visible = False
                RbtLegNo.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillRepurchaseTotal()
        Try
            'Dim tmpval As Integer = 0
            Dim tmpval As Decimal = 0
            Dim Dt1 As New DataTable
            If RbtLegNo.SelectedValue <> "2" Then
                Dim dv1 As DataView = New DataView(dtData)
                
                If Session("CompId") = "1010" Then
                    dv1.RowFilter = "[Group Name] = 'Group A'"
                Else
                    dv1.RowFilter = "[GroupName] = 'Left'"
                End If


                Dt1 = dv1.ToTable()
                If RbtProduct.SelectedValue = "R" Then
                    If Dt1.Rows.Count = 0 Then
                        lblrleftbv.Text = "Left Bv :0.00"
                        lblrleftbv.Visible = True
                    
                    End If
                Else
                    lblrleftbv.Visible = False
                End If
                
                For i As Integer = 0 To Dt1.Rows.Count - 1
                    If RbtProduct.SelectedValue = "R" Then
                        If Session("CompId") = "1010" Then
                            lblcnt.Text = Dt1.Rows(i)("BV")
                            tmpval = tmpval + lblcnt.Text
                            lblrleftbv.Text = "Left Bv : " & tmpval.ToString()
                            lblrleftbv.Visible = True
                        Else
                            lblcnt.Text = Dt1.Rows(i)("Repurchase BV")
                            tmpval = tmpval + lblcnt.Text
                            lblrleftbv.Text = "Total Left Repurchase Bv : " & tmpval.ToString()
                            lblrleftbv.Visible = True
                        End If
                        
                    Else
                        If Session("CompId") = "1010" Then
                            lblrleftbv.Visible = False

                        Else
                            lblcnt.Text = Dt1.Rows(i)("PV")
                            tmpval = tmpval + lblcnt.Text
                            lblrleftbv.Text = "Total Left PV : " & tmpval.ToString()
                            lblrleftbv.Visible = True
                        End If
                        
                    End If

                Next
            Else
                lblrleftbv.Text = "Left Bv :0.00"
                lblrleftbv.Visible = True
            End If
            tmpval = 0
            If RbtLegNo.SelectedValue <> "1" Then


                Dim dv2 As DataView = New DataView(dtData)
                If Session("CompId") = "1010" Then
                    dv2.RowFilter = "[Group Name] = 'Group B'"
                Else
                    dv2.RowFilter = "[GroupName] = 'Right'"
                End If

                Dt1 = dv2.ToTable()
                For i As Integer = 0 To Dt1.Rows.Count - 1
                    If RbtProduct.SelectedValue = "R" Then
                        If Session("CompId") = "1010" Then
                            lblcnt.Text = Dt1.Rows(i)("BV")
                            tmpval = tmpval + lblcnt.Text
                            lblrrightbv.Text = "Right Bv : " & tmpval.ToString()
                            lblrrightbv.Visible = True
                        Else
                            lblcnt.Text = Dt1.Rows(i)("Repurchase BV")
                            tmpval = tmpval + lblcnt.Text
                            lblrrightbv.Text = "Total Right Repurchase Bv : " & tmpval.ToString()
                            lblrrightbv.Visible = True
                        End If
                        
                    Else
                        If Session("CompId") = "1010" Then
                            lblrrightbv.Visible = False
                            
                        Else
                            lblcnt.Text = Dt1.Rows(i)("PV")
                            tmpval = tmpval + lblcnt.Text
                            lblrrightbv.Text = "Total Right PV : " & tmpval.ToString()
                            lblrrightbv.Visible = True
                        End If
                        
                    End If

                Next
                LeftSmartcardbv.Visible = False
                rightSmartcardbv.Visible = False
                'lblrleftbv.Visible = False
                lblleftbv.Visible = False
                lblTotalBV.Visible = False
                'lblrleftbv.Visible = False
            Else
                lblrrightbv.Text = ""

            End If
            'lblleftbv.Visible = False
            lblroyaltileftbv.Visible = False
            lblroyaltirightbv.Visible = False
           
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillRepurchaseTotal1()
        Try
            'Dim tmpval As Integer = 0
            Dim tmpval As Decimal = 0
            For i As Integer = 0 To dtData.Rows.Count - 1
                lblcnt1.Text = dtData.Rows(i)("PVValue")
                tmpval = tmpval + lblcnt1.Text
                lblleftbv.Text = "Total Pv : " & tmpval.ToString()
            Next
            'lblleftbv.Visible = False
            lblroyaltileftbv.Visible = False
            lblroyaltirightbv.Visible = False
        Catch ex As Exception

        End Try
    End Sub
End Class
