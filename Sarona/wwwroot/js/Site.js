
// Text format of validation.
$(document).ready(function () {
    $("input.input-validation-error")
        .closest(".form-group").addClass("has-danger");
});


// Clickable rows in tables with clickable-row class and "a" tag.
$(document).ready(function () {

    $('.clickable-row tr').click(function () {
        var href = $(this).find("a").attr("href");
        if (href) {
            window.location = href;
        }
    });

});

//Populate exchange "select tag".
function PopulateExchange(area, selectId) {
    $("#" + selectId).text("");
    $("#" + selectId).append("<option disable selected value=''>Please select an exchange.");
    $.getJSON("/api/exchange/" + area, function (result) {
        $.each(result, function () {
            $("#" + selectId).append($("<option />").val(this.abb).text(this.abb + " (" + this.name + ")"));
        });
    });
}

function PopulateNE(exchangeAbb, type, selectId) {
    $("#" + selectId).text('');
    $("#" + selectId).append("<option disable selected value=''>Please select an NE.");
    $.getJSON("/api/ne/" + exchangeAbb + "/" + type, function (result) {
        $.each(result, function () {
            $("#" + selectId).append($("<option />").val(this.id).text(this.name + " (" + this.model + ")"));
        });
    });
}

function PopulateCustomer(name, selectId) {
    $("#" + selectId).val();
    $("#" + selectId).text('');
    $.getJSON("/api/Customer/Name/" + name, function (result) {
        $.each(result, function () {
            $("#" + selectId).append($("<option />").val(this.id).text(this.name + " (" + this.abb + ")"));
        });
    });
}

function AbbValidation(newAbb, oldAbb, validationId) {
    var dup;
    newAbb = newAbb.toUpperCase();
    oldAbb = oldAbb.toUpperCase();
    $("#" + validationId).text("");
    if (newAbb === oldAbb)
        return true;
    $.get({
        async: false,
        url: '/Network/AbbValidation',
        data: { abb: newAbb },
        success: function (result) {
            if (result === true)
                dup = true;
            else {
                $("#" + validationId).text(result);
                dup = false;
            }
        }
    });
    return dup;
}

function NeNameValidation(neName, oldName, validationId) {
    var dup;
    neName = neName.toUpperCase();
    oldName = oldName.toUpperCase();
    $("#" + validationId).text("");
    if (neName === oldName)
        return true;
    $.get({
        async: false,
        url: '/Network/NeNameValidation',
        data: { name: neName },
        success: function (result) {
            if (result === true)
                dup = true;
            else {
                $("#" + validationId).text(result);
                dup = false;
            }
        }
    });
    return dup;
}

function LinkChannelValidation(channels, linkType, validationId) {
    var res;
    $("#" + validationId).text("");
    if (linkType === "ISUP" && channels % 31 > 0) {
        $("#" + validationId).text("Number of channels for ISUP links must be N*31");
        return false;
    }
    else
        return true;
}
