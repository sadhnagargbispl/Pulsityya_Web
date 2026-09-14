<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="fullpayoutMaster.aspx.vb" Inherits="App_UI_Application_Pages_fullpayoutMaster"
    Title="" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Full Payout Report
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <div class="table-responsive makeitresponsivegrid" style="min-height: 500px;">
                                <div align="center">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:CheckBox ID="ChkMember" runat="server" Text="Member Id:" />
                                                <asp:TextBox CssClass="form-control" ID="txtMemberId" runat="server" Width="200px"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblStartDate" runat="server" Text="From Date : "></asp:Label>
                                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control "></asp:TextBox>
                                                <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                                    Format="dd-MMM-yyyy">
                                                </AjaxToolkit:CalendarExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                                    ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblEndDate" runat="server" Text="To Date : "></asp:Label>
                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control "></asp:TextBox>
                                                <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                                    Format="dd-MMM-yyyy">
                                                </AjaxToolkit:CalendarExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                                    ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <a href="Addfullpayout.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 300,marginTop : 0 } )">
                                                    <asp:Button ID="BtnAddNew" runat="server" CssClass="btn btn-primary " Text="Add Full Payout" /></a>
                                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search"
                                                    ValidationGroup="Save" />
                                                <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Export To Excel"
                                                    Enabled="false" />
                                                <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" CssClass="btn btn-primary" Visible ="false"  />
                                                <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" CssClass="btn btn-primary" Visible="false"  />
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                        
                                        <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                            color: Red"></asp:Label>
                                    </div>
                                    <div style="margin-top: 20px; margin-bottom: 20px;">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."
                                            Visible="false"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                            color: Gray"></asp:Label>
                                        <br />
                                        <asp:Label ID="LblCredit" runat="server" Style="font-weight: bold; font-size: 14px;
                                            color: Gray"></asp:Label>
                                        <asp:Label ID="lblDebit" runat="server" Style="font-weight: bold; font-size: 14px;
                                            color: Gray"></asp:Label>
                                        <asp:Label ID="LblBalance" runat="server" Style="font-weight: bold; font-size: 14px;
                                            color: Gray"></asp:Label>
                                    </div>
                                    <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                        margin-bottom: 25px;" class="col-md-12">
                                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                            GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" PageSize="25" EmptyDataText="No data to display.">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                        <asp:Label ID="lBlRefNo" runat="server" Text='<%# Eval("Tid") %>' Visible="false"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IdNo" HeaderText="Idno" />
                                                <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                                <asp:BoundField DataField="Date" HeaderText="Date"></asp:BoundField>
                                                <%--<asp:BoundField DataField="Status" HeaderText="Status" />--%>
                                                <%--<asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Panel ID="PanlModify" runat="server" BackColor="Transparent">
                                                            <a class="btn btn-primary" href='<%# "Add10Points.aspx?TId=" & Eval("Tid")  %>'
                                                                onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 470,marginTop : 0 } )">
                                                                <i class="icon_plus_alt2"></i>
                                                                <asp:Label ID="LBModify" runat="server" Text="Modify" />
                                                            </a>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
        </div>
</asp:Content>
