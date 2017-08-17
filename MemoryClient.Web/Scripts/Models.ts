export enum ReviewField {
    Reading,
    Meaning,
    Pronunciation
}

export class AudioLocation {
    relFileName: string = "";
}

export class Language {
    id: number = 0;
    englishName: string = "";
    nativeName: string = "";
    englishCountryName: string = "";
    nativeCountryName: string = "";
    ietfTag: string = "";
}

export class Lesson {
    id: string = "00000000-0000-0000-0000-000000000000";
    languageFrom = new Language();
    languageTo = new Language();
    audio: AudioLocation[] = [];
    reading: string = "";
    pronunciation: string = "";
    meaning: string = "";
}

export class LessonAssignment {
    id: string = "00000000-0000-0000-0000-000000000000";
    lesson = new Lesson();
    stage: number = 0;
    nextReview = new Date(0);
}

export class LessonsEditorModel {
    id: string = "00000000-0000-0000-0000-000000000000";
    languageFromId: number = 0;
    languageToId: number = 0;
    reading: string = "";
    pronunciation: string = "";
    meaning: string = "";
}

export class Review {
    id: string = "00000000-0000-0000-0000-000000000000";
    lesson: LessonAssignment = new LessonAssignment();
    reviewDone: Date = new Date(0);
    wrongAnswers: number = 0;
    startStage: number = 0;
    endStage: number = 0;
}

export class User {
    id: string = "00000000-0000-0000-0000-000000000000";
    userName: string = "";
    email: string = "";
    signedUp = new Date(0);
    lastLogin = new Date(0);
    premiumUntil = new Date(0);
    normalizedUserName: string = "";
    normalizedEmail: string = "";
    emailConfirmed: boolean = false;
    phoneNumber: string = "";
    phoneNumberConfirmed: boolean = false;
}

export class LanguagePair {
    constructor(languageFrom, languageTo) {
        this.languageFrom = languageFrom;
        this.languageTo = languageTo;
    }
    
    languageFrom: Language;
    languageTo: Language;

    toString(): string {
        return `${this.languageFrom.ietfTag}-${this.languageTo.ietfTag}`;
    }
}

export class ActionResult {
    succeeded: boolean;
    errors: string[];
    errorCode: number;
}

export class ReviewModel {
    assignmentId: string = "00000000-0000-0000-0000-000000000000";
    fieldFrom: ReviewField = ReviewField.Meaning;
    fieldTo: ReviewField = ReviewField.Pronunciation;
    from: string = "";
    to: string = "";
    solution: string = "";
}

export class Theme {
    backgroundPrimary: string;
    backgroundSecondary: string;
    backgroundTertiary: string;

    borderPrimary: string;
    borderSecondary: string;
    borderTertiary: string;

    textPrimary: string;
    textSecondary: string;
    textTertiary: string;
}