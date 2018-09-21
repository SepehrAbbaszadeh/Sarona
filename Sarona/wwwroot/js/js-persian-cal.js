/*
 JS Persian Calendar

 @version 0.2.1.1
 @created on 2013 July 18 - 1392 Tir 27
 @licence GPL

 @author Amir Masoud Irani( AMIB )
 amibct@gmail.com
 http://amib.ir/weblog
*/
if ("undefined" === typeof AMIB) var AMIB = {};
var lastOpenPDatePicker = !1,
    lastOpenPDateMonthYearID = !1,
    pDatePickerZIndex = 9999,
    PERSIAN_MONTH_NAMES = "\u0641\u0631\u0648\u0631\u062f\u06cc\u0646 \u0627\u0631\u062f\u0628\u06cc\u0647\u0634\u062a \u062e\u0631\u062f\u0627\u062f \u062a\u06cc\u0631 \u0645\u0631\u062f\u0627\u062f \u0634\u0647\u0631\u06cc\u0648\u0631 \u0645\u0647\u0631 \u0622\u0628\u0627\u0646 \u0622\u0630\u0631 \u062f\u06cc \u0628\u0647\u0645\u0646 \u0627\u0633\u0641\u0646\u062f".split(" "),
    PERSIAN_SHORT_WEEKDAY_NAMES = "\u0634 \u06cc\u06a9 \u062f\u0648 \u0633\u0647 \u0686\u0647 \u067e\u0646 \u062c\u0645".split(" ");
