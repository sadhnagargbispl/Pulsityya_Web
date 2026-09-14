<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="IssuedEPinReport.aspx.vb" Inherits="App_UI_Application_Pages_IssuedEPinReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                            Issued EPin Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div>
                            <div align="center">
                                <div class="col-md-12">
                                    <div class="col-md-2" style="padding-top: 0.7%">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Issue Start Date : "></asp:Label>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator></div>
                                    <div class="col-md-2" style="padding-top: 0.7%">
                                        <asp:Label ID="lblEndDate" runat="server" Text="Issue End Date : "></asp:Label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkKit" runat="server" Text="Choose Package :" />
                                        <asp:DropDownList ID="CmbKit" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkMember" runat="server" Text="Choose Member Id:" />
                                        <asp:TextBox ID="TxtMemId" runat="server" class="form-control"></asp:TextBox></div>
                                    <div class="col-md-2" style="padding-top: 0.7%">
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
                                </div>
                                <div class="col-md-12" id="trpin" runat="server" visible="false">
                                    <div class="col-md-4">
                                        Pin Status:
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RadioButtonList ID="rbtnStatus" runat="server" class="form-control" RepeatDirection="Horizontal"
                                            Font-Bold="True">
                                            <asp:ListItem Selected="True" Text="Both" Value="Both"></asp:ListItem>
                                            <asp:ListItem Text="Used" Value="Used"></asp:ListItem>
                                            <asp:ListItem Text="UnUsed" Value="UnUsed"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" /></div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                            Enabled="false" Visible="false" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                            Enabled="false" Visible="false" />
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                        color: Red"></asp:Label></div>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div id="gvContainer" runat="server" class="col-md-12">
                                            <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px;
                                                color: Gray"></asp:Label>
                                            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                                GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                ShowHeader="true" PageSize="10" EmptyDataText="No data to display." AllowSorting="true"
                                                OnSorting="GvData_Sorting">
                                                <Columns>
                                                    <asp:BoundField DataField="SNo" HeaderText="SNo" SortExpression="SNo" />
                                                    <asp:BoundField DataField="IdNo" HeaderText="IdNo." ControlStyle-Width="50px" SortExpression="Idno" />
                                                    <asp:BoundField DataField="MemName" HeaderText="Member Name" SortExpression="MemName" />
                                                    <asp:BoundField DataField="KitName" HeaderText="Package Name" ControlStyle-Width="50px"
                                                        SortExpression="KitName" />
                                                    <asp:BoundField DataField="Bv" HeaderText="Package Bv" SortExpression="BV" />
                                                    <asp:BoundField DataField="KitAmount" HeaderText=" Package Amount" SortExpression="KitAmount" />
                                                    <asp:BoundField DataField="DispatchQty" HeaderText="Accept Quantity" SortExpression="DispatchQty" />
                                                    <asp:BoundField DataField="DepositAmount" HeaderText="Deposit Amount" SortExpression="DepositAmount" />
                                                    <asp:BoundField DataField="Remarks" HeaderText="Remark" SortExpression="Remarks" />
                                                    <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
                                                    <asp:BoundField DataField="TransactionDate" HeaderText="Transaction Date" SortExpression="TransactionDate" />
                                                    <asp:BoundField DataField="TransactionTime" HeaderText="Transaction Time" SortExpression="TransactionTime" />
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
                                        <asp:PostBackTrigger ControlID="btnExport" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
            </div>
        </div>
    </div>
</asp:Content>
