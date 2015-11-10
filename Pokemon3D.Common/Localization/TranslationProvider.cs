using System;

namespace Pokemon3D.Common.Localization
{
    /// <summary>
    /// Implements functionality to get translations from raw text sources.
    /// </summary>
    public interface TranslationProvider
    {
        string GetTranslation(string languageId, string sectionId, string tokenId);

        /// <summary>
        /// Translates text that has the i18n pattern: {i18n>section>key}.
        /// </summary>
        string TranslateText(string text);

        /// <summary>
        /// A factory method to bind a translation event to a text property of a Gui element.
        /// </summary>
        /// <param name="text">The original raw string.</param>
        /// <param name="resolveText">The action to resolve the string with.</param>
        void BindText(string text, Action<string> resolveText);

        /// <summary>
        /// An event that gets fired when the language of the game changes.
        /// </summary>
        event EventHandler LanguageChanged;

        void OnLanguageChanged(object sender, EventArgs e);
    }
}
