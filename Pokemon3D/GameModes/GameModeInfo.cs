using System;
using System.IO;
using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.GameMode;
using Pokemon3D.FileSystem;
using Pokemon3D.Common.Diagnostics;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// Contains global information about the GameMode.
    /// </summary>
    class GameModeInfo
    {
        private readonly GameModeModel _gameModeModel;

        public GameModeInfo(string directory)
        {
            DirectioryName = Path.GetFileName(directory);
            DirectoryPath = Path.Combine(GameModeFileProvider.GameModeFolder, DirectioryName);
            
            IsValid = false;
            string gameModeFile = GameModeFileProvider.GetGameModeFile(directory);
            
            if (File.Exists(gameModeFile))
            {
                try
                {
                    _gameModeModel = JsonDataModel<GameModeModel>.FromFile(gameModeFile);
                    IsValid = true;
                }
                catch (JsonDataLoadException)
                {
                    // todo: somehow log the exception, so that the information of the inner exception is preserved.
                    GameLogger.Instance.Log(MessageType.Error, "An error occurred trying to load the GameMode config file \"" + gameModeFile + "\".");
                }
            }
        }

        /// <summary>
        /// The path to this GameMode's folder.
        /// </summary>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// The name of this GameMode's folder (not full path).
        /// </summary>
        public string DirectioryName { get; private set; }
        
        /// <summary>
        /// Name of the Game Mode.
        /// </summary>
        public string Name => _gameModeModel?.Name;

        /// <summary>
        /// Author of the Game Mode.
        /// </summary>
        public string Author => _gameModeModel?.Author;

        /// <summary>
        /// Description of Game Mode.
        /// </summary>
        public string Description => _gameModeModel?.Description;

        /// <summary>
        /// Version of Game Mode.
        /// </summary>
        public string Version => _gameModeModel?.Version;

        /// <summary>
        /// The path (relative to \Maps\, without file extension) to the starting map of this GameMode.
        /// </summary>
        public string StartMap => _gameModeModel?.StartConfiguration.Map;

        /// <summary>
        /// The path (relative to \Scripts\, without file extension) to the starting script of this GameMode.
        /// </summary>
        public string StartScript => _gameModeModel?.StartConfiguration.Script;

        /// <summary>
        /// Is this Game Mode valid?
        /// </summary>
        public bool IsValid { get; private set; }
    }
}