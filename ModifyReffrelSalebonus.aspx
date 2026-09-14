<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ModifyReffrelSalebonus.aspx.vb" Inherits="ModifyReffrelSalebonus" %>

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
                                    Referral Sales</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding: 1%">
                                        <div class="col-md-4">
                                            Caller Name :
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="TxtCallerName" class="form-control" runat="server"></asp:TextBox></div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="padding: 1%">
                                        <div class="col-md-4">
                                            Remark :  
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtRemark" class="form-control" runat="server"></asp:TextBox></div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                   <div class="col-md-12" style="padding-top :1%">
                                    <div class="col-md-4">
                                        Status :</div>
                                    <div class="col-md-6">
                                        <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="10"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="true" Text="Close" Value ="A">Close &nbsp;&nbsp;</asp:ListItem>
                                            <asp:ListItem Text="Panding" Value="P">Panding</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                  
                                     <div class="col-md-12" style="padding: 1%;display:none ">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="membername" class="form-control" runat="server"></asp:TextBox></div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                     <div class="col-md-12" style="padding: 1%;display:none ">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="idno" class="form-control" runat="server"></asp:TextBox></div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                     <div class="col-md-12" style="padding: 1%;display:none ">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="Date1" class="form-control" runat="server"></asp:TextBox></div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                     <div class="col-md-12" style="padding: 1%;display:none ">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="cname" class="form-control" runat="server"></asp:TextBox></div>
                                        <div class="col-md-2">
                                        </div>
                                        </div>
                                         <div class="col-md-12" style="padding: 1%;display:none ">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="mobile" class="form-control" runat="server"></asp:TextBox></div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                     <div class="col-md-12" style="padding: 1%;display:none ">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="hlissue" class="form-control" runat="server"></asp:TextBox></div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                     <div class="col-md-12" style="padding: 1%;display:none ">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="Status" class="form-control" runat="server"></asp:TextBox></div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                    
                                    </div>
                                  
                                    <div class="col-md-12" style="padding: 1%">
                                        <div class="col-md-4">
                                            <asp:Button ID="BtnUpdate" class="btn btn-primary" Text="Save" runat="server" />
                                        </div>
                                        <div class="col-md-8">
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
