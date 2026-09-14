<%--<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="GoldProductReport.aspx.vb" Inherits="GoldProductReport" %>--%>
<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="GoldProductReport.aspx.vb" Inherits="GoldProductReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content2DT831731" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .PopCal
        {
            z-index: 100;
        }
    </style>
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
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                        <% If Session("Compid") = "1056" Then %>
                          Orbit Rank Achiever Report
                        <% Else%>
                         Gold Product Report
                        <% End If%>
                          
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <div class="table-responsive makeitresponsivegrid" style="min-height: 500px;">
                                <div align="center">
                                    <div class="col-md-12">
                                        <div class="row">
                                           
                                            <div class="col-md-2">
                                                <asp:Label ID="Label1" runat="server" Text="Member ID  "></asp:Label>
                                                <asp:TextBox ID="txtMemberID" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="Label3" runat="server" Text="From Date  "></asp:Label>
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
                                             <asp:Label ID="Label4" runat="server" Text="To Date  "></asp:Label>
                                                <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                                <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                                    Format="dd-MMM-yyyy">
                                                </AjaxToolkit:CalendarExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                                    ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                            </div>
                                            <div class="col-md-2"><br />
                                            <asp:RadioButtonList ID="RbReqStatus" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Table">
                                            <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="UnPaid" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="Paid" Value="Y"></asp:ListItem>
                                           </asp:RadioButtonList>
                                         </div>
                                             <div class="col-md-2" style="display:none">
                                                <asp:Label ID="Label2" runat="server" Text="Gold "></asp:Label>
                                                <asp:DropDownList ID="ddlstate" runat="server" class="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <br />
                                                <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                                <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                                <asp:Button ID="btnPaid" runat="server" class="btn btn-primary" Text="Paid" Visible ="false" Enabled ="true" />
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div id="DivRemark" runat="server" visible="false">
                                    <table id="TblRemark" runat="server" align="center" style="background-color: #13a89e;
                                        color: #ffffff; border-color: Black; border-width: 1px; margin-top: -10px;">
                                        <tr>
                                            <td align="left">
                                                <strong>Remark</strong>*
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtARemark" runat="server" TextMode="MultiLine" Style="color: Black"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPaids" runat="server" class="btn btn-primary" Text="Paid"
                                                    Visible="false" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" />
                                                <asp:Button ID="BtnReject" runat="server" Text="Reject " class="btn btn-primary"
                                                    Visible="false" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                
                                  <asp:Label ID="lblMsg" runat="server" Font-Size="13px" Visible="False"></asp:Label>
                                    <div class="col-md-12">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                            GridLines="None" AllowPaging="True" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" PageSize="50" EmptyDataText="No data to display." Width="100%">
                                            <Columns>
                                                <%--<asp:TemplateField HeaderText="CheckAll">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# Eval("EnableStatus") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                                                        <asp:Label ID="LblReqNo" runat="server" Text='<%# Eval("Gid") %>'></asp:Label>
                                                        <asp:Label ID="LblIdNo" runat="server" Text='<%# Eval("GoldIdno") %>'></asp:Label>
                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="GoldIdno" HeaderText="Member IDNo." />
                                                <asp:BoundField DataField="GoldMemberName" HeaderText="Member Name" />
                                                <asp:BoundField DataField="MemFirstName" HeaderText="Sponsor Name" />
                                                <asp:BoundField DataField="idno" HeaderText="Sponsor IDNo"></asp:BoundField>
                                                <asp:BoundField DataField="GoldName" HeaderText="Gold Product Code"></asp:BoundField>
                                                <asp:BoundField DataField="KitName" HeaderText="Package Name"></asp:BoundField>
                                                <asp:BoundField DataField="PurchaseDate" HeaderText="Purchase Date"></asp:BoundField>
                                                <asp:BoundField DataField="PaidDate" HeaderText="Paid Date"></asp:BoundField>
                                                <asp:BoundField DataField="Remarks" HeaderText="Member Remark"></asp:BoundField>
                                               <%--<asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a href='<%# "Img.aspx?&type=Payment&ID=" & Eval("Reqno") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 785,height: 580,marginTop : 0 } )">
                                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ScannedFile") %>' Height="80px"
                                                                Width="80px" Visible='<%# Eval("ScannedFileStatus") %>' />
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                        <%--<asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                            AutoGenerateColumns="true" ForeColor="Black" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            EmptyDataText="No data to display." GridLines="None">
                                            <Columns>
                                            
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                             
                                            <PagerStyle HorizontalAlign = "Right" CssClass = "pagination-ys" />
                                        </asp:GridView>--%>
                                       
                                    </div>
                                </div>
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
