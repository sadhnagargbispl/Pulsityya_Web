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
    var WISHLIST_IDS_URL = '/ProductDetail/WishlistIds';

    // every card of a product - the same product can sit in several sections,
    // and the carousel clones cards
    function hearts(prodid) {
        return $('.item-container[data-prodid="' + prodid + '"] .item_wshlst');
    }

    // navbar heart badge - _Layout renders it only for a logged-in member
    function setWishlistCount(count) {
        if (count >= 0) {
            $('#wishlistCount').text(count).prop('hidden', false);
        }
    }

    // on page load, show the navbar count and fill the hearts of products
    // already in the member's wishlist
    $(function () {
        if (!$('#wishlistCount').length) { return; }
        $.ajax({
            url: WISHLIST_IDS_URL,
            type: 'GET',
            dataType: 'json',
            cache: false
        }).done(function (ids) {
            ids = ids || [];
            $.each(ids, function (i, id) {
                hearts(id).addClass('wshlsted');
            });
            setWishlistCount(ids.length);
        });
    });

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
                    hearts(prodid).addClass('wshlsted');
                } else if (data.status === '2') {
                    hearts(prodid).removeClass('wshlsted');
                }
                setWishlistCount(data.count);
                // let the heart and the navbar count repaint before the alert blocks the page
                setTimeout(function () { alert(data.msg); }, 50);
            }).fail(function (error) {
                alert(error.statusText);
            });
        }).fail(function (error) {
            alert(error.statusText);
        });

        return false;
    });

})(jQuery);
