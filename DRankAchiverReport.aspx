<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="DRankAchiverReport.aspx.vb" Inherits="DRankAchiverReport" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content2DT831731" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .PopCal
        {
            z-index: 100;
        }
    </style>
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
                         Diamond Rank Achiever Report
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
                                                <asp:Label ID="Label2" runat="server" Text="Rank "></asp:Label>
                                                <asp:DropDownList ID="ddlstate" runat="server" class="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="Label1" runat="server" Text="Member ID  "></asp:Label>
                                                <asp:TextBox ID="txtMemberID" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <%--<div class="col-md-2">
                                             <asp:DropDownList ID="DDlFromDate" runat="server" class="form-control" Style="display: inline">
                                        </asp:DropDownList>
                                               <%-- <asp:Label ID="Label3" runat="server" Text="From Date  "></asp:Label>
                                                <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                                <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                                    Format="dd-MMM-yyyy">
                                                </AjaxToolkit:CalendarExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                                    ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>--%>
                                            <%--</div>
                                            <div class="col-md-2">
                                             <asp:DropDownList ID="DDltodate" runat="server" class="form-control">
                                        </asp:DropDownList>
                                            </div>--%>
                                            <div class="col-md-2">
                                                <br />
                                                <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                                <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="col-md-12">
                                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
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
                                        </asp:GridView>
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
