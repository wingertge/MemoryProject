"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const ReactDOM = require("react-dom");
const ReactIntl = require("react-intl");
const LocaleLoader_1 = require("./LocaleLoader");
const Home_1 = require("./Components/Home");
const en = require("react-intl/locale-data/en");
var IntlProvider = ReactIntl.IntlProvider;
const bootstrap = require("bootstrap"); //only for webpack
const appCreateDOM = (messages, css, themeData) => {
    function camelCaseToDash(myStr) {
        return myStr.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
    }
    const replaceAll = (target, search, replacement) => {
        function escapeRegExp(str) {
            return str.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
        }
        return target.replace(new RegExp(escapeRegExp(search), 'g'), replacement);
    };
    var theme = themeData[2].responseJSON.result;
    var temp = bootstrap;
    ReactIntl.addLocaleData(en);
    var fixedCss = css[2].responseText;
    for (var property in theme) {
        if (theme.hasOwnProperty(property)) {
            fixedCss = replaceAll(fixedCss, `"$${camelCaseToDash(property)}"`, theme[property]);
        }
    }
    var style = $(`<style>${fixedCss}</style>`);
    $("head").append(style);
    ReactDOM.render(React.createElement(IntlProvider, { locale: LocaleLoader_1.default.getCurrentLocale(), messages: messages[2].responseJSON },
        React.createElement(Home_1.default, null)), document.getElementById("content"));
};
var p1 = LocaleLoader_1.default.getStrings();
var p2 = $.get("/css/main.build.css", null, $.noop, "text");
var p3 = $.getJSON("/api/themes/current-theme");
$.when(p1, p2, p3).then(appCreateDOM).fail(() => console.log("initialisation error"));
//# sourceMappingURL=MainPage.js.map