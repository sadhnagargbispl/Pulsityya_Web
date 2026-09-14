<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="SuperStarPoint.aspx.vb" Inherits="SuperStarPoint" Title="" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Super Star Amount
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div align="center">
                            <div class="col-md-12">
                                <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" />
                                <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" />
                                <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                <a href="AddSuperStarPoint.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 470,marginTop : 0 } )">
                                    <asp:Button ID="BtnAddNew" runat="server" class="btn btn-primary" Text="Add New Amount" /></a>
                                <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                    Visible="false" />
                            </div>
                            <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" id="divContent"
                                runat="server" visible="false">
                                <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                                <br />
                                <br />
                                <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                            </div>
                            <div style="margin-bottom: 20px">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="false" class="table table-bordered" PagerStyle-CssClass="PagerStyle"
                                    AlternatingRowStyle-CssClass="alt" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                    PageSize="10" EmptyDataText="No data to display." AllowSorting="true" DataKeyNames="Sessid">
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="BId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("SID") %>'></asp:Label>
                                                <asp:Label ID="LblRankid" runat="server" Text='<%# Eval("RankID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="BankCode" HeaderText="Bank Code" Visible="false"/>--%>
                                        <asp:BoundField DataField="Date" HeaderText="Date" />
                                        <asp:BoundField DataField="amount" HeaderText="Amount" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remarks" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                   <%--      <asp:BoundField DataField="Rankid" HeaderText="Rankid" Visible ="false" />
                                        --%>
                                        <%--<asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn-group">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"
                                                    class="btn btn-danger"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="55px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
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
