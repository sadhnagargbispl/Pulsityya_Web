<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="SocialActivation.aspx.vb" Inherits="SocialActivation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function isNumberMonth() {

            var res = document.getElementById("ctl00_ContentPlaceHolder1_TxtMonth").value;

            if (res < 1 || res > 60)
                alert("Enter Month Between 1 to 60 ");
        }
        function validfloat(txt, ev) {
            ev.returnValue = ((ev.keyCode >= 48 && ev.keyCode <= 57) || (ev.keyCode == 46 && txt.value.indexOf('.') == -1));
        }
    </script>

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
                            Social Activation</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        Member Id:</div>
                                    <div class="col-md-4">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true"
                                                    ></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                                    ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-5">
                                    </div>
                                </div>
                                
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-3">
                                                Member Name:</div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtName" runat="server" class="form-control" Enabled="false"></asp:TextBox>
                                                <asp:TextBox ID="TxtFormNo" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                            </div>
                                            <div class="col-md-5">
                                            </div>
                                        </div>
                                        
                                <br />
                                <br />
                                        <div class="col-md-12">
                                        <br />
                                            <div class="col-md-3">
                                                Sponsor Id:</div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtSponsor"  runat="server" class="form-control" Enabled="false"></asp:TextBox>
                                                <asp:TextBox ID="TxtRefFormno" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                            </div>
                                            <div class="col-md-5">
                                            </div>
                                        </div>
                                        
                                <br />
                                <br />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <div class="col-md-12">
                                <br />
                                    <div class="col-md-3">
                                        Package :</div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="DDlPackage" runat="server" class="form-control" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-5">
                                    </div>
                                </div>
                                <br />
                                <br />
                                <div class="col-md-12">
                                <br />
                                    <div class="col-md-3">
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Button ID="BtnFundTransfer" runat="server" Text="Submit" 
                                            class="btn btn-primary" ValidationGroup="Save" />
                                        <asp:Button ID="BtnCancel" runat="server" Text="Cancel" class="btn btn-primary" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label></div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="BtnFundTransfer" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <!-- end of weather widget -->
    </div>
</asp:Content>
