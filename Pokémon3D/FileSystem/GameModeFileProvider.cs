﻿using System;
using System.Text;
using System.IO;

namespace Pokémon3D.FileSystem
{
    /// <summary>
    /// Provides access to paths related to GameModes.
    /// </summary>
    class GameModeFileProvider : FileProvider
    {
        private const string PATH_GAMEMODES = "GameModes";
        private const string FILE_GAMEMODE_MAIN = "GameMode.dat";

        /// <summary>
        /// The path to the base GameMode folder.
        /// </summary>
        public static string GameModeFolder
        {
            get
            {
                return Path.Combine(new string[] { StartupPath, PATH_GAMEMODES });
            }
        }

        /// <summary>
        /// Returns the path to a GameMode config file.
        /// </summary>
        public static string GetGameModeFile(string folder)
        {
            return Path.Combine(new string[] { folder, FILE_GAMEMODE_MAIN });
        }
    }
}
