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
//# sourceMappingURL=Models.js.map