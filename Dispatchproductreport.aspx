<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="Dispatchproductreport.aspx.vb" Inherits="Dispatchproductreport"
    Title="" %>

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
                            Dispatch Product Report
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                Member Id:
                                <asp:TextBox runat="server" ID="TxtMemID" class="form-control">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                Start Date:
                                <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                    Format="dd-MMM-yyyy">
                                </AjaxToolkit:CalendarExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                    ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-3">
                                End Date:
                                <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                    Format="dd-MMM-yyyy">
                                </AjaxToolkit:CalendarExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                    ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-3" style="padding-top: 1%;">
                                <asp:Button ID="BtnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                    Visible="false" /></div>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-12" id="divContent" runat="server" visible="false">
                                    <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                                </div>
                                <div style="margin-bottom: 20px">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" RowStyle-Height="20px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        AllowSorting="true" OnSorting="GvData_Sorting" ShowHeader="true" PageSize="10"
                                        EmptyDataText="No data to display.">
                                        <Columns>
                                            <asp:BoundField DataField="ReqID" HeaderText="ReqID" Visible="false" />
                                            <asp:TemplateField HeaderText="Member ID">
                                                <ItemTemplate>
                                                    <%# Eval("IDNo") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Member Name">
                                                <ItemTemplate>
                                                    <%# Eval("name") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Request Date">
                                                <ItemTemplate>
                                                    <%# Eval("OrderDate") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Package Name">
                                                <ItemTemplate>
                                                    <%#Eval("kitname")%>
                                                    <%--<asp:DropDownList ID="ddlPackage" runat="server" DataTextField="kitname" DataValueField="kitid">
                                                    </asp:DropDownList>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <%# Eval("useraddress") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PinCode">
                                                <ItemTemplate>
                                                    <%# Eval("PinCode") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="State">
                                                <ItemTemplate>
                                                    <%# Eval("statename") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="City">
                                                <ItemTemplate>
                                                    <%# Eval("City") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="District">
                                                <ItemTemplate>
                                                    <%# Eval("District") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dispatch Date">
                                                <ItemTemplate>
                                                    <%# Eval("approvedate") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dispatch Status">
                                                <ItemTemplate>
                                                    <%# Eval("Status") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remark">
                                                <ItemTemplate>
                                                    <%# Eval("Remark") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dispatch">
                                                <ItemTemplate>
                                                    <a href='<%# "DispatchStatus.aspx?OrderID=" & Eval("ReqID") & "&FormNo=" & Eval("FormNo") & "&ClaimId=" & Eval("ReqID")& "&type=WR" %>'
                                                        onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 854,height: 339,marginTop : 0 } )">
                                                        <asp:Button ID="Btndispectch" runat="server" ForeColor="white" Text="Dispatch" CssClass="btn btn-dark "
                                                            Visible='<%# Eval("IsVisible") %>' />
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice">
                                                <ItemTemplate>
                                                    <a href='<%# "DispatInvoice.aspx?OrderNo=" & Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(If(Eval("billno") IsNot Nothing, Eval("billno").ToString(), ""))) %>'
                                                        target="_blank" class="btn btn-sm btn-primary">Invoice </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
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
        </div>
</asp:Content>
