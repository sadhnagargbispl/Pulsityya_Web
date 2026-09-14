<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="GroupzaraDirects.aspx.vb" Inherits="GroupzaraDirects" %>


<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
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
    <style type="text/css">
        #doublescroll
        {
            overflow: auto;
            overflow-y: hidden;
        }
        #doublescroll p
        {
            margin: 0;
            padding: 1em;
            white-space: nowrap;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <%--  <div class="row">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <!-- Genex Business -->
                    <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                        <div class="profile-bar-simple red-border clearfix">--%>
<div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
              <div class="x_panel">
                <div class="x_title">
                            <h6>
                                Downline Detail Report
                            </h6>
                             <asp:Label ID="Label2" runat="server"></asp:Label>
                        </div>
                            <div class="panel-body">
                                        <div class="clr">
                                            <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                        </div>
                                        <div class="col-md-12">
                          <table class="table-responsive">                                   
                                <div class="col-md-3">
                                <asp:Label ID="Label3" runat="server" Text="Enter Member ID :"></asp:Label> 
    <asp:TextBox class="form-control" ID="txtMemberId" runat="server" AutoPostBack="true" ></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    </div>
                                   
                                 <div id="Div1" runat="server" class="col-md-3" >
                                <asp:Label ID="lbltype" runat="server" Text="Search Type: "></asp:Label>
                                <asp:DropDownList ID="DDlDate" runat="server" class="form-control" AutoPostBack="true">
                                     <asp:ListItem Text="All" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Joining" Value="J"></asp:ListItem>
                                    <asp:ListItem Text="Activation" Value="B"></asp:ListItem>
                                </asp:DropDownList>
                                </div> 
                              
                             <div id="Div2" runat="server" style="margin-top :15px;" class="col-md-3">
                               
                                <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
                                <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                    Format="dd-MMM-yyyy">
                                </AjaxToolkit:CalendarExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                    ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                 
                             <div id="Div3" runat="server" style="margin-top :15px;" class="col-md-3">
                                <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
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
                                                        Search By
                                                        <asp:DropDownList ID="rbtnsearch" AutoPostBack="true" runat="server" class="form-control">
                                                            <asp:ListItem Text="Level Wise" Selected="True" Value="L"></asp:ListItem>
                                                            <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div> 
                                                  
                                                    <div class="col-md-3">
                                                        <div id="lbllevel" runat="server">
                                                            Level
                                                            <asp:DropDownList ID="DdlLevel" CssClass="form-control" TabIndex="1" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                        </div> 
                                                 
                                                    <div class="col-md-3">
                                                        <div id="divSearch" runat="server">
                                                            Search
                                                            <asp:DropDownList ID="DDlSearchby" CssClass="form-control" TabIndex="2" runat="server"  >
                                                                <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        </div> 
                                                  
                                                    <div class="col-md-3">
                                                    <div id="div4" runat="server" style="margin-top:20px;">
                                                        <asp:Button ID="BtnSubmit" runat="server" Text="Search" TabIndex="3" class="btn btn-primary"
                                                            />
                                                            <asp:Button ID="BtnExportA" runat="server" Text="Export to Excel" class="btn btn-primary" /> 
                                                            </div>
                                                           
                                                            </div> 
                                                    
                                            </table>
                                        </div>
                                        <br>
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                            <div class="col-md-3">
                                            </div>
                                            <div class="sda-content-3">
                                                <table id="table" class="table table-bordered">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <th style="text-align: center">
                                                                Left
                                                            </th>
                                                            <th style="text-align: center">
                                                                Right
                                                            </th>
                                                            <th style="text-align: center">
                                                                Total
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                Total Direct
                                                            </th>
                                                            <td id="tdDirectleft" runat="server" style="text-align: center">
                                                                0
                                                            </td>
                                                            <td id="tdDirectright" runat="server" style="text-align: center">
                                                                0
                                                            </td>
                                                            <td id="TotalDirect" runat="server" style="text-align: center">
                                                                0
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                Active Direct
                                                            </th>
                                                            <td id="tddirectActive" runat="server" style="text-align: center">
                                                                0
                                                            </td>
                                                            <td id="tdindirectActive" runat="server" style="text-align: center">
                                                                0
                                                            </td>
                                                            <td id="TotalActive" runat="server" style="text-align: center">
                                                                0
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                Direct BV
                                                              
                                                            </th>
                                                            <td id="Directunit" runat="server" style="text-align: center">
                                                                0
                                                            </td>
                                                            <td id="indirectunit" runat="server" style="text-align: center">
                                                                0
                                                            </td>
                                                            <td id="totalunit" runat="server" style="text-align: center">
                                                                0
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <div class="col-md-3">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div id="DivSideA" runat="server">
                                                
                                               <%-- <asp:Label ID="lbltotal" runat="server"></asp:Label>--%>
                                                <div class="table-responsive">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text="Total Records" Visible="false" ></asp:Label>
                                                <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                               <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                                GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                ShowHeader="true" AllowSorting="true" PageSize="40" EmptyDataText="No data to display."
                                                ForeColor="Black">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="MLevel" HeaderText=" Level" />
                                                    <asp:BoundField DataField="IDNo" HeaderText=" ID No" />
                                                    <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                                                    <asp:BoundField DataField="SponsorId" HeaderText="Sponsor ID" />
                                                    <asp:BoundField DataField="MemberName" HeaderText="Sponsor Name" />
                                                    <%--<asp:BoundField DataField="Position" HeaderText="Position" />--%>
                                                    <asp:BoundField DataField="BV" HeaderText="BV" />
                                                    <asp:BoundField DataField="PackageName" HeaderText="Package Name" />
                                                    <asp:BoundField DataField="Status" HeaderText="Active Status" />
                                                    <asp:BoundField DataField="Doj" HeaderText="Date Of Joining" />
                                                    <asp:BoundField DataField="UpgradeDate" HeaderText="Activation Date" />
                                                </Columns>
                                            </asp:GridView>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                        <div class="row">
                            <!-- end of weather widget -->
                        </div>
                    </div>
                </div>
            </div>
        </div>
   

</asp:Content>
