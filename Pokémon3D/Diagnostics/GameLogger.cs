using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Pokémon3D.FileSystem;
using Pokémon3D.GameCore;

namespace Pokémon3D.Diagnostics
{
    /// <summary>
    /// The type of a message that is getting logged.
    /// </summary>
    enum MessageType
    {
        Message,
        Debug,
        Error,
        Warning,
        Entry
    }

    /// <summary>
    /// A class to log events during the game's runtime.
    /// </summary>
    class GameLogger
    {
        bool _isDebuggerAttached;
        private static GameLogger _gameLogger;

        public static GameLogger Instance
        {
            get
            {
                if (_gameLogger == null) _gameLogger = new GameLogger();
                return _gameLogger;
            }
        }
        
        /// <summary>
        /// Initializes the logger.
        /// </summary>
        private GameLogger()
        {
            //Add a handler for the game exit event:
            GameController.Instance.Exiting += new EventHandler<EventArgs>(Game_Exiting);

            _isDebuggerAttached = Debugger.IsAttached; //Check if a debugger is attached to the process.

            EnsureLogfileAndFolderExists();

            Log(MessageType.Message, "Game started.");
        }

        private void EnsureLogfileAndFolderExists()
        {
            if (!Directory.Exists(StaticFileProvider.LogDirectory))
            {
                Directory.CreateDirectory(StaticFileProvider.LogDirectory);
            }

            if (!File.Exists(StaticFileProvider.LogFile)) File.Create(StaticFileProvider.LogFile).Close();
        }

        private void Game_Exiting(object sender, EventArgs e)
        {
            Log(MessageType.Message, "Exiting game.");
        }

        private const string LoggerFileLineFormat = "{0} {1} {2}";
        private const string LoggerVsFormat = "{0} {1} {2}";

        /// <summary>
        /// Stores an entry in the game's log file.
        /// </summary>
        public void Log(MessageType messageType, string message)
        {
            string icon = GetMessageTypeIcon(messageType);

            var outLine = string.Format("{0} {1} {2}",  DateTime.Now.ToLongTimeString(), icon, message);

            File.AppendText(outLine);

            if (_isDebuggerAttached)
            {
                Debug.Print(string.Format(LoggerVsFormat, DateTime.Now.ToLongTimeString(), icon, message));
            }
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