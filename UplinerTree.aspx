<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="UplinerTree.aspx.vb" Inherits="UplinerTree" Title="Untitled Page" %>

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
                            Upliner Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div class="col-md-12">
                                <div class="col-md-4">
                                    <asp:Label ID="LblType" Text="Type" runat="server"></asp:Label>
                                    <asp:RadioButtonList ID="RbtType" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" AutoPostBack ="true">
                                        <asp:ListItem Text="Team View" Value="B" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Generation View" Value="G"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="LblMemberID" runat="server" Text="Member Id:"></asp:Label>
                                    <asp:TextBox ID="txtMemberId" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldvalidator1" runat="server" ControlToValidate="txtMemberId"
                                        ErrorMessage="Enter Member Id" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="LblLevel" runat="server" Text=" Choose Level:"></asp:Label>
                                    <asp:DropDownList ID="DDLLevel" runat="server" class="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12">
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
                                    <asp:TextBox ID="TxtToDate" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                        ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4" style="padding-top: 2%">
                                    <asp:Button ID="BtnSearch" runat="server" Text="Search" class="btn btn-primary" />
                                    <asp:Button ID="BtnExport" runat="server" Text="Export To Excel" class="btn btn-primary" />
                                </div>
                            </div>
                            <div id="TrDate" runat="server" class="col-md-12" visible="false">
                                <asp:CheckBox runat="server" ID="ChkDate" AutoPostBack="true" />
                                <asp:Label ID="LblDate" runat="server" Text=" Choose Date Type:"></asp:Label>
                                <asp:DropDownList ID="DDlDate" runat="server" class="form-control">
                                    <asp:ListItem Text="Date Of Activation" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Date Of Joining" Value="J"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-12">
                                <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label></div>
                            <div class="col-md-12">
                                <asp:GridView ID="GrdDirects" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" PageSize="10" EmptyDataText="No data to display.">
                                    <Columns>
                                        <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IDNo" HeaderText="ID No"></asp:BoundField>
                                        <asp:BoundField DataField="MemberName" HeaderText="Member Name"></asp:BoundField>
                                        <asp:BoundField DataField="DateOfJoining" HeaderText="Date Of joining"></asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Active Status"></asp:BoundField>
                                        <asp:BoundField DataField="UpgradeDate" HeaderText="Date Of Activation"></asp:BoundField>
                                        <asp:BoundField DataField="PackageName" HeaderText="Package Name"></asp:BoundField>
                                        <asp:BoundField DataField="Level" HeaderText="Level"></asp:BoundField>
                         
                                            <asp:TemplateField Headertext="Cadre">
                                            <ItemTemplate>
                                            <asp:Label ID="LblCadre" runat="server" Text= '<%# Eval("Cadre") %>' Visible='<%# Eval("CadreVisible") %>'></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Headertext="TotalPV">
                                            <ItemTemplate>
                                            <asp:Label ID="LblTotalPV" runat="server" Text= '<%# Eval("TotalPv") %>' Visible='<%# Eval("CadreVisible") %>'></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>--%>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle " />
                                </asp:GridView>
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
</asp:Content>
