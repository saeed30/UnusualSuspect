

// .category-swiper slider      ///////////////////

var categorySwiper = new Swiper(".category-swiper", {
    slidesPerView: 6,
    spaceBetween: 30,
    breakpoints: {
        320: {
            slidesPerView: 2.5,
            spaceBetween: 20,
        },
        420: {
            slidesPerView: 3,
            spaceBetween: 10,
        },
        520: {
            slidesPerView: 4,
            spaceBetween: 10,
        },
        650: {
            slidesPerView: 4.8,
            spaceBetween: 10,
        },
        768: {
            slidesPerView: 5,
            spaceBetween: 40,
        },
        850: {
            slidesPerView: 5.5,
            spaceBetween: 50,
        },
        850: {
            slidesPerView: 6,
            spaceBetween: 50,
        },
        1200: {
            slidesPerView: 6,
            spaceBetween: 50,
        },
    },
});
// ////////////////////////////////////////////



// .phone-model slider    //////////////////////
var phoneModel = new Swiper(".phone-model", {
    slidesPerView: 6,
    spaceBetween: 30,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        320: {
            slidesPerView: 2.5,
            spaceBetween: 20,
        },
        420: {
            slidesPerView: 3,
            spaceBetween: 10,
        },
        520: {
            slidesPerView: 4,
            spaceBetween: 10,
        },
        650: {
            slidesPerView: 4.8,
            spaceBetween: 10,
        },
        768: {
            slidesPerView: 5,
            spaceBetween: 40,
        },
        850: {
            slidesPerView: 5.5,
            spaceBetween: 50,
        },
        850: {
            slidesPerView: 6,
            spaceBetween: 50,
        },
        1200: {
            slidesPerView: 6,
            spaceBetween: 50,
        },
    },
});
// ////////////////////////////////////////////



// .specialSuggestion              /////////////////////////
var specialSuggestion = new Swiper(".specialSuggestion", {
    slidesPerView: 4.5,
    spaceBetween: 20,
    breakpoints: {
        320: {
            slidesPerView: 1.3,
        },
        420: {
            slidesPerView: 1.7,
        },
        520: {
            slidesPerView: 2,
        },
        680: {
            slidesPerView: 2.5,
        },
        768: {
            slidesPerView: 2.8,
        },
        850: {
            slidesPerView: 3.2,
        },
        992: {
            slidesPerView: 3.6,
        },
        1100: {
            slidesPerView: 3.9,
        },
        1200: {
            slidesPerView: 4.5,
        },
        1300: {
            slidesPerView: 4.6,
        },
    },
});
// ////////////////////////////////////////////






// .product-card slider            /////////////////////////////
var productCard = new Swiper(".product-card", {
    slidesPerView: 4.5,
    spaceBetween: 20,
    breakpoints: {
        320: {
            slidesPerView: 1.1,
        },
        350: {
            slidesPerView: 1.3,
        },
        420: {
            slidesPerView: 1.5,
        },
        520: {
            slidesPerView: 1.7,
        },
        680: {
            slidesPerView: 2,
        },
        768: {
            slidesPerView: 2.3,
        },
        850: {
            slidesPerView: 2.5,
        },
        992: {
            slidesPerView: 2.7,
        },
        1100: {
            slidesPerView: 3,
        },
        1200: {
            slidesPerView: 3.3,
        },
        // 1300: {
        //     slidesPerView: 4,
        //     spaceBetween: 10,
        // },
    },
});
// ////////////////////////////////////////////



// .whyDolatMart             //////////////////////////////
var whyDolatMart = new Swiper(".whyDolatMart", {
    slidesPerView: 3.8,
    spaceBetween: 10,
    breakpoints: {
        320: {
            slidesPerView: 1.2,
            spaceBetween: 10,
        },
        420: {
            slidesPerView: 1.5,
            spaceBetween: 10,
        },
        520: {
            slidesPerView: 1.8,
            spaceBetween: 10,
        },
        680: {
            slidesPerView: 2.3,
            spaceBetween: 10,
        },
        768: {
            slidesPerView: 2.9,
            spaceBetween: 10,
        },
        850: {
            slidesPerView: 3.3,
            spaceBetween: 10,
        },
        992: {
            slidesPerView: 3.8,
            spaceBetween: 10,
        },
        1100: {
            slidesPerView: 4,
            spaceBetween: 10,
        },

    },
});
// /////////////////////////////////////////////////



