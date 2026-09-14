<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="ApprovePaymentReqsAFId.aspx.vb" Inherits="ApprovePaymentReqsAFId" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        //$(document).ready(function() { $('[id$=chkSelectAll]').click(function() { $("[id$='chkSelect']").attr('checked', this.checked); }); });
        //    function reset() {
        //        $("[id$='chkSelect']").prop('checked', false);
        //    }



        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%= GvData.ClientID %>");
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
                            Approve
                            <% If Session("CompID") = "1015" Then%>
                            EP
                            <%Else%>
                            Amount
                            <% End If%>
                            Request</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        <asp:RadioButtonList ID="RbReqStatus" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Table">
                                            <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="Approved" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkMem" runat="server" AutoPostBack="true" Text="Member Id:" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox runat="server" Enabled="false" ID="TxtMemID" class="form-control"></asp:TextBox></div>
                                    <div class="col-md-3">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <%--<div class="col-md-2">
                                        Request Date:</div>--%>
                                        <div class="col-md-3">
                                        <asp:Label ID="Label2" runat="server" Text="Choose Date: "></asp:Label>
                                        <asp:DropDownList ID="CmbType" runat="server" class="form-control">
                                        <asp:ListItem Selected="True" Value="A">All</asp:ListItem>
                                            <asp:ListItem Value="N">Request Date</asp:ListItem>
                                            <asp:ListItem Value="Y">Approve Date</asp:ListItem>
                                            <asp:ListItem Value="R">Reject Date</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <%--<div class="col-md-2">
                                        </div>--%>
                                    <div class="col-md-3">
                                    Choose Start Date:
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <%--<div class="col-md-2">
                                        </div>--%>
                                    <div class="col-md-3">
                                    choose End Date:
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <%--<div class="col-md-2">
                                    </div>--%>
                                </div>
                                <div class="col-md-12">
                                    <asp:Button ID="BtnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                    <asp:Button ID="btnshowall" Visible="false" runat="server" class="btn btn-primary"
                                        Text="Show All" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel"
                                        Visible="false" />
                                    <asp:Button ID="BtnExportCsv" Visible="false" runat="server" class="btn btn-primary"
                                        Text="Export To CSV" />
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                        Visible="false" />
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                        Visible="false" />
                                    <asp:Button ID="btnApproove" runat="server" class="btn btn-primary" Text="Approve"
                                        Visible="false" />
                                    <asp:Button ID="BtnRejects" runat="server" class="btn btn-primary" Text="Reject"
                                        Visible="false" />
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                        color: Red"></asp:Label>
                                </div>
                                <br />
                                <div id="DivRemark" runat="server" visible="false">
                                    <table id="TblRemark" runat="server" align="center" style="background-color: #13a89e;
                                        color: #ffffff; border-color: Black; border-width: 1px; margin-top: -10px;">
                                        <tr>
                                            <td align="left">
                                                <strong>Remark</strong>*
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtARemark" runat="server" TextMode="MultiLine" Style="color: Black"></asp:TextBox>
                                                 <%--<asp:requiredfieldvalidator id="Requiredfieldvalidator4" controltovalidate="TxtARemark" display="static"
          errormessage="Please Enter  Remark *" runat="server" />--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnApprove" runat="server" class="btn btn-primary" Text="Approve"
                                                    Visible="false" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" />
                                                <asp:Button ID="BtnReject" runat="server" Text="Reject " class="btn btn-primary"
                                                    Visible="false" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                 <asp:Label ID="LblARemark" runat="server" ForeColor="red" Visible =false ></asp:Label>
                                <span style="font-size: 10px; font-weight: bold; margin-bottom: 11px; margin-top: 8px;
                                    padding-left: 10px">Please Verify Page By Page</span>
                                <div align="center" class="col-md-12">
                                    <asp:Label ID="lblMsg" runat="server" Font-Size="13px" Visible="False"></asp:Label>
                                    <div style="margin-top: 15px; overflow: scroll;">
                                        <div align="left" class="col-md-6">
                                            <b>No. Of Request:-
                                                <asp:Label ID="lblReqs" runat="server" Text="0"></asp:Label></b>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <b>Total Amount :-
                                                <asp:Label ID="lblTotalAmont" runat="server" Text="0.00"></asp:Label></b>
                                                 <asp:Label ID="LblEmailno" runat="server" Text="" Visible ="false"></asp:Label>
                                        </div>
                                        
                                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                            GridLines="None" AllowPaging="True" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" PageSize="50" EmptyDataText="No data to display." Width="90%" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="CheckAll">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# Eval("EnableStatus") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                                                        <asp:Label ID="LblReqNo" runat="server" Text='<%# Eval("ReqNo") %>'></asp:Label>
                                                        <asp:Label ID="LblIdNo" runat="server" Text='<%# Eval("IdNo") %>'></asp:Label>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                        <asp:Label ID="LblPaymode" runat="server" Text='<%# Eval("PayMode") %>'></asp:Label>
                                                        <asp:Label ID="LblMobileno" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label>
                                                        <asp:Label ID="lblactype" runat="server" Text='<%# Eval("Actype") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IDNo" HeaderText="ID No." />
                                                <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                                <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                                                <asp:BoundField DataField="ReqDate" HeaderText="Request Date"></asp:BoundField>
                                                <asp:BoundField DataField="PayMode" HeaderText="Payment Mode"></asp:BoundField>
                                                <asp:BoundField DataField="ChqNo" HeaderText="Cheque/ TransactionNo"></asp:BoundField>
                                                <asp:BoundField DataField="ChequeDate" HeaderText="Cheque/ Transaction Date"></asp:BoundField>
                                                <asp:BoundField DataField="BankName" HeaderText="Bank Name"></asp:BoundField>
                                                <asp:BoundField DataField="BranchName" HeaderText="Branch  Name"></asp:BoundField>
                                                <asp:BoundField DataField="Amount" HeaderText="Amount"></asp:BoundField>
                                                <asp:BoundField DataField="Remarks" HeaderText="Member Remark"></asp:BoundField>
                                                <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                                                <asp:BoundField DataField="ApproveRemark" HeaderText="User  Remark" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a href='<%# "Img.aspx?&type=Payment&ID=" & Eval("Reqno") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 785,height: 580,marginTop : 0 } )">
                                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ScannedFile") %>' Height="80px"
                                                                Width="80px" Visible='<%# Eval("ScannedFileStatus") %>' />
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
    </div>
</asp:Content>
