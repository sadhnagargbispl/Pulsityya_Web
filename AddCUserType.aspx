<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCUserType.aspx.vb" Inherits="AddCUserType" %>

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
                                    Complaint Type Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 3%">
                                    <div class="col-md-4">
                                        Complaint Type:</div>
                                    <div class="col-md-6">
                                        <asp:TextBox CssClass="form-control" ID="txtCType" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtCType"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 3%">
                                    <div class="col-md-4">
                                        User Name :
                                    </div>
                                    <div class="col-md-6">
                                    <asp:GridView ID="GridView1" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
    AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox ID="chkRow" runat="server" />
                 <asp:Label ID="lblUserid" runat="server" Text='<%# Eval("Userid") %>' Visible="false"></asp:Label>
            
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="UserName" HeaderText="UserName" ItemStyle-Width="150" />
        
    </Columns>
</asp:GridView>

                                        </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 3%">
                                    <div class="col-md-4">
                                        Email Id :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox CssClass="form-control" ID="TxtEmail" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 3%">
                                    <div class="col-md-4">
                                        Remarks :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox CssClass="form-control" ID="txtRemarks" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 3%">
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
                             <div class="col-md-12" style="padding: 2%">
                                    <div class="col-md-4">
                                       <asp:Button ID="BtnSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" />
                                    <asp:TextBox ID="txtCTypeID" runat="server" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                               </div>
                                    <div class="col-md-8">
                                    </div>
                                </div>
                            </div>
    </form>
</body>
</html>
