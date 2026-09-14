Imports System.Data.SqlClient
Imports System.Data
Partial Class Fixedtree
    Inherits System.Web.UI.Page
    Dim strQuery As String
    Dim minDeptLevel As Integer
    Dim conn As New SqlConnection
    Dim Comm As New SqlCommand
    Dim Adp1 As SqlDataAdapter
    Dim dtData As New DataTable
    Dim strDrawKit As String
    Dim objDal As DAL

    Private Sub ValidateTree()
        Dim strSelectedFormNo As String
        '---- If User Not Set MinDept level than set 10 dept level
        ''getKits()
        If Request("deptlevel") <> "" Then
            minDeptLevel = Convert.ToInt32(Request("deptlevel"))
        Else
            minDeptLevel = 3
        End If


        If (Request("DownLineFormNo") = "") Then
            'lblError.Text = "Please Enter Member Id in respective field."
            'lblError.Visible = True
        Else
            If CheckDownLineMemberTree() = False Then
                'Response.Write("Please Check DownLine Member ID")
                'Response.End()
                'lblError.Text = "Please Check DownLine Member ID."
                'lblError.Visible = True
            End If
            'Label1.Text = "Not Match :" & Request("DownLineFormNo") & " Fromno :" & Session("FORMNO")
            strSelectedFormNo = Request("DownLineFormNo")

            strQuery = getQuery(strSelectedFormNo, minDeptLevel)
            GenerateTree(strQuery)
        End If
        ''getKits()

    End Sub


    'Protected Sub BtnStepAbove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnStepAbove.Click
    '    Dim scrname As String = ""
    '    If Not Session("Upliner") Is Nothing Then
    '        Dim uplnformno As String = Session("Upliner")
    '        If uplnformno = 0 Then
    '            scrname = "<SCRIPT language='javascript'>alert('No Upliner Id !! ');" & "</SCRIPT>"
    '            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Incorrect", scrname, False)
    '            Exit Sub
    '        Else
    '            TreeFrame.Attributes.Item("src") = "Newtree.aspx?DownLineFormNo=" & uplnformno & "&deptlevel=" & Val(4)
    '        End If

    '    End If

    'End Sub
    'Private Sub getKits()
    '    Dim dtKit As New DataTable
    '    Dim qry As String
    '    objDal = New DAL()
    '    qry = "Select * from " & objDal.tblKitMaster & "  where ActiveStatus='Y' AND RowStatus='Y' Order by KitAmount"
    '    dtKit = objDal.GetData(qry)

    '    If (dtKit.Rows.Count > 0) Then
    '        img11.Visible = True
    '        img11.ImageUrl = "../Resources/images/" & dtKit.Rows(0)("JoinColor")
    '        td21.InnerText = dtKit.Rows(0)("KitAmount")

    '        If (dtKit.Rows.Count > 1) Then
    '            img12.Visible = True
    '            img12.ImageUrl = "../Resources/images/" & dtKit.Rows(1)("JoinColor")
    '            td22.InnerText = dtKit.Rows(1)("KitAmount")

    '            If (dtKit.Rows.Count > 2) Then
    '                img13.Visible = True
    '                img13.ImageUrl = "../Resources/images/" & dtKit.Rows(2)("JoinColor")
    '                td23.InnerText = dtKit.Rows(2)("KitAmount")

    '                If (dtKit.Rows.Count > 3) Then
    '                    img14.Visible = True
    '                    img14.ImageUrl = "../Resources/images/" & dtKit.Rows(3)("JoinColor")
    '                    td24.InnerText = dtKit.Rows(3)("KitAmount")

    '                    If (dtKit.Rows.Count > 4) Then
    '                        img15.Visible = True
    '                        img15.ImageUrl = "../Resources/images/" & dtKit.Rows(4)("JoinColor")
    '                        td25.InnerText = dtKit.Rows(4)("KitAmount")

    '                        If (dtKit.Rows.Count > 5) Then
    '                            img16.Visible = True
    '                            img16.ImageUrl = "../Resources/images/" & dtKit.Rows(5)("JoinColor")
    '                            td26.InnerText = dtKit.Rows(5)("KitAmount")

    '                            If (dtKit.Rows.Count > 6) Then
    '                                img17.Visible = True
    '                                img17.ImageUrl = "../Resources/images/" & dtKit.Rows(6)("JoinColor")
    '                                td27.InnerText = dtKit.Rows(6)("KitAmount")

    '                                If (dtKit.Rows.Count > 7) Then
    '                                    img18.Visible = True
    '                                    img18.ImageUrl = "../Resources/images/" & dtKit.Rows(7)("JoinColor")
    '                                    td28.InnerText = dtKit.Rows(7)("KitAmount")
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End If
    '    End If

    'End Sub

    Private Function CheckDownLineMemberTree() As Boolean
        Dim chk As New Boolean
        chk = False
        strQuery = " Select FormnoDwn FROM M_MemTreeRelation WHERE FormNo=" & Request("DownLineFormNo")
        'Dim conn As New SqlConnection
        'Dim Comm As New SqlCommand
        'Dim Adp1 As SqlDataAdapter
        'Dim ds1 As New DataSet
        'conn = New SqlConnection(Application("Connect"))
        'conn.Open()
        'Comm = New SqlCommand(strQuery, conn)
        'Adp1 = New SqlDataAdapter(Comm)
        'Adp1.Fill(ds1)

        dtData = New DataTable
        dtData = objDal.GetData(strQuery)
        If dtData.Rows.Count <= 0 Then
            chk = False
        Else
            chk = True
        End If
        Return chk
        ' ds1.Dispose()
    End Function

    Private Sub GenerateTree(ByVal strQuery As String)
        'conn = New SqlConnection(Application("Connect"))
        'conn.Open()
        'Comm = New SqlCommand(strQuery, conn)
        'Comm.CommandTimeout = 100000000
        'Adp1 = New SqlDataAdapter(Comm)
        'Adp1.Fill(dsGetQry)
        dtData = New DataTable
        dtData = objDal.GenerateTreeProc(strQuery)
        Dim dt1 As DataTable
        dt1 = New DataTable
        Dim ParentId As Double
        Dim FormNo As Double
        Dim MemberName As String
        Dim LegNo As String
        Dim Doj As String = ""
        Dim Category As String = ""
        Dim LeftBV, RightBV As Double
        Dim Currentsessionleftbv, Currentsessionrightbv As Double
        Dim LeftJoining, RightJoining, CurrentLeftPV, CurrentRightPV As Double
        Dim UpLiner, Sponsor As String
        Dim level As Integer
        Dim NodeName, IdNo As String
        Dim myRunTimeString As String = ""
        Dim ExpandYesNo As String
        Dim strImageFile As String
        Dim strUrlPath As String = ""
        Dim UpDt As String
        Dim Totalbv As String = ""
        Dim City As String = ""
        Dim Cadre As String = ""
        Dim Totalcurrentbv As String = ""
        Dim ActiveDirect, ActiveIndirect As Integer
        Dim LeftCarryBv, RightCarryBv As Double
        Dim tooltipstrig As String
        Dim Block As String = ""
        Dim BlockStatus As String = ""
        Dim UplinerId As String = ""
        Dim url As String = ""
        Dim Target_ As String = "_self"
        Dim LeftVirtualBv, RightVirtualBv, SelfVirtualBv, LeftRp, RightRp As Double
        Dim profilepic As String = ""
        Dim LeftMatchingBv As String = ""

        Dim RightMatchingBv As String = ""
        myRunTimeString = myRunTimeString + "<Script Language=Javascript>" + vbNewLine
        tooltipstrig = ToolTipTable()

        '--- Define Parent Setting ----------------
        ParentId = -1

        If Request("DownLineFormNo") <> "" Then
            FormNo = Val(Request("DownLineFormNo"))
        Else
            'FormNo = Val(Session("FormNo").ToString)
        End If
        strImageFile = "images/base.jpg"
        Dim UplineFormno As String = ""
        Dim i As Integer = 0
        'myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" & strImageFile & "');" + vbNewLine
        Dim LoopValue As Integer
        Dim FolderFile As String = "images/Deactivate.jpg"
        Dim activedirect1 As Double = 0
        Dim activeInDirect1 As Double = 0

        For Each dr As DataRow In dtData.Rows
            strImageFile = "img/" & dr.Item("JoinColor").ToString
            If i = 0 Then
                If Request("DownLineFormNo") <> "" And i = 0 Then
                    Session("Upliner") = dr.Item("uplinerformno").ToString
                Else
                    Session("Upliner") = Nothing
                End If
                myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" & strImageFile & "');" + vbNewLine
                i = i + 1
            End If
            ParentId = dr.Item("UPLNFORMNO").ToString
            FormNo = Val(dr.Item("FormNoDwn").ToString)
            LegNo = dr.Item("legno").ToString
            UpLiner = dr.Item("UpLiner").ToString
            Sponsor = dr.Item("Sponsor").ToString
            Doj = dr.Item("doj").ToString
            Category = dr.Item("Category").ToString
            LeftBV = dr.Item("LeftBV")
            RightBV = dr.Item("rightBV")
            LeftJoining = dr.Item("Leftjoining").ToString
            RightJoining = dr.Item("rightjoining").ToString
            ActiveDirect = dr.Item("LeftActive").ToString
            ActiveIndirect = dr.Item("RightActive").ToString
            IdNo = dr.Item("Formno").ToString
            'LeftCarryBv = dr.Item("LeftCarryForwardBv").ToString
            'RightCarryBv = dr.Item("RightCarryForwardBv").ToString
            Block = dr.Item("IsBlock").ToString
            BlockStatus = dr.Item("BlockedStatus").ToString
            level = dr.Item("level").ToString
            ' strUrlPath = "NewJoining.aspx?DownLineFormNo=" & FormNo
            'strUrlPath = "newtree.aspx?DownLineFormNo=" & FormNo
            UpDt = dr.Item("UpDt").ToString
            MemberName = dr.Item("Formno").ToString
            NodeName = dr.Item("memName").ToString
            LoopValue = dr.Item("mlevel")
            If dr.Item("Profilepic").ToString.Contains("http") Then
                profilepic = dr.Item("Profilepic").ToString + ""

            Else
                profilepic = Session("CompWeb") + dr.Item("Profilepic").ToString + ""


            End If
            Totalbv = Val(dr.Item("LeftBV")) + Val(dr.Item("rightBV"))
            City = dr.Item("City")
            'Cadre = dr.Item("Cadre")
            '   LeftVirtualBv = 
            '  RightVirtualBv = dr.Item("RightVirtualBv")
            ' SelfVirtualBv =
            'Currentsessionleftbv = dr.Item("Currentsessionleftbv").ToString
            'Currentsessionrightbv = dr.Item("Currentsessionrightbv").ToString
            'Totalcurrentbv = Val(dr.Item("Currentsessionleftbv")) + Val(dr.Item("Currentsessionrightbv"))
            LeftRp = dr.Item("LeftRp")
            RightRp = dr.Item("RightRp")
            'LeftMatchingBv = dr.Item("LeftMatchedBv")
            'RightMatchingBv = dr.Item("RightMatchedBv")
            activedirect1 = Val(dr.Item("Direct"))
            activeInDirect1 = Val(dr.Item("Indirect"))
            'If Val(ParentId) = -1 Then
            '    Dim sql As String = "select UpLnFormNo from M_MemberMaster where Formno='" & FormNo & "'"
            '    objDal = New DAL
            '    dt1 = objDal.GetData(sql)
            '    If dt1.Rows.Count > 0 Then
            '        UplineFormno = dt1.Rows(0)("UpLnFormNo")
            '        If UplineFormno = 1 Then
            '            'lblError.Text = " No Upliner"
            '        Else
            '            Session("Upliner") = UplineFormno
            '        End If
            '    End If
            'Else
            '    Session("Upliner") = ParentId

            'End If

            ID = dr.Item("Formno").ToString

            If LoopValue > 0 Then
                ExpandYesNo = "true"
            Else
                ExpandYesNo = "false"
            End If
            If ParentId = -1 Then
                ExpandYesNo = "true"
            End If

            If UpDt = "01 Jan 00" Then
                UpDt = ""
            End If

            If FormNo <= 0 Then
                'If dr("ActiveStatus") = "N" Then
                '    strImageFile = "img/deact.jpg"
                'ElseIf dr("Kitid") = 1 Then
                '    strImageFile = "img/act.jpg"
                'Else
                '    strImageFile = "img/deact.jpg"
                'End If

                strUrlPath = "" ''"newjoining.aspx?UpLnFormNo=" & ParentId & "&legno=" & LegNo
                If LegNo = 1 Then
                    MemberName = "Blank"
                Else
                    MemberName = "Blank"
                End If
            Else
                If dr("ActiveStatus") = "N" Then
                    strImageFile = "images/Red.jpg"
                ElseIf dr("Kitid") = 1 Then
                    strImageFile = "images/Red.jpg"
                ElseIf dr("Kitid") = 2 Then
                    strImageFile = "images/Blue.jpg"
                ElseIf dr("Kitid") = 3 Then
                    strImageFile = "images/Green.jpg"
                ElseIf dr("Kitid") = 4 Then
                    strImageFile = "images/Yellow.jpg"
                ElseIf dr("Kitid") = 5 Then
                    strImageFile = "images/Orange.jpg"
                ElseIf dr("Kitid") = 6 Then
                    strImageFile = "images/purpel.jpg"
                Else
                    strImageFile = "images/empty.jpg"

                End If

                strUrlPath = "fixedtree.aspx?DownLineFormNo=" & FormNo
            End If

            strImageFile = "img/" & dr.Item("JoinColor").ToString
            myRunTimeString = myRunTimeString + " mytree.add(" & FormNo & "," & ParentId & "," & "'" & _
                                                                   Category & "'" & "," & "'" & Doj & "','" & _
                                                                   MemberName & "','" & NodeName & "','" & _
                                                                   UpLiner & "'" & ",'" & Sponsor & "'," & _
                                                                   LeftBV & "," & RightBV & "," & ActiveDirect & "," & ActiveIndirect & "," & "'" & _
                                                                   strUrlPath & "'" & ",'" & MemberName & _
                                                                   "'," & "'" & Target_ & "'" & "," & "'" & _
                                                                   strImageFile & "'" & "," & "'" & _
                                                                   strImageFile & "'" & "," & ExpandYesNo & ",'" & _
                                                                   LeftJoining & "','" & RightJoining & "','" & _
                                                                   level & "' ,'" & UpDt & "','" & IdNo & "','" & LeftCarryBv & "','" & RightCarryBv & "'," & _
                                                                   " '" & Block & "','" & BlockStatus & "','" & CurrentLeftPV & "','" & CurrentRightPV & "'," & _
                                                                   " '" & LeftVirtualBv & "','" & RightVirtualBv & "','" & SelfVirtualBv & "','" & profilepic & "','" & Totalbv & "','" & Currentsessionleftbv & "','" & Currentsessionrightbv & "','" & Totalcurrentbv & "'," & _
                                                                   " '" & City & "','" & Cadre & "','" & LeftMatchingBv & "','" & RightMatchingBv & "','" & LeftRp & "','" & RightRp & "','" & activedirect1 & "','" & activeInDirect1 & "');" + vbNewLine
        Next




        myRunTimeString = myRunTimeString + vbNewLine & vbNewLine & vbNewLine & vbNewLine & " document.write(mytree);" + vbNewLine
        myRunTimeString = myRunTimeString + vbNewLine & "</script> " + "<br /> <br /> <br /> <br /> "

        RegisterClientScriptBlock("clientScript", myRunTimeString)


    End Sub
    Private Function ToolTipTable() As String
        Dim strToolTip As String
        'strToolTip = "onMouseOver=""ddrivetip('<table width=100% border=0 cellpadding=5 cellspacing=1 bgcolor=#CCCCCC class=containtd>  <tr>     <td width=50% bgcolor=#999999><font color=#FFFFFF><strong>Member ID</strong></font></td>  </tr>  <tr>     <td>430</td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Name</strong></font></td>  </tr>  <tr>     <td>Mr-MAHESH BHARDWAJ </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Date of Joining</strong></font></td>  </tr>  <tr>     <td>2008-08-07 16:14:54 </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Total Status</strong></font></td>  </tr>  <tr>     <td>LEFT:123 , RIGHT:2198 </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Product</strong></font></td>  </tr>  <tr>     <td>CODE NO. 01-S.L. &nbsp;</td>  </tr></table>')"" onMouseOut=""hideddrivetip()"""
        Return strToolTip
    End Function


    Private Function getQuery(ByVal strSelectedFormNo As String, ByVal minDeptLevel As Integer) As String
        '---- check if user pass downline member than .. make according to downline member other wise show his tree
        getQuery = "exec sp_ShowTree1 " & strSelectedFormNo & "," & minDeptLevel
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                ' Call getKits()
            Else
                Response.Redirect("Default.aspx")
            End If
            Call ValidateTree()
        End If
    End Sub




    Private Function get_FormNo(ByVal IDNo As String) As String
        'conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'conn.Open()
        Try
            Dim objDal1 As DAL

            objDal1 = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            Dim FormNo As String = ""
            'Dim dr As SqlDataReader
            Dim strQuery As String = "Select FormNo From M_MemberMaster Where IDNo='" & IDNo & "'"
            dtData = New DataTable
            dtData = objDal1.GetData(strQuery)
            If dtData.Rows.Count > 0 Then
                FormNo = dtData.Rows(0)("FormNo")
            End If
            'dr = Comm.ExecuteReader
            'If dr.Read = True Then
            '    FormNo = dr("FormNo")
            'End If
            'dr.Close()
            'Comm.Cancel()
            'conn.Close()
            Return FormNo
        Catch ex As Exception

        End Try
    End Function

    Protected Sub cmdSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave1.Click
        Try
            Dim scrname As String = ""
            Dim DownFormNo As String = get_FormNo(DownLineFormNo.Value)
            If DownFormNo <> "" Then
                Response.Redirect("fixedtree.aspx?DownLineFormNo=" & DownFormNo)
            Else
                scrname = "<SCRIPT language='javascript'>alert('Invalid distributor id');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("home.aspx")
    End Sub
End Class
