<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="UserPermission.aspx.vb" Inherits="App_UI_Application_Pages_UserPermission"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .parent-row {
            background-color: #e9f2fb !important;
            font-weight: bold;
        }
        .child-row td:nth-child(3) {
            padding-left: 30px;
        }
        .PagerStyle { text-align: center; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Set User Permission</h2>
                        <div class="clearfix"></div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <asp:Label ID="lblText" runat="server" Text="Select Group : " Font-Bold="True"></asp:Label>&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlGroup" runat="server" class="form-control" AutoPostBack="false" style="display:inline; width:55%">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnShow" runat="server" class="btn btn-primary" Text="Show" />
                                    <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="Save" />
                                </div>
                                <div class="col-md-6"></div>
                            </div>
                            <div align="center">
                                <asp:Label ID="lblMsg" runat="server" Font-Size="13px" Visible="False"></asp:Label>
                            </div>
                            <div class="col-md-12">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                    GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" EmptyDataText="No data to display."
                                    OnRowDataBound="GvData_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="MenuId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMenuId" runat="server" Text='<%# Eval("MenuId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="70px">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkMenuPermission" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MenuName" HeaderText="Menu" />
                                        <asp:TemplateField HeaderText="ParentId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParentId" runat="server" Text='<%# Eval("ParentId") %>'></asp:Label>
                                            </ItemTemplate>
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
    </div>
</asp:Content>