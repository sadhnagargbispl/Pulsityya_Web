<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddOfferImage.aspx.vb" Inherits="AddOfferImage" %>

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
                                    Add Offer</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="table-responsive makeitresponsivegrid">
                                    <div align="center">
                                        <div class="clearfix"></div>
                                        <div class="col-md-12" id="PimagePath" runat="server" style="padding: 1%">
                                            <div class="col-md-4">
                                                <asp:FileUpload ID="ImageUpload" runat="server" Style="height: 22px" />
                                                <asp:CustomValidator ID="CustomValidator1" OnServerValidate="ValidateFileSize" ForeColor="Red"
                                                    runat="server" />
                                            </div>
                                            <div class="col-md-4" id="PVideo" runat="server" visible="false">
                                              
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                    
                                    <div class="clearfix"></div>
                                    <br />
                                   
                                    <div class="col-md-12" style="padding: 1%">
                                        <div class="col-md-4">
                                            Status:
                                        </div>
                                        <div class="col-md-6">
                                            <asp:RadioButtonList ID="RbtStatus" runat="server" RepeatColumns ="2" RepeatDirection ="Horizontal" >
                                                <asp:ListItem Text="Active" Value="Y" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
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
