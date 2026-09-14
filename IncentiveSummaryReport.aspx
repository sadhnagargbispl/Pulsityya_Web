<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="IncentiveSummaryReport.aspx.vb" 
Inherits="App_UI_Application_Pages_IncentiveSummaryReport" title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="padding: 10px 10px 20px 10px">
            <table width="100%" align="left">
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Member ID Wise :" Font-Bold="true" />
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtMemId" runat="server" CssClass="TxtBox" Width="160px"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="CheckBox2" runat="server" Text="Session Wise :" Font-Bold="true" Checked="true" />
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSession" runat="server" CssClass="ddlSearch" Style="text-indent: 1px;"
                            Width="230px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="BtnShow" runat="server" CssClass="Btn" Text="Show Detail" />
                    </td>
                    <td>
                        <asp:Button ID="btnExport" runat="server" CssClass="Btn" Text="Export To Excel" />
                    </td>
                   
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 10px 10px 20px 10px" id="divSummary" runat="server" visible="false">
         <asp:GridView ID="GridView1" Width="100%" runat="server" AllowPaging="True" PageSize="200"
                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Vertical"
                Font-Size="12px" CssClass="table table-striped table-advance table-hover">
                <RowStyle BackColor="#F7F6F3" Height="29px" ForeColor="#333333" BorderWidth="1px"
                    BorderStyle="Dotted" BorderColor="Gray" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284779" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#1199b9" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                
                </Columns>
                </asp:GridView> 
        </div>
        
<div style="padding: 10px 10px 20px 10px" id="divDetail" runat="server" visible="false">
     
            <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="200"
                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Vertical"
                Font-Size="12px" CssClass="table table-striped table-advance table-hover">
                <RowStyle BackColor="#F7F6F3" Height="29px" ForeColor="#333333" BorderWidth="1px"
                    BorderStyle="Dotted" BorderColor="Gray" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284779" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#1199b9" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                   <asp:TemplateField HeaderText="CheckAll" >
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" Checked ="true"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("Status") %>'   />
<%--                            Enabled ='<%# Eval("IsSentSms").ToString().Equals("Sms Not sent") %>'--%>
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SNo.">
                        <ItemTemplate>
                         <asp:Label ID="LblFormNo" runat="server" Text='<%# Eval("FormNo") %>' Visible ="false"></asp:Label>
                         <asp:Label ID="LblSess" runat="server" Text='<%# Eval("SessId") %>' Visible ="false"></asp:Label> 
                       
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Session">
                        <ItemTemplate>
                            <asp:Label ID="StartDate" runat="server" Text='<%# Eval("PayoutDate") %>'></asp:Label>
                            <asp:Label ID="lblfromDate" runat="server" Text='<%# Eval("FromDate") %>' Visible="false"></asp:Label>
                       <asp:Label ID="lblTodate" runat="server" Text='<%# Eval("Todate") %>' Visible ="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Member Detail">
                        <ItemTemplate>
                            <strong>Member ID :</strong><br />
                            <asp:Label ID="LblMemId" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label><br />
                            <strong>Member Name :</strong><br />
                            <asp:Label ID="LblFullName" runat="server" Text='<%# Eval("MemName") %>'></asp:Label>
                            <br />
                            <strong>Mobile No. :</strong><br />
                            <asp:Label ID="LblMobl" runat="server" Text='<%# Eval("Mobl") %>'></asp:Label>
                            <br />
                            <asp:Label ID ="LblSentSms" runat="server" Text='<%# Eval("IsSentSms") %>' Visible ="True" Font-Bold ="true" ></asp:Label>
                     
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Matched Bv">
                        <ItemTemplate>
                        <strong>Matched Bv:</strong>
                        <asp:Label ID="LblMatchedBv" runat="server" Text='<%# Eval("LegXPaid") %>'></asp:Label>
                            <%--<strong>Bank Name :</strong><br />
                            <asp:Label ID="LblBankName" runat="server" Text='<%# Eval("BankName") %>'></asp:Label><br />
                            <strong>Account No. :</strong><br />
                            <asp:Label ID="LblAccountno" runat="server" Text='<%# Eval("AccountNo") %>'></asp:Label>
                            <br />
                            <strong>IFSCode :</strong><br />
                            <asp:Label ID="LblIFSC" runat="server" Text='<%# Eval("IFSCode") %>'></asp:Label><br />
                            <strong>PanNo :</strong><br />
                            <asp:Label ID="LblPan" runat="server" Text='<%# Eval("PanNo") %>'></asp:Label>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Incentive Detail">
                        <ItemTemplate>
                            <strong>Sales Matching Incentive :</strong><br />
                            <asp:Label ID="LblBinary" runat="server" Text='<%# Eval("BinaryIncome") %>'></asp:Label><br />
                        <strong>Magic Spill :</strong><br />
                            <asp:Label ID="LblDirectIncome" runat="server" Text='<%# Eval("SpillIncome") %>'></asp:Label><br />
                             <strong>Net Income :</strong><br />
                            <asp:Label ID="LblNetAmount" runat="server" Text='<%# Eval("NetIncome") %>'></asp:Label>
                            <asp:Label ID="LblNetAmount1" runat="server" Text='<%# Eval("NetIncome1") %>' Visible="false" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Deduction">
                        <ItemTemplate>
                            <strong>TDS Amount :</strong><br />
                            <asp:Label ID="LblTdsAmount" runat="server" Text='<%# Eval("TdsAmount") %>'></asp:Label><br />
                            <strong>Admin Charge :</strong><br />
                            <asp:Label ID="LblAdmin" runat="server" Text='<%# Eval("AdminCharge") %>'></asp:Label><br />            
                                 <strong>Repurchase Deduction :</strong><br />
                            <asp:Label ID="LblRepurchase" runat="server" Text='<%# Eval("CouponsAmt") %>'></asp:Label><br /> 
                               <strong>Loan Recovery :</strong><br />
                            <asp:Label ID="LblLoan" runat="server" Text='<%# Eval("LoanRecovery") %>'></asp:Label><br />                    
                            <strong>Total Deduction :</strong><br />
                            <asp:Label ID="LblTotaldeduct" runat="server" Text='<%# Eval("Deduction") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Net Payment">
                        <ItemTemplate>
                            <strong>Previous :</strong><br />
                            <asp:Label ID="LblPrevious" runat="server" Text='<%# Eval("PrevBal") %>'></asp:Label><br />
                            <strong>Net Amount :</strong><br />
                            <asp:Label ID="LblNetIncome" runat="server" Text='<%# Eval("ChqAmt") %>'></asp:Label><br />
                            <strong>Closing :</strong><br />
                            <asp:Label ID="LblClosing" runat="server" Text='<%# Eval("ClsBal") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
</asp:Content>

