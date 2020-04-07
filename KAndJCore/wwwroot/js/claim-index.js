// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);
    if (urlParams.has('key')) {
        $("#txt-search").val(urlParams.get('key'));
        $("#txt-search").focus();
    }

    $("#btn-search").click(function (e) {
        alert($(this).attr("data-pending"))
        e.preventDefault();
        $("#spn-search").css("display", "none");
        var val = $("#txt-search").val();
        if (!val || val.length < 3)
            $("#spn-search").css("display", "inline");
        else
            window.location.href = ($(this).attr("data-pending") === "true") ? "./Pending?key=" + val : "./Claims?key=" + val;
    });

    $("#txt-search").keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $("#btn-search").click();
        }
    });

    $(".a-next-revision").click(function (e) {
        e.preventDefault();
        var id = $(this).attr("data-claim-id");
        $.ajax({
            type: "POST",
            url: "../Claims/NextRevision/",
            data: { "id": id }
        }).done(function (data) {
            var component = $("#spn-revision-" + id);
            component.find(".spn-revision-date").html(data.newDate);
            component.find(".spn-revision").html(data.revision);
            if (data.revision >= 3) {
                $("#a-revision-" + id).css("display", "none");
                $("#a-close-claim-" + id).css("display", "inline");
                $("#a-reopen-claim-" + id).css("display", "none");
            }
        });
    });

    $(".a-close-claim").click(function (e) {
        e.preventDefault();
        var id = $(this).attr("data-claim-id");
        $.ajax({
            type: "POST",
            url: "../Claims/CloseClaim/",
            data: { "id": id }
        }).done(function (data) {
            $("#spn-revision-" + id).css("display", "none");
            $("#spn-revision-closed-" + id).css("display", "inline");
            $("#spn-status-" + id).addClass("red").html("Closed");
            $("#a-revision-" + id).css("display", "none");
            $("#a-close-claim-" + id).css("display", "none");
            $("#a-reopen-claim-" + id).css("display", "inline");

        });
    });

    $(".a-reopen-claim").click(function (e) {
        e.preventDefault();
        var id = $(this).attr("data-claim-id");
        $.ajax({
            type: "POST",
            url: "../Claims/ReopenClaim/",
            data: { "id": id }
        }).done(function (data) {
            var component = $("#spn-revision-" + id);
            component.find(".spn-revision-date").html(data.newDate);
            component.find(".spn-revision").html(1);
            component.css("display", "inline");
            $("#spn-revision-closed-" + id).css("display", "none");
            $("#spn-status-" + id).removeClass("red").html("Open");
            $("#a-revision-" + id).css("display", "inline");
            $("#a-close-claim-" + id).css("display", "none");
            $("#a-reopen-claim-" + id).css("display", "none");
        });
    });




});