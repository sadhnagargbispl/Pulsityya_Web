<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="MFA_Report.aspx.vb" Inherits="MFA_Report_123" %>

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
                            MFA Report
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <div class="table-responsive makeitresponsivegrid">
                                <div class="col-md-2">
                                    Member ID
                                    <asp:TextBox ID="txtMemberID" runat="server" class="form-control">
                                    </asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                <br />
                                     <asp:Button ID="btnShow" runat="server" Text="Show" class="btn btn-primary"  />
                                </div>
                                
                                 
                                <div align="center">
                                    <div class="col-md-12">
                                    <br />
                                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                            AutoGenerateColumns="true" ForeColor="Black" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            EmptyDataText="No data to display." GridLines="None">
                                            <PagerStyle CssClass="pagination-ys" />
                                        </asp:GridView>
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
