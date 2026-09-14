<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="MemberCountLevelWise.aspx.vb" Inherits="MemberCountLevelWise" %>

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
                            Member Count Level Wise
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
                                            <div class="col-md-3">
                                                <asp:Label ID="Label2" runat="server" Text="Member ID "></asp:Label>
                                                <asp:TextBox ID="txtMemberID" runat="server" class="form-control">
                                                </asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <br />
                                                <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="col-md-12">
                                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                            AutoGenerateColumns="False" ForeColor="Black" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            EmptyDataText="No data to display." GridLines="None">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Member ID" DataField="Idno" />
                                                <asp:BoundField HeaderText="Member Name" DataField="MemFirstName" />
                                                <asp:BoundField HeaderText="Level" DataField="MLevel" />
                                                <asp:BoundField HeaderText="Active Member" DataField="ActCnt" />
                                                <asp:BoundField HeaderText="De-Active Member" DataField="DActCnt" />
                                                  <asp:BoundField HeaderText="Total Member" DataField="TotalMember" />
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
