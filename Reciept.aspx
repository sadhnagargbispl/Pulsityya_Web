<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Reciept.aspx.vb" Inherits="Reciept" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reciept </title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
<link rel="stylesheet" href="style.css">

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    
     <style type ="text/css">
 /*  #textarea-section {
    margin-top: 2%;
    padding: 2em;
    border: 1px solid #94c9ff63;
    background: #94c9ff1e;
}
.table {
    --bs-table-bg: transparent;
    --bs-table-accent-bg: transparent;
    --bs-table-striped-color: #212529;
    --bs-table-striped-bg: rgba(0, 0, 0, 0.05);
    --bs-table-active-color: #212529;
    --bs-table-active-bg: rgba(0, 0, 0, 0.1);
    --bs-table-hover-color: #212529;
    --bs-table-hover-bg: rgba(0, 0, 0, 0.075);
    width: 100%;
    margin-bottom: 1rem;
    color: #212529;
    vertical-align: top;
    border-color: #dee2e6;
}
table {
    caption-side: bottom;
    border-collapse: collapse;
}
*, ::after, ::before {
    box-sizing: border-box;
}
user agent stylesheet
table {
    display: table;
    border-collapse: separate;
    box-sizing: border-box;
    text-indent: initial;
    border-spacing: 2px;
    border-color: grey;
}*/
    </style>

 <script type="text/javascript" language="javascript">
     function PrintDiv() {

         var divContents = document.getElementById("dvContents").innerHTML;
         var printWindow = window.open('', '', 'height=550,width=570');
         printWindow.document.write('<html><head>');
         printWindow.document.write('</head><body >');
         printWindow.document.write(divContents);
         printWindow.document.write('</body></html>');
         printWindow.document.close();
         printWindow.print();
         }
  </script>
   
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <button type="button" class="btn btn-primary" id="BtnPrint" runat="server" onclick="javascript:PrintDiv();">
            Print</button>
    </div>
    <div class="container" id="dvContents" runat ="server" >
    <section class="textarea-section" id="textarea-section" style="margin-top: 2%; padding: 2em; border: 1px solid #94c9ff63; background: #94c9ff1e;">                                       