if ("function" !== typeof fdef) var fdef = function (b) {
    return "function" === typeof b
};
if (!fdef(A$)) var A$ = function (b) {
    return document.getElementById(b)
};
if (!fdef(A$$)) var A$$ = function (b, a) {
    return b.getElementsByTagName(a)
};
if (!fdef(cE)) var cE = function (b) {
    return document.createElement(b)
};
if (!fdef(cTN)) var cTN = function (b) {
    return document.createTextNode(b)
};
AMIB.persianCalendar = function (b, a) {
    var c = {
        inputID: "",
        extraInputID: "",
        extraInputFormat: "JD",
        initialDate: "",
        defaultDate: "",
        autoCreateButton: !0,
        divPickerClassName: "picker",
        btnClassName: "pcalBtn",
        onchange: !1
    },
        d;
    this.getOption = function (a) {
        return c[a];
    };
    this.oldDocumentOnClick = !1;
    this.enabled = !0;
    this.inputID = b;
    this.input = A$(b);
    for (d in a) d in c ? c[d] = a[d] : alert(d.toString() + " option is not supported");
    c.defaultDate instanceof Array || (c.defaultDate = isPersianDate(c.defaultDate));
    d = this.input;
    if (c.autoCreateButton) {
        var e = cE("a");
        e.className = c.btnClassName;
        e.href = "#";
        e.id = b + "_btn"; console.log("before clicked");
        d.parentNode.insertBefore(e, d.nextSibling);
        var g = this;
        e.onclick = function (a) {
            g.showHidePicker(a);
            var pos = $("#" + e.id).offset();
            console.log("clicked");
            $("#" + b + "_pDatePicker").attr("style", "left: " + pos.left + "px; top: " + pos.top + "px; z-index: 9999;");
            return !1;
        };
    }
    var f = this;
    d.ondblclick = function (a) {
        f.showHidePicker(a);
        return !1
    };
    var h = this;
    d.onblur = function () {
        h.normalizeUserInput()
    };
    var k = this;
    addEvt(d, "keydown", function (a) {
        k.keyDownHandler(a, k)
    });
    c.initialDate && this.fillDateField(c.initialDate)
};
AMIB.persianCalendar.prototype.showHidePicker = function (b) {
    var a = this.inputID + "_pDatePicker",
        c;
    if (c = A$(a)) {
        if (c.parentNode.removeChild(c), document.onclick = this.oldDocumentOnClick, lastOpenPDatePicker = this.oldDocumentOnClick = !1, b = A$(lastOpenPDateMonthYearID)) b.parentNode.removeChild(b), lastOpenPDateMonthYearID = !1
    } else if (!0 === this.enabled) {
        var d = this.input;
        c = cE("div");
        c.id = a;
        c.className = this.getOption("divPickerClassName");
        this.createDateTable(c, d.value, d.value);
        document.body.appendChild(c);
        this.setMenuPos(c,
            d);
        c.style.zIndex = !lastOpenPDatePicker ? pDatePickerZIndex = 9999 : ++pDatePickerZIndex;
        "undefined" === typeof b && (b = window.event);
        lastOpenPDatePicker && lastOpenPDatePicker.showHidePicker(b);
        lastOpenPDatePicker = this;
        stopPropg(b);
        this.oldDocumentOnClick = document.onclick;
        var e = this,
            g = b;
        document.onclick = function () {
            e.showHidePicker(g)
        }
    }
};
AMIB.persianCalendar.prototype.createDateTable = function (b, a, c) {
    var d = cE("table"),
        e, g, f, h, k, j;
    g = this.getOption("defaultDate");
    j = new Date;
    f = gregorian_to_jd(j.getFullYear(), j.getMonth() + 1, j.getDate());
    j = jd_to_persian(f);
    (a = isPersianDate(a)) ? a = persian_to_jd(a[0], a[1], a[2]) : g ? (a = persian_to_jd(g[0], g[1], g[2]), c = g.join("/")) : a = f;
    c = isPersianDate(c);
    a = jd_to_persian(a);
    var l = Math.floor(persian_to_jd(a[0], a[1], 1) + 3) % 7;
    e = d.insertRow(-1);
    g = e.insertCell(-1);
    g.className = "pickerHead";
    g.colSpan = 7;
    e = cE("div");
    e.className =
        "navBack";
    f = cE("a");
    f.appendChild(cTN("\u00ab"));
    f.title = "\u0633\u0627\u0644 \u0642\u0628\u0644";
    f.className = "nav";
    f.href = "#";
    var m = this,
        n = a[0] - 1,
        p = a[1];
    f.onclick = function (a) {
        m.changePickerDate(a, n, p);
        return !1
    };
    e.appendChild(f);
    f = cE("a");
    f.appendChild(cTN("<"));
    f.title = PERSIAN_MONTH_NAMES[1 == a[1] ? 11 : a[1] - 2];
    f.className = "nav";
    f.href = "#";
    var q = this,
        r = 1 == a[1] ? a[0] - 1 : a[0],
        s = 1 == a[1] ? 12 : a[1] - 1;
    f.onclick = function (a) {
        q.changePickerDate(a, r, s);
        return !1
    };
    e.appendChild(f);
    g.appendChild(e);
    e = cE("div");
    e.className =
        "navFwd";
    f = cE("a");
    f.appendChild(cTN("\u00bb"));
    f.title = "\u0633\u0627\u0644 \u0628\u0639\u062f";
    f.className = "nav";
    f.href = "#";
    var t = this,
        u = a[0] + 1,
        v = a[1];
    f.onclick = function (a) {
        t.changePickerDate(a, u, v);
        return !1
    };
    e.appendChild(f);
    f = cE("a");
    f.appendChild(cTN(">"));
    f.title = PERSIAN_MONTH_NAMES[12 == a[1] ? 0 : a[1]];
    f.className = "nav";
    f.href = "#";
    var w = this,
        x = 12 == a[1] ? a[0] + 1 : a[0],
        y = 12 == a[1] ? 1 : a[1] + 1;
    f.onclick = function (a) {
        w.changePickerDate(a, x, y);
        return !1
    };
    e.appendChild(f);
    g.appendChild(e);
    f = cE("a");
    f.className =
        "monYear";
    f.appendChild(cTN(PERSIAN_MONTH_NAMES[a[1] - 1]));
    f.href = "#";
    var z = this,
        A = a[1],
        B = a[0];
    f.onclick = function (a) {
        z.showMonthPicker(a, A, B);
        return !1
    };
    g.appendChild(f);
    f = cE("a");
    f.className = "monYear";
    f.appendChild(cTN(enDigitsToFa(a[0])));
    f.href = "#";
    var C = this,
        E = a[1],
        F = a[0];
    f.onclick = function (a) {
        C.showYearPicker(a, E, F);
        return !1
    };
    g.appendChild(f);
    e = d.insertRow(-1);
    h = 0;
    for (k = PERSIAN_SHORT_WEEKDAY_NAMES.length; h < k; ++h) g = e.insertCell(-1), g.appendChild(cTN(PERSIAN_SHORT_WEEKDAY_NAMES[h])), g.className =
        "calWeekdays";
    e = d.insertRow(-1);
    for (h = 0; h < l; ++h) g = e.insertCell(-1);
    h = l;
    for (k = persianMonthDays(a[0], a[1]) + l; h < k; ++h) 0 == h % 7 && (e = d.insertRow(-1)), g = e.insertCell(-1), f = cE("a"), f.href = "#", a[2] = h - l + 1, f.className = "weekday", 6 == h % 7 && (f.className += " friday"), c && (a[2] == c[2] && a[1] == c[1] && a[0] == c[0]) && (f.className += " selected"), a[2] == j[2] && (a[1] == j[1] && a[0] == j[0]) && (f.className += " today", f.title += "\u0627\u0645\u0631\u0648\u0632"), f.onclick = function (a, b) {
        return function () {
            a.fillDateField(b);
            return !1
        }
    }(this, a.join("/")),
        f.appendChild(cTN(enDigitsToFa(a[2]))), g.appendChild(f);
    h = 0;
    for (k = 7 - (persianMonthDays(a[0], a[1]) + l) % 7; 7 != k && h < k; ++h) g = e.insertCell(-1);
    e = d.insertRow(-1);
    g = e.insertCell(-1);
    g.className = "pickerFoot";
    g.colSpan = 7;
    f = cE("a");
    f.appendChild(cTN(enDigitsToFa("\u0627\u0645\u0631\u0648\u0632: " + j[2].toString() + " " + PERSIAN_MONTH_NAMES[j[1] - 1].toString() + " " + j[0].toString())));
    f.href = "#";
    a[2] = h - l + 1;
    c = f;
    var G = this,
        H = j.join("/");
    c.onclick = function () {
        G.fillDateField(H);
        return !1
    };
    for (g.appendChild(f); b.firstChild;) b.removeChild(b.firstChild);
    b.appendChild(d)
};
AMIB.persianCalendar.prototype.changePickerDate = function (b, a, c) {
    var d;
    if (d = A$(this.inputID + "_pDatePicker")) b || (b = window.event), stopPropg(b), b = a.toString() + "/" + c.toString() + "/1", this.createDateTable(d, b, this.input.value)
};
AMIB.persianCalendar.prototype.showMonthPicker = function (b, a, c) {
    var d, e, g;
    if (A$(this.inputID + "_pDatePicker")) {
        b || (b = window.event);
        (e = A$(lastOpenPDateMonthYearID)) && e.parentNode.removeChild(e);
        stopPropg(b);
        e = cE("div");
        e.className = "monthYearPicker";
        e.id = lastOpenPDateMonthYearID = this.inputID + "monthYearPicker";
        for (d = 0; 12 > d; ++d) g = cE("a"), g.href = "#", g.appendChild(cTN(PERSIAN_MONTH_NAMES[d])), d == a - 1 && (g.className = "selected"), g.onclick = function (a, b, c) {
            return function (d) {
                a.monthYearPickerClick(d, b, c);
                return !1
            }
        }(this,
            d + 1, c), e.appendChild(g);
        document.body.appendChild(e);
        this.setMenuPos(e, b.target || b.srcElement);
        e.style.zIndex = ++pDatePickerZIndex
    }
};
AMIB.persianCalendar.prototype.showYearPicker = function (b, a, c) {
    var d, e, g;
    if (A$(this.inputID + "_pDatePicker")) {
        b || (b = window.event);
        (e = A$(lastOpenPDateMonthYearID)) && e.parentNode.removeChild(e);
        stopPropg(b);
        e = cE("div");
        e.className = "monthYearPicker";
        e.id = lastOpenPDateMonthYearID = this.inputID + "monthYearPicker";
        for (d = c - 5; d <= c + 5; ++d) g = cE("a"), g.href = "#", g.appendChild(cTN(enDigitsToFa(d))), d == c && (g.className = "selected"), g.onclick = function (a, b, c) {
            return function (d) {
                a.monthYearPickerClick(d, b, c);
                return !1
            }
        }(this,
            a, d), e.appendChild(g);
        document.body.appendChild(e);
        this.setMenuPos(e, b.target || b.srcElement);
        e.style.zIndex = ++pDatePickerZIndex
    }
};
AMIB.persianCalendar.prototype.setMenuPos = function (b, a) {
    var c = getOffset(a);
    b.style.top = 0 > c.top - b.offsetHeight || getScrollTop() + getViewPortHeight() > c.top + a.offsetHeight + b.offsetHeight ? c.top + a.offsetHeight + "px" : c.top - b.offsetHeight + "px";
    c = getOffset(a);
    b.style.left = c.left + a.offsetWidth - b.offsetWidth + "px"
};
AMIB.persianCalendar.prototype.monthYearPickerClick = function (b, a, c) {
    var d, e;
    if (d = A$(this.inputID + "_pDatePicker")) b || (b = window.event), stopPropg(b), (e = A$(lastOpenPDateMonthYearID)) && e.parentNode.removeChild(e), lastOpenPDateMonthYearID = !1, b = c.toString() + "/" + a.toString() + "/1", this.createDateTable(d, b, this.input.value)
};
AMIB.persianCalendar.prototype.normalizeUserInput = function () {
    var b = this.input,
        a, c = b.value.trim(),
        c = faDigitsToEn(c);
    (a = isPersianDate(c)) ? this.fillDateField(a) : (a = removeCssClass(b.className, "valid"), a = removeCssClass(a, "invalid"), "" !== c && (a += " invalid"), b.className = a, this.getOption("extraInputID").length && (A$(this.getOption("extraInputID")).value = ""), fdef(this.getOption("onchange")) && this.getOption("onchange")(!1))
};
AMIB.persianCalendar.prototype.fillDateField = function (b) {
    var a = this.input,
        c;
    b = b instanceof Array ? b : isPersianDate(b);
    a.value = b.join("/");
    c = removeCssClass(a.className, "invalid");
    c = removeCssClass(c, "valid");
    a.className = c + " valid";
    this.getOption("extraInputID").length && this.getOption("extraInputFormat").length && (a = this.getOption("extraInputFormat"), a = a.replace(/yyyy/g, zeroPad(b[0], 4)), a = a.replace(/mm/g, zeroPad(b[1], 2)), a = a.replace(/dd/g, zeroPad(b[2], 2)), a = a.replace(/yy/g, zeroPad(b[0] % 100, 2)), a = a.replace(/m/g,
        b[1].toString()), a = a.replace(/d/g, b[2].toString()), c = persian_to_jd(b[0], b[1], b[2]), a = a.replace(/JD/g, c), c = jd_to_gregorian(c), a = a.replace(/YYYY/g, zeroPad(c[0], 4)), a = a.replace(/MM/g, zeroPad(c[1], 2)), a = a.replace(/DD/g, zeroPad(c[2], 2)), a = a.replace(/YY/g, zeroPad(c[0] % 100, 2)), a = a.replace(/M/g, c[1].toString()), a = a.replace(/D/g, c[2].toString()), A$(this.getOption("extraInputID")).value = a);
    fdef(this.getOption("onchange")) && this.getOption("onchange")(b.slice(0))
};
AMIB.persianCalendar.prototype.keyDownHandler = function (b, a) {
    var c;
    b || (b = window.event);
    if (b.keyCode) c = b.keyCode;
    else if (b.which) c = b.which;
    else return !1;
    if (!(48 <= c && 57 >= c || 96 <= c && 105 >= c || 191 === c || 111 === c || 8 === c || 9 === c || 13 === c || 46 === c || 109 === c || 35 === c || 36 === c || 37 === c || 39 === c))
        if (38 === c || 40 === c) {
            var d = isPersianDate(a.input.value);
            !1 !== d && (38 === c ? a.fillDateField(jd_to_persian(persian_to_jd(d[0], d[1], d[2]) + 1)) : 40 === c && a.fillDateField(jd_to_persian(persian_to_jd(d[0], d[1], d[2]) - 1)), a.input.select(), prvDef(b))
        } else b.ctrlKey ||
            prvDef(b)
};

