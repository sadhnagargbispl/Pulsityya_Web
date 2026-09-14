<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="MyDirectMember.aspx.vb" Inherits="MyDirectMember" Title="Untitled Page" %>
    
    
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                          Direct Member</h2>
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
               <div class="col-md-4"> <asp:Label ID="LblMemberID" runat="server" Text ="Member Id:"></asp:Label>
                <asp:TextBox Id="txtMemberId" runat="server" class="form-control"></asp:TextBox> </div>
                
                <div class="col-md-4">  <asp:Label ID="LblLevel" runat="server" Text=" Choose Level:" ></asp:Label>
                        <asp:DropDownList ID="DDLLevel" runat="server" class="form-control" > 
                         </asp:DropDownList></div>
                         <div class="col-md-4"></div>
                         </div>
                         <div class="col-md-12">
                         <div class="col-md-4"><asp:CheckBox runat="server" ID="ChkDate" AutoPostBack ="true"  Checked="true"  />
                          <asp:Label ID="LblDate" runat="server" Text=" Choose Date Type:"  ></asp:Label>
                        <asp:DropDownList ID="DDlDate" runat="server" class="form-control"> 
                        <asp:ListItem Text ="Date Of Activation" Value ="A"></asp:ListItem>
                        <asp:ListItem Text="Date Of Joining" Value="J"></asp:ListItem>
                         </asp:DropDownList></div>
                         <div class="col-md-4"> <asp:Label ID="lblSessionDate" runat="server" Text="Choose From Date : "></asp:Label>
                    <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
         
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFromDate"
                        ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator> </div>
                        
                        <div class="col-md-4">  <asp:Label ID="lblToDate" runat="server" Text="Choose To Date : "></asp:Label>
                    <asp:TextBox ID="TxtToDate" runat="server" class="form-control" AutoPostBack ="true"></asp:TextBox>
         
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                        ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>  </div></div>
                        <div class="col-md-12">
                        <div class="col-md-4"> <asp:Button ID="BtnSearch" runat="server" Text="Search" class="btn btn-primary" />
                        <asp:Button ID="BtnExport" runat="server" Text="Export To Excel" class="btn btn-primary" /></div>
                        <div class="col-md-8"><asp:Label ID="LblError" runat="server" Visible="false"></asp:Label></div>
                        </div>
                
                </div>
                  
            
            <div class="col-md-12">
       <asp:GridView ID="GrdDirects" runat="server"  RowStyle-Height="25px"
               AutoGenerateColumns="False" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    EmptyDataText="No data to display." GridLines="None">
          
                                            <Columns>                                
                                                <asp:TemplateField  HeaderText ="S.No">
                                             
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                    </ItemTemplate>
                                              </asp:TemplateField>
                                              
                                                <asp:BoundField  DataField="IDNo" HeaderText="ID No"></asp:BoundField>
                                                <asp:BoundField  DataField="MemName" HeaderText="Member Name"></asp:BoundField>
                                                <asp:BoundField DataField="City" HeaderText="City"  ></asp:BoundField> 
                                                <asp:BoundField DataField ="MobileNo" HeaderText ="MobileNo" />
                                               
                                               <asp:BoundField  DataField="Doj" HeaderText="Date Of joining"></asp:BoundField>                                                                                                                                                
                                              <asp:BoundField  DataField="RefIdNo" HeaderText="Sponsor ID"></asp:BoundField>
                                               <asp:BoundField DataField ="ReferalName" HeaderText ="Sponsor Name" />                                              
                                    <asp:BoundField  DataField="MLevel" HeaderText="Level"></asp:BoundField>
                                              
                                               <asp:BoundField  DataField="ActiveStatus" HeaderText="Active Status"></asp:BoundField>
                                                <asp:BoundField  DataField="UpgradeDate" HeaderText="Date Of Activation"></asp:BoundField> 
                                                        </Columns>
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


