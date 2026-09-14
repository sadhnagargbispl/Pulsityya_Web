<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="ApprovePinReqs.aspx.vb" Inherits="ApprovePinReqs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Approve Epin Request
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive">
                            <div align="center">
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        <asp:RadioButtonList ID="RbReqStatus" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Table">
                                            <asp:ListItem Text="All" Value="N" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                            <asp:ListItem Text="Approved" Value="C"></asp:ListItem>
                                            <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Request Date : "></asp:Label></div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control" ></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-1">
                                        To</div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control" ></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkMem" runat="server" AutoPostBack="true" Text="Member Id:" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox runat="server" Enabled="false" ID="TxtMemID" class="form-control"></asp:TextBox></div>
                                    <div class="col-md-7">
                                        <asp:Button ID="BtnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                        <asp:Button ID="btnshowall" Visible="false" runat="server" class="btn btn-primary" Text="Show All" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" Visible="false" />
                                        <asp:Button ID="BtnExportCsv" Visible="false" runat="server" class="btn btn-primary" Text="Export To CSV" />
                                        <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                            Visible="false" />
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                            Visible="false" />
                                        <asp:Button ID="btnApproove" runat="server" class="btn btn-primary" Text="Approve" Visible="false" />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                            color: Red"></asp:Label></div>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblMsg" runat="server" Font-Size="13px" Visible="False"></asp:Label></div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <span style="font-size: 10px; font-weight: bold; margin-bottom: 11px; margin-top: 8px;
                                    padding-left: 10px">Please Verify Page By Page</span>
                                <div style="margin-top: 15px">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="None" AllowPaging="True" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="50" EmptyDataText="No data to display." AllowSorting="true"
                                        OnSorting="GvData_Sorting">
                                        <Columns>
                              
                                            <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                                                    <asp:Label ID="LblReqNo" runat="server" Text='<%# Eval("ReqNo") %>'></asp:Label>
                                                    <asp:Label ID="LblIdNo" runat="server" Text='<%# Eval("IdNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IDNo" HeaderText="ID No."  SortExpression ="IdNo"/>
                                            <asp:BoundField DataField="MemName" HeaderText="Member Name" SortExpression="MemName" />
                                            <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression ="MobileNo" />
                                            <asp:BoundField DataField="ReqNo" HeaderText="Req.No" SortExpression="ReqNo" />
                                            <asp:BoundField DataField ="ReqDate" HeaderText="RequestDate" SortExpression="ReqDate" />
                                            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amt." SortExpression ="TotalAmount" />
                                            <asp:BoundField DataField="PaymentMode" HeaderText="PaymentMode" SortExpression ="PaymentMode" />
                                            <asp:BoundField DataField="TransNo" HeaderText="Transaction/Cheque/DD No" SortExpression="TransNo" />
                                            <asp:BoundField DataField="DDDate" HeaderText="Transaction/Cheque/DD Date" SortExpression="DDDate"  />
                                            <asp:BoundField DataField ="BankName" HeaderText="Bank Name" SortExpression="BankName" />
                                            <asp:BoundField DataField ="BranchName" HeaderText="Branch Name" SortExpression ="BranchName" />
                                            <asp:BoundField DataField ="Remarks" HeaderText="UserRemark" SortExpression ="Remarks" />
                                  <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a href='<%# "Img.aspx?&type=PinRequest&ID=" & Eval("Reqno") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 785,height: 580,marginTop : 0 } )">
                                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Imagepath") %>' Height="80px"
                                                                Width="80px"  />
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression ="Status" />
                                            <asp:TemplateField HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <a class="btn btn-primary" href='<%# "ViewRemark.aspx?ReqNo=" & Eval("ReqNo")  %>'
                                                        onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 570,height: 450,marginTop : 0 } )">
                                                        <asp:Label ID="LBModify" runat="server" Text="View Detail" />
                                                    </a>
                                                   
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <p id="PApprove" runat="server" visible='<%#Eval("Approve") %>'>
                                                        <a class="btn btn-primary" href='<%# "ViewPinRqs.aspx?ReqNo=" & Eval("ReqNo")  %>'
                                                            visible='<%#Eval("Approve") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 570,height: 450,marginTop : 0 } )">
                                                            <asp:Label ID="LBApprove" runat="server" Text="Approve" Visible='<%# Eval("Approve") %>' />
                                                        </a>
                                                    </p>
                                                   
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <p id="PReject" runat="server" visible='<%#Eval("Approve") %>'>
                                                        <a class="btn btn-primary" href='<%# "PinRejectRemark.aspx?ReqNo=" & Eval("ReqNo")  %>'
                                                            visible='<%#Eval("Approve") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 570,height: 450,marginTop : 0 } )">
                                                            <asp:Label ID="LBReject" runat="server" Text="Reject" Visible='<%# Eval("Approve") %>' />
                                                        </a>
                                                    </p>
                                                   
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            
                                            
                                            
                                            
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <asp:Button ID="BtnReject" runat="server" class="btn btn-primary" Text="Reject" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
