<%--<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddSubCat.aspx.vb" Inherits="AddSubCat" %>--%>

<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="AddSubCat.aspx.vb" Inherits="AddSubCat" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    
    // Allow numbers (48-57) and a single dot (46) for decimal values
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 46)
        return false;

    return true;
}

        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
    </script>

    <style type="text/css">
        .col-md-12
        {
            margin-bottom: 10px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                            Sub Category Master</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <%-- Available Balance:--%><span class="red" id="AvailableBal" style="color: Red"
                                runat="server"></span>
                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Select Category :</div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlcountry" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="ddlcountry"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Sub Category Name :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtCatName" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtCatName"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        PARTNER :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtPARTNER" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        MASTER :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMASTER" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        AGENCY :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtAGENCY" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        AGENT :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtAGENT" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12" style="display: none;">
                                    <div class="col-md-4">
                                        EMALL :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtEMALL" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        Cashback (%) :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtCashback" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        Bonus (%) :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TXtBonus" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12"  style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Remarks :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtRemarks" runat="server" ></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Status :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                    <div class="col-md-4">
                                        <asp:RadioButtonList ID="rdblist" runat="server" Width="150px" CellPadding="0" CellSpacing="10"
                                            RepeatDirection="Horizontal" >
                                            <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                            <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCatId" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
                                    Sub Category Master</h2>
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
                                        Select Category :</div>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddlcountry" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="ddlcountry"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Sub Category Name :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtCatName" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtCatName"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        PARTNER :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtPARTNER" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        MASTER :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMASTER" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        AGENCY :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtAGENCY" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        AGENT :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtAGENT" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12" style="display: none;">
                                    <div class="col-md-4">
                                        EMALL :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtEMALL" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        Cashback (%) :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtCashback" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        Bonus (%) :
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TXtBonus" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
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
                                    <div class="col-md-6">
                                        <br />
                                        <br />
                                        <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" />
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
--%>