<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="CommunityTree.aspx.vb" Inherits="CommunityTree" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<link href="../Resources/CSS/Grid.css" rel="stylesheet" type="text/css" />--%>
    <%--<center>
				<table cellpadding="0" cellspacing="0" border="0" class="MainRounded" width="100%">
					<tbody valign="top">
						<tr>
							<td style="width:100%" valign="top">
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td align="left" valign="top">
											<table id="Table1" cellspacing="0" cellpadding="4" style="padding-top :0px">
												<tbody>
													<tr valign="top">
														<td >&nbsp;Downline Member&nbsp;ID</td>
														<td> 
														<asp:TextBox CssClass="TxtBox" ID="txtDownLineFormNo" maxlength="15" style="width:110px;" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtDownLineFormNo" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>

													    </td>
														<td >Down Level</td>
														
												        <td> 
														<asp:TextBox CssClass="TxtBox" ID="txtDeptlevel" maxlength="4" style="width:62px;" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtDeptlevel" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
													    </td>
														<td ><asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="true" ValidationGroup="Save"  CssClass="Btn" /></td>
														<td ><asp:Button ID="cmdBack" runat="server" Text="Back" CausesValidation="false" CssClass="Btn" /></td>
													</tr>
												</tbody>
											</table>
                                    	</td>
                                    	<td style="width:100%" valign="top">
								<table cellpadding="0" cellspacing="0" width="100%">
									<tr>
									<td  align="left" >
									<table>
									<tr>
									<td>
									  <asp:GridView ID="Gvdata" runat="server"  AutoGenerateColumns="False"  CssClass="mGrid"  
    PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" Width="300px"  >                             
                                           
									<Columns >
								<asp:TemplateField   HeaderText="S.No" ControlStyle-CssClass ="GridItem ">
									<ItemTemplate >
									<%#Container.DataItemIndex + 1%></ItemTemplate></asp:TemplateField >
									<asp:BoundField   DataField ="MLevel" HeaderText="Level" ControlStyle-CssClass ="GridItem "  />
									<asp:BoundField   DataField ="RequiredId" HeaderText="Required Id"  ControlStyle-CssClass ="GridItem " />
									<asp:BoundField   DataField ="FilledId" HeaderText="Filled Id" ControlStyle-CssClass ="GridItem " />
									<asp:BoundField   DataField ="RemainId" HeaderText="Remain Id" ControlStyle-CssClass ="GridItem " />
									<asp:BoundField DataField ="LevelDate" HeaderText=" Level Date" ControlStyle-CssClass ="GridItem" />
									</Columns>
									</asp:GridView></td></tr>
									</table>
									
									</td>
										
									</tr>
									</table>
									</td>
									</tr>
								
									<tr>
										<td align="center" style="width:100%;height:380px;" valign="top" colspan ="2">
											<iframe name="TreeFrame" frameborder="0" scrolling="auto" src="" width="100%" height="100%"
												id="TreeFrame" runat="server"></iframe>
                                         </td>
									</tr>
								</table>
							</td>
						</tr>
					</tbody>
				</table>
			</center>--%>
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Network View</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive">
                            <div class="col-sm-12 pull-none">
                                <div class="col-sm-3 pull-none">
                                    <div class="form-group ">
                                        <label for="inputdefault">
                                            Downline Member ID</label>
                                        <asp:TextBox class="form-control" ID="txtDownLineFormNo" MaxLength="15" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtDownLineFormNo"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 pull-none">
                                    <div class="form-group ">
                                        <label for="inputdefault">
                                            Down Level</label>
                                        <asp:TextBox class="form-control" ID="txtDeptlevel" MaxLength="4" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtDeptlevel"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                
                                <div class="col-sm-3 pull-none">
                                    <div class="form-group ">
                                        <label for="inputdefault">
                                            ReEntry</label>
                                        <asp:TextBox class="form-control" ID="TxtReEntry" MaxLength="4" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="TxtReEntry"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 pull-none" style="padding-top: 2%">
                                    <div class="form-group">
                                        <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="btn btn-primary" />
                                        <asp:Button ID="cmdBack" runat="server" Text="Back" class="btn btn-primary" /></div>
                                </div>
                                <div class="col-sm-4 pull-none">
                                    <asp:DataGrid ID="Gvdata" runat="server" CssClass="table table-striped table-bordered"
                                        AutoGenerateColumns="false" Visible="false">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Container.DataSetIndex + 1%></ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="MLevel" HeaderText="Level" />
                                            <asp:BoundColumn DataField="RequiredId" HeaderText="Required Id" />
                                            <asp:BoundColumn DataField="FilledId" HeaderText="Filled Id" />
                                            <asp:BoundColumn DataField="RemainId" HeaderText="Remain Id" />
                                            <asp:BoundColumn DataField="LevelDate" HeaderText="Level Date" />
                                        </Columns>
                                    </asp:DataGrid>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 pull-none" style="position: inherit">
                            <div class="form-group ">
                                <center>
                                    <iframe name="TreeFrame" frameborder="0" scrolling="auto" width="100%" height="500"
                                        id="TreeFrame" runat="server"></iframe>
                                </center>
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
</asp:Content>
