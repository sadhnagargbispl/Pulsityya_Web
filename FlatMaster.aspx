<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="FlatMaster.aspx.vb" Inherits="FlatMaster" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
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
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center1 img
        {
            height: 115px;
            width: 120px;
        }
    </style>
    <style type="text/css">
        .PopCal
        {
            z-index: 100;
            background-color: White;
            border: 1px solid black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="images/Loder.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Add/Edit Flat Master
                                </h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Project Name</label>
                                            <asp:Label ID="LblId" runat="server" Visible="false"></asp:Label>
                                            <asp:TextBox ID="TxtProjectName" runat="server" CssClass="form-control" ValidationGroup="Save">
                                            </asp:TextBox>
                                            <asp:Label ID="LblImage" runat="server" Visible="false" ></asp:Label>
                                     
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="TxtProjectName"
                                                ErrorMessage="Enter Project Name" ValidationGroup="Save" SetFocusOnError="true"
                                                Display="Dynamic" runat="server">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Address</label>
                                            <asp:TextBox ID="TxtAddress" runat="server" CssClass="form-control" ValidationGroup="Save">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="TxtAddress"
                                                ErrorMessage="Enter Address" ValidationGroup="Save" SetFocusOnError="true" runat="server">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Location</label>
                                            <asp:TextBox ID="TxtLocation" runat="server" CssClass="form-control">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="TxtLocation"
                                                ErrorMessage="Enter Location" ValidationGroup="Save" SetFocusOnError="true" runat="server">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>
                                                Floor</label>
                                            <asp:DropDownList ID="DDlFloor1" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>
                                                Unit</label>
                                            <asp:TextBox ID="txtUnit1" runat="server" CssClass="form-control" AutoPostBack="true"  onkeypress="return isNumberKey(event);">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-10">
                                        <asp:GridView ID="Gvfloor1" runat="server" AutoGenerateColumns="False" Width="100%"
                                            class="table table-bordered" HeaderStyle-CssClass="bg-primary">
                                            <Columns>
                                                <asp:TemplateField HeaderText="FlatNo">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFlatNo" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Flat Type">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="DDlFlatType" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Area">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtArea" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtAmount" runat="server" CssClass="form-control "  onkeypress="return isNumberKey(event);"></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Video Link">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtVideoLink" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                             
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>
                                                Floor</label>
                                            <asp:DropDownList ID="DDlFloor2" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>
                                                Unit</label>
                                            <asp:TextBox ID="TxtUnit2" runat="server" CssClass="form-control" AutoPostBack="true">
                                            </asp:TextBox>
                                        </div>
                                        
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-10">
                                        <asp:GridView ID="GvFloor2" runat="server" AutoGenerateColumns="False" Width="100%"
                                            class="table table-bordered" HeaderStyle-CssClass="bg-primary">
                                            <Columns>
                                                <asp:TemplateField HeaderText="FlatNo">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFlatNo" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Flat Type">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="DDlFlatType" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Area">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtArea" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtAmount" runat="server" CssClass="form-control "  onkeypress="return isNumberKey(event);"></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Video Link">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtVideoLink" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>
                                                Floor</label>
                                            <asp:DropDownList ID="DDlFloor3" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>
                                                Unit</label>
                                            <asp:TextBox ID="TxtUnit3" runat="server" CssClass="form-control" AutoPostBack="true">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-10">
                                        <asp:GridView ID="GVFloor3" runat="server" AutoGenerateColumns="False" Width="100%"
                                            class="table table-bordered" HeaderStyle-CssClass="bg-primary">
                                            <Columns>
                                                <asp:TemplateField HeaderText="FlatNo">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFlatNo" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Flat Type">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="DDlFlatType" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Area">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtArea" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtAmount" runat="server" CssClass="form-control "  onkeypress="return isNumberKey(event);"></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Video Link">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtVideoLink" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                             
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>
                                                Floor</label>
                                            <asp:DropDownList ID="DDlFloor4" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>
                                                Unit</label>
                                            <asp:TextBox ID="TxtUnit4" runat="server" CssClass="form-control" AutoPostBack="true">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-10">
                                        <asp:GridView ID="GvFloor4" runat="server" AutoGenerateColumns="False" Width="100%"
                                            class="table table-bordered" HeaderStyle-CssClass="bg-primary">
                                            <Columns>
                                                <asp:TemplateField HeaderText="FlatNo">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFlatNo" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Flat Type">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="DDlFlatType" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Area">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtArea" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtAmount" runat="server" CssClass="form-control "  onkeypress="return isNumberKey(event);"></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Video Link">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtVideoLink" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                 
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>
                                                Floor</label>
                                            <asp:DropDownList ID="ddlFloor5" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>
                                                Unit</label>
                                            <asp:TextBox ID="TxtUnit5" runat="server" CssClass="form-control" AutoPostBack="true">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-10">
                                        <asp:GridView ID="GvFloor5" runat="server" AutoGenerateColumns="False" Width="100%"
                                            class="table table-bordered" HeaderStyle-CssClass="bg-primary">
                                            <Columns>
                                                <asp:TemplateField HeaderText="FlatNo">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFlatNo" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Flat Type">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="DDlFlatType" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Area">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtArea" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtAmount" runat="server" CssClass="form-control "  onkeypress="return isNumberKey(event);"></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Video Link">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtVideoLink" runat="server" CssClass="form-control "></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Distance 1
                                            </label>
                                            <asp:TextBox ID="TxtDistance1" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RDistnace1" ControlToValidate="TxtDistance1" ErrorMessage="Enter Distance 1"
                                                ValidationGroup="Save" runat="server">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Distance 2
                                            </label>
                                            <asp:TextBox ID="TxtDistance2" runat="server" CssClass="form-control "></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Distance 3
                                            </label>
                                            <asp:TextBox ID="TxtDistance3" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Upload Image
                                            </label>
                                            <asp:FileUpload ID="FileUpload1" runat="server" multiple="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Video Link
                                            </label>
                                            <asp:TextBox ID="txtVideolink" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Description
                                            </label>
                                            <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                MaxLength="1000"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="TxtDesc"
                                                ErrorMessage="Enter Description" ValidationGroup="Save" runat="server">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Site Incharge
                                            </label>
                                            <asp:TextBox ID="TxtSiteIncharge" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="TxtSiteIncharge"
                                                ErrorMessage="Enter Site Incharge" ValidationGroup="Save" runat="server">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Name
                                            </label>
                                            <asp:TextBox ID="Txtname" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="Txtname"
                                                ErrorMessage="Enter Name" ValidationGroup="Save" runat="server">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Mobile No
                                            </label>
                                            <asp:TextBox ID="TxtMobileno" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TxtMobileno"
                                                ErrorMessage="Enter Mobile No" ValidationGroup="Save" runat="server">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Active Status
                                            </label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                                                <asp:ListItem Value="N">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="Save" />
                                            <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" />
                                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowSummary="False"
                                                ValidationGroup="Save" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
