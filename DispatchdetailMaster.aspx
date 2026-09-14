<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master"  AutoEventWireup="false" CodeFile="DispatchdetailMaster.aspx.vb" Inherits="App_UI_Application_Pages_DispatchdetailMaster" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Dispatch Detail Master</h2>
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
                                    <div class="col-md-6" style="display: flex; padding: 1%">
          <asp:DropDownList ID="ddlSearchFields" runat="server" class="form-control" Style="width: 28%;
                                            padding: 0px; border-radius: 5px; display: inline">
            <asp:ListItem Selected="True" Value="showall">--Search By--</asp:ListItem>
            <asp:ListItem Value="Idno">Idno</asp:ListItem>
            <asp:ListItem Value="kitname">Package Name</asp:ListItem>
            <asp:ListItem>Date</asp:ListItem>
             <asp:ListItem>Remarks</asp:ListItem>
           <%-- <asp:ListItem>Status</asp:ListItem>--%>
            
        </asp:DropDownList>
       
    <asp:TextBox ID="txtSearch" class="form-control" runat="server" Style="width: 28%;
                                                margin-right: 1%; padding: 0px; border-radius: 5px; display: inline; margin-left: 1%"></asp:TextBox>
                                            <asp:ImageButton ID="imgSearch" runat="server" CssClass="imgSearchButton" ImageUrl="Images/search.png"
                                                Width="32px" Height="32px" />
                                        </div>
                                        <div class="col-md-6">
                                        </div>
                                    </div>
                                </div>
                                 <div class="col-md-12">
            <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" Visible ="false"  /></td>
   <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" Visible ="false"  /></td>
            
                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
               
                    <a href="AddDispatch.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 486,height: 330,marginTop : 0 } )">
                    <asp:Button ID="BtnAdd" runat="server" class="btn btn-primary" Text="Add Dispatch" /></a>
              
                  <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All" Visible="false" />
                  
               
    </div></div>
     <div style="margin-left:25px;margin-top:20px;margin-bottom:20px;">
<asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated." visible ="false"></asp:Label>
<br />
<br />
<asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
</div>
    <div style="margin-bottom:20px">
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
            GridLines="None" AllowPaging="true" CssClass="table table-bordered" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="20" EmptyDataText="No data to display." HeaderStyle-CssClass="bg-primary">
            <Columns>
                <asp:TemplateField HeaderText="GrpID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="LblNewsID" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
               <%-- <asp:BoundField DataField="Type" HeaderText="Type" ControlStyle-Width="50px" />--%>
                <asp:BoundField DataField="Idno" HeaderText="IdNo" />
                <asp:BoundField DataField="kitname" HeaderText="Package Name" />
                 <asp:BoundField DataField="Date" HeaderText="Date" />
                 <%-- <asp:BoundField DataField="ToDate" HeaderText="ToDate" />--%>
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                <%--<asp:BoundField DataField="Status" HeaderText="Status"  />--%>
                <%--<asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("Status") %>' class='<%# Eval("StatusClass") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass ="btn-group">
                    <ItemTemplate>
                        <a href='<%# "AddDispatch.aspx?DId=" & Crypto.Encrypt(Eval("VDId"))  %>'
                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 350,marginTop : 0 } )"><i class="fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px"></i>
                            <asp:Label ID="LBModify" runat="server" />
                        </a>
                        <%--<asp:LinkButton ID="lnkModify" runat="server" Text="Modify" OnClick="ModifyGrp" CssClass="fancybox fancybox.iframe"></asp:LinkButton>--%>
                    </ItemTemplate>
                    <HeaderStyle Width="55px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteNews"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Width="55px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
     </div>
    </div>
    
    </div> 
    <div class="row">
                <!-- end of weather widget -->
            </div>
    </div> 
    </div> 
</asp:Content>

