<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="Dispatchproduct.aspx.vb" Inherits="App_UI_Application_Pages_Dispatchproduct" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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

    <script type="text/javascript">
        //$(document).ready(function() { $('[id$=chkSelectAll]').click(function() { $("[id$='chkSelect']").attr('checked', this.checked); }); });
        //    function reset() {
        //        $("[id$='chkSelect']").prop('checked', false);
        //    }

        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }

        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%= GvData.ClientID %>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 1; i < grid.rows.length; i++) {
                    //get the reference of first column
                    cell = grid.rows[i].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell 
                            //checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12" style="min-height: 300px !important;">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Goal Achievement</h2>
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
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkMem" runat="server" Text="MemberId :" TextAlign="Left" />
                                        <asp:TextBox ID="txtMember" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        Datewise
                                        <asp:DropDownList ID="ddllist" runat="server" class="form-control">
                                            <asp:ListItem Text="Achieve Date" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Eligible Date" Value="E"></asp:ListItem>
                                            <asp:ListItem Text="Dispatch Date" Value="D"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
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
                                    <div class="col-md-2">
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
                                    <div class="col-md-2">
                                        <asp:RadioButtonList ID="rbtnlist" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Dispatched" Value="Y" Selected="True">
                                            </asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="N">
                                            </asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                    <br />
                                    <div class="col-md-1">
                                        <asp:Button runat="server" ID="BtnSearch" class="btn btn-primary" Text="Search" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button runat="server" ID="BtnExport" class="btn btn-primary" Text="Export To Excel"
                                            Enabled="false" /></div>
                                    <div class="col-md-1">
                                        <asp:Button ID="BtnVerifiy" runat="server" Text="Dispatch" class="btn btn-primary"
                                            Enabled="false" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="BTnUnVerification" Enabled="false" OnClientClick="this.disabled=true;"
                                            runat="server" Visible="false" Text="Reject" class="btn btn-primary" />
                                    </div>
                                </div>
                                <div id="DivRemark" runat="server" visible="false">
                                    <table id="TblRemark" runat="server" align="center" style="background-color: #cceeff;
                                        color: #000000; border-color: Black; border-width: 1px; margin-top: -10px;">
                                        <tr>
                                            <td align="left">
                                                <br />
                                                <strong>Reason</strong>*
                                            </td>
                                            <td align="left">
                                                <br />
                                                <asp:DropDownList ID="DDlREason" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <br />
                                                <strong>Remark</strong>*
                                            </td>
                                            <td align="left">
                                                <br />
                                                <asp:TextBox ID="TxtARemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <br />
                                                <asp:Button ID="BtnUnVerify" runat="server" class="btn btn-primary" Text="Reject"
                                                    OnClientClick="return confirmation();this.disabled=true;" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-md-12" style="overflow: scroll">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="None" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                        PageSize="20" EmptyDataText="No data to display.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="CheckAll">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# Eval("enablestatus") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex +1 %>.</ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Aid" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblIdno" runat="server" Text='<%# Eval("Aid") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblmemberIdno" runat="server" Text='<%# Eval("IDNO") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IDNO" HeaderText="idno" />
                                            <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                            <asp:BoundField DataField="Achievementdate" HeaderText="Eligible Date" />
                                            <asp:BoundField DataField="Eligibledate" HeaderText="Achieve Date" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remark" />
                                            <asp:BoundField DataField="DispatchDate" HeaderText="DispatchDate" />
                                        </Columns>
                                        <PagerStyle CssClass="PagerStyle " />
                                        <PagerSettings Mode="NumericFirstLast" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
