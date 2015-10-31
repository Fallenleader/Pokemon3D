using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pokémon3D.GameCore;
using Pokémon3D.GameModes;

namespace Pokémon3D.UI.Screens
{
    class OverworldScreen : GameContextObject, Screen
    {
        private GameModeManager _gameModeManager;
        private GameMode _gameMode;
        
        public void OnOpening()
        {
            _gameModeManager = new GameModeManager();
            var gameModes = _gameModeManager.GetGameModeInfos();

            _gameMode = _gameModeManager.LoadGameMode(gameModes.First());
        }

        public void OnDraw(GameTime gameTime)
        {
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (Game.Keyboard.IsKeyDown(Keys.Escape))
            {
                Game.ScreenManager.SetScreen(typeof(MainMenuScreen));
            }
        }

        public void OnClosing()
        {
        }
        
    }
}
