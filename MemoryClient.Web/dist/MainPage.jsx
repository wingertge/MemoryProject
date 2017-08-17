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
const appCreateDOM = messages => {
    var temp = bootstrap;
    ReactIntl.addLocaleData(en);
    ReactDOM.render(<IntlProvider locale={LocaleLoader_1.default.getCurrentLocale()} messages={messages}>
            <Home_1.default />
        </IntlProvider>, document.getElementById("content"));
};
LocaleLoader_1.default.getStrings(appCreateDOM);
//# sourceMappingURL=MainPage.jsx.map