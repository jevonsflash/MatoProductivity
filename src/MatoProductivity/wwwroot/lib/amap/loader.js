'use strict'; (function (m, p) { "object" === typeof exports && "undefined" !== typeof module ? module.exports = p() : "function" === typeof define && define.amd ? define(p) : (m = m || self, m.AMapLoader = p()) })(this, function () {
    function m(a) { var b = []; a.AMapUI && b.push(p(a.AMapUI)); a.Loca && b.push(r(a.Loca)); return Promise.all(b) } function p(a) {
        return new Promise(function (h, c) {
            var f = []; if (a.plugins) for (var e = 0; e < a.plugins.length; e += 1)-1 == d.AMapUI.plugins.indexOf(a.plugins[e]) && f.push(a.plugins[e]); if (g.AMapUI === b.failed) c("\u524d\u6b21\u8bf7\u6c42 AMapUI \u5931\u8d25");
            else if (g.AMapUI === b.notload) {
                g.AMapUI = b.loading; d.AMapUI.version = a.version || d.AMapUI.version; e = d.AMapUI.version; var l = document.body || document.head, k = document.createElement("script"); k.type = "text/javascript"; k.src = "https://webapi.amap.com/ui/" + e + "/main.js"; k.onerror = function (a) { g.AMapUI = b.failed; c("\u8bf7\u6c42 AMapUI \u5931\u8d25") }; k.onload = function () {
                    g.AMapUI = b.loaded; if (f.length) window.AMapUI.loadUI(f, function () {
                        for (var a = 0, b = f.length; a < b; a++) {
                            var c = f[a].split("/").slice(-1)[0]; window.AMapUI[c] =
                                arguments[a]
                        } for (h(); n.AMapUI.length;)n.AMapUI.splice(0, 1)[0]()
                    }); else for (h(); n.AMapUI.length;)n.AMapUI.splice(0, 1)[0]()
                }; l.appendChild(k)
            } else g.AMapUI === b.loaded ? a.version && a.version !== d.AMapUI.version ? c("\u4e0d\u5141\u8bb8\u591a\u4e2a\u7248\u672c AMapUI \u6df7\u7528") : f.length ? window.AMapUI.loadUI(f, function () { for (var a = 0, b = f.length; a < b; a++) { var c = f[a].split("/").slice(-1)[0]; window.AMapUI[c] = arguments[a] } h() }) : h() : a.version && a.version !== d.AMapUI.version ? c("\u4e0d\u5141\u8bb8\u591a\u4e2a\u7248\u672c AMapUI \u6df7\u7528") :
                n.AMapUI.push(function (a) { a ? c(a) : f.length ? window.AMapUI.loadUI(f, function () { for (var a = 0, b = f.length; a < b; a++) { var c = f[a].split("/").slice(-1)[0]; window.AMapUI[c] = arguments[a] } h() }) : h() })
        })
    } function r(a) {
        return new Promise(function (h, c) {
            if (g.Loca === b.failed) c("\u524d\u6b21\u8bf7\u6c42 Loca \u5931\u8d25"); else if (g.Loca === b.notload) {
                g.Loca = b.loading; d.Loca.version = a.version || d.Loca.version; var f = d.Loca.version, e = d.AMap.version.startsWith("2"), l = f.startsWith("2"); if (e && !l || !e && l) c("JSAPI \u4e0e Loca \u7248\u672c\u4e0d\u5bf9\u5e94\uff01\uff01");
                else { e = d.key; l = document.body || document.head; var k = document.createElement("script"); k.type = "text/javascript"; k.src = "https://webapi.amap.com/loca?v=" + f + "&key=" + e; k.onerror = function (a) { g.Loca = b.failed; c("\u8bf7\u6c42 AMapUI \u5931\u8d25") }; k.onload = function () { g.Loca = b.loaded; for (h(); n.Loca.length;)n.Loca.splice(0, 1)[0]() }; l.appendChild(k) }
            } else g.Loca === b.loaded ? a.version && a.version !== d.Loca.version ? c("\u4e0d\u5141\u8bb8\u591a\u4e2a\u7248\u672c Loca \u6df7\u7528") : h() : a.version && a.version !== d.Loca.version ?
                c("\u4e0d\u5141\u8bb8\u591a\u4e2a\u7248\u672c Loca \u6df7\u7528") : n.Loca.push(function (a) { a ? c(a) : c() })
        })
    } if (!window) throw Error("AMap JSAPI can only be used in Browser."); var b; (function (a) { a.notload = "notload"; a.loading = "loading"; a.loaded = "loaded"; a.failed = "failed" })(b || (b = {})); var d = { key: "", AMap: { version: "1.4.15", plugins: [] }, AMapUI: { version: "1.1", plugins: [] }, Loca: { version: "1.3.2" } }, g = { AMap: b.notload, AMapUI: b.notload, Loca: b.notload }, n = { AMap: [], AMapUI: [], Loca: [] }, q = [], t = function (a) {
        "function" == typeof a &&
        (g.AMap === b.loaded ? a(window.AMap) : q.push(a))
    }; return {
        load: function (a) {
            return new Promise(function (h, c) {
                if (g.AMap == b.failed) c(""); else if (g.AMap == b.notload) {
                    var f = a.key, e = a.version, l = a.plugins; f ? (window.AMap && "lbs.amap.com" !== location.host && c("\u7981\u6b62\u591a\u79cdAPI\u52a0\u8f7d\u65b9\u5f0f\u6df7\u7528"), d.key = f, d.AMap.version = e || d.AMap.version, d.AMap.plugins = l || d.AMap.plugins, g.AMap = b.loading, e = document.body || document.head, window.___onAPILoaded = function (d) {
                        delete window.___onAPILoaded; if (d) g.AMap =
                            b.failed, c(d); else for (g.AMap = b.loaded, m(a).then(function () { h(window.AMap) })["catch"](c); q.length;)q.splice(0, 1)[0]()
                    }, l = document.createElement("script"), l.type = "text/javascript", l.src = "https://webapi.amap.com/maps?callback=___onAPILoaded&v=" + d.AMap.version + "&key=" + f + "&plugin=" + d.AMap.plugins.join(","), l.onerror = function (a) { g.AMap = b.failed; c(a) }, e.appendChild(l)) : c("\u8bf7\u586b\u5199key")
                } else if (g.AMap == b.loaded) if (a.key && a.key !== d.key) c("\u591a\u4e2a\u4e0d\u4e00\u81f4\u7684 key"); else if (a.version &&
                    a.version !== d.AMap.version) c("\u4e0d\u5141\u8bb8\u591a\u4e2a\u7248\u672c JSAPI \u6df7\u7528"); else { f = []; if (a.plugins) for (e = 0; e < a.plugins.length; e += 1)-1 == d.AMap.plugins.indexOf(a.plugins[e]) && f.push(a.plugins[e]); if (f.length) window.AMap.plugin(f, function () { m(a).then(function () { h(window.AMap) })["catch"](c) }); else m(a).then(function () { h(window.AMap) })["catch"](c) } else if (a.key && a.key !== d.key) c("\u591a\u4e2a\u4e0d\u4e00\u81f4\u7684 key"); else if (a.version && a.version !== d.AMap.version) c("\u4e0d\u5141\u8bb8\u591a\u4e2a\u7248\u672c JSAPI \u6df7\u7528");
                    else { var k = []; if (a.plugins) for (e = 0; e < a.plugins.length; e += 1)-1 == d.AMap.plugins.indexOf(a.plugins[e]) && k.push(a.plugins[e]); t(function () { if (k.length) window.AMap.plugin(k, function () { m(a).then(function () { h(window.AMap) })["catch"](c) }); else m(a).then(function () { h(window.AMap) })["catch"](c) }) }
            })
        }, reset: function () {
            delete window.AMap; delete window.AMapUI; delete window.Loca; d = { key: "", AMap: { version: "1.4.15", plugins: [] }, AMapUI: { version: "1.1", plugins: [] }, Loca: { version: "1.3.2" } }; g = {
                AMap: b.notload, AMapUI: b.notload,
                Loca: b.notload
            }; n = { AMap: [], AMapUI: [], Loca: [] }
        }
    }
})
