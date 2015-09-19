using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokémon3D.UI.i18n
{
    /// <summary>
    /// A class to manage all localizations available for the game.
    /// They are marked with the LCID of the language.
    /// </summary>
    /// <remarks>
    /// More Information:
    /// https://msdn.microsoft.com/en-us/goglobal/bb964664.aspx
    /// </remarks>
    static class LocalizationManager
    {
        private static Dictionary<string, Localization> _localList;

        private static void Initialize()
        {
            _localList = new Dictionary<string, Localization>();

            string lookupPath = FileSystem.TranslationFileProvider.LookupPath;

            foreach (string file in System.IO.Directory.GetFiles(lookupPath, "*.dat"))
            {
                var localization = new Localization(file);
                _localList.Add(localization.LCID, localization);
            }
        }

        /// <summary>
        /// Returns the current culture ISO code of the UI language used.
        /// </summary>
        private static string GetLocaleId()
        {
            return System.Globalization.CultureInfo.CurrentUICulture.LCID.ToString();
        }

        /// <summary>
        /// Returns the localized string or nothing, if no language or no identifier in that language exists.
        /// </summary>
        public static string GetLocalString(string identifier)
        {
            if (_localList == null)
                Initialize();

            string lcid = GetLocaleId();

            if (_localList.Keys.Contains(lcid))
                return _localList[lcid].GetTranslation(identifier);
            else
                return null;
        }
    }
}
