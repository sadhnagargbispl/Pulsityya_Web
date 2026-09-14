<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="MemberDownlinePurchase.aspx.vb" Inherits="App_UI_Application_Pages_MemberDownlinePurchase"
    Title="" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .DDl
        {
            display: block;
            width: 200px;
            height: 24px;
            padding: 3px 16px;
            font-size: 14px;
            line-height: 1.428571429;
            color: #8e8e93;
            vertical-align: middle;
            background-color: #ffffff;
            border: 1px solid #c7c7cc;
            border-radius: 4px;
            -webkit-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
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
                            Member Downline Report</h2>
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
                                    <div class="col-md-2">
                                        Enter Member ID :</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtMember" runat="server" CssClass="form-control" AutoPostBack="true"
                                            TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMember"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                        <asp:Label ID="LblFormno" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblName" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:RadioButtonList ID="RbtProduct" runat="server" TabIndex="3" RepeatColumns="2"
                                            RepeatDirection="Horizontal" AutoPostBack="true">
                                            <asp:ListItem Text="Repurchase" Value="R" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="TopUp" Value="T">                </asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2" visible="false">
                                        <asp:Label ID="LblLevel" runat="server" Text="Level Wise:" visible="false"></asp:Label></div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="DdlLevel" CssClass="form-control" TabIndex="2" runat="server"
                                            Visible="false" Style="display: inline">
                                        </asp:DropDownList>
                                        <asp:RadioButtonList ID="RbtLegNo" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                            Visible="true">
                                            <asp:ListItem Text="Both" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <br />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control"></asp:TextBox>
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
                                    <div class="col-md-4">
                                        <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                            color: Red"></asp:Label></div>
                                    <div class="col-md-8">
                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search"
                                            ValidationGroup="Save" />
                                        <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Export To Excel" />
                                        <%--<asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" CssClass="btn btn-primary" />--%>
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."
                                        Visible="false"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray"></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                          <asp:Label ID="lblTotalBV" Font-Bold="true" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" runat="server" Visible="false"  ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                         <asp:Label ID="lblleftbv" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" Visible="false"  ></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblbv" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray"></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        
                                        <asp:Label ID="lblroyaltileftbv" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" ></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblroyaltirightbv" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" ></asp:Label>
                                        
                                        <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" Visible="false" ></asp:Label>
                                        <asp:Label ID="lblcnt1" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" Visible="false" ></asp:Label>
                                        <asp:Label ID="lbllbv" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" Visible ="false" ></asp:Label>
                                        <asp:Label ID="lblrleftbv" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" Visible ="false" ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblrrightbv" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" Visible ="false"></asp:Label>
                                        <asp:Label ID="LeftSmartcardbv" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" Visible ="false"></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="rightSmartcardbv" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray" Visible ="false"></asp:Label>
                                        
                                </div>
                                <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                    margin-bottom: 25px;">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="25" EmptyDataText="No data to display.">
                                         <PagerStyle HorizontalAlign = "Right" CssClass = "pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
