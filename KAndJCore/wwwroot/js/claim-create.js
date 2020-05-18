// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);
    var cloning = $("#form-createClaim").attr("data-cloning");
    var cloningBuroId = $("#form-createClaim").attr("data-cloning-buro-id");
    var cloningTemplateId = $("#form-createClaim").attr("data-cloning-template-id");
    var cloningDisputes = $("#form-createClaim").attr("data-cloning-disputes");

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
            window.location.href = "/Claims/Create/?key=" + val;
    });

    $("#txt-search").keypress(function (e) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $("#btn-search").click();
            e.preventDefault();
        }
    });

    $(".cbx-buro").click(function () {
        $("#lbl-error-buro").css("display", "none");
        var includedBuros = "";
        $(".cbx-buro").each(function (index) {
            if ($(this).is(":checked"))
                includedBuros += $(this).attr("data-buro-id") + ";";
        });
        $("#IncludedBuros").val(includedBuros);
    });

    $(".sel-client").change(function () {
        $.ajax({
            url: "../../Accounts/AccountsByClient/" + $(this).val()
        }).done(function (data) {
            $(".div-account").remove();
            $(".div-dispute").remove();
            $.each(data.accounts, function (index, item) {
                var component = $("#div-accountSample").clone();
                component.addClass("div-account");
                component.attr("id", "");
                component.find(".lbl-account").html(item.alias);
                component.find(".cbx-account").attr("data-account-id", item.id);
                component.find(".cbx-account").attr("data-account-alias", item.alias);
                component.find(".cbx-account").attr("data-account-reason", resolveReason(item));
                component.css("display", "inline");
                component.appendTo("#div-accountsContainer");
            });

            $("#div-disputesContainer").attr("data-address", data.address);

            $(".cbx-account").click(function () {
                $("#lbl-error-dispute").css("display", "none");
                if ($(this).is(":checked")) {
                    var component = $("#div-disputeSample").clone();
                    component.addClass("div-dispute");
                    component.attr("id", "div-dispute-" + $(this).attr("data-account-id"));
                    component.attr("data-account-id", $(this).attr("data-account-id"));
                    component.find(".lbl-dispute").html($(this).attr("data-account-alias"));
                    component.find(".txt-dispute").html($(this).attr("data-account-reason"));
                    component.find(".btn-dispute").click(function () {
                        component.find(".txt-dispute").html(component.find(".txt-dispute").html() + " " + data.address);
                    });
                    component.css("display", "inline");
                    component.appendTo("#div-disputesContainer");
                }
                else {
                    $("#div-dispute-" + $(this).attr("data-account-id")).remove();
                }
                
            });

            if (cloning === "True") {
                $(".cbx-buro[data-buro-id='" + cloningBuroId + "']").click();
                $("#TemplateId").val(cloningTemplateId);
                $.each(JSON.parse(cloningDisputes), function (index, item) {
                    $(".cbx-account[data-account-id='" + item + "']").click();
                    // Aqui ver si Jessica necesita importar tambien las "Completed Reason"
                });
            }

        });
    });

    $("#form-submit").on('click', function (e) {
        if ($(".cbx-buro:checked").length < 1) {
            $("#lbl-error-buro").css("display", "inline");
            e.preventDefault();
        }
        if ($(".cbx-account:checked").length < 1) {
            $("#lbl-error-dispute").css("display", "inline");
            e.preventDefault();
        }
        var disputes = [];
        $(".div-dispute").each(function (index) {
            var dispute = new Object();
            dispute.accountId = $(this).attr("data-account-id");
            dispute.completeReason = $(this).find(".txt-dispute").html();
            disputes.push(dispute);
        });
        $("#IncludedAccounts").val(JSON.stringify(disputes));

    });

    function resolveReason(account) {
        return account.reason.value.replace("{owner}", account.owner).replace("{account}", account.alias);
    }

    $(".sel-client").trigger('change');
});