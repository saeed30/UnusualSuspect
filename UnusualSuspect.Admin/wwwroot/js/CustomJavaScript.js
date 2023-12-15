$(document).on('focus', '.select2-selection.select2-selection--single', function (e) {
    $(this).closest(".select2-container").siblings('select:enabled').select2('open');
});
$(document).on('select2:open', () => {
    document.querySelector('.select2-search__field').focus();
});


function RenderSelectTwo($Selectore) {
    $Selectore.select2({
        escapeMarkup: function (markup) {
            return markup;
        },
        language: "fa",
        selectionTitleAttribute: false,
        matcher: function (params, data) {
            return matchStart(params, data);
        }
    });
}
function RenderSelectTwoInModel($Selectore) {
    $Selectore.select2({
        escapeMarkup: function (markup) {
            return markup;
        },
        dropdownParent: $('#ModalDiv'),
        language: "fa",
        selectionTitleAttribute: false,
        //matcher: function (params, data) {
        //    return matchStart(params, data);
        //}
    });
}
function RenderSelectTwo($Selectore, placeholder) {
    $Selectore.select2({
        language: "fa",
        placeholder: placeholder,
    });
}


function matchStart(params, data) {
    params.term = params.term || '';
    if (data.text.toUpperCase().indexOf(params.term.toUpperCase()) === 0) {
        return data;
    }
    return false;
}

function PersianDatepicker($Selector) {
    $Selector.pDatepicker({
        viewMode: 'day',
        initialValue: false,
        initialValueType: 'persian',
        observer: true,
        format: 'YYYY/MM/DD',
        autoClose: true,
        dayPicker: {
            onSelect: function (v) {
            }
        }
    });
}

function PersianDateTimepicker($Selector) {
    $Selector.pDatepicker({
        viewMode: 'day',
        initialValue: false,
        initialValueType: 'persian',
        observer: true,
        format: 'YYYY/MM/DD hh:mm:ss',
        autoClose: true,
        timePicker: {
            enabled: true,
            meridian: {
                enabled: false
            },
            second: {
                enabled: false
            }
        },
    });
}

function ReadImageFromURL(input, $Selector) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $Selector.attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}


function animationWOW(boxClass) {
    wow = new WOW(
        {
            boxClass: boxClass,      // default
            animateClass: 'animated', // default
            offset: 0,          // default
            mobile: true,       // default
            live: false        // default
        }
    );
    wow.init();
}

function ReturnSelectListItems(ChildSelect, ActionName, jsonData) {
    $(ChildSelect).closest("div").addClass("DisableItemsSelect");
    $.ajax({
        url: ActionName,
        type: "POST",
        dataType: "Json",
        data: jsonData,
        success: function (items) {
            $(ChildSelect).find('option').remove();
            $.each(items, function (k) {
                $(ChildSelect)
                    .append($("<option></option>")
                        .attr("value", items[k].Value)
                        .text(items[k].Text));
            });
            $(ChildSelect).closest("div").removeClass("DisableItemsSelect");
        }
    });
}


function RenderSelectTwoWithAjax($Selectore, Url, RunAnotherAction) {
    $Selectore.select2({
        dir: "rtl",
        minimumInputLength: 2,
        selectOnClose: true,
        language: {
            "noResults": function () {
                return "آیتم مورد نظر شما پیدا نشد";
            },
            searching: function () {
                return "جستجو...";
            },
            inputTooShort: function (args) {
                return " لطفا بیشتر از " + args.minimum + " کاراکتر وارد نمائید ";
            },
            errorLoading: function () {
                return "نتیجه ای پیدا نشد";
            }
        },
        escapeMarkup: function (markup) {
            return markup;
        },
        ajax: {
            url: Url,
            dataType: 'json',
            delay: 100,
            data: function (params) {
                return {
                    term: params.term
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data
                };
            }
        }
    });
}


