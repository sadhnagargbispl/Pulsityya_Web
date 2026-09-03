(function($) {

	'use strict';
	// Mean Menu
	$('.mean-menu').meanmenu({
		meanScreenWidth: "991"
	});
	
	// Sticky, Go To Top JS
	$(window).on('scroll', function() {
		// Header Sticky JS
		if ($(this).scrollTop() >200){  
			$('.navbar-area').addClass("is-sticky");
		}
		else{
			$('.navbar-area').removeClass("is-sticky");
		};
	});
	
    //PRE LOADER
    $(window).on('load',function(){
        var preload=$('.ctn-preloader');
        if(preload.length>0){
        preload.delay(800).fadeOut('slow');
    }})

    //Progress Bar
	if($('.progress-line').length){
		$('.progress-line').appear(function(){
			var el = $(this);
			var percent = el.data('width');
			$(el).css('width',percent+'%');
		},{accY: 0});
	}
	if($('.count-box').length){
		$('.count-box').appear(function(){
			var $t = $(this),
				n = $t.find(".count-text").attr("data-stop"),
				r = parseInt($t.find(".count-text").attr("data-speed"), 10);

			if (!$t.hasClass("counted")) {
				$t.addClass("counted");
				$({
					countNum: $t.find(".count-text").text()
				}).animate({
					countNum: n
				}, {
					duration: r,
					easing: "linear",
					step: function() {
						$t.find(".count-text").text(Math.floor(this.countNum));
					},
					complete: function() {
						$t.find(".count-text").text(this.countNum);
					}
				});
			}
			
		},{accY: 0});
	}

    //ScrollCue animation
    scrollCue.init({
        disable: function() {
        var maxWidth = 900;
        return window.innerWidth < maxWidth;
        }
    });
    
	// Others Option For Responsive JS 
	$(".others-option-for-responsive .dot-menu .inner").on("click", function(){
		$(".others-option-for-responsive .container .container").toggleClass("active");
	});

    //Testimonial Slider
    $('.testimonial-slider').owlCarousel({
        loop:true,
        margin:20,
        nav:true,
        dots:false,
        thumbs: false,
        thumbsPrerendered: false,
        autoplay:true,
        smartSpeed: 1000,
        autoplayHoverPause:true,
		navText: [
            '<i class="flaticon-left-arrow"></i>', 
            '<i class="flaticon-next"></i>'
        ],
        responsive:{
            0:{
                items:1, 
            },
            768:{
                items:2,
            },
            992:{
                items:3,
            },
            1200:{
                items:3,
            },
                
        }
    });

    //Best Selling Slider
    $('.best-selling-slider').owlCarousel({
        loop:true,
        margin:20,
        nav:true,
        dots:false,
        thumbs: false,
        thumbsPrerendered: false,
        autoplay:true,
        smartSpeed: 1000,
        autoplayHoverPause:true,
		navText: [
            '<i class="flaticon-left-arrow"></i>', 
            '<i class="flaticon-next"></i>'
        ],
        responsive:{
            0:{
                items:1, 
            },
            768:{
                items:2,
            },
            992:{
                items:3,
            },
            1200:{
                items:3,
            },
                
        }
    });
    
    //Category Slider
    $('.category-slider').owlCarousel({
        loop:true,
        margin:20,
        nav:true,
        dots:false,
        thumbs: false,
        thumbsPrerendered: false,
        autoplay:true,
        smartSpeed: 1000,
        autoplayHoverPause:true,
		navText: [
            '<i class="flaticon-left-arrow"></i>', 
            '<i class="flaticon-next"></i>'
        ],
        responsive:{
            0:{
                items:2, 
            },
            576:{
                items:2,
            },
            768:{
                items:3,
            },
            992:{
                items:5,
            },
            1200:{
                items:6,
            },
                
        }
    });

    //Instagram Slider
    $('.instagram-slider').owlCarousel({
        loop:true,
        margin:20,
        nav:false,
        dots:false,
        thumbs: false,
        thumbsPrerendered: false,
        autoplay:true,
        smartSpeed: 1000,
        autoplayHoverPause:true,
        responsive:{
            0:{
                items:2, 
            },
            576:{
                items:3,
            },
            768:{
                items:4,
            },
            992:{
                items:6,
            },
            1200:{
                items:8,
            },
                
        }
    });

    //Feedback Slider
    $('.feedback-slider').owlCarousel({
        loop:true,
        margin:20,
        nav:true,
        dots:false,
        thumbs: false,
        thumbsPrerendered: false,
        autoplay:true,
        smartSpeed: 1000,
        autoplayHoverPause:true,
		navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>', 
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive:{
            0:{
                items:1, 
            },
            768:{
                items:2,
            },
            992:{
                items:2,
            },
            1200:{
                items:2,
            },
                
        }
    });

    //Partner Slider
    $('.partner-slider').owlCarousel({
        loop:true,
        margin:20,
        nav:false,
        dots:false,
        thumbs: false,
        thumbsPrerendered: false,
        autoplay:true,
        smartSpeed: 1000,
        autoplayHoverPause:true,
        responsive:{
            0:{
                items:2, 
            },
            768:{
                items:4,
            },
            992:{
                items:5,
            },
            1200:{
                items:6,
            },
                
        }
    });

    //Banner Slider
    $('.banner-slider2').owlCarousel({
        loop:true,
        margin:0,
        nav:false,
        dots:true,
        thumbs: false,
        thumbsPrerendered: false,
        autoplay:true,
        smartSpeed: 1000,
        autoplayHoverPause:true,
        items:1,
    });

    //Banner Slider
    var owl = $('.banner-slider');
    owl.owlCarousel({
        loop: true,
        nav: true,
        dots: true,
        margin: 0,
        items: 1,
        smartSpeed: 2500,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        autoplay: true,
        autoplayHoverPause: true,
        responsiveClass: true,
        autoHeight: true,
        onInitialized  : counter, 
        onTranslated : counter,
        navText: [
            '<i class="flaticon-left-arrow"></i>', 
            '<i class="flaticon-next"></i>'
        ],
    });

    //Shop Slider
    $('.shop-slider').owlCarousel({
        loop:true,
        margin:10,
        nav:true,
        dots:false,
        thumbs: false,
        thumbsPrerendered: false,
        autoplay:true,
        smartSpeed: 1000,
        autoplayHoverPause:true,
		navText: [
            '<i class="flaticon-left-arrow"></i>', 
            '<i class="flaticon-next"></i>'
        ],
        responsive:{
            0:{
                items:1, 
            },
            576:{
                items:2,
            },
            768:{
                items:2,
            },
            992:{
                items:4,
            },
            1200:{
                items:4,
            },
                
        }
    });

    //Shop Slider2
    $('.shop-slider2').owlCarousel({
        loop:false,
        margin:10,
        nav:false,
        dots:false,
        thumbs: false,
        thumbsPrerendered: false,
        autoplay:true,
        smartSpeed: 1000,
        autoplayHoverPause:true,
		navText: [
            '<i class="flaticon-left-arrow"></i>', 
            '<i class="flaticon-next"></i>'
        ],
        responsive:{
            0:{
                items:1, 
            },
            576:{
                items:2,
            },
            768:{
                items:2,
            },
            992:{
                items:4,
            },
            1200:{
                items:4,
            },
                
        }
    });
    
    function counter(event) {
        var element   = event.target;
        var items     = event.item.count;    
        var item      = event.item.index + 1;     
        
        // it loop is true then reset counter from 1
        if(item > items) {
        item = item - items
        }
        $('.slide-counter').html("<span class='first-slide-count'>" + 0 + item + "</span>" + "<span class='total-slide-count'>" + 0 + items + "</span>")
    }

	//* magnific popup
	$(document).ready(function() {
        $('.popup-youtube, .popup-vimeo, .popup-gmaps').magnificPopup({
            disableOn: 100,
            type: 'iframe',
            mainClass: 'mfp-fade',
            removalDelay: 160,
            preloader: false,
            fixedContentPos: false
        });
    });

    $('.popup-gallery').magnificPopup({
		delegate: 'a',
		type: 'image',
		tLoading: 'Loading image #%curr%...',
		mainClass: 'mfp-img-mobile',
		gallery: {
			enabled: true,
			navigateByImgClick: true,
			preload: [0,1] 
		},
		image: {
			tError: '<a href="%url%">The image #%curr%</a> could not be loaded.',
			titleSrc: function(item) {
				return item.el.attr('title') + '<small>by Marsel Van Oosten</small>';
			}
		}
	});
    
    // MixItUp Shorting
    try {
        var mixer = mixitup('.shorting', {
            controls: {
                toggleDefault: 'none'
            }
        });
    } catch (err) {}

    // Tabs
    $('.tab-menu li a').on('click', function(){
        var target = $(this).attr('data-rel');
     $('.tab-menu li a').removeClass('active');
        $(this).addClass('active');
        $("#"+target).fadeIn('slow').siblings(".tab-box").hide();
        return false;
    });

    // Products Filter Options
    $(function(){
        $(".icon-view-one").on("click", function(e){
            e.preventDefault();
            document.getElementById("products-collections-filter").classList.add('products-col-one')
            document.getElementById("products-collections-filter").classList.remove('products-col-two', 'products-col-three', 'products-col-four', 'products-row-view');
        });
        $(".icon-view-two").on("click", function(e){
            e.preventDefault();
            document.getElementById("products-collections-filter").classList.add('products-col-two')
            document.getElementById("products-collections-filter").classList.remove('products-col-one', 'products-col-three', 'products-col-four', 'products-row-view');
        });
        $(".icon-view-three").on("click", function(e){
            e.preventDefault();
            document.getElementById("products-collections-filter").classList.add('products-col-three')
            document.getElementById("products-collections-filter").classList.remove('products-col-one', 'products-col-two', 'products-col-four', 'products-row-view');
        });
        $(".icon-view-four").on("click", function(e){
            e.preventDefault();
            document.getElementById("products-collections-filter").classList.add('products-col-four')
            document.getElementById("products-collections-filter").classList.remove('products-col-one', 'products-col-two', 'products-col-three', 'products-row-view');
        });
        $(".view-grid-switch").on("click", function(e){
            e.preventDefault();
            document.getElementById("products-collections-filter").classList.add('products-row-view')
            document.getElementById("products-collections-filter").classList.remove('products-col-one', 'products-col-two', 'products-col-three', 'products-col-four');
        });
        $(".icon-view-six").on("click", function(e){
            e.preventDefault();
            document.getElementById("products-collections-filter").classList.add('products-col-six')
            document.getElementById("products-collections-filter").classList.remove('products-col-one', 'products-col-two', 'products-col-three', 'products-col-four', 'products-row-view');
        });
    });
    $('.grid-view-content .view-column a').on('click', function(){
        $('.view-column a').removeClass("active");
        $(this).addClass("active");
    });

    //Range Slider
    try {
        const
        range = document.getElementById('range'),
        tooltip = document.getElementById('tooltip'),
        setValue = ()=>{
            try {
            const
                newValue = Number( (range.value - range.min) * 100 / (range.max - range.min) ),
                newPosition = 16 - (newValue * 0.32);
            tooltip.innerHTML = `<span>${range.value}$ </span>`;
            tooltip.style.left = `calc(${newValue}% + (${newPosition}px))`;
            document.documentElement.style.setProperty("--range-progress", `calc(${newValue}% + (${newPosition}px))`);
        } catch (err) {}
        };
        try {
        document.addEventListener("DOMContentLoaded", setValue);
    } catch (err) {}
        range.addEventListener('input', setValue);
    } catch (err) {}

    //Zoom Image
    $('.product-img-main')
        // tile mouse actions
        .on('mouseover', function(){
          $(this).children('.product-img-main__image').css({'transform': 'scale('+ $(this).attr('data-scale') +')'});
        })
        .on('mouseout', function(){
          $(this).children('.product-img-main__image').css({'transform': 'scale(1)'});
        })
        .on('mousemove', function(e){
          $(this).children('.product-img-main__image').css({'transform-origin': ((e.pageX - $(this).offset().left) / $(this).width()) * 100 + '% ' + ((e.pageY - $(this).offset().top) / $(this).height()) * 100 +'%'});
        })
        // tiles set up
        .each(function(){
          $(this)
            // add a image container
            .append('<div class="product-img-main__image"></div>')
            // set up a background image for each tile based on data-image attribute
            .children('.product-img-main__image').css({'background-image': 'url('+ $(this).attr('data-image') +')'});
        });

         //Newsletter
        $(window).on('load',function(){
            var delayMs = 1500; // delay in milliseconds
            
            setTimeout(function(){
                $('#newsletter-modal').modal('show');
            }, delayMs);
        });

	// Subscribe form JS
    $(".newsletter-form").validator().on("submit", function (event) {
        if (event.isDefaultPrevented()) {
        // handle the invalid form...
            formErrorSub();
            submitMSGSub(false, "Please enter your email correctly.");
        } else {
            // everything looks good!
            event.preventDefault();
        }
    });
    function callbackFunction (resp) {
        if (resp.result === "success") {
            formSuccessSub();
        }
        else {
            formErrorSub();
        }
    }
    function formSuccessSub(){
        $(".newsletter-form")[0].reset();
        submitMSGSub(true, "Thank you for subscribing!");
        setTimeout(function() {
            $("#validator-newsletter, #validator-newsletter-2").addClass('hide');
        }, 4000)
    }
    function formErrorSub(){
        $(".newsletter-form").addClass("animated shake");
        setTimeout(function() {
            $(".newsletter-form").removeClass("animated shake");
        }, 1000)
    }
    function submitMSGSub(valid, msg){
        if(valid){
            var msgClasses = "validation-success";
        } else {
            var msgClasses = "validation-danger";
        }
        $("#validator-newsletter, #validator-newsletter-2").removeClass().addClass(msgClasses).text(msg);
    }

    // FAQ Accordion
	$('.accordion').find('.accordion-title').on('click', function(){
		$(this).toggleClass('active');
		$(this).next().slideToggle('fast');
		$('.accordion-content').not($(this).next()).slideUp('fast');
		$('.accordion-title').not($(this)).removeClass('active');		
	});

    // Input Plus & Minus Number JS
    $('.input-counter').each(function() {
        var spinner = jQuery(this),
        input = spinner.find('input[type="text"]'),
        btnUp = spinner.find('.plus-btn'),
        btnDown = spinner.find('.minus-btn'),
        min = input.attr('min'),
        max = input.attr('max');
        
        btnUp.on('click', function() {
            var oldValue = parseFloat(input.val());
            if (oldValue >= max) {
                var newVal = oldValue;
            } else {
                var newVal = oldValue + 1;
            }
            spinner.find("input").val(newVal);
            spinner.find("input").trigger("change");
        });
        btnDown.on('click', function() {
            var oldValue = parseFloat(input.val());
            if (oldValue <= min) {
                var newVal = oldValue;
            } else {
                var newVal = oldValue - 1;
            }
            spinner.find("input").val(newVal);
            spinner.find("input").trigger("change");
        });
    });

    // tooltip
	$(function () {
		$('[data-bs-toggle="tooltip"]').tooltip()
	});
    
	// Count Time 
	function makeTimer() {
		var endTime = new Date("September 20, 2025 17:00:00 PDT");
		var endTime = (Date.parse(endTime)) / 1000;
		var now = new Date();
		var now = (Date.parse(now) / 1000);
		var timeLeft = endTime - now;
		var days = Math.floor(timeLeft / 86400); 
		var hours = Math.floor((timeLeft - (days * 86400)) / 3600);
		var minutes = Math.floor((timeLeft - (days * 86400) - (hours * 3600 )) / 60);
		var seconds = Math.floor((timeLeft - (days * 86400) - (hours * 3600) - (minutes * 60)));
		if (hours < "10") { hours = "0" + hours; }
		if (minutes < "10") { minutes = "0" + minutes; }
		if (seconds < "10") { seconds = "0" + seconds; }
		$("#days").html(days + "<span>Days</span>");
		$("#hours").html(hours + "<span>Hours</span>");
		$("#minutes").html(minutes + "<span>Minutes</span>");
		$("#seconds").html(seconds + "<span>Seconds</span>");
	}
	setInterval(function() { makeTimer(); }, 0);

    // AJAX MailChimp JS
    $(".newsletter-form").ajaxChimp({
        url: "https://hibootstrap.us20.list-manage.com/subscribe/post?u=60e1ffe2e8a68ce1204cd39a5&amp;id=42d6d188d9", // Your url MailChimp
        callback: callbackFunction
    });

	// Go to Top
    $(window).on('scroll', function(){
        var scrolled = $(window).scrollTop();
        if (scrolled > 300) $('.go-top').addClass('active');
        if (scrolled < 300) $('.go-top').removeClass('active');
        });
    // Click Event
    $('.go-top').on('click', function() {
    $("html, body").animate({ scrollTop: "0" },  500);
    });

})(jQuery);
