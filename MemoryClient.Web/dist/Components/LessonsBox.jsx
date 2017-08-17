"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const react_intl_1 = require("react-intl");
const Models_1 = require("../Models");
const LessonsEditor_1 = require("./LessonsEditor");
const linq_1 = require("linq");
const react_loading_1 = require("react-loading");
class LessonsState {
    constructor() {
        this.lessons = [];
    }
}
exports.LessonsState = LessonsState;
class LessonEntry {
}
exports.LessonEntry = LessonEntry;
class LessonsBox extends React.Component {
    constructor() {
        super();
        this.loading = <react_loading_1.default type="bubbles" color="#444" className="loading-bubbles"/>;
        this.state = new LessonsState();
    }
    loadState() {
        $.getJSON("/api/lessons/current-list", (json) => {
            var list = linq_1.from(json.result);
            var lookup = list.toLookup(a => new Models_1.LanguagePair(a.lesson.languageFrom, a.lesson.languageTo), b => b);
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
                    <div className="modal-dialog">
                        <div className="modal-content">
                            <div className="modal-header">
                                <button type="button" className="close" data-dismiss="modal" data-target="#lessons-box-modal">&times;</button>
                                <h4 className="modal-title"><react_intl_1.FormattedMessage id="lessonsLabel" defaultMessage={"Lessons"}/></h4>
                            </div>
                            <div className="modal-body">
                                <button type="button" id="lessons-add-btn" className="btn btn-success" data-toggle="modal" data-target="#lessons-editor">
                                    <react_intl_1.FormattedMessage id="add" defaultMessage={"Add"}/><span className="glyphicon-plus"></span>
                                </button>
                                <LessonsList lessons={this.state.lessons} loading={this.loading}/>
                            </div>
                        </div>
                    </div>
                </div>
                <LessonsEditor_1.default />
            </div>);
    }
}
exports.LessonsBox = LessonsBox;
class LessonsList extends React.Component {
    render() {
        const subListNodes = this.props.lessons.map(entry => {
            return (<LessonsSubList key={entry.languages.toString()} languages={entry.languages} lessons={entry.lessons}/>);
        });
        return (<div id="lessons-list">
                {this.props.loading || subListNodes}
            </div>);
    }
}
exports.LessonsList = LessonsList;
class LessonsSubList extends React.Component {
    render() {
        const rows = this.props.lessons.map(assignment => {
            return (<tr key={assignment.id}>
                    <td>{assignment.lesson.reading}</td>
                    <td>{assignment.lesson.pronunciation}</td>
                    <td>{assignment.lesson.meaning}</td>
                    <td><react_intl_1.FormattedNumber value={assignment.stage}/></td>
                    <td><react_intl_1.FormattedRelative value={assignment.nextReview}/></td>
                </tr>);
        });
        return (<div className="lessons-sub-list">
                <h4><react_intl_1.FormattedMessage id="languagePair" defaultMessage="{languageFrom} ({countryFrom}) - {languageTo} ({countryTo})" values={{
            languageFrom: this.props.languages.languageFrom.englishName,
            languageTo: this.props.languages.languageTo.englishName,
            countryFrom: this.props.languages.languageFrom.englishCountryName,
            countryTo: this.props.languages.languageTo.englishCountryName
        }}/></h4>
                <table className="table table-hover table-responsive bg-primary">
                    <thead>
                        <tr>
                            <th><react_intl_1.FormattedMessage id="reading" defaultMessage="Reading" description="test"/></th>
                            <th><react_intl_1.FormattedMessage id="pronunciation" defaultMessage={"Pronunciation"}/></th>
                            <th><react_intl_1.FormattedMessage id="meaning" defaultMessage={"Meaning"}/></th>
                            <th><react_intl_1.FormattedMessage id="currentGrade" defaultMessage={"Current Grade"}/></th>
                            <th><react_intl_1.FormattedMessage id="nextReview" defaultMessage={"Next Review"}/></th>
                        </tr>
                    </thead>
                    <tbody>
                        {rows}
                    </tbody>
                </table>
            </div>);
    }
}
exports.LessonsSubList = LessonsSubList;
exports.default = LessonsBox;
//# sourceMappingURL=LessonsBox.jsx.map