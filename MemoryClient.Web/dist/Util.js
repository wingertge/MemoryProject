"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const $ = require("jquery");
class Util {
    static postJson(url, body, success) {
        return $.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify(body),
            contentType: "application/json",
            dataType: 'json',
            success: success
        });
    }
}
exports.Util = Util;
exports.default = Util;
//# sourceMappingURL=Util.js.map