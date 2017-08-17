import * as React from "react";
import { FormattedMessage, FormattedNumber, FormattedRelative } from "react-intl";
import { LanguagePair } from "../Models";
import LessonsEditor from "./LessonsEditor";
import { from as enumerableFrom } from "linq";
import ReactLoading from "react-loading";
export class LessonsState {
    constructor() {
        this.lessons = [];
    }
}
export class LessonEntry {
}
export class LessonsBox extends React.Component {
    constructor() {
        super();
        this.loading = <ReactLoading type="bubbles" color="#444" className="loading-bubbles"/>;
        this.state = new LessonsState();
    }
    loadState() {
        $.getJSON("/api/lessons/current-list", (json) => {
            var list = enumerableFrom(json.result);
            var lookup = list.toLookup(a => new LanguagePair(a.lesson.languageFrom, a.lesson.languageTo), b => b);
            var parsed = lookup.toEnumerable().select(a => {
                return {
                    languages: a.key(),
                    lessons: a.toArray()
                };
            }).toArray();
            this.loading = null;
            this.setState({ lessons: parsed });
        });
    }
    componentDidMount() {
        $("#lessons-box-modal").on("shown.bs.modal", this.loadState.bind(this));
    }
    render() {
        return (<div id="lessons-box">
                <div className="modal fade" role="dialog" id="lessons-box-modal">
                    <div className="modal-dialog modal-lg">
                        <div className="modal-content">
                            <div className="modal-header">
                                <button type="button" className="close" data-dismiss="modal" data-target="#lessons-box-modal">&times;</button>
                                <h4 className="modal-title"><FormattedMessage id="lessonsLabel" defaultMessage={"Lessons"}/></h4>
                            </div>
                            <div className="modal-body">
                                <button type="button" id="lessons-add-btn" className="btn btn-success" data-toggle="modal" data-target="#lessons-editor">
                                    <FormattedMessage id="add" defaultMessage={"Add"}/><span className="glyphicon-plus"></span>
                                </button>
                                <LessonsList lessons={this.state.lessons} loading={this.loading}/>
                            </div>
                        </div>
                    </div>
                </div>
                <LessonsEditor />
            </div>);
    }
}
export class LessonsList extends React.Component {
    render() {
        const subListNodes = this.props.lessons.map(entry => {
            return (<LessonsSubList key={entry.languages.toString()} languages={entry.languages} lessons={entry.lessons}/>);
        });
        return (<div id="lessons-list">
                {this.props.loading || subListNodes}
            </div>);
    }
}
export class LessonsSubList extends React.Component {
    getFormattedTime(time) {
        if (time < new Date(Date.now()))
            return (<FormattedMessage id="rightNow" defaultMessage="Right Now"/>);
        return (<FormattedRelative value={time}/>);
    }
    render() {
        const rows = this.props.lessons.map(assignment => {
            return (<tr key={assignment.id}>
                    <td>{assignment.lesson.reading}</td>
                    <td>{assignment.lesson.pronunciation}</td>
                    <td>{assignment.lesson.meaning}</td>
                    <td><FormattedNumber value={assignment.stage}/></td>
                    <td>{this.getFormattedTime(assignment.nextReview)}</td>
                </tr>);
        });
        return (<div className="lessons-sub-list">
                <h4><FormattedMessage id="languagePair" defaultMessage="{languageFrom} ({countryFrom}) - {languageTo} ({countryTo})" values={{
            languageFrom: this.props.languages.languageFrom.englishName,
            languageTo: this.props.languages.languageTo.englishName,
            countryFrom: this.props.languages.languageFrom.englishCountryName,
            countryTo: this.props.languages.languageTo.englishCountryName
        }}/></h4>
                <table className="table table-hover table-responsive table-bordered table-sm">
                    <thead className="bg-primary">
                        <tr>
                            <th><FormattedMessage id="reading" defaultMessage="Reading" description="test"/></th>
                            <th><FormattedMessage id="pronunciation" defaultMessage={"Pronunciation"}/></th>
                            <th><FormattedMessage id="meaning" defaultMessage={"Meaning"}/></th>
                            <th><FormattedMessage id="currentStage" defaultMessage={"Stage"}/></th>
                            <th style={{ width: "8rem" }}><FormattedMessage id="nextReview" defaultMessage={"Next Review"}/></th>
                        </tr>
                    </thead>
                    <tbody>
                        {rows}
                    </tbody>
                </table>
            </div>);
    }
}
export default LessonsBox;
//# sourceMappingURL=LessonsBox.jsx.map