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
        private Player _player;
        private SpriteFont _debugSpriteFont;
        private bool _showRenderStatistics;

        public void OnOpening(object enterInformation)
        {
            var gameModes = Game.GameModeManager.GetGameModeInfos();
            _gameMode = Game.GameModeManager.LoadGameMode(gameModes.First());
            Game.Resources.SetPrimitiveProvider(_gameMode);

            _scene = new Scene(Game, new WindowsSceneEffect(Game.Content));
            _scene.Renderer.LightDirection = new Vector3(0, -1, 0);
            _currentMap = _gameMode.MapManager.LoadMap(_gameMode.GameModeInfo.StartMap, _scene, Game.Resources);

            _player = new Player(_scene);

            _debugSpriteFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.DebugFont);
        }

        public void OnUpdate(float elapsedTime)
        {
            _player.Update(elapsedTime);
            _currentMap.Update(elapsedTime);
            _scene.Update(elapsedTime);

            if (Game.Keyboard.IsKeyDown(Keys.Escape))
            {
                Game.ScreenManager.SetScreen(typeof(MainMenuScreen));
            }

            if (Game.Keyboard.IsKeyDownOnce(Keys.F12))
            {
                _showRenderStatistics = !_showRenderStatistics;
            }
        }

        public void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer,Color.Black, 1.0f, 0);
            _scene.Draw();

            if (_showRenderStatistics) DrawRenderStatsitics();
        }

        private void DrawRenderStatsitics()
        {
            var renderStatistics = _scene.Renderer.RenderStatistics;

            const int spacing = 5;
            var elementHeight = _debugSpriteFont.LineSpacing + spacing;
            var height = elementHeight*4 + spacing;
            const int width = 180;

            var startPosition = new Vector2(Game.ScreenBounds.Width - width, Game.ScreenBounds.Height - height);

            Game.SpriteBatch.Begin();
            Game.ShapeRenderer.DrawFilledRectangle((int) startPosition.X,
                (int) startPosition.Y,
                width,
                height,
                Color.DarkGreen);

            startPosition.X += spacing;
            startPosition.Y += spacing;
            Game.SpriteBatch.DrawString(_debugSpriteFont,
                string.Format("Average DrawTime[ms]: {0:0.00}", renderStatistics.AverageDrawTime), startPosition, Color.White);
            startPosition.Y += elementHeight;
            Game.SpriteBatch.DrawString(_debugSpriteFont, string.Format("Total Drawcalls: {0}", renderStatistics.DrawCalls),
                startPosition, Color.White);
            startPosition.Y += elementHeight;
            Game.SpriteBatch.DrawString(_debugSpriteFont,
                string.Format("Solid Drawcalls: {0}", renderStatistics.SolidObjectDrawCalls), startPosition, Color.White);
            startPosition.Y += elementHeight;
            Game.SpriteBatch.DrawString(_debugSpriteFont,
                string.Format("Trans Drawcalls: {0}", renderStatistics.TransparentObjectDrawCalls), startPosition, Color.White);
            Game.SpriteBatch.End();
        }

        public void OnClosing()
        {
        }
        
    }
}