function isPersianDate(b) {
    var a;
    if (0 == b.search(/^\d+\-\d+\-\d+$/)) a = b.split("-", 3);
    else if (0 == b.search(/^\d+\/\d+\/\d+$/)) a = b.split("/", 3);
    else if (0 == b.search(/^\d{6}|\d{8}$/)) a = [b.substr(0, b.length - 4), b.substr(b.length - 4, 2), b.substr(b.length - 2, 2)];
    else if ((a = b.match(/^(\d{2})(\d{2})$/)) || (a = b.match(/^(\d{1,2})[/\-](\d{1,2})$/)) || (a = b.match(/^(\d{1,2})$/))) b = new Date, b = gregorian_to_jd(b.getFullYear(), b.getMonth() + 1, b.getDate()), b = jd_to_persian(b), a[0] = b[0], "undefined" == typeof a[2] && (a[2] = b[1]), 12 >=
        parseInt(a[2], 10) && (b = a[1], a[1] = a[2], a[2] = b);
    else return !1;
    for (b = 0; 2 >= b; ++b) a[b] = parseInt(a[b], 10);
    if (12 < a[1] || 0 >= a[1]) return !1;
    a[2] > persianMonthDays(longYear(a[0]), a[1]) && (b = a[2], a[2] = a[0], a[0] = b);
    if (9999 < a[0]) return !1;
    100 > a[0] && (a[0] = longYear(a[0]));
    return 0 === a[2] || a[2] > persianMonthDays(a[0], a[1]) ? !1 : a
}

