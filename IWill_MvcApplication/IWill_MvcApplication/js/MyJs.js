function changeRowColorAndFocus(obj) {
    $(obj).css("background-color", "#FFBFB1");

    var w = $(window);
    var row = $(obj);
    if (row.length) {
        $('html,body').animate({ scrollTop: row.offset().top - (w.height() / 2) }, 1000);

    }

    setTimeout(function () {
        $(obj).css("background-color", "");
    }, 3000);
}

function ValidateControl(obj) {
    if (obj.val().length == 0 || obj.val() == "null") {
        obj.css("border", "1px solid red");
        obj.focus();
        return false;
    }
    else
        obj.css("border", "");
}

function ErrorMsgTimeOut(Msg) {
    $("div.error").fadeIn();
    $("div.error").html(Msg)
    setTimeout(function () { $('.error').fadeOut('slow'); }, 5000);

}
function SuccessMsgTimeOut(Msg) {


    $("div.success").fadeIn('slow');
    $("div.success").html(Msg)
    setTimeout(function () { $('.success').fadeOut('slow'); }, 5000);

}



function ShowLoading(obj) {
    obj.addClass('loading');
    obj.append('<div class="cssload-container"><div class="cssload-loading"><i></i><i></i><i></i><i></i></div></div>');
}
function HideLoading(obj) {
    obj.removeClass('loading');
    obj.find('.cssload-container').remove();
}
