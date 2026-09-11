$(document).ready(function() {
	
	wow = new WOW(
		{
			boxClass:     'wow',      // default
			animateClass: 'animated', // default
			offset:       0,          // default
			mobile:       true,       // default
			live:         true        // default
		}
	)

	wow.init();   
	
	// header fixing

	$(window).scroll(function() {
		var scrollTop = $('.tp-row').height() ; 	
		if ($(this).scrollTop() > scrollTop){  				 
			$('header').addClass("header-scrolled");
		}else{
			$('header').removeClass("header-scrolled");
		}
	}); 
	
	// menu scrolling
	
	if($(window).width() > 991){
		var widthTpMenu = $('.tp-menu').innerWidth();
		var widthMenuContainer = $('.tp-menu-container').innerWidth();
		var widthDiff = widthTpMenu - widthMenuContainer;
		
		if(widthTpMenu > widthMenuContainer){
			$('.pull-right').show();
		}
		
		$('.tp-menu-container').scroll(function() {
			if($(this).scrollLeft() > widthDiff){ 
				$('.pull-right').hide();	 
			}else{
				$('.pull-right').show();
			} 
			if($(this).scrollLeft() > 50){
				$('.pull-left').show(); 
			}else{
				$('.pull-left').hide(); 
			}
		});
		
		$('.pull-right').click(function() {
			event.preventDefault();
			$('.tp-menu-container').animate({
				scrollLeft: "+=200px"
			}, "slow");
		});
	
		 $('.pull-left').click(function() {
			event.preventDefault();
			$('.tp-menu-container').animate({
				scrollLeft: "-=200px"
			}, "slow");
		}); 
	} 
	 
	// desktop menu
	
	$('.tp-menu-row .stge-1 > li.has').hover(function() {  				
		$(this).children('a:first').addClass("active");
	}, function() {  
		$(this).children('a:first').removeClass("active");
	}); 
	
	// drawer menu
	
	$(document).on('click', '.mb-menu', function() { 
		$('body').append('<div class="bdy-ovrlay for-nav"></div>').find('.bdy-ovrlay').fadeIn(); 
		$('.mb-menu-col').animate({    
			left: "0"
		}, 200);  
	});
	
	$(document).on('click', '.mb-menu-col .clse-btn',function() {   
		$(this).parents('.mb-menu-col').animate({  
			left: "-300px"
		}, 200);  
		$('body').find('.bdy-ovrlay.for-nav').fadeOut(function(){$(this).remove()});
	}); 
	
	$(document).on('click', '.mb-menu-lst ul li.has', function(){ 
		$('.mb-menu-lst ul li').find('ul').slideUp(200);
		$('.mb-menu-lst ul li').find('ul').parents('li').addClass('has').removeClass('hasnt');
		
		$(this).find('a').next('ul').slideDown(200);	
		$(this).addClass('hasnt').removeClass('has');
	})
		
	$(document).on('click', '.mb-menu-lst ul li.hasnt', function(){
		$(this).find('a').next('ul').slideUp(200);	
		$(this).addClass('has').removeClass('hasnt');
	})   
	 
	$('.mb-menu-lst.main').html('<ul>' + $('.stge-1').html() + '</ul>');
	
	var menuStge_2 = $('.mb-menu-lst.main').find('.stge-2');
	menuStge_2.unwrap(); 
	menuStge_2.removeAttr('class');
	
	// product filter mobile  
	 
	$(document).on('click', '.itm-lst-row .mb-fltr', function() { 
		$('body').append('<div class="bdy-ovrlay for-fltr"></div>').find('.bdy-ovrlay').fadeIn(); 
		$('.mb-fltr-col').animate({    
			right: "0"
		}, 200);  
	});
	
	$(document).on('click', '.mb-fltr-col .clse-btn',function() {   
		$(this).parents('.mb-fltr-col').animate({  
			right: "-300px"
		}, 200);  
		$('body').find('.bdy-ovrlay.for-fltr').fadeOut(function(){$(this).remove()});
	}); 

	$(document).on('click', '.bdy-ovrlay.for-fltr',function() {   
		$('.mb-fltr-col').animate({  
			right: "-300px"
		}, 200);  
		$(this).fadeOut(function(){$(this).remove()});
	});

	// password hide & view

	$(document).on('click', '.fo-pass-block .toggle-password', function(){
		 $(this).toggleClass("open-eye close-eye");
		 var input = $(this).parents(".fo-pass-block").find('input'); 
		 if(input.attr('type')=='password'){
			input.attr('type','text');
		 }else{
			input.attr('type','password'); 
		 } 
	})   

	// hero slider banners

	$('#home-slider').nivoSlider({
		manualAdvance:false,
		directionNav: false,
		animSpeed: 1000,
		effect: 'sliceDown',
		slices: 18,
		pauseTime: 5000,
		pauseOnHover: false,
		controlNav: true,
		prevText: '<i class="fa fa-angle-left"></i>',
		nextText: '<i class="fa fa-angle-right"></i>'
	})

	// hero slider banners with carousel
	 
	$(".slide-carousel").owlCarousel({
		margin: 10,
		stagePadding: 0, 
		loop: $(this).find('.slide').length > 1,
		autoplay:true,
		autoplaySpeed: 1000,
		autoplayTimeout: 3000,
		dots: true,
		nav: false,
		items:1    
	})
	
	// product scrolling
	
	$(".product-carousel").owlCarousel({
		margin: 10,
		stagePadding: 0, 
		loop: $(this).find('.item').length > 4,
		autoplay:true,
		autoplaySpeed: 1000,
		autoplayTimeout: 3000,
		dots: false,
		nav: true,
		rtl: true,
		responsive:{
			320:{items:2},
			768:{items:3},
			991:{items:4},
			1199:{items:4}
		}     
	}) 

	// product page image scrolling

	$(".itm-dtls-carousel").owlCarousel({
		margin: 15,
		stagePadding: 0, 
		loop: $(this).find('.item').length > 1,
		autoplay:false,
		autoplaySpeed: 1000,
		autoplayTimeout: 3000,
		dots: false,
		nav: true,
		thumbs: true,
		responsive:{
			320:{
				items:1
			} 
		}     
	}) 
});
  

