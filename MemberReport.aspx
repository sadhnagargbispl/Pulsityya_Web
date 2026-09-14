<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="MemberReport.aspx.vb" Inherits="App_UI_Application_Pages_MemberReport"
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
                            Member Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-4">
                                    <asp:Label ID="Label2" runat="server" Text="Choose Date: "></asp:Label>
                                    <asp:DropDownList ID="CmbType" runat="server" Style="display: inline" class="form-control">
                                        <asp:ListItem Selected="True" Value="J">Joining Date</asp:ListItem>
                                        <asp:ListItem Value="A">Activation Date</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
                                    <asp:TextBox ID="txtStartDate" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                        ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:CheckBox ID="ChkKit" runat="server" Text="Choose Package :" TextAlign="Left" />
                                    <asp:DropDownList ID="CmbKit" runat="server" class="form-control" Style="display: inline">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:CheckBox ID="ChkSearch" runat="server" Text="Search By" TextAlign="Left" />
                                    <asp:DropDownList ID="DDlSerchBy" runat="server" class="form-control">
                                        <asp:ListItem Text="Id" Value="I" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Mobile" Value="M"></asp:ListItem>
                                        <asp:ListItem Text="ProductBv" Value="B"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4" style=" padding-top:2.5%">
                                    <asp:TextBox ID="TxtSearch" runat="server" class="form-control"></asp:TextBox></div>
                                <div class="col-md-8" style="padding-top :1%">
                                    <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                    <asp:Button ID="btnshowall" runat="server" class="btn btn-primary" Text="Show All" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" />
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" />
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                        color: Red"></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."
                                        Visible="false"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray"></asp:Label>
                                </div>
                                
                                <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                    margin-bottom: 25px;" class="col-md-12">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="10" EmptyDataText="No data to display.">
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
</asp:Content>
