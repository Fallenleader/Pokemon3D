using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokémon3D.FileSystem;

namespace Pokémon3D.GameModes
{
    /// <summary>
    /// A class to handle all loaded GameModes.
    /// </summary>
    class GameModeManager
    {
        private List<GameMode> _gameModes;
        private int _activeGameMode = -1;

        public GameModeManager()
        {
            _gameModes = new List<GameMode>();
        }

        /// <summary>
        /// Scans the GameMode folder for compatible GameModes and loads them.
        /// </summary>
        public void LoadGameModes()
        {
            //Unload and clear all old GameModes first:
            foreach (GameMode oldGameMode in _gameModes)
            {
                oldGameMode.Unload();
            }
            _gameModes.Clear();

            string gameModeFilePath = "";
            string[] folders = System.IO.Directory.GetDirectories(GameModeFileProvider.GameModeFolder, "*.*", System.IO.SearchOption.TopDirectoryOnly);

            foreach (string folder in folders)
            {
                //Check if the GameMode.dat file exists inside the selected folder:
                gameModeFilePath = GameModeFileProvider.GetGameModeFile(folder);

                if (System.IO.File.Exists(gameModeFilePath))
                {
                    var newGameMode = new GameMode(gameModeFilePath);
                    if (newGameMode.IsValid)
                        _gameModes.Add(newGameMode);
                }
            }
        }

        /// <summary>
        /// Returns the currently active GameMode.
        /// </summary>
        public GameMode ActiveGameMode
        {
            get
            {
                if (_activeGameMode == -1)
                    return null;

                return _gameModes[_activeGameMode];
            }
        }

        /// <summary>
        /// The list of loaded GameModes.
        /// </summary>
        public GameMode[] GameModes
        {
            get { return _gameModes.ToArray(); }
        }

        /// <summary>
        /// Sets the active GameMode with an index based on the loaded GameMode list.
        /// </summary>
        public void SetActiveGameMode(int index)
        {
            _activeGameMode = index;
        }

        /// <summary>
        /// Sets a new active GameMode.
        /// </summary>
        public void SetActiveGameMode(GameMode gameMode)
        {
            _activeGameMode = _gameModes.IndexOf(gameMode);
        }
    }
}
