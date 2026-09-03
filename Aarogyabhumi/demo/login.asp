<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Login | Pulastya Globals India (P) Limited | A Better Tomorrow Together</title>
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
                <div class="login-col"> 
                    <h3>Are you Registered ?</h3>
                    <p>Have an account with us? Log in to get started.</p>
                    <div class="form-group">
                        <label>Email ID / Username</label>
                        <input name="buyer_email" type="text"   id="" class="form-control" >
                    </div>
                    <div class="form-group fo-pass-block">
                        <label>Password</label>
                        <input name="buyer_pass" type="password"  id="" class="form-control fo-pass" >
                        <span toggle=".fo-pass" class="toggle-password open-eye"></span> 
                    </div>
                    <a href="" class="link-forgot">Forgot password ?</a>
                    <input type="submit" name="submit" value="Login" id="" class="btn btn-sub"> 
                    <p class="link-register">Don't have an account yet ? <a href="register.asp">Sign up</a> now.</p>
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

