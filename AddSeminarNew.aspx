<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddSeminarNew.aspx.vb" Inherits="App_UI_Application_Pages_AddSeminarNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form2" runat="server">
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Seminar Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="table-responsive makeitresponsivegrid">
                                    <div align="center">
                                        <div class="col-md-12" style="padding: 1%">
                                            <div class="col-md-4">
                                                Seminar Name :</div>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtStateName" class="form-control" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtStateName"
                                                    runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                        <div class="col-md-12" style="padding: 1%">
                                            <div class="col-md-4">
                                                Image :</div>
                                            <div class="col-md-6">
                                                <asp:FileUpload runat="server" ID="FlUpld1" multiaccept="Image/*" />
                                                <asp:Label ID="LblPreImage" Text="Previous Image" runat="server" Visible="false"></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="FlUpld1"
                                                    runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                        <%-- <p style="color: #666666;line-height: 25px;"> Remarks : <br />
    <asp:TextBox CssClass="TxtBox" ID="txtRemarks" runat="server"></asp:TextBox>
    </p>--%>
                                        <div class="col-md-12" style="padding: 1%">
                                            <div class="col-md-4">
                                                Status :
                                            </div>
                                            <div class="col-md-6">
                                                <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="10"
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                                    <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                        <div class="col-md-12" style="padding: 1%">
                                            <div class="col-md-4">
                                                <asp:Button ID="BtnSave" CssClass="Btn btn-primary" runat="server" Text="Save" ValidationGroup="Save"
                                                    OnClientClick="this.disabled=true;" UseSubmitBehavior="false" /></div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtStateCOde" class="form-control" runat="server" Visible="false"></asp:TextBox></div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtActiveStatus" class="form-control" runat="server" Visible="false"></asp:TextBox></div>
                                        </div>
                                        <%-- <asp:TextBox  ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>--%>
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
