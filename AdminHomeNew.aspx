<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="AdminHomeNew.aspx.vb" Inherits="App_UI_Application_Pages_AdminHomeNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">

        window.addEventListener('mouseup', function(event) {
            var box = document.getElementById('contextMenu');
            var Btn = document.getElementById('DivOptions');
            if (event.target != Btn && event.target.parentnode != box) {
                box.style.display = 'none';
            }
        });

        function ShowMenu(Dv, control, e) {
        debugger ;
            var posx = e.clientX + window.pageXOffset + 'px'; //Left Position of Mouse Pointer
            var posy = e.clientY + window.pageYOffset + 'px'; //Top Position of Mouse Pointer
            var StrArr = Dv.id.split(';');
            var el = StrArr[0];
            var IsActive = StrArr[1];
            var IsBlock = StrArr[2];
            var Site = StrArr[3];
            var Passw = StrArr[4];
            var compid = StrArr[5];
            document.getElementById(control).style.position = 'absolute';
            document.getElementById(control).style.display = 'inline';
            document.getElementById(control).style.left = posx;
            document.getElementById(control).style.top = posy;
            var currentdate = new Date();
            var TmID = currentdate.getDate().toString() + currentdate.getHours().toString() + currentdate.getFullYear().toString() + currentdate.getMonth().toString();
            //alert(compid);
            var inHtm = '';
           if (compid == 1093) 
           {
    inHtm = '<li><a href="https://sollywood.in/Login.aspx?lgnT=' + Passw + '&ID=' + TmID + '" target="_blank">View Account</a></li>';
} 
else if (compid == 1102) 
{
    inHtm = '<li><a href="' + Passw + '" target="_blank">View Account</a></li>';
}
//else if (compid == 1106) 
//{
//    inHtm = '<li><a href="' + Passw + '" target="_blank">View Account</a></li>';
//}
else {
if (compid == 1101) 
{
inHtm = '<li><a href="' + Site + '/Default.aspx?lgnT=' + Passw + '&ID=' + TmID + '" target="_blank">View Account</a></li>';
inHtm = inHtm + '<li class="separator"><a href="Binarytree.aspx?key=' + el + '">Level Tree</a></li>';
}
else {
inHtm = '<li><a href="' + Site + '/Default.aspx?lgnT=' + Passw + '&ID=' + TmID + '" target="_blank">View Account</a></li>';
}
    
}
//            
            //inHtm = inHtm + ' <li class="separator"><a href="Profile.aspx?key=' + el + '">Update Profile</a></li>';
            //below commit 28 Oct 2022
            //inHtm = inHtm + '<li class="separator"><a href="Binarytree.aspx?key=' + el + '">View Tree</a></li>';

            //            if (IsBlock == 'Y')
            //                inHtm = inHtm + '<li class="separator"><a href="UnBlock.aspx?Tp=S&key=' + el + '">Unblock</a></li>';
            //            else
            //                inHtm = inHtm + '<li class="separator"><a href="Block.aspx?Tp=S&key=' + el + '">Block Now</a></li>';
            document.getElementById(control).innerHTML = inHtm
        }
    </script>

    <style type="text/css">
        .ContextItem /*Context Menu Item Style */
        {
            width: 150px;
            background: #337ab7;
            color: Black;
            font-weight: normal; /*border: solid 1px #CCC;*/
            text-align: left;
        }
        .ContextItem LI
        {
            width: 150px;
            list-style: none;
            padding: 0px;
            margin: 0px;
        }
        .ContextItem LI:hover
        {
            font-weight: bold;
            background: #C2CEEB;
            width: 110px;
        }
        .ContextItem A
        {
            width: 150px;
            color: #fff;
            text-decoration: none;
            line-height: 20px;
            height: 20px; /*padding: 1px 5px;
	padding-left: 18px;*/
        }
        .ContextItem LI.separator
        {
            border-top: solid 0px #CCC;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Home</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-2">
                                    <h3 style="font-family: Montserrat, sans-serif; font-size: medium">
                                        Search Records By :</h3>
                                </div>
                                <div class="col-md-3">
                                    <% If Session("CompID") = 1007 Then%>
                                    <asp:DropDownList ID="ddlSearch1" runat="server" class="form-control">
                                        <asp:ListItem Value="0" Selected="True">Search Type</asp:ListItem>
                                        <asp:ListItem Value="IDNo">Distributor ID</asp:ListItem>
                                        <asp:ListItem Value="MemName">Name</asp:ListItem>
                                        <asp:ListItem Value="City">City</asp:ListItem>
                                        <asp:ListItem Value="StateName">State</asp:ListItem>
                                        <asp:ListItem Value="Mobl">Mobile</asp:ListItem>
                                        <asp:ListItem Value="EMail">Email</asp:ListItem>
                                        <asp:ListItem Value="DOJ">Joining Date</asp:ListItem>
                                        <asp:ListItem Value="KitName">Package Name</asp:ListItem>
                                        <asp:ListItem Value="Panno">PanCard No</asp:ListItem>
                                        <asp:ListItem Value="Aadharno">Aadhar No</asp:ListItem>
                                        <asp:ListItem Value="acno">Account No</asp:ListItem>
                                    </asp:DropDownList>
                                    <%Else%>
                                    <asp:DropDownList ID="ddlSearch" runat="server" class="form-control">
                                        <asp:ListItem Value="0" Selected="True">Search Type</asp:ListItem>
                                        <asp:ListItem Value="IDNo">Distributor ID</asp:ListItem>
                                        <asp:ListItem Value="MemName">Name</asp:ListItem>
                                        <asp:ListItem Value="City">City</asp:ListItem>
                                        <asp:ListItem Value="StateName">State</asp:ListItem>
                                        <asp:ListItem Value="Mobl">Mobile</asp:ListItem>
                                        <asp:ListItem Value="EMail">Email</asp:ListItem>
                                        <asp:ListItem Value="DOJ">Joining Date</asp:ListItem>
                                        <asp:ListItem Value="KitName">Package Name</asp:ListItem>
                                        <asp:ListItem Value="Panno">PanCard No</asp:ListItem>
                                    </asp:DropDownList>
                                    <%End If%>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtSrchText" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="BtnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                    <asp:Button ID="btnExportCsv" runat="server" class="btn btn-primary" Text="Export To Csv"
                                        Visible="false" /></div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                </div>
                            </div>
                            <div style="overflow: scroll" class="table table-bordered">
                                <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                    AutoGenerateColumns="False" ForeColor="Black" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    EmptyDataText="No data to display." GridLines="None" AllowSorting="true" OnSorting="GvData_Sorting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SNo.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Member ID" SortExpression="IdNo" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="MemberID" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" SortExpression="Qstr" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <%#Eval("Name1")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sponsor ID" SortExpression="SponsorId" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="SponsorId" runat="server" Text='<%# Eval("SponsorId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sponsor Name" SortExpression="SponsorName" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="SponsorName" runat="server" Text='<%# Eval("SponsorName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PresenterId" SortExpression="PresenterId" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpresenterid" runat="server" Text='<%# Eval("PresenterId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Joining Date" ControlStyle-Width="150px" ItemStyle-Width="150px"
                                            SortExpression="Doj" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Doj" runat="server" Text='<%# Eval("Doj") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Click Here!" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <div id="DivOptions">
                                                    <div id='<%# Eval("IDNo") &";"& Eval("ActiveStatus") &";"& Eval("IsBlock") &";"& Eval("Site") &";"& Eval("LgnID")&";"& session("compid")  %>'
                                                        style="background-color: #99CCCC; color: Black; font-weight: bold; font-size: 12px"
                                                        onclick="ShowMenu(this,'contextMenu',event);">
                                                        <asp:Image runat="server" ID="Image1" ImageUrl="~/img/moreopt.jpg" AlternateText="More Option.." />
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No." SortExpression="MobileNo" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Mobile" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email" SortExpression="Email" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Email" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Activ.Date Plan A" SortExpression="UpgrdDate" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="UpgradeDateA" runat="server" Text='<%# Eval("UpgradeDateA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Package Plan A" SortExpression="KitName" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Package" runat="server" Text='<%# Eval("KitNamePlanA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Activ.Date Plan B" SortExpression="UpgrdDate" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="UpgrdDate" runat="server" Text='<%# Eval("UpgrdDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                      <%--  <asp:TemplateField HeaderText="Package Amount Plan A" SortExpression="KitAmount" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="PackageAmount" runat="server" Text='<%# Eval("KitAmountPlanA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Package Plan B" SortExpression="KitName" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Package" runat="server" Text='<%# Eval("KitName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <%-- <asp:TemplateField HeaderText="Package Amount Plan B" SortExpression="KitAmount" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="PackageAmount" runat="server" Text='<%# Eval("KitAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                     <%--   <asp:TemplateField SortExpression="BV">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHeader" runat="server" Text="Package BV" ForeColor="White"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="PackageBv" runat="server" Text='<%# Eval("Bv") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <%--<asp:TemplateField HeaderText="Package BV" SortExpression="BV" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="PackageBv" runat="server" Text='<%# Eval("Bv") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="City" Visible="false" SortExpression="City" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="City" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="State" SortExpression="StateName" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="State" runat="server" Text='<%# Eval("StateName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Country" SortExpression="CountryName" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="country" runat="server" Text='<%# Eval("CountryName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       <%-- <asp:TemplateField HeaderText="Std Code" SortExpression="StdCode" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="StdCode" runat="server" Text='<%# Eval("StdCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Password" Visible="false" SortExpression="Passw">
                                            <ItemTemplate>
                                                <asp:Label ID="Password" runat="server" Text='<%# Eval("Passw") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="Panno" SortExpression="Panno" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Panno" runat="server" Text='<%# Eval("Panno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" SortExpression="Status" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Status" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="E-Wallet" Visible="false" SortExpression="Balance"
                                            HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Wallet" runat="server" Text='<%# Eval("Balance") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aadhar No" SortExpression="Aadharno" HeaderStyle-ForeColor="White"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Status" runat="server" Text='<%# Eval("Aadharno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Account No" SortExpression="Aadharno" HeaderStyle-ForeColor="White"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Account" runat="server" Text='<%# Eval("acno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <!-- end of weather widget -->
        </div>
    </div>
    <ul id="contextMenu" class="ContextItem">
    </ul>
</asp:Content>
