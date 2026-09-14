<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="DispatchProductMaster.aspx.vb" Inherits="App_UI_Application_Pages_DispatchProductMaster"
    Title="" EnableEventValidation="false" %>

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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="images/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Dispatch Product Master</h2>
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
                                    <div id="DivTopup" runat="server" visible="false" style="width: 100%">
                                        <center>
                                            <table align="center" border="1px" style="background-color: #394a59; color: #d0d8df;">
                                                <tr>
                                                    <td align="left">
                                                        <strong>Id No.</strong>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtIdNo" CssClass="TxtBox" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <strong>Courier Name</strong>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDlCourier" runat="server" CssClass="DDl" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="LblOrderNo" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="OtherCourier" runat="server" visible="false">
                                                    <td align="left">
                                                        <strong>Other</strong>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtCourier" CssClass="TxtBox" runat="server"></asp:TextBox>
                                                        <asp:Label ID="LblAddress" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="LblPlanId" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <strong>Docket No.</strong>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtDocket" CssClass="TxtBox" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="BtnDispatch" runat="server" class="buttonBG" Style="height: 24px;
                                                            width: 64px;" Text="Dispatch" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-2">
                                            <asp:CheckBox ID="ChkMember" runat="server" Text="Choose Member Id:" /></div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="TxtMemId" runat="server" class="form-control"></asp:TextBox></div>
                                        <div class="col-md-2">
                                            Search By:
                                            <asp:Label ID="LblMemName" runat="server" Visible="False"></asp:Label></div>
                                        <div class="col-md-3">
                                            <asp:RadioButtonList ID="RbtSearch" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Pending" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="Dispatch" Value="Y"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="col-md-2">
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
                                    <div class="col-md-12" style="margin-top: 2%;">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblStartDate" runat="server" Text="From Date : "></asp:Label>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                                Format="dd-MMM-yyyy">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                                ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:Label ID="lblEndDate" runat="server" Text="To Date : "></asp:Label>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                                Format="dd-MMM-yyyy">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                                ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </div>
                                        <div class="col-md-3">
                                            <a href="images/UploadImage/CourierDetail.xlsx" style="color: Green">Download Template</a></div>
                                    </div>
                                    <div class="col-md-12">
                                        <br />
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                        <%-- <asp:Button ID="btnshowall" runat="server" class="btn btn-primary" Text="Show All" />--%>
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                        <asp:Button ID="Btnsummery" runat="server" class="btn btn-primary" Text="Export To Excel Summery" />
                                        <asp:Button ID="BtnImport" runat="server" class="btn btn-primary" Text="Import Courier Detail"
                                            OnClientClick="this.disabled=true;" UseSubmitBehavior="false" /></div>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-12">
                                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                                    GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                    ShowHeader="true" AllowSorting="true" OnSorting="GvData_Sorting" PageSize="20"
                                                    EmptyDataText="No data to display.">
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblIdno" runat="server" Visible="false" Text='<%# Eval("Fcode") %>'></asp:Label>
                                                                <asp:Label ID="LblOrderNo" runat="server" Visible="false" Text='<%# Eval("Orderno") %>'></asp:Label>
                                                                <asp:Label ID="LblFormno" runat="server" Visible="false" Text='<%# Eval("Formno") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField ="SNo" HeaderText="SNO" SortExpression ="SNo" />
                                                        <asp:BoundField DataField="OrderNo" HeaderText="Order No" SortExpression="OrderNo" />
                                                        <asp:BoundField DataField="OrderDate" HeaderText="Order Date" SortExpression="OrderDate" />
                                                        <asp:BoundField DataField="Fcode" HeaderText="IdNo" SortExpression="Fcode" />
                                                        <asp:BoundField DataField="MemberName" HeaderText="Member Name" SortExpression="MemberName" />
                                                        <asp:BoundField DataField="MobileNo" HeaderText="MobileNo" SortExpression="MobileNo" />
                                                        <asp:BoundField DataField="ProductName" HeaderText="Product Combo" SortExpression="ProductName" />
                                                        <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" />
                                                        <asp:BoundField DataField="qty" HeaderText="Qty" SortExpression="qty" />
                                                        <asp:BoundField DataField="TotalAmount" HeaderText="TotalAmount" SortExpression="TotalAmount" />
                                                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                                        <asp:TemplateField HeaderText="Dispatch" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                                            SortExpression="DispatchStatus">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LBApprove" runat="server" Text="Dispatch" OnClientClick="return confirmation();"
                                                                    OnClick="DispatchData" Visible='<%# Eval("DispatchStatus") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CourierName" HeaderText="Courier Name" SortExpression="CourierName" />
                                                        <asp:BoundField DataField="DocketNo" HeaderText="Docket Number" SortExpression="DocketNo" />
                                                        <asp:BoundField DataField="DocketDate" HeaderText="Courier Date" SortExpression="DocketDate" />
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
        </div>
        <br />
        <br />
</asp:Content>
