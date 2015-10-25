using System;
using Microsoft.Xna.Framework;
using Pokemon3D.Rendering.GUI;
using Pokémon3D.GameCore;

namespace Pokémon3D.UI.Screens
{
    class MainMenuScreen : GameContextObject, Screen
    {
        private GuiPanel _mainMenuPanel;

        public void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            Game.SpriteBatch.Begin();
            _mainMenuPanel.Draw();
            Game.SpriteBatch.End();
        }

        public void OnUpdate(GameTime gameTime)
        {
            _mainMenuPanel.Update(gameTime.ElapsedGameTime.Milliseconds * 0.001f);
        }

        public void OnClosing()
        {
        }

        public void OnOpening()
        {
            _mainMenuPanel = new GuiPanel(Game);
            var root = Game.GuiSystem.CreateGuiHierarchyFromXml<GuiElement>("Content/Gui/MainMenu.xml");
            _mainMenuPanel.AddElement(root);

            root.FindGuiElementById<Button>("StartButton").Click += OnStartClick;
            root.FindGuiElementById<Button>("QuitButton").Click += OnQuitClick;
        }

        private void OnQuitClick()
        {
            Game.ScreenManager.NotifyQuitGame();
        }

        private void OnStartClick()
        {
            Game.ScreenManager.SetScreen(typeof(RenderingTestScreen));
        }
    }
}
