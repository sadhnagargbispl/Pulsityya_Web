<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EMIDeposit.aspx.vb" Inherits="EMIDeposit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript" language="javascript">
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
      
    </script>
    <title></title>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <%--  <link href="css/font-awesome.min.css" rel="stylesheet" />--%>
    <%--    <link href="css/custom.min.css" rel="stylesheet" />--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="dvMap" style="width: 100%; height: 500px">
            <div class="col-md-12">
                <div class="col-md-5">
                    <asp:Label ID="LblDeviceId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="LblRefno" runat="server" Visible="false"></asp:Label>
                     <asp:Label ID="LblFormNo" runat="server" Visible="false"></asp:Label>
                     <asp:Label ID="LblEmimonth" runat="server" Visible="false"></asp:Label>
                     
                </div>
            </div>
            <div class="col-md-12">
                <asp:GridView ID="GvData" runat="server" Width="100%" AllowPaging="false" AutoGenerateColumns="False"
                    class="table table-bordered" HeaderStyle-CssClass="bg-primary" RowStyle-Height="25px"
                    GridLines="Both">
                    <Columns>
                        <asp:TemplateField HeaderText="SNO">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1%>.</ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Amount" HeaderText="Amount" />
                        <asp:BoundField DataField="EmiMonth" HeaderText="EMI Month" />
                          <asp:BoundField DataField="PaidDate" HeaderText="Paid Date" />
                        <asp:TemplateField>
                            <ItemTemplate>
                            <asp:Label ID="LblAmount" runat="server" Visible ="false" Text='<%# Eval("Amount") %>'></asp:Label>
                                <asp:Label ID="LblFormno" runat="server" Text='<%# Eval("Formno") %>' Visible="false"></asp:Label>
                                <asp:Label ID="Reqno" runat="server" Text='<%# Eval("Reqno") %>' Visible="false"></asp:Label>
                                <asp:Label ID="LblRecieptNo" runat="server" Text='<%# Eval("RecieptNo") %>' Visible="false"></asp:Label>
                                <asp:Label ID="LblRefno" runat="server" Text='<%# Eval("RefNo") %>' Visible="false"></asp:Label>
                                <asp:TextBox ID="TxtRemark" runat="server" Text='<%# Eval("Remark") %>' Enabled ='<%# Eval("VisibleStatus") %>'></asp:TextBox>
                                <asp:TextBox ID="TextBox1" runat="server" Enabled ='<%# Eval("NewStatus") %>' Visible ="false" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href='<%# "Reciept.aspx?Key=" & Crypto.Encrypt(Eval("Reqno")) & "&RefNo=" &Crypto.Encrypt(Eval("RefNo")) & "&FormNo=" &Crypto.Encrypt(Eval("FormNO")) & "&EmiMonth="&Crypto.Encrypt(Eval("EmiMonth")) %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 970,height: 600,marginTop : 0 } )" visible ="false" target="_blank">
                                   <%-- <i class="icon-check-alt2"></i>--%>
                                    <asp:Label ID="LBModify" runat="server" Text="Print Reciept" OnClick="ViewDetails" Visible='<%# Eval("NewStatus") %>' />
                                   
                                   <%--<asp:LinkButton ID="LBview" runat="server" Text="View" OnClick="ViewDetails" Visible='<%# Eval("NewStatus") %>'
                                    CssClass="btn btn-primary"></asp:LinkButton>--%>
                                </a>
                                <asp:LinkButton ID="LBDelete" runat="server" Text="Pay" OnClick="DeleteGroup" Visible='<%# Eval("VisibleStatus") %>'
                                    CssClass="btn btn-primary" OnClientClick="return confirmation();"></asp:LinkButton>
                                    
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <script src="js/jquery.min.js"></script>

    <script src="js/bootstrap.min.js"></script>

    <script src="js/custom.min.js"></script>

    </form>
</body>
</html>
