$(document).ready(function () {
    $('#EditPasswordForm').bootstrapValidator({
        live: 'enabled',
        feedbackIcons: {
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            "ApplicationUser.OldPassword": {
                validators: {
                    notEmpty: {

                    }
                }
            },
            "ApplicationUser.Password": {
                validators: {
                    notEmpty: {

                    },
                    stringLength: {
                        min: 6
                    }
                }
            },
            "ApplicationUser.ConfirmPassword": {
                validators: {
                    notEmpty: {
                    },
                    identical: {
                        field: 'ApplicationUser.Password',

                    },
                    stringLength: {
                        min: 6
                    }
                }
            }
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
                    $("#OldPassword").val('');
                    $("#Password").val('');
                    $("#ConfirmPassword").val('');
                }
                else {
                    toastr.error(result.MessageList, '', { positionClass: "toast-bottom-center" });
                }
                $form.removeClass("DisableItems");
                $("button[type='submit']").prop('disabled', false);
            },
            error: function (jqXHR, error, errorThrown) {
                if (jqXHR.status && jqXHR.status == 400) {
                    alert(jqXHR.responseText);
                } else {
                    alert("Something went wrong");
                }
            },
            cache: false,
            contentType: false,
            processData: false
        });
    });
});
