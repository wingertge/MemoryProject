import * as React from "react";
import { FormattedMessage } from "react-intl";
class ErrorBox extends React.Component {
    render() {
        return (<div className="modal fade" id="error-modal">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title"><FormattedMessage id="errorHeader" defaultMessage="An Error occured"/></h5>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">

                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn primary" data-dismiss="modal" data-target="#error-modal"><FormattedMessage id="errorConfirm" defaultMessage="Close"/></button>
                        </div>
                    </div>
                </div>
            </div>);
    }
}
export default ErrorBox;
//# sourceMappingURL=ErrorBox.jsx.map