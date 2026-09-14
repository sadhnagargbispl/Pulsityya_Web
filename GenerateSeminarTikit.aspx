<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="GenerateSeminarTikit.aspx.vb" Inherits="GenerateSeminarTikit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
                            Generate Seminar ticket</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div align="center">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        Seminar Name :</div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlProgram" runat="server" class="form-control" >
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="ddlProgram"
                                            runat="server" ValidationGroup="Save" InitialValue="0">Please Select Seminar.!!</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-5">
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        Qty. :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtqty" runat="server" class="form-control">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="txtqty"
                                            runat="server" ValidationGroup="Save">Please Enter Quantity.!!</asp:RequiredFieldValidator>
                                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                            TargetControlID="txtqty" ValidChars="0123456789">
                                        </ajax:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-5">
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Button ID="BtnGenerate" runat="server" Text="Generate" class="btn btn-primary"
                                            ValidationGroup="Save" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label></div>
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
