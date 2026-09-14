<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="TopupDetailVerify.aspx.vb" Inherits="TopUpDetailVerify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                          Product Request Approve</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div class="col-md-12">
                            <div class="col-md-2">
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Member ID Wise :" Font-Bold="true" />
                        &nbsp;&nbsp;
                   </div>
                   <div class="col-md-3">
                        <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" />
                    
                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                  </div>
                   <div class="col-md-3"></div>
               <div class="col-md-12">
                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                   
        </div>
        <br />
        <center>
            <div id="DivRemark" runat="server" visible="false">
                <table id="TblRemark" runat="server" align="center" style="background-color: #5AA9CA;
                    color: #fff; border-color: Black; border-width: 1px; margin-top: -10px; margin-bottom: -40px">
                    <tr>
                        <td align="left">
                            <strong>Idno</strong>*
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TxtIdno" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Remark</strong>*
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TxtARemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                            <asp:Label ID="LblOrderNo" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="LblMemId" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="LblPlanId" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="BtnReject" runat="server" class="buttonBG" Style="height: 24px;"
                                Text="Reject" Visible="false" OnClientClick="return confirm('Are you sure you want to continue')">
                            </asp:Button>
                            <asp:Button ID="btnApprove" runat="server" class="buttonBG" Style="height: 24px;"
                                Text="Approve " Visible="false" OnClientClick="return confirm('Are you sure you want to continue')" />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <br />
       
            <div  class="col-md-12" style="padding: 10px 10px 20px 10px">
                <asp:GridView ID="GvData" Width="100%" runat="server" RowStyle-Height="25px" GridLines="Both"
                                    AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" PageSize="10" EmptyDataText="No data to display." AutoGenerateColumns ="false">
                    <Columns>
                        <asp:TemplateField HeaderText="SNo.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LblFormno" runat="server" Text='<%# Eval("FormNo") %>' Visible="false"></asp:Label>
                                <asp:Label ID="LblOrderNo" runat="server" Text='<%# Eval("OrderNo") %>' Visible="false"></asp:Label>
                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("Idno") %>'></asp:Label>
                                <asp:Label ID="LblPlanId" runat="server" Text='<%# Eval("PlanId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Idno" HeaderText="Idno" />
                        <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                        <asp:BoundField DataField="Packagename" HeaderText="Package Name" />
                        <asp:BoundField DataField="KitAmount" HeaderText="Package Amount" />
                       <%-- <asp:BoundField DataField="PayMode" HeaderText="PaymentMode" />
                        <asp:BoundField DataField="ChDDNo" HeaderText="Mode No" />
                        <asp:BoundField DataField="BankName" HeaderText="BankName" />
                        <asp:BoundField DataField="BranchName" HeaderText="BranchName" />--%>
                        <asp:BoundField DataField="Qty" HeaderText="Quantity" /> 
                        <asp:BoundField DataField="DepositDate" HeaderText="DepositDate" />
                        <asp:BoundField DataField="Amount" HeaderText="TotalAmount" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label><br />
                                <asp:Label ID="LblReamrk" runat="server" Text='<%# Eval("Remark") %>'></asp:Label><br />
                                <asp:Label ID="LblAppDate" runat="server" Text='<%# Eval("ApproveDate") %>'></asp:Label><br />
                            </ItemTemplate>
                        </asp:TemplateField>
                    <%--    <asp:TemplateField HeaderText="View Product Detail" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <a href='<%# "ProductRequestDetail.aspx?IdNo="&Eval("IdNo")&"&OrderNo="& Eval("OrderNo")%>'
                                    onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 460,height: 430,marginTop : 0 } )">
                                    View Detail </a>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Approve" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="LBApprove" runat="server" Text="Approve" OnClick="ApproveData"
                                    Visible='<%# Eval("IsVisible") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reject" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="LBReject" runat="server" Text="Reject" OnClick="RejectData" Visible='<%# Eval("IsVisible") %>'></asp:LinkButton>
                                <%-- <asp:LinkButton ID="LBnotReject" runat="server" Text=" Not Reject" OnClientClick="return confirmation();"
                                OnClick="Rejected" Visible='<%# Eval("IsReject") %>'></asp:LinkButton>--%>
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
        <div class="row">
            <!-- end of weather widget -->
        </div>
</asp:Content>
