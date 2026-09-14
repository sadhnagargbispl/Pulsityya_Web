<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StatementDailyPB.aspx.vb"
    Inherits="Css_StatementDailyPB" %>

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
                                                        IDNO
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
                                                    <td style="text-align: left; width: 10%;">
                                                        Payout No.
                                                    </td>
                                                    <td style="text-align: center; width: 5%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 85%;">
                                                        <div id="PayoutTime" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 10%;">
                                                        Period
                                                    </td>
                                                    <td style="text-align: center; width: 5%;">
                                                        :
                                                    </td>
                                                    <td style="text-align: left; width: 85%;">
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
                                <td width="40%" valign="top" runat="server" id="DivGeMartIncomeElse" visible="true">
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
                                <td width="40%" valign="top" runat="server" id="DivGeMartIncome" visible="true">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <td style="text-align: left; width: 65%;">
                                                    <% If Session("Compid") = "1091" Then%>
                                                    Team Performance Incentive
                                                    <% Else%>
                                                    Matching Income
                                                    <% End If%>
                                                </td>
                                                <td style="text-align: center; width: 5%;">
                                                    :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:Label ID="LblMatchingIncome" runat="server" Text="0.00"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 65%;">
                                                    <% If Session("Compid") = "1091" Then%>
                                                    Direct Referral Incentive
                                                    <% Else%>
                                                    Direct Sponsor Income
                                                    <% End If%>
                                                </td>
                                                <td style="text-align: center; width: 10%;">
                                                    :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:Label ID="LblDirectSponsorIncome" runat="server" Text="0.00"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 65%;">
                                                    <% If Session("Compid") = "1091" Then%>
                                                    Daily Performance Incentive
                                                    <% Else%>
                                                    Sponsor Matching Income
                                                    <% End If%>
                                                </td>
                                                <td style="text-align: center; width: 10%;">
                                                    :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:Label ID="LblSponsorMatchingIncome" runat="server" Text="0.00"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 65%;">
                                                    <% If Session("Compid") = "1091" Then%>
                                                    Leadership Rank Incentive
                                                    <% Else%>
                                                    Reward Income
                                                    <% End If%>
                                                </td>
                                                <td style="text-align: center; width: 10%;">
                                                    :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:Label ID="LblRewardIncome" runat="server" Text="0.00"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 65%;">
                                                    <% If Session("Compid") = "1091" Then%>
                                                    Solar Connection Incentive
                                                    <% Else%>
                                                    Royalty Income
                                                    <% End If%>
                                                </td>
                                                <td style="text-align: center; width: 10%;">
                                                    :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:Label ID="LblRoyaltyIncome" runat="server" Text="0.00"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <% If Session("Compid") = "1091" Then%>
                                                <% Else%>
                                                <td style="text-align: left; width: 65%;">
                                                    Leadership Dev. Bonus
                                                </td>
                                                <td style="text-align: center; width: 10%;">
                                                    :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:Label ID="LblLeadershipDevBonus" runat="server" Text="0.00"></asp:Label>
                                                </td>
                                                <% End If%>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 65%;">
                                                    <b>Total Earnings</b>
                                                </td>
                                                <td style="text-align: center; width: 10%;">
                                                    :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <b>
                                                        <asp:Label ID="LblTotalEarnings" runat="server" Text="0.00"></asp:Label></b>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top" runat="server" id="DivGeMartElse" visible="true">
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
                                <td width="40%" valign="top" runat="server" id="DivGeMart" visible="false">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <td style="text-align: left; width: 65%;">
                                                    TDS Amount
                                                </td>
                                                <td style="text-align: center; width: 10%;">
                                                    :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:Label ID="LblTDSAmount" runat="server" Text="0.00"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 65%;">
                                                    Admin Charge
                                                </td>
                                                <td style="text-align: center; width: 5%;">
                                                    :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:Label ID="LblAdminCharge" runat="server" Text="0.00"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 65%;">
                                                    Repurchase Deduction
                                                </td>
                                                <td style="text-align: center; width: 5%;">
                                                    :
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:Label ID="LblRepurchaseDeduction" runat="server" Text="0.00"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 65%;">
                                                    <b>Total Deduction</b>
                                                </td>
                                                <td style="text-align: center; width: 5%;">
                                                    <b>:</b>
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <b>
                                                        <asp:Label ID="LblTotalDeducation" runat="server" Text="0.00"></asp:Label></b>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="row">
                        <table class="table-bordered" width="98%">
                            <tr>
                                <td width="40%" valign="top" runat="server" id="DivTotalEar" visible="true">
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
                                <td width="40%" valign="top" runat="server" id="DivTotaldis" visible="true">
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
                            <tr>
                                <td width="40%" valign="top">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <th style="text-align: left;">
                                                    <b>Net Payble Amount (Rs.) </b>
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
                        </table>
                    </div>
                    <br />
                    <div class="row">
                        <table class="table-bordered" width="98%">
                            <tr>
                                <td width="40%" valign="top">
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
                                    <div id="divMentorshipLife" runat="server" visible="false">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <td colspan="6" style="background-color: #999999; text-align: center; color: White;">
                                                    <% If Session("Compid") = "1075" Then%>
                                                    <b>Single Side Bonus Detail</b>
                                                    <% Else%>
                                                    <b>Mentorship Bonus Detail</b>
                                                    <% End If%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="text-align: left;">
                                                    IDNO
                                                </th>
                                                <% If Session("Compid") = "1075" Then%>
                                                <th style="text-align: left;">
                                                    Name
                                                </th>
                                                <% Else%>
                                                <% End If%>
                                                <th style="text-align: right;">
                                                    Level
                                                </th>
                                                <% If Session("Compid") = "1075" Then%>
                                                <% Else%>
                                                <th style="text-align: right;">
                                                    Matching Bonus
                                                </th>
                                                <th style="text-align: right;">
                                                    Slab
                                                </th>
                                                <% End If%>
                                                <th style="text-align: right;">
                                                    Comm.
                                                </th>
                                            </tr>
                                            <asp:Repeater ID="repMentorship" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <%#Eval("IdNo")%>
                                                        </td>
                                                        <% If Session("Compid") = "1075" Then%>
                                                        <td style="text-align: left;">
                                                            <%#Eval("Name")%>
                                                        </td>
                                                        <% Else%>
                                                        <% End If%>
                                                        <td style="text-align: right;">
                                                            <%#Eval("MLevel")%>
                                                        </td>
                                                        <% If Session("Compid") = "1075" Then%>
                                                        <% Else%>
                                                        <td style="text-align: right;">
                                                            <%#Eval("PairIncome")%>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <%#Eval("Slab")%>
                                                        </td>
                                                        <% End If%>
                                                        <td style="text-align: right;">
                                                            <%#Eval("Comm")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                    <div id="divlevelIncomeGoldwings" runat="server" visible="false">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <td colspan="6" style="background-color: #999999; text-align: center; color: White;">
                                                    <b>
                                                        <% If Session("Compid") = "1023" Then%>
                                                        Upline Bonus
                                                        <% ElseIf Session("Compid") = "1075" Then%>
                                                        Direct Income
                                                        <% ElseIf Session("Compid") = "1090" Then%>
                                                        Sponsor Matching Income
                                                        <% Else%>
                                                        Level Income
                                                        <% End If%>
                                                    </b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="text-align: left;">
                                                    IDNO
                                                </th>
                                                <%--<th style="text-align: left;">
                                                    Member Name
                                                </th>--%>
                                                <th style="text-align: right;">
                                                    <% If Session("Compid") = "1075" Then%>
                                                    Business
                                                    <% Else%>
                                                    Level
                                                    <% End If%>
                                                </th>
                                                <% If Session("Compid") = "1075" Then%>
                                                <% Else%>
                                                <th style="text-align: right;">
                                                    <% If Session("Compid") = "1023" Then%>
                                                    Upline Bonus
                                                    <% ElseIf Session("Compid") = "1090" Then%>
                                                    Business
                                                    <% Else%>
                                                    Matching Bonus
                                                    <% End If%>
                                                </th>
                                                <% End If%>
                                                <th style="text-align: right;">
                                                    Slab
                                                </th>
                                                <th style="text-align: right;">
                                                    <% If Session("Compid") = "1090" Then%>
                                                    Income
                                                    <% Else%>
                                                    Comm.
                                                    <% End If%>
                                                </th>
                                            </tr>
                                            <asp:Repeater ID="RepLevelGoldwings" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <%#Eval("IdNo")%>
                                                        </td>
                                                        <%-- <td style="text-align: left;">
                                                            <%#Eval("Name")%>
                                                        </td>--%>
                                                        <td style="text-align: right;">
                                                            <% If Session("Compid") = "1075" Then%>
                                                            <%#Eval("Business")%>
                                                            <% Else%>
                                                            <%#Eval("MLevel")%>
                                                            <% End If%>
                                                        </td>
                                                        <% If Session("Compid") = "1075" Then%>
                                                        <% Else%>
                                                        <td style="text-align: right;">
                                                            <%#Eval("PairIncome")%>
                                                        </td>
                                                        <% End If%>
                                                        <td style="text-align: right;">
                                                            <%#Eval("Slab")%>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <%#Eval("Comm")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                    <div id="divDownlineIncome" runat="server" visible="false">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <td colspan="6" style="background-color: #999999; text-align: center; color: White;">
                                                    <b>
                                                        <% If Session("Compid") = "1090" Or Session("compid") = "1091" Then%>
                                                        Direct Sponsor Income
                                                        <% Else%>
                                                        Downline Income Detail
                                                        <% End If%>
                                                    </b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="text-align: left;">
                                                    IDNO
                                                </th>
                                                <% If Session("Compid") = "1090" Or Session("compid") = "1091" Then%>
                                                <% Else%>
                                                <th style="text-align: right;">
                                                    <% If Session("Compid") = "1090" Or Session("compid") = "1091" Then%>
                                                    Member Name
                                                    <% Else%>
                                                    Level
                                                    <% End If%>
                                                </th>
                                                <% End If%>
                                                <th style="text-align: right;">
                                                    <% If Session("Compid") = "1090" Or Session("compid") = "1091" Then%>
                                                    Business
                                                    <% Else%>
                                                    Downline Bonus
                                                    <% End If%>
                                                </th>
                                                <th style="text-align: right;">
                                                    Slab
                                                </th>
                                                <th style="text-align: right;">
                                                    <% If Session("Compid") = "1090" Or Session("compid") = "1091" Then%>
                                                    Income
                                                    <% Else%>
                                                    Comm.
                                                    <% End If%>
                                                </th>
                                            </tr>
                                            <asp:Repeater ID="repdownlineincome" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <%#Eval("IdNo")%>
                                                        </td>
                                                        <% If Session("Compid") = "1090" Or Session("compid") = "1091" Then%>
                                                        <% Else%>
                                                        <td style="text-align: right;">
                                                            <% If Session("Compid") = "1090" Or Session("compid") = "1091" Then%>
                                                            <%#Eval("Name")%>
                                                            <% Else%>
                                                            <%#Eval("MLevel")%>
                                                            <% End If%>
                                                        </td>
                                                        <% End If%>
                                                        <td style="text-align: right;">
                                                            <% If Session("Compid") = "1090" Or Session("compid") = "1091" Then%>
                                                            <%#Eval("Business")%>
                                                            <% Else%>
                                                            <%#Eval("PairIncome")%>
                                                            <% End If%>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <%#Eval("Slab")%>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <%#Eval("Comm")%>
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
