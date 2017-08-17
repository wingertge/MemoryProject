webpackJsonp([0],{

/***/ 100:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function($) {
Object.defineProperty(exports, "__esModule", { value: true });
const React = __webpack_require__(6);
const ReactDOM = __webpack_require__(66);
const ReactIntl = __webpack_require__(22);
const LocaleLoader_1 = __webpack_require__(56);
const Home_1 = __webpack_require__(220);
const en = __webpack_require__(235);
var IntlProvider = ReactIntl.IntlProvider;
const bootstrap = __webpack_require__(236); //only for webpack
const appCreateDOM = (messages, css, themeData) => {
    function camelCaseToDash(myStr) {
        return myStr.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
    }
    const replaceAll = (target, search, replacement) => {
        function escapeRegExp(str) {
            return str.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
        }
        return target.replace(new RegExp(escapeRegExp(search), 'g'), replacement);
    };
    var theme = themeData[2].responseJSON.result;
    var temp = bootstrap;
    ReactIntl.addLocaleData(en);
    var fixedCss = css[2].responseText;
    for (var property in theme) {
        if (theme.hasOwnProperty(property)) {
            fixedCss = replaceAll(fixedCss, `"$${camelCaseToDash(property)}"`, theme[property]);
        }
    }
    var style = $(`<style>${fixedCss}</style>`);
    $("head").append(style);
    ReactDOM.render(React.createElement(IntlProvider, { locale: LocaleLoader_1.default.getCurrentLocale(), messages: messages[2].responseJSON },
        React.createElement(Home_1.default, null)), document.getElementById("content"));
};
var p1 = LocaleLoader_1.default.getStrings();
var p2 = $.get("/css/main.build.css", null, $.noop, "text");
var p3 = $.getJSON("/api/themes/current-theme");
$.when(p1, p2, p3).then(appCreateDOM).fail(() => console.log("initialisation error"));

/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(14)))

/***/ }),

/***/ 199:
/***/ (function(module, exports) {

/* (ignored) */

/***/ }),

/***/ 207:
/***/ (function(module, exports) {

/* (ignored) */

/***/ }),

/***/ 214:
/***/ (function(module, exports) {

/* (ignored) */

/***/ }),

/***/ 220:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function($) {
Object.defineProperty(exports, "__esModule", { value: true });
const React = __webpack_require__(6);
const react_intl_1 = __webpack_require__(22);
const LessonsBox_1 = __webpack_require__(221);
const ErrorBox_1 = __webpack_require__(233);
const react_loading_1 = __webpack_require__(37);
const Models_1 = __webpack_require__(57);
const LocaleLoader_1 = __webpack_require__(56);
const ReviewBox_1 = __webpack_require__(234);
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

/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(14)))

/***/ }),

/***/ 221:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function($) {
Object.defineProperty(exports, "__esModule", { value: true });
const React = __webpack_require__(6);
const react_intl_1 = __webpack_require__(22);
const Models_1 = __webpack_require__(57);
const LessonsEditor_1 = __webpack_require__(222);
const linq_1 = __webpack_require__(231);
const react_loading_1 = __webpack_require__(37);
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

/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(14)))

/***/ }),