function longYear(b) {
    var a, c;
    if (100 <= b) return b;
    a = new Date;
    a = gregorian_to_jd(a.getFullYear(), a.getMonth() + 1, a.getDate());
    a = jd_to_persian(a);
    c = 100 * Math.floor(a[0] / 100);
    70 < Math.abs(a[0] - c - b) && (c += 100);
    return c + b
}
var GREGORIAN_EPOCH = 1721425.5,
    PERSIAN_EPOCH = 1948320.5;
if (!fdef(mod)) var mod = function (b, a) {
    return b - a * Math.floor(b / a)
};

function leap_persian(b) {
    return 682 > 682 * ((b - (0 < b ? 474 : 473)) % 2820 + 512) % 2816
}

function jd_to_persian(b) {
    var a, c, d;
    b = Math.floor(b) + 0.5;
    c = b - 2121445.5;
    a = Math.floor(c / 1029983);
    d = mod(c, 1029983);
    1029982 == d ? c = 2820 : (c = Math.floor(d / 366), d = mod(d, 366), c = Math.floor((2134 * c + 2816 * d + 2815) / 1028522) + c + 1);
    a = c + 2820 * a + 474;
    0 >= a && a--;
    c = b - persian_to_jd(a, 1, 1) + 1;
    c = 186 >= c ? Math.ceil(c / 31) : Math.ceil((c - 6) / 30);
    b = b - persian_to_jd(a, c, 1) + 1;
    return [a, c, b]
}

