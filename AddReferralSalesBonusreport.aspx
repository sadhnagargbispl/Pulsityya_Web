<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="AddReferralSalesBonusreport.aspx.vb" Inherits="AddReferralSalesBonusreport" %>

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
        .PagerStyle table23
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
                          Referral Sales Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                       <div class="col-md-12">
                        
                                <%--<div class="col-md-3" style="padding-top: 9Px">
                                Search Records By :
                                    <asp:DropDownList ID="ddlSearch" runat="server" class="form-control">
                                        <asp:ListItem Value="0" Selected="True">Search Type</asp:ListItem>
                                        <asp:ListItem Value="IDNo">Member ID</asp:ListItem>
                                        <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
                                        <asp:ListItem Value="CustomerMobileNo">Customer Mobile No</asp:ListItem>
                                        <asp:ListItem Value="CallerName">Caller Name</asp:ListItem>
                                         </asp:DropDownList>
                                </div>
                                <div class="col-md-3" style="padding-top: 9Px">
                               <br />
                                    <asp:TextBox ID="txtSrchText" runat="server" class="form-control"></asp:TextBox>
                                </div>--%>
                        <%--<div class="col-md-3" style="padding-top: 9Px">
                            From Date
                            <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                Format="dd-MMM-yyyy">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFromDate"
                                ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                        </div>--%>
                       <%-- <div class="col-md-3" style="padding-top: 9Px">
                            To Date :
                            <asp:TextBox ID="TxtToDate" runat="server" class="form-control"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                Format="dd-MMM-yyyy">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                        </div>--%>
                        <%--<div class="col-md-3" style="padding-top: 9Px">
                            Status
                            <asp:DropDownList ID="ddllist" runat="server" class="form-control">
                            <asp:ListItem Value="All" >All</asp:ListItem>
                                        <asp:ListItem Value="P">Pending</asp:ListItem>
                                        <asp:ListItem Value="A">Close</asp:ListItem>
                            </asp:DropDownList>
                        </div>--%>
                <%--        <div class="col-md-3" style="padding-top: 24Px">
                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                        <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                            color: Red"></asp:Label>
                            </div> --%>
                    </div>
                    <div class="col-md-12">
                        
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div class="col-md-12">
                                <div class="col-md-3" style="padding-top: 9Px">
                                Search Records By :
                                    <asp:DropDownList ID="ddlSearch" runat="server" class="form-control">
                                        <asp:ListItem Value="0" Selected="True">Search Type</asp:ListItem>
                                        <asp:ListItem Value="IDNo">Member ID</asp:ListItem>
                                        <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
                                        <asp:ListItem Value="CustomerMobileNo">Customer Mobile No</asp:ListItem>
                                        <asp:ListItem Value="CallerName">Caller Name</asp:ListItem>
                                         </asp:DropDownList>
                                </div>
                                <div class="col-md-3" style="padding-top: 9Px">
                               <br />
                                    <asp:TextBox ID="txtSrchText" runat="server" class="form-control"></asp:TextBox>
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
                        <div class="col-md-3" style="padding-top: 9Px">
                            Status
                            <asp:DropDownList ID="ddllist" runat="server" class="form-control">
                            <asp:ListItem Value="All" >All</asp:ListItem>
                                        <asp:ListItem Value="P">Pending</asp:ListItem>
                                        <asp:ListItem Value="A">Close</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                         <div class="col-md-3" style="padding-top: 24Px">
                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" /> <br /> <br />
                        <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                            color: Red"></asp:Label>
                            </div>
                           
                            </div>
                            <div class="col-md-12">
                                <asp:Label ID="lbl" runat="server" Font-Bold="true" Visible="false"></asp:Label>
                             
                                <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false"
                                 Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                            </div>
                            <div style="margin-bottom: 20px">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" PageSize="10" EmptyDataText="No data to display.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SNo.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="KitId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("m_id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Member ID">
                                            <ItemTemplate>
                                                <asp:Label ID="LblImageType" runat="server" Text='<%# Eval("idno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Member Name">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFileType" runat="server" Text='<%# Eval("membername") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                             <asp:Label ID="lblRemark1" runat="server" Text='<%# Eval("cname") %>'></asp:Label>
                                                </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Mobile No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("mobile") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Health Issue" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("hlissue") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus14" runat="server" Text='<%# Eval("date1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Caller Name" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus1" runat="server" Text='<%# Eval("callername") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus13" runat="server" Text='<%# Eval("remark") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus12" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Modify" >
                                            <ItemTemplate>
                                                <a href='<%# "ModifyReffrelSalebonus.aspx?m_id=" & Crypto.Encrypt(Eval("m_id"))  %>' 
                                                onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 330,marginTop : 0 } )" class="btn btn-primary">
                                                    <asp:Label ID="lblStatus123" runat="server" Text='Modify'></asp:Label>
                                                    
                                                    <asp:Label ID="LBModify" runat="server" />
                                                </a>
                                                </ItemTemplate>
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
            <div class="row">
                <!-- end of weather widget -->
            </div>
        </div>
</asp:Content>
