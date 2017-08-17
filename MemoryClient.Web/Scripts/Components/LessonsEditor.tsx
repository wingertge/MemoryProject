import * as React from "react";
import * as Select from "react-select";
import ReactLoading from "react-loading";
import Util from "../Util";
import { FormattedMessage, InjectedIntlProps, injectIntl } from "react-intl";
import I18n from "../LocaleLoader";
import { Language, ActionResult, LessonsEditorModel } from "../Models";

class LessonEditorState {
    formModel = new LessonsEditorModel();
    languagesFrom: Language[] = [];
    languagesTo: Language[] = [];
    readingOptions = new SelectOptionsEntry();
    pronunciationOptions = new SelectOptionsEntry();
    meaningOptions = new SelectOptionsEntry();
}

class SelectOptionsEntry {
    cache: { [key: string]: Select.Option[] } = {};
    options: Select.Option[] = [];
}

class LessonEditorInitialState {
    initialState?: LessonEditorState;
}

enum EditorField {
    Reading,
    Pronunciation,
    Meaning
}

class LessonsEditor extends React.Component<LessonEditorInitialState & InjectedIntlProps, LessonEditorState> {
    constructor() {
        super();
        this.state = (this.props && this.props.initialState) || new LessonEditorState();
        this.submit = this.submit.bind(this);
        this.loadAutocomplete = this.loadAutocomplete.bind(this);
    }

    componentDidMount() {
        $("#lessons-editor").on("shown.bs.modal", this.loadStateFromServer.bind(this));
        $("#lessons-editor").on("hidden.bs.modal", this.resetComponent.bind(this))
    }

    loadingComponent: JSX.Element = <div className="d-flex flex-row justify-content-center"><ReactLoading type="spinningBubbles" color="#ccc" delay={100} /></div>;
    loading = this.loadingComponent;

    resetComponent() {
        this.loading = this.loadingComponent;
        this.forceUpdate();
    }

