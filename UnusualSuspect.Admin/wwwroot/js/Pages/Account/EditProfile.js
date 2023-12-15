
$('#EditProfileForm').bootstrapValidator({
    live: 'enabled',
    feedbackIcons: {
        validating: 'glyphicon glyphicon-refresh'
    },
}).on('success.form.bv', function (e) {
    e.preventDefault();
    var $form = $(e.target);
    var formData = new FormData(this);
    var bv = $form.data('bootstrapValidator');
    jQuery.ajaxSettings.traditional = true;
    $.ajax({
        beforeSend: function () {
            $form.addClass("DisableItems");
        },
        url: $form.attr("action"),
        type: $form.attr("method"),
        data: formData,
        success: function (result) {
            console.log(result);
            if (result.Success) {
                toastr.success(result.MessageList, '', { positionClass: "toast-bottom-center" });
                $form.removeClass("DisableItems");

                $("button[type='submit']").prop('disabled', false);
                //window.location.reload('EditProfileForm');
            }
            else {
                $("button[type='submit']").prop('disabled', false);
                toastr.error(result.MessageList, '', { positionClass: "toast-bottom-center" });
                $form.removeClass("DisableItems");
            }
        },
        cache: false,
        contentType: false,
        processData: false
    });
});
//  });

$(document).on('click', function (e) {
    if ($(e.target).is('.input-edit')) {
        $(e.target).attr('data-mode', 'edit');
        let inp = $(e.target).closest('.input-box').find('input');
        let txta = $(e.target).closest('.input-box').find('textarea');
        inp.attr('readonly', false);
        txta.attr('readonly', false);
        inp.focus();
        txta.focus();
        var tmpStr = inp.val();
        var tmpStrTxt = txta.val();
        inp.val('');
        txta.val('');
        inp.val(tmpStr);
        txta.val(tmpStrTxt);
        $(e.target).closest('.input-box').css('border-color', '#7b61ff');
    } else if ($(e.target).is('input') || $(e.target).is('textarea')) {
        if ($(e.target).closest('.input-box').find('.input-edit').attr('data-mode') == 'edit') {
            let inp = $(e.target).closest('.input-box').find('input');
            let txta = $(e.target).closest('.input-box').find('textarea');
            inp.attr('readonly', false);
            txta.attr('readonly', false);
            inp.focus();
            txta.focus();
            var tmpStr = inp.val();
            var tmpStrTxt = txta.val();
            inp.val('');
            txta.val('');
            inp.val(tmpStr);
            txta.val(tmpStrTxt);
            $(e.target).closest('.input-box').css('border-color', '#7b61ff');
        } else {
            let inp = $(e.target).closest('.input-box').find('input');
            let txta = $(e.target).closest('.input-box').find('textarea');
            txta.attr('readonly', true);
            inp.attr('readonly', true);
            $('.input-box').css('border-color', '#e5e5ea');
            $('.input-edit').attr('data-mode', 'read');
        }
    } else {
        let inp = $(e.target).closest('.input-box').find('input');
        let txta = $(e.target).closest('.input-box').find('textarea');
        txta.attr('readonly', true);
        inp.attr('readonly', true);
        $('.input-box').css('border-color', '#e5e5ea');
        $('.input-edit').attr('data-mode', 'read');
    }
});
