<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="InvestmentReportUpdate.aspx.vb" Inherits="InvestmentReportUpdate" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<style>
a{
   color: #271e1e;
   text-decoration:none
}
</style>
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
            <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Business Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div>
                            <div class="row">
                                <div class="col-md-3">
                                    Member ID :
                                    <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblStartDate" runat="server" Text="Start Date: "></asp:Label>
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
                                    <asp:Label ID="lblEndDate" runat="server" Text="End Date : "></asp:Label>
                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-3">
                                    Type:
                                    <asp:DropDownList ID="ddltype" runat="server" class="form-control" AutoPostBack="True">
                                    </asp:DropDownList>
                                </div>
                                  </div>
                                  <div class="row">
                                <div class="col-md-3">
                                    Select Package:
                                    <asp:DropDownList ID="DDlPackage" runat="server" class="form-control" AutoPostBack="True">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    PageSize:
                                    <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control" AutoPostBack="True">
                                    </asp:DropDownList>
                                </div>
                               <div class="col-md-3" style="margin-top: 15px;">
                                    <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                </div>
                            </div>
                      
                            <div id="doublescroll" class="col-md-12">
                                <p>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <div id="gvContainer" runat="server" class="table table-bordered" style="overflow: scroll">
                                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px;
                                                    color: Gray"></asp:Label>
                                                <asp:Label ID="lblError" runat="server" Style="font-weight: bold; font-size: 12px;
                                                    color: Gray"></asp:Label>
                                                <asp:Label ID="lblinv" runat="server" Style="font-weight: bold; font-size: 12px;
                                                    color: Gray"></asp:Label>
                                                <asp:GridView ID="GvData1" runat="server" AutoGenerateColumns="true" AllowPaging="true"
                                                    CssClass="table table-bordered" HeaderStyle-CssClass="bg-primary" PageSize="20"
                                                    EmptyDataText="No data to display.">
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
    </div>
</asp:Content>
