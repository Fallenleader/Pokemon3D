using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.GUI;
using Pokemon3D.UI.Transitions;

namespace Pokemon3D.UI.Screens
{
    class MainMenuScreen : InitializeableScreen
    {
        private Sprite _headerSprite;
        private SpriteText _versionInformation;

        private GuiPanel _mainMenuPanel;
        private GuiPanel _optionsMenuPanel;

        protected override void OnInitialize(object enterInformation)
        {
            _headerSprite = new Sprite(Game.Content.Load<Texture2D>(ResourceNames.Textures.P3D))
            {
                Scale = new Vector2(0.5f)
            };
            _headerSprite.Position = new Vector2(Game.ScreenBounds.Width * 0.5f, _headerSprite.Bounds.Height * 0.5f + 10.0f);
            _versionInformation = new SpriteText(Game.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont))
            {
                Color = Color.White,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Text = Game.VersionInformation
            };
            _versionInformation.SetTargetRectangle(new Rectangle(0,0, Game.ScreenBounds.Width-10, Game.ScreenBounds.Height - 10));
            
            var guiSpace = new Rectangle(0,50, Game.ScreenBounds.Width, Game.ScreenBounds.Height-30);
            
            var root = Game.GuiSystem.CreateGuiHierarchyFromXml<GuiElement>("Content/Gui/MainMenu.xml");
            _mainMenuPanel = new GuiPanel(Game, root, guiSpace);

            root.FindGuiElementById<Button>("StartButton").Click += OnStartClick;
            root.FindGuiElementById<Button>("LoadButton").Click += OnLoadClick;
            root.FindGuiElementById<Button>("OptionsButton").Click += OnOptionsClick;
            root.FindGuiElementById<Button>("QuitButton").Click += OnQuitClick;

            root = Game.GuiSystem.CreateGuiHierarchyFromXml<GuiElement>("Content/Gui/OptionsMenu.xml");
            _optionsMenuPanel = new GuiPanel(Game, root, guiSpace)
            {
                IsEnabled = false
            };
            root.FindGuiElementById<Button>("BackToMainMenuButton").Click += OnBackToMainMenuButtonClick;
        }

        private void OnBackToMainMenuButtonClick()
        {
            _optionsMenuPanel.IsEnabled = false;
            _mainMenuPanel.IsEnabled = true;
        }

        private void OnOptionsClick()
        {
            _optionsMenuPanel.IsEnabled = true;
            _mainMenuPanel.IsEnabled = false;
        }

        private void OnLoadClick()
        {
            
        }

        public override void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            Game.SpriteBatch.Begin();

            _headerSprite.Draw(Game.SpriteBatch);
            _versionInformation.Draw(Game.SpriteBatch);

            _mainMenuPanel.Draw();
            _optionsMenuPanel.Draw();
            Game.SpriteBatch.End();
        }

        public override void OnUpdate(float elapsedTime)
        {
            _mainMenuPanel.Update(elapsedTime);
            _optionsMenuPanel.Update(elapsedTime);
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
            Game.ScreenManager.SetScreen(typeof(OverworldScreen), typeof(SlideTransition));
        }
    }
}
