using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.FileSystem;
using Pokemon3D.UI.Screens;
using Pokemon3D.UI.Localization;
using Pokemon3D.Common;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.Rendering.Data;
using Pokemon3D.Rendering.GUI;
using Pokemon3D.GameModes;
using Pokemon3D.Common.Localization;
using Pokemon3D.UI;

namespace Pokemon3D.GameCore
{
    /// <summary>
    /// Wraps around the MonoGame <see cref="Game"/> class.
    /// </summary>
    class GameController : Game, GameContext
    {
        /// <summary>
        /// The singleton instance of the main GameController class.
        /// </summary>
        public static GameController Instance { get; private set; }

        /// <summary>
        /// The name of the game.
        /// </summary>
        public const string GAME_NAME = "Pokémon3D";

        /// <summary>
        /// The current version of the game.
        /// </summary>
        public const string VERSION = "1.0";

        /// <summary>
        /// The development stage of the game.
        /// </summary>
        public const string DEVELOPMENT_STAGE = "Alpha";

        /// <summary>
        /// The internal build number of the game. This number will increase with every release.
        /// </summary>
        public const string INTERNAL_VERSION = "89";

        /// <summary>
        /// If the debug mode is currently active.
        /// </summary>
        public const bool IS_DEBUG_ACTIVE = true;

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public ScreenManager ScreenManager { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public GuiSystem GuiSystem { get; private set; }
        public KeyboardEx Keyboard { get; private set; }
        public GameConfiguration GameConfig { get; private set; }
        public TranslationProvider TranslationProvider { get; private set; }
        public NotificationBar NotificationBar { get; private set; }

        public string VersionInformation => string.Format("{0} {1}", VERSION, DEVELOPMENT_STAGE);

        /// <summary>
        /// Object to manage loaded GameModes.
        /// </summary>
        public GameModeManager GameModeManager { get; private set; }

        public ResourceManager Resources { get; private set; }

        public Rectangle ScreenBounds => Window.ClientBounds;

        public ShapeRenderer ShapeRenderer { get; private set; }
        
        public GameController()
        {
            if (Instance != null) throw new InvalidOperationException("Game is singleton and can be created just once");

            GameLogger.Instance.Initialize(this, StaticFileProvider.LogFile);

            Instance = this;

            Content.RootDirectory = "Content";
            GameConfig = new GameConfiguration();
            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = GameConfig.WindowSize.Width,
                PreferredBackBufferHeight = GameConfig.WindowSize.Height
            };
        }
        
        protected override void LoadContent()
        {
            base.LoadContent();

            IsMouseVisible = true;

            GameModeManager = new GameModeManager();
            Resources = new ResourceManager(GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Keyboard = new KeyboardEx();
            GuiSystem = new GuiSystem(this);
            ShapeRenderer =  new ShapeRenderer(SpriteBatch, GraphicsDevice);
            ScreenManager = new ScreenManager();
            TranslationProvider = new CoreTranslationManager();
            NotificationBar = new NotificationBar(400);

            GameConfig.ConfigFileLoaded += TranslationProvider.OnLanguageChanged;

            GuiSystem.SetSkin(new GuiSystemSkinParameters()
            {
                SkinTexture = Content.Load<Texture2D>(ResourceNames.Textures.guiskin),
                BigFont = Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont),
                NormalFont = Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont),
                XmlSkinDescriptorFile = "Content/GUI/GuiSkin.xml"
            });

#if DEBUG
            ScreenManager.SetScreen(typeof(MainMenuScreen));
#else
            ScreenManager.SetScreen(typeof(IntroScreen));
#endif
        }
        
        protected override void Update(GameTime gameTime)
        {
            var elapsedSeconds = gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            base.Update(gameTime);
            Keyboard.Update();
            if (!ScreenManager.Update(elapsedSeconds)) Exit();
            NotificationBar.Update(elapsedSeconds);
        }

        protected override void Draw(GameTime gameTime)
        {
            ScreenManager.Draw(gameTime);
            NotificationBar.Draw();
            base.Draw(gameTime);
        }
        
    }
}
