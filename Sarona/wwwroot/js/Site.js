
$.validator.addMethod('minmax',
    function (value, element, params) {
        // Get element value. Classic genre has value '0'.
        var genre = $(params[0]).val(),
            year = params[1],
            date = new Date(value);
        if (genre && genre.length > 0 && genre[0] === '0') {
            // Since this is a classic movie, invalid if release date is after given year.
            return date.getFullYear() <= year;
        }

        return true;
    });

$.validator.unobtrusive.adapters.add('classicmovie',
    ['year'],
    function (options) {
        var element = $(options.form).find('select#Genre')[0];
        options.rules['classicmovie'] = [element, parseInt(options.params['year'])];
        options.messages['classicmovie'] = options.message;
    });




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
    $.getJSON("/api/ne/abb/" + exchangeAbb + "/" + type, function (result) {
        $.each(result, function () {
            $("#" + selectId).append($("<option />").val(this.id).text(this.name + " (" + this.model + " - " + this.manufacturer + ")"));
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



function AbbValidation(newAbb, oldAbb, validationId, onlyExchanges = false) {
    var dup;
    newAbb = newAbb.toUpperCase();
    oldAbb = oldAbb.toUpperCase();
    $("#" + validationId).text("");
    if (newAbb === oldAbb)
        return true;
    $.get({
        async: false,
        url: '/Network/AbbValidation',
        data: { abb: newAbb, onlyExchanges },
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

function PrefixValidation(prefix, validationId, oldPrefix = '') {
    var res;
    $("#" + validationId).text("");
    if (prefix === '')
        return false;
    $.ajax({
        async: false,
        url: '/Numbering/PrefixValidation',
        data: { prefix, oldPrefix },
        success: function (result) {
            if (result === 0) {
                $("#" + validationId).text("");
                res = true;
            }
            else if (result > 1) {
                $("#" + validationId).text("There are " + result + " overlaps.");
                res = false;
            }

            else if (result === 1) {
                $("#" + validationId).text("There is one overlap.");
                res = false;
            }
        },
        error: function () {
            alert("Cannot get prefix validation.");
        }
    });
    return res;


}

function NeNameValidation(neName, oldName, validationId) {
    var dup;
    neName = neName.toUpperCase();
    oldName = oldName.toUpperCase();
    $("#" + validationId).text("");
    if (neName === oldName)
        return true;
    $.ajax({
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
    $("#" + validationId).text("");
    if (linkType === "ISUP" && channels % 31 > 0) {
        $("#" + validationId).text("Number of channels for ISUP links must be N*31");
        return false;
    }
    else
        return true;
}

function Stm1E1Representation(channels, id) {
    var p = $("#" + id);
    p.text("");
    var stm1 = Math.floor(channels / 1953);
    var e1 = Math.floor((channels - stm1 * 1953) / 31);
    var ch = channels - stm1 * 1953 - e1 * 31;
    p.text("STM1=" + stm1 + " E1=" + e1 + " Channels=" + ch);
}

function ChangePassword() {
    var oldPass = $('#currentPassword').val();
    var newPass = $('#newPassword').val();
    $.post('/account/changepassword',
        { oldPass, newPass },
        function (result) {
            if (result === true) {
                alert('Password changed successfully.');
                $('#changePasswordModal').modal("hide");
            }
            else {
                $('#passwordErrors').text(result);
            }
        });
}

function gregorian_to_jalali(gy, gm, gd) {
    g_d_m = [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334];
    if (gy > 1600) {
        jy = 979;
        gy -= 1600;
    } else {
        jy = 0;
        gy -= 621;
    }
    gy2 = (gm > 2) ? (gy + 1) : gy;
    days = (365 * gy) + (parseInt((gy2 + 3) / 4)) - (parseInt((gy2 + 99) / 100)) + (parseInt((gy2 + 399) / 400)) - 80 + gd + g_d_m[gm - 1];
    jy += 33 * (parseInt(days / 12053));
    days %= 12053;
    jy += 4 * (parseInt(days / 1461));
    days %= 1461;
    if (days > 365) {
        jy += parseInt((days - 1) / 365);
        days = (days - 1) % 365;
    }
    jm = (days < 186) ? 1 + parseInt(days / 31) : 7 + parseInt((days - 186) / 30);
    jd = 1 + ((days < 186) ? (days % 31) : ((days - 186) % 30));
    return [jy, jm, jd];
}


function jalali_to_gregorian(jy, jm, jd) {
    if (jy > 979) {
        gy = 1600;
        jy -= 979;
    } else {
        gy = 621;
    }
    days = (365 * jy) + ((parseInt(jy / 33)) * 8) + (parseInt(((jy % 33) + 3) / 4)) + 78 + jd + ((jm < 7) ? (jm - 1) * 31 : ((jm - 7) * 30) + 186);
    gy += 400 * (parseInt(days / 146097));
    days %= 146097;
    if (days > 36524) {
        gy += 100 * (parseInt(--days / 36524));
        days %= 36524;
        if (days >= 365) days++;
    }
    gy += 4 * (parseInt(days / 1461));
    days %= 1461;
    if (days > 365) {
        gy += parseInt((days - 1) / 365);
        days = (days - 1) % 365;
    }
    gd = days + 1;
    sal_a = [0, 31, ((gy % 4 === 0 && gy % 100 !== 0) || (gy % 400 === 0)) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    for (gm = 0; gm < 13; gm++) {
        v = sal_a[gm];
        if (gd <= v) break;
        gd -= v;
    }
    return [gy, gm, gd];
}
