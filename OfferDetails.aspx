<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OfferDetails.aspx.vb" Inherits="OfferDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    offer Details</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gv" runat="server" CssClass="" class="table table-bordered"
                                    HeaderStyle-CssClass="bg-primary" AutoGenerateColumns="true">
                                    <Columns>
                                       <%-- <asp:TemplateField HeaderText="Kit Name">
                                            <ItemTemplate>
                                                <%#Eval("KitName")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Left Active">
                                            <ItemTemplate>
                                                <%#Eval("LeftActive")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Right Active">
                                            <ItemTemplate>
                                                <%#Eval("RightActive")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
