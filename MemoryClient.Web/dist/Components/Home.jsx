"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_intl_1 = require("react-intl");
const LessonsBox_1 = require("./LessonsBox");
const ErrorBox_1 = require("./ErrorBox");
const react_loading_1 = require("react-loading");
const Models_1 = require("../Models");
const LocaleLoader_1 = require("../LocaleLoader");
class HomeState {
    constructor() {
        this.lessonsCount = 0;
        this.reviewsCount = 0;
        this.user = new Models_1.User();
    }
}
class Home extends React.Component {
    constructor() {
        super();
        this.loading = <div id="loading-screen">
        <div>
            <img src="~/img/logo.png" alt="Memory"/>
            <react_loading_1.default type="spinningBubbles" color="#464a4c"/>
        </div>
    </div>;
        this.state = new HomeState();
    }
    componentDidMount() {
        var promises = this.loadState();
        $.when.apply(this, promises).then(a => {
            this.loading = null;
            this.forceUpdate();
        }, error => {
            console.log(error);
            location.reload();
        });
        window.setInterval(this.loadState.bind(this), 900000);
    }
    loadState() {
        const p1 = $.getJSON("/api/reviews/current-count", a => this.setState({ reviewsCount: a.result || 0 }));
        const p2 = $.getJSON("/api/lessons/current-count", a => this.setState({ lessonsCount: a.result || 0 }));
        const p3 = $.getJSON("/api/users/current", a => this.setState({ user: a.result || new Models_1.User() }));
        return [p1, p2, p3];
    }
    signOut(e) {
        e.preventDefault();
        $.getJSON("/api/auth/signout", result => {
            if (result.succeeded) {
                window.location.href = "/signin";
            }
            else {
                $("#error-modal .modal-body").html(LocaleLoader_1.default.format(this.props.intl, "An unknown error has occured.", "unknownError"));
                $("#error-modal").modal("show");
            }
        }).catch((requestObj, error, errorThrown) => {
            $("#error-modal .modal-body").html(errorThrown);
            $("#error-modal").modal("show");
        });
    }
    page() {
        return (<div id="main">
                <div id="header" className="page-header">
                    <h1><react_intl_1.FormattedMessage id="appName" defaultMessage={"Memory"}/></h1>
                    <div id="lessons-count" className="button-group">
                        <button className="btn btn-sq primary" id="lessons-btn" data-toggle="modal" data-target="#lessons-box-modal"><react_intl_1.FormattedNumber maximumSignificantDigits={2} value={this.state.lessonsCount}/></button>
                        <span id="lessons-text"><react_intl_1.FormattedMessage id="lessonsLabel" defaultMessage={"Lessons"}/></span>
                    </div>
            
                    <div id="reviews-count" className="button-group">
                        <button className="btn btn-sq primary" id="reviews-btn"><react_intl_1.FormattedNumber maximumSignificantDigits={2} value={this.state.reviewsCount}/></button>
                        <span id="reviews-text"><react_intl_1.FormattedMessage id="reviewsLabel" defaultMessage={"Reviews"}/></span>
                    </div>

                    <div id="account-box" className="dropdown">
                        <button type="button" className="btn btn-lg primary dropdown-toggle" data-toggle="dropdown">
                            {this.state.user.userName}
                        </button>
                        <div className="dropdown-menu dropdown-menu-right">
                            <button type="button" className="dropdown-item" data-toggle="modal" data-target="#account-modal"><react_intl_1.FormattedMessage id="manageAccount" defaultMessage="Account Settings"/></button>
                            <div className="dropdown-divider"></div>
                            <button type="button" className="dropdown-item" onClick={this.signOut}><react_intl_1.FormattedMessage id="logout" defaultMessage="Sign Out"/> <i className="fa fa-sign-out" aria-hidden={true}></i></button>
                        </div>
                    </div>
                </div>
                <div id="body">

                </div>
                <div id="footer">
                    Footer
                </div>
                <LessonsBox_1.default />
                <ErrorBox_1.default />
            </div>);
    }
    render() {
        return this.loading || this.page();
    }
}
exports.default = react_intl_1.injectIntl(Home);
//# sourceMappingURL=Home.jsx.map