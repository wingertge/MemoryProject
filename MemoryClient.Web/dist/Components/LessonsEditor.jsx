"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const Select = require("react-select");
const Util_1 = require("../Util");
const react_intl_1 = require("react-intl");
const LocaleLoader_1 = require("../LocaleLoader");
const Models_1 = require("../Models");
class LessonEditorState {
    constructor() {
        this.formModel = new Models_1.LessonsEditorModel();
        this.languagesFrom = [];
        this.languagesTo = [];
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
    }
    componentDidMount() {
        this.loadStateFromServer();
    }
    loadStateFromServer() {
        $.getJSON(`/api/lessons/language-list/${this.state.formModel.id}`, {}, data => {
            const newState = (this.props && this.props.initialState) || new LessonEditorState();
            const pairs = data;
            pairs.from.forEach((value, key) => {
                newState.languagesFrom[key] = value;
            });
            pairs.to.forEach((value, key) => {
                newState.languagesTo[key] = value;
            });
            newState.formModel.languageFromId = pairs.from[0].id;
            newState.formModel.languageToId = pairs.to[0].id;
            this.setState(newState);
        });
    }
    formatLangOptions(languages) {
        return languages.map((language) => {
            return ({ value: language.id, label: LocaleLoader_1.default.format(this.props.intl, language.englishName, language.ietfTag) });
        });
    }
    loadAutocomplete(value, field) {
        return Promise.resolve($.getJSON(`/common/autocomplete?query=${value}&type=LessonsEditor.${EditorField[field]}`, json => {
            json.unshift({ value: value, label: value });
            return { options: json };
        }).catch(a => {
            console.error(a);
            return { options: [{ value: value, label: value }] };
        }));
    }
    renderPreview(value, componentClass) {
        const bestMatch = $(`.${componentClass} .Select-option`);
        const completedString = (bestMatch.length > 1 && bestMatch[1] && bestMatch[1].children.toString()) || value;
        return (<p><span>{value}</span><span className="text-muted">{completedString.substring(value.length)}</span></p>);
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
    render() {
        const intl = this.props.intl;
        return (<div id="lessons-editor" className="modal fade">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <button type="button" className="close" data-dismiss="modal" data-target="#lessons-editor">&times;</button>
                            <h4 className="modal-title">New Lesson</h4>
                        </div>
                        <div className="modal-body">
                            <form id="lessons-editor-form" action="#">
                                <fieldset>
                                    <input type="hidden" name="id" value={this.state.formModel.id}/>
                                    <div className="row">
                                        <div className="col-sm-6 flex-row">
                                            <label><react_intl_1.FormattedMessage id="from" defaultMessage="From"/></label>
                                            <Select className="country-select" name="languageFrom" value={this.state.formModel.languageFromId} onChange={this.changeHandlers.languageFrom} options={this.formatLangOptions(this.state.languagesFrom)} openOnFocus={true} clearable={false}/>
                                        </div>
                                        <div className="col-sm-6 flex-row">
                                            <label><react_intl_1.FormattedMessage id="to" defaultMessage="To"/></label>
                                            <Select className="country-select" name="languageTo" value={this.state.formModel.languageToId} onChange={this.changeHandlers.languageTo} options={this.formatLangOptions(this.state.languagesTo)} clearable={false} openOnFocus={true}/>
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-sm-12 text-center">
                                            <Select.Async name="reading" placeholder={LocaleLoader_1.default.format(intl, "Reading")} required={true} className="focus-on-submit reading" autofocus={true} autoload={false} value={this.state.formModel.reading} valueRenderer={value => this.renderPreview(value.label, "reading")} onChange={this.changeHandlers.reading} loadOptions={value => this.loadAutocomplete(value, EditorField.Reading)} arrowRenderer={(props) => <span></span>} onBlur={(e) => this.changeHandlers.reading(e.target)} ignoreAccents={false} ignoreCase={false}/>
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-sm-12 text-center">
                                            <Select.Async name="pronunciation" placeholder={LocaleLoader_1.default.format(intl, "Pronunciation")} className="pronunciation" autoload={false} value={this.state.formModel.pronunciation} onChange={this.changeHandlers.pronunciation} valueRenderer={value => this.renderPreview(value.label, "pronunciation")} loadOptions={value => this.loadAutocomplete(value, EditorField.Pronunciation)} arrowRenderer={(props) => <span></span>} onBlur={(e) => this.changeHandlers.pronunciation(e.target)} ignoreAccents={false} ignoreCase={false}/>
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-sm-12 text-center">
                                            <Select.Async name="meaning" placeholder={LocaleLoader_1.default.format(intl, "Meaning")} required={true} autoload={false} className="meaning" value={this.state.formModel.meaning} onChange={this.changeHandlers.meaning} valueRenderer={value => this.renderPreview(value.label, "meaning")} loadOptions={value => this.loadAutocomplete(value, EditorField.Meaning)} arrowRenderer={(props) => <span></span>} onBlur={(e) => this.changeHandlers.meaning(e.target)} ignoreAccents={false} ignoreCase={false}/>
                                        </div>
                                    </div>
                                    <div className="row button-row">
                                        <div className="col-sm-6">
                                            <button type="button" className="btn btn-lg btn-primary" onClick={this.submit}><react_intl_1.FormattedMessage id="save" defaultMessage="Save"/></button>
                                        </div>
                                        <div className="col-sm-6 text-right">
                                            <button type="cancel" className="btn btn-lg btn-warning" data-dismiss="modal" data-target="#lessons-editor"><react_intl_1.FormattedMessage id="cancel" defaultMessage="Cancel"/></button>
                                        </div>
                                    </div>
                                </fieldset>
                            </form>
                        </div>
                    </div>
                </div>
            </div>);
    }
}
exports.default = react_intl_1.injectIntl(LessonsEditor);
//# sourceMappingURL=LessonsEditor.jsx.map