<%--<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="Addmasterincome.aspx.vb" Inherits="Addmasterincome" Title="" EnableEventValidation="true" %>--%>
    <%@ Page Language="VB" AutoEventWireup="false"
    CodeFile="Addmasterincome.aspx.vb" Inherits="Addmasterincome" Title="" EnableEventValidation="true" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <script type="text/javascript" language="javascript">
        function confirmation() {
            debugger;
            var Amount = document.getElementById("<%= TxtFund.ClientID %>").value;

        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        //        function isNumberMonth() {

        //            var res = document.getElementById("ctl00_ContentPlaceHolder1_TxtMonth").value;

        //            if (res < 1 || res > 60)
        //                alert("Enter Month Between 1 to 60 ");
        //        }
        function validfloat(txt, ev) {
            ev.returnValue = ((ev.keyCode >= 48 && ev.keyCode <= 57) || (ev.keyCode == 46 && txt.value.indexOf('.') == -1));
        }
    </script>

    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-family: Arial;
        }
        .modal1
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        .center1
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center1 img
        {
            height: 128px;
            width: 128px;
        }
    </style>
    
     <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
<%--</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">--%>
</head>
<body>
    <form id="form1" runat="server">
     <ajaxtoolkit:ToolkitScriptManager id="scriptmanager1" runat="server">
</ajaxtoolkit:ToolkitScriptManager>
<div class="container body">
        <div class="main_container">   
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
                            Add Master Income</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                             <div class="panel-body">
                                <table cellspacing="10px" cellpadding="0%">
                                    <tbody>
                                        <tr>
                                            <td colspan="4" style="width: 100%">
                                                <br />
                                            </td>
                                        </tr>
                                        <tr style="margin-top: 20px; padding-top: 20px">
                                            <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                             
                                                <strong>Member ID :</strong>
                                            </td>
                                            <td>
                                            
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true"
                                                            Width="200px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                                            ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                    </ContentTemplate>
                                               </asp:UpdatePanel>
                                             </td>
                                        </tr>
                                        
                                          <%-- <tr style="margin-top: 20px; padding-top: 20px; margin-bottom: 20px;"><td><br /><br /></td></tr>--%>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                                            <ContentTemplate>
                                                <tr style="margin-top: 20px; padding-top: 20px; margin-bottom: 20px;">
                                                    <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                                        <strong>Member Name:</strong>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtName" Width="200px" runat="server" class="form-control" Enabled="false"></asp:TextBox>
                                                        <asp:TextBox ID="TxtFormNo" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                              
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                       <tr style="margin-top: 20px; padding-top: 20px; margin-bottom: 20px;"><td><br /></td></tr>
                                        <tr style="margin-top: 20px; padding-top: 20px">
                                            <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                                <strong>Amount:</strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtFund" Width="200px" runat="server" class="form-control" onkeypress="return isNumberKey(event);"
                                                   ></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Amount."
                                                    ControlToValidate="TxtFund" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <%--<tr style="margin-top: 20px; padding-top: 20px">
                                            <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                                <strong>Month :</strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtMonth" Width="200px" runat="server" class="form-control" onkeypress="return isNumberKey(event);"
                                                    MaxLength="2"  AutoPostBack="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="* Enter Month From 1 to 60 ."
                                                    ControlToValidate="TxtMonth" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                   
                                        <tr style="margin-top: 20px; padding-top: 20px">
                                            <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                                <strong>EMI Amount:</strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtROI" Width="200px" runat="server" class="form-control" onkeypress="validfloat(this, event);"
                                                    Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                           <tr style="margin-top: 20px; padding-top: 20px; margin-bottom: 20px;"><td><br /></td></tr>
                                        <tr style="margin-top: 20px; padding-top: 20px">
                                            <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                                <strong>Remarks</strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtRemarks" Width="200px" runat="server" class="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Remrks"
                                                    ControlToValidate="TxtRemarks" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <br />
                                            </td>
                                        </tr>--%>
                                        <tr style="margin-top: 20px; padding-top: 20px">
                                            <td width="14%" align="left" valign="middle" style="padding-left: 20px;">
                                            </td>
                                            <td>
                                                <asp:Button ID="BtnFundTransfer" runat="server" Text="Submit" OnClientClick="return confirmation();"
                                                    class="btn btn-primary" ValidationGroup="Save" />
                                                <asp:Button ID="BtnCancel" runat="server" Text="Cancel" class="btn btn-primary" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                                <br />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                </div> 
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="BtnFundTransfer" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <!-- end of weather widget -->
    </div>
    </div> 
    </div> 
     </form>
</body>
</html>
<%--</asp:Content>--%>
