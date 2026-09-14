<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="GroceryDetail.aspx.vb" 
Inherits="GroceryDetail" title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">

        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
    </script>

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
                    <div class="x_title">
                        <h2>
                            Grocery Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-3">
                            Member ID:
                            <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3" >
                            Reward
                            <asp:DropDownList ID="ddllist" runat="server" class="form-control">
                            </asp:DropDownList>
                        </div>
                          <div class="col-md-3" ><br />
                           <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                        <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                            color: Red"></asp:Label>
                        </div>
                    </div>
                   
                    <div style="margin-top: 20px; margin-bottom: 20px;">
                        <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                            color: Gray"></asp:Label>
                    </div>
                    <div style="padding: 10px 10px 20px 10px" id="divDetail" runat="server">
                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="true" GridLines="Both"
                            class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                            EmptyDataText="No data to display." AutoGenerateColumns="true" PageSize="10"
                            PagerStyle-CssClass="PagerStyle">
                            
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
</asp:Content>