// navbar-toggler             /////////////////////////////
$('.navbar-toggler').on('click', function () {
    $('.navbar-collapse').slideToggle("slow");
    $('.navbar-toggler span:first-child').toggleClass('active', 1000);
    $('.navbar-toggler span:last-child').toggleClass('d-none');
    $('header nav').toggleClass('bg-black');
    $('header .nav-item .card-body').toggleClass('bg-black');
});
// //////////////////////////////////


// more          ///////////////////////////
$(document).ready(function () {
    $('[data-bs-toggle="popover"]').popover();
});
$('.more').on('click', function () {
    $('.des-text').toggleClass('show');
    if ($('.more').text() == ('مشاهده بیشتر')) {
        $(this).text('مشاهده کمتر');
    } else {
        $(this).text('مشاهده بیشتر');
    }
});
// ////////////////////////////////////


//  custome selectbox and dropdown           /////////////
$(document).on('click', function (e) {
    if ($(e.target).hasClass('select-box-btn') || $(e.target).is('.select-box-btn span') || $(e.target).is('.select-box')) {
        let selectBoxUl = $(e.target).closest('.select-box').find('.select-box-ul');
        if (!$(selectBoxUl).hasClass('show')) {
            $('.select-box-ul').removeClass('show');
            $(selectBoxUl).addClass('show');
            console.log($(e.target));

        } else {
            $(selectBoxUl).removeClass('show');
        }
    } else if ($(e.target).is('.searchAddress input')) {
        $(e.target).prop('readonly', false);
    } else if ($(e.target).is('.plus') || $(e.target).is('.delete-item') || $(e.target).is('.minus')) {
        console.log('ok');
    } else {
        $('.select-box-ul').removeClass('show');
        // console.log($(e.target));
    }
});

//$('.addressBox .select-item').on('click', function () {
    
//    $(this).closest('.select-box').find('.select-box-btn span').text($(this).text());
//    // $('.select-box-btn span').text($(this).text());
   
//    $('.selectedAddress').text($('.select-box-btn span').text());
//    $(this).closest('.address-sec .select-box').find('.select-box-btn span').addClass('e-font');
//    $(this).closest('.compare-box-item').find('.details').show();
//    $(this).closest('.compare-box-item').find('.addToCompareImg').hide();
//    //$('#CustomerAddressId').val($(this).data("id"));
//});
 
$('.selectedAddress').text($('.select-box-btn span').text());

$('').on('click', function (e) {
    $('.select-box-ul').addClass('show');
    console.log(e.target);
})
// ////////////////////////////////////////////



// timer               //////////////////////////
let timer = function () {
    var timer2 = "2:01";
    var interval = setInterval(function () {
        var timer = timer2.split(':');
        //by parsing integer, I avoid all extra string processing
        var minutes = parseInt(timer[0], 10);
        var seconds = parseInt(timer[1], 10);
        --seconds;
        minutes = (seconds < 0) ? --minutes : minutes;
        if (minutes < 0) {
            clearInterval(interval);
            $('.countdown').hide();
            $('.reSend').show();
        };
        seconds = (seconds < 0) ? 59 : seconds;
        seconds = (seconds < 10) ? '0' + seconds : seconds;
        //minutes = (minutes < 10) ?  minutes : minutes;
        $('.countdown').html(minutes + ':' + seconds);
        timer2 = minutes + ':' + seconds;
    }, 1000);
}
// ////////////////////////////////////////////


