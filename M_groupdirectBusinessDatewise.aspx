<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="M_groupdirectBusinessDatewise.aspx.vb" Inherits="M_groupdirectBusinessDatewise"
    Title="Untitled Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Direct Member</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-4">
                                    <asp:Label ID="LblMemberID" runat="server" Text="Member Id:"></asp:Label>
                                    <asp:TextBox ID="txtMemberId" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId"
                                        runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblSessionDate" runat="server" Text="Choose From Date : "></asp:Label>
                                    <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFromDate"
                                        ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblToDate" runat="server" Text="Choose To Date : "></asp:Label>
                                    <asp:TextBox ID="TxtToDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                        ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        <asp:Button ID="BtnSearch" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Save" />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label></div>
                                </div>
                            </div>
                            <div id="DivSideA" runat="server" class="col-md-12" visible="false">
                                <asp:Label ID="Label2" runat="server" Text="Total Records" Visible="false"></asp:Label>
                                <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                <div class="table-responsive" style="overflow: scroll;">
                                    <table id="Table1" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>
                                                    SNo
                                                </th>
                                                <th>
                                                    User ID
                                                </th>
                                                <th>
                                                    Name
                                                </th>
                                                <th>
                                                    Activation Date
                                                </th>
                                                <th>
                                                    Rank
                                                </th>
                                                <th>
                                                    Self Business
                                                </th>
                                                <th>
                                                    Team Business
                                                </th>
                                                <th>
                                                    Total Business
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Grdtotal" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%--  <asp:Label ID="lblID" runat="server" Text='<%#Eval("FormNo")%>' Visible="false"></asp:Label>--%>
                                                            <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                        </td>
                                                        <td>
                                                            <%#Eval("IDno")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("MemberName")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Activation Date")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Rank")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Investment")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("TeamInvestment")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Total")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                    <br />
                                    <table id="customers2" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>
                                                    Leg No.
                                                </th>
                                                <th>
                                                    User ID
                                                </th>
                                                <th>
                                                    Name
                                                </th>
                                                <th>
                                                    Joining Date
                                                </th>
                                                <th>
                                                    Activation Date
                                                </th>
                                                <th>
                                                    Rank
                                                </th>
                                                <th>
                                                    Self Business
                                                </th>
                                                <th>
                                                    Team Business
                                                </th>
                                                <th>
                                                    Total Business
                                                </th>
                                                <th>
                                                    Downline
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="DLDirects" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblID" runat="server" Text='<%#Eval("FormNo")%>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                        </td>
                                                        <td>
                                                            <%#Eval("IDno")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("MemFirstName")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Doj")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("UpgradeDate")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Rank")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Investment")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("TeamInvestment")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Total")%>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblStatus" runat="server" Text="Downline"></asp:Label>
                                                            <asp:ImageButton ID="edit" runat="server" ImageUrl="images/down.png" Style="background-color: White;"
                                                                OnClick="PerformData" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <!-- end of weather widget -->
        </div>
    </div>
</asp:Content>
