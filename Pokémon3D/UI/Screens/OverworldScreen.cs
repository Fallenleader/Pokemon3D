using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Rendering.Scene;
using Pokémon3D.GameCore;
using Pokémon3D.GameModes;
using Pokémon3D.GameModes.Maps;

namespace Pokémon3D.UI.Screens
{
    class OverworldScreen : GameContextObject, Screen
    {
        private GameModeManager _gameModeManager;
        private GameMode _gameMode;
        private Map _currentMap;
        private Scene _scene;
        
        public void OnOpening()
        {
            _gameModeManager = new GameModeManager();
            var gameModes = _gameModeManager.GetGameModeInfos();
            _gameMode = _gameModeManager.LoadGameMode(gameModes.First());

            _scene = new Scene(Game, new WindowsSceneEffect(Game.Content));
            _currentMap = _gameMode.MapManager.LoadMap(_gameMode.StartMap);
        }

        public void OnUpdate(GameTime gameTime)
        {
            var elapsedSeconds = gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            _currentMap.Update(elapsedSeconds);
            _scene.Update(elapsedSeconds);

            if (Game.Keyboard.IsKeyDown(Keys.Escape))
            {
                Game.ScreenManager.SetScreen(typeof(MainMenuScreen));
            }
        }

        public void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer,Color.Black, 1.0f, 0);
            _scene.Draw();
        }

        public void OnClosing()
        {
        }
        
    }
}
