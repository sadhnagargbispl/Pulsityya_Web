<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="WalletReport.aspx.vb" Inherits="App_UI_Application_Pages_PairDirectReport"
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
                         <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                            Wallet Balance Report</h2>
                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <div style="background-color: White">
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkMem" runat="server" Text="Enter Member ID :" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtMemberId" class="form-control" runat="server" Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                             
                                    </div>
                                    <div class="col-md-2">
                                        Page Size:</div>
                                    <div class="col-md-3">
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
                                            <asp:ListItem Text="1000" Value="1000" />
                                            <asp:ListItem Text="2000" Value="2000" />
                                        </asp:DropDownList>
                                    </div>
                                   <%-- <div class="col-md-2">
                                    </div>--%>
                                </div>
                                <br />
                                <div class="col-md-12" style="padding: 10px" id="Datefilter" runat="server" visible="false">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Transaction Start Date: "></asp:Label></div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control" onkeypress ="return false;"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator></div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblEndDate" runat="server" Text="Transaction End Date : "></asp:Label></div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control" onkeypress ="return false;"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator></div>
                                </div>
                                <div class="col-md-12" style="padding: 10px">
                                    <div class="col-md-2">
                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" /></div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" /></div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnPrintCurrent" runat="server" Text="print" class="btn btn-primary"
                                            Visible="false" /></div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                            Visible="false" /></div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                            color: Red"></asp:Label></div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <div id="gvContainer" runat="server" style="overflow: scroll;" class="col-md-12">
                                        <div class="col-md-12">
                                            <div class="col-md-3">
                                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                                    color: Gray"></asp:Label>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblPoint" runat="server" Style="font-weight: bold; font-size: 14px;
                                                    color: Gray"></asp:Label>
                                            </div>
                                               <div class="col-md-3">
                                                <asp:Label ID="lblGift" runat="server" Style="font-weight: bold; font-size: 14px;
                                                    color: Gray"></asp:Label>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblMain" runat="server" Style="font-weight: bold; font-size: 14px;
                                                    color: Gray"></asp:Label>
                                            </div>
                                            
                                            <div class="col-md-3">
                                                <asp:Label ID="lblPin" runat="server" Style="font-weight: bold; font-size: 14px;
                                                    color: Gray"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true"  RowStyle-Height="25px"
                                                GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                PagerStyle-CssClass="PagerStyle" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                                PageSize="10" EmptyDataText="No data to display." AllowSorting="true" OnSorting="GvData_Sorting">
                                                <%--<Columns>
                                                    <asp:BoundField DataField="SNO" HeaderText="SNo" SortExpression="SNo" />
                                                    <asp:BoundField DataField="Idno" HeaderText="IdNo" SortExpression="Idno" />
                                                    <asp:BoundField DataField="MemberName" HeaderText="MemberName" SortExpression="MemberName" />
                                                    <asp:BoundField DataField="M-WalletCredit" HeaderText="M-Wallet Credit" SortExpression="M-WalletCredit" />
                                                    <asp:BoundField DataField="M-WalletDebit" HeaderText="M-Wallet Debit" SortExpression="M-WalletDebit" />
                                                    <asp:BoundField DataField="M-WalletBalance" HeaderText="M-Wallet Balance" SortExpression="M-WalletBalance" />
                                                    <asp:BoundField DataField="PinWalletCredit" HeaderText="Shopping Wallet Credit" SortExpression="PinWalletCredit" />
                                                    <asp:BoundField DataField="PinWalletDebit" HeaderText="Shopping Wallet Debit" SortExpression="PinWalletDebit" />
                                                    <asp:BoundField DataField="PinWalletBalance" HeaderText="Shopping Wallet Balance"
                                                        SortExpression="PinWalletBalance" />
                                                </Columns>--%>
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
    <br />
    <br />
</asp:Content>