/***/ 222:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function($) {
Object.defineProperty(exports, "__esModule", { value: true });
const React = __webpack_require__(6);
const Select = __webpack_require__(35);
const react_loading_1 = __webpack_require__(37);
const Util_1 = __webpack_require__(98);
const react_intl_1 = __webpack_require__(22);
const LocaleLoader_1 = __webpack_require__(56);
const Models_1 = __webpack_require__(57);
class LessonEditorState {
    constructor() {
        this.formModel = new Models_1.LessonsEditorModel();
        this.languagesFrom = [];
        this.languagesTo = [];
        this.readingOptions = new SelectOptionsEntry();
        this.pronunciationOptions = new SelectOptionsEntry();
        this.meaningOptions = new SelectOptionsEntry();
    }
}
class SelectOptionsEntry {
    constructor() {
        this.cache = {};
        this.options = [];
    }
}
class LessonEditorInitialState {
}
var EditorField;
(function (EditorField) {
    EditorField[EditorField["Reading"] = 0] = "Reading";
    EditorField[EditorField["Pronunciation"] = 1] = "Pronunciation";
    EditorField[EditorField["Meaning"] = 2] = "Meaning";
})(EditorField || (EditorField = {}));
class LessonsEditor extends React.Component {
    constructor() {
        super();
        this.loadingComponent = React.createElement("div", { className: "d-flex flex-row justify-content-center" },
            React.createElement(react_loading_1.default, { type: "spinningBubbles", color: "#ccc", delay: 100 }));
        this.loading = this.loadingComponent;
        this.myRefs = {
            readingField: null,
            pronunciationField: null,
            meaningField: null
        };
        this.changeHandlers = {
            languageFrom: (e) => {
                this.state.formModel.languageFromId = e.value;
                this.setState(this.state);
            },
            languageTo: (e) => {
                this.state.formModel.languageToId = e.value;
                this.setState(this.state);
            },
            reading: (e) => {
                this.state.formModel.reading = e.value;
                this.setState(this.state);
            },
            pronunciation: (e) => {
                this.state.formModel.pronunciation = e.value;
                this.setState(this.state);
            },
            meaning: (e) => {
                this.state.formModel.meaning = e.value;
                this.setState(this.state);
            }
        };
        this.state = (this.props && this.props.initialState) || new LessonEditorState();
        this.submit = this.submit.bind(this);
        this.loadAutocomplete = this.loadAutocomplete.bind(this);
    }
    componentDidMount() {
        $("#lessons-editor").on("shown.bs.modal", this.loadStateFromServer.bind(this));
        $("#lessons-editor").on("hidden.bs.modal", this.resetComponent.bind(this));
    }
    resetComponent() {
        this.loading = this.loadingComponent;
        this.forceUpdate();
    }
    loadStateFromServer() {
        $.getJSON(`/api/lessons/language-list/${this.state.formModel.id}`, {}, data => {
            const newState = (this.props && this.props.initialState) || new LessonEditorState();
            const pairs = data.result;
            pairs.from.forEach((value, key) => {
                newState.languagesFrom[key] = value;
            });
            pairs.to.forEach((value, key) => {
                newState.languagesTo[key] = value;
            });
            newState.formModel.languageFromId = pairs.from[0].id;
            newState.formModel.languageToId = pairs.to[0].id;
            this.loading = null;
            this.setState(newState);
        });
    }
    formatLangOptions(languages) {
        return languages.map((language) => {
            return ({ value: language.id, label: LocaleLoader_1.default.format(this.props.intl, language.englishName, language.ietfTag) });
        });
    }
    loadAutocomplete(value, field, context, cb) {
        if (context.cache[value])
            return context.cache[value];
        var requestObj = $.ajax({
            url: "/api/lessons/editor-autocomplete",
            data: {
                query: value,
                type: field,
                langFromId: this.state.formModel.languageFromId,
                langToId: this.state.formModel.languageToId
            },
            success: json => {
                var result = json.result;
                result.add({ value: value, label: value });
                context.cache[value] = result;
                context.options = result;
                cb(context);
            },
            error: a => {
                console.error(a);
                context.options = [{ value: value, label: value }];
                cb(context);
            }
        });
    }
    renderPreview(value, componentClass) {
        const bestMatch = $(`.${componentClass} .Select-option`);
        const completedString = (bestMatch.length > 1 && bestMatch[1] && bestMatch[1].children.toString()) || value;
        return (React.createElement("p", null,
            React.createElement("span", null, value),
            React.createElement("span", { className: "text-muted" }, completedString.substring(value.length))));
    }
    objectifyForm(formArray) {
        const returnArray = {};
        for (let i = 0; i < formArray.length; i++) {
            returnArray[formArray[i]["name"]] = formArray[i]["value"];
        }
        return returnArray;
    }
    submit(event) {
        event.preventDefault();
        Util_1.default.postJson("/api/lessons/put", this.state.formModel, data => {
            const result = data;
            if (result.succeeded) {
                this.state.formModel.reading = "";
                this.state.formModel.pronunciation = "";
                this.state.formModel.meaning = "";
                this.setState(this.state);
                $("#lessons-editor .focus-on-submit").focus();
            }
        });
    }
    form() {
        const intl = this.props.intl;
        return (React.createElement("form", { id: "lessons-editor-form", action: "#" },
            React.createElement("input", { type: "hidden", name: "id", value: this.state.formModel.id }),
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col-sm-6 d-inline-flex" },
                    React.createElement("label", null,
                        React.createElement(react_intl_1.FormattedMessage, { id: "from", defaultMessage: "From" })),
                    React.createElement(Select, { className: "country-select", name: "languageFrom", value: this.state.formModel
                            .languageFromId, onChange: this.changeHandlers.languageFrom, options: this.formatLangOptions(this.state.languagesFrom), openOnFocus: true, clearable: false })),
                React.createElement("div", { className: "col-sm-6 d-inline-flex" },
                    React.createElement("label", null,
                        React.createElement(react_intl_1.FormattedMessage, { id: "to", defaultMessage: "To" })),
                    React.createElement(Select, { className: "country-select", name: "languageTo", value: this.state.formModel.languageToId, onChange: this.changeHandlers.languageTo, options: this.formatLangOptions(this.state.languagesTo), clearable: false, openOnFocus: true }))),
            React.createElement("div", { className: "row padding" },
                React.createElement("div", { className: "col-sm-12 text-center" },
                    React.createElement(Select, { name: "reading", placeholder: LocaleLoader_1.default.format(intl, "Reading"), required: true, className: "focus-on-submit reading", autofocus: true, value: this.state.formModel.reading, valueRenderer: value => this.renderPreview(value.label, "reading"), onChange: this.changeHandlers.reading, onInputChange: value => this.loadAutocomplete(value, EditorField.Reading, this.state.readingOptions, (a) => this.setState({ readingOptions: a })), arrowRenderer: (props) => React.createElement("span", null), onBlur: (e) => this.changeHandlers.reading(e.target), ignoreAccents: false, ignoreCase: false, ref: (a) => this.myRefs.readingField = a, options: this.state.readingOptions.options }))),
            React.createElement("div", { className: "row padding" },
                React.createElement("div", { className: "col-sm-12 text-center" },
                    React.createElement(Select, { name: "pronunciation", placeholder: LocaleLoader_1.default.format(intl, "Pronunciation"), className: "pronunciation", value: this.state.formModel.pronunciation, onChange: this.changeHandlers
                            .pronunciation, valueRenderer: value => this.renderPreview(value.label, "pronunciation"), onInputChange: value => this.loadAutocomplete(value, EditorField.Pronunciation, this.state.pronunciationOptions, (a) => this.setState({ pronunciationOptions: a })), arrowRenderer: (props) => React.createElement("span", null), onBlur: (e) => this.changeHandlers.pronunciation(e.target), ignoreAccents: false, ignoreCase: false, ref: (a) => this.myRefs.pronunciationField = a, options: this.state.pronunciationOptions.options }))),
            React.createElement("div", { className: "row padding" },
                React.createElement("div", { className: "col-sm-12 text-center" },
                    React.createElement(Select, { name: "meaning", placeholder: LocaleLoader_1.default.format(intl, "Meaning"), required: true, className: "meaning", value: this.state.formModel.meaning, onChange: this.changeHandlers.meaning, valueRenderer: value => this.renderPreview(value.label, "meaning"), onInputChange: value => this.loadAutocomplete(value, EditorField.Meaning, this.state.meaningOptions, (a) => this.setState({ meaningOptions: a })), arrowRenderer: (props) => React.createElement("span", null), onBlur: (e) => this.changeHandlers.meaning(e.target), options: this.state.meaningOptions.options, ignoreAccents: false, ignoreCase: false, ref: (a) => this.myRefs.meaningField = a }))),
            React.createElement("div", { className: "row button-row" },
                React.createElement("div", { className: "col-sm-6" },
                    React.createElement("button", { type: "button", className: "btn btn-lg btn-primary", onClick: this.submit },
                        React.createElement(react_intl_1.FormattedMessage, { id: "save", defaultMessage: "Save" }))),
                React.createElement("div", { className: "col-sm-6 text-right" },
                    React.createElement("button", { type: "cancel", className: "btn btn-lg btn-warning", "data-dismiss": "modal", "data-target": "#lessons-editor" },
                        React.createElement(react_intl_1.FormattedMessage, { id: "cancel", defaultMessage: "Cancel" }))))));
    }
    render() {
        return (React.createElement("div", { id: "lessons-editor", className: "modal fade" },
            React.createElement("div", { className: "modal-dialog" },
                React.createElement("div", { className: "modal-content" },
                    React.createElement("div", { className: "modal-header" },
                        React.createElement("button", { type: "button", className: "close", "data-dismiss": "modal", "data-target": "#lessons-editor" }, "\u00D7"),
                        React.createElement("h4", { className: "modal-title" }, "New Lesson")),
                    React.createElement("div", { className: "modal-body" }, this.loading || this.form())))));
    }
}
exports.default = react_intl_1.injectIntl(LessonsEditor);

