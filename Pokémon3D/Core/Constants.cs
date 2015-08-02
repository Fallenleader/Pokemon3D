namespace Pokémon3D.Core
{
    partial class MainGame
    {
        /// <summary>
        /// The name of the game.
        /// </summary>
        public const string GAME_NAME = "Pokémon3D";

        /// <summary>
        /// The current version of the game.
        /// </summary>
        public const string VERSION = "1.0";

        /// <summary>
        /// The development stage of the game.
        /// </summary>
        public const string DEVELOPMENT_STAGE = "Alpha";

        /// <summary>
        /// The internal build number of the game. This number will increase with every release.
        /// </summary>
        public const string INTERNAL_VERSION = "89";

        /// <summary>
        /// If the debug mode is currently active.
        /// </summary>
        public const bool IS_DEBUG_ACTIVE = true;

        /// <summary>
        /// This will get activated if the player enters the Konami Code in the Main Menu Screen.
        /// The game will apply a pixelating effect to everything that renders when this value is True.
        /// </summary>
        public static bool KonamiEffect = false;

    }
}