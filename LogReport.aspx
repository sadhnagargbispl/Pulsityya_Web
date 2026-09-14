<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="LogReport.aspx.vb" Inherits="App_UI_Application_Pages_LogReport" Title=""
    EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style type="text/css">
    
.PagerStyle 
{
    background-image: url(../Images/td.jpg);
    background-position:center;
    background-repeat:repeat-x;
    background-color:#ffffff; 
    font-weight:bold;
    text-align: center;
    width: 00px;   
}

.PagerStyle table
{
	text-align:center;
    margin:auto;
}
.PagerStyle table td
{
    border:0px;
    padding:5px;
}
.PagerStyle td
{
    border-top: #1d1d1d 3px solid;
}
.PagerStyle a
{
    color:#000000;
    text-decoration:none;
    padding:2px 10px 2px 10px;
    border-top:solid 1px #777777;
    border-right:solid 1px #333333;
    border-bottom:solid 1px #333333;
    border-left:solid 1px #777777;
}
.PagerStyle span
{
    font-weight:bold;
    color:#000000;
    text-decoration:none;
    padding:2px 10px 2px 10px;
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
                            Log Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive">
                            <div class="col-md-12">
                                <div class="col-md-3">
                                    <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
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
                                    <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-2" style="padding-top :2%">
                                    <asp:RadioButtonList ID="RbtUser" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                        AutoPostBack="true">
                                        <asp:ListItem Text="Member" Value="M" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Admin" Value="A"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="col-md-4">
                                    <asp:CheckBox ID="ChkMem" runat="server" Text="Member Wise :" TextAlign="Left" />
                                    <asp:TextBox ID="txtMember" runat="server" class="form-control"></asp:TextBox>
                                    <asp:DropDownList ID="DDlMember" runat="server" class="form-control" Visible="false">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-8">
                                    <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                  
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                   
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" />
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" />
                                </div>
                                <div class="col-md-4">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                    color: Red"></asp:Label>
                            </div>
                            <div style="margin-top: 20px; margin-bottom: 20px;">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."
                                    Visible="false"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                    color: Gray"></asp:Label>
                            </div>
                            <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                margin-bottom: 25px;">
                                <asp:GridView ID="GvData" runat="server" RowStyle-Height="25px" GridLines="Both"
                                    AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" PageSize="10" EmptyDataText="No data to display.">
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass ="PagerStyle" />
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
</asp:Content>