<div class="col-md-12" >
                <div class="col-md-5">
                    <asp:Label ID="LblDeviceId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="LblRefno" runat="server" Visible="false"></asp:Label>
                     <asp:Label ID="LblFormno" runat="server" Visible="false"></asp:Label>
                     <asp:Label ID="LblEmimonth" runat="server" Visible="false"></asp:Label>
                     
                </div>
                
          
                <!-- Logo -->
                    <div class="logo" style=" text-align :center ; ">
                       <%-- <img src="logo.png" alt="Company Logo">--%>
                       <img src="images/logo.png" style="max-width: 150px;" runat="server" id="imgLogo" />
                    </div>
                <!-- Logo -->

                <!-- Text -->
                    <div class="header" style=" text-align :center ; ">
                        <p><b>Receipt GST No : </b>09AADFW3588J1ZJ</p>
                        <h3>WELL VALUE INDIA PVT. LTD.</h3>
                        <p>Silver Plaza Complex, Near Jeevan Shah, Jhansi (UP) India - 284001</p>
                    </div>
                    <br>
                <!-- Text -->
               <!-- Table Start class="table table-striped table-bordered"-->
                    <table style=" width: 100%;" cellspacing="0">
                        <tbody>
                            <tr>
                                <td style="background: #DCDCDC; padding: 0.5em; border-top: 1px solid #bdbdbd;border-bottom: 1px solid #bdbdbd;border-left: 1px solid #bdbdbd;" colspan="8" ><b style="font-size :small">Reciept No : </b><asp:Label ID="lblrecieptNo" runat="server" Text=""></asp:Label></td>
                                <td style="background: #DCDCDC; padding: 0.5em;border-top: 1px solid #bdbdbd;border-right: 1px solid #bdbdbd; border-bottom: 1px solid #bdbdbd;" colspan="6"><b style="font-size :small">Date : </b><asp:Label ID="lblrecieptdate" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="padding: 0.5em; border-left:1px solid #bdbdbd;border-right:1px solid #bdbdbd;" colspan="8" >Buyer Name : <b style="font-size :small"><asp:Label ID="lblbuyername" runat="server" Text=""></asp:Label></b></td>
                                <td style="padding: 0.5em; border-right:1px solid #bdbdbd;" colspan="6" ><b style="font-size :small">Mobile No : </b><asp:Label ID="lblmobileno" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="background: #DCDCDC; padding: 0.5em; border: 1px solid #bdbdbd;" colspan="14"> Buyer's Address : <b style="font-size :small"><asp:Label ID="lblbuyerAddress" runat="server" Text=""></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td style="padding: 0.5em; border-left:1px solid #bdbdbd;border-right:1px solid #bdbdbd;" colspan="5"><b style="font-size :small">Booking Date  : </b><asp:Label ID="lblbookingdate" runat="server" Text=""></asp:Label></td>
                                <td style="padding: 0.5em; border-right:1px solid #bdbdbd;" colspan="5"><b style="font-size :small">Registry Date  : </b><asp:Label ID="lblregdate" runat="server" Text=""></asp:Label></td>
                                <td style="padding: 0.5em;border-right:1px solid #bdbdbd;" colspan="4"><b style="font-size :small">Site Name  : </b><asp:Label ID="lblsitename" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="background: #DCDCDC; padding: 0.5em; border-left:1px solid #bdbdbd;border-right:1px solid #bdbdbd;border-top:1px solid #bdbdbd;border-bottom:1px solid #bdbdbd;" colspan="2">Plot No  : <b style="font-size :small"><asp:Label ID="lblploteno" runat="server" Text=""></asp:Label></b></td>
                                <td style="background: #DCDCDC; padding: 0.5em; border-top:1px solid #bdbdbd;border-bottom:1px solid #bdbdbd;" colspan="4"> Rate  :<b style="font-size :small"><asp:Label ID="lblRate" runat="server" Text=""></asp:Label> </b></td>
                                <td style="background: #DCDCDC; padding: 0.5em; border-left:1px solid #bdbdbd;border-right:1px solid #bdbdbd;border-top:1px solid #bdbdbd;border-bottom:1px solid #bdbdbd;" colspan="3">Size  : <b style="font-size :small"><asp:Label ID="LblSize" runat="server" Text=""></asp:Label></b> </td>
                                <td style="background: #DCDCDC; padding: 0.5em; border-top:1px solid #bdbdbd;border-bottom:1px solid #bdbdbd;" colspan="4">Plot  : <b style="font-size :small"><asp:Label ID="lblplote" runat="server" Text=""></asp:Label></b></td>
                                <td style="background: #DCDCDC; padding: 0.5em; border-left:1px solid #bdbdbd;border-right:1px solid #bdbdbd;border-top:1px solid #bdbdbd;border-bottom:1px solid #bdbdbd;" colspan="1">Total Size :<b style="font-size :small"><asp:Label ID="lbltotalsize" runat="server" Text=""></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td style=" padding: 0.5em; border-left:1px solid #bdbdbd;border-right:1px solid #bdbdbd;"colspan="4">Installment No  :<b style="font-size :small"><asp:Label ID="lblinstallmentno" runat="server" Text=""></asp:Label></b></td>
                                <td style=" padding: 0.5em; "colspan="8">Installment Month  : <b style="font-size :small"><asp:Label ID="lblInstallmentmonth" runat="server" Text=""></asp:Label></b></td>
                                <td style=" padding: 0.5em;border-left:1px solid #bdbdbd;border-right:1px solid #bdbdbd;" colspan="2">Due Date  : <b style="font-size :small"><asp:Label ID="lblduedate" runat="server" Text=""></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td style=" background: #DCDCDC; padding: 0.5em; border: 1px solid #bdbdbd;" colspan="5"><b style="font-size :small">Total Ins. Amount : </b> <asp:Label ID="lbltotalinsAmount" runat="server" Text=""></asp:Label> </td>
                                <td style="background: #DCDCDC; padding: 0.5em; border-top: 1px solid #bdbdbd;border-bottom: 1px solid #bdbdbd;" colspan="5">Installment Amount : <b style="font-size :small"><asp:Label ID="lblinstallmentamt" runat="server" Text=""></asp:Label></b></td>
                                <td style="background: #DCDCDC; padding: 0.5em; border: 1px solid #bdbdbd;" colspan="4">Late Fees Total Amount  : <b style="font-size :small">0000.00</b></td>
                            </tr>
                            <tr>
                                <td style="padding: 0.5em;  border-left:1px solid #bdbdbd;" colspan="10"><b style="font-size :small">Due Amount : </b> <asp:Label ID="lbldueamount" runat="server" Text=""></asp:Label> </td>
                                <td style="padding: 0.5em; border-left:1px solid #bdbdbd;border-right:1px solid #bdbdbd;" colspan="4">Payment Mode/UPI : <b style="font-size :small"><asp:Label ID="lblpaymentMode" runat="server" Text=""></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td style="background: #DCDCDC; padding: 0.5em; border-top: 1px solid #bdbdbd;border-bottom: 1px solid #bdbdbd;border-left: 1px solid #bdbdbd; padding-top: 4em;" colspan="7"><b style="font-size :small">SELLER SIGN</b></td>
                                <td style="background: #DCDCDC; padding: 0.5em; padding-top: 4em;border: 1px solid #bdbdbd;" colspan="7"><b style="font-size :small">BUYER SIGN</b></td>
                            </tr>
                        </tbody>
                    </table>
                <!-- Table End -->

                <hr>

                <!-- Terms And Conditions -->
                    <div class="terms">
                        <h3>नियम व शर्ते </h3>
                        <ol class="olstyle">
                        <li>पूर्ण भुगतान करने पर ही प्लॉट की रजिस्ट्री की जाएगी। </li>
                        <li>रजिस्ट्री खर्च क्रेता द्वारा ही वहन किया जायेगा। </li>
                        <li>किस्तों वाले प्लॉट का एग्रीमेन्ट/नोटरी का खर्च क्रेता द्वारा वहन किया जायेगा।  </li>
                        <li>किस्त के भुगतान न करने पर प्रतिमाह रु0  500.00 का अतिरिक्त भुगतान देय होगा। </li>
                        <li>किसी भी दिशा में प्लॉट निरस्त होने पर जमा देय रकम का 25% का काटकर भुगतान किया जाएगा। </li>
                        <li>छः माह लगातार किस्तों का भुगतान न करने पर बुकिंग स्वतः निरस्त मानी जाएगी।  </li>
                        <li>बुकिंग के एक माह के अन्दर रजिस्ट्री कराना अनिवार्य होगी अन्यथा बुकिंग स्वतः ही निरस्त मानी जाएगी।  </li>
                        <li>समस्त विवादों का अन्तिम निर्णय विक्रेता का होगा एवं समस्त न्याय क्षेत्र झाँसी न्यायालय में होगा। </li>
                        </ol>
                    </div>
                <!-- Terms And Conditions -->
                  </div>
<%--</div> --%>
            </section>
             <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"
        integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p"
        crossorigin="anonymous"></script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js"
        integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF"
        crossorigin="anonymous"></script>
    </div>


   

    </form>
</body>
</html>
