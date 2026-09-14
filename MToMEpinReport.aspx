<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="MToMEpinReport.aspx.vb" Inherits="MToMEpinReport" Title="" EnableEventValidation="false" %>

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
                            M To M Epin Transfer Report</h2>
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
                                    <div class="col-md-1">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Start Date : "></asp:Label></div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Label ID="lblEndDate" runat="server" Text="End Date : "></asp:Label></div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-1">
                                        PageSize:</div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed"
                                            class="form-control">
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
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-1">
                                        <asp:CheckBox ID="ChkMem" runat="server" Text="Search By :" TextAlign="Left" /></div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="DDlSearchName" runat="server" class="form-control">
                                            <asp:ListItem Text="From User Id" Value="F"></asp:ListItem>
                                            <asp:ListItem Text="To User Id" Value="I"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtMember" runat="server" class="form-control"></asp:TextBox></div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkKit" runat="server" Text="Choose Package :" TextAlign="Left" /></div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="CmbKit" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                    <div class="col-md-8">
                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                        <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                            Enabled="false" Visible="false" />
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                            Enabled="false" Visible="false" />
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                          
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div id="gvContainer" runat="server" style="overflow: scroll;" class="col-md-12">
                                            <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                                color: Gray"></asp:Label>
                                                <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                        color: Red"></asp:Label>
                                            <asp:GridView ID="GvData" runat="server" RowStyle-Height="25px" AutoGenerateColumns="false"
                                                GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                ShowHeader="true" PageSize="10" EmptyDataText="No data to display." AllowSorting="true"
                                                OnSorting="GvData_Sorting">
                                                <Columns>
                                                    <asp:BoundField DataField="SNo" HeaderText="SNo" SortExpression="SNo" />
                                                    <asp:BoundField DataField="FromIdno" HeaderText="From IdNo" SortExpression="FromIdno" />
                                                    <asp:BoundField DataField="FromUserName" HeaderText="From Username" SortExpression="FromUserName" />
                                                    <asp:BoundField DataField="ToIdno" HeaderText="To IdNo" SortExpression="ToIdNo" />
                                                    <asp:BoundField DataField="ToUserName" HeaderText="To User Name" SortExpression="ToUsername" />
                                                    <asp:BoundField DataField="Package Name" HeaderText="Package Name" SortExpression="Package Name" />
                                                    <asp:BoundField DataField="Package MRP" HeaderText="Package MRP" SortExpression="Package MRP" />
                                                    <asp:BoundField DataField="Package BV" HeaderText="Package BV" SortExpression="Package BV" />
                                                    <asp:BoundField DataField="TransferQty" HeaderText="Transfer Qty" SortExpression="TransferQty" />
                                                    <asp:BoundField DataField="EpinValue" HeaderText="Epin Value" SortExpression="EpinValue" />
                                                    <asp:BoundField DataField="TransactionDateAndTime" HeaderText="Transaction Date And Time"
                                                        SortExpression="TransactionDateAndTime" />
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
            <div class="row">
            </div>
        </div>
    </div>
</asp:Content>
