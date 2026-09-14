<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddSeminarMaster.aspx.vb"
    Inherits="AddSeminarMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
     <AjaxToolkit:ToolkitScriptManager ID="scriptmanager1" runat="server">
    </AjaxToolkit:ToolkitScriptManager>
    <div style="height: 200px; padding-left: 10px">
        <h4 style="border-bottom: dashed 1px #666666; margin-bottom: 11px; margin-top: 8px">
            Seminar Master</h4>
        <p style="color: #666666; line-height: 25px;">
            <table>
                <tr>
                    <td width="10%">
                        Name :
                    </td>
                    <td width="50%">
                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtDate"
                            runat="server" ValidationGroup="Save">Please Enter Name.!!</asp:RequiredFieldValidator>
                    </td>
                    <tr>
                        <td>
                            Date
                        </td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server" onkeypress="return false;"></asp:TextBox>
                            <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtDate" Format="dd-MMM-yyyy"
                                runat="server">
                            </ajax:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtDate"
                                runat="server" ValidationGroup="Save">Please Select Date.!!</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Pin Code :
                        </td>
                        <td>
                            <asp:TextBox ID="txtpinCode" runat="server" MaxLength="6"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="txtpinCode"
                                runat="server" ValidationGroup="Save">Please Enter Pin Code.!!</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                TargetControlID="txtpinCode" ValidChars="0123456789">
                            </ajax:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            State :
                        </td>
                        <td>
                            <asp:TextBox ID="txtState" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:HiddenField ID="hdnStateID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            City :
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:HiddenField ID="hdnCity" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Area :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAreaCode" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
            </table>
            <asp:Button ID="BtnSave" CssClass="Btn" runat="server" Text="Save" ValidationGroup="Save" />
        </p>
    </div>
    </form>
</body>
</html>
