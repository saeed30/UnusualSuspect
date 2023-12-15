 


///////
 
$(document).ready(function () {
    $.get("/ShoppingCart/CartCountItem", {
    }, function (data) {
        //$(".cart-button").removeClass("hide");
        $(".total-count").text(data);
    });
    $.get("/ShoppingCart/ShowItemsCart", {

    }, function (content) {

        $(".basket-icon-list").html(content);
    });
});
$(document).on("click", ".show-cart .delete-item", function () {
    //var $ParentDiv = $(this).closest("li");
    //$ParentDiv.addClass("DisableItems");
    var cartItemId = $(this).attr("data-id");
    $.get("/ShoppingCart/RemoveOfCart", {
        Id: cartItemId
    }, function (result) {
        if (result) {
            $.get("/ShoppingCart/CartCountItem", null, function (data) {
                $(".total-count").text(data);
            });

            $.get("/ShoppingCart/ShowItemsCart", null, function (content) {
                $(".basket-icon-list").html(content);
            });
            toastr.Success("محصول انتخاب شده از سبد خرید حذف گردید", '', { positionClass: "toast-bottom-center" });
        }
        else {
            toastr.error("در حذف سبد خرید خطایی رخ داده است ", '', { positionClass: "toast-bottom-center" });
        }
    });
});

$(document).on("click", ".clear-cart", function () {
    //var $ParentDiv = $(this).closest("li");
    //$ParentDiv.addClass("DisableItems"); 
    $.get("/ShoppingCart/RemoveAllCart", null, function (result) {
        if (result) {
            $.get("/ShoppingCart/CartCountItem", null, function (data) {
                $(".total-count").text(data);
            });

            $.get("/ShoppingCart/ShowItemsCart", null, function (content) {
                $(".basket-icon-list").html(content);
            });
            $.get("/ShoppingCart/ShowItemsCartInBasket", null, function (content) {
                $(".basket-page").html(content);
            }); 
            toastr.Success("محصولات از سبد خرید حذف گردیدند", '', { positionClass: "toast-bottom-center" });
        }
        else {
            toastr.error("در حذف سبد خرید خطایی رخ داده است ", '', { positionClass: "toast-bottom-center" });
        }
    });
});
$(document).on("click", ".cart .removefromcart", function () {
    var $ParentDiv = $(this).closest("li");
    $ParentDiv.addClass("DisableItems");
    var cartItemId = $(this).attr("data-id");
    $.get("/ShoppingCart/RemoveOfCart", {
        Id: cartItemId
    }, function (result) {
        if (result) {

            $.get("/ShoppingCart/CartCountItem", {
            }, function (data) {
                $(".cart-button .badge").text(data);
            });
            $.get("/ShoppingCart/ShowItemsCart", {
            }, function (data) {
                $(".cart .cart-items").html(data).addClass("open");
                $(".cart").addClass("open");
            });
            toastr.Success("محصول انتخاب شده از سبد خرید حذف گردید", '', { positionClass: "toast-bottom-center" });
        }
        else {
            toastr.error("در حذف سبد خرید خطایی رخ داده است ", '', { positionClass: "toast-bottom-center" });
        }
    });
});

function ShowShoppingCart() {
    $.get("/ShoppingCart/CartCountItem", {
    }, function (data) {
        if (data !== 0) {
            animationWOW('cart-button');
            $(".cart-button").removeClass("hide");
            $(".cart-button .badge").removeClass("hide").text(data);
        }
    });
}
$(document).on("click", ".cart-button", function () {
    if ($(".cart").hasClass("open") === false) {
        $.get("/ShoppingCart/ShowItemsCart", {
        }, function (data) {
            $(".cart .cart-items").html(data).addClass("open");
            $(".cart").addClass("open");
        });
    }
    else {
        $(".cart").removeClass("open");
    }
});

$(document).on('click', '.basket-cart .plus', function (e) {
    var input = $(this).next();
    e.stopPropagation();
    input.val(parseInt(input.val()) + 1);
    ChangePQuantityOfCartItems($(this), input);
});
$(document).on('click', '.basket-cart .minus', function (e) {

    e.stopPropagation();
    var input = $(this).prev();
    input.val(parseInt(input.val()) - 1);
    if (input.val() < 1) {
        input.val(1);
    }
    ChangePQuantityOfCartItems($(this), input);
});

function ChangePQuantityOfCartItems($this, $input,) {
    var $ParentDiv = $this.closest("li");
    $ParentDiv.addClass("DisableItems");
    var cartItemId = $this.attr("data-id");
    $.get("/ShoppingCart/ChangePQuantityOfCartItems", {
        Id: cartItemId,
        NewQuantity: $input.val()
    }, function (result) {
        if (result.Success) {
            $.get("/ShoppingCart/CartCountItem", null, function (data) {
                $(".total-count").text(data);
            });

            $.get("/ShoppingCart/ShowItemsCart", null, function (content) {
                $(".basket-icon-list").html(content);
            });
        }
        else {
            toastr.error(result.MessageList, '', { positionClass: "toast-bottom-center" });
            $ParentDiv.removeClass("DisableItems");
            $input.val(parseInt($input.val()) - 1);
        }
    });
}
$(document).on('click', '.basket-list__item .plus', function (e) {
     
    var input = $(this).next();
    e.stopPropagation();
    input.val(parseInt(input.val()) + 1);
    ChangePQuantityOfCartItems2($(this), input);
});
$(document).on('click', '.basket-list__item .minus', function (e) {
    e.stopPropagation();
    var input = $(this).prev();
    input.val(parseInt(input.val()) - 1);
    if (input.val() < 1) {
        input.val(1);
    }
    ChangePQuantityOfCartItems2($(this), input);
});

function ChangePQuantityOfCartItems2($this, $input) {
    var $ParentDiv = $this.closest("li");
    $ParentDiv.addClass("DisableItems");
    var cartItemId = $this.attr("data-id");
    
    $.get("/ShoppingCart/ChangePQuantityOfCartItems", {
        Id: cartItemId,
        NewQuantity: $input.val()
    }, function (result) {
        if (result.Success) {
        
            $.get("/ShoppingCart/ShowItemsCartInBasket", null, function (content) {
                $(".basket-page").html(content);
            });
        }
        else {
            toastr.error(result.MessageList, '', { positionClass: "toast-bottom-center" });
            $ParentDiv.removeClass("DisableItems");
            $input.val(parseInt($input.val()) - 1);
        }
    });
}
