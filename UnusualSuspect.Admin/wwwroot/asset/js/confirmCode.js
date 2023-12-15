
var doFocus = function (eid) {
    if (!!eid)
        document.getElementById(eid).focus();
}

function moveLeft(myId) {
    var prevId = myId > 1 ? `p_${myId - 1}` : null
    doFocus(prevId)
}

function moveRight(myId) {
    var nextId = myId < 5 ? `p_${myId + 1}` : null
    doFocus(nextId)
}

function onKeyUp(e) {
    var field = this
    var myId = +field.getAttribute('id').split('_')[1]

    if (e.keyCode == 37) {
        // Left arrow
        moveLeft(myId)
    } else if (e.keyCode == 39) {
        // Right arrow
        moveRight(myId)
    } else if (e.keyCode != 8 && field.value.length == 1) {
        // The field has exactly one character
        moveRight(myId)
    }

    checkPin()
}

function onKeyDown(e) {
    var letGo = [46, 8, 9, 27, 13, 110, 190].some(k => k == e.keyCode)
        // Allow: Ctrl+A, Command+A
        || (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true))
        // Allow: home, end, left, right, down, up
        || (e.keyCode >= 35 && e.keyCode <= 40)

    // Ensure that it is a number and stop the keypress
    if (
        !letGo
        && (
            (e.shiftKey || (e.keyCode < 48 || e.keyCode > 57))
            && (e.keyCode < 96 || e.keyCode > 105)
        )
    ) {
        e.preventDefault();
    }

    var field = this
    var myId = +field.getAttribute('id').split('_')[1]
    if (e.keyCode == 8) {
        // Backspace
        if (field.value == '') {
            moveLeft(myId)
            e.preventDefault();
            return false;
        } else {
            field.value = ''
            return true
        }
    }
    return true
}

function checkPin() {
    var pin = [...document.querySelectorAll('.k-code')].map(e => e.value).join('')
    // document.getElementById('submit')[pin.length == 4 ? 'setAttribute' : 'removeAttribute']('class', 'active')
}

[...document.querySelectorAll('.k-code')].map(e => {
    e.addEventListener('keyup', onKeyUp, true)
    e.addEventListener('keydown', onKeyDown, true)
}
)

//document.getElementById('p_1').focus();

// //////////////////////////////////////////

$(".k-code").on("keyup", function () {
    $(".k-code").each(function () {
        if ($(this).val() != "") {
            $("#submit").addClass('active');
        } else {
            $("#submit").removeClass('active');
        }
    });
});

// login with code
var mobInputAction = function () {
    $(".k-code-box").addClass("on");
    $("#mobInput").hide();
    //$("#mobBtn").hide();
    $(".k-sign-label").text("کد تایید را وارد کنید");
    // $(".sign > div").css("min-height", "420px");

    var phone = $("#mobInput").val();
    // $('.code-box p span').text(phone[0] + phone.slice(1, 5) + "*".repeat(4) + phone.slice(-2));
    $(".k-code-box p span").text(phone);
    $('#p_1').focus();
    doFocus();
    timer();
};

$("document").ready(function () {
    $("#mobBtn").click(function () {
        mobInputAction();
    });
    $("#mobInput").keypress(function (e) {
        var inpVal = $("#mobInput").val();
        if (e.which == 13 && inpVal) {
            //Enter key pressed
            console.log($("#mobInput").val());
            mobInputAction();
        }
    });
    $(".k-reSend").on("click", function () {
        timer();
        $(".k-countdown").show();
        $(this).hide();
    });

    $(".k-changeNumber").on("click", function () {
        var minp = $("#mobInput");
        $(".k-code-box").removeClass("on");
        $(".k-code").val("");
        // minp.removeClass("deActive");
        // $(".sign > div").css("min-height", "200px");
        minp.show();
        $("#mobBtn").show();
        var tmpStr = minp.val();
        minp.val("");
        minp.val(tmpStr);
        minp.focus();
        $(".k-sign-label").text("شماره همراه خود را وارد کنید");
    });
});

// ///////////////////////////////

$(".k-step-one button").on("click", function () {
    $(".k-step-one").hide();
    $(".k-code-box").show();
    doFocus();
});

// timer               //////////////////////////
var timer = function () {
    var timer2 = "2:01";
    var interval = setInterval(function () {
        var timer = timer2.split(":");
        //by parsing integer, I avoid all extra string processing
        var minutes = parseInt(timer[0], 10);
        var seconds = parseInt(timer[1], 10);
        --seconds;
        minutes = seconds < 0 ? --minutes : minutes;
        if (minutes < 0) {
            clearInterval(interval);
            $(".k-countdown").hide();
            $(".k-reSend").show();
        }
        seconds = seconds < 0 ? 59 : seconds;
        seconds = seconds < 10 ? "0" + seconds : seconds;
        //minutes = (minutes < 10) ?  minutes : minutes;
        $(".k-countdown").html(minutes + ":" + seconds);
        timer2 = minutes + ":" + seconds;
    }, 1000);
};
// ////////////////////////////////////////////


// Install input filters.
//setInputFilter(document.getElementById("mobInput"), function (value) {
//    return /^-?\d*$/.test(value);
//}, "Must be an integer");


