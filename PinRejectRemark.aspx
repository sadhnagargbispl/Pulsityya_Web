<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PinRejectRemark.aspx.vb"
    Inherits="PinRejectRemark" Title="" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <!-- bootstrap theme -->
    <link href="css/bootstrap-theme.css" rel="stylesheet">
    <!--external css-->
    <!-- font icon -->
    <link href="css/Grid.css" rel="Stylesheet" type="text/css" />
    <link href="css/elegant-icons-style.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <!-- Custom styles -->
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style-responsive.css" rel="stylesheet" />
    <style type="text/css">
        p
        {
            font-weight: bold;
            color: #666666;
            margin: 0px;
            line-height: 25px;
            width: 400px;
            padding-bottom: 8px;
            text-align: left;
        }
    </style>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Pin Detail</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 2%">
                                    <div class="col-md-3">
                                        <asp:Label ID="LblNo" runat="server" ForeColor="Black" Font-Size="14px"></asp:Label>
                                    </div>
                                    <div class="col-md-9">
                                    </div>
                                </div>
                                <div style="margin-bottom: 20px">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="20" EmptyDataText="No data to display.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Req.No." Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblReqNo" runat="server" Text='<%# Eval("ReqNo") %>'></asp:Label>
                                                    <asp:Label ID="lblFormno" runat="server" Text='<%# Eval("Formno") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="BankCode" HeaderText="Bank Code" Visible="false"/>--%>
                                            <asp:BoundField DataField="IDNo" HeaderText="ID No." />
                                            <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                                            <asp:BoundField DataField="ReqNo" HeaderText="Req.No" />
                                            <asp:BoundField DataField="KitName" HeaderText="Kit Name" />
                                            <asp:BoundField DataField="Qty" HeaderText="Req.Qty" />
                                            <asp:BoundField DataField="DispQty" HeaderText="Sent Qty" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        Remark:</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtRemark" runat="server" class="form-control"></asp:TextBox></div>
                                    <div class="col-md-2">
                                        <asp:Button ID="BtnReject" runat="server" class="btn btn-primary" Text="Reject" /></div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
