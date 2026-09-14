<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="UserIdUpdate.aspx.vb" Inherits="App_UI_Application_Pages_UserIdUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

    <script type="text/javascript">
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
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
                            Update Member User Id
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div class="table-responsive makeitresponsivegrid">
                                    <div align="center">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"
                                                Visible="false"></asp:Label>
                                            <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;">
                                            </span>
                                        </div>
                                        <div class="col-md-12" style="margin-bottom: 1%">
                                            <div class="col-md-2">
                                                Old User ID :</div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true">
                                                </asp:TextBox>
                                                <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                                <asp:HiddenField ID="hdnFormno" runat="server" />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Old User ID."
                                                    ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="TxtFormNo" runat="server" class="form-control" Visible="false"></asp:TextBox></div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                        <div class="col-md-12" style="margin-bottom: 1%">
                                            <div class="col-md-2">
                                                <strong>New User ID:</strong></div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtNewUserID" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter New User ID."
                                                    ControlToValidate="txtNewUserID" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                        <div class="col-md-12" style="margin-bottom: 1%">
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Button ID="BtnLegShift" runat="server" Text="Update" OnClientClick="return confirmation();"
                                                    class="btn btn-primary" ValidationGroup="Save" /></div>
                                            <div class="col-md-6">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtNewUserID" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
