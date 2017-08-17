"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_intl_1 = require("react-intl");
class ErrorBox extends React.Component {
    render() {
        return (<div className="modal fade" id="error-modal">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title"><react_intl_1.FormattedMessage id="errorHeader" defaultMessage="An Error occured"/></h5>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">

                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn primary" data-dismiss="modal" data-target="#error-modal"><react_intl_1.FormattedMessage id="errorConfirm" defaultMessage="Close"/></button>
                        </div>
                    </div>
                </div>
            </div>);
    }
}
exports.default = ErrorBox;
//# sourceMappingURL=ErrorBox.jsx.map