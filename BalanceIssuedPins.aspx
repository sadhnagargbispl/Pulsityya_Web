<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="BalanceIssuedPins.aspx.vb" Inherits="App_UI_Application_Pages_BalanceIssuedPins"
    EnableEventValidation="false" %>

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
                            Epin Balance Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-12">
                                    <div class="col-md-1">
                                        <asp:CheckBox ID="ChkMem" runat="server" Text="Member Id:" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="TxtMemId" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkKit" runat="server" Text="Choose Package:" /></div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="CmbKit" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        PageSize:</div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed"
                                            class="form-control">
                                            <asp:ListItem Text="10" Value="10" />
                                            <asp:ListItem Text="20" Value="20" />
                                            <asp:ListItem Text="50" Value="50" />
                                            <asp:ListItem Text="100" Value="100" />
                                            <asp:ListItem Text="500" Value="500" />
                                            <asp:ListItem Text="1000" Value="1000" />
                                            <asp:ListItem Text="2000" Value="2000" />
                                            <asp:ListItem Text="5000" Value="5000" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-9">
                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" ValidationGroup="Save" />
                                        <%--   <asp:Button ID="btnshowall" runat="server" class="btn btn-primary" Text="Show All" />--%>
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                        <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                            Enabled="false" Visible="false" />
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                            Enabled="false" Visible="false" />
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label><asp:Label ID="lblCount"
                                            runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                        <div id="gvContainer" runat="server">
                                            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                                GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                ShowHeader="true" PageSize="50" EmptyDataText="No data to display." AllowSorting="true"
                                                OnSorting="GvData_Sorting">
                                                <Columns>
                                                 <%--   <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="35px">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:BoundField DataField="SNo" HeaderText="SNo" SortExpression="SNo" />
                                                    <asp:TemplateField HeaderText="GrpID" Visible="false">
                                                        <ItemTemplate>
                                                            <%--<asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="IdNo" HeaderText="Id No." ControlStyle-Width="50px" SortExpression="IdNo" />
                                                    <asp:BoundField DataField="MemName" HeaderText="Issued Member Name" SortExpression="MemName" />
                                                    <asp:BoundField DataField="KitName" HeaderText="Package Name" SortExpression="KitName" />
                                                    <asp:BoundField DataField="KitAmount" HeaderText="Package Amount" SortExpression="KitAmount" />
                                                    <asp:BoundField DataField="Bv" HeaderText="Package Bv" SortExpression="BV" />
                                                    <asp:BoundField DataField="ReceivedFromId" HeaderText="Received From Id" ControlStyle-Width="50px"
                                                        SortExpression="ReceivedFromId" />
                                                    <asp:BoundField DataField="ReceivedFromAdmin" HeaderText="Received From Admin" SortExpression="ReceivedFromAdmin" />
                                                    <asp:BoundField DataField="TransferPin" HeaderText="Transfer Pin" SortExpression="TransferPin" />
                                                    <asp:BoundField DataField="UsedPin" HeaderText="UsedPin" SortExpression="UsedPin" />
                                                    <asp:BoundField DataField="BalancePin" HeaderText="Balance Pin" SortExpression="BalancePin" />
                                                    <asp:BoundField DataField="BalanceEpinValue" HeaderText="Balance Epin Value" SortExpression="BalanceEpinValue" />
                                                </Columns>
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
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
