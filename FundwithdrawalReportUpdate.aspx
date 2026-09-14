<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="FundwithdrawalReportUpdate.aspx.vb" Inherits="FundwithdrawalReportUpdate"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        /* ── Wallet Authentication – redesigned styles only ── */.wa-page
        {
            padding: 20px;
            font-family: 'Segoe UI' , Arial, sans-serif;
        }
        /* Panel */.wa-panel
        {
            background: #fff;
            border: 1px solid #e0e4ea;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 1px 4px rgba(0,0,0,0.06);
        }
        /* Title bar */.wa-title-bar
        {
            background: #1a4a8a;
            padding: 12px 20px;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        .wa-title-bar h2
        {
            margin: 0;
            font-size: 16px;
            font-weight: 600;
            color: #fff;
            letter-spacing: 0.3px;
        }
        /* Alert */#lblt
        {
            display: block;
            margin: 10px 20px 0;
            font-size: 13px;
        }
        /* Filter strip */.wa-filter-strip
        {
            display: flex;
            align-items: center;
            flex-wrap: wrap;
            gap: 10px;
            padding: 16px 20px 12px;
            border-bottom: 1px solid #e8ecf0;
            background: #f8fafc;
        }
        .wa-filter-group
        {
            display: flex;
            align-items: center;
            gap: 7px;
        }
        .wa-filter-sep
        {
            width: 1px;
            height: 28px;
            background: #d4dae3;
        }
        .wa-filter-strip .form-control
        {
            height: 32px;
            font-size: 13px;
            border: 1px solid #cdd3db;
            border-radius: 5px;
            padding: 0 9px;
            background: #fff;
            color: #333;
            outline: none;
            transition: border-color 0.15s;
            min-width: 130px;
        }
        .wa-filter-strip .form-control:focus
        {
            border-color: #1a4a8a;
        }
        .wa-chk-label
        {
            font-size: 13px;
            font-weight: 600;
            color: #3a4a5c;
            display: flex;
            align-items: center;
            gap: 5px;
            cursor: pointer;
            white-space: nowrap;
        }
        .wa-radio-group
        {
            display: flex;
            align-items: center;
            gap: 12px;
        }
        .wa-radio-group label
        {
            font-size: 13px;
            color: #3a4a5c;
            display: flex;
            align-items: center;
            gap: 4px;
            cursor: pointer;
        }
        /* Force ASP RadioButtonList to render inline horizontally */#RbtStatus
        {
            display: flex !important;
            align-items: center;
            gap: 12px;
        }
        #RbtStatus tr, #RbtStatus td
        {
            display: inline-flex !important;
            align-items: center;
            gap: 4px;
        }
        #RbtStatus table
        {
            display: inline-flex !important;
            gap: 12px;
        }
        #RbtStatus label
        {
            font-size: 13px;
            color: #3a4a5c;
            cursor: pointer;
            white-space: nowrap;
        }
        #RbtStatus input[type=radio]
        {
            accent-color: #1a4a8a;
            width: 14px;
            height: 14px;
        }
        /* Button strip */.wa-btn-strip
        {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
            padding: 12px 20px 14px;
            border-bottom: 1px solid #e8ecf0;
            background: #fff;
            align-items: center;
        }
        .wa-btn-strip .btn
        {
            height: 33px;
            padding: 0 16px;
            font-size: 13px;
            font-weight: 500;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            transition: opacity 0.15s, transform 0.1s;
            letter-spacing: 0.1px;
        }
        .wa-btn-strip .btn:hover
        {
            opacity: 0.88;
        }
        .wa-btn-strip .btn:active
        {
            transform: scale(0.98);
        }
        .btn-show
        {
            background: #1a4a8a;
            color: #fff;
        }
        .btn-export
        {
            background: #0f7048;
            color: #fff;
        }
        .btn-hdfc
        {
            background: #3c3489;
            color: #fff;
        }
        .btn-nonhdfc
        {
            background: #6b4fa8;
            color: #fff;
        }
        .btn-approve
        {
            background: #217a3c;
            color: #fff;
        }
        .btn-reject
        {
            background: #b02a2a;
            color: #fff;
        }
        /* Error label */#lblError
        {
            font-size: 13px;
        }
        /* Pop-up panels (DivTopup / DivRemark) */#DivTopup table, #DivRemark table
        {
            border-radius: 8px;
            overflow: hidden;
            border: 1px solid #394a59;
            font-size: 13px;
        }
        #DivTopup td, #DivRemark td
        {
            padding: 7px 12px;
        }
        #DivTopup input[type=text], #DivTopup textarea, #DivRemark input[type=text], #DivRemark textarea
        {
            border: 1px solid #6a7f92;
            border-radius: 4px;
            padding: 4px 8px;
            background: #2e3d4a;
            color: #d0d8df;
            font-size: 13px;
            outline: none;
        }
        .buttonBG
        {
            background: #1a4a8a;
            color: #fff;
            border: none;
            border-radius: 4px;
            padding: 4px 14px;
            font-size: 13px;
            cursor: pointer;
        }
        .buttonBG:hover
        {
            background: #15397a;
        }
        /* Grid wrapper */.wa-grid-wrap
        {
            padding: 16px 20px 20px;
            overflow-x: auto;
        }
        /* GridView overrides */#GvData
        {
            width: 100%;
            border-collapse: collapse;
            font-size: 12px;
            color: #333;
        }
        #GvData th
        {
            background: #1a4a8a !important;
            color: #fff !important;
            font-weight: 600;
            font-size: 12px;
            padding: 8px 10px;
            text-align: left;
            white-space: nowrap;
            border: 1px solid #163d75;
        }
        #GvData td
        {
            padding: 6px 10px;
            border: 1px solid #dde2e8;
            vertical-align: middle;
        }
        #GvData tr:nth-child(even) td
        {
            background: #f5f7fa;
        }
        #GvData tr:hover td
        {
            background: #eaf0fb;
        }
        /* ── Pagination row ── */#GvData tr[align="center"] td, #GvData .GridPager td
        {
            background: #f8fafc !important;
            border-top: 1px solid #e0e4ea !important;
            border-bottom: none !important;
            border-left: none !important;
            border-right: none !important;
            padding: 10px 12px !important;
            text-align: center;
        }
        /* Page number links */#GvData tr[align="center"] td a, #GvData .GridPager td a
        {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-width: 30px;
            height: 30px;
            padding: 0 8px;
            margin: 0 2px;
            border: 1px solid #cdd3db;
            border-radius: 5px;
            background: #fff;
            color: #1a4a8a;
            font-size: 12px;
            font-weight: 500;
            text-decoration: none;
            transition: background 0.15s, border-color 0.15s;
        }
        #GvData tr[align="center"] td a:hover, #GvData .GridPager td a:hover
        {
            background: #1a4a8a;
            color: #fff;
            border-color: #1a4a8a;
        }
        /* Current page (rendered as plain text span) */#GvData tr[align="center"] td span, #GvData .GridPager td span
        {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-width: 30px;
            height: 30px;
            padding: 0 8px;
            margin: 0 2px;
            border: 1px solid #1a4a8a;
            border-radius: 5px;
            background: #1a4a8a;
            color: #fff;
            font-size: 12px;
            font-weight: 600;
        }
        /* LinkButtons inside grid (non-pager) */#GvData a
        {
            color: #1a4a8a;
            font-weight: 500;
            text-decoration: none;
        }
        #GvData a:hover
        {
            text-decoration: underline;
        }
        /* Inline remark textbox */#GvData input[type=text]
        {
            border: 1px solid #cdd3db;
            border-radius: 4px;
            padding: 2px 6px;
            font-size: 12px;
            width: 110px;
        }
    </style>

    <script type="text/javascript">
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        window.addEventListener('DOMContentLoaded', function () {
            /* Force RadioButtonList table cells inline so they render horizontally */
            var rbl = document.getElementById('<%= RbtStatus.ClientID %>');
            if (rbl) {
                var tbl = rbl.querySelector('table');
                if (tbl) {
                    tbl.style.cssText = 'display:inline-flex;gap:14px;align-items:center;';
                    var tds = tbl.querySelectorAll('td');
                    tds.forEach(function(td) {
                        td.style.cssText = 'display:inline-flex;align-items:center;gap:4px;padding:0;';
                    });
                } else {
                    /* Flow layout — just ensure no block display */
                    rbl.style.display = 'flex';
                    rbl.style.gap = '14px';
                    rbl.style.alignItems = 'center';
                }
            }
        });
    </script>

    <script type="text/javascript">
        function SelectAll(id) {
            var grid = document.getElementById("<%=GvData.ClientID%>");
            var cell;
            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[0];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>

    <style type="text/css">
        /* ── Pagination (ID-independent) ── */
        .GridPager td
        {
            background: #f8fafc !important;
            border: none !important;
            padding: 12px 0 !important;
            text-align: center;
        }
        /* numeric pager ek nested table mein aata hai — uske cells neutralize karo */
        .GridPager td table
        {
            margin: 0 auto;
            border-collapse: separate;
        }
        .GridPager td table td
        {
            border: none !important;
            background: transparent !important;
            padding: 0 !important;
        }
        /* page links + current page (span) ko box + spacing do */
        .GridPager a, .GridPager span
        {
            display: inline-block;
            min-width: 32px;
            height: 32px;
            line-height: 30px;
            padding: 0 10px;
            margin: 0 4px; /* yahi "123" ko alag karega */
            border: 1px solid #cdd3db;
            border-radius: 5px;
            text-align: center;
            font-size: 12px;
            font-weight: 500;
            text-decoration: none;
        }
        .GridPager a
        {
            background: #fff;
            color: #1a4a8a;
        }
        .GridPager a:hover
        {
            background: #1a4a8a;
            color: #fff;
            border-color: #1a4a8a;
        }
        .GridPager span
        {
            /* current/active page */
            background: #1a4a8a;
            color: #fff;
            border-color: #1a4a8a;
            font-weight: 600;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col wa-page" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="wa-panel">
                    <!-- Title bar -->
                    <div class="wa-title-bar">
                        <h2>
                            Wallet Authentication</h2>
                    </div>
                    <div class="panel-body" style="padding: 0;">
                        <!-- Alert row -->
                        <div style="padding: 0 20px;" align="center">
                            <span id="lblt" class="text-danger"></span>
                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        </div>
                        <!-- ── Filter strip ── -->
                        <div class="wa-filter-strip">
                            <div class="wa-filter-group">
                                <label class="wa-chk-label">
                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                    Member ID Wise
                                </label>
                                <asp:TextBox ID="txtMemId" runat="server" class="form-control" placeholder="Member ID"></asp:TextBox>
                            </div>
                            <div class="wa-filter-sep">
                            </div>
                            <div class="wa-filter-group">
                                <label class="wa-chk-label">
                                    <asp:CheckBox ID="CheckBox2" runat="server" Checked="true" />
                                    Date Wise
                                </label>
                                <asp:DropDownList ID="ddlSession" runat="server" class="form-control" Style="text-indent: 1px;">
                                </asp:DropDownList>
                            </div>
                            <div class="wa-filter-sep">
                            </div>
                            <div class="wa-radio-group">
                                <asp:RadioButtonList ID="RbtStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Text="All" Value="N" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Approve" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <!-- ── Button strip ── -->
                        <div class="wa-btn-strip">
                            <asp:Button ID="BtnShow" runat="server" class="btn btn-show" Text="Show Detail" />
                            <asp:Button ID="btnExport" runat="server" class="btn btn-export" Text="Export To Excel" />
                            <asp:Button ID="BtnExportToCsv" runat="server" class="btn btn-export" Text="HDFC Excel" />
                            <asp:Button ID="Button1" runat="server" class="btn btn-export" Text="Non HDFC Excel" />
                            <asp:Button ID="BtnApproveAll" runat="server" class="btn btn-export" Text="Approve All" />
                            <asp:Button ID="btnRejectAll" runat="server" class="btn btn-export" Text="Reject All" />
                            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                        </div>
                        <!-- ── Topup detail panel ── -->
                        <div id="DivTopup" runat="server" visible="false" style="width: 100%; padding: 12px 20px;">
                            <center>
                                <table align="center" border="1" style="background-color: #394a59; color: #d0d8df;">
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
                                                <strong>Cheque Number</strong>
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
                                        <td style="height: 26px;">
                                        </td>
                                        <td style="height: 26px;" align="left">
                                            <asp:Button ID="btnConfirm" runat="server" Text="Confirm" class="buttonBG" Style="height: 24px;
                                                width: 64px;" ValidationGroup="confirm" />
                                            &nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="buttonBG" Style="height: 24px;
                                                width: 64px;" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                        <!-- ── Remark/Reject panel ── -->
                        <div id="DivRemark" runat="server" visible="false" style="padding: 12px 20px;">
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
                                        <asp:Button ID="btnReject" runat="server" Text="Reject " class="buttonBG" Visible="false" />
                                        <asp:Button ID="btnRejectSingle" runat="server" Text="Reject" />
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
                        <!-- ── GridView ── -->
                        <div class="wa-grid-wrap">
                            <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" Font-Size="12px"
                                GridLines="Both" HeaderStyle-CssClass="bg-primary" class="table table-bordered"
                                ClientIDMode="Static">
                                <PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
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
                                            <%# Container.DataItemIndex + 1 %></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="LblWeek" runat="server" Text='<%# Eval("WeekNo") %>'></asp:Label>
                                            <asp:Label ID="LblID" runat="server" Text='<%# Eval("ReqID") %>'></asp:Label>
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
                                    <asp:TemplateField HeaderText="Withdrawal Date">
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
                                    <%--<asp:TemplateField HeaderText="Admin Charge">
                        <ItemTemplate>
                            <asp:Label ID="LblAdminCharge" runat="server" Text='<%# Eval("AdminCharge") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                    <%-- <asp:TemplateField HeaderText="Net Amount">
                        <ItemTemplate>
                            <asp:Label ID="LblNetAmount" runat="server" Text='<%# Eval("NetAmount") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtRemarks" runat="server" Style="display: inline" Text='<%# Eval("Remark") %>'
                                                Enabled='<%# Eval("IsVisible") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bank Name">
                                        <ItemTemplate>
                                            <asp:Label ID="LblBankname" runat="server" Text='<%# Eval("BankName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Branch Name">
                                        <ItemTemplate>
                                            <asp:Label ID="LblBranch" runat="server" Text='<%# Eval("BranchName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account No.">
                                        <ItemTemplate>
                                            <asp:Label ID="LblAccountNo" runat="server" Text='<%# Eval("AcNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IFSC Code">
                                        <ItemTemplate>
                                            <asp:Label ID="LblIFsCode" runat="server" Text='<%# Eval("IFsCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PanNo" HeaderText="PanNo" />
                                    <asp:TemplateField HeaderText="Cheque No">
                                        <ItemTemplate>
                                            <asp:Label ID="LblChequeNo" runat="server" Text='<%# Eval("ChequeNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cheque Date">
                                        <ItemTemplate>
                                            <asp:Label ID="LblChequeDate" runat="server" Text='<%# Eval("ChequeDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:TemplateField HeaderText="Approve" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LBApprove" runat="server" Text="Approve" OnClientClick="return confirmation();"
                                                OnClick="ApproveData" Visible='<%# Eval("IsVisible") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reject" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LBReject" runat="server" Text="Reject" OnClientClick="return confirmation();"
                                                OnClick="RejectData" Visible='<%# Eval("IsVisible") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProcessedBy" HeaderText="Processed By" />
                                    <asp:BoundField DataField="ProcessedDate" HeaderText="Processed Date" />
                                    <asp:BoundField DataField="WeekNo" HeaderText="Week No." />
                                    <asp:BoundField DataField="Fromdate" HeaderText="FromDate" />
                                    <asp:BoundField DataField="ToDate" HeaderText="ToDate" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <%-- /panel-body --%>
                </div>
                <%-- /wa-panel --%>
                <br />
            </div>
        </div>
    </div>
</asp:Content>
