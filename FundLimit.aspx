<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="FundLimit.aspx.vb" Inherits="FundLimit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

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
                            Fund Limit Master</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-9">
                                    <div class="col-md-4">
                                        Minimum Withdrawl Limit</div>
                                    <div class="col-md-5">
                                        <div class="form-group ">
                                            <asp:TextBox ID="TxtWithdraLimit" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event);">
                                            </asp:TextBox>
                                            
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="* Enter Minimum Withdrawl Amount."
                                            ControlToValidate="TxtWithdraLimit" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </div>
                                    </div>
                                </div>
                                  <div class="col-md-9">
                                    <div class="col-md-4">
                                        Maximum Withdrawl Limit</div>
                                    <div class="col-md-5">
                                        <div class="form-group ">
                                            <asp:TextBox ID="txtmaxWithdrawllimit" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="* Enter Maximum Withdrawl Amount."
                                            ControlToValidate="txtmaxWithdrawllimit" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </div>
                                    </div>
                                </div>
                                <div class="col-md-9">
                                    <div class="col-md-4">
                                        Withdrawl Deduction %</div>
                                    <div class="col-md-5">
                                        <div class="form-group ">
                                            <asp:TextBox ID="TxtWithDrawDedution" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event);">
                                            </asp:TextBox>
                                            
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Withdrawl Deduction."
                                            ControlToValidate="TxtWithDrawDedution" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-9" style="display:none">
                                    <div class="col-md-4">
                                        Minimum Wallet Limit</div>
                                    <div class="col-md-5">
                                        <div class="form-group ">
                                            <asp:TextBox ID="TxtWalletLimit" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Minimum Wallet Amount."
                                            ControlToValidate="TxtWalletLimit" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </div>
                                    </div>
                                </div>
                                
                                <div class="col-md-9" style="display:none">
                                    <div class="col-md-4">
                                        Wallet Deduction%</div>
                                    <div class="col-md-5">
                                        <div class="form-group ">
                                            <asp:TextBox ID="TxtWalletDeduct" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Wallet Deduction."
                                            ControlToValidate="TxtWalletDeduct" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                
                                            </div>
                                    
                                    
                                    </div>
                                </div>
                                <div class="col-md-8">
                                    <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-primary" Text="Save" ValidationGroup="Save" />
                                    <asp:Button ID="BtnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" /></div>
                            </div>
                            <div style="margin-bottom: 20px">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="True" RowStyle-Height="25px"
                                    GridLines="None" AllowPaging="true"   class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                    PageSize="20" EmptyDataText="No data to display.">
                                </asp:GridView>
                            </div>
                            <br />
                            <br />
                            <br />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
