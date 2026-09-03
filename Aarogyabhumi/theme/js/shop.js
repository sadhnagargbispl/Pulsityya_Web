/*********************************************************************************
Pulastya theme - product card behaviour.

The cards mirror the demo exactly, so the "Add to cart" row is hidden by
layout.css and adding to the cart happens on the product detail page. The only
interactive control on a card is the wishlist heart, which the demo shows too.

Every card carries its id on the .item-container element: data-prodid
***************/
(function ($) {

    var LOGIN_URL = '/Account/Login';
    var CHECK_LOGIN_URL = '/ProductDetail/CheckLogin';
    var WISHLIST_URL = '/ProductDetail/SaveToWishlist';

    $(document).on('click', '.item_wshlst', function (e) {
        e.preventDefault();

        var $btn = $(this);
        var prodid = $btn.closest('.item-container').data('prodid');

        // the product detail page reuses .item-container without card data
        if (typeof prodid === 'undefined') { return; }

        $.ajax({
            url: CHECK_LOGIN_URL,
            type: 'GET'
        }).done(function (result) {
            if (result !== 'Yes') {
                window.location.href = LOGIN_URL;
                return;
            }
            $.ajax({
                type: 'POST',
                url: WISHLIST_URL,
                dataType: 'json',
                data: { ProductID: prodid }
            }).done(function (data) {
                if (data.status === '1') {
                    $btn.addClass('wshlsted');
                } else if (data.status === '2') {
                    $btn.removeClass('wshlsted');
                }
                alert(data.msg);
            }).fail(function (error) {
                alert(error.statusText);
            });
        }).fail(function (error) {
            alert(error.statusText);
        });

        return false;
    });

})(jQuery);
