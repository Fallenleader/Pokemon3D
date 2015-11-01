using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Rendering;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;
using Pokemon3D.GameModes.Maps;

namespace Pokemon3D.UI.Screens
{
    class OverworldScreen : GameContextObject, Screen
    {
        private GameMode _gameMode;
        private Map _currentMap;
        private Scene _scene;
        private Camera _camera;
        
        public void OnOpening()
        {
            var gameModes = Game.GameModeManager.GetGameModeInfos();
            _gameMode = Game.GameModeManager.LoadGameMode(gameModes.First());
            Game.Resources.SetPrimitiveProvider(_gameMode);

            _scene = new Scene(Game, new WindowsSceneEffect(Game.Content));
            _scene.Renderer.LightDirection = new Vector3(0, -1, 0);
            _currentMap = _gameMode.MapManager.LoadMap(_gameMode.StartMap, _scene, Game.Resources);

            _camera = _scene.CreateCamera();
            _camera.Position = new Vector3(6.0f, 8.0f, 14.0f);
            _camera.RotateX(-MathHelper.PiOver4);
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
