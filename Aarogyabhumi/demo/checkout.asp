<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Checkout | Pulastya Globals India (P) Limited | A Better Tomorrow Together</title>
<meta name="viewport" content="width=device-width, initial-scale=1">
<link rel="icon" type="image/x-icon" href="img/favicon.ico"/> 
<meta name="theme-color" content="#176a3a" />
<meta name="language" content="EN" />
<meta name="audience" content="all">
<meta name="content-Language" content="English">
<meta name="distribution" content="global"> 
<!--css styles -->
<link rel="stylesheet" href="css/bootstrap.min.css">
<link rel="stylesheet" href="css/layout.css?ver=<%=Timer%>">
<link rel="stylesheet" href="css/layout-responsive.css?ver=<%=Timer%>"> 
<link rel="stylesheet" href="css/font-awesome.min.css">
<link rel="stylesheet" href="css/message.css" />
<!--nav styles-->
<link rel="stylesheet" href="css/menu.css?ver=<%=Timer%>"> 
<!--Plugins-->
<link rel="stylesheet" href="plugins/animation/css/animate.css"> 
<!--Fonts-->
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Nunito+Sans:ital,opsz,wght@0,6..12,200..1000;1,6..12,200..1000&display=swap" rel="stylesheet">
<!-- #include file="inc/other-inc.asp" -->
</head>
<body>
<!-- #include file="inc/header.asp" -->
<section class="contents"> 
    <div class="breadcrmb">
        <img src="img/page-banner.jpg" alt="About us">
        <div class="breadcrmb-content">
            <div class="container">
                <h1>Checkout</h1> 
                <ul class="breadcrmb-loc">
                    <li><a href="/">Home</a></li>
                    <li><a class="active">Checkout</a></li>
                </ul>
            </div>
        </div> 
    </div>
    <!--  -->
    <div class="content-row"> 
        <div class="container">
            <div class="cart-row">
                <div class="row">
                    <div class="col-sm-7 col-md-8">
                        <div class="container-address"> 
                            <h5>Shipping Address </h5>
                            <form enctype="multipart/form-data"> 
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label>Purchase By</label>
                                        <input type="text" class="form-control text-capitalize" value="">
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label>Email  ID</label>
                                        <input type="text" class="form-control" value="">
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label>Mobile No.</label>
                                        <input type="text" class="form-control" maxlength="10"  onkeypress="return isNumber(event)">
                                    </div>
                                </div>
                            </div>
                            <!--  -->
                            <div class="row"> 
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label>Address</label>
                                        <input type="text" class="form-control text-capitalize" value="">
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label>State</label>
                                        <input type="text" class="form-control text-capitalize" value="">
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label>City</label>
                                        <input type="text" class="form-control text-capitalize" value="">
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="form-group">
                                        <label>Pincode</label>
                                        <input type="text" class="form-control text-capitalize" value=""  maxlength="6"  onkeypress="return isNumber(event)">
                                    </div>
                                </div>
                            </div> 
                            </form>  
                        </div>
                    </div>
                    <div class="col-sm-5 col-md-4 ">
                        <div class="price-lst-smry">
                            <h5>Order Summary</h5>
                            <div class="form-group">
                                <label>Delivery Address</label> 
                                <select class="form-control">
                                    <option value="">Select Delivery Type</option>
                                    <option value="Self Pickup">Self Pickup</option>
                                    <option value="Courier">By Courier</option>
                                </select>
                            </div>
                            <div class="form-group"> 
                                <label>Topup</label> 
                                <select class="form-control"> 
                                    <option value="Upgrade">Upgrade</option>
                                    <option value="Repurchase">Repurchase</option>
                                </select>
                            </div>
                            <ul> 
                                <li><span class="lft">Total MRP</span><span class="rght">₹  500.00</span></li>
                                <li><span class="lft">Discount on MRP (-)</span><span class="rght">₹ 150.00</span></li>
                                <li class="ftr"><span class="lft">Grand Total </span><span class="rght">₹ 350.00</span></li>

                                <li><span class="lft">Shipping Fee (+)</span><span class="rght">₹ 60.00</span></li>
                                <li class="ftr"><span class="lft">Net Payable </span><span class="rght">₹ 410.00</span></li>
                            </ul>
                             
                            <ul class="smry-nte">
                                <li>Free Delivery on Order above Rs. 2500/-.</li>
                                <li> Tax included. Shipping calculated at checkout.</li>
                            </ul>
                            <div class="form-group" style="margin-bottom :0px!important">
                                <label class="label-check"><input type="checkbox">I agree with <a>Terms &amp; Conditions</a>. </label>
                            </div>
                             <div class="form-group" style="margin-bottom :10px!important">
                                <label class="label-check"><input type="checkbox">I agree with <a href="">Return</a> & <a href="">Shipping</a> policy. </label>
                            </div>
 
                            <a class="btn btn-sub btn-order" href="login">Proceed to Pay</a>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</section>  
<!-- #include file="inc/footer.asp" -->
<!-- scripts -->
<script src="js/jquery.min.js"></script>
<script src="js/bootstrap.min.js"></script>
<!--Plugins-->
<script src="plugins/animation/js/wow.min.js"></script>  
<!-- custom js -->
<script src="js/layout.js?ver=<%=Timer%>"></script>
<!-- disable js -->
<script src="js/inactive.js"></script> 
</body>
</html>

