<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="IdActivate.aspx.vb" Inherits="App_UI_Application_Pages_IdActivate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
    </script>

    <style type="text/css">
        .col-md-12
        {
            margin-bottom: 10px;
        }
    </style>
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="images/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Upgrade Id</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <%-- Available Balance:--%><span class="red" id="AvailableBal" style="color: Red"
                                runat="server"></span>
                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="LblCondition" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Member ID :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                                    ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                <asp:Label ID="LblKitId" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblNewKitid" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblFormno" runat="server" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DDlKit" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Member Name :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="LblMemName" runat="server" class="form-control" Style="text-align: left"></asp:Label></div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Sponsor ID :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="TxtSponsorid" runat="server" class="form-control" Style="text-align: left"></asp:Label></div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Sponsor Name :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="TxtSponsorName" runat="server" class="form-control" Style="text-align: left"></asp:Label></div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Current Package :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="LblKitName" runat="server" class="form-control" Style="text-align: left"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12" runat="server" id="crntBV">
                                            <div class="col-md-4">
                                                Current BV :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="LblCrntBV" runat="server" class="form-control" Style="text-align: left"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Package Name :</div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DDlKit" runat="server" class="form-control" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12" runat="server" id="droputr">
                                            <div class="col-md-4">
                                                Choose Method :</div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropDownList1" runat="server" class="form-control" AutoPostBack="true">
                                                    <asp:ListItem Text="Choose Method" Value="" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Cash" Value="C"></asp:ListItem>
                                                    <asp:ListItem Text="Online" Value="O"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12" runat="server" id="utr">
                                            <div class="col-md-4">
                                                UTR No :</div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtUTRno" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12" runat="server" id="remark">
                                            <div class="col-md-4">
                                                Remark :</div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtUTRRemark" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12" id="TrDilivery" runat="server" visible="false">
                                            <div class="col-md-4">
                                                Delivery :</div>
                                            <div class="col-md-4">
                                                <asp:RadioButtonList ID="RbtDelivery" runat="server" AutoPostBack="True" RepeatColumns="3"
                                                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <asp:ListItem Text="By Courier" Value="C" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="By SpeedPost" Value="S"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12" id="trDeliveryCenter" runat="server" visible="false">
                                            <div class="col-md-4">
                                                Delivery Center</div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DDlDeliveryCenter" runat="server" Class="form-control">
                                                </asp:DropDownList>
                                                <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblPassw" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblEmail" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="Lblsponsormobl" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12" id="trDeliveryAddress" runat="server" visible="false">
                                            <div class="col-md-4">
                                                Delivery Address</div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtDeliveryAddress" runat="server" Class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-12">
                                                    <div class="col-md-4">
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Button ID="BtnUpgrade" runat="server" Text="Upgrade" class="btn btn-primary"
                                                            ValidationGroup="Save" Enabled="false" OnClientClick="return confirmation();" />
                                                        <asp:Button ID="BtnCancel" class="btn btn-primary" runat="server" Text="Cancel" />
                                                    </div>
                                                    <div class="col-md-4">
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="DDlKit" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <div class="col-md-12">
                                            <asp:DataGrid ID="GrdDirects1" runat="server" CssClass="table table-striped table-advance table-hover"
                                                RowStyle-Height="25px" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                CellPadding="3" HorizontalAlign="Center" AutoGenerateColumns="False" AllowPaging="True"
                                                Width="100%" ShowHeader="true" PageSize="5" EmptyDataText="No data to display."
                                                Visible="false">
                                                <Columns>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <%#Container.DataSetIndex + 1%>.
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="IDNO" HeaderText="ID No">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="MemName" HeaderText="Member Name">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="KitName" HeaderText="Package Name">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="UpgradeDate" HeaderText="Top Up Date">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="BV" HeaderText="Top Up Date BV">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                    </asp:BoundColumn>
                                                </Columns>
                                                <PagerStyle Mode="NumericPages" CssClass="PagerStyle"></PagerStyle>
                                                <%--   <ItemStyle CssClass="RowStyle" HorizontalAlign="Center" VerticalAlign="Top" Wrap="False" />--%>
                                            </asp:DataGrid>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="DDlKit" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
