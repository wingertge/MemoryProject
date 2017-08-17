"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const $ = require("jquery");
class I18n {
    static getCurrentLocale() {
        return "en-US";
    }
    static getStrings() {
        const url = `/lang/${this.getCurrentLocale()}.json`;
        return $.getJSON(url);
    }
    static init() {
    }
    static format(intl, defaultMessage, key, values) {
        return intl.formatMessage({
            id: key || defaultMessage.toLocaleLowerCase(),
            defaultMessage: defaultMessage
        });
    }
}
exports.default = I18n;
//# sourceMappingURL=LocaleLoader.js.map