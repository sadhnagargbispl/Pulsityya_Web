<%@ Page Language="VB" AutoEventWireup="false" CodeFile="viewmember.aspx.vb" Inherits="App_UI_Application_Pages_viewmember" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/custom.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div style=" background-color :White; overflow :scroll;">
   <%-- <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">--%>
                    <div class="x_title">
                        <h2>
                            Visiting Member Details</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="col-md-12">
                            <div class="col-md-4">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" PageSize="20" Width="430px" EmptyDataText="No data to display.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SNo.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GrpID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblId" runat="server" Text='<%# Eval("VId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Member Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Lblmemname" runat="server" Text='<%# Eval("MemName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Age">
                                            <ItemTemplate>
                                                <asp:Label ID="Lblage" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pincode">
                                            <ItemTemplate>
                                                <asp:Label ID="Lblpincode" runat="server" Text='<%# Eval("Pincode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="State Name">
                                            <ItemTemplate>
                                                <asp:Label ID="LblStatename" runat="server" Text='<%# Eval("StateName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="City Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Lblcityname" runat="server" Text='<%# Eval("CityName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Visiting Date">
                                            <ItemTemplate>
                                            <%#Convert.ToDateTime(Eval("Visitingdate")).ToString("dd-MM-yyyy")%>
                                                <%--<asp:Label ID="LblVisitingDate" runat="server" Text='<%# Eval("rectimestamp") %>'></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <%--<asp:BoundField DataField="MemName" HeaderText="Member Name" />--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="col-md-8">
                            </div>
                        </div>
                    </div>
              <%--</div>
            </div>
        </div>
    </div>--%>
    </div>
    </form>
</body>
</html>
