<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master"
    AutoEventWireup="false" CodeFile="TransferAmount.aspx.vb" Inherits="TransferAmount" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        p
        {
            font-weight: bold;
            color: #666666;
            margin: 0px;
            line-height: 25px;
            width: 100%;
            padding-bottom: 8px;
            text-align: left;
        }
        .right-mid .welcome
        {
            height: 23px;
            border-top: 1px solid #203454;
            border-bottom: 1px solid #203454;
            background-color: #395e96;
            color: #e4efff;
            font-weight: bold;
            margin: 0px 0px 5px 0px;
        }
        .right-mid .welcome span
        {
            padding: 3px 0px 0px 15px;
            display: block;
        }
        .right-mid .form-heading
        {
            background-color: #71B061;
            height: 24px;
            border-top: 1px solid #5c0871;
            border-bottom: 1px solid #4671b4;
            font: bold 12px Tahoma, Geneva, sans-serif;
            color: #ffffff;
            text-shadow: #445f8a 1px 2px 0px;
            text-align: center;
        }
    </style>
   
    <div class="right-mid">
    <ol class="breadcrumb" style =" margin-right:20px">
						<li><i class="fa fa-home"></i><a href="Home.aspx">Home</a></li>
						<li>Admin</li>
						<li>Transfer Amount</li>
					</ol>
   
        <div id="divDetailSection" runat="server" style="margin-bottom: 30px; margin-right: 20px">
            <table cellspacing="10px" cellpadding="0%" style =" background-color :White; height:50%" width ="100%" ">
                <tbody>
                             
              <tr>
                <td colspan="4" style=" width: 100%">
                   <%-- <asp:Label runat="server" Style="margin-bottom: 11px" ID="lblMessage"
                        Text="Transfer Amount" Font-Bold="true" Font-Size="15px"></asp:Label>--%>
                    <br />
                </td>
            </tr>
            
                
                 <tr  style ="margin-top :20px; padding-top :20px">
                 
                        <td width="14%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong>Member Id :&nbsp;</strong>
                        </td>
                        <td>                           
                          
                            <asp:TextBox ID="TextMemberId" CssClass="TxtBox" runat="server" Width="200px" ForeColor="black" Font-Bold="true" Font-Size="12px" AutoPostBack="true"  ></asp:TextBox><br />
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr >
                    <td colspan ="4"></td></tr>
                    <br />
                <tr style ="margin-top :20px;padding-top :20px">
                        <td width="14%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong>Member Name :&nbsp;</strong>
                        </td>
                        <td>                           
                           
                            <asp:TextBox ID="TextMemberName" CssClass="TxtBox" runat="server" Width="200px" ForeColor="black" Font-Bold="true" Font-Size="12px" ></asp:TextBox><br />
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr >
                    <td colspan ="4"></td></tr>
                    <br />
                <tr  style ="margin-top :20px; padding-top :20px">
                        <td width="15%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong>Transfer Amount :&nbsp;</strong>
                        </td>
                        <td>                           
                           
                            <asp:TextBox ID="TextAmount" CssClass="TxtBox" runat="server" Width="200px" ForeColor="black" Font-Bold="true" Font-Size="12px" ></asp:TextBox><br />
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr >
                    <td colspan ="4"></td></tr>
                    
                    <tr  style ="margin-top :20px; padding-top:20px">
                        <td width="14%" align="left" valign="middle" style="padding-left :20px;" >
                           <%--<strong>Transfer Amount :&nbsp;</strong>--%>
                        </td>
                        <td>  
                        <asp:Button ID="BtnTransfer" runat="server" Text="Transfer Amount" CssClass="Btn" />                          
                           
                            <%--<asp:TextBox ID="TextBox1" CssClass="TxtBox" runat="server" Width="200px" ForeColor="black" Font-Bold="true" Font-Size="12px" ></asp:TextBox>--%>
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    
                   <tr>
                <td colspan="4" style=" width: 100%">
                   <%-- <asp:Label runat="server" Style="margin-bottom: 11px" ID="lblMessage"
                        Text="Transfer Amount" Font-Bold="true" Font-Size="15px"></asp:Label>--%>
                    <br />
                </td>
            </tr>
                    <tr >
                    <td colspan ="4"></td></tr>
                    <tr >
                        <td align="Left" valign="middle">
                        <asp:Label ID="LblError" runat="server" visible="false"></asp:Label>
                        </td>
                        </tr> 
                    
                                   </tbody>
            </table>
        </div>
    </div>
</asp:Content>
