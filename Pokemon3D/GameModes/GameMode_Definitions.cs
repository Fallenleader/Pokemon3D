using System.IO;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// Contains definitions to reduce clutter in the main class file.
    /// </summary>
    partial class GameMode
    {
        private const string PATH_CONTENT = "Content";
        private const string PATH_CONTENT_TEXTURES = "Textures";
        private const string PATH_CONTENT_MODELS = "Models";
        private const string PATH_DATA = "Data";
        private const string PATH_MAPS = "Maps";
        private const string PATH_FRAGMENTS = "Fragments";
        private const string PATH_DATA_i18n = "i18n";

        public const string FILE_DATA_PRIMITIVES = "Primitives.json";

        /// <summary>
        /// The path to the texture base folder of this GameMode.
        /// </summary>
        public string TexturePath => Path.Combine(GameModeInfo.DirectoryPath, PATH_CONTENT, PATH_CONTENT_TEXTURES);

        /// <summary>
        /// The path to the texture base folder of this GameMode.
        /// </summary>
        public string DataPath => Path.Combine(GameModeInfo.DirectoryPath, PATH_DATA);

        /// <summary>
        /// The path to the model base folder of this GameMode.
        /// </summary>
        public string ModelPath => Path.Combine(GameModeInfo.DirectoryPath, PATH_CONTENT, PATH_CONTENT_MODELS);

        /// <summary>
        /// The path to the map base folder of this GameMode.
        /// </summary>
        public string MapPath => Path.Combine(GameModeInfo.DirectoryPath, PATH_MAPS);

        /// <summary>
        /// The path to the fragments base folder of this GameMode.
        /// </summary>
        public string FragmentsPath => Path.Combine(GameModeInfo.DirectoryPath, PATH_FRAGMENTS);

        public string i18nPath => Path.Combine(DataPath, PATH_DATA_i18n);
    }
}
