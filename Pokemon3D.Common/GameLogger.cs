using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Common
{
    /// <summary>
    /// A class to log events during the game's runtime.
    /// </summary>
    public class GameLogger : Singleton<GameLogger>
    {
        private const string LoggerFileLineFormat = "{0} {1} {2}\r\n";
        private const string LoggerVsFormat = "{0} {1} {2}\r\n";
        private const string ExceptionMessageFormat = "An exception occurred: {0}\r\n";

        private string _logFilePath;
        
        private GameLogger()
        {
        }

        private void EnsureFolderExists()
        {
            var directory = Path.GetDirectoryName(_logFilePath) ?? "";
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        private void OnGameExit(object sender, EventArgs e)
        {
            Log(MessageType.Message, "Exiting game.");
        }

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        public void Initialize(Game game, string logFilePath)
        {
            game.Exiting += OnGameExit;
            _logFilePath = logFilePath;
            EnsureFolderExists();

            Log(MessageType.Message, "Game started.");
        }

        /// <summary>
        /// Stores an entry in the game's log file.
        /// </summary>
        public void Log(MessageType messageType, string message)
        {
            var icon = GetMessageTypeIcon(messageType);
            var outLine = string.Format(LoggerFileLineFormat, DateTime.Now.ToLongTimeString(), icon, message);
            File.AppendAllText(_logFilePath, outLine);

#if DEBUG
            if (Debugger.IsAttached) Debug.Print(LoggerVsFormat, DateTime.Now.ToLongTimeString(), icon, message);
#endif
        }
        
        /// <summary>
        /// Stores an exception message in the game's log file.
        /// </summary>
        public void Log(Exception ex)
        {
            Log(MessageType.Error, string.Format(ExceptionMessageFormat, ex));
        }

        /// <summary>
        /// Returns an ASCII icon for a message type.
        /// </summary>
        private string GetMessageTypeIcon(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Debug:
                    return "|>|";
                case MessageType.Message:
                    return "(!)";
                case MessageType.Warning:
                    return @"/!\";
                case MessageType.Error:
                    return "(X)";
                default:
                    return string.Empty;
            }
        }
    }
}