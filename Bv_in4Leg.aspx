<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="Bv_in4Leg.aspx.vb" Inherits="Bv_in4Leg" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                            Bv in 4 Leg</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div>
                            <div align="center">
                                <div class="col-md-12">
                               
                                    <div class="col-md-2">
                                    BV:
                                        <asp:TextBox class="form-control" ID="txtAmounLeg" runat="server" placeholder="BV"></asp:TextBox>
                                         </div>
                                        
                                        <div class="col-md-2">
                                    ID:
                                        <asp:TextBox class="form-control" ID="txtID" runat="server" placeholder="ID"></asp:TextBox>
                                        <asp:DropDownList CssClass="form-control" ID="DropDownList1" runat="server" Visible ="false"></asp:DropDownList>
                                        </div>
                                        
                                       <div class="col-md-3">
                                       From Date
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-3">
                                    To Date
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator></div>
                                            <div class="col-md-2">
                                            Page Size
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" 
                                            class="form-control">
                                            <asp:ListItem Text="10" Value="10" />
                                            <asp:ListItem Text="20" Value="20" />
                                            <asp:ListItem Text="50" Value="50" />
                                            <asp:ListItem Text="100" Value="100" />
                                            <asp:ListItem Text="200" Value="200" />
                                            <asp:ListItem Text="300" Value="300" />
                                            <asp:ListItem Text="400" Value="400" />
                                            <asp:ListItem Text="500" Value="500" />
                                            <asp:ListItem Text="1000" Value="1000" />
                                            <asp:ListItem Text="2000" Value="2000" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                
                                <div class="col-md-12">
                                   <div class="col-md-6">
                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" ValidationGroup="Save" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                        <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                            Enabled="false" Visible="false" />
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                            Enabled="false" Visible="false" /></div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div id="gvContainer" runat="server">
                                            <div class="col-md-12">
                                                <div class="col-md-2">
                                                    <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                                        color: Red">
                                                    </asp:Label>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px;
                                                        color: Gray"></asp:Label></div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="LblTotalEpin" runat="server" Visible="false"></asp:Label>
                                                </div>
                                                <div class="col-md-6">
                                                </div>
                                            </div>
                                            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px"
                                                GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                ShowHeader="true" PageSize="15" EmptyDataText="No data to display." AllowSorting="true"
                                                >
                                               <Columns>
                                                       <asp:TemplateField HeaderText= "S.No.">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1%>.
                    </ItemTemplate>
                </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Idno" SortExpression="Idno">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Idno" runat="server" Text='<%# Eval("Idno") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    
                                                       
                                                        <asp:TemplateField HeaderText="Member Name" SortExpression="Membername">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblMemberName" runat="server" Text='<%# Eval("Membername") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mobileno" SortExpression="Mobileno">
                                                            <ItemTemplate>
                                                            
                                                                <asp:Label ID="lblSelfIncome" runat="server" Text='<%# Eval("Mobileno") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                         <asp:TemplateField HeaderText="View" SortExpression="SelfIncome">
                                                            <ItemTemplate>
                                                            
                                                         
                                                            <a  class="btn btn-primary" href='<%# "ViewSelfBV.aspx?Idno="& Eval("Idno")&"&amount="& eval("Amount") &"&startdate="& eval("StartDate") &"&enddate="& eval("Enddate")   %>'
                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )"><i class="icon_plus_alt2"></i>
                            <asp:Label ID="Label1" runat="server" Text="View" />
                        </a>
                                                        <%--  <a href='<%# "ViewSelfBV.aspx?Idno="& Eval("Idno")  %>'
                                                                    onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 620,height: 450,marginTop : 0 } )">
                                                                    <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text='<%# Eval("TotalBv") %>'></asp:Label></a><br />
                                                               <%-- <asp:Label ID="lblPairIncentive" runat="server" Text='<%# Eval("PairIncentive") %>'></asp:Label><br />--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        </Columns>
                                            </asp:GridView>
                                            <asp:Repeater ID="rptPager" runat="server">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                        CssClass='<%# If(Convert.ToBoolean(Eval("Enabled")), "page_enabled", "page_disabled")%>'
                                                        OnClick="Page_Changed" OnClientClick='<%# If(Not Convert.ToBoolean(Eval("Enabled")), "return false;", "") %>'
                                                        ForeColor="Black"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                       <%-- <asp:AsyncPostBackTrigger ControlID="ddlPageSize" EventName="SelectedIndexChanged" />--%>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                </div>
            </div>
        </div>
    </div>
    </div> </div>
</asp:Content>
