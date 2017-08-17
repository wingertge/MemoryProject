"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const Select = require("react-select");
const react_loading_1 = require("react-loading");
const Util_1 = require("../Util");
const react_intl_1 = require("react-intl");
const LocaleLoader_1 = require("../LocaleLoader");
const Models_1 = require("../Models");
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
//# sourceMappingURL=LessonsEditor.js.map