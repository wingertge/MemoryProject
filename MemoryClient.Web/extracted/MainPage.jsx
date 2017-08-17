import * as React from "react";
import * as ReactDOM from "react-dom";
import * as ReactIntl from "react-intl";
import I18n from "./LocaleLoader";
import Home from "./Components/Home";
import * as en from "react-intl/locale-data/en";
var IntlProvider = ReactIntl.IntlProvider;
import * as bootstrap from "bootstrap"; //only for webpack
const appCreateDOM = messages => {
    var temp = bootstrap;
    ReactIntl.addLocaleData(en);
    ReactDOM.render(<IntlProvider locale={I18n.getCurrentLocale()} messages={messages}>
            <Home />
        </IntlProvider>, document.getElementById("content"));
};
I18n.getStrings(appCreateDOM);
//# sourceMappingURL=MainPage.jsx.map