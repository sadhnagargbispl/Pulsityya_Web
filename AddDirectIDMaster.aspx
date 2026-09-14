<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="AddDirectIDMaster.aspx.vb" Inherits="AddDirectIDMaster" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="panel-body">
                        <table cellspacing="10px" cellpadding="0%">
                            <tbody>
                                <tr style="margin-top: 20px; padding-top: 20px">
                                    <td>
                                        <strong>Member ID</strong>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="LblAmount" runat="server" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtoffer" runat="server" class="form-control" AutoPostBack="true"
                                                    Width="200px"></asp:TextBox>
                                                <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                                <asp:TextBox ID="TxtFormNo" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                                <br />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtoffer" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="margin-top: 20px; padding-top: 20px">
                                    <td>
                                        <strong>Leg : </strong>
                                        <asp:RadioButtonList ID="rdblist" runat="server" Width="150px" CellPadding="0" CellSpacing="10"
                                            RepeatDirection="Horizontal" Style="margin-left: 16%">
                                            <asp:ListItem Selected="true" Text="Left" Value="1">Left</asp:ListItem>
                                            <asp:ListItem Text="Right" Value="2">Right</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr style="margin-top: 20px; padding-top: 20px">
                                    <td>
                                        <asp:Button ID="BtnFundTransfer" runat="server" Text="Add" class="btn btn-primary"
                                            ValidationGroup="Save" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px"
                            GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                            ShowHeader="true" PageSize="20" EmptyDataText="No data to display.">
                            <Columns>
                                <asp:TemplateField HeaderText="KitId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("formno") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Member ID" HeaderText="Member ID" />
                                <asp:BoundField DataField="Member Name" HeaderText="Member Name" />
                                <asp:BoundField DataField="Leg" HeaderText="Leg" />
                                <asp:BoundField DataField="Date" HeaderText="Date" />
                                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
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
    </div>
    <%--    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                           </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                           <div class="row"></div> 
                                <div class="col-md-12">
                               <div class="col-md-6">
                                  <asp:TextBox ID="txtoffer" runat="server" class="form-control" Width="150px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtoffer" ValidationGroup="Save"></asp:RequiredFieldValidator>
                               </div> 
                                     
                                   
                                   <br />
                                        <asp:Button ID="BtnFundTransfer" runat="server" Text="Add" class="btn btn-primary"
                                            ValidationGroup="Save" OnClientClick ="this.disabled=true; this.value='Sending Request…';" UseSubmitBehavior="false"/>
                                   
                                
                   
                                </div>
                                <div style="margin-bottom: 20px" class="col-md-12">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true"  class="table table-bordered"
                                        HeaderStyle-CssClass="bg-primary" ShowHeader="true" PageSize="20" EmptyDataText="No data to display.">
                                     
                                    </asp:GridView>
                                </div>
                           
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />--%>
</asp:Content>