function convertNumbers2English(string) {
    return string.replace(/[\u0660-\u0669]/g, function (c) {
        return c.charCodeAt(0) - 0x0660;
    }).replace(/[\u06f0-\u06f9]/g, function (c) {
        return c.charCodeAt(0) - 0x06f0;
    });
}
function isValidPhoneNumber(input) {
    input = convertNumbers2English(input);
    if (!/(0|\+98)?([ ]|-|[()]){0,2}9[0|1|2|3|4|9]([ ]|-|[()]){0,2}(?:[0-9]([ ]|-|[()]){0,2}){8}/.test(input))
        return {
            valid: false,    // or false
            message: "لطفا شماره موبایل معتبر وارد نمائید"
        }
    else {
        return {
            valid: true,    // or false
        }
    }
}
function isValidItem2(input, input2,message) {

    if (input2 == 2 && input == '')
        return {
            valid: false,    // or false
            message: message
        }
    else {
        return {

            valid: true,    // or false
        }
    }
}
function CheckStrongPassWord(input) {
    input = convertNumbers2English(input);
    if (!/(?=.*[a-z])(?=.*[A-Z])/.test(input))
        return {
            valid: false,    // or false
            message: "رمز عبور باید حداقل شامل یک حرف انگلیسی کوچک و بزرگ باشد"
        }
    if (!/(?=.*[0-9])/.test(input))
        return {
            valid: false,    // or false
            message: "رمز عبور باید حداقل شامل یک عدد باشد"
        }
    if (!/(?=.*[!@#\$%\^&\*])/.test(input))
        return {
            valid: false,    // or false
            message: "رمز عبور باید حداقل شامل یکی از سیمبل های *[!@#$%^&*].  باشد"
        }
    return {
        valid: true,    // or false
    }
}

function LessThan(input, min) {
    input = convertNumbers2English(input);
    if (input < min)
        return {
            valid: false,    // or false
            message: "مقدار وارد شده باید بزرگتر از " + min + " باشد."
        }
    else {
        return {
            valid: true,    // or false
        }
    }

}

function isValidIranianNationalCode(input, flag) { 
    if (flag == 1) {

        if (!/^\d{10}$/.test(input))
            return {
                valid: false,    // or false
                message: 'کد ملی معتبری وارد نمایید'
            }
        var check = parseInt(input[9]);
        var sum = 0;
        var i;
        for (i = 0; i < 9; ++i) {
            sum += parseInt(input[i]) * (10 - i);
        }
        sum %= 11;
        var result = (sum < 2 && check === sum) || (sum >= 2 && check + sum === 11);
        if (result)
            return {
                valid: true,    // or false
            }
        else
            return {
                valid: false,    // or false
            }
    }
    else {
        var L = input.length;

        if (L < 11 || parseInt(input, 10) == 0) return false;

        if (parseInt(input.substr(3, 6), 10) == 0) return false;
        var c = parseInt(input.substr(10, 1), 10);
        var d = parseInt(input.substr(9, 1), 10) + 2;
        var z = new Array(29, 27, 23, 19, 17);
        var s = 0;
        for (var i = 0; i < 10; i++)
            s += (d + parseInt(input.substr(i, 1), 10)) * z[i % 5];
        s = s % 11; if (s == 10) s = 0;
        return (c == s);
    }

   
}

function isValidDigit(input) {
    input = convertNumbers2English(input);
    return {
        valid: true,    // or false
    }
}

function RenderTinyMce(Selectore, $FocusSelector, language) {
    if (!language)
        language = 'fa_IR';
    tinymce.init({
        selector: Selectore,
        statusbar: false,
        autosave_ask_before_unload: false,
        setup: function (ed) {
            ed.on('init', function (ed) {
                this.execCommand("fontName", false, "IRANSans");
                this.execCommand("fontSize", false, "13px");
                //if ($FocusSelector != null)
                //    $FocusSelector.focus();
            });
            ed.on('LoadContent', function (e) {
                if ((language === 'fa_IR') || (language === 'ar_MA'))
                    $("iframe").contents().find("body#tinymce").attr("dir", "rtl").css("font-family", "IRANSans");
            });
        },
        language: language,
        directionality: "rtl",
        font_formats: "IRANSans=IRANSans;",
        fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt",
        plugins: [
            "textcolor colorpicker advlist directionality autolink autosave link image lists charmap print preview hr anchor pagebreak",
            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
            "table contextmenu  paste  fullpage"
        ],
        height: 350,
        theme: 'silver',
        toolbar: "rtl ltr  | fontselect |  fontsizeselect | forecolor backcolor  bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
        color_picker_callback: function (callback, value) {
            callback('#FF00FF');
        },
        textcolor_cols: "5",
        images_upload_url: '/AdminPanel/MiscPictures/UploadImage',
    });

}


$(document).on("click", "#selectLanguage .language-items a", function () {
    $("#selectLanguage #culture").val($(this).data("flagname"));
    $("#selectLanguage").submit();
});

$(document).on("keyup", ".price", function () {
    $(this).val(numeral($(this).val()).format(0, 0));
});
function animateshowHidden(classSelected) {
    var i = 1;
    $(classSelected).each(function () {
        var pic = $(this);
        setTimeout(function () {
            pic.removeClass('hide').addClass("show");
        }, i++ * 200);
    });
}



function AddSpinner($this) {
    $this.attr("disabled", true);
    $this.find(".spinner-border").first().removeClass("hide");
    $this.find(".fa").first().addClass("hide");
}


function RemoveSpinner($this) {
    $this.attr("disabled", false);
    $this.find(".spinner-border").first().addClass("hide");
    $this.find(".fa").first().removeClass("hide");
}