/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(14)))

/***/ }),

/***/ 233:
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
const React = __webpack_require__(6);
const react_intl_1 = __webpack_require__(22);
class ErrorBox extends React.Component {
    render() {
        return (React.createElement("div", { className: "modal fade", id: "error-modal" },
            React.createElement("div", { className: "modal-dialog" },
                React.createElement("div", { className: "modal-content" },
                    React.createElement("div", { className: "modal-header" },
                        React.createElement("h5", { className: "modal-title" },
                            React.createElement(react_intl_1.FormattedMessage, { id: "errorHeader", defaultMessage: "An Error occured" })),
                        React.createElement("button", { type: "button", className: "close", "data-dismiss": "modal", "aria-label": "Close" },
                            React.createElement("span", { "aria-hidden": "true" }, "\u00D7"))),
                    React.createElement("div", { className: "modal-body" }),
                    React.createElement("div", { className: "modal-footer" },
                        React.createElement("button", { type: "button", className: "btn primary", "data-dismiss": "modal", "data-target": "#error-modal" },
                            React.createElement(react_intl_1.FormattedMessage, { id: "errorConfirm", defaultMessage: "Close" })))))));
    }
}
exports.default = ErrorBox;


/***/ }),

/***/ 234:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function($) {
Object.defineProperty(exports, "__esModule", { value: true });
const React = __webpack_require__(6);
const react_intl_1 = __webpack_require__(22);
const react_loading_1 = __webpack_require__(37);
const Util_1 = __webpack_require__(98);
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

/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(14)))

