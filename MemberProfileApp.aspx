<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="MemberProfileApp.aspx.vb" Inherits="App_UI_Application_Pages_MemberProfileApp"
    Title="" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .PagerStyle
        {
            background-image: url(../Images/td.jpg);
            background-position: center;
            background-repeat: repeat-x;
            background-color: #ffffff;
            font-weight: bold;
            text-align: center;
            width: 00px;
        }
        .PagerStyle table
        {
            text-align: center;
            margin: auto;
        }
        .PagerStyle table td
        {
            border: 0px;
            padding: 5px;
        }
        .PagerStyle td
        {
            border-top: #1d1d1d 3px solid;
        }
        .PagerStyle a
        {
            color: #000000;
            text-decoration: none;
            padding: 2px 10px 2px 10px;
            border-top: solid 1px #777777;
            border-right: solid 1px #333333;
            border-bottom: solid 1px #333333;
            border-left: solid 1px #777777;
        }
        .PagerStyle span
        {
            font-weight: bold;
            color: #000000;
            text-decoration: none;
            padding: 2px 10px 2px 10px;
        }
    </style>
    <style type="text/css">
        #doublescroll
        {
            overflow: auto;
            overflow-y: hidden;
        }
        #doublescroll p
        {
            margin: 0;
            padding: 1em;
            white-space: nowrap;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="images/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Member List</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="col-md-12">
                            <div align="center">
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        <asp:Label ID="Label2" runat="server" Text="Choose Date: "></asp:Label>
                                        <asp:DropDownList ID="CmbType" runat="server" class="form-control">
                                            <asp:ListItem Selected="True" Value="J">Joining Date</asp:ListItem>
                                            <asp:ListItem Value="A">Activation Date</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:CheckBox ID="ChkKit" runat="server" Text="Choose Package :" TextAlign="Left" />
                                        <asp:DropDownList ID="CmbKit" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        <asp:CheckBox ID="ChkBank" runat="server" Text="Choose Bank :" TextAlign="Left" />
                                        <asp:DropDownList ID="DdlBank" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:CheckBox ID="ChkMem" runat="server" Text="MemberId :" TextAlign="Left" />
                                        <asp:TextBox ID="txtMember" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        PageSize:
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed"
                                            class="form-control">
                                            <asp:ListItem Text="10" Value="10" />
                                            <asp:ListItem Text="20" Value="20" />
                                            <asp:ListItem Text="50" Value="50" />
                                            <asp:ListItem Text="100" Value="100" />
                                            <asp:ListItem Text="200" Value="200" />
                                            <asp:ListItem Text="300" Value="300" />
                                            <asp:ListItem Text="400" Value="400" />
                                            <asp:ListItem Text="500" Value="500" />
                                            <asp:ListItem Text="600" Value="600" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                     Registion From:
                                        <asp:DropDownList ID="ddlRegfrom" runat="server" 
                                            class="form-control">
                                            <asp:ListItem Text="Both" Value="Both" />
                                            <asp:ListItem Text="App" Value="App" />
                                            <asp:ListItem Text="Web" Value="Web" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                    <%-- <asp:Button ID="btnshowall" runat="server" class="btn btn-primary" Text="Show All" />--%>
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                    <asp:Button ID="BtnExportCsv" runat="server" class="btn btn-primary" Text="Export To CSV"
                                        Visible="false" />
                                    <asp:Button ID="BtnBankDetail" runat="server" class="btn btn-primary" Text="View Bank Detail "
                                        Visible="false" />
                                    <asp:Button ID="BtnExportBank" runat="server" class="btn btn-primary" Text="Export Bank Detail"
                                        Visible="false" />
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                        Visible="false" />
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                        Visible="false" />
                                </div>
                                <div class="col-md-12">
                                </div>
                                <div class="col-md-12">
                                </div>
                                <div id="doublescroll" class="col-md-12">
                                    <p>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div id="gvContainer" runat="server" class="table table-bordered" style="overflow: scroll">
                                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                                        color: Red"></asp:Label>
                                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                                        color: Gray"></asp:Label>&nbsp&nbsp&nbsp
                                                    <asp:Label ID="lblactive" runat="server" Style="font-weight: bold; font-size: 14px;
                                                        color: Gray"></asp:Label>&nbsp&nbsp&nbsp
                                                    <asp:Label ID="lbldeactive" runat="server" Style="font-weight: bold; font-size: 14px;
                                                        color: Gray"></asp:Label>&nbsp&nbsp&nbsp
                                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                                        GridLines="Both" AllowPaging="False" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                        ShowHeader="true" AllowSorting="true" OnSorting="GvData_Sorting" PageSize="10"
                                                        EmptyDataText="No data to display.">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex +1 %>.</ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%-- <asp:BoundField DataField="Sessid" HeaderText="Sessid" SortExpression="Sessid" />--%>
                                                            <asp:BoundField DataField="IdNo" HeaderText="IdNo" SortExpression="IdNo" />
                                                            <asp:BoundField DataField="FirstName" HeaderText="MemberName" SortExpression="FirstName" />
                                                            <asp:BoundField DataField="DOJ" HeaderText="DOJ" SortExpression="DOJ" />
                                                            <asp:BoundField DataField="UpgradeDate" HeaderText="Date of Activation" SortExpression="UpgradeDate" />
                                                            <asp:BoundField DataField="PackageName" HeaderText="PackageName" SortExpression="PackageName" />
                                                            <asp:BoundField DataField="SponsorId" HeaderText="SponsorId" SortExpression="SponsorId" />
                                                            <asp:BoundField DataField="Sponsorname" HeaderText="SponsorName" SortExpression="Sponsorname" />
                                                            <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
                                                            <asp:BoundField DataField="EPassWord" HeaderText="Transaction Password" SortExpression="EPassWord" />
                                                            <asp:BoundField DataField="MobileNo" HeaderText="MobileNo" SortExpression="MobileNo" />
                                                            <asp:BoundField DataField="Pv" HeaderText="Unit" SortExpression="Pv" />
                                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                                            <%--  <asp:BoundField DataField="UplinerId" HeaderText="UplinerId" SortExpression="UplinerId" />
                                                            <asp:BoundField DataField="Uplinername" HeaderText="Uplinername" SortExpression="Uplinername" />
                                                           
                                                              <asp:BoundField DataField="Legname" HeaderText="Legname" SortExpression="Legname" />--%>
                                                            <asp:BoundField DataField="MemberDOB" HeaderText="DateOfBirth" SortExpression="MemberDoB" />
                                                            <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                                                            <asp:BoundField DataField="Address1" HeaderText="Address" SortExpression="Address1" />
                                                            <asp:BoundField DataField="CityName" HeaderText="CityName" SortExpression="CityName" />
                                                            <asp:BoundField DataField="DistrictName" HeaderText="DistrictName" SortExpression="DistrictName" />
                                                            <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
                                                            <asp:BoundField DataField="PinCode" HeaderText="PinCode" SortExpression="PinCode" />
                                                            <asp:BoundField DataField="NomineeName" HeaderText="NomineeName" SortExpression="NomineeName" />
                                                            <asp:BoundField DataField="PANNo" HeaderText="PANNo" SortExpression="PANNo" />
                                                            <asp:BoundField DataField="BankName" HeaderText="BankName" SortExpression="BankName" />
                                                            <asp:BoundField DataField="AccountNo" HeaderText="AccountNo" SortExpression="AccountNo" />
                                                            <asp:BoundField DataField="IFSCCode" HeaderText="IFSC Code" SortExpression="IFSCCode" />
                                                            <asp:BoundField DataField="UpgradeSessid" HeaderText="UpgradeSessid" SortExpression="UpgradeSessid" />
                                                            <asp:BoundField DataField="RegFrom" HeaderText="Reg. From" SortExpression="RegFrom" />
                                                        </Columns>
                                                        <PagerStyle CssClass="PagerStyle" />
                                                        <PagerSettings Mode="NumericFirstLast" />
                                                    </asp:GridView>
                                                    <asp:Repeater ID="rptPager" runat="server">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                CssClass='<%# If(Convert.ToBoolean(Eval("Enabled")), "page_enabled", "page_disabled")%>'
                                                                OnClick="Page_Changed" OnClientClick='<%# If(Not Convert.ToBoolean(Eval("Enabled")), "return false;", "") %>'
                                                                ForeColor="Black"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlPageSize" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="btnExport" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </p>
                                </div>

                                <script type="text/javascript">

                                    function DoubleScroll(element) {
                                        var scrollbar = document.createElement('div');
                                        scrollbar.appendChild(document.createElement('div'));
                                        scrollbar.style.overflow = 'auto';
                                        scrollbar.style.overflowY = 'hidden';
                                        scrollbar.firstChild.style.width = element.scrollWidth + 'px';
                                        scrollbar.firstChild.style.paddingTop = '1px';
                                        scrollbar.firstChild.appendChild(document.createTextNode('\xA0'));
                                        scrollbar.onscroll = function() { element.scrollLeft = scrollbar.scrollLeft; };
                                        element.onscroll = function() { scrollbar.scrollLeft = element.scrollLeft; };
                                        element.parentNode.insertBefore(scrollbar, element);
                                    } DoubleScroll(document.getElementById('doublescroll'));</script>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <!-- end of weather widget -->
            </div>
        </div>
</asp:Content>
