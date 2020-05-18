$(document).ready(function () {


    $("#btn-show-ssn").click(function (e) {
        $("#div-fg-redssn").addClass("hidden");
        $("#div-fg-ssn").removeClass("hidden");
    });

    $("#btn-show-red").click(function (e) {
        $("#div-fg-ssn").addClass("hidden");
        $("#div-fg-redssn").removeClass("hidden");
    });


});