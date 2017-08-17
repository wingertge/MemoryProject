"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_intl_1 = require("react-intl");
const LessonsBox_1 = require("./LessonsBox");
const ErrorBox_1 = require("./ErrorBox");
const react_loading_1 = require("react-loading");
const Models_1 = require("../Models");
const LocaleLoader_1 = require("../LocaleLoader");
const ReviewBox_1 = require("./ReviewBox");
class HomeState {
    constructor() {
        this.lessonsCount = 0;
        this.reviewsCount = 0;
        this.nextReview = new Date(0);
        this.reviewsNextHour = 0;
        this.reviewsNextDay = 0;
        this.user = new Models_1.User();
    }
}
class HomeProps {
}
class Home extends React.Component {
    constructor() {
        super();
        this.loading = React.createElement("div", { id: "loading-screen" },
            React.createElement("div", null,
                React.createElement("img", { src: "~/img/logo.png", alt: "Memory" }),
                React.createElement(react_loading_1.default, { type: "spinningBubbles", color: "#464a4c" })));
        this.state = new HomeState();
        this.signOut = this.signOut.bind(this);
    }
    componentDidMount() {
        var promises = this.loadState();
        $.when.apply(this, promises).then(a => {
            this.loading = null;
            this.forceUpdate();
        }, error => {
            console.log(error);
        });
        window.setInterval(this.loadState.bind(this), 900000);
        window.setInterval(() => this.setState({ nextReview: this.state.nextReview }), 1000); //Update relative time
    }
    loadState() {
        const p1 = $.getJSON("/api/reviews/current-count", a => this.setState({ reviewsCount: a.result || 0 }));
        const p2 = $.getJSON("/api/lessons/current-count", a => this.setState({ lessonsCount: a.result || 0 }));
        const p3 = $.getJSON("/api/users/current", a => this.setState({ user: a.result || new Models_1.User() }));
        const p4 = $.getJSON("/api/reviews/next-review-time", a => this.setState({ nextReview: new Date(a.result) }));
        const p5 = $.getJSON("/api/reviews/next-hour-count", a => this.setState({ reviewsNextHour: a.result }));
        const p6 = $.getJSON("/api/reviews/next-day-count", a => this.setState({ reviewsNextDay: a.result }));
        return [p1, p2, p3, p4, p5, p6];
    }
    signOut(e) {
        e.preventDefault();
        $.getJSON("/api/auth/logout", result => {
            window.location.href = "/login";
        }).catch((requestObj, error, errorThrown) => {
            if (errorThrown === "")
                errorThrown = LocaleLoader_1.default.format(this.props.intl, "An unknown error has occured.", "unknownError");
            $("#error-modal .modal-body").html(requestObj.status + errorThrown);
            $("#error-modal").modal("show");
        });
    }
    nextReviewFormatted() {
        if (this.state.nextReview < new Date(Date.now()))
            return (React.createElement(react_intl_1.FormattedMessage, { id: "rightNow", defaultMessage: "Right now" }));
        else
            return (React.createElement(react_intl_1.FormattedRelative, { value: this.state.nextReview }));
    }
    page() {
        return (React.createElement("div", { id: "main" },
            React.createElement("div", { id: "header", className: "page-header" },
                React.createElement("h1", null,
                    React.createElement(react_intl_1.FormattedMessage, { id: "appName", defaultMessage: "Memory" })),
                React.createElement("div", { id: "lessons-count", className: "button-group" },
                    React.createElement("button", { className: "btn btn-sq primary", id: "lessons-btn", "data-toggle": "modal", "data-target": "#review-box", "data-type": "lessons" },
                        React.createElement(react_intl_1.FormattedNumber, { maximumSignificantDigits: 2, value: this.state.lessonsCount })),
                    React.createElement("span", { id: "lessons-text" },
                        React.createElement(react_intl_1.FormattedMessage, { id: "lessonsLabel", defaultMessage: "Lessons" }))),
                React.createElement("div", { id: "reviews-count", className: "button-group" },
                    React.createElement("button", { className: "btn btn-sq primary", id: "reviews-btn", "data-toggle": "modal", "data-target": "#review-box", "data-type": "reviews" },
                        React.createElement(react_intl_1.FormattedNumber, { maximumSignificantDigits: 2, value: this.state.reviewsCount })),
                    React.createElement("span", { id: "reviews-text" },
                        React.createElement(react_intl_1.FormattedMessage, { id: "reviewsLabel", defaultMessage: "Reviews" }))),
                React.createElement("div", { id: "account-box", className: "dropdown" },
                    React.createElement("button", { type: "button", className: "btn btn-lg primary dropdown-toggle", "data-toggle": "dropdown" }, this.state.user.userName),
                    React.createElement("div", { className: "dropdown-menu dropdown-menu-right" },
                        React.createElement("button", { type: "button", className: "dropdown-item", "data-toggle": "modal", "data-target": "#lessons-box-modal" },
                            React.createElement(react_intl_1.FormattedMessage, { id: "viewAssignments", defaultMessage: "View Active Lessons" })),
                        React.createElement("button", { type: "button", className: "dropdown-item", "data-toggle": "modal", "data-target": "#account-modal" },
                            React.createElement(react_intl_1.FormattedMessage, { id: "manageAccount", defaultMessage: "Account Settings" })),
                        React.createElement("div", { className: "dropdown-divider" }),
                        React.createElement("button", { type: "button", className: "dropdown-item", onClick: this.signOut },
                            React.createElement(react_intl_1.FormattedMessage, { id: "logout", defaultMessage: "Sign Out" }),
                            " ",
                            React.createElement("i", { className: "fa fa-sign-out", "aria-hidden": true }))))),
            React.createElement("div", { id: "body", className: "container" },
                React.createElement("div", { className: "row" },
                    React.createElement("div", { className: "col-4" },
                        React.createElement("h1", null, this.nextReviewFormatted()),
                        React.createElement("span", { className: "text-gray-dark text-sm" },
                            React.createElement(react_intl_1.FormattedMessage, { id: "nextReviews", defaultMessage: "Next Reviews" }))),
                    React.createElement("div", { className: "col-4" },
                        React.createElement("h1", null,
                            React.createElement(react_intl_1.FormattedNumber, { value: this.state.reviewsNextHour })),
                        React.createElement("span", { className: "text-gray-dark text-sm" },
                            React.createElement(react_intl_1.FormattedMessage, { id: "reviewsNextHour", defaultMessage: "Next Hour" }))),
                    React.createElement("div", { className: "col-4" },
                        React.createElement("h1", null,
                            React.createElement(react_intl_1.FormattedNumber, { value: this.state.reviewsNextDay })),
                        React.createElement("span", { className: "text-gray-dark text-sm" },
                            React.createElement(react_intl_1.FormattedMessage, { id: "reviewsNextDay", defaultMessage: "Next 24h" }))))),
            React.createElement("div", { id: "footer" }, "Footer"),
            React.createElement(LessonsBox_1.default, null),
            React.createElement(ReviewBox_1.default, null),
            React.createElement(ErrorBox_1.default, null)));
    }
    render() {
        return this.loading || this.page();
    }
}
exports.default = react_intl_1.injectIntl(Home);
//# sourceMappingURL=Home.js.map