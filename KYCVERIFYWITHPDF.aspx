<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="KYCVERIFYWITHPDF.aspx.vb" Inherits="KYCVERIFYWITHPDF" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .PagerStyle
        {
            background-image: url(../Images/td.jpg);
            background-position: center;
            background-repeat: repeat-x;
            background-color: #ffffff;
            font-weight: bold;
            text-align: center;
            width: 00px;
        }
        .PagerStyle table
        {
            text-align: center;
            margin: auto;
        }
        .PagerStyle table td
        {
            border: 0px;
            padding: 5px;
        }
        .PagerStyle td
        {
            border-top: #1d1d1d 3px solid;
        }
        .PagerStyle a
        {
            color: #000000;
            text-decoration: none;
            padding: 2px 10px 2px 10px;
            border-top: solid 1px #777777;
            border-right: solid 1px #333333;
            border-bottom: solid 1px #333333;
            border-left: solid 1px #777777;
        }
        .PagerStyle span
        {
            font-weight: bold;
            color: #000000;
            text-decoration: none;
            padding: 2px 10px 2px 10px;
        }
    </style>

    <script type="text/javascript">
        //$(document).ready(function() { $('[id$=chkSelectAll]').click(function() { $("[id$='chkSelect']").attr('checked', this.checked); }); });
        //    function reset() {
        //        $("[id$='chkSelect']").prop('checked', false);
        //    }

        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }

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
                            KYC Verify</h2>
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
                                    <div class="col-md-3">
                                        <strong>Member ID Wise :</strong>
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <strong>Status: </strong>
                                        <asp:DropDownList ID="RbtSearch" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                            class="form-control">
                                            <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <strong>Verify Status: </strong>
                                        <asp:DropDownList ID="DDlVerify" runat="server" class="form-control">
                                            <asp:ListItem Text="VERIFICATION DUE" Value="N" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="VERIFY" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="REJECTED" Value="R"></asp:ListItem>
                                            <asp:ListItem Text="All" Value="S"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <%-- <div class="col-md-3">
                                        <strong>Select Date: </strong>
                                        <asp:DropDownList ID="DDlDateselect" runat="server" class="form-control">
                                            <asp:ListItem Text="Verify Date" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Reject Date" Value="R"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>--%>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Start Date: "></asp:Label>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblEndDate" runat="server" Text="End Date : "></asp:Label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-4" style="padding: 1%">
                                        <asp:Button runat="server" ID="BtnSearch" class="btn btn-primary" Text="Search" />
                                        <asp:Button runat="server" ID="BtnExport" class="btn btn-primary" Text="Export To Excel"
                                            Enabled="false" />
                                        <asp:Button ID="BtnVerifiy" runat="server" Text="Verification" class="btn btn-primary"
                                            Enabled="false" />
                                        <asp:Button ID="BTnUnVerification" runat="server" Text="Reject" class="btn btn-primary" />
                                    </div>
                                </div>
                                <div id="DivRemark" runat="server" visible="false">
                                    <table id="TblRemark" runat="server" align="center" style="background-color: #cceeff;
                                        color: #000000; border-color: Black; border-width: 1px; margin-top: -10px;">
                                        <tr>
                                            <td align="left">
                                                <br />
                                                <strong>Reason</strong>*
                                            </td>
                                            <td align="left">
                                                <br />
                                                <asp:DropDownList ID="DDlREason" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <br />
                                                <strong>Remark</strong>*
                                            </td>
                                            <td align="left">
                                                <br />
                                                <asp:TextBox ID="TxtARemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <br />
                                                <asp:Button ID="BtnUnVerify" runat="server" class="btn btn-primary" Text="Reject"
                                                    OnClientClick="return confirmation();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Label ID="LblARemark" runat="server" ForeColor="red" Visible="false"></asp:Label>
                                <div class="col-md-12" style="overflow: scroll">
                                    <br />
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="None" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                        PageSize="20" EmptyDataText="No data to display.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="CheckAll">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                                                    <asp:Label ID="LblIdno" runat="server" Text='<%# Eval("IdNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex +1 %>.</ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IDNo" HeaderText="ID No." />
                                            <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                                            <asp:BoundField DataField="Doj" HeaderText="Date Of Joining" />
                                            <asp:BoundField DataField="ActivationDate" HeaderText=" Date Of Activation" />
                                            <asp:BoundField DataField="IdType" HeaderText="IdType" />
                                            <asp:BoundField DataField="IdProofNo" HeaderText="Address Proof No" />
                                            <asp:BoundField DataField="City" HeaderText="City" />
                                            <asp:BoundField DataField="District" HeaderText="District" />
                                            <asp:BoundField DataField="statename" HeaderText="State" />
                                            <asp:BoundField DataField="Address1" HeaderText="Address" />
                                            <asp:BoundField DataField="Pincode" HeaderText="Pincode" />
                                            <asp:BoundField DataField="Bankname" HeaderText="Bank Name" />
                                            <asp:BoundField DataField="Acno" HeaderText="Account No" />
                                            <asp:BoundField DataField="Branchname" HeaderText="Branch Name" />
                                            <asp:BoundField DataField="Ifscode" HeaderText="IFSC Code" />
                                            <asp:BoundField DataField="Panno" HeaderText="Pan No." />
                                            <asp:TemplateField HeaderText="Front Address Proof" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <a href='<%# "Img.aspx?ID=" & Eval("FormNo") & "&Type=FrontAddress" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                        <asp:Image ID="Image3" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("AddressproofStatus")  %>' />
                                                    </a>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Back Address Proof" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <a href='<%# "Img.aspx?ID=" & Eval("FormNo") & "&Type=BackAddress" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                        <asp:Image ID="Image4" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("BackAdressProof")  %>' />
                                                    </a>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bank Proof" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <a href='<%# "Img.aspx?ID=" & Eval("FormNo") & "&Type=BankProof" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                        <asp:Image ID="ImageBank" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("BankProofStatus")  %>' />
                                                    </a>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pancard" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <a href='<%# "Img.aspx?ID=" & Eval("FormNo") & "&Type=Pancard" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                        <asp:Image ID="Imagepan" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("PanproofStatus")  %>' />
                                                    </a>
                                                    <br />
                                                    <asp:Label ID="Lbldate1" runat="server" Text='<%#Eval("PanProofDate") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Direct Seller Agreement" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <a href='<%# Eval("FrontSideForm") %>' class="btn btn-sm btn-success" target="_blank"
                                                        rel="noopener noreferrer">VIEW PDF </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Uploaded Address Proof Date" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <strong>Verify Detail:</strong>
                                                    <asp:Label ID="LblIdVerify" runat="server" Text='<%#Eval("AddrssVerf") %>'></asp:Label>
                                                    <br />
                                                    <strong>Verify Date</strong>
                                                    <asp:Label ID="LblVerifyDate" runat="server" Text='<%# Eval("AddressVerifyDate") %>'></asp:Label>
                                                    <strong>Processed By:</strong>
                                                    <asp:Label ID="LblProcessby" runat="server" Text='<%# Eval("VerifyBy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reject">
                                                <ItemTemplate>
                                                    <strong>Reject Remark:</strong>
                                                    <asp:Label ID="LblRejectRemark" runat="server" Text='<%#Eval("RejectRemark") %>'></asp:Label>
                                                    <br />
                                                    <strong>Reject Reason:</strong>
                                                    <asp:Label ID="LblRejectReason" runat="server" Text='<%#Eval("RejectReason") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="PagerStyle " />
                                        <PagerSettings Mode="NumericFirstLast" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
