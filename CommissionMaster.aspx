<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="CommissionMaster.aspx.vb" Inherits="CommissionMaster"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script type="text/javascript">
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
        </script>
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
                           Commission Master</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                           
                            <div align="center">
                                <asp:Label ID="lblMsg" runat="server" Font-Size="13px" Visible="False"></asp:Label>
                            </div>
                            <div class="col-md-12">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                    GridLines="Both"  class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true"  EmptyDataText="No data to display.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="clubId" Visible="false">
                                            <ItemTemplate>
                                            <asp:Label ID="LblClub" runat="server" Text='<%# Eval("Club") %>' Visible="false"></asp:Label>                                                <asp:Label ID="lblclubId" runat="server" Text='<%# Eval("ClubId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                     
                                        <asp:BoundField DataField="Club" HeaderText="Club" />
                                     <asp:TemplateField HeaderText ="Level1">
                                     <ItemTemplate >
                                     <asp:TextBox ID="TxtL1" runat="server" Text='<%# Eval("L1") %>' Width ="100px"></asp:TextBox></ItemTemplate>
                                     </asp:TemplateField>
                                       <asp:TemplateField HeaderText ="Level2">
                                     <ItemTemplate >
                                     <asp:TextBox ID="TxtL2" runat="server" Text='<%# Eval("L2") %>' Width ="100px"></asp:TextBox></ItemTemplate>
                                     </asp:TemplateField>
                                                 <asp:TemplateField HeaderText ="Level3">
                                     <ItemTemplate >
                                     <asp:TextBox ID="TxtL3" runat="server" Text='<%# Eval("L3") %>' Width ="100px"></asp:TextBox></ItemTemplate>
                                     </asp:TemplateField>
                                                 <asp:TemplateField HeaderText ="Level4">
                                     <ItemTemplate >
                                     <asp:TextBox ID="TxtL4" runat="server" Text='<%# Eval("L4") %>' Width ="100px"></asp:TextBox></ItemTemplate>
                                     </asp:TemplateField>
                                                 <asp:TemplateField HeaderText ="Level5">
                                     <ItemTemplate >
                                     <asp:TextBox ID="TxtL5" runat="server" Text='<%# Eval("L5") %>' Width ="100px"></asp:TextBox></ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Modify">
                                     <ItemTemplate >
                                     <asp:Button ID="BtnModify" runat="server" Text="Modify" class="btn btn-primary" OnClick="UpdateCommission" OnClientClick="return confirmation();"  /></ItemTemplate>
                                     
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
    </div>
    <div class="row">
        <!-- end of weather widget -->
    </div>
    </div>
</asp:Content>