function persian_to_jd(b, a, c) {
    var d;
    b -= 0 <= b ? 474 : 473;
    d = 474 + mod(b, 2820);
    return c + (7 >= a ? 31 * (a - 1) : 30 * (a - 1) + 6) + Math.floor((682 * d - 110) / 2816) + 365 * (d - 1) + 1029983 * Math.floor(b / 2820) + (PERSIAN_EPOCH - 1)
}

function leap_gregorian(b) {
    return 0 == b % 4 && !(0 == b % 100 && 0 != b % 400)
}

function jd_to_gregorian(b) {
    var a, c, d, e;
    b = Math.floor(b - 0.5) + 0.5;
    a = b - GREGORIAN_EPOCH;
    c = Math.floor(a / 146097);
    d = mod(a, 146097);
    a = Math.floor(d / 36524);
    e = mod(d, 36524);
    d = Math.floor(e / 1461);
    e = mod(e, 1461);
    e = Math.floor(e / 365);
    c = 400 * c + 100 * a + 4 * d + e;
    4 == a || 4 == e || c++;
    a = b - gregorian_to_jd(c, 1, 1);
    d = b < gregorian_to_jd(c, 3, 1) ? 0 : leap_gregorian(c) ? 1 : 2;
    a = Math.floor((12 * (a + d) + 373) / 367);
    b = b - gregorian_to_jd(c, a, 1) + 1;
    return [c, a, b]
}

