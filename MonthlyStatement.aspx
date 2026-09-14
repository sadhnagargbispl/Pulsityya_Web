<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MonthlyStatement.aspx.vb"
    Inherits="MonthlyStatement" %>

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
                                        Phone/Mobile No.:-
                                        <%=Session("CompMobile")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="row">
                        <div class="col-md-12" style="background-color: #999999; text-align: center; color: White;">
                            <b><span style="font-size: 18px; font-family: Arial Baltic;">Monthly Income Statement
                            </span></b>
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
                                <td width="40%" valign="top" id="tdGross" runat="server">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <td style="text-align: right;" colspan="2">
                                                    <asp:Repeater ID="rptgross" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="text-align: left; font-weight: bold">
                                                                    <%#Eval("Gross")%>
                                                                </td>
                                                                <td style="text-align: right; font-weight: bold">
                                                                    <%#Eval("Amount")%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top" id="tdClosing" runat="server">
                                    <div class="col-md-12">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <td style="text-align: right;" colspan="2">
                                                    <asp:Repeater ID="rptclose" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="text-align: left; font-weight: bold">
                                                                    <%#Eval("Gross")%>
                                                                </td>
                                                                <td style="text-align: right; font-weight: bold">
                                                                    <%#Eval("Amount")%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div class="row">
                        <table class="table-bordered" width="98%">
                            <tr>
                                <td width="40%" valign="top">
                                    <div class="col-md-12" id="DivSlefRepurBV" runat="server" visible="false">
                                        <table class="table-bordered" width="98%">
                                            <tr>
                                                <td style="background-color: #999999; text-align: left; color: White; width: 70%;">
                                                    Self Repurchase
                                                    <%=Session("ColName2")%>
                                                </td>
                                                <td id="selfREP" runat="server" style="text-align: right;">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-md-12" id="DivLifeSalePV" runat="server" visible="false">
                                        <table class="table-bordered" width="98%">
                                            <tr>
                                                <td style="background-color: #999999; text-align: left; color: White; width: 70%;">
                                                 Franchise Self Sale PV
                                                </td>
                                                <td id="TDSalePV" runat="server" style="text-align: right;">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <div class="col-md-12" id="DivLifeSaleEP" runat="server" visible="false">
                                        <table class="table-bordered" width="98%">
                                            <tr>
                                                <td style="background-color: #999999; text-align: left; color: White; width: 70%;">
                                                    Franchise Self Sale EP
                                                </td>
                                                <td id="TDSaleEP" runat="server" style="text-align: right;">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                      <br />
                                    <div class="col-md-12" id="DivSponsorSaleEP" runat="server" visible="false">
                                        <table class="table-bordered" width="98%">
                                            <tr>
                                                <td style="background-color: #999999; text-align: center; color: White; width: 70%;"
                                                    colspan="6">
                                                    Franchise Sponsor Commission Detail
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Franchise ID
                                                </td>
                                                <td style="text-align: right;">
                                                    EP
                                                </td>
                                            </tr>
                                            <asp:Repeater ID="FrnRep" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <%#Eval("UserPartyCode")%>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <%#Eval("EP")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                    <br />
                                    <br />
                                    <div class="col-md-12" id="divUcmPoint" runat="server" visible="false">
                                        <table class="table-bordered" width="98%">
                                            <tr>
                                                <td colspan="6" style="background-color: #999999; text-align: center; color: White;">
                                                    <b>Income Point</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Leadership Bonus
                                                </td>
                                                <td id="TdLeadership" runat="server">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Master Leadership Bonus
                                                </td>
                                                <td id="TdMasterLeadership" runat="server">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Super Master Bonus
                                                </td>
                                                <td id="Tdsupermasterbonus" runat="server">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Travelling Fund
                                                </td>
                                                <td id="TdTravelling" runat="server">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Car Fund
                                                </td>
                                                <td id="TdCar" runat="server">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Royalty Income
                                                </td>
                                                <td id="TdRoyalty" runat="server">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    House Fund
                                                </td>
                                                <td id="TdHouse" runat="server">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="40%" valign="top">
                                    <div id="divMentorshipLife" runat="server" visible="false">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <td colspan="6" style="background-color: #999999; text-align: center; color: White;">
                                                    <% If Session("Compid") = "1030" Then%>
                                                    <b>Repurchase Incentive Detail</b>
                                                    <% Else%>
                                                    <b>Performance Bonus Detail</b>
                                                    <% End If%>
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
                                                    Level
                                                </th>
                                                <th style="text-align: right;">
                                                    <% If Session("Compid") = "1030" Then%>
                                                    Repurchase EP
                                                    <% ElseIf Session("Compid") = "1038" Then%>
                                                    BV
                                                    <% Else%>
                                                    Matching Bonus
                                                    <% End If%>
                                                </th>
                                                <th style="text-align: right;">
                                                    Slab
                                                </th>
                                                <th style="text-align: right;">
                                                    Diff. Slab
                                                </th>
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
                                                        <%-- <td style="text-align: left;">
                                                            <%#Eval("Name")%>
                                                        </td>--%>
                                                        <td style="text-align: right;">
                                                            <%#Eval("MLevel")%>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <%#Eval("PairIncome")%>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <%#Eval("MainSlab")%>
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
                                    <div id="divproholistic" runat="server" visible="false">
                                        <table class="table-bordered" width="100%" cellpadding="1">
                                            <tr>
                                                <td colspan="6" style="background-color: #999999; text-align: center; color: White;">
                                                    <% If Session("Compid") = "1075" Then%>
                                                    
                                                     <b>Repurchase Level Income Detail</b>
                                                    
                                                    <% End If%>
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
                                                    Level
                                                </th>
                                                <th style="text-align: right;">
                                                    BV
                                                </th>
                                                <%--<th style="text-align: right;">
                                                   Slab
                                                </th>--%>
                                                <th style="text-align: right;">
                                                     Slab
                                                </th>
                                                <th style="text-align: right;">
                                                    Comm.
                                                </th>
                                            </tr>
                                            <asp:Repeater ID="rptproholistic" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <%#Eval("IdNo")%>
                                                        </td>
                                                        <%-- <td style="text-align: left;">
                                                            <%#Eval("Name")%>
                                                        </td>--%>
                                                        <td style="text-align: right;">
                                                            <%#Eval("MLevel")%>
                                                        </td>
                                                        <td style="text-align: right;">
                                                         <%#Eval("PairIncome")%>
                                                        </td>
                                                       <%-- <td style="text-align: right;">
                                                            <%#Eval("MainSlab")%>
                                                        </td>--%>
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
