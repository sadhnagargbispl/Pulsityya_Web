<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="GenerateEP.aspx.vb" Inherits="App_UI_Application_Pages_GenerateEP" %>

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
                            Generate EP</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="table-responsive makeitresponsivegrid">
                            <div class="col-md-6">
                                <asp:GridView ID="gv" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered"
                                    HeaderStyle-CssClass="bg-primary" ShowHeader="true">
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="col-md-1">
                                        EP :</div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="TxtQty" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Quantity."
                                            ControlToValidate="TxtQty" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="col-md-1">
                                        Remark :</div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="txtRemark" runat="server" class="form-control" TextMode="MultiLine"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Remark."
                                            ControlToValidate="TxtQty" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary" ValidationGroup="Save" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                  
                                    <div class="col-md-12">
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
