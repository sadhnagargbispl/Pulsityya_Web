 
$('#enq-form').validate({
	rules: {   
		Name: "required", 
		MobileNo: { 
			required: true,
			minlength: 10, 
		},
		EmailID: {
			required: true,	
			email: true
		} 		 
	}, 
	messages: {
		Name: "Please enter your Name.", 
		MobileNo: { 
			required: "Please enter your Mobile No.",
			minlength: "Mobile no. must be 10 digits long." 
		},
		EmailID: { 
			required: "Please enter your Email ID.",
			email: "Please enter a valid Email ID."
		} 	
	},
	submitHandler: function() {     
		$('#enq-form').ajaxSubmit({
			beforeSend: function() { 
				$('body').append(loader);
			},  
			complete: function(xhr) { 				
				var msg;
				var response = $.parseJSON(xhr.responseText);
				 
				$('.loader').remove();
				if(response.success == true){ 
					msg += '<div class="msg-overlay"><div class="msg success wow zoomIn animated"><img src="img/icon-success.gif">';  
					msg += '<span>'+response.message+'</span>';
					msg += '<a onclick="return DelMsg(this)" ID="DelMsg">Ok</a></div></div>';   
				}else if(response.success == false){
					msg += '<div class="msg-overlay"><div class="msg error wow zoomIn animated"><img src="img/icon-error.gif">';  
					msg += '<span>'+response.message+'</span>';
					msg += '<a onclick="return DelMsg(this)" ID="DelMsg">Ok</a></div></div>';
				}
				
				$('body').append(msg); 
				setTimeout(function(){DelMsg('#DelMsg');}, 4000);  	 	
				$('#enq-form')[0].reset();
			}	
		});
	}
}); 