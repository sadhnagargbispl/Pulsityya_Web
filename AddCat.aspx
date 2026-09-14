<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCat.aspx.vb" Inherits="App_UI_Application_Pages_AddCat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
    <title></title>
    <link href="css/Main.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

    </script>

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
                                    Category Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Category Name :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtCatName" runat="server"></asp:TextBox>
                                        <%--   <asp:TextBox class="form-control" ID="txtkitName" runat="server" Style="margin-left: 27%"></asp:TextBox>--%>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtCatName"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Remarks :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtRemarks" runat="server" Style="margin-left: 24%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Status :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                    <div class="col-md-6">
                                        <asp:RadioButtonList ID="rdblist" runat="server" Width="150px" CellPadding="0" CellSpacing="10"
                                            RepeatDirection="Horizontal" Style="margin-left: 16%">
                                            <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                            <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCatId" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6"><br /><br />
                                        <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save"
                                            />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--<div style="height:270px;padding-left:10px">
    <h5 style="border-bottom:dashed 1px #666666;margin-bottom:11px;margin-top:8px" >Category Master</h5>
   <p style="color: #666666;line-height: 25px;"> Category Name : <br />
    <asp:TextBox CssClass="TxtBox" ID="txtCatName" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtCatName" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    </p> 
    <p style="color: #666666;line-height: 25px;"> Remarks : <br />
    <asp:TextBox CssClass="TxtBox" ID="txtRemarks" runat="server"></asp:TextBox>
    </p>
    <p style="color: #666666;line-height: 25px;"> Status : <br />
    <asp:RadioButtonList id="rdblist" runat="server" CellPadding="0" CellSpacing="10" 
            RepeatDirection="Horizontal">
    <asp:ListItem selected="true" Text="Active">Active</asp:ListItem>
    <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
    </asp:RadioButtonList>
    </p>
    <p align="left">
    <asp:Button ID="BtnSave" CssClass="Btn"  runat="server" Text="Save" ValidationGroup="Save" />
    <asp:TextBox  ID="txtCatId" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox  ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox  ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
    </p>
    </div>--%>
    </form>
</body>
</html>
