<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="ListFloorMaster.aspx.vb" Inherits="ListFloorMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Floor Master</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div class="col-md-12">
                                <div class=" col-md-12">
                                    <div class="col-md-10">
                                        <a href="FloorTypeMaster.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 330,marginTop : 0 } )">
                                            <asp:Button ID="BtnAdd" runat="server" class="btn btn-primary" Text="Add New Floor" /></a>
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel"
                                            Enabled="false" />
                                        <asp:Button ID="BtnPrint" runat="server" class="btn btn-primary" Text="Print" Enabled="false" />
                                        <%--  <asp:Button ID="BtnExportPDF" runat="server" class="btn btn-primary" Text="Export To PDF" Enabled="false"/></div>
--%>
                                    </div>
                                    <div class="col-md-2">
                                        &nbsp;
                                    </div>
                                </div>
                            </div>
                            <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" visible="false">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                            </div>
                            <div class="col-md-12">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" PageSize="50" EmptyDataText="No data to display.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="GrpID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SNo.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FloorType" HeaderText="Floor Type" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:TemplateField HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn-group">
                                            <ItemTemplate>
                                                <a href='<%# "FloorTypeMaster.aspx?key=" & Crypto.Encrypt(Eval("ID"))  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 330,marginTop : 0 } )"><i class="fa fa-edit"
                                                    style="color: #337ab7; font-size: 20px; margin-right: 20px"></i>
                                                    <asp:Label ID="LBModify" runat="server" /></a>
                                                <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="85px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <!-- end of weather widget -->
        </div>
    </div>
</asp:Content>
