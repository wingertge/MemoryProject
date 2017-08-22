import * as $ from "jquery";

export class Util {
    static postJson(url, body, success): JQuery.jqXHR<any> {
        return $.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify(body),
            contentType: "application/json",
            dataType: 'json',
            success: success
        });
    }

    static setupValidator() {
        $.validator.addMethod(
            "regex",
            function (value, element, regexp) {
                const re = new RegExp(regexp);
                return this.optional(element) || re.test(value);
            },
            "Please check your input."
        );
    }

    static setCustomStyle(css, themeData) {
        function camelCaseToDash(myStr) {
            return myStr.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
        }

        const replaceAll = (target, search, replacement) => {
            function escapeRegExp(str) {
                return str.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
            }

            return target.replace(new RegExp(escapeRegExp(search), 'g'), replacement);
        };

        let theme = themeData[2].responseJSON.result;
        
        let fixedCss = css[2].responseText;
        for (let property in theme) {
            if (theme.hasOwnProperty(property)) {
                fixedCss = replaceAll(fixedCss, `"$${camelCaseToDash(property)}"`, theme[property]);
            }
        }

        let style = $(`<style>${fixedCss}</style>`);
        $("head").append(style);
    }
}

export default Util;