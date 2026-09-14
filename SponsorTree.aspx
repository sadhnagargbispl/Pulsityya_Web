<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="SponsorTree.aspx.vb" Inherits="SponsorTree" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Sponsor Tree</h2>
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
                                    <input class="form-control" id="DownLineFormNo" runat="server" type="text" maxlength="15"
                                        name="DownLineFormNo" runat="server" /></div>
                                <div class="col-md-2" style="display: none;">
                                    Down Level</div>
                                <div class="col-md-3" style="display: none;">
                                    <input class="form-control" id="deptlevel" type="text" maxlength="4" name="deptlevel" />
                                </div>
                                <div class="col-md-3">
                                    <asp:Button ID="Button1" runat="server" Text="Search" class="btn btn-primary" />
                                    <asp:Button ID="cmdBack" runat="server" Text="Back" class="btn btn-primary" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <%--<iframe name="TreeFrame" frameborder="0" scrolling="auto" src="Referaltree.aspx" width="100%" height="430"
												id="TreeFrame" runat="server"></iframe>--%>
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
