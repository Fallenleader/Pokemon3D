﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Rendering;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;
using Pokemon3D.GameModes.Maps;
using Pokemon3D.Rendering.Compositor;

namespace Pokemon3D.UI.Screens
{
    class OverworldScreen : GameObject, Screen
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
            _scene.Renderer.LightDirection = new Vector3(-1.5f,-1,-0.5f);
            _scene.Renderer.AmbientLight = new Vector4(0.7f, 0.5f, 0.5f, 1.0f);
            _scene.Renderer.EnableShadows = true;
            _currentMap = _gameMode.MapManager.GetMap(_gameMode.GameModeInfo.StartMap, _scene, Game.Resources);

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
            if (Game.Keyboard.IsKeyDownOnce(Keys.F11))
            {
                _scene.Renderer.EnableShadows = !_scene.Renderer.EnableShadows;
                if (_scene.Renderer.EnableShadows)
                {
                    Game.NotificationBar.PushNotification(NotificationKind.Information, "Enabled Shadows");
                }
                else
                {
                    Game.NotificationBar.PushNotification(NotificationKind.Information, "Disabled Shadows");
                }
            }

            if (Game.Keyboard.IsKeyDownOnce(Keys.V))
            {
                if (_player.MovementMode == PlayerMovementMode.GodMode)
                {
                    Game.NotificationBar.PushNotification(NotificationKind.Information, "Disabled God Mode");
                }
                if (_player.MovementMode == PlayerMovementMode.FirstPerson)
                {
                    _player.MovementMode = PlayerMovementMode.ThirdPerson;
                }
                else
                {
                    _player.MovementMode = PlayerMovementMode.FirstPerson;
                }
            }

            if (Game.Keyboard.IsKeyDownOnce(Keys.F10))
            {
                _player.MovementMode = PlayerMovementMode.GodMode;
                Game.NotificationBar.PushNotification(NotificationKind.Information, "Enabled God Mode");
            }
        }

        public void OnDraw(GameTime gameTime)
        {
            _scene.Draw();

            if (_showRenderStatistics) DrawRenderStatsitics();
        }

        private void DrawRenderStatsitics()
        {
            var renderStatistics = RenderStatistics.Instance;

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
            Game.SpriteBatch.End();
        }

        public void OnClosing()
        {
        }
        
    }
}
