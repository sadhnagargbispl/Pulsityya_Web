<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="NewFranchise.aspx.vb" Inherits="NewFranchise" %>

<asp:Content ID="Content2DT831731" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                            Make Franchise
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <div class="table-responsive makeitresponsivegrid">
                                <div align="center">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="form-group">
                                                        <div class="col-md-2">
                                                            Enter Member ID :</div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="txtMemberId" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnFormno" runat="server" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId"
                                                                runat="server" ValidationGroup="Save"> Please  Enter Member ID.!! </asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="form-group">
                                                        <div class="col-md-2">
                                                            Name :</div>
                                                        <div class="col-md-3">
                                                            <asp:TexTBox ID="lblMemberName" runat="server" Text=""  class="form-control" ReadOnly ="true"></asp:TexTBox> 
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="form-group">
                                                        <div class="col-md-2">
                                                            Remark :</div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="TxtRemark" runat="server" class="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="TxtRemark"
                                                                runat="server" ValidationGroup="Save"> Please  Enter Remark.!! </asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-2">
                                                        &nbsp;</div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <asp:Button ID="btnSubmit" runat="server" Text="Save" class="btn btn-primary" ValidationGroup="Save"  />
                                                            <asp:Button ID="btncencal" runat="server" Text="Cancel" class="btn btn-danger" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtMemberId" EventName="TextChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
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
