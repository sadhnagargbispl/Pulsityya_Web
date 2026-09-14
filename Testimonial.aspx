

<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="Testimonial.aspx.vb" Inherits="App_UI_Application_Pages_Testimonial"
    Title="" EnableEventValidation="false" %>

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="images/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Testimonial Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
    <div style="height:100%;padding-left:10px">
   
    
    <table>
    <tr>
    <td>   <asp:RadioButtonList ID="RbtTest" runat="server" RepeatDirection ="Horizontal" RepeatLayout ="Flow" >
    <asp:ListItem Text ="Pending" Value ="P" Selected ="True" ></asp:ListItem>
    <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
    <asp:ListItem Text="Rejected" Value="R"></asp:ListItem></asp:RadioButtonList></td><td></td>
    <td><asp:Button Text="Show" ID="BtnShow" runat="server" class="btn btn-primary" /></td></tr></table>
 
    
    
      <div style="margin-bottom: 20px">
            <asp:GridView ID="GvData" runat="server" RowStyle-Height="25px" AutoGenerateColumns="false"
                                                GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                ShowHeader="true" PageSize="10" EmptyDataText="No data to display." 
                                              > 
                <Columns>
                    <asp:TemplateField HeaderText="SNo.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LblAID" runat="server" Text='<%# Eval("AID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IDNo" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="LblID" runat="server" Text='<%# Eval("IdNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Member Name" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="LblIDname" runat="server" Text='<%# Eval("MemFirstname") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FormNo" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LblFormNo" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                  
                     <asp:BoundField DataField="Descriptions" HeaderText="Description" />
                    
                   
                                                  
                                     
                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label><br />
                           
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:LinkButton ID="LBApprove" runat="server" ForeColor="#372dd8" Text="Approve" OnClientClick="return confirmation();"
                                OnClick="ApproveData" Visible='<%# Eval("IsVisible") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                 <asp:TemplateField HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="LBReject" runat="server" ForeColor="#372dd8" Text="Reject" OnClientClick="return confirmation();"
                                OnClick="RejectData" Visible='<%# Eval("IsVisible") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="LBldelete" runat="server" ForeColor="#372dd8" Text="Delete" OnClientClick="return confirmation();"
                                OnClick="DeleteData"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                           <a href='<%# "TestEdit.aspx?Aid=" & Eval("AID") & "" %>' runat="server" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 500,height: 500,marginTop : 0 } )" class="fancybox fancybox.iframe" >
                           <asp:Label ID="lbledit" runat="server" ForeColor="#372dd8" Text="Edit"></asp:Label></a>
                           
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
   
     
    </div></div></div></div></div>
    </asp:Content>
