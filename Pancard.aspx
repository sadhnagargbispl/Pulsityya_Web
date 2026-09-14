<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Pancard.aspx.vb" Inherits="Pancard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        input
        {
            text-transform: uppercase;
        }
    </style>
    <style type="text/css">
        .style1
        {
            height: 15%;
            width: 358px;
        }
        .style2
        {
            height: 2px;
            width: 304px;
        }
        .style3
        {
            height: 2px;
            width: 358px;
        }
    </style>

    <script type="text/javascript">
        function Validation() {

            if (rates = "true") {
                var a = document.getElementById("<%= txtPan.ClientId %>").value;
                a = a.toUpperCase();
                var regex1 = /^[A-Z]{5}\d{4}[A-Z]{1}$/;  //this is the pattern of regular expersion
                if (regex1.test(a) == false) {
                    alert('Please enter valid pan number');
                    document.getElementById("<%= txtPan.ClientId %>").value = ""
                    return false;
                }
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container bgwhite mt40 border1 pb20">
        <h3>
          Pan Card
        </h3>
      
                        <div class="table-responsive makeitresponsivegrid">
                            <div class="clr">
                                <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
                            </div>
                            <div class="col-lg-12 col-md-12 col-xs-12">
                                <div class="col-lg-7 col-md-12 col-xs-12">
                                    <div class="content-box">
                                        <p align="center">
                                            Dear
                                            <%=Session("MemName")%>
                                            (<asp:Label ID="lblid" runat="server"></asp:Label>), Update Your PAN Card Image.
                                        </p>
                                        <div class="form-group">
                                            <label for="inputdefault">
                                                Upload :</label>
                                            <asp:FileUpload ID="Fuidentity" runat="server" />
                                            <asp:Label ID="lblimage" runat="server" Visible="false"></asp:Label>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputdefault">
                                                Pan Card No. :</label>
                                            <asp:TextBox ID="txtpan" runat="server" class="form-control"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please check PAN Format"
                                                SetFocusOnError="true" ControlToValidate="txtpan" ValidationExpression="[A-Za-z]{5}\d{4}[A-Za-z]{1}"
                                                ValidationGroup="eInformation"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-offset-3 col-sm-9">
                                                <asp:Button ID="BtnIdentity" runat="server" ValidationGroup="eInformation" CssClass="btn btn-primary"
                                                    Text="submit" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-12 col-xs-12">
                                    <div class="image">
                                        <asp:Image ID="ShowIdentity" Width="250px" Height="250px" runat="server" />
                                        <br />
                                        <asp:Label ID="lblverstatus" Font-Bold="true" runat="server"></asp:Label>
                                        <asp:Label ID="Lblon" Font-Bold="true" Text="ON" runat="server"></asp:Label>
                                        <asp:Label ID="Lblverdate" Font-Bold="true" runat="server"></asp:Label>
                                        <br />
                                        <asp:Label ID="LblRemark" Font-Bold="true" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