function gregorian_to_jd(b, a, c) {
    return GREGORIAN_EPOCH - 1 + 365 * (b - 1) + Math.floor((b - 1) / 4) + -Math.floor((b - 1) / 100) + Math.floor((b - 1) / 400) + Math.floor((367 * a - 362) / 12 + (2 >= a ? 0 : leap_gregorian(b) ? -1 : -2) + c)
}

function persianMonthDays(b, a) {
    return 6 >= a ? 31 : 11 >= a ? 30 : 12 == a ? leap_persian(b) ? 30 : 29 : 0
}
if (!fdef(stopPropg)) var stopPropg = function (b) {
    b.stopPropagation ? b.stopPropagation() : b.cancelBubble = !0
};
if (!fdef(prvDef)) var prvDef = function (b) {
    b.preventDefault ? b.preventDefault() : b.returnValue = !1
};
if (!fdef(faDigitsToEn)) var faDigitsToEn = function (b) {
    for (var a = b.length, c = "", d = 0, d = 0; d < a; ++d) c += String.fromCharCode(1776 <= b.charCodeAt(d) && 1785 >= b.charCodeAt(d) ? b.charCodeAt(d) - 1728 : b.charCodeAt(d));
    return c
};
if (!fdef(enDigitsToFa)) var enDigitsToFa = function (b) {
    b = b.toString();
    for (var a = b.length, c = "", d = 0, d = 0; d < a; ++d) c += String.fromCharCode(48 <= b.charCodeAt(d) && 57 >= b.charCodeAt(d) ? b.charCodeAt(d) + 1728 : b.charCodeAt(d));
    return c
};
"trim" in String.prototype || (String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "")
});
"map" in Array.prototype || (Array.prototype.map = function (b, a) {
    for (var c = Array(this.length), d = 0, e = this.length; d < e; d++) d in this && (c[d] = b.call(a, this[d], d, this));
    return c
});
if (!fdef(removeCssClass)) var removeCssClass = function (b, a) {
    return b.split(/\s+/).map(function (b) {
        return b === a ? "" : b
    }).join(" ")
};
if (!fdef(getOffset)) var getOffset = function (b) {
    for (var a = 0, c = 0; b && !isNaN(b.offsetLeft) && !isNaN(b.offsetTop);) a += b.offsetLeft, c += b.offsetTop, b = b.offsetParent;
    return {
        top: c,
        left: a
    }
};
if (!fdef(zeroPad)) var zeroPad = function (b, a) {
    var c = Math.abs(b),
        d = Math.max(0, a - Math.floor(c).toString().length),
        d = Math.pow(10, d).toString().substr(1);
    0 > b && (d = "-" + d);
    return d + c
};
if (!fdef(getViewPortHeight)) var getViewPortHeight = function () {
    return "undefined" != typeof window.innerWidth ? window.innerHeight : document.documentElement && document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight
};
if (!fdef(getScrollTop)) var getScrollTop = function (b) {
    var a = 0;
    if ("undefined" !== typeof b) {
        for (;
            (b = b.parentNode) && "BODY" !== b.nodeName && "undefined" !== typeof b.scrollTop;) a += b.scrollTop;
        return a
    }
    if ("undefined" !== typeof pageYOffset) return pageYOffset;
    b = document.body;
    D = document.documentElement;
    D = D.clientHeight ? D : b;
    return D.scrollTop
};
if (!fdef(addEvt)) var addEvt = function (b, a, c) {
    b.addEventListener ? b.addEventListener(a, c, !1) : b.attachEvent && b.attachEvent("on" + a, c)
};