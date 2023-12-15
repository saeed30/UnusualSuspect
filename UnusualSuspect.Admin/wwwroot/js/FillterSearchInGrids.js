var Step = 1;
function SendToServer() {
    var $form = $('form#SearchForm');
    var formData = new FormData($form[0]);
    formData.append('Step', Step);
    
    $.ajax({
        beforeSend: function () {
        },
        url: $form.attr("action"),
        type: $form.attr("method"),
        data: formData,
        success: function (result) { 
            $("#ResultSearchList").html(result);
            $("#ResultSearchList").removeClass("DisableItems");
            $("button[type='submit']").prop('disabled', false);
        },
        cache: false,
        contentType: false,
        processData: false
    });
}

function SubmitSearch($Selectore) {
    $Selectore.bootstrapValidator({
        live: 'enabled',
        feedbackIcons: {
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {

        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        Step = 1;
        SendToServer();
    });
}

$(document).on("click", ".previous", function () {
    Step = Step - 1;
    if (Step >= 1)
        SendToServer();
    if (Step === 1)
        $(".left-sideSearch").addClass("hide");
    if (Step < 1)
        Step = 1;
});

$(document).on("click", ".next", function () {
    Step = Step + 1;
    SendToServer();
    if (Step > 1)
        $(".left-sideSearch").removeClass("hide");
});
$(document).on("click", ".Page-number", function () {
    Step = parseInt($(this).attr("data-step"), 10);
    SendToServer();
});


//$(document).on("submit", "#SearchForm", function () {
//    SendToServer();
//});


$(document).on("click", ".CreateItemInOverLay , .EditItemInOverLay", function () {
   
    $(".OverLayDiv").removeClass("hide");
    $(".OverLayDiv").addClass("DisableItems");
    $.get($(this).data("remote"), {
    }, function (data) {
        $(".OverLayDiv").html(data);
        $(".OverLayDiv").removeClass("DisableItems");
    });
});
$('body').on("click", ".EditItem , .btn-create", function () {
    $("#ModalDiv .modal-dialog").removeClass("modal-lg minmodal");
    $("#ModalDiv .modal-dialog").addClass($(this).data("modaltype"));
    $('#ModalDiv .modal-content').load($(this).data("remote"));
   
    //$.get($(this).data("remote"), {

    //}, function (data) { 
    //    $('#ModalDiv .modal-content').html(data);
    //});
     

    $("#ModalDiv").modal('show');

});

$(document).on("click", "#cancelBtn", function () {
    var ParentDiv = $(this).closest(".OverLayDiv");
    ParentDiv.addClass("hide").empty();
});
$(document).on("click", ".DetailsInNextTrBtn", function () {
    $(this).closest("tr").next("tr.detailsTr").toggleClass("hide");
});


$(document).on("click", ".a-create", function () {
    //$(".btn-create").addClass("flash").removeClass("zoomIn");
    $(".btn-create").css("animation-name",'jello');
    animationWOW('btn-create');
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
            url: $ParentTd.data("action")+"/" + Id,
            dataType: "json",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            cache: false,
            success: function (data) {

                if (data.Success) {
                    var grid = $("#grid").data("kendoGrid");
                    grid.dataSource.read();
                  
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
$(document).on("click", ".DeleteItemtype2", function () {
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
        closeOnConfirm: true,
        showLoaderOnConfirm: true
    }, function () {
        $.get($ParentTd.data("action"), {
            Id: Id
        }, function (result) {
            var selector = '#ResultSearchList ' + '#' + Id;
            if (result.Success) {
                toastr.success(result.MessageList, '', { positionClass: "toast-bottom-center" });
                $(selector).remove();
            }
            else {
                toastr.error(result.MessageList, '', { positionClass: "toast-bottom-center" });
                $(selector).addClass("alert-danger");
            }
        });
    });
});

function HoverImage($Selectore) {
    $Selectore.popover({
        content: function () {
            return '<img style="width:100%;" src="' + $(this).attr('src') + '" />';
        },
        html: true,
        placement: "right",
        trigger: "hover"
    }); 
}


$(document).on("click", ".getreport", function () {
    var UrlAc = $(this).data("url");
    var FileType = $(this).data("filetype");
    var $form = $('form#SearchForm');
    var formData = new FormData($form[0]);
    formData.append('Step', Step);
    formData.append('FileType', FileType);
    $.ajax({
        beforeSend: function () {
            $("#ResultSearchList").addClass("DisableItems");
        },
        url: UrlAc,
        type: $form.attr("method"),
        data: formData,
        success: function (result) {
            if (result.params1 !== "") {
                window.location.href = "/Home/Download/?filename=" + result.params1;
            }
            $("#ResultSearchList").removeClass("DisableItems");
        },
        cache: false,
        contentType: false,
        processData: false
    });
});
 
function RefreshGrid(u) {

    

    var g = $("#grid").data("kendoGrid");
    g.dataSource.transport.options.read.url = u ;
     g.dataSource.read();

}

$("#searchForm").submit(function (event) {

    var $form = $('#searchForm');
    var data = $form.serialize();

    event.preventDefault();
    var url = $('.SearchBtn').data("url");
   
    RefreshGrid(url + '?' + data);
});


function showDetails(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var url = '@Url.Content("~/Slide/ShowSlides/")' + dataItem.Id;
    window.location.href = url;
}


$(document).on("click", ".nextpreviewFromProject", function () {
    var id = $(this).data("id"); 
    $.ajax({

        url: '/Member/CheckExistProjct/")',
        type: 'GET',
        cache: false,
        data: {
            memberId: id
}
                            }).done(function (result) {

                                if (result.Success) {

        $.get("/Member/PreView", {
            MemberId: id,
    }, function (data) {

        //document.getElementById("Previewid").click();
        openCity($("#Previewid"), 'Preview');
        $("#Preview").html(data);
    });
                                }
                                else {

    swal(result.MessageList, "", "error");
}
                            });
    });

$(document).on("click", ".qgactive", function () {
  
    var cid = $(this).attr('rel');
    $.ajax({
        type: "POST",
        url: "/Project/SelectIndexProject/",
        data: { id: cid },
        success: function (data) {
            $("#grid").data("kendoGrid").dataSource.read();
            $("#grid").data("kendoGrid").refresh();
            if (data.Success) {
                swal(data.MessageList, "", "success");
            }
            else {
                swal(data.MessageList, "", "error");
            }

        }
    });
});
