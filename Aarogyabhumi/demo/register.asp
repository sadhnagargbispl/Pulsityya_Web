<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Register | Pulastya Globals India (P) Limited | A Better Tomorrow Together</title>
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
    <div class="content-row"> 
        <div class="container">
            <div class="login-bar"> 
                <div class="login-col rgstr-col"> 
                    <h3>Become a Direct seller</h3>
                    <p>Please fill all the mandatory details to complete your registration.</p>
                    <div class="form-group">
                        <label class="label-radio"><input type="radio" checked>Direct seller</label>
                        <label><input type="radio">Customer</label>
                    </div> 
                    <!--  -->
                    <div class="row">
                        <div class="col-sm-6">
                             <div class="form-group">
                                <label>Sponsor ID</label>
                                <input type="text" class="form-control">
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label>Full Name</label>
                                <input type="text" class="form-control">
                            </div> 
                        </div>
                    </div> 
                    <!--  -->
                    <div class="row">
                        <div class="col-sm-6">
                             <div class="form-group">
                                <label>DOB</label>
                                <input type="date" class="form-control">
                            </div>
                        </div>
                        <div class="col-sm-6">
                             <div class="form-group">
                                <label>PAN No.</label>
                                <input type="date" class="form-control">
                            </div> 
                        </div>
                    </div> 
                    <!--  -->
                    <div class="row">
                        <div class="col-sm-6">
                             <div class="form-group">
                                <label>Email</label>
                                <input type="text" class="form-control">
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label>Mobile No.</label>
                                <input type="text" class="form-control" maxLength="10" onKeyPress="return isNumber(event)">
                            </div> 
                        </div>
                    </div> 
                    <!--  --> 
                    <div class="form-group" style="margin:0px 0 20px!important">
                        <label class="label-check">
                        <input type="checkbox" name="buyer_agree" id="buyer_agree" value="Yes" checked>
                        I agree with <a>Terms &amp; Conditions</a>. </label>
                    </div> 
                    <div class="row">
                        <div class="col-sm-3"><input type="submit"  value="Register" id="" class="btn btn-sub"> </div>
                        <div class="col-sm-9"><p class="link-login">have an account yet ? please <a href="login.asp">Login</a></p></div>
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

