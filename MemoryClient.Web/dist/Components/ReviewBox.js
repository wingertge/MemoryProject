"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_intl_1 = require("react-intl");
const react_loading_1 = require("react-loading");
const Util_1 = require("../Util");
class ReviewBoxState {
    constructor() {
        this.invalid = false;
        this.errors = {};
        this.type = "reviews";
    }
}
class ReviewBoxProps {
}
class ReviewBox extends React.Component {
    constructor() {
        super();
        this.state = new ReviewBoxState();
        this.onOpened = this.onOpened.bind(this);
        this.fetchNextReview = this.fetchNextReview.bind(this);
        this.submitReview = this.submitReview.bind(this);
        this.loading = this.loading.bind(this);
        this.handleInput = this.handleInput.bind(this);
    }
    componentDidMount() {
        $("#review-box").on("shown.bs.modal", this.onOpened);
    }
    onOpened(event) {
        var button = $(event.relatedTarget);
        this.setState({ type: button.attr("data-type") });
        this.fetchNextReview();
    }
    fetchNextReview() {
        this.setState({ formModel: null });
        $.getJSON(`/api/${this.state.type}/next-review`, json => {
            if (!json.result) {
                $("#review-box").modal("hide");
                return;
            }
            this.setState({ formModel: json.result });
        });
    }
    submitReview(event) {
        if (event.key != "Enter")
            return;
        event.preventDefault();
        if (this.state.invalid) {
            this.fetchNextReview();
            return;
        }
        $("#reviewInputOverlay").show();
        Util_1.Util.postJson(`/api/${this.state.type}/submit-review`, this.state.formModel, (json) => {
            if (json.succeeded)
                this.fetchNextReview();
            else {
                $("#reviewInputOverlay").hide();
                this.setState({ invalid: true });
            }
        });
    }
    loading() {
        if (this.state.formModel)
            return null;
        return (React.createElement(react_loading_1.default, { type: "spinningBubbles", color: "#ccc", delay: 100 }));
    }
    handleInput(e) {
        this.state.formModel.to = e.target.value;
        this.setState({ formModel: this.state.formModel });
    }
    content() {
        return (React.createElement("div", { className: "review-form" },
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col-12", id: "review-box-from" },
                    React.createElement("span", { id: "review-from-text" }, this.state.formModel.from))),
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col-12", id: "review-box-to" },
                    React.createElement("form", null,
                        React.createElement("div", { className: "overlay-container" },
                            React.createElement("input", { type: "text", name: "to", value: this.state.formModel.to, onKeyPress: this.submitReview, onChange: this.handleInput }),
                            React.createElement("div", { className: "overlay", id: "reviewInputOverlay" },
                                React.createElement(react_loading_1.default, { type: "bubbles", color: "#ccc" })))))),
            React.createElement("div", { className: "btn-group btn-group-justified" },
                React.createElement("div", { className: "btn-group" },
                    React.createElement("button", { type: "button", className: "btn btn-secondary" }, "Test1")),
                React.createElement("div", { className: "btn-group dropdown" },
                    React.createElement("button", { type: "button", className: `btn btn-secondary ${!this.state.invalid ? "disabled" : "btn-info"}`, "data-toggle": "dropdown", "aria-haspopup": "true", "aria-expanded": "false", id: "solutionDropdownToggle" },
                        React.createElement(react_intl_1.FormattedMessage, { id: "solutionButton", defaultMessage: "Show Solution" })),
                    React.createElement("div", { className: "dropdown-menu", "aria-labelledby": "solutionDropdownToggle" },
                        React.createElement("button", { type: "button" }, this.state.formModel.solution))),
                React.createElement("div", { className: "btn-group" },
                    React.createElement("button", { type: "button", className: "btn btn-secondary" }, "Test1")))));
    }
    render() {
        return (React.createElement("div", { id: "review-box", className: "modal fade" },
            React.createElement("div", { className: "modal-dialog" },
                React.createElement("div", { className: "modal-content" },
                    React.createElement("div", { className: "modal-header" },
                        React.createElement("h4", { className: "modal-title" },
                            React.createElement(react_intl_1.FormattedMessage, { id: "reviewModalTitle", defaultMessage: "Review" })),
                        React.createElement("button", { type: "button", className: "close", "data-dismiss": "modal", "data-target": "#review-box" }, "\u00D7")),
                    React.createElement("div", { className: "modal-body" }, this.loading() || this.content())))));
    }
}
exports.ReviewBox = ReviewBox;
exports.default = ReviewBox;
//# sourceMappingURL=ReviewBox.js.map