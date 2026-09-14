<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="Directreport.aspx.vb" Inherits="App_UI_Application_Pages_Directreport"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
                        <div align="center">
                            <div class="col-md-12" style="padding: 1.3%">
                                <div class="col-md-2">
                                    <asp:Label ID="LblMemberID" runat="server" Text="Member Id:"></asp:Label></div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtMemberId" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    DirectMember</div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="Txtnumber" runat="server" class="form-control"></asp:TextBox></div>
                                <div class="col-md-1">
                                    Page Size:</div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed"
                                        class="form-control">
                                        <asp:ListItem Text="10" Value="10" />
                                        <asp:ListItem Text="20" Value="20" />
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
                            <br />
                            <div class="col-md-12">
                                <div class="col-md-2">
                                    <asp:Label ID="lblSessionDate" runat="server" Text="Choose From Date : "></asp:Label></div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                        ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblToDate" runat="server" Text="Choose To Date : "></asp:Label></div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                        ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="BtnSearch" runat="server" Text="Search" class="btn btn-primary" />
                                    <asp:Button ID="BtnExport" runat="server" Text="Export To Excel" class="btn btn-primary" />
                                    <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label></div>
                            </div>
                            <div class="col-md-12">
                                <br />
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <div id="gvContainer" runat="server">
                                        <div class="col-md-12">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                                    color: Red">
                                                </asp:Label>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px;
                                                    color: Gray"></asp:Label></div>
                                            <div class="col-md-2">
                                                <asp:Label ID="LblTotalEpin" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <div class="col-md-6">
                                            </div>
                                        </div>
                                        <asp:GridView ID="GvData" Visible="true" runat="server" AutoGenerateColumns="False"
                                            RowStyle-Height="25px" GridLines="Both" AllowPaging="false" class="table table-bordered"
                                            HeaderStyle-CssClass="bg-primary" ShowHeader="true" PageSize="15" EmptyDataText="No data to display."
                                            AllowSorting="true" OnSorting="GvData_Sorting">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IDNo" HeaderText="ID No" SortExpression="IDNo"></asp:BoundField>
                                                <asp:BoundField DataField="MemberName" HeaderText="Member Name" SortExpression="MemberName">
                                                </asp:BoundField>
                                                <%--     <asp:BoundField DataField="City" HeaderText="City"></asp:BoundField>--%>
                                                <asp:BoundField DataField="Mobl" HeaderText="MobileNo" SortExpression="Mobl" />
                                                <asp:BoundField DataField="Bankname" HeaderText="Bank Name" SortExpression="Bankname" />
                                                <asp:BoundField DataField="Acno" HeaderText="Account No" SortExpression="Acno" />
                                                <asp:BoundField DataField="Branchname" HeaderText="Branch Name" SortExpression="Branchname" />
                                                <asp:BoundField DataField="Ifscode" HeaderText="IFSC Code" SortExpression="Ifscode" />
                                                <%-- <asp:BoundField DataField="ActiveStatus" HeaderText="Active Status"></asp:BoundField>--%>
                                                <%--    <asp:BoundField DataField="UpgradeDate" Visible HeaderText="Date Of Activation"></asp:BoundField>--%>
                                                <%--   <asp:TemplateField HeaderText="Direct member" SortExpression="Directmember">
                                                            <ItemTemplate>
                                                              
                                                                    <asp:Label ID="Lbldate" runat="server" ForeColor="Blue" Text='<%# Eval("UpgradeDate") %>'></asp:Label></a><br />
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Direct member" SortExpression="Directmember">
                                                    <ItemTemplate>
                                                        <a href='<%# "Viewdirect.aspx?Idno="& Eval("IDNo")&"&fromDate="& Eval("FromDate")&"&Todate="& Eval("Todate") %>'
                                                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 620,height: 450,marginTop : 0 } )">
                                                            <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text='<%# Eval("Directmember") %>'></asp:Label></a><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
    </div>
    <div class="row">
    </div>
</asp:Content>
