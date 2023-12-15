$('.show-pass').on('click', function () {
    if ($(this).closest('.input-box').find('.pass-input').attr('type') == 'password') {
        $(this).closest('.input-box').find('.pass-input').attr('type', 'text');
        $(this).removeClass('icon-feather-eye').addClass('icon-feather-eye-off');
    } else {
        $(this).closest('.input-box').find('.pass-input').attr('type', 'password');
        $(this).removeClass('icon-feather-eye-off').addClass('icon-feather-eye');
    }
});

$(document).ready(function () {
    $("#mobInput").focus();

    //$('#mobBtn').on('click', function () {
    //    alert($("#mobInput").val());
    //    $.post("/Account/SendCodeToPhoneNumber", { PhoneNumber: $("#mobInput").val(), ReturnUrl: $("#ReturnUrl").val() }, function (result) {
    //        if (result.Success) {
    //            toastr.success(result.MessageList, '', { positionClass: "toast-bottom-center" });
    //            $.get("/Account/NewConfirmLoginCode", {
    //                PhonNumber: result.Params2,
    //                retUrl: result.Params1,
    //                SendDate: result.Params3
    //            }, function (result2) {
    //                $(".k-code-box").html(result2);
    //                $form.removeClass("DisableItems");
    //            });
    //        }
    //        else {
    //            toastr.error(result.MessageList, '', { positionClass: "toast-bottom-center" });
    //            $form.removeClass("DisableItems");
    //            $form.find("button[type='submit']").prop("disabled", false);
    //        }
    //    });
    //});


    $('#LoginUser').bootstrapValidator({
        live: 'disabled',
        feedbackIcons: {
            validating: 'glyphicon glyphicon-refresh'
        },
        locale: 'fa_IR',
        group: '.form-feild',
        fields: {
            PhoneNumber: {
                validators: {
                    notEmpty: {
                        message: ''
                    },
                    //numeric: {
                    //    message: 'لطفا مقدار عددی وارد نمائید.',

                    //},
                    //callback: {
                    //    callback: function (value, validator) {
                    //        return isValidPhoneNumber(value);
                    //    }
                    //},

                }
            },
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
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.Success) {
                    location.href = result.Url;
                    //toastr.success(result.MessageList, '', { positionClass: "toast-bottom-center" });
                    //$.get("/Account/NewConfirmLoginCode", {
                    //    PhonNumber: result.Params2,
                    //    retUrl: result.Params1,
                    //    SendDate: result.Params3
                    //}, function (result2) {
                    //    $(".k-code-box").html(result2);
                    //    $("#p_1").focus();
                    //    $form.removeClass("DisableItems");
                    //});
                }
                else {
                    toastr.error(result.MessageList, '', { positionClass: "toast-bottom-center" });
                    $form.removeClass("DisableItems");
                    $form.find("button[type='submit']").prop("disabled", false);
                }
            }
        });
    });
});
