<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewPinDetail.aspx.vb" Inherits="ViewPinDetail"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <!-- bootstrap theme -->
    <link href="css/bootstrap-theme.css" rel="stylesheet">
    <!--external css-->
    <!-- font icon -->
    <link href="css/Grid.css" rel="Stylesheet" type="text/css" />
    <link href="css/elegant-icons-style.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <!-- Custom styles -->
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style-responsive.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="LblNo" runat="server" ForeColor="Black" Font-Size="14px"></asp:Label>
    </div>
    <p style="color: #666666; line-height: 25px; padding-left: 20px; padding-right: 20px">
        Pin Status :
        <br />
        <asp:RadioButtonList ID="rbtnStatus" runat="server" CssClass="textB" RepeatDirection="Horizontal"
            Font-Bold="True" AutoPostBack="true">
            <asp:ListItem Selected="True" Text="Both" Value="Both"></asp:ListItem>
            <asp:ListItem Text="Used" Value="Used"></asp:ListItem>
            <asp:ListItem Text="UnUsed" Value="UnUsed"></asp:ListItem>
        </asp:RadioButtonList>
    </p>
    <div style="margin-bottom: 20px; padding-right: 20px">
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
            GridLines="Both" AllowPaging="true" CssClass="table table-striped table-advance table-hover"
            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" Width="95%" ShowHeader="true"
            PageSize="50" EmptyDataText="No data to display.">
            <Columns>
                <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="35px">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="GrpID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                        <asp:Label ID="LblFormNo" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                        <asp:Label ID="LblScratchno" runat="server" Text='<%# Eval("ScratchNo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ChallanNo" HeaderText="Bill No." ControlStyle-Width="50px" />
                <asp:BoundField DataField="KitName" HeaderText="Package" ControlStyle-Width="50px" />
                <asp:BoundField DataField="FormNo" HeaderText="Pin No." />
                <asp:BoundField DataField="ScratchNo" HeaderText="Scratch No." />
                <asp:BoundField DataField="FCode" HeaderText="Issued ID No." />
                <asp:BoundField DataField="MemName" HeaderText="Issued Member Name" />
                <asp:BoundField DataField="IssueDt" HeaderText="Issue Date" />
                <asp:BoundField DataField="UsedId" HeaderText="Used By" />
                <asp:BoundField DataField="UsedDate1" HeaderText="Used Date" />
                <asp:BoundField DataField="Status" HeaderText="PinStatus" />
                <asp:TemplateField HeaderText="IsCancel" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="LblIsCancel" runat="server" Text='<%# Eval("IsCancel") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Issue Cancel" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="LnkCancelIssue" runat="server" OnClick="CancelIssue" Text='<%# Eval("PinCancel") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Width="55px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    </form>
</body>
</html>
