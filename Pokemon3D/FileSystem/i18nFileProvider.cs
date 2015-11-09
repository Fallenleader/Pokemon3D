using System.IO;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// A file provider for localization files.
    /// </summary>
    class i18nFileProvider : FileProvider
    {
        const string i18nDirectory = "i18n";

        /// <summary>
        /// The lookup path for localization files.
        /// </summary>
        public static string LookupPath
        {
            get 
            {
                return Path.Combine(new string[] { StartupPath, i18nDirectory });
            }
        }
    }
}
