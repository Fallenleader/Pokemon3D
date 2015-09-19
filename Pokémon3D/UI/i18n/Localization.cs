using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokémon3D.DataModel.Json;
using Pokémon3D.DataModel.Json.i18n;

namespace Pokémon3D.UI.i18n
{
    /// <summary>
    /// The content of a localization file.
    /// </summary>
    class Localization
    {
        private string _lcid;
        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();
        private string _filePath = "";
        private bool _hasLoaded = false;

        /// <summary>
        /// The language specific LCID.
        /// </summary>
        public string LCID
        {
            get { return _lcid; }
        }

        public Localization(string filePath)
        {
            _filePath = filePath;

            // Take the lcid from the file name:
            _lcid = System.IO.Path.GetFileNameWithoutExtension(_filePath);
        }

        /// <summary>
        /// Lazy-load data from file.
        /// </summary>
        private void Load()
        {
            string fileContent = System.IO.File.ReadAllText(_filePath);
            LocalizationModel dataModel = JsonDataModel.FromString<LocalizationModel>(fileContent);

            foreach (var token in dataModel.Tokens)
            {
                _dictionary.Add(token.Id, token.Val);
            }

            _hasLoaded = true;
        }

        /// <summary>
        /// Returns the translation of this localization for an identifier.
        /// </summary>
        public string GetTranslation(string identifier)
        {
            if (!_hasLoaded)
                Load();

            if (_dictionary.Keys.Contains(identifier))
                return _dictionary[identifier];
            else
                return null;
        }
    }
}
