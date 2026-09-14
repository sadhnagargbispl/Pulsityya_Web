<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="IDBlockalka.aspx.vb" Inherits="App_UI_Application_Pages_IDBlockalka" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">

        window.addEventListener('mouseup', function(event) {
            var box = document.getElementById('contextMenu');
            var Btn = document.getElementById('DivOptions');
            if (event.target != Btn && event.target.parentnode != box) {
                box.style.display = 'none';
            }
        });

        function ShowMenu(Dv, control, e) {
            var posx = e.clientX + window.pageXOffset + 'px'; //Left Position of Mouse Pointer
            var posy = e.clientY + window.pageYOffset + 'px'; //Top Position of Mouse Pointer
            var StrArr = Dv.id.split(';');
            var el = StrArr[0];
            var IsActive = StrArr[1];
            var IsBlock = StrArr[2];
            var Site = StrArr[3];
            var Passw = StrArr[4];
            document.getElementById(control).style.position = 'absolute';
            document.getElementById(control).style.display = 'inline';
            document.getElementById(control).style.left = posx;
            document.getElementById(control).style.top = posy;
            var currentdate = new Date();
            var TmID = currentdate.getDate().toString() + currentdate.getHours().toString() + currentdate.getFullYear().toString() + currentdate.getMonth().toString();
            //alert(Site);
            var inHtm
            if (IsBlock == 'Y')
                inHtm = '<li class="separator"><a href="UnBlock.aspx?Tp=S&key=' + el + '">Unblock</a></li>';
            else
                inHtm = '<li class="separator"><a href="Block.aspx?Tp=S&key=' + el + '">Block Now</a></li>';
            document.getElementById(control).innerHTML = inHtm
        }
    </script>

    <style type="text/css">
        .ContextItem /*Context Menu Item Style */
        {
            width: 150px;
            background: #337ab7;
            color: Black;
            font-weight: normal; /*border: solid 1px #CCC;*/
            text-align: left;
        }
        .ContextItem LI
        {
            width: 150px;
            list-style: none;
            padding: 0px;
            margin: 0px;
        }
        .ContextItem LI:hover
        {
            font-weight: bold;
            background: #C2CEEB;
            width: 110px;
        }
        .ContextItem A
        {
            width: 150px;
            color: #fff;
            text-decoration: none;
            line-height: 20px;
            height: 20px; /*padding: 1px 5px;
	padding-left: 18px;*/
        }
        .ContextItem LI.separator
        {
            border-top: solid 0px #CCC;
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
                            ID Block And Unblock</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                
                                <div class="col-md-3">
                                 <asp:Label ID="Label1" runat="server" Text="Member Name"></asp:Label>
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
                                
                                <%--  <div class ="col-md-2">
                                         PageSize:
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed">
                                            <asp:ListItem Text="100" Value="100" />
                                            <asp:ListItem Text="200" Value="200" />
                                            <asp:ListItem Text="300" Value="300" />
                                            <asp:ListItem Text="400" Value="400" />
                                            <asp:ListItem Text="500" Value="500" />
                                            <asp:ListItem Text="600" Value="600" />
                                        </asp:DropDownList>
                                        
                                        </div>--%>
                                <div class="col-md-4">
                                    <asp:Button ID="BtnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" Visible="false" />
                                    <asp:Button ID="btnExportCsv" runat="server" class="btn btn-primary" Text="Export To Csv"
                                        Visible="false" /></div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                </div>
                            </div>
                            <div style="overflow: scroll" class="table table-bordered">
                                <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                    AutoGenerateColumns="False" ForeColor="Black" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    EmptyDataText="No data to display." GridLines="None" AllowSorting="true" OnSorting="GvData_Sorting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SNo.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Member ID" SortExpression="IdNo" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="MemberID" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" SortExpression="Qstr" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <%#Eval("memname")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      

                                        <asp:TemplateField HeaderText="Click Here!" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <%-- <div id='<%# Eval("IDNo") %>' style="background-color:#99CCCC;color:Black;font-weight:bold;font-size:12px" >
                                <asp:Image runat="server" ID="BtnMenus" ImageUrl="../Resources/images/moreopt.jpg" AlternateText=""  />
                            </div>--%>
                                                <div id="DivOptions">
                                                    <div id='<%# Eval("IDNo") &";"& Eval("ActiveStatus") &";"& Eval("IsBlock") &";"& Eval("Site") &";"& Eval("LgnID")  %>'
                                                        style="background-color: #99CCCC; color: Black; font-weight: bold; font-size: 12px"
                                                        onclick="ShowMenu(this,'contextMenu',event);">
                                                        <asp:Image runat="server" ID="Image1" ImageUrl="~/img/moreopt.jpg" AlternateText="More Option.." />
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No." SortExpression="MobileNo" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Mobile" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Pan No." SortExpression="MobileNo" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Mobile" runat="server" Text='<%# Eval("PanNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                       
                                      
                                        <asp:TemplateField HeaderText="Block Remark" SortExpression="Status" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Status" runat="server" Text='<%# Eval("BlockRemark") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Block Date" SortExpression="Status" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="Status" runat="server" Text='<%# Eval("BlockDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    
                                    </Columns>
                                    <PagerStyle HorizontalAlign = "Left" CssClass = "pagination-ys" ForeColor="blue" />
                                </asp:GridView>
                                <%--<asp:Repeater ID="rptPager" runat="server">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                    Enabled='<%# Eval("Enabled") %>' OnClick="Page_Changed"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:Repeater>--%>
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
    <ul id="contextMenu" class="ContextItem">
    </ul>
</asp:Content>
