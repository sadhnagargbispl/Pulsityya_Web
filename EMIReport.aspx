<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="EMIReport.aspx.vb" Inherits="EMIReport" Title="" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                           EMI Report
                        </h2>
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
                                    <div class="col-md-2"> Enter IdNo</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMember" runat="server" class="form-control"></asp:TextBox></div>
                                    
                                    <div class="col-md-6">
                                     
                                    <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Excel" />
                              
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print" class="btn btn-primary" />
                                    <asp:Button ID="BtADd" runat="server" class="btn btn-primary" Text="Add EMI" />
                                </div>
                                </div>
                                
                                <div class="col-md-12">
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                        color: Red"></asp:Label>
                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray"></asp:Label>
                                </div>
                            </div>
                            <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                margin-bottom: 25px;" class="col-md-12">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false"  RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                    PageSize="25" EmptyDataText="No data to display.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1%>.</ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField ="Idno" HeaderText="IdNo" />
                                        <asp:BoundField DataField ="MemberName" HeaderText="Member Name" />
                                        <asp:BoundField DataField ="Total EMI" HeaderText="Total EMI" />
                                          <asp:BoundField DataField ="Paid EMI" HeaderText="Paid EMI" />
                                         <asp:BoundField DataField ="Due EMI" HeaderText="Due EMI" />
                                         <asp:TemplateField >
                                         <ItemTemplate >
                                         <a class="btn btn-primary" href='<%# "EMIDeposit.aspx?key=" & Crypto.Encrypt(Eval("Reqno"))  %>'  onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 570,height: 550,marginTop : 0 } )" > <i class="icon-check-alt2"></i>
<asp:Label ID="LBModify" runat="server" Text="View"/>
</a></ItemTemplate></asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <br />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
