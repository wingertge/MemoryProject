import * as Intl from "react-intl";
import * as $ from "jquery";
import InjectedIntl = Intl.InjectedIntl;

class I18n {
    static getCurrentLocale(): string {
        return "en-US";
    }

    static getStrings() {
        const url = `/lang/${this.getCurrentLocale()}.json`;
        return $.getJSON(url);
    }

    static init() {
        
    }

    static format(intl: InjectedIntl, defaultMessage: string, key?: string, values?: object): string {
        return intl.formatMessage({
            id: key || defaultMessage.toLocaleLowerCase(),
            defaultMessage: defaultMessage
        });
    }
}

export default I18n;