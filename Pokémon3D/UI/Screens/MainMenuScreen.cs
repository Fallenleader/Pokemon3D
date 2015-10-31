using Microsoft.Xna.Framework;
using Pokemon3D.Rendering.GUI;

namespace Pokémon3D.UI.Screens
{
    class MainMenuScreen : InitializeableScreen
    {
        private GuiPanel _mainMenuPanel;

        protected override void OnInitialize()
        {
            _mainMenuPanel = new GuiPanel(Game);
            var root = Game.GuiSystem.CreateGuiHierarchyFromXml<GuiElement>("Content/Gui/MainMenu.xml");
            _mainMenuPanel.AddElement(root);

            root.FindGuiElementById<Button>("StartButton").Click += OnStartClick;
            root.FindGuiElementById<Button>("LoadButton").Click += OnLoadClick;
            root.FindGuiElementById<Button>("QuitButton").Click += OnQuitClick;
        }

        private void OnLoadClick()
        {
            Game.ScreenManager.SetScreen(typeof(OverworldScreen));
        }

        public override void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            Game.SpriteBatch.Begin();
            _mainMenuPanel.Draw();
            Game.SpriteBatch.End();
        }

        public override void OnUpdate(GameTime gameTime)
        {
            _mainMenuPanel.Update(gameTime.ElapsedGameTime.Milliseconds * 0.001f);
        }

        public override void OnClosing()
        {
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
