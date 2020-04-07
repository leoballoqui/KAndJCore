// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

    $("#form-submit").on('click', function (e) {
        var disputes = [];
        $(".div-dispute").each(function (index) {
            var dispute = new Object();
            if (!$(this).hasClass("deleted-dispute")) {
                dispute.id = $(this).attr("data-id");
                dispute.completeReason = $(this).find(".txt-dispute").val();
                disputes.push(dispute);
            }
        });
        $("#IncludedAccounts").val(JSON.stringify(disputes));
        console.log(JSON.stringify(disputes));
       // e.preventDefault();
    });

    $(".btn-delete").on('click', function (e) {
        $(this).parent().toggleClass("deleted-dispute");
        //$(this).parent.toggleClass("deleted-dispute");
    });

   
});