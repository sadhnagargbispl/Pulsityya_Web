<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="RewardPointVoucher.aspx.vb" Inherits="RewardPointVoucher" %>

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

    <script type="text/javascript">
        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=GvData.ClientID%>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 1; i < grid.rows.length; i++) {
                    //get the reference of first column
                    cell = grid.rows[i].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell 
                            //checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
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
                            Reward Point Voucher
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        </div>
                        <div style="background-color: White">
                            <div class="col-md-12">
                                <div class="col-md-2">
                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Member ID Wise :" Font-Bold="true" />
                                    <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:CheckBox ID="ChkVoucher" runat="server" Text="Voucher No Wise :" Font-Bold="true" />
                                    <asp:TextBox ID="TxtVoucher" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:CheckBox ID="CheckBox2" runat="server" Text="Date Wise From Date :" Checked="true" />
                                    <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtStartDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtStartDate"
                                        ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-2">
                                    <label>
                                        To Date :
                                    </label>
                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtEndDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEndDate"
                                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-2">
                                    <label>
                                        Status:</label>
                                    <asp:RadioButtonList ID="RbtStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                        CssClass="form-control">
                                        <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Used" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="UnUsed" Value="N"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-3">
                                    <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-12" style="padding: 10px">
                                    <div class="col-md-2">
                                        <asp:Button ID="BtnApproveAll" runat="server" Text="ApproveAll" class="btn btn-primary"
                                            OnClientClick=" this.disabled=true;" UseSubmitBehavior="false" Visible="false" /></div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnRejectAll" runat="server" Text="RejectAll" class="btn btn-primary"
                                            OnClientClick="this.disabled=true;" UseSubmitBehavior="false" Visible="false" /></div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div id="DivRemark" runat="server" visible="false">
                                <table id="TblRemark" runat="server" align="center" style="background-color: #394a59;
                                    color: #d0d8df; border-color: Black; border-width: 1px; margin-top: -10px;">
                                    <tr>
                                        <td align="left">
                                            <strong>Remark</strong>*
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TxtARemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnApprove" runat="server" class="buttonBG" Text="Approve " Visible="false" />
                                        </td>
                                        <td>
                                            <asp:Label ID="LblRejIdNo" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="LblRejReqNo" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="padding: 10px 10px 20px 10px; margin: 10px 10px 10px 10px; overflow: scroll"
                                class="col-md-12">
                                <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="200"
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" Font-Size="12px"
                                    lass="table table-bordered" HeaderStyle-CssClass="bg-primary" GridLines="Both">
                                    <Columns>
                                        <asp:TemplateField HeaderText="CheckAll" Visible="false">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# Eval("VisibleStatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SNo.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Member ID">
                                            <ItemTemplate>
                                                <asp:Label ID="LblIDNo" runat="server" Text='<%# Eval("Member ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Member Name">
                                            <ItemTemplate>
                                                <asp:Label ID="LblPayeeName" runat="server" Text='<%# Eval("Member Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Voucher No">
                                            <ItemTemplate>
                                                <asp:Label ID="LblVoucherNo" runat="server" Text='<%# Eval("Voucher No") %>'></asp:Label>
                                                <asp:Label ID="LBlFormno" runat="server" Text='<%# Eval("Formno") %>' Visible="false"></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Voucher Amount" HeaderText="Voucher Amount" />
                                        <asp:TemplateField HeaderText="Generate Date">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGenerateDate" runat="server" Text='<%# Eval("Generate Date") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:BoundField DataField="Voucher Used Date" HeaderText="Voucher Used Date" />
                                        <asp:TemplateField HeaderText="Approve" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LBApprove" runat="server" Text="Approve" OnClientClick="return confirmation();"
                                                    OnClick="ApproveData" Visible='<%# Eval("VisibleStatus") %>' Style="color: Blue"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Reject" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LBReject" runat="server" Text="Reject" OnClientClick="return confirmation();"
                                                    OnClick="RejectData" Visible='<%# Eval("IsVisible") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtRemarks" runat="server" Style="display: inline" Text='<%# Eval("Remark") %>'
                                                    Enabled='<%# Eval("VisibleStatus") %>'></asp:TextBox>
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
        <%--</div>--%>
</asp:Content>
