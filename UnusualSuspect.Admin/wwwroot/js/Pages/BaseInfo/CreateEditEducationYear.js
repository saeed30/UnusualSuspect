$(document).ready(function () {
    $('#CreateEditEducationYearForm').bootstrapValidator({
        live: 'enabled',
        feedbackIcons: {
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            'Name': {
                validators: {
                    notEmpty: {
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
                if (result.Success) {
                    toastr.success(result.MessageList, '', { positionClass: "toast-bottom-center" });

                    $("#grid").data("kendoGrid").dataSource.read();
                    $('#ModalDiv').modal('hide');


                    $('#grid').data('kendoGrid').dataSource.read().then(function () {
                        var dataGrid = $('#grid').data('kendoGrid');
                        let dataView = dataGrid.dataSource.view();

                        for (let i = 0; i < dataView.length; i++) {

                            if (dataView[i].Id == result.Id) {

                                dataGrid.table.find("tr[data-uid='" + dataView[i].uid + "']").addClass("alert-success-row");
                            }
                        }
                    });


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
});
