<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="KitProdMaster.aspx.vb" Inherits="App_UI_Application_Pages_KitProdMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
    <div align="right" class="divSearchContent">
        <asp:DropDownList ID="ddlGroupFields" runat="server" CssClass="ddlSearch">
            <asp:ListItem Value="None" Selected="True">--Search By--</asp:ListItem>
            <asp:ListItem Value="CatName">Cat Name</asp:ListItem>
            <asp:ListItem Value="ProductName">Product Name</asp:ListItem>
            <asp:ListItem Value="Remarks">Remarks</asp:ListItem>
            <asp:ListItem>Status</asp:ListItem>
            <asp:ListItem>ShowAll</asp:ListItem>
        </asp:DropDownList>
        <div class="Wrapper">
            <asp:TextBox ID="txtSearch" CssClass="txtSearch" runat="server" Height="22px"></asp:TextBox>
            <asp:ImageButton ID="imgSearch" runat="server" CssClass="imgSearchButton" ImageUrl="Images/search.png"
                Width="22px" Height="22px" />
        </div>
    </div>
    <div>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnExport" runat="server" CssClass="Btn" Text="Export To Excel" />
                </td>
                <td>
                    <a href="AddProd.aspx" class="fancybox fancybox.iframe">
                        <asp:Button ID="BtnAdd" runat="server" CssClass="Btn" Text="Add Product" /></a>
                </td>
                <td>
                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" CssClass="Btn" />
                </td>
                <td>
                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" CssClass="Btn" />
                </td>
                <%--<asp:Button ID="btnPrint" runat="server" CssClass="Btn" Text="Print" OnClientClick="PrintGridData() />--%>
                <td>
                    <asp:Button ID="btnShowRecord" runat="server" CssClass="Btn" Text="View All" Visible="false" />
                    <%--<asp:Button ID="BtnAdvSearch" runat="server" CssClass="Btn" Text="Advanced Search" />--%>
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;">
        <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
        <br />
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
                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("ProdID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CatName" HeaderText="Category" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" ControlStyle-Width="50px" />
                <asp:BoundField DataField="Rate" HeaderText="Rate" />
                <asp:BoundField DataField="Qty" HeaderText="Qty" />
                <asp:BoundField DataField="Tax" HeaderText="Tax" />
                <asp:BoundField DataField="TaxAmount" HeaderText="Tax Amount" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" />
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                    ControlStyle-CssClass="btn-group ">
                    <ItemTemplate>
                        <a href='<%# "AddProd.aspx?ProdId=" & Crypto.Encrypt(Eval("VProdId"))  %>' class="fancybox fancybox.iframe"
                            class="btn btn-primary"><i class="icon_plus_alt2"></i>
                            <%--<asp:Label ID="LBModify" runat="server" Text="Modify"/>--%>
                        </a>
                    </ItemTemplate>
                    <HeaderStyle Width="55px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteProd" class="btn btn-danger"><i class="icon_close_alt2"></i></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Width="55px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    <%--<input type="button" id="btnPrint" value="Print" onclick="PrintGridData()" />
--%>
    <br />
</asp:Content>
