

var swiper = new Swiper(".tumbImg", {
    spaceBetween: 30,
    slidesPerView: 5,
    freeMode: true,
    watchSlidesProgress: true,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
});
var swiper2 = new Swiper(".bigImg", {
    spaceBetween: 10,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    thumbs: {
        swiper: swiper,
    },
});

$('.more-btn').on('click', function () {
    $(this).closest('.property').find('.property-describe').css('display', 'block');
    $(this).hide();
});

$(document).on('click', function (e) {
    if ($(e.target).is('.basket-icon *') || $(e.target).is('.basket-icon')) {
        if ($('.basket-icon-list').hasClass('on')) {
            if (!$(e.target).is('.basket-icon-list') && !$(e.target).is('.basket-icon-list *') && !$(e.target).is('button')) {
                $('.basket-icon-list').removeClass('on');
            }
        } else {
            $('.basket-icon-list').addClass('on');
        }
    } else {
        $('.basket-icon-list').removeClass('on');
    }
});


var myregex = /([\u0000-\u007F]*)\b|([0-9]*×?[0-9]*)/g;
var text = $("#sentences");


text.html(function (i, oldHTML) {
    function checkKeyboard(ob, e) {
        re = /\d|\w|[\.\$@\*\\\/\+\-\^\!\(\)\[\]\~\%\&\=\?\>\<\{\}\"\'\,\:\;\_]/g;
        a = e.key.match(re);
        if (a == null) {
            console.log('aaa');
        }
        return oldHTML.replace(myregex, '<span>$1</span>');

    }
});

    $(document).on("click", ".btn-addtocart", function () {
        
            var url = $(this).data("remote");
    var basket = $(this);

    $.post(url, null, function (Resultdata) {
                
                if (Resultdata.Success) {
                      $.get("/ShoppingCart/CartCountItem", null, function (data) { 
                          $(".total-count").text(data);
                        
                        });

                      $.get("/ShoppingCart/ShowItemsCart", null, function (content) {
                            $(".basket-icon-list").html(content);
                      }); 
                    toastr.success(Resultdata.MessageList, '', { positionClass: "toast-top-center" });

                  }
                else {
                    swal("", Resultdata.MessageList, "error"); 
                        }
        });
    });

