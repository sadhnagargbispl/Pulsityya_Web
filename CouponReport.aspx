<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="CouponReport.aspx.vb" Inherits="CouponReport" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">

        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
    </script>

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
                            Coupon Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-3">
                            Member ID:
                            <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3" style="padding-top: 9Px">
                            Coupon Code
                            <asp:TextBox ID="TxtCouponCode" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3" style="padding-top: 9Px">
                            From Date
                            <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                Format="dd-MMM-yyyy">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFromDate"
                                ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-3" style="padding-top: 9Px">
                            To Date :
                            <asp:TextBox ID="TxtToDate" runat="server" class="form-control"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                Format="dd-MMM-yyyy">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-4" style="margin-top: 10Px; margin-bottom: 10px">
                            <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                            <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                            <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                color: Red"></asp:Label>
                        </div>
                    </div>
                    <div style="margin-top: 20px; margin-bottom: 20px;">
                        <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                            color: Gray"></asp:Label>
                    </div>
                    <div style="padding: 10px 10px 20px 10px" id="divDetail" runat="server">
                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="true" GridLines="Both"
                            class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                            EmptyDataText="No data to display." AutoGenerateColumns="True" PageSize="20"
                            PagerStyle-CssClass="PagerStyle">
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
</asp:Content>
