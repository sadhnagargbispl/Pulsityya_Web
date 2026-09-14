<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="DistrictMaster.aspx.vb" Inherits="DistrictMaster" title="Untitled Page" %>

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
                            District Master</h2>
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
                                <div class="col-md-6" style="display:flex; padding :1%">
                                    <asp:DropDownList ID="ddlSearchFields" runat="server" class="form-control" Style="width: 28%; padding:0px; border-radius:5px;  display: inline">
                                        <asp:ListItem Selected="True">--Search By--</asp:ListItem>
                                        <asp:ListItem Value="StateName">District Name</asp:ListItem>
                                        
                                        <asp:ListItem>Status</asp:ListItem>
                                        <asp:ListItem>ShowAll</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtSearch" class="form-control" runat="server" Style="width: 28%; margin-right :1%;padding:0px; border-radius:5px;  display: inline; margin-left :1%"></asp:TextBox>
                                     <asp:ImageButton ID="imgSearch" runat="server" CssClass="imgSearchButton" ImageUrl="Images/search.png" Width="32px" Height="32px" />
                                  
                                     <%-- <asp:Button ID="BtnAddNew" runat="server" class="btn btn-primary" Text="Add State" /></a>--%>
                                    </div>
                                    
                                    <div class="col-md-6">
                                    
                              </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" Visible="false" />
                                <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" Visible="false"/>
                                <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" Visible="false"/>
                                <a href="AddDistrict.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 350,marginTop : 0 } )">
                                 <asp:Button ID="BtnAddNew" runat="server" class="btn btn-primary" Text="Add District" /></a>
                                <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                    Visible="false" />
                             
                            </div>
                            <div class="col-md-12">
                                <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."
                                    Visible="false"></asp:Label>
                                <br />
                                <br />
                                <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                            </div>
                            <div style="margin-bottom: 20px">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" AllowSorting="true"
               OnSorting="GvData_Sorting" PageSize="10" EmptyDataText="No data to display.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="DistrictCode" Visible="false" SortExpression="DistrictCode">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("DistrictCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="DistrictCode" HeaderText="District Code" SortExpression="DistrictCode" />
                                        <asp:BoundField DataField="StateName" HeaderText="StateName" SortExpression="StateName" />
                                        <asp:BoundField DataField="DistrictName" HeaderText="District Name" SortExpression="DistrictName" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" Visible="false"/>
                                       <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                       <ItemTemplate >
                                       <asp:label Id="LblStatus" runat="server" Text='<%# Eval("Status") %>' class='<%# Eval("StatusClass") %>'></asp:label>
                                       </ItemTemplate>
                                       </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <a href='<%# "AddDistrict.aspx?DistrictCode=" & Crypto.Encrypt(Eval("DistrictCode"))  %>'
                                                    onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 350,marginTop : 0 } )">
                                                    <i class=" fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px">
                                                    </i>
                                                    <asp:Label ID="LBModify" runat="server" />
                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                                </a>
                                            </ItemTemplate>
                                            <HeaderStyle Width="85px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                      
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass ="PagerStyle" />
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
    </div>
</asp:Content>

