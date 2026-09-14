<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="RepurchaseWithdrawl.aspx.vb" Inherits="RepurchaseWithdrawl" %>

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
                        Repurchase Withdrawal List</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                
                <div class="panel-body">
                    <div align="center">
                        <span id="lblt" class="text-danger"></span>
                    </div>
                    <div style="background-color: White">
                        <div class="col-md-12">
                            <div class="col-md-2">
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="Member ID Wise :" Font-Bold="true" />
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox></div>
                            <div class="col-md-2">
                                <asp:CheckBox ID="CheckBox2" runat="server" Text="Date Wise :" Font-Bold="true" Checked="true" /></div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlSession" runat="server" class="form-control" Style="text-indent: 1px;">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <asp:RadioButtonList ID="RbtStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Text="All" Value="N" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Approve" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="col-md-12" style="padding: 10px">
                            <div class="col-md-2">
                                <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" /></div>
                            <div class="col-md-2">
                                <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" /></div>
                            <div class="col-md-2">
                                <asp:Button ID="BtnExportToCsv" runat="server" class="btn btn-primary" Text="Export To Csv" /></div>
                            <div class="col-md-2">
                                <asp:Button ID="BtnApproveAll" runat="server" Text="ApproveAll" class="btn btn-primary" OnClientClick="return confirmation();" /></div>
                            <div class="col-md-2">
                                <asp:Button ID="btnRejectAll" runat="server" Text="RejectAll" class="btn btn-primary" OnClientClick="return confirmation();"/></div>
                            <div class="col-md-2">
                                <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label></div>
                        </div>
                    </div>
                    <div id="DivTopup" runat="server" visible="false" style="width: 100%">
                        <center>
                            <table align="center" border="1px" style="background-color: #394a59; color: #d0d8df;">
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtRemark"
                                            ErrorMessage="Remark Not Found, Check?" Font-Bold="True" ForeColor="Maroon" ValidationGroup="confirm"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <strong>IDNo</strong>*
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtIDNo" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                                        <asp:Label ID="lblID" runat="server" Visible="false"></asp:Label>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <strong>Name</strong>*
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtName" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                                        <asp:Label ID="LblFDate" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblTdate" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblMobil" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LnblWeek" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblForm1" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LabelDate1" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <strong>Amount</strong>*
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtAmount" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <strong>Payment Type</strong>
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="RbtPayment" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" AutoPostBack="true">
                                            <asp:ListItem Text="Cash" Value="C" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Cheque" Value="Q"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <div id="BankDetail" runat="server" visible="false">
                                    <tr>
                                        <td align="left">
                                            <strong>Bank Name</strong>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="DDlBank" runat="server" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <strong>Branch Name</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtBranchName" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <strong>IFSC Code</strong>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TxtIFSCode" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <strong>Account Number</strong>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtAccount" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <strong>Cheque Number </strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCheque" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <strong>Cheque Date</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChequeDate" runat="server"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtChequeDate"
                                                Format="dd-MMM-yyyy">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtChequeDate"
                                                ErrorMessage="Invalid Cheque Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </div>
                                <tr>
                                    <td align="left">
                                        <strong>Remark</strong>*
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trcommand" runat="server">
                                    <td style="height: 26px">
                                    </td>
                                    <td style="height: 26px" align="left">
                                        <asp:Button ID="btnConfirm" runat="server" Text="Confirm" class="buttonBG" Style="height: 24px;
                                            width: 64px;" ValidationGroup="confirm" />
                                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" class="buttonBG" Style="height: 24px;
                                            width: 64px;" />
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </center>
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
                                    <asp:Button ID="btnApprove" runat="server" class="buttonBG" Text="Approve " Visible="false" /><asp:Button
                                        ID="btnReject" runat="server" Text="Reject " class="ButtonBG" Visible="false" /><asp:Button
                                            ID="btnRejectSingle" runat="server" Text="Reject" runtat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="LblRejIdNo" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="LblRejWeek" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblRejFormno" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="LblRejDateOn" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="LblRejReqNo" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
               
                <div style="padding: 10px 10px 20px 10px; margin: 10px 10px 10px 10px; overflow :scroll "class="col-md-12">
                    <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="200"
                        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                        Font-Size="12px" lass="table table-bordered" HeaderStyle-CssClass="bg-primary" GridLines ="Both" >
                        <Columns>
                            <asp:TemplateField HeaderText="CheckAll">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAll" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# Eval("IsVisible") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SNo.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LblWeek" runat="server" Text='<%# Eval("WeekNo") %>'></asp:Label>
                                    <asp:Label ID="LblID" runat="server" Text='<%# Eval("Sno") %>'></asp:Label>
                                    <asp:Label ID="LblMobl" runat="server" Visible="false" Text='<%# Eval("Mobl") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FormNo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LblFormNo" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                                    <asp:Label ID="LblFromDate" runat="server" Text='<%# Eval("FromDate") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="LblTodate" runat="server" Text='<%# Eval("ToDate") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Withdrawl Date">
                                <ItemTemplate>
                                    <asp:Label ID="LblDate" runat="server" Text='<%# Eval("WithDrawDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Member ID">
                                <ItemTemplate>
                                    <asp:Label ID="LblIDNo" runat="server" Text='<%# Eval("MemberID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payee Name">
                                <ItemTemplate>
                                    <asp:Label ID="LblPayeeName" runat="server" Text='<%# Eval("PayeeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="LblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bank Name">
                                <ItemTemplate>
                                    <asp:Label ID="LblBankname" runat="server" Text='<%# Eval("BankName") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Name">
                                <ItemTemplate>
                                    <asp:Label ID="LblBranch" runat="server" Text='<%# Eval("BranchName") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account No.">
                                <ItemTemplate>
                                    <asp:Label ID="LblAccountNo" runat="server" Text='<%# Eval("AcNo") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IFSC Code">
                                <ItemTemplate>
                                    <asp:Label ID="LblIFsCode" runat="server" Text='<%# Eval("IFsCode") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PanNo" HeaderText="PanNo" />
                            <asp:TemplateField HeaderText=" Cheque No">
                                <ItemTemplate>
                                    <asp:Label ID="LblChequeNo" runat="server" Text='<%# Eval("ChequeNo") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cheque Date">
                                <ItemTemplate>
                                    <asp:Label ID="LblChequeDate" runat="server" Text='<%# Eval("ChequeDate") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:TemplateField HeaderText="Approve" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" Visible ="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LBApprove" runat="server" Text="Approve" OnClientClick="return confirmation();"
                                        OnClick="ApproveData" Visible='<%# Eval("IsVisible") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reject" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" Visible ="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LBReject" runat="server" Text="Reject" OnClientClick="return confirmation();"
                                        OnClick="RejectData" Visible='<%# Eval("IsVisible") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                            <asp:TextBox ID="TxtRemarks" runat="server" Style="display: inline"
                            Text='<%# Eval("Remark") %>' Enabled='<%# Eval("IsVisible") %>'></asp:TextBox>
                            </ItemTemplate></asp:TemplateField>
                            <%--<asp:BoundField DataField="Remark" HeaderText="Remark" />--%>
                            <asp:BoundField DataField="ProcessedBy" HeaderText="Processed By" />
                            <asp:BoundField DataField="ProcessedDate" HeaderText="Processed Date" />
                            <asp:BoundField DataField="WeekNo" HeaderText="Week No." />
                            <asp:BoundField DataField="Fromdate" HeaderText="FromDate" />
                            <asp:BoundField DataField="ToDate" HeaderText="ToDate" />
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
