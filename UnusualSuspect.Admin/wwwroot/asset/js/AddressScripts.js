$("select#StateId").on('change', function (e) {
    var optionSelected = $("option:selected", this);
    var ProvinceId = this.value;
    $("#StateAndCityId").closest("div").addClass("DisableItemsSelect");
    $.ajax({
        url: "/Customer/RetrieveCitiesOfProvince",
        type: "POST",
        dataType: "Json",
        data: "ProvinceId=" + ProvinceId,
        success: function (CityList) {

            $('#StateAndCityId').find('option').remove();
            $.each(CityList, function (k) {
                $('select#StateAndCityId')
                    .append($("<option></option>")
                        .attr("value", CityList[k].Value)
                        .text(CityList[k].Text));
            });
            $("#StateAndCityId").closest("div").removeClass("DisableItemsSelect");
        }
    });
});

$(document).ready(function () {


    $('.RegionBox li:not(.searchAddress)').on('click', function () {

        $(this).closest('.select-box').find('.select-box-btn span').text($(this).text());

        $('#StateId').val($(this).data("id"));


        $.get("/ShoppingCart/RetrieveCitiesOfProvince", {

            ProvinceId: $(this).data("id")
        }, function (CityList) {
            $(".CityBox li").closest('.select-box').find('.select-box-btn span').text("انتخاب شهر");

            $('.CityBox').find('li:not(.searchAddress)').remove();
            $.each(CityList, function (k) {
                $('.CityBox')
                    .append($("<li></li>")
                        .attr("data-id", CityList[k].Value)
                        .text(CityList[k].Text));

                $('.CityBox li:not(.searchAddress)').on('click', function () {

                    $(this).closest('.select-box').find('.select-box-btn span').text($(this).text());

                    $('#CityId').val($(this).data("id"));

                });

            });
        });




    });

    $('.CityBox li:not(.searchAddress)').on('click', function () {

        $(this).closest('.select-box').find('.select-box-btn span').text($(this).text());

        $('#CityId').val($(this).data("id"));

    });

    $(".searchAddress-input").on("keyup", function () {
        var value = this.value.toLowerCase().trim();
        $(this).closest('.select-box-ul').find('li:not(.searchAddress)').show().filter(function () {
            return $(this).text().toLowerCase().trim().indexOf(value) == -1;
        }).hide();
    });


});

$(document).on("click", ".DeleteItem", function () {

    var $ParentTd = $(this);
    var Id = $(this).data("id");
    swal({
        title: $ParentTd.data("titlesweet"),
        text: $ParentTd.data("textsweet"),
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: $ParentTd.data("confirmbtn"),
        cancelButtonText: $ParentTd.data("cancelbtn"),
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, function () {

        $.ajax({
            url: $ParentTd.data("action") + "/" + Id,
            dataType: "json",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            cache: false,
            success: function (data) {

                if (data.Success) {

                    $('#editAddress').modal('hide');

                    $.get("/ShoppingCart/ShowAddressBoxInFactor", {
                    }, function (data) {
                        //$(".cart-button").removeClass("hide");
                        $(".address-sec").html(data);
                    });

                    swal(data.MessageList, "", "success");
                }
                else {
                    swal(data.MessageList, "", "warning");
                }

            },
            error: function (xhr) {
                swal("خطایی  در حذف اطلاعات اتفاق افتاده است", "", "warning");
            }
        });


    });
});