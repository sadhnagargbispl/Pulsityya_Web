<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="AddLevelIncome.aspx.vb" Inherits="AddLevelIncome" %>
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
                            Level Income</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
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
                                                <asp:Label ID="LblMemName" runat="server" class="form-control" style="text-align :left"></asp:Label></div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                    
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-12">
                                                    <div class="col-md-4">
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Button ID="BtnUpgrade" runat="server" Text="Submit" class="btn btn-primary"
                                                            ValidationGroup="Save" Enabled="false" />
                                                       
                                                    </div>
                                                    <div class="col-md-4">
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                          <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                                
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <div class="col-md-12">
                                           <asp:GridView ID="GrdDirects1s" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="10" EmptyDataText="No data to display." Visible="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>.
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:BoundField DataField="idno" HeaderText="Member ID" />
                                            <asp:BoundField DataField="membername" HeaderText="Member Name" />
                                            <asp:BoundField DataField="datetime" HeaderText="Date" />
                                           </Columns>
                                           </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                              <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                       
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
