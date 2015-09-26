using System.IO;

namespace Pokémon3D.GameModes
{

    //Contains definitions to reduce clutter in the main class file.

    partial class GameMode
    {
        private const string PATH_CONTENT = @"Content\";

        private const string PATH_CONTENT_TEXTURES = @"Textures\";
        private const string PATH_CONTENT_MODELS = @"Models\";

        /// <summary>
        /// The path to the texture base folder of this GameMode.
        /// </summary>
        public string TexturePath
        {
            get
            {
                return Path.Combine(new string[] { _gameModeFolder, PATH_CONTENT, PATH_CONTENT_TEXTURES });
            }
        }

        /// <summary>
        /// The path to the model base folder of this GameMode.
        /// </summary>
        public string ModelPath
        {
            get
            {
                return Path.Combine(new string[] { _gameModeFolder, PATH_CONTENT, PATH_CONTENT_MODELS });
            }
        }
    }
}
