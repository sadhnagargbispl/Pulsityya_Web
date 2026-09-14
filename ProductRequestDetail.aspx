<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="ProductRequestDetail.aspx.vb" Inherits="ProductRequestDetail" Title="Product Request Detail" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
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
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                           Product Request Detail</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                             <%--  <div class="table-responsive makeitresponsivegrid">--%>
                            <div class="col-md-12">
                                <div class="col-md-4">
                                    <asp:Label ID="LblMemberID" runat="server" Text="Member Id:"></asp:Label>
                                    <asp:TextBox ID="txtMemberId" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                              <%--      <asp:RequiredFieldValidator ID="RequiredFieldvalidator1" runat="server" ControlToValidate="txtMemberId"
                                        ErrorMessage="Enter Member Id" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblSessionDate" runat="server" Text="Choose From Date : "></asp:Label>
                                    <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFromDate"
                                        ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblToDate" runat="server" Text="Choose To Date : "></asp:Label>
                                    <asp:TextBox ID="TxtToDate" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                        ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                               
                     </div>
                     <div class="col-md-12">
                                
                              
                                <div class="col-md-4" style="padding-top :2%">
                                    <asp:Button ID="BtnSearch" runat="server" Text="Search" class="btn btn-primary" />
                                    <asp:Button ID="BtnExport" runat="server" Text="Export To Excel" class="btn btn-primary" />
                                </div>
                                <div class="col-md-4">
                                      <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label>
                                </div>
                                
                                        <div class="col-md-4">
                                        </div>
                                </div>
                        <div class="row">
                        <div class="col-md-12">
                                 <div class="table-responsive">
                                                                <asp:DataGrid ID="DgReceivedPin" runat="server" PageSize="10" CssClass="table table-striped table-bordered" AutoGenerateColumns="False"
                                                                    AllowPaging="True" Width="90%">
                                                                    <Columns>
                                                                    <asp:TemplateColumn HeaderText="SNo." >
                                                                    <ItemTemplate>
                                                                    <%#Container.DataSetIndex + 1%></ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                  <asp:BoundColumn DataField ="IdnO" HeaderText="IdNo">
                                                                  </asp:BoundColumn>
                                                                     <asp:BoundColumn DataField ="MemberName" HeaderText="Member Name">
                                                                  </asp:BoundColumn>
                                                                       <asp:TemplateColumn HeaderText="OrderNo">
                                                                      <ItemTemplate >
                                                        <a href='<%# "ViewProductDetail.aspx?ReqNo=" & Eval("OrderNo")  %>'   onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 490,height: 390,marginTop : 0 } )" >
                                                                    <asp:Label ID="LblOrderno" Text='<%# Eval("OrderNo") %>' runat="server" style="color:#337ab7;" ></asp:Label>  </a> </ItemTemplate></asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="OrderDate" HeaderText="OrderDate">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField ="OrderQty" HeaderText="Order qty"></asp:BoundColumn>
                                                                         <asp:BoundColumn DataField="OrderAmt" HeaderText="Order Amount"></asp:BoundColumn>
                                                                                                                   
                                                                        <asp:BoundColumn DataField="Remark" HeaderText="Remark"></asp:BoundColumn>  
                                                                        
                                                                        <asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>  
                                                                        
                                                                        <asp:BoundColumn DataField="DispatchStatus" HeaderText="Order Status"></asp:BoundColumn>  
                                                                          <%--<asp:BoundColumn DataField="changeDispatch" HeaderText="Change Dispatch Mode"></asp:BoundColumn>  --%>
                                                                                                                                          
                                                                    </Columns>
                                                                    <PagerStyle Mode="NumericPages" CssClass="PagerStyle"></PagerStyle>
                                                                </asp:DataGrid>
                                                          </div>
                                
                                <div id="NoData" runat="server" visible="false">
                                    <h4>No Record Found!</h4>
                                  </div>
                                  </div>
                        </div>
                   <%-- </div>--%>
                </div>
            </div>
        </div>
        </div>
        <div class="row">
        </div>
    </div>
</asp:Content>
