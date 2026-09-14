<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddMembership.aspx.vb" Inherits="App_UI_Application_Pages_AddMembership" %>

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
                                    Membership Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
    <div class="text-center mb-3">
        <span id="lblt" class="text-danger"></span>
    </div>

    <div class="row mb-3" style="padding-top: 1%">
        <div class="col-md-4 text-end">
            Person Count :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlPersoncount" runat="server" CssClass="form-control" Width="180px"></asp:DropDownList>
        </div>
    </div>

    <div class="row mb-3" style="padding-top: 1%">
        <div class="col-md-4 text-end">
            Year Duration :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlyear" runat="server" CssClass="form-control" Width="180px"></asp:DropDownList>
        </div>
    </div>

    <div class="row mb-3" style="padding-top: 1%">
        <div class="col-md-4 text-end">
            MRP :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  
        </div>
        <div class="col-md-6">
            <asp:TextBox ID="txtmrp" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
        </div>
    </div>

    <div class="row mb-3" style="padding-top: 1%">
        <div class="col-md-4 text-end">
            Discount Percent :
        </div>
        <div class="col-md-6">
            <asp:TextBox ID="txtdiscount" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event);" AutoPostBack="true"></asp:TextBox>
        </div>
    </div>

    <div class="row mb-3" style="padding-top: 1%">
        <div class="col-md-4 text-end">
            Discount Amount :
        </div>
        <div class="col-md-6">
            <asp:TextBox ID="txtdiscountamt" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>
    </div>

    <div class="row mb-3" style="padding-top: 1%">
        <div class="col-md-4 text-end">
            Offer Price :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-md-6">
            <asp:TextBox ID="txtofferprice" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>
    </div>

    <div class="row mt-4">
        <div class="col-md-10 text-end">
            <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="Save" />
            <asp:TextBox ID="txtKitId" runat="server" Visible="false"></asp:TextBox>
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
