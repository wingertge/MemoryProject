import * as React from "react";
import { FormattedNumber, FormattedMessage, injectIntl, FormattedRelative } from "react-intl";
import LessonsBox from "./LessonsBox";
import ErrorBox from "./ErrorBox";
import ReactLoading from "react-loading";
import { User } from "../Models";
import I18n from "../LocaleLoader";
import ReviewBox from "./ReviewBox";
class HomeState {
    constructor() {
        this.lessonsCount = 0;
        this.reviewsCount = 0;
        this.nextReview = new Date(0);
        this.reviewsNextHour = 0;
        this.reviewsNextDay = 0;
        this.user = new User();
    }
}
class HomeProps {
}
class Home extends React.Component {
    constructor() {
        super();
        this.loading = <div id="loading-screen">
        <div>
            <img src="~/img/logo.png" alt="Memory"/>
            <ReactLoading type="spinningBubbles" color="#464a4c"/>
        </div>
    </div>;
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
        const p3 = $.getJSON("/api/users/current", a => this.setState({ user: a.result || new User() }));
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
                errorThrown = I18n.format(this.props.intl, "An unknown error has occured.", "unknownError");
            $("#error-modal .modal-body").html(requestObj.status + errorThrown);
            $("#error-modal").modal("show");
        });
    }
    nextReviewFormatted() {
        if (this.state.nextReview < new Date(Date.now()))
            return (<FormattedMessage id="rightNow" defaultMessage="Right now"/>);
        else
            return (<FormattedRelative value={this.state.nextReview}/>);
    }
    page() {
        return (<div id="main">
                <div id="header" className="page-header">
                    <h1><FormattedMessage id="appName" defaultMessage={"Memory"}/></h1>
                    <div id="lessons-count" className="button-group">
                        <button className="btn btn-sq primary" id="lessons-btn" data-toggle="modal" data-target="#review-box" data-type="lessons"><FormattedNumber maximumSignificantDigits={2} value={this.state.lessonsCount}/></button>
                        <span id="lessons-text"><FormattedMessage id="lessonsLabel" defaultMessage={"Lessons"}/></span>
                    </div>
            
                    <div id="reviews-count" className="button-group">
                        <button className="btn btn-sq primary" id="reviews-btn" data-toggle="modal" data-target="#review-box" data-type="reviews"><FormattedNumber maximumSignificantDigits={2} value={this.state.reviewsCount}/></button>
                        <span id="reviews-text"><FormattedMessage id="reviewsLabel" defaultMessage={"Reviews"}/></span>
                    </div>

                    <div id="account-box" className="dropdown">
                        <button type="button" className="btn btn-lg primary dropdown-toggle" data-toggle="dropdown">
                            {this.state.user.userName}
                        </button>
                        <div className="dropdown-menu dropdown-menu-right">
                            <button type="button" className="dropdown-item" data-toggle="modal" data-target="#lessons-box-modal"><FormattedMessage id="viewAssignments" defaultMessage="View Active Lessons"/></button>
                            <button type="button" className="dropdown-item" data-toggle="modal" data-target="#account-modal"><FormattedMessage id="manageAccount" defaultMessage="Account Settings"/></button>
                            <div className="dropdown-divider"></div>
                            <button type="button" className="dropdown-item" onClick={this.signOut}><FormattedMessage id="logout" defaultMessage="Sign Out"/> <i className="fa fa-sign-out" aria-hidden={true}></i></button>
                        </div>
                    </div>
                </div>
                <div id="body" className="container">
                    <div className="row">
                        <div className="col-4">
                            <h1>{this.nextReviewFormatted()}</h1>
                            <span className="text-gray-dark text-sm"><FormattedMessage id="nextReviews" defaultMessage="Next Reviews"/></span>
                        </div>
                        <div className="col-4">
                            <h1><FormattedNumber value={this.state.reviewsNextHour}/></h1>
                            <span className="text-gray-dark text-sm"><FormattedMessage id="reviewsNextHour" defaultMessage="Next Hour"/></span>
                        </div>
                        <div className="col-4">
                            <h1><FormattedNumber value={this.state.reviewsNextDay}/></h1>
                            <span className="text-gray-dark text-sm"><FormattedMessage id="reviewsNextDay" defaultMessage="Next 24h"/></span>
                        </div>
                    </div>
                </div>
                <div id="footer">
                    Footer
                </div>
                <LessonsBox />
                <ReviewBox />
                <ErrorBox />
            </div>);
    }
    render() {
        return this.loading || this.page();
    }
}
export default injectIntl(Home);
//# sourceMappingURL=Home.jsx.map