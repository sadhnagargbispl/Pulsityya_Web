<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="ProductRequestDeactiveDetail.aspx.vb" Inherits="ProductRequestDeactiveDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript" language="javascript">
//        function isNumberKey1(evt) {
//            var charCode = (evt.which) ? evt.which : event.keyCode
//            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
//                return false;

//            return true;
//        }
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
         
    </script>
    <%--<div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">--%>
        <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
            <div class="x_title">
                        <h2>
                        <% If Session("Compid") = "1056" Then %>
                          Orbit Rank Achiever Report
                        <% Else%>
                          Product Request Report
                        <% End If%>
                          
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
           <%-- <div class="clearfix gen-profile-box" style="min-height: auto;">--%>
           <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <div class="table-responsive makeitresponsivegrid" style="min-height: 500px;">
                                <div align="center">
                                    <div class="col-md-12">
                                        <div class="row">
                <%--<div class="profile-bar clearfix" style="background: #fff;">--%>
                    <%--<div class="clearfix">
                        <br>
                        <div class="centered">--%>
                            <div class="clr">
                               <%-- <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>--%>
                               <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                            color: Red"></asp:Label>
                            </div>
                            <div class="clr">
                            </div>
                            <%--<div class="form-horizontal">
                                <div class="table-responsive">--%>
                                <div class="col-md-2">
                                                <asp:Label ID="Label1" runat="server" Text="Member ID  "></asp:Label>
                                                <asp:TextBox ID="txtMemberID" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" Text="From Date  "></asp:Label>
                                <asp:TextBox ID="txtfrmdate" runat="server" class="form-control" ></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtfrmdate"
                                                    Format="dd-MMM-yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtfrmdate"
                                                    ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                                </div> 
                                                <div class="col-md-2">
                                           <asp:Label ID="Label4" runat="server" Text="To Date  "></asp:Label>
                                                <asp:TextBox ID="txttodate" runat="server" class="form-control" ></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txttodate"
                                                    Format="dd-MMM-yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txttodate"
                                                    ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                                </div> 
                                                <div class="col-md-2">
                                            <asp:Button ID="btnsubmit" runat ="server" class="btn btn-primary" Text="Search" />    
                                           <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" Visible="false" />
                                           </div> 
                                          <%-- </div>--%>
                                 </div> 
                            </div>     
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                            AutoGenerateColumns="False" ForeColor="Black" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            EmptyDataText="No data to display." GridLines="None">
                                            <Columns>
                                                <asp:TemplateField HeaderText=" OrderNo/Bill NO." Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("Orderno") %>' ></asp:Label>
                                                    <%-- <asp:Label ID="lblformno" runat="server" Text='<%# Eval("formno") %>'></asp:Label>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S.No.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>.
                                                </ItemTemplate>
                                            </asp:TemplateField>
                           <%--<asp:BoundField DataField="Idno" HeaderText="Idno" />--%>
                            <asp:TemplateField HeaderText="Idno">
                                                <ItemTemplate>
                                                      <asp:Label ID="lblidno" runat="server" Text='<%# Eval("idno") %>'></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                           <asp:BoundField DataField="Member Name" HeaderText="Member Name" />
                           <asp:BoundField DataField="Mobile No" HeaderText="Mobile No" />
                                            <asp:BoundField DataField="Orderno" HeaderText="Order No./Bill NO." />
                                            <asp:BoundField DataField="OrderDate" HeaderText="Order Date" />
                                            <%--<asp:BoundField DataField="productname" HeaderText="Product Name" />--%>
                                            <asp:BoundField DataField="OrderAmount" HeaderText="Order Amount" />
                                            
                                            <asp:BoundField DataField="Part Payment By Order" HeaderText=" Part Payment By Order" />
                                            <asp:BoundField DataField="OtherAmt" HeaderText="Deposit by wallet" />
                                            <asp:BoundField DataField="Amount Rec. By Payout" HeaderText="Amount Rec. By Payout" />
                                            <asp:BoundField DataField="Total Recieved Amount" HeaderText="Total Recieved Amount" />
                                            <asp:BoundField DataField="Required Amount" HeaderText=" Required Amount" />
                                                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" Visible='<%# Eval("VisibleStatus") %>' OnClick="DeleteGroup" OnClientClick="return confirmation();"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                                    <%--<asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup" OnClientClick="return confirmation();"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>--%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign = "Right" CssClass = "pagination-ys" />
                                        </asp:GridView>
                                            <%--<table id="customers2" class="table table-bordered table-striped table-actions">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            SNo
                                                        </th>
                                                        <th>
                                                            OrderNo/Bill NO.
                                                        </th>
                                                        <th>
                                                            Order Date
                                                        </th>
                                                       
                                                        <th>
                                                            Order Amount
                                                        </th>
                                                        <th>
                                                            Wallet Amount
                                                        </th>
                                                        <th>
                                                            Required Amount
                                                        </th>
                                                        <th style =" display : none;">
                                                        <% If Session("CompID") = "1056" Then%>
                                                             PV
                                                             <% Else%>
                                                             <%=Session("ColName2")%>
                                                             <% End If%>
                                                        </th>
                                                        
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="RptDirects" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Orderno")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("OrderDate")%>
                                                                </td>
                                                                
                                                                <td>
                                                                    <%#Eval("OrderAmount")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("WalletAmt")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("RemainingAmt")%>
                                                                </td>
                                                                <td style="display :none;">
                                                                <%If Session("CompID") = "1056" Then%>
                                                                <%#Eval("BV")%>
                                                                <%Else%>
                                                                <%#Eval("BV")%>
                                                                <%End If%>
                                                                    
                                                                    
                                                                </td>
                                                                
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>--%>
                                            </div> </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                        </div>
                    </div>
                </div>
                </div>
                                    </div>
                                    <br />
            </div>
        </div>
    </div>
</div>
</div>
    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <script type="text/javascript" src="assets/jquery.dataTables.min.js"></script>

    <script type="text/javascript" src="assets/tableExport.js"></script>

    <script type="text/javascript">
        var jq = $.noConflict();
        function pageLoad(sender, args) {

            jq(document).ready(function() {
                jq('#customers2').DataTable();

            });
        }


    </script>

</asp:Content>
