import * as $ from "jquery";
class I18n {
    static getCurrentLocale() {
        return "en-US";
    }
    static getStrings(callback) {
        const url = `/lang/${this.getCurrentLocale()}.json`;
        $.getJSON(url, {}, callback).fail(a => console.log(a));
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
export default I18n;
//# sourceMappingURL=LocaleLoader.js.map