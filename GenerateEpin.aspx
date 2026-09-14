<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="GenerateEpin.aspx.vb" Inherits="App_UI_Application_Pages_GenerateEpin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Generate Epin</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-12" id="trstock" runat="server" visible="false">
                                    <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;">
                                    </span>
                                    <asp:GridView ID="GrdStock" Width="100%" runat="server" GridLines="Both" AllowPaging="true"
                                        class="table table-bordered" HeaderStyle-CssClass="bg-primary" AutoGenerateColumns="false"
                                        ShowHeader="true" PageSize="10" EmptyDataText="No data to display.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNo.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Package Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="LBlKitName" runat="server" Text='<%# Eval("KitName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Package MRP.">
                                                <ItemTemplate>
                                                    <asp:Label ID="LBlMRP" runat="server" Text='<%# Eval("MRP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Package BV">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblKitBv" runat="server" Text='<%# Eval("KitBv") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblStock" runat="server" Text='<%# Eval("Stock") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        Package :</div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="CmbKit" runat="server" class="form-control" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-5">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                    <div class="col-md-3">
                                        Quantity :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtQty" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Quantity."
                                            ControlToValidate="TxtQty" ValidationGroup="Save"></asp:RequiredFieldValidator></div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Button ID="BtnExportStock" Text="Export Available Stock" runat="server" class="btn btn-primary" />
                                        <asp:Button ID="BtnGenerate" runat="server" Text="Generate" class="btn btn-primary"
                                            ValidationGroup="Save" />
                                        <asp:Button ID="BtnCancel" class="btn btn-primary" runat="server" Text="Cancel" ValidationGroup="Cancel" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label></div>
                                </div>
                                <div style="padding: 10px 10px 20px 10px" class="col-md-12">
                                    <asp:GridView ID="GvBatchMaster" Width="100%" runat="server" GridLines="Both" AllowPaging="true"
                                        class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                        PageSize="10" EmptyDataText="No data to display." AutoGenerateColumns ="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNo.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Package">
                                                <ItemTemplate>
                                                    <asp:Label ID="KitName" runat="server" Text='<%# Eval("KitName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Epin No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="EpinNo" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Scratch No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="ScratchNo" runat="server" Text='<%# Eval("ScratchNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="StockDate" runat="server" Text='<%# Eval("StockDate") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div>
                                    <center>
                                        <asp:Button ID="BtnExport" runat="server" Text="Export To Excel" class="btn btn-primary"
                                            ValidationGroup="Save" Visible="false" />
                                    </center>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
            </div>
        </div>
    </div>
</asp:Content>
