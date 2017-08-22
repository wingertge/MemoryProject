import * as React from "react";
import * as ReactDOM from "react-dom";
import * as ReactIntl from "react-intl";
import I18n from "./LocaleLoader";
import Home from "./Components/Home";
import * as en from "react-intl/locale-data/en";
import { Theme } from "./Models";
import Util from "./Util";
import IntlProvider = ReactIntl.IntlProvider;

import * as bootstrap from "bootstrap"; //only for webpack

const appCreateDOM = (messages, css, themeData) => {
    var temp = bootstrap;
    ReactIntl.addLocaleData(en);

    Util.setupValidator();

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