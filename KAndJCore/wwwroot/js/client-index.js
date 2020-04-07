$(document).ready(function () {

    var urlParams = new URLSearchParams(window.location.search);
    if (urlParams.has('key')) {
        $("#txt-search").val(urlParams.get('key'));
        $("#txt-search").focus();
    }

    $("#btn-search").click(function (e) {
        e.preventDefault();
        $("#spn-search").css("display", "none");
        var val = $("#txt-search").val();
        if (!val || val.length < 3)
            $("#spn-search").css("display", "inline");
        else
            window.location.href = "./Clients?key=" + val;
    });

    $("#txt-search").keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $("#btn-search").click();
        }
    });


});