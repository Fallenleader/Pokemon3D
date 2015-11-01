using System.Collections.Generic;
using System.IO;
using Pokemon3D.FileSystem;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// A class to handle all loaded GameModes.
    /// </summary>
    class GameModeManager
    {
        /// <summary>
        /// Returns a collection of GameModes information.
        /// </summary>
        /// <returns></returns>
        public GameModeInfo[] GetGameModeInfos()
        {
            var gameModes = new List<GameModeInfo>();
            foreach (var gameModeDirectory in Directory.GetDirectories(GameModeFileProvider.GameModeFolder, "*.*", SearchOption.TopDirectoryOnly))
            {
                gameModes.Add(new GameModeInfo(gameModeDirectory));
            }
            return gameModes.ToArray();
        }

        public GameMode LoadGameMode(GameModeInfo gameModeInfo)
        {
            var gameModeDirectory = Path.Combine(GameModeFileProvider.GameModeFolder, gameModeInfo.DirectioryName);
            var gameModeFilePath = GameModeFileProvider.GetGameModeFile(gameModeDirectory);
            return new GameMode(gameModeFilePath);
        }
    }
}
