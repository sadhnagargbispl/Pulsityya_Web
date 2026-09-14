<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="VendorReport.aspx.vb" Inherits="VendorReport" Title="" %>

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
                            Vendor Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <asp:Label ID="Label1" runat="server" Text="Member Id: "></asp:Label>
                                <asp:TextBox runat="server" Enabled="true" ID="TxtMemID" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                Choose Start Date:
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
                                choose End Date:
                                <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                    Format="dd-MMM-yyyy">
                                </AjaxToolkit:CalendarExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                    ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-3" style="padding: 1%">
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
                                            <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Sno" HeaderText="Sr.No" />
                                            <asp:BoundField DataField="idno" HeaderText="Member ID" />
                                            <asp:BoundField DataField="memfirstname" HeaderText="Member Name" />
                                            <asp:BoundField DataField="sponsorid" HeaderText="Sponsor ID" />
                                            <asp:BoundField DataField="sponsorname" HeaderText="Sponsor Name" />
                                            <asp:BoundField DataField="RedistartionDate" HeaderText="Date" />
                                            <asp:BoundField DataField="ShopName" HeaderText="Vendor Name"></asp:BoundField>
                                            <asp:BoundField DataField="MobileNo" HeaderText="Mobile No"></asp:BoundField>
                                            <asp:BoundField DataField="CatName" HeaderText="Category Name" />
                                            <asp:BoundField DataField="SubCatName" HeaderText="Sub Category Category" />
                                            <asp:BoundField DataField="City" HeaderText="City"></asp:BoundField>
                                            <asp:BoundField DataField="statename" HeaderText="State" />
                                            <asp:BoundField DataField="PinCode" HeaderText="Pin Code" />
                                            <asp:BoundField DataField="rankid1" HeaderText="PARTNER" />
                                            <asp:BoundField DataField="rankid2" HeaderText="MASTER" />
                                            <asp:BoundField DataField="rankid3" HeaderText="AGENCY" />
                                            <asp:BoundField DataField="rankid4" HeaderText="AGENT" />
                                            <asp:BoundField DataField="rankid5" HeaderText="EMALL" />
                                            <asp:BoundField DataField="Cashback" HeaderText="Cashback" />
                                            <asp:BoundField DataField="Commission" HeaderText="Bonus" />
                                            <asp:BoundField DataField="Astatus" HeaderText="Vendor Status" />
                                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" />
                                            <asp:BoundField DataField="Astatus" HeaderText="Approve Status" />
                                            <asp:BoundField DataField="Approvedate" HeaderText="Approve Date" />
                                            <asp:TemplateField HeaderText="Approve" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                                ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <a href='<%# "VendorRegistration.aspx?catid=" & Eval("ID")  %>' style="color: Blue"
                                                        runat="server" visible='<%# Eval("EnableStatus") %>'>Approve
                                                        <asp:Label ID="LBModify" runat="server" />
                                                    </a>
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
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
