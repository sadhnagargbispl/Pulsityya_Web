<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="PayoutExportForTally.aspx.vb" Inherits="App_UI_Application_Pages_PayoutExportForTally" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
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
                            Payout Export For Tally</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        <strong>Select Session :</strong></div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlsession" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlsession" runat="server" ControlToValidate="ddlsession"
                                            InitialValue="0" ErrorMessage="Please Select Session.!!" ValidationGroup="vg1"
                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                 
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClientClick="return confirmation();"
                                            class="btn btn-primary" ValidationGroup="Save" /></div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%; display:none " >
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="True" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" CssClass="table table-striped table-advance table-hover"
                                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                        PageSize="25" EmptyDataText="No data to display.">
                                        <Columns>
                                           <%-- <asp:TemplateField HeaderText="Vch No." HeaderStyle-Width="35px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1%></ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
