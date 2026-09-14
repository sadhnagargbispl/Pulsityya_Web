<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="ComplaintType.aspx.vb" Inherits="App_UI_Application_Pages_ComplaintType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Complaint Type Master</h2>
                        <div class="clearfix">
                        </div>
                        <div class="panel-body">
                            <div align="center">
                                <span id="lblt" class="text-danger"></span>
                            </div>
                            <div class="table-responsive makeitresponsivegrid">
                                <div align="center">
                                    <div class="col-md-12">
                                        <div class="col-md-6" style="display: flex; padding: 1%">
                                            <asp:DropDownList ID="ddlGroupFields" runat="server" class="form-control" Style="width: 28%;
                                                padding: 0px; border-radius: 5px; display: inline">
                                                <asp:ListItem Value="None" Selected="True">--Search By--</asp:ListItem>
                                                <asp:ListItem Value="CType">Complaint Type</asp:ListItem>
                                                <asp:ListItem>Remarks</asp:ListItem>
                                                <asp:ListItem>Status</asp:ListItem>
                                                <asp:ListItem>ShowAll</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtSearch" runat="server" Style="width: 28%; margin-right: 1%; padding: 0px;
                                                border-radius: 5px; display: inline; margin-left: 1%"></asp:TextBox>
                                            <asp:ImageButton ID="imgSearch" runat="server" CssClass="imgSearchButton" ImageUrl="Images/search.png"
                                                Width="32px" Height="32px" /></div>
                                        <div class="col-md-6">
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                    <a href="AddCUserType.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 350,marginTop : 0 } )">
                                        <asp:Button ID="BtnAdd" runat="server" class="btn btn-primary" Text="Add Complaint Type" /></a>
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" />
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" />
                                    <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                        Visible="false" />
                                </div>
                                <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;">
                                    <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."
                                        Visible="false"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                                </div>
                                <div style="margin-bottom: 20px">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="None" AllowPaging="true" CssClass="table table-striped table-advance table-hover"
                                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                        PageSize="20" EmptyDataText="No data to display.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="GrpID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("CTypeID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CType" HeaderText="Group Name" ControlStyle-Width="50px" />
                                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <%--<asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass ="btn-group">
<ItemTemplate>
<a class="btn btn-primary" href='<%# "AddCType.aspx?Type=" & Crypto.Encrypt(Eval("VCTypeID"))  %>'
      onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )"><i class="icon_plus_alt2"></i>
                            <asp:Label ID="LBModify" runat="server" Text="Modify" />
</a>
</ItemTemplate>

<HeaderStyle Width="55px"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" >
<ItemTemplate>
<asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup" class="btn btn-danger"><i class="icon_close_alt2"></i></asp:LinkButton>
</ItemTemplate>
<HeaderStyle Width="55px"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <a href='<%# "AddCType.aspx?Type=" & Crypto.Encrypt(Eval("VCTypeID"))  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 350,marginTop : 0 } )">
                                                        <i class=" fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px">
                                                        </i>
                                                        <asp:Label ID="LBModify" runat="server" />
                                                        <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                                    </a>
                                                </ItemTemplate>
                                                <HeaderStyle Width="85px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
                                <br />
                                <br />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
