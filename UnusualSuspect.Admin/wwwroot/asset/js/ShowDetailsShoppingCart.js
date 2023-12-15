
$('.addressBox .select-item').on('click', function (e) { 
    $(this).closest('.select-box').find('.select-box-btn span').text($(this).text());
    // $('.select-box-btn span').text($(this).text());

    $('.selectedAddress').text($('.address-sec-step-two .select-box-btn span').text());
    $(this).closest('.address-sec .select-box').find('.select-box-btn span').addClass('e-font');
    $(this).closest('.compare-box-item').find('.details').show();
    $(this).closest('.compare-box-item').find('.addToCompareImg').hide(); 
    $('#CustomerAddressId').val($(this).data("id"));
    //console.log($('.select-box-btn span').text());
    //console.log($(e.target).text());
   
});


$(function () {
    $.ajaxSetup({ cache: false });
    $("button[data-modal2], a[data-modal2]").on("click", function (e) {

        $('#editAdressContent').load($(this).data("url"), function () {

            $('#editAddress').modal({
                keyboard: true
            }, 'show');

        });

        return false;
    });
    
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

//$('document').on('click', '.input-edit2', function (e) {
//    console.log('aaaa');
//    let inp = $(this).closest('.input-box').find('input:not(.searchAddress-input)');
//    let txta = $(this).closest('.input-box').find('textarea');
//    console.log(inp, inp.prop('readonly'))
//    inp.prop('readonly', false);
//    txta.prop('readonly', false);
//    inp.prop('readOnly', false);
//    txta.prop('readOnly', false);
//    inp.focus();
//    txta.focus();
//    var tmpStr = inp.val();
//    var tmpStrTxt = txta.val();
//    inp.val('');
//    txta.val('');
//    inp.val(tmpStr);
//    txta.val(tmpStrTxt);
//    $(e.target).closest('.input-box').css('border-color', '#7b61ff');
//});


$(function () {

    $.ajaxSetup({ cache: false });
    $("button[data-modal2], a[data-modal2]").on("click", function (e) {

        $('#editAdressContent').load($(this).data("url"), function () {

            $('#editAddress').modal({
                keyboard: true
            }, 'show');

        });

        return false;
    });
});


$(document).ready(function () {
    var dd = $('.delivery-day');

    dd.each(i => {
        var pDate = new persianDate().add('days', i + 2);
        var wDay = pDate.format('dddd');
        var mYear = pDate.format('MMMM');
        var dMounth = pDate.date();
        dd.ready(function () {
            $('strong:contains("جمعه")').closest('.delivery-day').addClass('disable');
        })
        $('strong:contains("جمعه")').closest('label').addClass('disable');

        $($(dd)[i]).find('strong').text(wDay);
        $($(dd)[i]).find('time').text(dMounth + ' ' + mYear);
    });
});

$('.delivery-day input').on('change', function () {
    $('.selectedDate').text(' | ' + $(this).closest('label').find('strong').text());

    $('#deliveryDay').val($(this).closest('label').find('strong').text());

});
$('.delivery-time input').on('change', function () {
    $('.selectedTime').text(' | ' + $(this).closest('label').text())

});


$("button[data-modal2], a[data-modal2]").on("click", function (e) {
    var that = $(this);
    $('#editAdressContent').load($(this).data("url"), function () {
        $('#editAddress').attr('data-source', that.attr('id'));
        $('#editAddress').modal({
            keyboard: true
        }, 'show');

    });

    return false;
});

