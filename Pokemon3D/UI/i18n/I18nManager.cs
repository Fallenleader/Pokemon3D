using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json.i18n;

namespace Pokemon3D.UI.i18n
{
    /// <summary>
    /// The base class for internationalization Managers.
    /// </summary>
    abstract class I18nManager : GameCore.GameContextObject
    {
        protected const string i18nFileExtension = ".json";
        private const string KeyFormat = "{0}>{1}>{2}";

        private Dictionary<string, string> _translations = new Dictionary<string, string>();

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
    }
}