// codebox                    //////////////////////////
let codeBox = function () {
    const codes = document.querySelectorAll(".code");
    codes[0].focus();

    if (codes.length !== 0) {
        codes[0].focus();
        codes.forEach((code, idx) => {
            code.addEventListener("keydown", (e) => {
                if (e.key >= 0 && e.key <= 9) {
                    codes[idx].value = "";
                    setTimeout(function () {
                        if (codes[idx + 1] !== undefined) {
                            codes[idx + 1].focus();
                        }
                    }, 10);
                } else if (e.key === "Backspace") {
                    setTimeout(function () {
                        if (codes[idx - 1] !== undefined) {
                            codes[idx - 1].focus();
                        }
                    }, 10);
                }
            });
        });
    }
}
// //////////////////////////////////////////


// login with code
let mobInputAction = function () {
    $(".code-box").addClass("on");
    $("#mobInput").hide();
    //$("#mobBtn").hide();
    $('.sign-label').text('کد تایید را وارد کنید');
    // $(".sign > div").css("min-height", "420px");

    var phone = $('#mobInput').val();
    // $('.code-box p span').text(phone[0] + phone.slice(1, 5) + "*".repeat(4) + phone.slice(-2));
    $('.code-box p span').text(phone);
    codeBox();
    timer();
}


$("document").ready(function () {
    $("#mobBtn").click(function () {
        mobInputAction();
    });
    $("#mobInput").keypress(function (e) {
        if (e.which == 13) {
            //Enter key pressed
            mobInputAction();
        }
    });
    $(".reSend").on("click", function () {
        timer();
        $(".countdown").show();
        $(this).hide();
    });
    $(".code").on("keyup", function () {
        $(".code").each(function () {
            if ($(this).val() != "") {
                $(".confirmCode").addClass('active');
            } else {
                $(".confirmCode").removeClass('active');
            }
        });
    });
    $('.changeNumber').on('click', function () {
        var minp = $('#mobInput');
        $('.code-box').removeClass('on');
        $('.code').val('');
        // minp.removeClass("deActive");
        // $(".sign > div").css("min-height", "200px");
        minp.show();
        $('#mobBtn').show();
        var tmpStr = minp.val();
        minp.val('');
        minp.val(tmpStr);
        minp.focus();
        $('.sign-label').text('شماره همراه خود را وارد کنید');
    });
});

// ///////////////////////////////




// input[type='text'] get number
function setInputFilter(textbox, inputFilter, errMsg) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop", "focusout"].forEach(function (event) {
        if (textbox) {
            textbox.addEventListener(event, function (e) {
                if (inputFilter(this.value)) {
                    // Accepted value
                    if (["keydown", "mousedown", "focusout"].indexOf(e.type) >= 0) {
                        this.classList.remove("input-error");
                        this.setCustomValidity("");
                    }
                    this.oldValue = this.value;
                    this.oldSelectionStart = this.selectionStart;
                    this.oldSelectionEnd = this.selectionEnd;
                } else if (this.hasOwnProperty("oldValue")) {
                    // Rejected value - restore the previous one
                    this.classList.add("input-error");
                    this.setCustomValidity(errMsg);
                    this.reportValidity();
                    this.value = this.oldValue;
                    this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                } else {
                    // Rejected value - nothing to restore
                    this.value = "";
                }
            });
        }
    });
}
// //////////////////////////////////////


// Install input filters.
setInputFilter(document.getElementById("mobInput"), function (value) {
    return /^-?\d*$/.test(value);
}, "Must be an integer");



// plus and minus            /////////////////////////
$('.plus').on('click', function (e) {
    let itemCount = parseInt($(e.target).closest('.item-count').find('.item-count__number').text());
    itemCount += 1;
    $(e.target).closest('.item-count').find('.item-count__number').text(itemCount);
    $(e.target).closest('.item-count').find('.minus').removeClass('icon-feather-trash-2').addClass('icon-feather-minus');
});
$('.minus').on('click', function (e) {
    let itemCount = parseInt($(e.target).closest('.item-count').find('.item-count__number').text());
    if (itemCount > 1) {
        itemCount -= 1;
        if (itemCount == 1) {
            $(e.target).closest('.item-count').find('.minus').removeClass('icon-feather-minus').addClass('icon-feather-trash-2');
        }
    }
    $(e.target).closest('.item-count').find('.item-count__number').text(itemCount);
});

