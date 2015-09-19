using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static UI.Screens.ScreenManager ScreenManager { get; private set; }

        public static void Initialize(GameController controller)
        {
            Controller = controller;

            Logger = new Debug.GameLogger();
            ScreenManager = new UI.Screens.ScreenManager();
        }

    }
}
