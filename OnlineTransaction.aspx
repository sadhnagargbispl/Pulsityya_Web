<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="OnlineTransaction.aspx.vb" Inherits="OnlineTransaction"
     EnableEventValidation="false" %>

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
                            Online Transaction </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center"> <div class="col-md-12">
                                    <div class="col-md-4">
                                        <asp:RadioButtonList ID="RbReqStatus" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Table">
                                            <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Complete" Value="Complete"></asp:ListItem>
                                            <asp:ListItem Text="Failed" Value="Failed"></asp:ListItem>
                                           
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2">
                                        Member Id:                                   </div>
                                    <div class="col-md-3">
                                        <asp:TextBox runat="server"  ID="TxtMemID" class="form-control"></asp:TextBox></div>
                                    <div class="col-md-3">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        Transaction Date:</div>
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
                                    <div class="col-md-2">
                                        To Date:</div>
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
                                    </div>
                                </div>
                               
                                <div class="col-md-12">
                                <asp:Button ID="BtnSearch" runat="server" class="btn btn-primary" Text="Search"
                                         />
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" />
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                    <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                        Visible="false" />
                                </div>
                                <div class="col-md-12" id="divContent" runat="server" visible="false">
                                    <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                                </div>
                                <div style="margin-bottom: 20px">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        AllowSorting="true" OnSorting="GvData_Sorting" ShowHeader="true" PageSize="10"
                                        EmptyDataText="No data to display.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="TransactionId" Visible="false" SortExpression="TransactionId">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("TransactionId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                          
                                            <asp:BoundField DataField="Idno" HeaderText="MemberId" SortExpression="Idno" />
                                            <asp:BoundField DataField="MemberName" HeaderText="Member Name" SortExpression="Member Name" />
                                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                                            <asp:BoundField DataField="Kitname" HeaderText="Package Name" SortExpression="Package Name" />
                                            <asp:BoundField DataField="KitAmount" HeaderText="Package Amount" SortExpression="Package Amount" />
                                            
                                            <asp:BoundField DataField="TransactionId" HeaderText="Transaction ID" SortExpression="Transaction Id" />
                                            
                                            <asp:BoundField DataField="TransactionDate" HeaderText="Transaction Date" SortExpression="Transaction Date" />
                                            
                                            <asp:BoundField DataField="SuccessDate" HeaderText="Success Date" SortExpression="Success Date" />
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("Status") %>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center"
                                                ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    
                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup" Visible='<%# Eval("VisibleStatus") %>'><i class="fa fa-edit" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton></ItemTemplate>
                                                <HeaderStyle Width="85px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
