<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="VendorRegistration.aspx.vb" Inherits="VendorRegistration" Title="" %>

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
                            Vendor Registration</h2>
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
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="LblCondition" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Sponsor ID :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtsponsorid" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                                <asp:Label ID="lblsponsorname" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="lblrankid" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblvendorid" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblsponsorformno" runat="server" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Member ID :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="LblKitId" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblNewKitid" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblFormno" runat="server" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Member Name :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="LblMemName" runat="server" class="form-control" Style="text-align: left"></asp:Label></div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Mobile No. :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="LblMobileNo" runat="server" class="form-control" Style="text-align: left"></asp:Label>
                                                <%-- <asp:TextBox ID="LblMobileNo" runat="server" class="form-control" MaxLength="10"
                                                    onkeypress="return isNumberKey(event);"></asp:TextBox>--%>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Select Category :</div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DDlCategory" runat="server" class="form-control" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Select Sub Category :</div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DDlSubCaegory" runat="server" class="form-control" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Shop Name :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtshopName" runat="server" class="form-control"></asp:TextBox>
                                                <%-- <asp:Label ID="TxtSponsorid" runat="server" class="form-control" Style="text-align: left"></asp:Label>--%>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                City :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtCity" runat="server" class="form-control"></asp:TextBox>
                                                <%--   <asp:Label ID="TxtSponsorName" runat="server" class="form-control" Style="text-align: left"></asp:Label>--%>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                State :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DDlState" runat="server" class="form-control" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                PinCode :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtPicCode" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                                <%-- <asp:Label ID="LblPincode" runat="server" class="form-control" Style="text-align: left"></asp:Label>--%>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                PARTNER :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtPARTNER" runat="server" class="form-control" onkeypress="return isNumberKey(event);"
                                                    Enabled="false"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                MASTER :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtMASTER" runat="server" class="form-control" onkeypress="return isNumberKey(event);" Enabled="false"></asp:TextBox>
                                                <%--<asp:Label ID="LblBonus" runat="server" class="form-control" Style="text-align: left"></asp:Label>--%>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                AGENCY :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtAGENCY" runat="server" class="form-control" onkeypress="return isNumberKey(event);" Enabled="false"></asp:TextBox>
                                                <%--<asp:Label ID="LblBonus" runat="server" class="form-control" Style="text-align: left"></asp:Label>--%>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                AGENT :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtAGENT" runat="server" class="form-control" onkeypress="return isNumberKey(event);" Enabled="false"></asp:TextBox>
                                                <%--<asp:Label ID="LblBonus" runat="server" class="form-control" Style="text-align: left"></asp:Label>--%>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12" style="display: none;">
                                            <div class="col-md-4">
                                                EMALL :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtEMALL" runat="server" class="form-control" onkeypress="return isNumberKey(event);" Enabled="false"></asp:TextBox>
                                                <%--<asp:Label ID="LblBonus" runat="server" class="form-control" Style="text-align: left"></asp:Label>--%>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Cashback (%) :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtCashback" runat="server" class="form-control" onkeypress="return isNumberKey(event);" Enabled="false"></asp:TextBox>
                                                <%--<asp:Label ID="LblCashback" runat="server" class="form-control" Style="text-align: left"></asp:Label>--%>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Bonus (%) :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TXtBonus" runat="server" class="form-control" onkeypress="return isNumberKey(event);" Enabled="false"></asp:TextBox>
                                                <%--<asp:Label ID="LblCashback" runat="server" class="form-control" Style="text-align: left"></asp:Label>--%>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12" id="TrDilivery" runat="server" visible="false">
                                            <div class="col-md-4">
                                                Status :</div>
                                            <div class="col-md-4">
                                                <asp:RadioButtonList ID="RbtDelivery" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow">
                                                    <asp:ListItem Text="Active" Value="Y" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="De-Active" Value="N"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-12">
                                                    <div class="col-md-4">
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Button ID="BtnUpgrade" runat="server" Text="Submit" class="btn btn-primary"
                                                            ValidationGroup="Save" />
                                                        <%-- <asp:Button ID="BtnCancel" class="btn btn-primary" runat="server" Text="Cancel" />--%>
                                                    </div>
                                                    <div class="col-md-4">
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
