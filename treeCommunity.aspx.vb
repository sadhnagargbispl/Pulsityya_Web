Imports System.Data.SqlClient
Imports System.Data
Partial Class treeCommunity
    Inherits System.Web.UI.Page
    Dim strQuery As String
    Dim minDeptLevel As Integer

    Dim reentry As Integer
    Dim conn As New SqlConnection
    Dim Comm As New SqlCommand
    Dim Adp1 As SqlDataAdapter
    Dim dtData As New DataTable
    Dim strDrawKit As String
    Dim objDal As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Private Sub ValidateTree()
        Dim strSelectedFormNo As String
        '---- If User Not Set MinDept level than set 10 dept level
        ''getKits()
        If Request("deptlevel") <> "" Then
            minDeptLevel = Convert.ToInt32(Request("deptlevel"))
        Else
            minDeptLevel = 3
        End If
        If Request("entry") <> "" Then
            reentry = Convert.ToInt32(Request("entry"))
        Else
            reentry = 1
        End If

        If (Request("DownLineFormNo") = "") Then
            lblError.Text = "Please Enter Member Id in respective field."
            lblError.Visible = True
        Else
            'If CheckDownLineMemberTree() = False Then

            '    lblError.Text = "Please Check DownLine Member ID."
            '    lblError.Visible = True
            'End If
            'Label1.Text = "Not Match :" & Request("DownLineFormNo") & " Fromno :" & Session("FORMNO")
            strSelectedFormNo = Request("DownLineFormNo")

            strQuery = getQuery(strSelectedFormNo, minDeptLevel, reentry)
            GenerateTree(strQuery)
        End If
        ''getKits()

    End Sub



    Private Function CheckDownLineMemberTree() As Boolean
        Dim chk As New Boolean
        chk = False
        strQuery = " Select FormnoDwn FROM M_PoolTreeRelation WHERE  FormNo=" & Request("DownLineFormNo")



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
        'conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'conn.Open()
        'Comm = New SqlCommand(strQuery, conn)
        'Comm.CommandTimeout = 100000000
        'Adp1 = New SqlDataAdapter(Comm)
        'Adp1.Fill(dsGetQry)
        dtData = New DataTable
        dtData = objDal.GenerateTreeProc(strQuery)

        Dim ParentId As Double
        Dim FormNo As Double
        Dim MemberName As String
        Dim LegNo As String
        Dim Doj As String = ""
        Dim Category As String = ""
        Dim LeftBV, RightBV As Double
        Dim LeftJoining, RightJoining As Double
        Dim UpLiner As String = ""
        Dim Sponsor As String = ""
        Dim IMECode As String = ""
        Dim CorpusName As String = ""
        Dim level As Integer
        Dim NodeName As String
        Dim myRunTimeString As String = ""
        Dim ExpandYesNo As String
        Dim strImageFile As String
        Dim strUrlPath As String = ""
        Dim UpDt As String
        Dim tooltipstrig As String
        Dim idno As String = ""
        Dim Id As String = ""
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
        Dim i As Integer = 0
        'myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" & strImageFile & "');" + vbNewLine
        Dim LoopValue As Integer
        Dim FolderFile As String = "images/Deactivate.jpg"

        For Each dr As DataRow In dtData.Rows
            strImageFile = "images/" & dr.Item("JoinColor").ToString
            If i = 0 Then
                myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" & strImageFile & "');" + vbNewLine
                i = i + 1
            End If
            ParentId = dr.Item("UPLNFORMNO").ToString
            FormNo = Val(dr.Item("FormNoDwn").ToString)
            LegNo = dr.Item("legno").ToString
            UpLiner = dr.Item("UpLiner").ToString
            Sponsor = dr.Item("Sponsor").ToString
            If Session("compid") = 1067 Then
                Doj = dr.Item("LevelDate").ToString
            Else
                Doj = dr.Item("doj").ToString
            End If
            ' Doj = dr.Item("doj").ToString
            Category = dr.Item("Category").ToString
            LeftBV = dr.Item("LeftBV").ToString
            RightBV = dr.Item("rightBV").ToString
            LeftJoining = dr.Item("Leftjoining").ToString
            RightJoining = dr.Item("rightjoining").ToString
            level = dr.Item("level").ToString
            IMECode = dr.Item("IMECode").ToString
            'CorpusName = dr.Item("CorpusName").ToString
            strUrlPath = "Unitree.aspx?DownLineFormNo=" & FormNo
            UpDt = dr.Item("UpDt").ToString
            MemberName = "(" & dr.Item("Formno").ToString & ")" & "(" & dr.Item("memName") & ")"
            NodeName = dr.Item("memName").ToString
            LoopValue = dr.Item("mlevel")
            idno = dr.Item("Formno").ToString
            If Session("CompID") = 1061 Then
                Id = "(" & dr.Item("Formno").ToString & ")" & "<br>" & "(" & dr.Item("memName") & ")"
            Else
                Id = dr.Item("Formno").ToString
            End If
            If LoopValue < 4 And LoopValue > 0 Then
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
                    MemberName = "Left"
                Else
                    MemberName = "Right"
                End If
            Else
                If dr("ActiveStatus") = "N" Then
                    strImageFile = "images/deact.jpg"
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

                strUrlPath = "treeCommunity.aspx?DownLineFormNo=" & FormNo
            End If

            strImageFile = "images/" & dr.Item("JoinColor").ToString
            myRunTimeString = myRunTimeString + " mytree.add(" & FormNo & "," & ParentId & "," & "'" & _
                                                                Category & "'" & "," & "'" & Doj & "','" & _
                                                                MemberName & "','" & NodeName & "','" & _
                                                                UpLiner & "'" & ",'" & Sponsor & "'," & _
                                                                LeftBV & "," & RightBV & "," & "'" & _
                                                                strUrlPath & "'" & ",'" & MemberName & _
                                                                "'," & "'" & "" & "'" & "," & "'" & _
                                                                strImageFile & "'" & "," & "'" & _
                                                                strImageFile & "'" & "," & ExpandYesNo & ",'" & _
                                                                LeftJoining & "','" & RightJoining & "','" & IMECode & "','" & _
                                                                level & "' ,'" & UpDt & "','" & CorpusName & "','" & idno & "','" & Id & "');" + vbNewLine
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


    Private Function getQuery(ByVal strSelectedFormNo As String, ByVal minDeptLevel As Integer, ByVal reentry As Integer) As String
        '---- check if user pass downline member than .. make according to downline member other wise show his tree
        ' getQuery = "exec sp_UnivShowTree " & strSelectedFormNo & "," & minDeptLevel
        getQuery = "exec sp_CommunityUnivShowTree " & strSelectedFormNo & "," & minDeptLevel & "," & reentry & ""
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
End Class
