var selectedPrefixes = [];
var oldPrefix;
function SearchSubscriber() {
    var search = $("#searchSubscriber").val();
    $.get({
        url:
            "/numbering/GetSubscribers",
        data:
            { name: search },
        success:
            function (result) {
                $("#subscriberResults").text('');
                $("#resultBadge").text(result.length);
                $.each(result, function () {
                    $("#subscriberResults").append($("<option />").val(this.id).text(this.subscriberName + " (" + this.abb + ")"));
                });
            },
        error:
            function () {
                alert("Cannot get subscriber names.");
            }
    });
}


function ShowAttachModal(prefix, multiple = false) {
    if (multiple) {
        if (selectedPrefixes.length === 0) {
            alert("Please select at least one prefix.");
            return;
        }
        $("#attachPrefix").val(selectedPrefixes.join(','));
    }
    else {
        $("#attachPrefix").val(prefix);
    }
    $("#attachPrefixModal").modal();
}
function SelectAllPrefixes(checkbox) {
    $(".my-check").each(function () {
        this.checked = checkbox.checked;
        SelectPrefix(this, $(this).val());

    });
}

function SelectPrefix(checkbox) {
    var prefix = $(checkbox).val();
    if (checkbox.checked) {
        if (selectedPrefixes.indexOf(prefix) === -1) {
            selectedPrefixes.push(prefix);
        }
    }

    else {
        var index = selectedPrefixes.indexOf(prefix);
        if (index > -1) {
            selectedPrefixes.splice(index, 1);
        }
    }
}
function OpenReserveCodeModal() {
    if (selectedPrefixes.length === 0)
        alert("Please select at least one prefix.");
    else {
        $("#reserveMessage").text("You have selected " + selectedPrefixes.length + " prefixes.");
        $("#reservePrefixModal").modal();
    }

}
function ReserveCodes() {
    $.post(
        "/numbering/reservecodes",
        {
            prefixes: selectedPrefixes,
            area: $("#reservedArea option:selected").val(),
            link: $("#reservedLink option:selected").val(),
            min: $("#reservedMin").val(),
            max: $("#reservedMax").val(),
            changeMinMax: document.getElementById("reservedChangeMinMax").checked
        },
        function () { location.reload(); });

}
function LoadPage(select, prefix) {
    var selectedPage = $(select).find(":selected").text();
    location.assign("/numbering/pool?prefix=" + prefix + "&page=" + selectedPage);
}

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    var objCal1 = new AMIB.persianCalendar('pcal1', {
        extraInputID: "NewPrefix_ExpireDate",
        extraInputFormat: "YYYY/MM/DD",
        initialDate: "1500/01/01",
        btnClassName: "fa fa-calendar-alt float-right"
    });

    var objCal2 = new AMIB.persianCalendar('pcal2', {
        extraInputID: "EditPrefix_ExpireDate",
        extraInputFormat: "MM-DD-YYYY",
        btnClassName: "fa fa-calendar-alt float-right"
    });
});


function ShowDeletePrefix(prefix, multiple = false) {
    if (multiple) {
        if (selectedPrefixes.length === 0) {
            alert("Please select at least one prefix.");
            return;
        }
        $("#deletePrefixCode").val(selectedPrefixes.join(','));
    }
    else {
        $("#deletePrefixCode").val(prefix);
    }
    $("#deletePrefixSpan").text(prefix);
    $('#deletePrefixModal').modal();

}

function ShowRemoveModal(prefix, multiple = false) {
    $("#removePrefixTitle").text(prefix);
    if (multiple) {
        if (selectedPrefixes.length === 0) {
            alert("Please select at least one prefix.");
            return;
        }
        $("#removePrefix").val(selectedPrefixes.join(','));
    }
    else {
        $("#removePrefix").val(prefix);
    }
    
    $("#removePrefixModal").modal();
}


function ShowAssignModal(prefix, link, multiple = false) {

    if (link === "") {
        $("#linkType").val(2);
        $("#linkTypeAvailableCustomer").val(2);
        $("#linkTypeNewCustomer").val(2);
    }
    else if (link === 'PRA') {
        $("#linkType").val(2);
        $("#linkTypeAvailableCustomer").val(2);
        $("#linkTypeNewCustomer").val(2);
    }
    else if (link === 'SIP') {
        $("#linkType").val(1);
        $("#linkTypeAvailableCustomer").val(1);
        $("#linkTypeNewCustomer").val(1);
    }
    if (multiple) {
        if (selectedPrefixes.length === 0) {
            alert("Please select at least one prefix.");
            return;
        }
        $("#assignPrefixTitle").text(prefix);
        $("#prefixWithAssign").val(selectedPrefixes.join(','));
        $("#prefixWithAddingCustomer").val(selectedPrefixes.join(','));
    }
    else {
        $("#assignPrefixTitle").text(prefix);
        $("#prefixWithAssign").val(prefix);
        $("#prefixWithAddingCustomer").val(prefix);
    }

    $("#assignPrefixModal").modal();
}

function CheckSelectedCustomer() {
    var selected = $("#subscriberResults option:selected");
    if (selected.length === 0) {
        $("#selectedCustomerError").text("Please select a customer.");
        return false;
    }
    return true;

}

function ShowChangePrefix(prefix) {
    $("#changePrefixTitle").text(prefix);
    $("#changePrefixModal").modal();
}

function ShowEditPrefix(id) {
    $.get("/numbering/prefix/" + id,
        function (result) {
            $("#EditPrefix_Id").val(result.id);
            $("#EditPrefix_Min").val(result.min);
            $("#EditPrefix_Max").val(result.max);
            $("#EditPrefix_ChargingCase").val(result.chargingCase);
            $("#EditPrefix_Remark").val(result.remark);
            $("#EditPrefix_Owner").val(result.owner);
            $("#EditPrefix_NumberType").val(result.numberType);
            $("#EditPrefix_RoutingType").val(result.routingType);
            $("#EditPrefix_ExpireDate").val(result.expireDate);
            $("#editPrefixModalTitle").text("Edit Prefix (" + result.prefix + ")");
            $("#EditPrefix_Prefix").val(result.prefix);
            oldPrefix = result.prefix;
            $("#EditPrefix_Abb").val(result.abb);
            $("#EditPrefix_Link").val(result.link);
            $("#EditPrefix_Area").val(result.area);
            $("#EditPrefix_SecondaryMin").val(result.secondaryMin);
            $("#EditPrefix_SecondaryMax").val(result.secondaryMax);
            $("#EditPrefix_SecondaryArea").val(result.secondaryArea);
            $("#EditPrefix_SubscriberName").val(result.subscriberName);
            $("#EditPrefix_Direction").val(result.direction);

            if (result.isFloat === true)
                $("#EditPrefix_IsFloat").prop("checked", true);
            if (result.isKeshvari === true)
                $("#EditPrefix_IsKeshvari").prop("checked", true);
            var date = new Date(result.expireDate);
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            var pDate = gregorian_to_jalali(y, m, d);
            $("#pcal2").val(pDate.join("/"));
            $("#editPrefixModal").modal();
        });
}