import * as $ from "jquery";
export class Util {
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
export default Util;
//# sourceMappingURL=Util.js.map