
$(document).on('click', '.menu-item', function (event) {
    $(".menu-item").removeClass("hover show");
    $(this).toggleClass("hover show");
    localStorage.setItem("SelectedSubLi", $(this).attr("id"));
});

if (localStorage.getItem("SelectedSubLi") !== null) {
    var Selector = "#" + localStorage.getItem("SelectedSubLi");
    $(Selector).addClass('hover show');
   
}


function showmsg(obj) {
   
    obj = obj.toString();
    obj = obj.replace(/$$/g, "'");
    obj = obj.replace(/%%/g, "\"");

    swal({
        title: "شرح",
        text: obj,
        html: true,
        confirmButtonText: "بستن"
    });
}

