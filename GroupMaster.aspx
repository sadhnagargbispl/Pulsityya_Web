<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="GroupMaster.aspx.vb" Inherits="App_UI_Application_Pages_GroupMaster"
    Title="User Group Master" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            New Department</h2>
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
                                    <div class="col-md-6" style="display: flex; padding: 1%">
                                        <asp:DropDownList ID="ddlGroupFields" runat="server" class="form-control" Style="width: 28%;
                                            padding: 0px; border-radius: 5px; display: inline">
                                            <asp:ListItem Selected="True" Value="showall">--Search By--</asp:ListItem>
                                           <%-- <% If Session("CompID") = "1066" Then%>--%>
                                            <asp:ListItem Value="GroupName">Department Name</asp:ListItem>
                                           <%-- <% Else%>
                                             <asp:ListItem Value="GroupName">Group Name</asp:ListItem>
                                            <% End If%>--%>
                                            <asp:ListItem>Remarks</asp:ListItem>
                                            <asp:ListItem>Status</asp:ListItem>
                                            <asp:ListItem>ShowAll</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtSearch" class="form-control" runat="server" Style="width: 28%;
                                            margin-right: 1%; padding: 0px; border-radius: 5px; display: inline; margin-left: 1%"></asp:TextBox>
                                        <asp:ImageButton ID="imgSearch" runat="server" CssClass="imgSearchButton" ImageUrl="Images/search.png"
                                            Width="32px" Height="32px" />
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=" col-md-12">
                            <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                            <a href="AddGroup.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 330,marginTop : 0 } )">
                                <%--<asp:Button ID="BtnAdd" runat="server" class="btn btn-primary" Text="Add Group" />--%>
                                <asp:Button ID="BtnAdd" runat="server" class="btn btn-primary" Text="Add Department" />
                            </a>
                            <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" />
                            <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" />
                            <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                Visible="false" />
                        </div>
                        <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" id="DivView"
                            runat="server" visible="false">
                            <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                        </div>
                        <div style="margin-bottom: 20px">
                            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                ShowHeader="true" PageSize="20" EmptyDataText="No data to display.">
                                <Columns>
                                    <asp:TemplateField HeaderText="GrpID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("GroupID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="GroupName" HeaderText="Group Name" ControlStyle-Width="50px" />--%>
                                    <%-- <% If Session("CompID") = "1066" Then%>--%>
                                    <asp:BoundField DataField="GroupName" HeaderText="Department Name" ControlStyle-Width="50px" />
                                    <%--<% Else%>
                                    <asp:BoundField DataField="GroupName" HeaderText="Group Name" ControlStyle-Width="50px" />
                                    <% End If%>--%>
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:TemplateField HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <a href='<%# "AddGroup.aspx?GroupId=" & Crypto.Encrypt(Eval("VGroupId"))  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 330,marginTop : 0 } )">
                                                <i class="fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px">
                                                </i>
                                                <asp:Label ID="LBModify" runat="server" />
                                            </a>
                                            <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle Width="85px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
        </div>
    </div>
</asp:Content>
