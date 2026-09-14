<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="AssignRankAllot.aspx.vb" Inherits="AssignRankAllot" Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Assign Rank</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-12">
                                    <asp:Label ID="LblCondition" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"
                                        Visible="false"></asp:Label>
                                    <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;">
                                    </span>
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-12" style="margin-bottom: 1%">
                                        <div class="col-md-2">
                                            Select Category :</div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="DdlCategory" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="* Select Category.!"
                                                ControlToValidate="DdlCategory" ValidationGroup="Save" InitialValue="---Select Category---"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        Member ID :</div>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="HdnFormno" runat="server" />
                                        <asp:TextBox ID="txtMemberID" runat="server" class="form-control" AutoPostBack="true">
                                        </asp:TextBox>
                                        <asp:Label ID="lblMemberName" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID.!"
                                            ControlToValidate="txtMemberID" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                    <br />
                                    <div class="col-md-2">
                                        Rank :</div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlRank" runat="server" class="form-control" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Select Rank.!"
                                            ControlToValidate="ddlRank" ValidationGroup="Save" InitialValue="---Select Rank---"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                    <br />
                                    <div class="col-md-12" style="margin-bottom: 1%">
                                        <div class="col-md-2">
                                            Remark :</div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtRemark" runat="server" class="form-control">
                                            </asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Remark.!"
                                                ControlToValidate="txtRemark" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                    <div class="col-md-12" style="margin-bottom: 1%">
                                        <div class="col-md-2">
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Button ID="BtnLegShift" runat="server" Text="Save" class="btn btn-primary" ValidationGroup="Save"
                                                Enabled="false" /></div>
                                        <div class="col-md-6">
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
