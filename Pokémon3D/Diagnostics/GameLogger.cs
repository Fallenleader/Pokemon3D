using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Pokémon3D.FileSystem;

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
    class GameLogger : IDisposable
    {
        StreamWriter _writer;
        bool _isDebuggerAttached;
        
        /// <summary>
        /// Initializes the logger.
        /// </summary>
        public GameLogger()
        {
            //Add a handler for the game exit event:
            GameCore.State.Controller.Exiting += new EventHandler<EventArgs>(Game_Exiting);

            _isDebuggerAttached = Debugger.IsAttached; //Check if a debugger is attached to the process.

            //Check if the log directory exists:
            if (!Directory.Exists(StaticFileProvider.LogDirectory))
                Directory.CreateDirectory(StaticFileProvider.LogDirectory);

            string logFile = StaticFileProvider.LogFile;

            //If a log file for this day already exists, read the content and append:
            if (File.Exists(logFile))
            {
                StreamReader reader = new StreamReader(logFile);
                string buffer = reader.ReadToEnd();
                reader.Close();

                _writer = new StreamWriter(logFile);
                _writer.Write(buffer);
            }
            else
            {
                _writer = new StreamWriter(logFile);
            }

            Log(MessageType.Message, "Game started.");
        }

        private void Game_Exiting(object sender, EventArgs e)
        {
            //When the writer has written any lines to the buffer, write them to the file:
            if (_writer != null)
            {
                Log(MessageType.Message, "Exiting game.");
                _writer.Close();
            }
        }

        /// <summary>
        /// Stores an entry in the game's log file.
        /// </summary>
        public void Log(MessageType messageType, string message)
        {
            string icon = GetMessageTypeIcon(messageType);
            string outLine = string.Concat(new string[]
                {
                    DateTime.Now.ToLongTimeString(),
                    " ",
                    icon,
                    " ",
                    message
                });

            _writer.WriteLine(outLine);

            if (_isDebuggerAttached)
            {
                System.Diagnostics.Debug.Print(DateTime.Now.ToLongTimeString() + " | " + icon + " | " + message);
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
                    return "";
            }
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}