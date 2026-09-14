<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="InvoiceVerified.aspx.vb" Inherits="InvoiceVerified" %>

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
    <style type="text/css">
/* The Modal (background) */
.modal {
  display: none; /* Hidden by default */
  position: fixed; /* Stay in place */
  z-index: 1; /* Sit on top */
  padding-top: 100px; /* Location of the box */
  left: 0;
  top: 0;
  width: 100%; /* Full width */
  height: 100%; /* Full height */
  overflow: auto; /* Enable scroll if needed */
  background-color: rgb(0,0,0); /* Fallback color */
  background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
}

/* Modal Content */
.modal-content {
  position: relative;
  background-color: #fefefe;
  margin: auto;
  padding: 0;
  border: 1px solid #888;
  width: 80%;
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
  -webkit-animation-name: animatetop;
  -webkit-animation-duration: 0.8s;
  animation-name: animatetop;
  animation-duration: 0.8s
}

/* Add Animation */
@-webkit-keyframes animatetop {
  from {top:-300px; opacity:0} 
  to {top:0; opacity:1}
}

@keyframes animatetop {
  from {top:-300px; opacity:0}
  to {top:0; opacity:1}
}

/* The Close Button */
.close {
  color: white;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: #000;
  text-decoration: none;
  cursor: pointer;
}

.modal-header {
  padding: 2px 8px;
  background-color: #ffffff;
  color: white;
}

.modal-body {padding: 2px 16px;}

.modal-footer {
  padding: 2px 8px;
  background-color: #ffffff;
  color: white;
}
    </style>
     <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
<%--    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet">--%>


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
                            Invoice Verify</h2>
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
                                        <asp:CheckBox ID="ChkMem" runat="server" Text="Member ID Wise :" Font-Bold="true" /></div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <strong>Verify Status: </strong>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="DDlVerify" runat="server" class="form-control">
                                            <asp:ListItem Text="All" Value="S" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="VERIFY" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="VERIFICATION DUE" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="REJECTED" Value="R"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <%--<div class="col-md-2">
                                        <strong>Status: </strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RadioButtonList ID="RbtSearch" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>--%>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                    
                                    <div class="col-md-1">
                                        <asp:Button runat="server" ID="BtnSearch" class="btn btn-primary" Text="Search" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button runat="server" ID="BtnExport" class="btn btn-primary" Text="Export To Excel"
                                            Enabled="false" /></div>
                                    <div class="col-md-1">
                                        <asp:Button ID="BtnVerifiy" runat="server" Text="Verification" class="btn btn-primary"
                                            Enabled="false" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="BTnUnVerification" runat="server" Text="Reject" class="btn btn-primary" /></div>
                                </div>
                                <div id="DivRemark" runat="server" visible="false">
                                    <table id="TblRemark" runat="server" align="center" style="background-color: #cceeff;
                                        color: #000000; border-color: Black; border-width: 1px; margin-top: -10px;">
                                       <%-- <tr>
                                            <td align="left">
                                                <br />
                                                <strong>Reason</strong>*
                                            </td>
                                            <td align="left">
                                                <br />
                                                <asp:DropDownList ID="DDlREason" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>--%>
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
                                <div class="col-md-12">
                                    <div class="col-md-10">
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
                                                        <asp:CheckBox ID="chkSelect" runat="server"  Enabled ='<%# Eval("EnableStatus") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                    <ItemTemplate>
                                                     <asp:Label ID="Lblid" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                                                        <asp:Label ID="LblIdno" runat="server" Text='<%# Eval("IdNo") %>'></asp:Label>
                                                        <asp:Label ID="Lblamount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex +1 %>.</ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IDNo" HeaderText="ID No." />
                                                <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                                                <asp:BoundField DataField="Doj" HeaderText="Date Of Joining" />
                                                <asp:BoundField DataField="Invoiceno" HeaderText="Invoice No." />
                                                <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                                   <asp:BoundField DataField="Date" HeaderText="Date" />
                                                <asp:TemplateField HeaderText="Invoice" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                       <%-- <asp:LinkButton ID="LBApprove" runat="server" Text="Show Image" OnClick="ApproveData" style="color:Black" data-toggle="modal" data-target="#modal_open"  href="javascript:void(0);"></asp:LinkButton>--%>
                                                         <a href='<%# "Img.aspx?ID=" & Eval("FormNo") & "&Type=Invoice&Reqid="& Eval("id") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                    <asp:Image ID="Image3" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("Invoiceurl")  %>' />
                                                </a>
                                                        <br />
                                                      
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Verify Date" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        Verify Status:
                                                        <asp:Label ID="LblIdVerify" runat="server" Text='<%#Eval("PanVerf") %>'></asp:Label>
                                                        <br />
                                                        Verify Date:
                                                        <asp:Label ID="LblVerifyDate" runat="server" Text='<%#Eval("PanVerifyDate") %>'></asp:Label>
                                                        <br />
                                                        <%-- Reject Remark:
                                                <asp:Label ID="LblRejectRemark" runat="server" Text='<%#Eval("VerifyRemark") %>'></asp:Label>
                                                <br />--%>
                                                        Processed By:
                                                        <asp:Label ID="LblProcessby" runat="server" Text='<%# Eval("VerifyBy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reject">
                                                    <ItemTemplate>
                                                        <strong>Reject Remark:</strong>
                                                        <asp:Label ID="LblRejectRemark" runat="server" Text='<%#Eval("VerifyRemark") %>'></asp:Label>
                                                        <br />
                                                        <%--<strong>Reject Reason:</strong>
                                                        <asp:Label ID="LblRejectReason" runat="server" Text='<%#Eval("RejectReason") %>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="PagerStyle " />
                                            <PagerSettings Mode="NumericFirstLast" />
                                        </asp:GridView>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="modal" id="myModal" style="margin-top: 100px">
                                            <div class="modal-dialog">
                                                <div class="modal-content">
                                                    <!-- Modal Header -->
                                                    <div class="modal-header">
                                                        <h4 class="modal-title">
                                                        </h4>
                                                        <button type="button" class="close" data-dismiss="modal">
                                                            &times;</button>
                                                    </div>
                                                    <!-- Modal body -->
                                                    <div class="modal-body">
                                                        <asp:Image ID="ImgPan" runat="server" />
                                                    </div>
                                                    <!-- Modal footer -->
                                                    <div class="modal-footer">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
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
