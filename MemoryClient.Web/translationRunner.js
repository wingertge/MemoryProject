// translationRunner.js 
const manageTranslations = require('react-intl-translations-manager').default;
 
// es2015 import 
// import manageTranslations from 'react-intl-translations-manager'; 
 
manageTranslations({
  messagesDirectory: 'Scripts/lang/extracted',
  translationsDirectory: 'Scripts/lang/extracted-parsed',
  languages: ['en-US', 'ja-JP'], // any language you need 
});