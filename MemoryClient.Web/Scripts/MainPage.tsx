import * as React from "react";
import * as ReactDOM from "react-dom";
import * as ReactIntl from "react-intl";
import I18n from "./LocaleLoader";
import Home from "./Components/Home";
import * as en from "react-intl/locale-data/en";
import { Theme } from "./Models";
import IntlProvider = ReactIntl.IntlProvider;

import * as bootstrap from "bootstrap"; //only for webpack

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

    ReactDOM.render(
        <IntlProvider locale={I18n.getCurrentLocale()} messages={messages[2].responseJSON}>
            <Home/>
        </IntlProvider>,
        document.getElementById("content")
    );
}

var p1 = I18n.getStrings();
var p2 = $.get("/css/main.build.css", null, $.noop, "text");
var p3 = $.getJSON("/api/themes/current-theme");
$.when(p1, p2, p3).then(appCreateDOM).fail(() => console.log("initialisation error"));