"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_intl_1 = require("react-intl");
class ErrorBox extends React.Component {
    render() {
        return (React.createElement("div", { className: "modal fade", id: "error-modal" },
            React.createElement("div", { className: "modal-dialog" },
                React.createElement("div", { className: "modal-content" },
                    React.createElement("div", { className: "modal-header" },
                        React.createElement("h5", { className: "modal-title" },
                            React.createElement(react_intl_1.FormattedMessage, { id: "errorHeader", defaultMessage: "An Error occured" })),
                        React.createElement("button", { type: "button", className: "close", "data-dismiss": "modal", "aria-label": "Close" },
                            React.createElement("span", { "aria-hidden": "true" }, "\u00D7"))),
                    React.createElement("div", { className: "modal-body" }),
                    React.createElement("div", { className: "modal-footer" },
                        React.createElement("button", { type: "button", className: "btn primary", "data-dismiss": "modal", "data-target": "#error-modal" },
                            React.createElement(react_intl_1.FormattedMessage, { id: "errorConfirm", defaultMessage: "Close" })))))));
    }
}
exports.default = ErrorBox;
//# sourceMappingURL=ErrorBox.js.map