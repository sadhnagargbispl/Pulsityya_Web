 
$('#price-range').slider({
	range: true,
	min: lowest_price,
	max: highest_price,
	values: [ lowest_price, highest_price ],
	slide: function( event, ui ) {
		$('.ui-slider-handle:eq(0) .value-min').html('₹ ' + ui.values[0]);
		$('.ui-slider-handle:eq(1) .value-max').html('₹ ' + ui.values[1]); 
		 
		if (ui.values[0] === ui.values[1]) {
			$('.ui-slider-handle .value').css('display', 'none');
		} else {
			$('.ui-slider-handle .value').css('display', 'inline');
		}
		
		$('.itm-lst .col-item').hide().filter(function() {
			var price = parseInt($(this).find('.range_price_item').val(), 10);
			return price >= ui.values[0] && price <= ui.values[1];
		}).show();  
	}
});

$('.ui-slider-handle:eq(0)').append('<span class="value-min value">₹ ' + $('#price-range').slider('values', 0) + '</span>');
$('.ui-slider-handle:eq(1)').append('<span class="value-max value">₹ ' + $('#price-range').slider('values', 1) + '</span>');

