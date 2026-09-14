<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="EMIReminder.aspx.vb" Inherits="EMIReminder" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
         
    </script>
        <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
            <div class="x_title">
                        <h2>
                       EMI Reminder
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>           <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <div class="table-responsive makeitresponsivegrid" style="min-height: 500px;">
                                <div align="center">
                                    <div class="col-md-12">
                                        <div class="row">
               
                            <div class="clr">
                             
                               <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                            color: Red"></asp:Label>
                            </div>
                            <div class="clr">
                            </div>
                                <div class="col-md-2">
                                                <asp:Label ID="Label1" runat="server" Text="Member ID  "></asp:Label>
                                                <asp:TextBox ID="txtMemberID" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" Text="From Date  "></asp:Label>
                                <asp:TextBox ID="txtfrmdate" runat="server" class="form-control" ></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtfrmdate"
                                                    Format="dd-MMM-yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtfrmdate"
                                                    ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                                </div> 
                                                <div class="col-md-2">
                                           <asp:Label ID="Label4" runat="server" Text="To Date  "></asp:Label>
                                                <asp:TextBox ID="txttodate" runat="server" class="form-control" ></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txttodate"
                                                    Format="dd-MMM-yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txttodate"
                                                    ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                                </div> 
                                                <br />
                                                <div class="col-md-2">
                                            <asp:Button ID="btnsubmit" runat ="server" class="btn btn-primary" Text="Search" />    
                                           <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" Visible="false" />
                                           </div> 
                                          <%-- </div>--%>
                                 </div> 
                            </div>     
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                        <asp:GridView ID="GvData"  runat="server" AllowPaging="True" PageSize="20"
                                            AutoGenerateColumns="True" ForeColor="Black" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            EmptyDataText="No data to display." GridLines="None">
                                           
                                            <PagerStyle HorizontalAlign = "Right" CssClass = "pagination-ys" />
                                        </asp:GridView>
                                            
                                            </div> </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                        </div>
                    </div>
                </div>
                </div>
                                    </div>
                                    <br />
            </div>
        </div>
    </div>
</div>
</div>
    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <script type="text/javascript" src="assets/jquery.dataTables.min.js"></script>

    <script type="text/javascript" src="assets/tableExport.js"></script>

    <script type="text/javascript">
        var jq = $.noConflict();
        function pageLoad(sender, args) {

            jq(document).ready(function() {
                jq('#customers2').DataTable();

            });
        }


    </script>

</asp:Content>
