<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="OldPayoutdetail.aspx.vb" Inherits="App_UI_Application_Pages_OldPayoutdetail"
    Title="" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  <style type="text/css">
    
.PagerStyle 
{
    background-image: url(../Images/td.jpg);
    background-position:center;
    background-repeat:repeat-x;
    background-color:#ffffff; 
    font-weight:bold;
    text-align: center;
    width: 00px;   
}

.PagerStyle table
{
	text-align:center;
    margin:auto;
}
.PagerStyle table td
{
    border:0px;
    padding:5px;
}
.PagerStyle td
{
    border-top: #1d1d1d 3px solid;
}
.PagerStyle a
{
    color:#000000;
    text-decoration:none;
    padding:2px 10px 2px 10px;
    border-top:solid 1px #777777;
    border-right:solid 1px #333333;
    border-bottom:solid 1px #333333;
    border-left:solid 1px #777777;
}
.PagerStyle span
{
    font-weight:bold;
    color:#000000;
    text-decoration:none;
    padding:2px 10px 2px 10px;
}



</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Daily Incentive Detail
                        </h2>
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
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Member ID Wise :" Font-Bold="true"
                                            Style="display: inline" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox></div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="CheckBox2" runat="server" Text="Session Wise :" Font-Bold="true"
                                            Checked="true" Style="display: inline" /></div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="DDlFromDate" runat="server" class="form-control" Style="display: inline">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="DDltodate" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="DdlSearch" runat="server" class="form-control">
                                            <asp:ListItem Text="Descending By Amount" Value="D" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Ascending By Amount" Value="A"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                               
                                    <div class="col-md-12">
                                    <br />
                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" /></div>
                                    <div class="col-md-12">
                                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label></div>
                                    <div style="padding: 10px 10px 20px 10px">
                                        <asp:GridView ID="GrdTotal" Width="100%" runat="server" AllowPaging="True" PageSize="200"
                                            GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Session">
                                                    <ItemTemplate>
                                                        <asp:Label ID="StartDate" runat="server" Text='<%# Eval("FromDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Member ID">
                        <ItemTemplate>
      <%--                     
                            <asp:Label ID="LblMemberId" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label><br />
                            
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Total Join">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblTotalJoin" runat="server" Text='<%# Eval("TotalJoin") %>'></asp:Label><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Active">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblTotalActive" runat="server" Text='<%# Eval("TotalActive") %>'></asp:Label><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total PV">
                                                    <ItemTemplate>
                                                        <%--<strong>Bank Name :</strong><br />--%>
                                                        <asp:Label ID="LblTotalPV" runat="server" Text='<%# Eval("TotalActivePv") %>'></asp:Label><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Match Pair ">
                                                    <ItemTemplate>
                                                        <%--<strong>Bank Name :</strong><br />--%>
                                                        <asp:Label ID="LblMatchPair" runat="server" Text='<%# Eval("MatchPair") %>'></asp:Label><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Pair Income">
                                                    <ItemTemplate>
                                                        <%--<strong>Bank Name :</strong><br />--%>
                                                        <asp:Label ID="LblTotalPair" runat="server" Text='<%# Eval("TotalBinary") %>'></asp:Label><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total TDS">
                                                    <ItemTemplate>
                                                        <%--<strong>Bank Name :</strong><br />--%>
                                                        <asp:Label ID="LblTotalTDS" runat="server" Text='<%# Eval("TDS") %>'></asp:Label><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Admin Charge">
                                                    <ItemTemplate>
                                                        <%--<strong>Bank Name :</strong><br />--%>
                                                        <asp:Label ID="LblTotalAdminCharge" runat="server" Text='<%# Eval("AdminCharge") %>'></asp:Label><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Paid">
                                                    <ItemTemplate>
                                                        <%--<strong>Bank Name :</strong><br />--%>
                                                        <asp:Label ID="LblTotalPaid" runat="server" Text='<%# Eval("PaidAmount") %>'></asp:Label><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closing Balance">
                                                    <ItemTemplate>
                                                        <%--<strong>Bank Name :</strong><br />--%>
                                                        <asp:Label ID="LblTotalClosing" runat="server" Text='<%# Eval("ClosingAmount") %>'></asp:Label><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <div style="padding: 10px 10px 20px 10px">
                                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="10"
                                            GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Session">
                                                    <ItemTemplate>
                                                        <asp:Label ID="StartDate" runat="server" Text='<%# Eval("PayoutDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Member Detail" ControlStyle-Width="200px" ItemStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <strong>Member ID :</strong><br />
                                                        <asp:Label ID="LblMemberId" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label><br />
                                                        <strong>Member Name :</strong><br />
                                                        <asp:Label ID="LblMemberName" runat="server" Text='<%# Eval("MemName") %>'></asp:Label>
                                                        <br />
                                                        <strong>Mobile No. :</strong><br />
                                                        <asp:Label ID="LblMobile" runat="server" Text='<%# Eval("Mobl") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Matched Bv">
                                                    <ItemTemplate>
                                                        <strong>Matched Bv:</strong>
                                                        <asp:Label ID="LblMatchedBv" runat="server" Text='<%# Eval("MatchedBv") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Incentive Detail">
                                                    <ItemTemplate>
                                                        <strong>Binary Income :</strong><br />
                                                        <asp:Label ID="LblBinaryIncome" runat="server" Text='<%# Eval("BinaryIncome") %>'></asp:Label><br />
                                                           <strong>Net Income :</strong><br />
                                                        <asp:Label ID="LblNetIncome" runat="server" Text='<%# Eval("NetIncome") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Deduction">
                                                    <ItemTemplate>
                                                        <strong>TDS Amount :</strong><br />
                                                        <asp:Label ID="LblTDSAmount" runat="server" Text='<%# Eval("TdsAmount") %>'></asp:Label><br />
                                                        <strong>Admin Charge :</strong><br />
                                                        <asp:Label ID="LblAdmin" runat="server" Text='<%# Eval("AdminCharge") %>'></asp:Label><br />
                                                         <strong>Repurchase :</strong><br />
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("CouponsAmt") %>'></asp:Label><br />
                                                              <strong>Total Deduction :</strong><br />
                                                        <asp:Label ID="LblTotalDeduction" runat="server" Text='<%# Eval("Deduction") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Net Payment">
                                                    <ItemTemplate>
                                                        <strong>Net Amount :</strong><br />
                                                        <asp:Label ID="LblNetIncome" runat="server" Text='<%# Eval("ChqAmt") %>'></asp:Label><br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle  CssClass ="PagerStyle" />
                                            <PagerSettings Mode="NumericFirstLast" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
</asp:Content>
