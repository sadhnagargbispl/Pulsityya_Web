<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RedeemVoucherDetails.aspx.vb"
    Inherits="RedeemVoucherDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Creative - Bootstrap 3 Responsive Admin Template">
    <meta name="author" content="GeeksLabs">
    <meta name="keyword" content="Creative, Dashboard, Admin, Template, Theme, Bootstrap, Responsive, Retina, Minimal">
    <link rel="shortcut icon" href="img/favicon.png">
    <title>Welcome to
        <%=Session("CompName")%></title>
    <!-- Bootstrap CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/font-awesome.min.css" rel="stylesheet">
    <link href="css/custom.min.css" rel="stylesheet">
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 -->
    <!--[if lt IE 9]>
      <script src="js/html5shiv.js"></script>
      <script src="js/respond.min.js"></script>
      <script src="js/lte-ie7.js"></script>
    <![endif]-->

    <script type="text/javascript" src="highslide/highslide-full.js"></script>

    <link rel="stylesheet" type="text/css" href="highslide/highslide.css" />

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

    <style type="text/css">
        .rptclass
        {
            color: #337ab7;
        }
    </style>
    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-family: Arial;
        }
        .modal1
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        .center1
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center1 img
        {
            height: 128px;
            width: 128px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>
                    Redeem Voucher Detail
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <div class="clearfix">
                    </div>
                    <br>
                    <div class="centered">
                        <div class="clr">
                            <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                        </div>
                        <div class="clr">
                        </div>
                        <div class="form-horizontal">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                   <%-- <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>--%>
                                            <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                                AutoGenerateColumns="False" ForeColor="Black" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                EmptyDataText="No data to display." GridLines="None">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SNo.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Member ID" SortExpression="IdNo" HeaderStyle-ForeColor="White">
                                                        <ItemTemplate>
                                                            <asp:Label ID="MemberID" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name" SortExpression="MemFirstName" HeaderStyle-ForeColor="White">
                                                        <ItemTemplate>
                                                            <%#Eval("MemFirstName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Add Date" SortExpression="AddDate" HeaderStyle-ForeColor="White">
                                                        <ItemTemplate>
                                                            <asp:Label ID="AddDate" runat="server" Text='<%# Eval("AddDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" SortExpression="Status" HeaderStyle-ForeColor="White">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Status" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="UseType" SortExpression="Use Type" HeaderStyle-ForeColor="White">
                                                        <ItemTemplate>
                                                            <asp:Label ID="UseType" runat="server" Text='<%# Eval("UseType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="UseDate" SortExpression="UseDate" HeaderStyle-ForeColor="White">
                                                        <ItemTemplate>
                                                            <asp:Label ID="UseDate" runat="server" Text='<%# Eval("UseDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                       <%-- </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <%--    <script type="text/javascript" src="assets/datatable.css"></script>--%>

    <script type="text/javascript" src="assets/jquery.dataTables.min.js"></script>

    <%--  <script type="text/javascript" src="js/plugins/datatables/jquery.dataTables.min.js"></script>--%>

    <script type="text/javascript" src="assets/tableExport.js"></script>

    <script type="text/javascript">
        var jq = $.noConflict();
        function pageLoad(sender, args) {
            debugger;

            jq(document).ready(function() {
                jq('#customers2').DataTable();

            });
        }


    </script>

    </form>
</body>
</html>
