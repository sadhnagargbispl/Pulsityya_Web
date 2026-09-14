<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddSeminarWithMrp.aspx.vb" Inherits="App_UI_Application_Pages_AddSeminarWithMrp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" tagPrefix="AjaxToolkit"  %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <AjaxToolkit:ToolkitScriptManager ID="scriptmanager1" runat="server">
    </AjaxToolkit:ToolkitScriptManager>
   
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Seminar Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Seminar Date *
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtDate" class="form-control" runat="server"></asp:TextBox>
                                        <asp:TextBox Visible="false" Width="10px" runat="server" ID="TxtMeetingID"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="TxtDate">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="TxtDate"
                                            runat="server" ErrorMessage="*" ValidationGroup="Form-submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtDate"
                                            ErrorMessage="Invalid" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            Display="Dynamic" ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Seminar Time *
                                    </div>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DdlHH" Width="60px" class="form-control" style="display :inline ;padding:0px 0px;" runat="server">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="DdlMM" Width="50px" runat="server" class="form-control" style="display :inline ;padding:0px 0px;">
                                            <asp:ListItem Value="0" Text="00" />
                                            <asp:ListItem Value="15" Text="15" />
                                            <asp:ListItem Value="30" Text="30" />
                                            <asp:ListItem Value="45" Text="45" />
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="Ddltt" Width="50px" runat="server" class="form-control" style="display :inline ;padding:0px 0px;">
                                            <asp:ListItem Value="PM" Text="PM" />
                                            <asp:ListItem Text="AM" Value="AM" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                         Seminar Program *
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtProg" class="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Display="Dynamic" ControlToValidate="TxtProg"
                                            ValidationGroup="Form-submit" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                          Seminar Speaker *
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtSpeaker" class="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="TxtSpeaker"
                                            ValidationGroup="Form-submit" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                         Seminar State *
                                    </div>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="CmbState" runat="server" AutoPostBack="true" class="form-control"
                                            TabIndex="14">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Seminar  District
                                   </div>
                                   <div class="col-md-6"> <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true" class="form-control"
                                               TabIndex="15">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:TextBox ID="TxtOtherDist" runat="server" class="form-control" 
                                                    placeholder="Other"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="CmbState" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                     <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                          Seminar City
                                   </div>
                                   <div class="col-md-6">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlTehsil" class="form-control" 
                                                    TabIndex="16" runat="server">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:TextBox ID="TxtOtherCity" runat="server" class="form-control" 
                                                    placeholder="Other"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlDistrict" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        </div> 
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                          Seminar Address *
                                    </div>
                                   <div class="col-md-6">
                                        <asp:TextBox ID="TxtAddr" class="form-control"  runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="TxtAddr"
                                            ValidationGroup="Form-submit" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                   </div> 
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Contact Person *
                                    </div> 
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtContPerson" class="form-control"  runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" ControlToValidate="TxtContPerson"
                                            ValidationGroup="Form-submit" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                     </div> 
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Contact No. *
                                   </div>
                                   <div class="col-md-6">
                                        <asp:TextBox ID="TxtContactNo" class="form-control"  runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Display="Dynamic" ControlToValidate="TxtContactNo"
                                            ValidationGroup="Form-submit" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </div> 
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Ticket MRP. *
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtMrp" class="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" Display="Dynamic" ControlToValidate="txtMrp"
                                            ValidationGroup="Form-submit" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                            FilterType="Numbers,Custom" TargetControlID="txtMrp" ValidChars=".0123456789">
                                        </AjaxToolkit:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                         Person *
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtPerson" class="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" Display="Dynamic" ControlToValidate="txtPerson"
                                            ValidationGroup="Form-submit" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                            FilterType="Numbers" TargetControlID="txtPerson" ValidChars="0123456789">
                                        </AjaxToolkit:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                
                                
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                          Seminar Description
                                    </div>
                                        <div class="col-md-6">
                                        <asp:TextBox ID="TxtRemarks" TextMode="MultiLine"  class="form-control"
                                            runat="server"></asp:TextBox>
                                   </div> 
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                   Status
                                    </div>
                                        <div class="col-md-6">
                                        <asp:RadioButtonList ID="RbStatus" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                        </asp:RadioButtonList>
                                     </div> 
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                       
                                    </div>
                                        <div class="col-md-6">
                                        <asp:Button ID="BtnAdd" runat="server" Text="ADD" class="btn btn-primary" ValidationGroup="Form-submit"
                                            Width="155px" />
                                    </div> 
                                    <div class="col-md-2">
                                    </div>
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
    </form>
</body>
</html>
