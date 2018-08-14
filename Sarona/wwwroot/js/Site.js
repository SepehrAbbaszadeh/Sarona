$(document).ready(function () {
    $("input.input-validation-error")
        .closest(".form-group").addClass("has-danger");
});

$(document).ready(function () {

    $('.clickable-row tr').click(function () {
        var href = $(this).find("a").attr("href");
        if (href) {
            window.location = href;
        }
    });

});
