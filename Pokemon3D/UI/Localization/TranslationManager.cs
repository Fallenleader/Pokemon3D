using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json.i18n;
using Pokemon3D.Common.Localization;

namespace Pokemon3D.UI.Localization
{
    /// <summary>
    /// The base class for internationalization Managers.
    /// </summary>
    abstract class TranslationManager : GameCore.GameContextObject, TranslationProvider
    {
        protected const string i18nFileExtension = ".json";
        private const string KeyFormat = "{0}>{1}>{2}";

        private Dictionary<string, string> _translations = new Dictionary<string, string>();

        public event EventHandler LanguageChanged;

        /// <summary>
        /// Processes the loaded section models into tokens.
        /// </summary>
        protected void Load(SectionModel[] sectionModels)
        {
            foreach (var section in sectionModels)
            {
                foreach (var token in section.Tokens)
                {
                    _translations.Add(string.Format(KeyFormat, section.Language, section.Id, token.Id), token.Val);
                }
            }
        }

        public string GetTranslation(string languageId, string sectionId, string tokenId)
        {
            string key = string.Format(KeyFormat, languageId, sectionId, tokenId);
            string value = "";
            _translations.TryGetValue(key, out value);
            return value;
        }

        public string TranslateText(string text)
        {
            int searchIndex = 0;
            int foundIndex = text.IndexOf("{i18n>", searchIndex);
            int endIndex = -1;

            while (foundIndex > -1)
            {
                endIndex = text.IndexOf("}", foundIndex);
                if (endIndex > -1)
                {
                    string replace = text.Substring(foundIndex, endIndex - foundIndex + 1);
                    string[] parts = replace.Trim('{', '}').Split('>');

                    if (parts.Length == 3)
                    {
                        text = text.Remove(foundIndex, endIndex - foundIndex + 1);
                        string result = GetTranslation(Game.GameConfig.DisplayLanguage, parts[1], parts[2]);

                        if (result != null)
                        {
                            text = text.Insert(foundIndex, result);
                            searchIndex += result.Length;
                        }
                        else
                        {
                            text = text.Insert(foundIndex, replace);
                            searchIndex += replace.Length;
                        }
                    }
                    else
                    {
                        searchIndex = endIndex;
                    }
                }
                else
                {
                    searchIndex = foundIndex + 6;
                }
                foundIndex = text.IndexOf("{i18n>", searchIndex);
            }

            return text;
        }

        public void BindText(string text, Action<string> resolveText)
        {
            TranslationTextBinding.Create(this, text, resolveText);
        }

        public void OnLanguageChanged(object sender, EventArgs e)
        {
            LanguageChanged(this, e);
        }
    }
}