/***/ }),

/***/ 238:
/***/ (function(module, exports) {

// removed by extract-text-webpack-plugin

/***/ }),

/***/ 56:
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
const $ = __webpack_require__(14);
class I18n {
    static getCurrentLocale() {
        return "en-US";
    }
    static getStrings() {
        const url = `/lang/${this.getCurrentLocale()}.json`;
        return $.getJSON(url);
    }
    static init() {
    }
    static format(intl, defaultMessage, key, values) {
        return intl.formatMessage({
            id: key || defaultMessage.toLocaleLowerCase(),
            defaultMessage: defaultMessage
        });
    }
}
exports.default = I18n;


/***/ }),

/***/ 57:
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var ReviewField;
(function (ReviewField) {
    ReviewField[ReviewField["Reading"] = 0] = "Reading";
    ReviewField[ReviewField["Meaning"] = 1] = "Meaning";
    ReviewField[ReviewField["Pronunciation"] = 2] = "Pronunciation";
})(ReviewField = exports.ReviewField || (exports.ReviewField = {}));
class AudioLocation {
    constructor() {
        this.relFileName = "";
    }
}
exports.AudioLocation = AudioLocation;
class Language {
    constructor() {
        this.id = 0;
        this.englishName = "";
        this.nativeName = "";
        this.englishCountryName = "";
        this.nativeCountryName = "";
        this.ietfTag = "";
    }
}
exports.Language = Language;
class Lesson {
    constructor() {
        this.id = "00000000-0000-0000-0000-000000000000";
        this.languageFrom = new Language();
        this.languageTo = new Language();
        this.audio = [];
        this.reading = "";
        this.pronunciation = "";
        this.meaning = "";
    }
}
exports.Lesson = Lesson;
class LessonAssignment {
    constructor() {
        this.id = "00000000-0000-0000-0000-000000000000";
        this.lesson = new Lesson();
        this.stage = 0;
        this.nextReview = new Date(0);
    }
}
exports.LessonAssignment = LessonAssignment;
class LessonsEditorModel {
    constructor() {
        this.id = "00000000-0000-0000-0000-000000000000";
        this.languageFromId = 0;
        this.languageToId = 0;
        this.reading = "";
        this.pronunciation = "";
        this.meaning = "";
    }
}
exports.LessonsEditorModel = LessonsEditorModel;
class Review {
    constructor() {
        this.id = "00000000-0000-0000-0000-000000000000";
        this.lesson = new LessonAssignment();
        this.reviewDone = new Date(0);
        this.wrongAnswers = 0;
        this.startStage = 0;
        this.endStage = 0;
    }
}
exports.Review = Review;
class User {
    constructor() {
        this.id = "00000000-0000-0000-0000-000000000000";
        this.userName = "";
        this.email = "";
        this.signedUp = new Date(0);
        this.lastLogin = new Date(0);
        this.premiumUntil = new Date(0);
        this.normalizedUserName = "";
        this.normalizedEmail = "";
        this.emailConfirmed = false;
        this.phoneNumber = "";
        this.phoneNumberConfirmed = false;
    }
}
exports.User = User;
class LanguagePair {
    constructor(languageFrom, languageTo) {
        this.languageFrom = languageFrom;
        this.languageTo = languageTo;
    }
    toString() {
        return `${this.languageFrom.ietfTag}-${this.languageTo.ietfTag}`;
    }
}
exports.LanguagePair = LanguagePair;
class ActionResult {
}
exports.ActionResult = ActionResult;
class ReviewModel {
    constructor() {
        this.assignmentId = "00000000-0000-0000-0000-000000000000";
        this.fieldFrom = ReviewField.Meaning;
        this.fieldTo = ReviewField.Pronunciation;
        this.from = "";
        this.to = "";
        this.solution = "";
    }
}
exports.ReviewModel = ReviewModel;
class Theme {
}
exports.Theme = Theme;


/***/ }),

/***/ 98:
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
const $ = __webpack_require__(14);
class Util {
    static postJson(url, body, success) {
        return $.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify(body),
            contentType: "application/json",
            dataType: 'json',
            success: success
        });
    }
}
exports.Util = Util;
exports.default = Util;


/***/ }),

/***/ 99:
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(100);
module.exports = __webpack_require__(238);


/***/ })

},[99]);
//# sourceMappingURL=main.build.js.map