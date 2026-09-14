<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="AddLoanValue.aspx.vb" Inherits="AddLoanValue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function isNumberMonth() {

            var res = document.getElementById("ctl00_ContentPlaceHolder1_TxtMonth").value;

            if (res < 1 || res > 10)
                alert("Enter EMI Between 1 to 10 ");
        }
        function validfloat(txt, ev)
        {
            ev.returnValue = ((ev.keyCode >= 48 && ev.keyCode <= 57)||(ev.keyCode == 46 && txt.value.indexOf('.') == -1));
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
                            Add Loan</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-3">
                                            Member ID:
                                        </div>
                                        <div class="col-md-6">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true"
                                                        ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                                        ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-md-3">
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-12">
                                                <div class="col-md-3">
                                                    MemberName:</div>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="TxtName"  runat="server" class="form-control" Enabled="false"></asp:TextBox>
                                                    <asp:TextBox ID="TxtFormNo" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-12">
                                            <br />
                                                <div class="col-md-3">
                                                    Choose Product:</div>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="DDlProduct" runat="server" AutoPostBack="true" class="form-control"
                                                        >
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-3">
                                                </div>
                                                                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                         <div class="col-md-12">
                                            <br />

                                                <div class="col-md-3">
                                                    Serial No</div>
                                                <div class="col-md-6">
                                                  <asp:DropDownList ID="DDlSerial" runat="server" class="form-control"></asp:DropDownList>
                                                  
                                                      </div>
                                                <div class="col-md-3">
                                                </div>
                                            </div>
                                            
                                            <div class="col-md-12">
                                            <br />

                                                <div class="col-md-3">
                                                    Total Amount:</div>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="TxtFund"  runat="server" class="form-control" onkeypress="return isNumberKey(event);"
                                                        AutoPostBack="true"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Amount."
                                                        ControlToValidate="TxtFund" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-3">
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DDLProduct" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="col-md-12">
                                        <div class="col-md-3">
                                            Advance Amount:</div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="TxtROI"  runat="server" class="form-control" AutoPostBack="true"  onkeypress="validfloat(this, event);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="* Enter Advance Amount."
                                                ControlToValidate="TxtROI" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3">
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                    <div class="col-md-12">
                                        <div class="col-md-3">
                                            Remaining Amount:</div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="TxtRemaining"  runat="server" class="form-control"
                                                onkeypress="validfloat(this, event);" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="* Enter Remaining Amount."
                                                ControlToValidate="TxtRemaining" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3">
                                        </div>
                                    </div>
                                      </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="TxtROI" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="col-md-12">
                                        <div class="col-md-3">
                                            No. Of EMI</div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="TxtMonth"  runat="server" class="form-control" onkeyup="isNumberMonth();"
                                                onkeypress="return isNumberKey(event);" MaxLength="2" AutoPostBack="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="* Enter EMI From 1 to 10 ."
                                                ControlToValidate="TxtMonth" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-3">
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-3">
                                            EMI DETAIL</div>
                                        <div class="col-md-6">
                                            <asp:GridView ID="GrdDirect" runat="server" AutoGenerateColumns="false" class="table table-bordered"
                                                HeaderStyle-CssClass="bg-primary">
                                                <Columns>
                                                    <asp:BoundField DataField="LoanNo" HeaderText="Loan No" />
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblAMount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ChequeNo">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtCheqUeNo" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldChequeNo" runat="server" ErrorMessage="* Enter Cheque No."
                                                        ControlToValidate="TxtCheqUeNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Chequedate">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtCheqUeDate" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldChequeDate" runat="server" ErrorMessage="* Enter Cheque Date."
                                                        ControlToValidate="TxtCheqUeDate" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                            
                                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtCheqUeDate"
                                                                Format="dd-MMM-yyyy">
                                                            </AjaxToolkit:CalendarExtender>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtCheqUeDate"
                                                                ErrorMessage="Invalid Cheque Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="display:none">
                                        <div class="col-md-3">
                                            Remark</div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="TxtRemarks"  runat="server" class="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Remrks"
                                                ControlToValidate="TxtRemarks" ValidationGroup="Save"></asp:RequiredFieldValidator></div>
                                        <div class="col-md-3">
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-3">
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Button ID="BtnFundTransfer" runat="server" Text="Submit" OnClientClick="return confirmation();"
                                                class="btn btn-primary" ValidationGroup="Save" />
                                            <asp:Button ID="BtnCancel" runat="server" Text="Cancel" class="btn btn-primary" />
                                        </div>
                                        <div class="col-md-3">
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="BtnFundTransfer" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <!-- end of weather widget -->
    </div>
</asp:Content>