    loadStateFromServer() {
        $.getJSON(`/api/lessons/language-list/${this.state.formModel.id}`, {}, data => {
            const newState = (this.props && this.props.initialState) || new LessonEditorState();
            const pairs = data.result as { from: Language[], to: Language[] };
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

    formatLangOptions(languages: Language[]) {
        return languages.map((language) => {
            return (
                { value: language.id, label: I18n.format(this.props.intl, language.englishName, language.ietfTag)}
            );
        });
    }

    loadAutocomplete(value: string, field: EditorField, context: SelectOptionsEntry, cb: (SelectOptionsEntry) => void) {
        if (context.cache[value]) return context.cache[value];
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

    renderPreview(value: string, componentClass: string): JSX.Element {
        const bestMatch = $(`.${componentClass} .Select-option`);
        const completedString = (bestMatch.length > 1 && bestMatch[1] && bestMatch[1].children.toString()) || value;
        return (
            <p><span>{value}</span><span className="text-muted">{completedString.substring(value.length)}</span></p>
        );
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
        Util.postJson("/api/lessons/put",
            this.state.formModel,
            data => {
                const result = data as ActionResult;
                if (result.succeeded) {
                    this.state.formModel.reading = "";
                    this.state.formModel.pronunciation = "";
                    this.state.formModel.meaning = "";
                    this.setState(this.state);
                    $("#lessons-editor .focus-on-submit").focus();
                }
            });
    }

    myRefs: { [key: string]: any } = {
        readingField: null,
        pronunciationField: null,
        meaningField: null
    }

    changeHandlers = {
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
    }

    form(): JSX.Element {
        const intl = this.props.intl;
        return (
            <form id="lessons-editor-form" action="#">
                    <input type="hidden" name="id" value={this.state.formModel.id}/>
                    <div className="row">
                        <div className="col-sm-6 d-inline-flex">
                            <label><FormattedMessage id="from" defaultMessage="From"/></label>
                            <Select className="country-select" name="languageFrom" value={this.state.formModel
                            .languageFromId} onChange={this.changeHandlers.languageFrom} options={this.formatLangOptions(this.state.languagesFrom)}
                                    openOnFocus={true} clearable={false}/>
                        </div>
                        <div className="col-sm-6 d-inline-flex">
                            <label><FormattedMessage id="to" defaultMessage="To"/></label>
                            <Select className="country-select" name="languageTo" value={this.state.formModel.languageToId} onChange={this.changeHandlers.languageTo} options={this.formatLangOptions(this.state.languagesTo)}
                                    clearable={false} openOnFocus={true}/>
                        </div>
                    </div>
                    <div className="row padding">
                        <div className="col-sm-12 text-center">
                            <Select name="reading" placeholder={I18n.format(intl, "Reading")} required={true} className="focus-on-submit reading" autofocus={true}
                                          value={this.state.formModel.reading} valueRenderer={value => this.renderPreview(value.label, "reading")} onChange={this.changeHandlers.reading}
                                          onInputChange={value => this.loadAutocomplete(value, EditorField.Reading, this.state.readingOptions, (a) => this.setState({readingOptions: a}))}
                                          arrowRenderer={(props) => <span></span>} onBlur={(e) => this.changeHandlers.reading(e.target)}
                                          ignoreAccents={false} ignoreCase={false} ref={(a) => this.myRefs.readingField = a} options={this.state.readingOptions.options}/>
                        </div>
                    </div>
                    <div className="row padding">
                        <div className="col-sm-12 text-center">
                            <Select name="pronunciation" placeholder={I18n.format(intl, "Pronunciation")} className="pronunciation"
                                          value={this.state.formModel.pronunciation} onChange={this.changeHandlers
                                          .pronunciation} valueRenderer={value => this.renderPreview(value.label, "pronunciation")}
                                          onInputChange={value => this.loadAutocomplete(value, EditorField.Pronunciation, this.state.pronunciationOptions, (a) => this.setState({pronunciationOptions: a}))}
                                          arrowRenderer={(props) => <span></span>} onBlur={(e) => this.changeHandlers.pronunciation(e.target)}
                                          ignoreAccents={false} ignoreCase={false} ref={(a) => this.myRefs.pronunciationField = a} options={this.state.pronunciationOptions.options}/>
                        </div>
                    </div>
                    <div className="row padding">
                        <div className="col-sm-12 text-center">
                            <Select name="meaning" placeholder={I18n.format(intl, "Meaning")} required={true} className="meaning"
                                          value={this.state.formModel.meaning} onChange={this.changeHandlers.meaning} valueRenderer={value => this.renderPreview(value.label, "meaning")}
                                          onInputChange={value => this.loadAutocomplete(value, EditorField.Meaning, this.state.meaningOptions, (a) => this.setState({meaningOptions: a}))}
                                          arrowRenderer={(props) => <span></span>} onBlur={(e) => this.changeHandlers.meaning(e.target)} options={this.state.meaningOptions.options}
                                          ignoreAccents={false} ignoreCase={false} ref={(a) => this.myRefs.meaningField = a}/>
                        </div>
                    </div>
                    <div className="row button-row">
                        <div className="col-sm-6">
                            <button type="button" className="btn btn-lg btn-primary" onClick={this.submit}><FormattedMessage id="save" defaultMessage="Save"/></button>
                        </div>
                        <div className="col-sm-6 text-right">
                            <button type="cancel" className="btn btn-lg btn-warning" data-dismiss="modal" data-target="#lessons-editor"><FormattedMessage id="cancel" defaultMessage="Cancel"/></button>
                        </div>
                    </div>
            </form>);
    }

    render(): JSX.Element {
        return (
            <div id="lessons-editor" className="modal fade">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <button type="button" className="close" data-dismiss="modal" data-target="#lessons-editor">&times;</button>
                            <h4 className="modal-title">New Lesson</h4>
                        </div>
                        <div className="modal-body">
                            {this.loading || this.form()}
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default injectIntl(LessonsEditor);