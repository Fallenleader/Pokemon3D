using Pokémon3D.UI.Screens;

namespace Pokémon3D.GameCore
{
    /// <summary>
    /// A class to hold static references to the current game state.
    /// </summary>
    static class State
    {
        /// <summary>
        /// The game's main controller class.
        /// </summary>
        public static GameController Controller { get; private set; }

        /// <summary>
        /// The game's active logger instance.
        /// </summary>
        public static Debug.GameLogger Logger { get; private set; }

        /// <summary>
        /// The game's active screen manager to access and manage the currently active screen instance.
        /// </summary>
        public static ScreenManager ScreenManager { get; private set; }

        /// <summary>
        /// Object to manage loaded GameModes.
        /// </summary>
        public static GameModes.GameModeManager GameModeManager { get; private set; }

        public static void Initialize(GameController controller)
        {
            Controller = controller;

            Logger = new Debug.GameLogger();
            ScreenManager = new ScreenManager();
            GameModeManager = new GameModes.GameModeManager();
        }

    }
}
