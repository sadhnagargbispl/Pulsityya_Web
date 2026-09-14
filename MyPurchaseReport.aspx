<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="MyPurchaseReport.aspx.vb" Inherits="MyPurchaseReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .PopCal
        {
            z-index: 100;
        }
    </style>

    <script type="text/javascript">
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="0">
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
                            My Purchase Report
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="Span1" class="text-danger"></span>
                            <div class="makeitresponsivegrid">
                                <div align="center">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <asp:CheckBox ID="CheckBox1" runat="server" Text="Member ID :" Font-Bold="true" Visible="false" />
                                            <asp:Label ID="Label4" runat="server" Text="Member ID  "></asp:Label>
                                            <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label6" runat="server" Text="From Date  "></asp:Label>
                                            <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtStartDate"
                                                Format="dd-MMM-yyyy">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtStartDate"
                                                ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label7" runat="server" Text="To Date  "></asp:Label>
                                            <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtEndDate"
                                                Format="dd-MMM-yyyy">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtEndDate"
                                                ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-3">
                                            <label>
                                                Page Size:</label>
                                            <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control" AutoPostBack="true">
                                                <asp:ListItem Text="10" Value="10" />
                                                <asp:ListItem Text="20" Value="20" />
                                                <asp:ListItem Text="50" Value="50" />
                                                <asp:ListItem Text="100" Value="100" />
                                                <asp:ListItem Text="500" Value="500" />
                                                <asp:ListItem Text="1000" Value="1000" />
                                                <asp:ListItem Text="2000" Value="2000" />
                                                <asp:ListItem Text="5000" Value="5000"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <label>
                                                By Select:</label>
                                            <asp:DropDownList ID="DDlBYSelectUser" runat="server" class="form-control" AutoPostBack="true">
                                                <asp:ListItem Value="Z" Selected="True" Text="All"></asp:ListItem>
                                                <asp:ListItem Value="C" Text="By User"></asp:ListItem>
                                                <asp:ListItem Value="A" Text="By Admin"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2" style="padding: 2%">
                                            <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Search" />
                                            <asp:Button ID="btnSendSms" runat="server" class="btn btn-primary" Text="Send Sms"
                                                Visible="false" />
                                            <asp:Button ID="BtnSendSmsToAll" runat="server" class="btn btn-primary" Text="Send Sms To All"
                                                Visible="false" />
                                        </div>
                                        <div class="col-md-2" style="padding: 2%">
                                            <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                        </div>
                                    </div>
                                    <asp:Label ID="LblSessionNo" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label></div>
                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px;
                                    color: Gray"></asp:Label>
                                <asp:Label ID="Label1" runat="server" Style="font-weight: bold; font-size: 12px;
                                    color: Gray"></asp:Label>
                                <asp:Label ID="lblinv" runat="server" Style="font-weight: bold; font-size: 12px;
                                    color: Gray"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                    AutoGenerateColumns="true" AllowSorting="true" ShowHeader="true" class="table table-bordered"
                                    HeaderStyle-CssClass="bg-primary" EmptyDataText="No data to display." GridLines="Both">
                                    <%--<PagerStyle HorizontalAlign = "Right" CssClass = "pagination-ys" />--%>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="http://code.jquery.com/jquery-latest.min.js" type="text/javascript"></script>

    <script src="highslide/script.js"></script>

    <script type="text/javascript" src="highslide/highslide-full.js"></script>

    <link rel="stylesheet" type="text/css" href="highslide/highslide.css" />
    <style type="text/css">
        .page
        {
            margin: 2%;
        }
    </style>

    <script type="text/javascript">
        hs.graphicsDir = 'highslide/graphics/';
        hs.align = 'center';
        hs.transitions = ['expand', 'crossfade'];
        hs.fadeInOut = true;
        hs.dimmingOpacity = 0.8;
        hs.outlineType = 'rounded-white';
        hs.marginTop = 60;
        hs.marginBottom = 40;
        hs.numberPosition = '';
        hs.wrapperClassName = 'custom';
        hs.width = 600;
        hs.height = 500;
        hs.number = 'Page %1 of %2';
        hs.captionOverlay.fade = 0;

        // Add the slideshow providing the controlbar and the thumbstrip

    </script>

</asp:Content>
