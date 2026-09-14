<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StatementWeekly.aspx.vb"
    Inherits="Css_StatementWeekly" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="fren/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <script src="fren/bootstrap.min.js" type="text/javascript"></script>

    <script src="fren/jquery.min.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <center>
            <div class="row" style="border: 1px solid black; width: 50pc;">
                <div class="col-md-12">
                    <div class="row">
                        <table class="table-bordered" width="98%">
                            <tr>
                                <td width="30%">
                                    <div class="col-md-12">
                                        <img src="Images/logo.jpg" style="width: 150px; float: left;" id="imglogo" runat="server" />
                                    </div>
                                </td>
                                <td width="70%">
                                    <div class="col-md-12" style="text-align: left">
                                        <b>
                                            <%=Session("CompName")%></b>
                                        <br />
                                        Address:-
                                        <%=Session("CompAdd")%>
                                        <br />
                                        Email:-
                                        <%=Session("CompMail")%>
                                        <br />
                                        Mobile No.:-
                                        <%=Session("CompMobile")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="row">
                        <div class="col-md-12" style="background-color: #999999; text-align: center; color: White;">
                            <b><span style="font-size: 18px; font-family: Arial Baltic;">Income Statement </span>
                            </b>
                        </div>
                    </div>
                    <div class="row">
                        <table class="table-bordered" width="98%">
                            <tr>
                                <td width="40%">
                                    <div class="col-md-12" style="text-align: center;">
                                        <b><span style="font-size: 16px; font-family: Arial Baltic;">Distributor Detail </span>
                                        </b>
                                        <div class="col-md-12">
                                            <table class="table-bordered" width="100%">
                                                <tr>
                                                    <td style="text-align: left; width: 30%;">
                                                        Name
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 60%;">
                                                        <div id="MemName" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 30%;">
                                                        ID
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 60%;">
                                                        <div id="IDNO" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 30%;">
                                                        Address
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 60%;">
                                                        <div id="Add" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 30%;">
                                                        Mob. No.
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 60%;">
                                                        <div id="Mobile" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 30%;">
                                                        City
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 60%;">
                                                        <div id="City" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 30%;">
                                                        District
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 60%;">
                                                        <div id="District" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 30%;">
                                                        Pin Code
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 60%;">
                                                        <div id="PinCode" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 30%;">
                                                        State
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 60%;">
                                                        <div id="State" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                    <div class="col-md-12" style="text-align: center;">
                                        <b><span style="font-size: 16px; font-family: Arial Baltic;">Payout Detail </span>
                                        </b>
                                        <div class="col-md-12">
                                            <table class="table-bordered" width="100%">
                                                <tr>
                                                    <td style="text-align: left; width: 30%;">
                                                        Payout No.
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 60%;">
                                                        <div id="PayoutTime" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 30%;">
                                                        Period
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 60%;">
                                                        <div id="Period" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-12" style="background-color: #999999; text-align: center; color: White;">
                            <b><span style="font-size: 18px; font-family: Arial Baltic;">Income Details </span>
                            </b>
                        </div>
                    </div>
                    <div class="row">
                        <table class="table-bordered" width="98%">
                            <tr>
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <th style="text-align: left;">
                                                    Earnings
                                                </th>
                                                <th style="text-align: right;">
                                                    Amount In Rs.
                                                </th>
                                            </tr>
                                            <asp:Repeater ID="gvincome" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <%#Eval("Earnings")%>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <%#Eval("Amount")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <th style="text-align: left;">
                                                    Deductions
                                                </th>
                                                <th style="text-align: right;">
                                                    Amount In Rs.
                                                </th>
                                            </tr>
                                            <asp:Repeater ID="gvDeduction" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <%#Eval("Deductions")%>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <%#Eval("Amount")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="row">
                        <table class="table-bordered" width="98%">
                            <tr>
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <th style="text-align: left;">
                                                    <b>Total Earnings </b>
                                                </th>
                                                <th style="text-align: right;">
                                                    <div id="TotalEarnings" runat="server">
                                                    </div>
                                                </th>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <th style="text-align: left;">
                                                    <b>Total Deductions</b>
                                                </th>
                                                <th style="text-align: right;">
                                                    <div id="TotalDeductions" runat="server">
                                                    </div>
                                                </th>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="row">
                        <table class="table-bordered" width="98%">
                            <tr id="trGrossPrevVadic" runat="server" visible="false">
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <th style="text-align: left;">
                                                    <b>Gross Prev. </b>
                                                </th>
                                                <th style="text-align: right;">
                                                    <div id="GrossPrev" runat="server">
                                                    </div>
                                                </th>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                </td>
                            </tr>
                            <tr id="trnetIncome" runat="server" visible="false">
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <th style="text-align: left;">
                                                    <b>Gross Income </b>
                                                </th>
                                                <th style="text-align: right;">
                                                    <div id="GrossIncome" runat="server">
                                                    </div>
                                                </th>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                </td>
                            </tr>
                            <tr>
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <th style="text-align: left;">
                                                    <b>Net Payble Amount </b>
                                                </th>
                                                <th style="text-align: right;">
                                                    <div id="NetPayble" runat="server">
                                                    </div>
                                                </th>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                </td>
                            </tr>
                            <tr id="trNetCls" runat="server" visible="false">
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <th style="text-align: left;">
                                                    <b>Gross Closing </b>
                                                </th>
                                                <th style="text-align: right;">
                                                    <div id="GrossClosing" runat="server">
                                                    </div>
                                                </th>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div class="row">
                        <table class="table-bordered" width="98%">
                            <tr >
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1" id="trBussinessDetailAll" runat="server" visible="false">
                                            <tr>
                                                <td colspan="3" style="background-color: #999999; text-align: center; color: White;">
                                                    <b>Business Details</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td style="text-align: right;">
                                                    <b>Power Group</b>
                                                </td>
                                                <td style="text-align: right;">
                                                    <b>Weaker Group</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    B/F
                                                    <%=Session("ColName1")%>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="BfXBV" runat="server">
                                                    </div>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="BfYBV" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    New
                                                    <%=Session("ColName1")%>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="NewXBV" runat="server">
                                                    </div>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="NewYBV" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Matched
                                                    <%=Session("ColName1")%>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="MatchedXBV" runat="server">
                                                    </div>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="MatchedYBV" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    C/F
                                                    <%=Session("ColName1")%>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="CfXBV" runat="server">
                                                    </div>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="CfYBV" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1" id="divsponserIncome"
                                            runat="server" visible="true">
                                            <tr>
                                                <td colspan="8" style="background-color: #999999; text-align: center; color: White;">
                                                    <b>Sponsor Matching Bonus</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="8">
                                                    <asp:GridView ID="gv" style="width:100%;text-align:right;" runat="server" class="table-bordered" AutoGenerateColumns="true">
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trBussinessDetailVedic" runat="server" visible="false">
                                <td width="100%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <td colspan="3" style="background-color: #999999; text-align: center; color: White;">
                                                    <b>Business Details</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td style="text-align: right;">
                                                    <b>Left</b>
                                                </td>
                                                <td style="text-align: right;">
                                                    <b>Right</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Self Repurchase
                                                    <%=Session("ColName1")%>
                                                </td>
                                                <td style="text-align: right;" colspan="2">
                                                    <div id="SelfRBv" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    New Joining
                                                    <%=Session("ColName1")%>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="LeftBv" runat="server">
                                                    </div>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="RightBv" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    New Repur.
                                                    <%=Session("ColName1")%>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="LeftRBv" runat="server">
                                                    </div>
                                                </td>
                                                <td style="text-align: right;">
                                                    <div id="RightRBv" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="row" style="text-align: right; margin-top: 10px;">
                        <div class="col-md-12">
                            Authorised Signatory
                        </div>
                    </div>
                </div>
            </div>
        </center>
    </div>
    </form>
</body>
</html>