// filter dropdown

$('.filter-by').change(function(){ 
	if($(this).val() == 1) {
		var divList = $(".col-item");
		divList.sort(function(a, b){ return $(a).data("id")-$(b).data("id")});
		$(".pr_list .row").html(divList);
	} else if($(this).val() == 2) {
		var divList = $(".col-item");
		divList.sort(function(a, b){ return $(b).data("id")-$(a).data("id")});
		$(".pr_list .row").html(divList);
	} else if($(this).val() == 3) {
		var divList = $(".col-item");
		divList.sort(function(a, b){ return $(a).data("listing-price")-$(b).data("listing-price")});
		$(".pr_list .row").html(divList);
	} else if($(this).val() == 4) {
		var divList = $(".col-item");
		divList.sort(function(a, b){ return $(b).data("listing-price")-$(a).data("listing-price")});
		$(".pr_list .row").html(divList);
	}  
});  


// qunatity increament

$(document).on('click', '.num-incr-decr .decrMent', function() { 
	var inptBox = $(this).parent().find('input[type=text]');
	var valStep = inptBox.attr('step');
	var valMin = $(this).attr('dt-min');

	var valNew = Number(inptBox.val()) - Number(valStep) ;
	
	if(valNew >= valMin){ 
		inptBox.val(valNew);
		$('.cart-btn-col input[name=item_qty]').val(valNew);
	} 
}); 

// quantity decreament

$(document).on('click', '.num-incr-decr .incrMent', function() { 
	var inptBox = $(this).parent().find('input[type=text]');
	var valStep = inptBox.attr('step');
	var valMax = $(this).attr('dt-max');

	var valNew = Number(inptBox.val()) + Number(valStep) ;
	
	if(valNew <= valMax){ 
		inptBox.val(valNew);
		$('.cart-btn-col input[name=item_qty]').val(valNew);
	}  
});

// loader 

var loader  = '<div class="loader">';
	loader += '<div class="msg" ><img src="img/icon-load.jpg">Please wait . . .</div></div>' ; 

var pg_loader  = '<div class="loader pg">';
	pg_loader += '<svg class="circle" width="48px" height="48px">';
    pg_loader += '<circle class="path-bg" cx="24" cy="24" r="22" fill="none" stroke-width="4" stroke="#eee"></circle>';
    pg_loader += '<circle class="path" cx="24" cy="24" r="22" fill="none" stroke-width="4" stroke-miterlimit="10" stroke="#176a3a"></circle>';
	pg_loader += '</svg>';
	pg_loader += '</div>';

// delete message

function DelMsg(btn){  
	$(btn).parents('.msg').removeClass('zoomIn').addClass('zoomOut').parents('.msg-overlay').fadeOut(); 
	setTimeout(function(){
	  $(btn).parents('.msg-overlay').remove();	
	}, 2000);
} 

// checking is valid number

function isNumber(evt) {
	evt = (evt) ? evt : window.event;
	var charCode = (evt.which) ? evt.which : evt.keyCode;
	if (charCode > 31 && (charCode < 48 || charCode > 57)) {
		return false;
	}
	return true;
}  

// page loader

$('body').append(pg_loader); 
	
setTimeout(function() { 
	if($('.loader').length > 0) { 
		$('.loader').fadeOut(function() {
			$('.loader').remove();
		});
	}
}, 200);

function shareItem(title, itemURL) {
    if (navigator.share) {
        navigator.share({
            title: title,
            text: '',
            url: itemURL
        });
    }else{
        alert('Sharing not supported');
    }
}