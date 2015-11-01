using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Pokemon3D.UI.i18n
{
    /// <summary>
    /// The base class for all translation objects for UI elements.
    /// </summary>
    abstract class Translation
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Instructions how to use classes that inherit from this:                                                                                 //
        // Their type name must start with "LOCAL_".                                                                                               //
        // All constants delared private and with a name starting with "C_" are put in the dictionary.                                             //
        // The "C_" and "LOCAL_" are not considered when looking up identifiers in external files.                                                 //
        // External files use "TypeName:Constant" as lookup (again, without "LOCAL_" and "C_").                                                    //
        // For the class LOCAL_InventoryScreen and its constant C_INFO_ITEM_OPTION_USE, the lookup is: InventoryScreen:INFO_ITEM_OPTION_USE.       //
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// A token to be inserted in a translated text.
        /// </summary>
        protected struct TranslationToken
        {
            public int Number;
            public string Data;
        }

        private Dictionary<string, string> _stringDic;
        private string _typeName = "";

        protected Translation()
        {
            _stringDic = new Dictionary<string, string>();

            // Grab all constants in the class and add them to the dictionary.
            // Those are the raw values to translate.

            var t = GetType();

            if (!t.Name.StartsWith("LOCAL_"))
                throw new ArgumentException("The type used as localization calss has to start with \"LOCAL_\".");

            _typeName = t.Name.Remove(0, "LOCAL_".Length);

            // Only get constants that start with "C_".
            var constants = t.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                             .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.Name.StartsWith("C_")).ToArray();

            foreach (FieldInfo cField in constants)
            {
                var cName = cField.Name.Remove(0, 2); // Remove the "C_" at the start of the constant name.
                var cValue = (string)(cField.GetRawConstantValue());

                AddTranslation(cName, cValue);
            }
        }

        /// <summary>
        /// Adds a translation to the dictionary.
        /// </summary>
        private void AddTranslation(string identifier, string englishDefault)
        {
            if (englishDefault == null)
                throw new ArgumentException("The default english value must not be null.", nameof(englishDefault));

            _stringDic.Add(identifier, englishDefault);
        }

        /// <summary>
        /// Builds a list of tokens out of an input list.
        /// </summary>
        private TranslationToken[] BuildTokens(object[] inputVars)
        {
            List<TranslationToken> tokens = new List<TranslationToken>();

            for (int i = 0; i < inputVars.Length; i++)
            {
                tokens.Add(new TranslationToken()
                {
                    Data = inputVars[i].ToString(),
                    Number = i + 1
                });
            }

            return tokens.ToArray();
        }

        /// <summary>
        /// Returns a translation for a UI string.
        /// </summary>
        /// <param name="identifier">The UI string identifier set with <see cref="AddTranslation"/>.</param>
        /// <param name="tokens">The tokens to be inserted into the string.</param>
        protected string GetTranslation(string identifier, string[] tokens)
        {
            return GetTranslation(identifier, BuildTokens(tokens));
        }

        /// <summary>
        /// Returns the translation for a UI string.
        /// </summary>
        /// <param name="identifier">The UI string identifier set with <see cref="AddTranslation"/>.</param>
        protected string GetTranslation(string identifier)
        {
            return GetTranslation(identifier, new TranslationToken[] { });
        }

        /// <summary>
        /// Returns the translation for a UI string.
        /// </summary>
        /// <param name="identifier">The UI string identifier set with <see cref="AddTranslation"/>.</param>
        /// <param name="tokens">The tokens inserted into the string.</param>
        protected string GetTranslation(string identifier, TranslationToken[] tokens)
        {
            // Translation tokens use .Net's String.Format format: {numeric}.
            // So the first token replaces "{0}".

            if (identifier == null)
                throw new ArgumentException("Identifier must not be null.", nameof(identifier));

            // Remove the "C_" from the identifier:
            identifier = identifier.Remove(0, 2);

            if (!_stringDic.Keys.Contains(identifier))
                throw new ArgumentException("The identifier is not present in the dictionary.", nameof(identifier));

            var translatedString = LocalizationManager.GetLocalString(_typeName + ":" + identifier);

            // Apply the fallback string if the language found does not have the identifier:
            if (translatedString == null)
                translatedString = _stringDic[identifier];

            // Only attempt to replace tokens if possible:
            if (tokens != null && tokens.Length > 0)
            {
                var parameters = tokens.OrderBy(x => x.Number).Select(x => x.Data).ToArray();

                translatedString = string.Format(translatedString, parameters);
            }

            return translatedString;
        }
    }
}
