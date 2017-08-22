import * as React from "react";
import { FormattedMessage } from "react-intl";
import ReactLoading from "react-loading";
import { User } from "../Models";
import "jquery-validation";
import { Util } from "../Util";
import { ChangePasswordModel, ActionResult } from "../Models";
import * as Enumerable from "linq";
import * as ReactLoader from "react-loader";

class AccountBoxState {
    user: User = null;
}

class ChangePasswordBoxState {
    loading: boolean;
    model = new ChangePasswordModel();
}

class ChangePasswordBoxProps {
    onSubmit: (model: ChangePasswordModel) => void;
    initialModel?: ChangePasswordModel = null;
}


class ChangePasswordForm extends React.Component<ChangePasswordBoxProps, ChangePasswordModel> {
    constructor() {
        super();
        this.state = this.props.initialModel || new ChangePasswordModel();
    }

    componentDidMount() {
        $("#change-password-form").validate({
            rules: {
                oldPassword: "required",
                newPassword: {
                    required: true,
                    regex: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/
                },
                newPasswordConfirm: {
                    equalTo: "#newPassword"
                }
            }
        });
    }

    render() {
        return (
            <form id="change-password-form">
                <div className="form-group">
                    <label><FormattedMessage id="oldPasswordLabel" defaultMessage="Current password" /></label>
                    <input type="password" className="form-control" name="oldPassword" value={this.state.oldPassword} onChange={e => this.setState({ oldPassword: e.target.value })} />
                </div>
                <div className="form-group">
                    <label><FormattedMessage id="newPasswordLabel" defaultMessage="New password" /></label>
                    <input type="password" className="form-control" name="newPassword" id="newPassword" value={this.state.newPassword} onChange={e => this.setState({ newPassword: e.target.value })} />
                </div>
                <div className="form-group">
                    <label><FormattedMessage id="newPasswordConfirmLabel" defaultMessage="Confirm password" /></label>
                    <input type="password" className="form-control" name="newPasswordConfirm" value={this.state.newPasswordConfirm} onChange={e => this.setState({ newPasswordConfirm: e.target.value })} />
                </div>
                <button type="button" onClick={a => this.props.onSubmit(this.state)} className="btn primary"><FormattedMessage id="saveButton" defaultMessage="Save" /></button>
                <button type="button" data-dismiss="modal" data-target="#change-password-modal" className="btn secondary"><FormattedMessage id="cancelButton" defaultMessage="Cancel" /></button>
            </form>
        );
    }
}

class ChangePasswordBox extends React.Component<AccountBoxState, ChangePasswordBoxState> {
    constructor() {
        super();
        this.state = new ChangePasswordBoxState();
    }

    save(model: ChangePasswordModel) {
        this.setState({ model: model });
        if ($("#change-password-form").valid()) {
            Util.postJson("/api/account/change-password", this.state, (json: ActionResult) => {
                if (json.succeeded) {
                    $("#change-password-modal").modal("hide");
                } else {
                    var errors = Enumerable.from(json.errors);
                    var selected = errors.select(error => {
                        var err = {};
                        err[error.key] = error.value[0];
                        return err;
                    });
                    $("#change-password-form").validate().showErrors(selected.toArray());
                }
            });
        }
    }

    componentDidMount() {
        $("#account-modal").on("shown.bs.modal", this.onModalShown.bind(this));
    }

    onModalShown() {
        this.setState({ model: new ChangePasswordModel() });
    }

    render() {
        return (
            <div className="modal fade" id="change-password-modal">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <FormattedMessage id="changePasswordHeader" defaultMessage="Change password" />
                            <button type="button" className="close" data-dismiss="modal" data-target="#change-password-modal">&times;</button>
                        </div>
                        <div className="modal-body">
                            <ReactLoader loaded={!this.state.loading}>
                                <ChangePasswordForm onSubmit={this.save.bind(this)} initialModel={this.state.model} />
                            </ReactLoader>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

class AccountBox extends React.Component<undefined, AccountBoxState> {
    constructor() {
        super();
        this.state = new AccountBoxState();
    }

    componentDidMount() {
        $("#account-modal").on("shown.bs.modal", this.modalToggled.bind(this));
    }

    modalToggled() {
        $.getJSON("/api/auth/current-user", json => {
            this.setState({ user: json.result });
            this.forceUpdate();
        });
    }

    content() {
        return (
            <div id="account-modal-content">
                <form>
                    <div className="form-group row" style={{ marginLeft: "0.5rem", marginRight: "0.5rem", marginTop: "0.5rem" }}>
                        <label className="col-8 col-form-label"><FormattedMessage id="changePasswordLabel" defaultMessage="Change password:" /></label>
                        <button type="button" data-toggle="modal" data-target="#change-password-modal" className="btn primary col-4"><FormattedMessage id="changePasswordButton" defaultMessage="Change" /></button>
                    </div>
                    <div className="form-group row" style={{marginLeft: "0.5rem", marginRight: "0.5rem"}}>
                        <button type="button" className="btn primary col-6"><FormattedMessage id="saveButton" defaultMessage="Save" /></button>
                        <button type="button" className="btn secondary col-6"><FormattedMessage id="cancelButton" defaultMessage="Cancel" /></button>
                    </div>
                </form>
            </div>
        );
    }

    loading() {
        if (this.state.user) return null;
        return (
            <ReactLoading type="bubbles" color="#ddd" />
        );
    }

    render() {
        return (
            <div>
                <div className="modal fade" id="account-modal">
                    <div className="modal-dialog modal-lg">
                        <div className="modal-content">
                            <div className="modal-header">
                                <FormattedMessage id="accountSettingsHeader" defaultMessage="Account Settings" />
                                <button type="button" className="close" data-dismiss="modal" data-target="#account-box">&times;</button>
                            </div>
                            {this.loading() || this.content()}
                        </div>
                    </div>
                </div>
                <ChangePasswordBox user={this.state.user} />
            </div>
        );
    }
}

export default AccountBox