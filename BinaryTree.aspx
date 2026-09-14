<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="BinaryTree.aspx.vb" Inherits="BinaryTree" Title="Member Tree" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Genealogy</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div class="col-md-12">
                                <div class="col-md-1">
                                    &nbsp;Member&nbsp;ID</div>
                                <div class="col-md-3">
                                    <asp:TextBox class="form-control" ID="txtDownLineFormNo" MaxLength="15" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtDownLineFormNo"
                                        runat="server" ValidationGroup="Save">*
                                    </asp:RequiredFieldValidator></div>
                                <div class="col-md-2">
                                    Down Level</div>
                                <div class="col-md-2">
                                    <asp:TextBox class="form-control" ID="txtDeptlevel" MaxLength="4" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtDeptlevel"
                                        runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-primary" CausesValidation="true"
                                        ValidationGroup="Save" />
                                    <asp:Button ID="cmdBack" runat="server" Text="Back" class="btn btn-primary" CausesValidation="false"  Visible="false" />
                                    <asp:Button ID="BtnStepAbove" runat="server" Text="1 Step Above" class="btn btn-primary"
                                        CausesValidation="false" Visible="false" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <%--<iframe name="TreeFrame" frameborder="0" scrolling="Yes" src="Newtree.aspx" width="100%"
                                    height="800" id="TreeFrame" runat="server"></iframe>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <!-- end of weather widget -->
    </div>
</asp:Content>
