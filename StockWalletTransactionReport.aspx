<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="StockWalletTransactionReport.aspx.vb" Inherits="StockWalletTransactionReport"
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
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Stock Wallet Transaction Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div style="background-color: White">
                            <div class="col-md-12">
                                <div class="col-md-2">
                             Member Id:
                             </div>
                                <div class="col-md-4">
                                <asp:DropDownList ID="DDlMember" runat="server" class="form-control"></asp:DropDownList></div>
                                <div class="col-md-2" id="DivWallet" runat="server" >
                                   Type:</div>
                                <div class="col-md-4" id="DivWalletType" runat="server">
                                    <asp:RadioButtonList ID="RbtWalletType" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" AutoPostBack ="true" >
                                        <asp:ListItem Text="Balance" Value="B" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Transaction" Value="T"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                               
                            </div>
                            <div class="col-md-12" style="padding: 10px">
                                <div class="col-md-2">
                                    <asp:Label ID="lblStartDate" runat="server" Text="Start Date: "></asp:Label></div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtStartDate" runat="server" class="form-control" Enabled ="false" ></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                        ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator></div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblEndDate" runat="server" Text=" End Date : "></asp:Label></div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control" Enabled ="false"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator></div>
                               <%--  <div class="col-md-3">
                                   <asp:DropDownList ID="DDlSearchBy" runat="server" class="form-control">
                                        <asp:ListItem Text="Descending By Date" Value="D"></asp:ListItem>
                                        <asp:ListItem Text="Ascending By Date" Value="A"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1">
                                </div>--%>
                            </div>
                            <div class="col-md-12" style="padding: 5px">
                            <div class="col-md-2">
                            
                            <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                color: Gray; margin-right: 10px"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" ValidationGroup="Save" /></div>
                                <%-- <asp:Button ID="btnshowall" runat="server" CssClass="Btn" Text="Show All" />--%>
                                <div class="col-md-2">
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel"
                                        Enabled="false" /></div>
                                <%--   <asp:Button ID="BtnExportCsv" runat="server" CssClass ="Btn" Text="Export To CSV" />--%>
                                <div class="col-md-2">
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" /></div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" /></div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                        color: Red"></asp:Label></div>
                                
                            </div>
                        </div>
                
                        <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                            margin-bottom: 25px;">
                            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                PagerStyle-CssClass="PagerStyle" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                PageSize="10" EmptyDataText="No data to display.">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%></ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
</asp:Content>
