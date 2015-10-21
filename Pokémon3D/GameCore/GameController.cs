using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokémon3D.FileSystem;
using Pokémon3D.UI.Screens;
using Pokemon3D.Common;
using Pokemon3D.Common.Diagnostics;

namespace Pokémon3D.GameCore
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

        /// <summary>
        /// Object to manage loaded GameModes.
        /// </summary>
        public GameModes.GameModeManager GameModeManager { get; private set; }

        public KeyboardEx Keyboard
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Rectangle ScreenBounds
        {
            get
            {
                return Window.ClientBounds;
            }
        }

        public ShapeRenderer ShapeRenderer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public GameController()
        {
            if (Instance != null) throw new InvalidOperationException("Game is singleton and can be created just once");

            GameLogger.Instance.Initialize(this, StaticFileProvider.LogFile);

            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1024,
                PreferredBackBufferHeight = 600
            };
            Content.RootDirectory = "Content";
            
            Instance = this;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager = new ScreenManager();

            ScreenManager.SetScreen(typeof(IntroScreen));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ScreenManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            ScreenManager.Draw(gameTime);
            base.Draw(gameTime);
        }
        
    }
}
