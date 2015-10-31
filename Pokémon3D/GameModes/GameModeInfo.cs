using System;
using System.IO;
using Pokémon3D.DataModel.Json;
using Pokémon3D.DataModel.Json.GameMode;
using Pokémon3D.FileSystem;

namespace Pokémon3D.GameModes
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
            IsValid = false;
            var gameModeFile = GameModeFileProvider.GetGameModeFile(directory);
            
            if (File.Exists(gameModeFile))
            {
                try
                {
                    _gameModeModel = JsonDataModel.FromFile<GameModeModel>(gameModeFile);
                    IsValid = true;
                }
                catch { }
            }
        }

        /// <summary>
        /// Folder Name
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
        /// Is this Game Mode valid?
        /// </summary>
        public bool IsValid { get; private set; }
    }
}