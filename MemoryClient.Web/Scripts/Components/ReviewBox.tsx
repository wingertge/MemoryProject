import * as React from "react";
import { ReviewModel, ActionResult } from "../Models";
import { FormattedMessage } from "react-intl";
import ReactLoading from "react-loading";
import { Util } from "../Util";

class ReviewBoxState {
    formModel: ReviewModel;
    invalid: boolean = false;
    errors: { [key: string]: string } = {};
    type: "lessons" | "reviews" = "reviews";
}

class ReviewBoxProps {
    
}

export class ReviewBox extends React.Component<ReviewBoxProps, ReviewBoxState> {
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
        this.setState({ type: button.attr("data-type") as "lessons" | "reviews" });
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
        if (event.key != "Enter") return;
        event.preventDefault();
        if (this.state.invalid) {
            this.fetchNextReview();
            return;
        }

        $("#reviewInputOverlay").show();
        Util.postJson(`/api/${this.state.type}/submit-review`, this.state.formModel, (json: ActionResult) => {
            if (json.succeeded)
                this.fetchNextReview();
            else {
                $("#reviewInputOverlay").hide();
                this.setState({ invalid: true })
            }
        });
    }

    loading(): JSX.Element {
        if (this.state.formModel) return null;
        return (
            <ReactLoading type="spinningBubbles" color="#ccc" delay={100} />
        );
    }

    handleInput(e) {
        this.state.formModel.to = e.target.value;
        this.setState({ formModel: this.state.formModel });
    }

    content(): JSX.Element {
        return (
            <div className="review-form">
                <div className="row">
                    <div className="col-12" id="review-box-from">
                        <span id="review-from-text">{this.state.formModel.from}</span>
                    </div>
                </div>
                <div className="row">
                    <div className="col-12" id="review-box-to">
                        <form>
                            <div className="overlay-container">
                                <input type="text" name="to" value={this.state.formModel.to} onKeyPress={this.submitReview} onChange={this.handleInput} />
                                <div className="overlay" id="reviewInputOverlay"><ReactLoading type="bubbles" color="#ccc" /></div>
                            </div>
                        </form>
                    </div>
                </div>
                <div className="btn-group btn-group-justified">
                    <div className="btn-group">
                        <button type="button" className="btn btn-secondary">Test1</button>
                    </div>
                    <div className="btn-group dropdown">
                        <button type="button" className={`btn btn-secondary ${!this.state.invalid ? "disabled" : "btn-info"}`} data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" id="solutionDropdownToggle">
                            <FormattedMessage id="solutionButton" defaultMessage="Show Solution" />
                        </button>
                        <div className="dropdown-menu" aria-labelledby="solutionDropdownToggle">
                            <button type="button">{ this.state.formModel.solution }</button>
                        </div>
                    </div>
                    <div className="btn-group">
                        <button type="button" className="btn btn-secondary">Test1</button>
                    </div>
                </div>
            </div>
        );
    }

    render(): JSX.Element {
        return (
            <div id="review-box" className="modal fade">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h4 className="modal-title"><FormattedMessage id="reviewModalTitle" defaultMessage="Review" /></h4>
                            <button type="button" className="close" data-dismiss="modal" data-target="#review-box">&times;</button>
                        </div>
                        <div className="modal-body">
                            { this.loading() || this.content() }
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default ReviewBox;