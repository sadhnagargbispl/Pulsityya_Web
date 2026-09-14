<%@ Page Language="VB" AutoEventWireup="false" CodeFile="viewPatient.aspx.vb" Inherits="App_UI_Application_Pages_viewPatient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/custom.min.css" rel="stylesheet">
</head>
<body>

    <form id="form1" runat="server">
    <div style=" background-color :White; overflow :scroll;">
   <%-- <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">--%>
                    <div class="x_title">
                        <h2>
                            Patient Member Details</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <%--<div class="col-md-12">
                            <div class="col-md-4">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" PageSize="20" Width="430px" EmptyDataText="No data to display.">
                                    <Columns>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="col-md-8">
                            </div>
                        </div>--%>
                        <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Name Of Patient :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtnameofpatient" runat="server" ></asp:TextBox>
                                    </div>
                                    
                               <%-- </div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Father/Husband Name :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtFatherName" runat="server" ></asp:TextBox></div>
                                   
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Contact No :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtcontactno" runat="server" ></asp:TextBox></div>
                                    
                               <%-- </div>
                                <div class="col-md-12" >--%>
                                    <div class="col-md-2">
                                        Address :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtaddress" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Age :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtage" runat="server" ></asp:TextBox></div>
                                <%--</div>
                                <div class="col-md-12" >--%>
                                    <div class="col-md-2">
                                        Marital Status :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtMaritalStatus" runat="server" ></asp:TextBox></div>
                               </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Occupation :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtOccupation" runat="server" ></asp:TextBox></div>
                                   
                                <%--</div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Children :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtChildren" runat="server" ></asp:TextBox></div>
                                    
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Genetic Disease :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtGeneticDisease" runat="server" ></asp:TextBox></div>
                                   
                                <%--</div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Family History :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtFamilyHistory" runat="server" ></asp:TextBox></div>
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Cancer :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtCancer" runat="server" ></asp:TextBox></div>
                                    
                                <%--</div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Parkinson :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtParkinson" runat="server" ></asp:TextBox></div>
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Arthritis :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtArthritis" runat="server" ></asp:TextBox></div>
                                    
                                <%--</div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                       Heart Disease  :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtHeartDisease" runat="server" ></asp:TextBox></div>
                                    
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                         BP :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtBP" runat="server" ></asp:TextBox></div>
                                <%--</div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Thyroid :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtThyroid" runat="server" ></asp:TextBox></div>
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                       Chickenpox :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtChickenpox" runat="server" ></asp:TextBox></div>
                               <%-- </div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Typhoid :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtTyphoid" runat="server" ></asp:TextBox></div>
                                </div>
                                
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Jaundice :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtJaundice" runat="server" ></asp:TextBox></div>
                                    
                              <%--  </div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Measles :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtMeasles" runat="server" ></asp:TextBox></div>
                                    <%--<div class="col-md-2">
                                    </div>--%>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Dengue :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtDengue" runat="server" ></asp:TextBox></div>
                                   
                               <%-- </div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Accident :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtAccident" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Operation :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtOperation" runat="server" ></asp:TextBox></div>
                                    
                                <%--</div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Hospitalization :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtHospitalization" runat="server" ></asp:TextBox></div>
                                    
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Constipation :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtConstipation" runat="server" ></asp:TextBox></div>
                                    
                              <%--  </div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Gas Acidity :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtGasAcidity" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Sleep :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtSleep" runat="server" ></asp:TextBox></div>
                              <%--  </div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Headache :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtHeadache" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Pain :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtPain" runat="server" ></asp:TextBox></div>
                                <%--</div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Craving :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtCraving" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Sweet :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtSweet" runat="server" ></asp:TextBox></div>
                                   <%--
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Spicy :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtSpicy" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Restlessness :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtRestlessness" runat="server" ></asp:TextBox></div>
                                <%--</div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Grief :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtGrief" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Depression :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtDepression" runat="server" ></asp:TextBox></div>
                                   
                               <%-- </div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Alcohol :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtAlcohol" runat="server" ></asp:TextBox></div>
                                    
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Smoking :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtSmoking" runat="server" ></asp:TextBox></div>
                               <%-- </div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        TobaccoChewing :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtTobaccoChewing" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Sinusitis :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtSinusitis" runat="server" ></asp:TextBox></div>
                                    <%--
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                   --%> 
                                   <div class="col-md-2">
                                        Cold Cough :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtColdCough" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                       Allergy :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtAllergy" runat="server" ></asp:TextBox></div>
                                    
                                <%--</div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Nasal Polyps :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtNasalPolyps" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Ears :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtEars" runat="server" ></asp:TextBox></div>
                                    <%--
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Eyes :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtEyes" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Hair :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtHair" runat="server" ></asp:TextBox></div>
                                <%--</div>
                                <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Mouth :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtMouth" runat="server" ></asp:TextBox></div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Throat :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtThroat" runat="server" ></asp:TextBox></div>
                                <%--</div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Lungs :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtLungs" runat="server" ></asp:TextBox></div>
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Nose :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtNose" runat="server" ></asp:TextBox></div>
                                    
                                <%--</div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Stomach :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtStomach" runat="server" ></asp:TextBox></div>
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Spleen :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtSpleen" runat="server" ></asp:TextBox></div>
                                <%--</div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Renal Problem :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtRenalProblem" runat="server" ></asp:TextBox></div>
                                    
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Kidney Stone :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtKidneyStone" runat="server" ></asp:TextBox></div>
                                <%--</div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        UTI :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtUTI" runat="server" ></asp:TextBox></div>
                                  
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-2">
                                        Knees :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtKnees" runat="server" ></asp:TextBox></div>
                                <%--</div>
                                 <div class="col-md-12" style="padding-top: 1%">--%>
                                    <div class="col-md-2">
                                        Date :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtdate" runat="server" ></asp:TextBox></div>
                                </div>                     
                                
                                
                    </div>
              <%--</div>
            </div>
        </div>
    </div>--%>
    </div>
    </form>
</body>
</html>
