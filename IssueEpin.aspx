<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="IssueEpin.aspx.vb" Inherits="App_UI_Application_Pages_IssueEpin" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-family: Arial;
        }
        .modal1
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        .center1
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center1 img
        {
            height: 128px;
            width: 128px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="images/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Issue Epin</h2>
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
                                            <asp:Button ID="BtnExpStock" runat="server" Text="Export Available Stock" class="btn btn-primary"
                                                ValidationGroup="Export" Visible="false" />
                                            <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;">
                                            </span>
                                            <asp:Label ID="LblProductAvail" runat="server" Visible="false"></asp:Label>
                                        </div>
                                        <div class="col-md-12">
                                            <asp:GridView ID="GrdStock" Width="60%" runat="server" GridLines="Both" AllowPaging="true"
                                                class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                                AutoGenerateColumns="false" PageSize="10" EmptyDataText="No data to display.">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SNo.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="KitName" HeaderText="Package" />
                                                    <asp:BoundField DataField="KitAmount" HeaderText="Package MRP" />
                                                    <asp:BoundField DataField="KitBv" HeaderText="Package BV" />
                                                    <asp:BoundField DataField="Stock" HeaderText="Stock" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-3">
                                                Member ID :</div>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                                        <asp:Label ID="LblMemMobl" runat="server" CssClass="label-text " Visible="false"></asp:Label>
                                                        <asp:Label ID="LblFormno" runat="server" Visible="false"></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                                    ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <br />
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
                                                <asp:UpdatePanel ID="UpdtPanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="TxtQty" runat="server" class="form-control" onkeypress="return isNumberKey(event);"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <asp:Label ID="LblStockChk" runat="server" CssClass="label-text" ForeColor="Red"></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="TxtQty" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Quantity."
                                                    ControlToValidate="TxtQty" ValidationGroup="Save"></asp:RequiredFieldValidator></div>
                                        </div>
                                        <div class="col-md-12">
                                            <br />
                                            <div class="col-md-3">
                                                Remark :</div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtRemark" runat="server" class="form-control"></asp:TextBox></div>
                                            <div class="col-md-5">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Remark."
                                                    ControlToValidate="TxtRemark" ValidationGroup="Save"></asp:RequiredFieldValidator></div>
                                        </div>
                                        <div class="col-md-12">
                                            <br />
                                            <div class="col-md-3">
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Button ID="BtnGenerate" runat="server" Text="Issue" class="btn btn-primary"
                                                    ValidationGroup="Save" />
                                                <asp:Button ID="BtnCancel" class="btn btn-primary" runat="server" Text="Cancel" ValidationGroup="Cancel" /></div>
                                            <div class="col-md-5">
                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label></div>
                                        </div>
                                        <div style="padding: 10px 10px 20px 10px" class="col-md-12">
                                            <asp:GridView ID="GvBatchMaster" Width="100%" runat="server" GridLines="Both" AllowPaging="true"
                                                class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                                PageSize="10" EmptyDataText="No data to display." AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SNo.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bill No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="BillNo" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="IDNo" runat="server" Text='<%# Eval("FCode") %>'></asp:Label>
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
                                                    <asp:TemplateField HeaderText="Issue Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="ScratchNo" runat="server" Text='<%# Eval("IssueDt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div>
                                                <center>
                                                    <asp:Button ID="BtnExport" runat="server" Text="Export To Excel" Visible="false"
                                                        class="btn btn-primary" ValidationGroup="Export" />
                                                </center>
                                            </div>
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TxtQty" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="BtnGenerate" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
