using System;

namespace Pokemon3D.Common.Localization
{
    /// <summary>
    /// Implements functionality to get translations from raw text sources.
    /// </summary>
    public interface TranslationProvider
    {
        string GetTranslation(string sectionId, string tokenId);
        
        /// <summary>
        /// An event that gets fired when the language of the game changes.
        /// </summary>
        event EventHandler LanguageChanged;

        void OnLanguageChanged(object sender, EventArgs e);
    }
}