$('.basket-item-count').text($('.basket-list__item').length);


// star rating             /////////////////////
var starClicked = false;

$(function () {

    $('.star').click(function () {

        $(this).children('.selected').addClass('is-animated');
        $(this).children('.selected').addClass('pulse');

        var target = this;

        setTimeout(function () {
            $(target).children('.selected').removeClass('is-animated');
            $(target).children('.selected').removeClass('pulse');
        }, 1000);

        starClicked = true;
    })

    $('.half').click(function () {
        if (starClicked == true) {
            setHalfStarState(this)
        }
        $(this).closest('.rating').find('.js-score').text($(this).data('value'));
        $(this).closest('.rating').find('.js-score').text($(this).data('value'));
        $(this).closest('.rating').data('vote', $(this).data('value'));
        console.log(parseInt($(this).data('value')));

    })

    $('.full').click(function () {
        if (starClicked == true) {
            setFullStarState(this)
        }
        $(this).closest('.rating').find('.js-score').text($(this).data('value'));

        $(this).closest('.rating').find('.js-score').text($(this).data('value'));

        $(this).closest('.rating').data('vote', $(this).data('value'));

        console.log(parseInt($(this).data('value')));
    })

    $('.half').hover(function () {
        if (starClicked == false) {
            setHalfStarState(this)
        }

    })

    $('.full').hover(function () {
        if (starClicked == false) {
            setFullStarState(this)
        }
    })

})

function updateStarState(target) {
    $(target).parent().prevAll().addClass('animate');
    $(target).parent().prevAll().children().addClass('star-colour');

    $(target).parent().nextAll().removeClass('animate');
    $(target).parent().nextAll().children().removeClass('star-colour');
}

function setHalfStarState(target) {
    $(target).addClass('star-colour');
    $(target).siblings('.full').removeClass('star-colour');
    updateStarState(target)
}

function setFullStarState(target) {
    $(target).addClass('star-colour');
    $(target).parent().addClass('animate');
    $(target).siblings('.half').addClass('star-colour');

    updateStarState(target)
}

// ///////////////////////////////////////




// search overlay          ///////////////////
$(document).on('click', function (e) {
    if ($(e.target).is('.search-icon')) {
        $('.search-icon').css('right', '-50px');
        $('.search-overlay').addClass('on');
        $('.navbar').addClass('m-fadeOut');
        $('.search-overlay-item').addClass('on');
        $('.search-overlay-item input').focus();
        $('.most-search').show('slow');
    } else if ($(e.target).is('.close-overlay')) {
        if ($('.search-overlay-item input').val()) {
            $('.search-overlay-item input').val('');
        } else {
            $('.search-icon').css('right', '0');
            $('.search-overlay').removeClass('on');
            $('.navbar').removeClass('m-fadeOut');
            $('.search-overlay-item').removeClass('on');
            $('.most-search').hide();
        }
    }
});
// /////////////////////////////////


// set image with color           //////////////////////
function setColoredImage(that) {
    var selectedColor = $(that).attr('data-color');
    $(that).closest('.product-img').find('.bigImage img').each(function () {
        if ($(this).attr('data-color') == selectedColor) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
}
(function () {
    let checkLabelInput = $('.check-label input');
    let imgCheckInput = $('.bigImage img');
    $('.check-label input:checked').each(function () {
        setColoredImage($(this))
    })
    checkLabelInput.on('change', function () {
        setColoredImage($(this))
    })
})();

// //////////////////////////////////////////////


// filter more
$('.filter-box .btn-link').on('click', function () {
    $(this).closest('.filter-box').find('.more-option').toggleClass('d-grid');
    $(this).closest('.filter-box').toggleClass('on');
    if ($(this).closest('.filter-box').hasClass('on')) {
        $('.filter-box .btn-link').text('گزینه های کمتر');
    } else {
        $('.filter-box .btn-link').text('گزینه های بیشتر');
    }
});

$('.delete-filter').on('click', function () {
    $('.filter-box .filter-check input').prop('checked', false);
});

$(document).ready(function () {
    $('[data-bs-toggle="tooltip"]').tooltip();
});











