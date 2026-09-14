<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="FloorTypeMaster.aspx.vb"
    Inherits="FloorTypeMaster" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                                    Floor Type Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="row">
                                    <div class="col-md-12" style="padding-top :3%">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>
                                                    Floor Type</label>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="LblId" runat="server" Visible="false"></asp:Label>
                                            <asp:TextBox ID="TxtFlatType" runat="server" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                        <div class ="col-md-2"></div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12" style="padding-top :3%">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>
                                                    Active Status
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                                                <asp:ListItem Value="N">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class ="col-md-2"></div>
                                    </div>
                                </div>
                                <div class="row">
                                 <div class="col-md-12" style="padding-top :3%">
                                    <div class="col-md-8">
                                        <div class="form-group">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" />
                                            <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" />
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
