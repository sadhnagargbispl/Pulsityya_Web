<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="FundWithdrawReport.aspx.vb" Inherits="App_UI_Application_Pages_FundWithdrawReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
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
                            Wallet Authentication Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div style="background-color: White">
                            <div class="col-md-12">
                                <div class="col-md-1">
                                    <b>Member ID :</b>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox></div>
                                <div class="col-md-1">
                                    <asp:Label ID="lblStartDate" runat="server" Text="From Date : "></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtStartDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtStartDate"
                                        ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="lblEndDate" runat="server" Text="To Date : "></asp:Label></div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtEndDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEndDate"
                                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator></div>
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-4">
                                    <asp:RadioButtonList ID="RbtStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Text="All" Value="N" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Approve" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                        <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" /></div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" /></div>
                            </div>
                            <div style="padding: 10px 10px 20px 10px; margin: 10px 10px 10px 10px; overflow: scroll"
                                class="col-md-12">
                                <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="100"
                                    AutoGenerateColumns="true" CellPadding="4" ForeColor="#333333" Font-Size="12px"
                                    lass="table table-bordered" HeaderStyle-CssClass="bg-primary" GridLines="Both">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SNo.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
