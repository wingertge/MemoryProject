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
        this.loading = React.createElement(react_loading_1.default, { type: "bubbles", color: "#444", className: "loading-bubbles" });
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
        return (React.createElement("div", { id: "lessons-box" },
            React.createElement("div", { className: "modal fade", role: "dialog", id: "lessons-box-modal" },
                React.createElement("div", { className: "modal-dialog modal-lg" },
                    React.createElement("div", { className: "modal-content" },
                        React.createElement("div", { className: "modal-header" },
                            React.createElement("button", { type: "button", className: "close", "data-dismiss": "modal", "data-target": "#lessons-box-modal" }, "\u00D7"),
                            React.createElement("h4", { className: "modal-title" },
                                React.createElement(react_intl_1.FormattedMessage, { id: "lessonsLabel", defaultMessage: "Lessons" }))),
                        React.createElement("div", { className: "modal-body" },
                            React.createElement("button", { type: "button", id: "lessons-add-btn", className: "btn btn-success", "data-toggle": "modal", "data-target": "#lessons-editor" },
                                React.createElement(react_intl_1.FormattedMessage, { id: "add", defaultMessage: "Add" }),
                                React.createElement("span", { className: "glyphicon-plus" })),
                            React.createElement(LessonsList, { lessons: this.state.lessons, loading: this.loading }))))),
            React.createElement(LessonsEditor_1.default, null)));
    }
}
exports.LessonsBox = LessonsBox;
class LessonsList extends React.Component {
    render() {
        const subListNodes = this.props.lessons.map(entry => {
            return (React.createElement(LessonsSubList, { key: entry.languages.toString(), languages: entry.languages, lessons: entry.lessons }));
        });
        return (React.createElement("div", { id: "lessons-list" }, this.props.loading || subListNodes));
    }
}
exports.LessonsList = LessonsList;
class LessonsSubList extends React.Component {
    getFormattedTime(time) {
        if (time < new Date(Date.now()))
            return (React.createElement(react_intl_1.FormattedMessage, { id: "rightNow", defaultMessage: "Right Now" }));
        return (React.createElement(react_intl_1.FormattedRelative, { value: time }));
    }
    render() {
        const rows = this.props.lessons.map(assignment => {
            return (React.createElement("tr", { key: assignment.id },
                React.createElement("td", null, assignment.lesson.reading),
                React.createElement("td", null, assignment.lesson.pronunciation),
                React.createElement("td", null, assignment.lesson.meaning),
                React.createElement("td", null,
                    React.createElement(react_intl_1.FormattedNumber, { value: assignment.stage })),
                React.createElement("td", null, this.getFormattedTime(assignment.nextReview))));
        });
        return (React.createElement("div", { className: "lessons-sub-list" },
            React.createElement("h4", null,
                React.createElement(react_intl_1.FormattedMessage, { id: "languagePair", defaultMessage: "{languageFrom} ({countryFrom}) - {languageTo} ({countryTo})", values: {
                        languageFrom: this.props.languages.languageFrom.englishName,
                        languageTo: this.props.languages.languageTo.englishName,
                        countryFrom: this.props.languages.languageFrom.englishCountryName,
                        countryTo: this.props.languages.languageTo.englishCountryName
                    } })),
            React.createElement("table", { className: "table table-hover table-responsive table-bordered table-sm" },
                React.createElement("thead", { className: "bg-primary" },
                    React.createElement("tr", null,
                        React.createElement("th", null,
                            React.createElement(react_intl_1.FormattedMessage, { id: "reading", defaultMessage: "Reading", description: "test" })),
                        React.createElement("th", null,
                            React.createElement(react_intl_1.FormattedMessage, { id: "pronunciation", defaultMessage: "Pronunciation" })),
                        React.createElement("th", null,
                            React.createElement(react_intl_1.FormattedMessage, { id: "meaning", defaultMessage: "Meaning" })),
                        React.createElement("th", null,
                            React.createElement(react_intl_1.FormattedMessage, { id: "currentStage", defaultMessage: "Stage" })),
                        React.createElement("th", { style: { width: "8rem" } },
                            React.createElement(react_intl_1.FormattedMessage, { id: "nextReview", defaultMessage: "Next Review" })))),
                React.createElement("tbody", null, rows))));
    }
}
exports.LessonsSubList = LessonsSubList;
exports.default = LessonsBox;
//# sourceMappingURL=LessonsBox.js.map