<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="VendorPaymentReceiveReport.aspx.vb" Inherits="VendorPaymentReceiveReport"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        //$(document).ready(function() { $('[id$=chkSelectAll]').click(function() { $("[id$='chkSelect']").attr('checked', this.checked); }); });
        //    function reset() {
        //        $("[id$='chkSelect']").prop('checked', false);
        //    }



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
                            Vendor Payment Receive Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" Text="Member ID: "></asp:Label>
                                <asp:TextBox runat="server" Enabled="true" ID="TxtMemID" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
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
                            <div class="col-md-2">
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
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" Text="Bill No: "></asp:Label>
                                <asp:TextBox runat="server" Enabled="true" ID="txtBillNo" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                Status :
                                <asp:DropDownList ID="RbtStatus" runat="server" RepeatDirection="Horizontal" CssClass="form-control"
                                    RepeatLayout="Flow">
                                    <asp:ListItem Text="All" Value="Z" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Approve" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2" style="padding: 1%">
                                <asp:Button ID="BtnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel"
                                    Visible="false" />
                            </div>
                        </div>
                        <div class="col-md-12">
                            <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                color: Red"></asp:Label>
                        </div>
                        <br />
                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        <div class="table-responsive makeitresponsivegrid">
                            <asp:Label ID="LblARemark" runat="server" ForeColor="red" Visible="false"></asp:Label>
                            <div align="center" class="col-md-12">
                                <asp:Label ID="lblMsg" runat="server" Font-Size="13px" Visible="False"></asp:Label>
                                <div style="margin-top: 15px; overflow: scroll;">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="None" AllowPaging="True" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="30" EmptyDataText="No data to display." Width="100%"
                                        PagerStyle-CssClass="PagerStyle">
                                        <Columns>
                                            <asp:TemplateField HeaderText="TransactionId" Visible="false" SortExpression="TransactionId">
                                                <ItemTemplate>
                                                    <asp:Label ID="HdnBillNo" runat="server" Text='<%# Eval("ChqNo") %>'></asp:Label>
                                                    <asp:Label ID="HdnReqNo" runat="server" Text='<%# Eval("ReqNo") %>'></asp:Label>
                                                    <asp:Label ID="HdnAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                    <asp:Label ID="Lblidno" runat="server" Text='<%# Eval("idno") %>'></asp:Label>
                                                    <asp:Label ID="Lblformno" runat="server" Text='<%# Eval("formno") %>'></asp:Label>
                                                    <asp:Label ID="LblShopname" runat="server" Text='<%# Eval("PayMode") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Sno" HeaderText="Sr.No" />
                                            <asp:BoundField DataField="ReqDate" HeaderText="Date" />
                                            <asp:BoundField DataField="PayMode" HeaderText="Vendor Name"></asp:BoundField>
                                            <asp:BoundField DataField="MobileNo" HeaderText="Mobile No"></asp:BoundField>
                                            <asp:BoundField DataField="City" HeaderText="City"></asp:BoundField>
                                            <asp:BoundField DataField="statename" HeaderText="State" />
                                            <asp:BoundField DataField="PinCode" HeaderText="Pin Code" />
                                            <asp:BoundField DataField="idno" HeaderText="User ID" />
                                            <asp:BoundField DataField="memfirstname" HeaderText="User Name" />
                                            <asp:BoundField DataField="Chqno" HeaderText="Bill No." />
                                            <asp:BoundField DataField="Amount" HeaderText="Bill Amount"></asp:BoundField>
                                            <asp:BoundField DataField="Commission" HeaderText="Bonus"></asp:BoundField>
                                            <asp:BoundField DataField="status" HeaderText="Payment Status" />
                                            <asp:BoundField DataField="ApproveDate" HeaderText="Payment Approve Date" />
                                            <asp:TemplateField HeaderText="Approve" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center"
                                                ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <asp:Button ID="BtnSubmit" runat="server" Text="Approve" TabIndex="3" class="form-btn"
                                                        OnClick="DeleteGroup" Visible='<%# Convert.ToBoolean(Eval("statustStatus")) %>'
                                                        ForeColor="Black" OnClientClick="return confirm('Are You Sure To Proceed?');" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="85px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
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
