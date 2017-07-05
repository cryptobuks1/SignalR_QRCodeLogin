// noty settings
var notify_default = {
    theme: 'relax',
    dismissQueue: true,
    layout: 'bottomRight',
    maxVisible: 15,
    timeout: 5000
};

function addNotify(level, text) {
    var settings = { 'text': text, 'type': level };

    $.extend(settings, notify_default);

    noty(settings);
}

// date format string: http://fanli7.net/a/bianchengyuyan/JS-HTML-WEB/20101028/58473.html
Number.prototype.pad2 = function () {
    return this > 9 ? this : '0' + this;
}
Date.prototype.format = function (format) {
    var it = new Date();
    var it = this;
    var M = it.getMonth() + 1, H = it.getHours(), m = it.getMinutes(), d = it.getDate(), s = it.getSeconds();
    var n = {
        'yyyy': it.getFullYear()
            , 'MM': M.pad2(), 'M': M
            , 'dd': d.pad2(), 'd': d
            , 'HH': H.pad2(), 'H': H
            , 'mm': m.pad2(), 'm': m
            , 'ss': s.pad2(), 's': s
    };
    return format.replace(/([a-zA-Z]+)/g, function (s, $1) { return n[$1]; });
}