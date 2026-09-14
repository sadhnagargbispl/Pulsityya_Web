<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="UploadExcel.aspx.vb" Inherits="UploadExcel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        .PopCal
        {
            z-index: 100;
        }
    </style>

    <script type="text/javascript">
    function DateSelected(sender, e) {
    debugger;
        var lastDate = sender.get_selectedDate();
        var year = lastDate.getFullYear();
        var month = lastDate.getMonth();
        var day = new Date(year, month + 1, 0).getDate(); // last date of current month

        var selectedDate = sender.get_selectedDate();

        if (selectedDate.getDate() !== day) {
            alert('Please select only the last date of the month.');
            // Reset the textbox to last date
            var lastDateString = ("0" + day).slice(-2) + "-" + 
                                 getMonthName(month) + "-" + 
                                 year;
            $get('<%= txtStartDate.ClientID %>').value = '';
            sender._textbox.value = '';
        }

        function getMonthName(monthIndex) {
            var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
                              "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            return monthNames[monthIndex];
        }
    }
    </script>

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
                            Repurchase Business Report
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
                                <div class="col-md-12" style="min-height: 500px;">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <b>Upload Excel : </b>
                                            <asp:FileUpload ID="FileUpload1" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-3">
                                            <b>Select Payout Date : </b>
                                            <asp:TextBox ID="txtStartDate" runat="server" class="form-control" ></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                                Format="dd-MMM-yyyy" OnClientDateSelectionChanged="DateSelected">
                                            </AjaxToolkit:CalendarExtender>
                                            <%--<AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                                Format="dd-MMM-yyyy">
                                            </AjaxToolkit:CalendarExtender>--%>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                                ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-3" style="padding: 1%">
                                            <asp:Button ID="btnUpload" runat="server" Text="Upload & Validate" OnClick="btnUpload_Click"
                                                class="btn btn-primary" />
                                            <asp:Button ID="btnDownload" runat="server" Text="Download Sample Excel" OnClick="btnDownload_Click"
                                                class="btn btn-primary" />
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-12">
                                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px;
                                                    color: Gray"></asp:Label>
                                                <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                            </div>
                                            <div style="padding: 10px 10px 20px 10px">
                                                <asp:GridView ID="gvPreview" Width="100%" runat="server" AllowPaging="false" PageSize="200"
                                                    GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                    ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="true"
                                                    OnRowDataBound="gvPreview_RowDataBound">
                                                </asp:GridView>
                                                <br />
                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit Valid Data" OnClick="btnSubmit_Click"
                                                    class="btn btn-primary" Visible="false" />
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
                                            </div>
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
</asp:Content>
