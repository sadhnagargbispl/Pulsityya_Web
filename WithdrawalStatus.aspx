<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="WithdrawalStatus.aspx.vb" Inherits="WithdrawalStatus" Title="Untitled Page" %>

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
                            Withdrawal Status
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="col-md-12">
                          <div class="col-md-4">
                         <b> Current Status  : - </b>    <asp:Label ID="lblStatus" runat="server" Text="Label" ForeColor ="Red" Font-Size="Large" ></asp:Label>
                          </div>
                          <br />
                            <div class="col-md-4">
                                <asp:RadioButtonList ID="Rbtstatus" runat="server" RepeatDirection="Horizontal" Style="margin: 2px"
                                    RepeatLayout="Flow">
                                    <asp:ListItem Text="Start Withdrawal" Value="Y" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Stop Withdrawal" Value="N"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="Btnsubmit" runat="server" Text="Submit" class="btn btn-primary" OnClientClick="return confirmation();" /></div>
                            <div class="col-md-4">
                            </div>
                        </div>
</asp:Content>
