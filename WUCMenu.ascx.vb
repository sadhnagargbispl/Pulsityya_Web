Imports System.Data
Imports System.Data.SqlClient

Partial Class WUCMenu
    Inherits System.Web.UI.UserControl

    Dim dtMenu As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                Load_Menu()
            End If
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Private Sub Load_Menu()
        Dim html As String = ""

        ' Home link (top logo)
        ahome.HRef = Session("HomePage").ToString().Replace("~/", "")

        ' ===== FULL menu list Session me daalo (UserPermission grid ke liye) =====
        Dim fullSql As String = _
            " Select MenuId, MenuName, ParentId, OnSelect, Hierar " & _
            " from M_CompWiseWebMenuMaster " & _
            " where ActiveStatus='Y' AND RowStatus='Y' " & _
            " order by Hierar, MenuId"

        Dim dtFull As DataTable = objDAL.GetData(fullSql)
        Session("Menu") = dtFull    ' <<< UserPermission page yahi se grid banayega

        ' Login user ka GroupId
        Dim groupId As String = "0"
        If Session("GroupId") IsNot Nothing Then
            groupId = Val(Session("GroupId").ToString()).ToString()
        End If

        ' ===== Sidebar ke liye: sirf permitted menu + unke parent =====
        Dim sql As String = _
            " SELECT m.MenuId, m.MenuName, m.ParentId, m.OnSelect " & _
            " FROM M_CompWiseWebMenuMaster m " & _
            " WHERE m.ActiveStatus='Y' AND m.RowStatus='Y' " & _
            "   AND ( " & _
            "         m.MenuId IN ( " & _
            "             SELECT p.MenuId FROM M_UserPermissionMaster p " & _
            "             WHERE p.RowStatus='Y' AND p.ActiveStatus='Y' AND p.GroupId='" & groupId & "' " & _
            "         ) " & _
            "         OR m.MenuId IN ( " & _
            "             SELECT DISTINCT c.ParentId FROM M_CompWiseWebMenuMaster c " & _
            "             INNER JOIN M_UserPermissionMaster p2 ON c.MenuId = p2.MenuId " & _
            "             WHERE p2.RowStatus='Y' AND p2.ActiveStatus='Y' AND p2.GroupId='" & groupId & "' " & _
            "               AND c.ParentId <> 0 " & _
            "         ) " & _
            "       ) " & _
            " ORDER BY m.Hierar, m.MenuId"

        dtMenu = objDAL.GetData(sql)

        ' ===== Parent menus loop =====
        If dtMenu.Rows.Count > 0 Then
            For Each dr As DataRow In dtMenu.Rows
                Dim parentId As String = dr("ParentId").ToString()

                If Not String.Equals(dr("MenuName").ToString(), "-") Then
                    If Val(parentId) = 0 Then
                        Dim menuName As String = dr("MenuName").ToString()
                        Dim onSelect As String = dr("OnSelect").ToString()
                        Dim icon As String = GetIcon(menuName)

                        Dim subMenu As String = Load_SubMenu(dr("MenuId").ToString(), dtMenu)

                        If onSelect = "" Then
                            html &= " <li><a><i class=""" & icon & """></i>" & menuName & _
                                    " <span class=""fa fa-chevron-down""></span></a>" & _
                                    " <ul class=""nav child_menu"">" & subMenu & "</ul></li>"
                        Else
                            html &= "<li><a href=""" & onSelect & """><i class=""" & icon & """></i>" & menuName & "</a></li>"
                        End If
                    End If
                End If
            Next
        End If

        menu.InnerHtml = html
    End Sub
    'Private Sub Load_Menu()
    '    Dim html As String = ""

    '    ' Home link (top logo)
    '    ahome.HRef = Session("HomePage").ToString().Replace("~/", "")

    '    ' Login user ka GroupId
    '    Dim groupId As String = "0"
    '    If Session("GroupId") IsNot Nothing Then
    '        groupId = Val(Session("GroupId").ToString()).ToString()
    '    End If

    '    ' ===== Menu + Permission fetch (group-wise) =====
    '    ' Sirf wahi menu jinki permission is group ko di gayi hai (M_UserPermissionMaster)
    '    ' ===== Menu + Permission fetch (group-wise) =====
    '    ' Permitted child menu + unke parent bhi auto-include ho jayenge
    '    Dim sql As String = _
    '        " SELECT m.MenuId, m.MenuName, m.ParentId, m.OnSelect " & _
    '        " FROM M_CompWiseWebMenuMaster m " & _
    '        " WHERE m.ActiveStatus='Y' AND m.RowStatus='Y' " & _
    '        "   AND ( " & _
    '        "         m.MenuId IN ( " & _
    '        "             SELECT p.MenuId FROM M_UserPermissionMaster p " & _
    '        "             WHERE p.RowStatus='Y' AND p.ActiveStatus='Y' AND p.GroupId='" & groupId & "' " & _
    '        "         ) " & _
    '        "         OR m.MenuId IN ( " & _
    '        "             SELECT DISTINCT c.ParentId FROM M_CompWiseWebMenuMaster c " & _
    '        "             INNER JOIN M_UserPermissionMaster p2 ON c.MenuId = p2.MenuId " & _
    '        "             WHERE p2.RowStatus='Y' AND p2.ActiveStatus='Y' AND p2.GroupId='" & groupId & "' " & _
    '        "               AND c.ParentId <> 0 " & _
    '        "         ) " & _
    '        "       ) " & _
    '        " ORDER BY m.Hierar, m.MenuId"

    '    dtMenu = objDAL.GetData(sql)

    '    ' ===== Parent menus loop =====
    '    If dtMenu.Rows.Count > 0 Then
    '        For Each dr As DataRow In dtMenu.Rows
    '            Dim parentId As String = dr("ParentId").ToString()

    '            If Not String.Equals(dr("MenuName").ToString(), "-") Then
    '                If Val(parentId) = 0 Then
    '                    Dim menuName As String = dr("MenuName").ToString()
    '                    Dim onSelect As String = dr("OnSelect").ToString()
    '                    Dim icon As String = GetIcon(menuName)

    '                    ' Child menu banao
    '                    Dim subMenu As String = Load_SubMenu(dr("MenuId").ToString(), dtMenu)

    '                    If onSelect = "" Then
    '                        ' Parent with children (dropdown)
    '                        html &= " <li><a><i class=""" & icon & """></i>" & menuName & _
    '                                " <span class=""fa fa-chevron-down""></span></a>" & _
    '                                " <ul class=""nav child_menu"">" & subMenu & "</ul></li>"
    '                    Else
    '                        ' Direct link parent (Home, Logout)
    '                        html &= "<li><a href=""" & onSelect & """><i class=""" & icon & """></i>" & menuName & "</a></li>"
    '                    End If
    '                End If
    '            End If
    '        Next
    '    End If

    '    menu.InnerHtml = html
    'End Sub

    ' ===== Child menu banane wala function =====
    Private Function Load_SubMenu(ByVal parentMenuId As String, ByRef dt As DataTable) As String
        Dim html As String = ""
        Try
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim parentId As String = dr("ParentId").ToString()

                    If Val(parentId) <> 0 AndAlso Val(parentId) = Val(parentMenuId) _
                       AndAlso Not String.Equals(dr("MenuName").ToString(), "-") Then

                        html &= "<li><a href=""" & dr("OnSelect").ToString() & """>" & _
                                dr("MenuName").ToString() & "</a></li>"
                    End If
                Next
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
        Return html
    End Function

    ' ===== Menu name ke hisaab se icon =====
    Private Function GetIcon(ByVal menuName As String) As String
        Select Case menuName
            Case "Home" : Return "fa fa-table"
            Case "User" : Return "fa fa-user"
            Case "Master" : Return "fa fa-edit"
            Case "Member" : Return "fa fa-sitemap"
            Case "Report" : Return "fa fa-list-alt"
            Case "KYC Verify" : Return "fa fa-check-square-o"
            Case "Wallet" : Return "fa fa-folder-open-o"
            Case "Complaint" : Return "fa fa-comments-o"
            Case "Logout" : Return "fa fa-lock"
            Case Else : Return "fa fa-circle-o"
        End Select
    End Function

End Class
'Imports System.Data
'Imports System.Data.SqlClient

'Partial Class WUCMenu
'    Inherits System.Web.UI.UserControl
'    Dim dtMenu As New DataTable
'    Dim dtMenuMain As New DataTable
'    Dim Condtion As String = "MenuID > 1"
'    Dim objDAL As DAL
'    Dim objGen As clsGeneral = New clsGeneral
'    Dim TotalVerify, Addressverify, BankVerify, PanVerify, Walletreq, Totalwallet, BankWithdrawl, uploadform, gstn, Totalpayout As Integer

'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'        If Session("AStatus") = "OK" Then
'            If Not Page.IsPostBack Then
'                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

'                Load_Menu()
'            End If
'        Else
'            Response.Redirect("Default.aspx")
'        End If
'    End Sub

'    Private Sub Load_Menu()
'        Dim html As String = ""
'        Dim userid As String = Session("UserID")
'        ahome.HRef = Session("HomePage").ToString().Replace("~/", "")

'        ''Dim sql As String = "Select a.MenuId as MenuId, a.MenuName as MenuName,a.ParentId as ParentId, a.OnSelect as OnSelect from " & objDAL.tblMenuMaster & " as a," & objDAL.tblUserMaster & " as b," & objDAL.tblUserPermision & " as c where a.RowStatus='Y' AND b.RowStatus='Y' AND c.RowStatus='Y' AND a.ActiveStatus='Y' AND b.ActiveStatus='Y' AND c.ActiveStatus='Y' AND a.MenuId=c.MenuId AND c.GroupId=b.GroupId AND b.UserId='" & Val(userid) & "' AND c.GroupId=b.GroupId order by a.Hierar,a.MenuId"
'        Dim sql As String = "Exec Proc_GetUserPermission '" & Val(userid) & "' "
'        dtMenu = New DataTable
'        dtMenu = objDAL.GetData(sql)

'        Dim dv As DataView = New DataView


'        Dim dt As DataTable = New DataTable
'        Dim ds As DataSet = New DataSet
'        Dim str As String = " Select a.MenuId as MenuId, a.MenuName as MenuName,a.ParentId as ParentId, a.OnSelect as OnSelect from "
'        str &= " M_CompWiseWebMenuMaster a Where a.ActiveStatus = 'Y' And a.RowStatus ='Y' And a.CompanyID = '" & HttpContext.Current.Session("CompID") & "' order by a.Hierar,a.MenuId"
'        ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
'        dtMenuMain = ds.Tables(0)

'        Session("Menu") = dtMenuMain

'        ''dtMenuMain = CType(Session("Menu"), DataTable)
'        dv.Table = dtMenuMain


'        dv.RowFilter = "MenuID in (" & dtMenu.Rows(0)("MenuID") & ")"
'        dtMenu = dv.ToTable()


'        Dim icon As String = ""

'        Dim notification As String = ""
'        Dim s As String = ""
'        If (Session("CompID") = "1030") Then
'            s = "Exec Proc_Count"
'        ElseIf (Session("CompID") = "1075") Then
'            s = "select sum(BankVerify)As BankVerify,sum(AddressVerify)As AddressVerify,sum(PanVerify)as PanVerify,sum(BankVerify)+sum(AddressVerify)+sum(PanVerify) TotalVerify,sum(WalletReq)As Walletreq, sum(bankWithdrawl)as bankWithdrawl, sum(bankWithdrawl)+sum(walletreq)as TotalWallet,0 as Totalpayout " & _
'            " from(select count(*)as BankVerify,0 as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl  from KycVerify as a where BankProof<>'' and IsBankVerified='N'" & _
'                                   " Union All " & _
'                                   "select 0 as BankVerify,count(*)as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl  from KycVerify where (BackAddressProof<>'' Or AddrProof <>'') and IsAddrssverified='N'" & _
'                                   "Union All " & _
'                                   "select 0 as BankVerify,0 as AddressVerify,count(*)as PanVerify,0 as Walletreq ,0 as bankWithdrawl  from KycVerify where PanImg<>'' and IsPanVerified='N'" & _
'                        " Union All select 0 as BankVerify,0 as AddressVerify,0 as PanVerify, " & _
'                    " count(*) as Walletreq,0 as bankWithdrawl  from Walletreq where IsApprove='N' Union All select 0 as BankVerify," & _
'                " 0 as AddressVerify,0 as PanVerify, 0 as Walletreq ,count(*) as bankWithdrawl from Fundwithdrawls where Status='P')  as Temp"
'        ElseIf (Session("CompID") = "1078") Then
'            s = "select sum(BankVerify)As BankVerify,sum(AddressVerify)As AddressVerify,sum(PanVerify)as PanVerify, " & _
'                                "sum(BankVerify)+sum(AddressVerify)+sum(PanVerify) TotalVerify,sum(WalletReq)As Walletreq, sum(bankWithdrawl)as bankWithdrawl," & _
'                                " sum(bankWithdrawl)+sum(walletreq)as TotalWallet,0 as Totalpayout from(  " & _
'                                " select Count(0) as BankVerify,0 as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl  from KycVerify  " & _
'                                    " where BankProof<>'' and IsBankVerified='N'  And (BackAddressProof<>'' Or AddrProof <>'') and IsAddrssverified='N'   " & _
'                                    "And PanImg<>'' and IsPanVerified='N' Union All select 0 as BankVerify,0 as AddressVerify,0 as PanVerify, " & _
'                                " count(*) as Walletreq,0 as bankWithdrawl  from Walletreq where IsApprove='N' Union All select 0 as BankVerify," & _
'                            " 0 as AddressVerify,0 as PanVerify, 0 as Walletreq ,count(*) as bankWithdrawl from Monthwithdrawls where Status='P')  as Temp"

'        ElseIf (Session("CompID") = "1093") Then
'            s = "select sum(BankVerify)As BankVerify,sum(AddressVerify)As AddressVerify,sum(PanVerify)as PanVerify, " & _
'                                "sum(BankVerify)+sum(AddressVerify)+sum(PanVerify) TotalVerify,sum(WalletReq)As Walletreq, sum(bankWithdrawl)as bankWithdrawl," & _
'                                " sum(bankWithdrawl)+sum(walletreq)as TotalWallet,0 as Totalpayout from(  " & _
'                                " select Count(0) as BankVerify,0 as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl  from KycVerify  " & _
'                                    " where BankProof<>'' and IsBankVerified='N'  And (BackAddressProof<>'' Or AddrProof <>'') and IsAddrssverified='N'   " & _
'                                    "And PanImg<>'' and IsPanVerified='N' Union All select 0 as BankVerify,0 as AddressVerify,0 as PanVerify, " & _
'                                " count(*) as Walletreq,0 as bankWithdrawl  from Walletreq where IsApprove='N' Union All select 0 as BankVerify," & _
'                            " 0 as AddressVerify,0 as PanVerify, 0 as Walletreq ,count(*) as bankWithdrawl from Monthwithdrawls where Status='P')  as Temp"
'        ElseIf (Session("CompID") = "1007") Then
'            s = "select sum(BankVerify)As BankVerify,sum(AddressVerify)As AddressVerify,sum(PanVerify)as PanVerify,Sum(IsGSTVerified) as IsGSTVerified,sum(uploadform) as uploadform, sum(BankVerify)+sum(AddressVerify)+sum(PanVerify)+Sum(IsGSTVerified)+sum(uploadform)  TotalVerify,sum(WalletReq)As Walletreq, sum(bankWithdrawl)as bankWithdrawl, sum(bankWithdrawl)+sum(walletreq)as TotalWallet,0 as Totalpayout from" & _
'            "( select Count(*) as BankVerify,0 as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl ,0 as IsGSTVerified,0 as uploadform from KycVerify   where BankProof<>'' and IsBankVerified='N'  " & _
'            "Union All " & _
'            "select 0 as BankVerify,count(*)as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl,0 as IsGSTVerified,0 as uploadform  from KycVerify where (BackAddressProof<>'' Or AddrProof <>'') and IsAddrssverified='N' " & _
'            "Union All " & _
'            "select 0 as BankVerify,0 as AddressVerify,count(*)as PanVerify,0 as Walletreq ,0 as bankWithdrawl,0 as IsGSTVerified,0 as uploadform  from KycVerify where PanImg<>'' and IsPanVerified='N'" & _
'            "Union All " & _
'            " select 0 as BankVerify,0 as AddressVerify,0 as PanVerify,  count(*) as Walletreq,0 as bankWithdrawl,0 as IsGSTVerified,0 as uploadform  from Walletreq where IsApprove='N'" & _
'            "Union All " & _
'            " select 0 as BankVerify, 0 as AddressVerify,0 as PanVerify, 0 as Walletreq ,count(*) as bankWithdrawl,0 as IsGSTVerified,0 as uploadform from Fundwithdrawls where Status='P'" & _
'            "union all" & _
'            " select 0 as BankVerify,0 as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl,count(*) as IsGSTVerified,0 as uploadform  from KycVerify   where GStImage<>'' and IsGSTVerified='N'  " & _
'            "union all" & _
'            " select 0 as BankVerify,0 as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl,0 as IsGSTVerified ,count(*) as uploadform from M_FormUpload   where ActiveStatus='N' " & _
'            ")  as Temp"
'        ElseIf (Session("CompID") = "1101") Then
'            s = "select sum(BankVerify)As BankVerify,sum(AddressVerify)As AddressVerify,sum(PanVerify)as PanVerify, " & _
'                       "sum(BankVerify)+sum(AddressVerify)+sum(PanVerify) TotalVerify,sum(WalletReq)As Walletreq, sum(bankWithdrawl)as bankWithdrawl," & _
'                       " sum(bankWithdrawl)+sum(walletreq)as TotalWallet,0 as Totalpayout from(  " & _
'                       " select Count(0) as BankVerify,0 as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl  from KycVerify  " & _
'                           " where BankProof<>'' and IsBankVerified='N'  And (BackAddressProof<>'' Or AddrProof <>'') and IsAddrssverified='N'   " & _
'                           "And PanImg<>'' and IsPanVerified='N' Union All select 0 as BankVerify,0 as AddressVerify,0 as PanVerify, " & _
'                       " count(*) as Walletreq,0 as bankWithdrawl  from Walletreq where IsApprove='N' Union All select 0 as BankVerify," & _
'                   " 0 as AddressVerify,0 as PanVerify, 0 as Walletreq ,count(*) as bankWithdrawl from Dailywithdrawls where Status='P')  as Temp"
'        ElseIf (Session("CompID") = "1103") Then
'            s = "select sum(BankVerify)As BankVerify,sum(AddressVerify)As AddressVerify,sum(PanVerify)as PanVerify, " & _
'                   "sum(BankVerify)+sum(AddressVerify)+sum(PanVerify) TotalVerify,sum(WalletReq)As Walletreq, sum(bankWithdrawl)as bankWithdrawl," & _
'                   " sum(walletreq)as TotalWallet,sum(bankWithdrawl)+sum(walletreq) as Totalpayout from(  " & _
'                   " select Count(0) as BankVerify,0 as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl  from KycVerify  " & _
'                       " where BankProof<>'' and IsBankVerified='N'  And (BackAddressProof<>'' Or AddrProof <>'') and IsAddrssverified='N'   " & _
'                       "And PanImg<>'' and IsPanVerified='N' Union All select 0 as BankVerify,0 as AddressVerify,0 as PanVerify, " & _
'                   " count(*) as Walletreq,0 as bankWithdrawl  from Walletreq where IsApprove='N' Union All select 0 as BankVerify," & _
'               " 0 as AddressVerify,0 as PanVerify, 0 as Walletreq ,count(*) as bankWithdrawl from Fundwithdrawls where Status='P')  as Temp"
'        Else
'            s = "select sum(BankVerify)As BankVerify,sum(AddressVerify)As AddressVerify,sum(PanVerify)as PanVerify, " & _
'                    "sum(BankVerify)+sum(AddressVerify)+sum(PanVerify) TotalVerify,sum(WalletReq)As Walletreq, sum(bankWithdrawl)as bankWithdrawl," & _
'                    " sum(bankWithdrawl)+sum(walletreq)as TotalWallet,0 as Totalpayout from(  " & _
'                    " select Count(0) as BankVerify,0 as AddressVerify,0 as PanVerify,0 as Walletreq ,0 as bankWithdrawl  from KycVerify  " & _
'                        " where BankProof<>'' and IsBankVerified='N'  And (BackAddressProof<>'' Or AddrProof <>'') and IsAddrssverified='N'   " & _
'                        "And PanImg<>'' and IsPanVerified='N' Union All select 0 as BankVerify,0 as AddressVerify,0 as PanVerify, " & _
'                    " count(*) as Walletreq,0 as bankWithdrawl  from Walletreq where IsApprove='N' Union All select 0 as BankVerify," & _
'                " 0 as AddressVerify,0 as PanVerify, 0 as Walletreq ,count(*) as bankWithdrawl from Fundwithdrawls where Status='P')  as Temp"
'        End If



'        dt = objDAL.GetData(s)
'        If dt.Rows.Count > 0 Then
'            If Session("CompId") = 1007 Then
'                uploadform = dt.Rows(0)("uploadform")
'                gstn = dt.Rows(0)("IsGSTVerified")
'            End If

'            TotalVerify = dt.Rows(0)("TotalVerify")
'            Addressverify = dt.Rows(0)("AddressVerify")
'            BankVerify = dt.Rows(0)("BankVerify")
'            PanVerify = dt.Rows(0)("PanVerify")
'            Walletreq = dt.Rows(0)("WalletReq")
'            Totalwallet = dt.Rows(0)("TotalWallet")
'            Totalpayout = dt.Rows(0)("Totalpayout")
'            BankWithdrawl = dt.Rows(0)("bankWithdrawl")

'        End If
'        'da.Fill(dtCat)
'        'html = html & "<div class=""container-page-title"" > <ul id=""menu"">"
'        If dtMenu.Rows.Count > 0 Then
'            For Each dr As DataRow In dtMenu.Rows
'                Dim parentId As String = dr("ParentId")
'                If String.Equals(dr("MenuName"), "-") = False Then
'                    If Val(parentId) = 0 Then
'                        'html = html & "<li><a href=""#"">" & dr("MenuName") & "</a></li>"
'                        Dim MainMenu As String = dr("MenuName")
'                        Dim SubMenu As String = Load_SubMenu(dr("MenuId"), dtMenu)
'                        If dr("OnSelect") = "" Then
'                            If dr("MenuName") = "User" Then
'                                icon = "fa fa-user"
'                                notification = ""
'                            ElseIf dr("MenuName") = "Payouts" Then
'                                icon = "fa  fa-folder-open-o"
'                                'notification = ""
'                                notification = "<span class=""label label-rouded label-custom pull-right"" style=""background-color: #ff2704;"">" & Totalpayout & "</span>"
'                            ElseIf dr("MenuName") = "Wallet" Then
'                                icon = "fa  fa-folder-open-o"
'                                'notification = ""
'                                notification = "<span class=""label label-rouded label-custom pull-right"" style=""background-color: #ff2704;"">" & Totalwallet & "</span>"
'                            ElseIf dr("MenuName") = "Complaint" Then
'                                icon = "fa  fa-folder-open-o"
'                                notification = ""
'                            ElseIf dr("MenuName") = "Master" Then
'                                icon = "fa fa-edit"
'                                notification = ""
'                            ElseIf dr("MenuName") = "Report" Then
'                                icon = "fa fa-list-alt"
'                                notification = ""
'                            ElseIf dr("Menuname") = "Member" Then
'                                icon = "fa fa-sitemap"
'                                notification = ""
'                            ElseIf dr("MenuName") = "Home" Then
'                                icon = "fa fa-table"
'                                notification = ""
'                            ElseIf dr("MenuName") = "KYC Verify" Then
'                                '  If TotalVerify > 0 Then
'                                notification = "<span class=""label label-rouded label-custom pull-right"" style=""background-color: #ff2704;"">" & TotalVerify & "</span>"
'                                'End If
'                            ElseIf dr("menuName") = "LogOut" Then
'                                icon = "fa fa-signout"
'                                notification = ""
'                            End If
'                            'MainMenu = " <li><a><i class=""" & icon & """></i>" & dr("MenuName") & " <span class=""fa fa-chevron-down""> " & _
'                            '          " </span></a> <ul class=""nav child_menu"">" & SubMenu & " </ul></li>"
'                            MainMenu = " <li><a><i class=""" & icon & """></i>" & dr("MenuName") & " <span class=""fa fa-chevron-down""> " & _
'                                   " </span> " & notification & " </a> <ul class=""nav child_menu"">" & SubMenu & " </ul></li>"

'                        Else
'                            If dr("MenuName") = "User" Then
'                                icon = "fa fa-user"
'                            ElseIf dr("MenuName") = "Wallet" Then
'                                icon = "fa  fa-folder-open-o"
'                            ElseIf dr("MenuName") = "Master" Then
'                                icon = "fa fa-edit"
'                            ElseIf dr("MenuName") = "Report" Then
'                                icon = "fa fa-list-alt"
'                            ElseIf dr("Menuname") = "Member" Then
'                                icon = "fa fa-sitemap"
'                            ElseIf dr("MenuName") = "Home" Then
'                                icon = "fa fa-table"
'                                'ElseIf dr("MenuName") = "KYC Verify" Then
'                                '    '  If TotalVerify > 0 Then
'                                '    notification = "<span class=""label label-rouded label-custom pull-right"" style=""background-color: deepskyblue;"">" & TotalVerify & "</span>"

'                            ElseIf dr("menuName") = "LogOut" Then
'                                icon = "fa fa-lock"
'                            End If
'                            MainMenu = "<li><a href=""" & dr("OnSelect") & """><i class=""" & icon & """></i>" & dr("MenuName") & "</a> </li> "


'                        End If
'                        html = html & MainMenu
'                    End If
'                End If
'            Next
'        End If
'        'html = html & "</ul>  </div>"
'        'WUCMenu()

'        menu.InnerHtml = html
'    End Sub


'    Private Function Load_SubMenu(ByVal MenuId As String, ByRef dt As DataTable) As String
'        Dim html As String = ""
'        Try
'            If dt.Rows.Count > 0 Then
'                For Each dr As DataRow In dt.Rows
'                    Dim parentId As String = dr("ParentId")
'                    If Val(parentId) <> 0 And Val(parentId) = Val(MenuId) And String.Equals(dr("MenuName"), "-") = False Then
'                        Dim onselect As String = "'" & dr("OnSelect") & "'"
'                        Dim menuName As String = dr("MenuName")
'                        If menuName = "Address Verify" Then
'                            html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "<span class=""label label-rouded label-custom pull-right"" style=""background-color: #ff2704;"">" & Addressverify & "</span></a></li>"
'                        ElseIf menuName = "Bank Verify" Then
'                            html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "<span class=""label label-rouded label-custom pull-right"" style=""background-color: red;"">" & BankVerify & "</span></a></li>"
'                        ElseIf menuName = "PanCard Verify" Then
'                            html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "<span class=""label label-rouded label-custom pull-right"" style=""background-color: red;"">" & PanVerify & "</span></a></li>"



'                        ElseIf menuName = "Form Verify" Then
'                            If Session("CompId") = 1007 Then
'                                html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "<span class=""label label-rouded label-custom pull-right"" style=""background-color: red;"">" & uploadform & "</span></a></li>"
'                            End If
'                        ElseIf menuName = "GSTIN Verify" Then
'                            If Session("CompId") = 1106 Then
'                                html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "<span class=""label label-rouded label-custom pull-right"" style=""background-color: red;"">" & gstn & "</span></a></li>"
'                            End If


'                        ElseIf menuName = "Approve EP Request" Then
'                            html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "<span class=""label label-rouded label-custom pull-right"" style=""background-color: red;"">" & Walletreq & "</span></a></li>"
'                        ElseIf menuName = "Approve Payment Request" Then
'                            html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "<span class=""label label-rouded label-custom pull-right"" style=""background-color: red;"">" & Walletreq & "</span></a></li>"

'                        ElseIf menuName = "Bank Withdrawal List" Then
'                            html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "<span class=""label label-rouded label-custom pull-right"" style=""background-color: red;"">" & BankWithdrawl & "</span></a></li>"
'                        ElseIf menuName = "Bank Withdrawal" Then
'                            html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "<span class=""label label-rouded label-custom pull-right"" style=""background-color: red;"">" & BankWithdrawl & "</span></a></li>"
'                        ElseIf menuName = "Wallet Authentication" Then
'                            html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "<span class=""label label-rouded label-custom pull-right"" style=""background-color: red;"">" & BankWithdrawl & "</span></a></li>"


'                        Else
'                            html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "</a></li>"
'                        End If


'                        'html = html & "<li><a  href=""" & dr("OnSelect") & """>" & dr("MenuName") & "</a></li>                          "
'                    End If
'                Next

'            End If
'        Catch ex As Exception
'            Response.Write(ex.Message)
'        End Try
'        Return html
'    End Function

'End Class
