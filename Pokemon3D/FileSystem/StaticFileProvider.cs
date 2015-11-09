using System;
using System.Text;
using System.IO;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// Provides paths to static files of the game.
    /// </summary>
    class StaticFileProvider : FileProvider
    {
        #region Logs

        const string LOG_FILE_EXTENSION = ".txt";
        const string LOG_FILE_PREFIX = "log_";
        const string LOG_DIRECTORY = "Logs";

        /// <summary>
        /// The directory logs are placed in.
        /// </summary>
        public static string LogDirectory
        {
            get
            {
                return Path.Combine(StartupPath, LOG_DIRECTORY);
            }
        }

        /// <summary>
        /// Returns the path to the current log file.
        /// </summary>
        public static string LogFile
        {
            get
            {
                /*
                  The log file will have this format:
                  log_yyyy-mm-dd.txt, corresponding to when the log file name is requested.
                */

                var now = DateTime.Now;
                StringBuilder sb = new StringBuilder(LOG_FILE_PREFIX);

                //D2 formats the number with a leading zero, if needed.
                sb.Append(now.Year.ToString());
                sb.Append("-");
                sb.Append(now.Month.ToString("D2"));
                sb.Append("-");
                sb.Append(now.Day.ToString("D2"));
                sb.Append(LOG_FILE_EXTENSION); //Append file ending.

                //Combine the startup path and the file name constructed by the string builder:
                return Path.Combine(LogDirectory, sb.ToString());
            }
        }

        #endregion

        #region Configuration

        private const string CONFIG_FILE_NAME = "configuration.json";

        /// <summary>
        /// The path to the main configuration file of the game.
        /// </summary>
        public static string ConfigFile
        {
            get
            {
                return Path.Combine(StartupPath, CONFIG_FILE_NAME);
            }
        }

        #endregion

        #region i18n

        const string i18n_DIRECTORY_NAME = "i18n";

        /// <summary>
        /// The path to the internationalization directory.
        /// </summary>
        public static string i18nDirectory
        {
            get
            {
                return Path.Combine(StartupPath, i18n_DIRECTORY_NAME);
            }
        }

        #endregion
    }
}