<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="AdminIndex.aspx.vb" Inherits="App_UI_Application_Pages_AdminIndex"
     %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

   <link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/JS/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/JS/jquery.contextMenu.js" type="text/javascript"></script>

    <link href="../Resources/CSS/jquery.contextMenu.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function() {
            $("#ctl00_ContentPlaceHolder1_GvData div").contextMenu({
                menu: 'myMenu'
            },
			function(action, el, pos) {
            if (action == 'Profile') { window.location.href = 'Profile.aspx?key=' + $(el).attr('id'); }
            else if (action == 'Account') { window.location.href = 'Account.aspx?key=' + $(el).attr('id'); }
//            else if (action == 'Distr') { window.location.href = 'BecomeDistributor.aspx?key=' + $(el).attr('id'); }
//            else if (action == 'Imgs') { window.location.href = 'UploadedPhotos.aspx?key=' + $(el).attr('id'); }
			    else if (action == 'Tree') { window.location.href = 'BinaryTree.aspx?key=' + $(el).attr('id'); }
			    else if (action == 'Activate') { window.location.href = 'ActivateID.aspx?key=' + $(el).attr('id'); }
//			    else if (action == 'Tax') { window.location.href = 'TaxInvoice.aspx?key=' + $(el).attr('id'); }
//			    else if (action == 'Cour') { window.location.href = 'CourierDetail.aspx?key=' + $(el).attr('id'); }
			    else if (action == 'SMS') { window.location.href = 'SMSSending.aspx?key=' + $(el).attr('id'); }
			    else if (action == 'Block') { window.location.href = 'Block.aspx?Tp=S&key=' + $(el).attr('id'); }
			    else if (action == 'BlockTree') { window.location.href = 'Block.aspx?Tp=M&key=' + $(el).attr('id'); }
			    else if (action == 'UnBlock') { window.location.href = 'UnBlock.aspx?Tp=S&key=' + $(el).attr('id'); }
			    else if (action == 'UnblockTree') { window.location.href = 'UnBlock.aspx?Tp=M&key=' + $(el).attr('id'); }
//			    else if (action == 'Reward') { window.location.href = 'RewardDetail.aspx?key=' + $(el).attr('id'); }
			    else if (action == 'Incentive') { window.location.href = 'IncentiveDetailReport.aspx?key=' + $(el).attr('id'); }
			    else if (action == 'Status') { window.location.href = 'TeamStatus.aspx?key=' + $(el).attr('id'); }
			    else { alert('option not enabled.'); return false; }
			});
        });

</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="min-height: 250px;">
        <h3 style="border-bottom: dashed 1px #666666; margin-bottom: 2px; margin-top: 8px">
            <span style="font-size: 15px; font-weight: bold; margin-bottom: 11px; margin-top: 8px;
                padding-left: 10px"> Member Detail</span></h3>
        <div style="padding: 10px 10px 20px 10px">
            <table width="70%" align="center">
                <tr>
                    <td>
                        <h3>
                            Search Records By :</h3>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSearch" runat="server" CssClass="ddlSearch" Width="120px">
                            <asp:ListItem Value="0" Selected="True">Search Type</asp:ListItem>
                            <asp:ListItem Value="IDNo">Distributor ID</asp:ListItem>
                            <asp:ListItem Value="MemName">Name</asp:ListItem>
                            <asp:ListItem Value="City">City</asp:ListItem>
                            <asp:ListItem Value="StateName">State</asp:ListItem>
                            <asp:ListItem Value="Mobl">Mobile</asp:ListItem>
                            <asp:ListItem Value="EMail">Email</asp:ListItem>
                            <asp:ListItem Value="DOJ">Joining Date</asp:ListItem>
                            <asp:ListItem Value="RMemID">Sponsor ID</asp:ListItem>
                            <asp:ListItem Value="RMemName">Sponsor Name</asp:ListItem>
                            <asp:ListItem Value="KitName">Package Name</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSrchText" runat="server" CssClass="TxtBox" Width="160px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="BtnSearch" runat="server" CssClass="Btn" Text="Search" />
                    </td>
                    <td>
                        <asp:Button ID="btnExport" runat="server" CssClass="Btn" Text="Export To Excel" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>       
        <div style="padding: 10px 10px 20px 0px">
            <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="50"
                AutoGenerateColumns="False" CssClass="table table-striped table-advance table-hover" PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" EmptyDataText="No data to display.">
                <Columns>
                    <asp:TemplateField HeaderText="SNo.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Member ID">
                        <ItemTemplate>
                            <asp:Label ID="MemberID" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                        <%#Eval("Qstr")%>
                         
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sponsor ID">
                        <ItemTemplate>
                            <asp:Label ID="RefIDNo" runat="server" Text='<%# Eval("RefIDNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Password">
                        <ItemTemplate>
                            <asp:Label ID="Password" runat="server" Text='<%# Eval("Passw") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Joining Date">
                        <ItemTemplate>
                            <asp:Label ID="Doj" runat="server" Text='<%# Eval("Doj") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Click Here!">
                        <ItemTemplate>
                            <div id='<%# Eval("IDNo") %>' style="background-color:#99CCCC;color:Black;font-weight:bold;font-size:12px" >
                                <asp:Image runat="server" ID="BtnMenus" ImageUrl="../Resources/images/moreopt.jpg" AlternateText=""  />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="City" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="State">
                        <ItemTemplate>
                            <asp:Label ID="State" runat="server" Text='<%# Eval("StateName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mobile No.">
                        <ItemTemplate>
                            <asp:Label ID="Mobile" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:Label ID="Email" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Package">
                        <ItemTemplate>
                            <asp:Label ID="Package" runat="server" Text='<%# Eval("KitName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Activ.Date">
                        <ItemTemplate>
                            <asp:Label ID="UpgrdDate" runat="server" Text='<%# Eval("UpgrdDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="Status" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
            
                     <asp:TemplateField HeaderText="E-Wallet">
                        <ItemTemplate>
                            <asp:Label ID="Wallet" runat="server" Text='<%# Eval("Balance") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                                      
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <ul id="myMenu" class="contextMenu">
        <li class="edit"><a href="#Account">View Account</a></li>
        <li class="cut separator"><a href="#Profile">Update Profile</a></li>
        <li class="cut separator"><a href="#Tree">View Tree</a></li>
        <li class="paste separator"><a href="#Activate">Activate</a></li>
        <li class="delete separator"><a href="#SMS">Send SMS</a></li>
        <li class="quit separator"><a href="#Block">Block Now</a></li>
        <li class="quit separator"><a href="#BlockTree">Block Tree</a></li>
        <li class="quit separator"><a href="#UnBlock">Unblock</a></li>
        <li class="quit separator"><a href="#UnblockTree">Unblock Tree</a></li>
<%--        <li class="quit separator"><a href="#Reward">Reward Status</a></li>--%>
        <li class="quit separator"><a href="#Incentive">Incentive Detail</a></li>
        <li class="quit separator"><a href="#Status">Complete Status</a></li>
    </ul>
</asp:Content>
