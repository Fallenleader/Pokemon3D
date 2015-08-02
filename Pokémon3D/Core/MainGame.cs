using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokémon3D.Core
{
    /// <summary>
    /// The main game type.
    /// </summary>
    partial class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// The main <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> instance.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public MainGame() : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set the screen size to 720p:
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            Window.AllowUserResizing = true;
        }

        /// <summary>
        /// This initializes all game components.
        /// </summary>
        protected override void Initialize()
        {
            Components.Add(new Debug.GameLogger(this));
            Components.Add(new Screens.ScreenManager(this));
            Components.Add(new UI.Graphics(this));

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            base.Draw(gameTime);

            _spriteBatch.End();
        }

        #region ServiceProviders

        //This section will allow easy access to always existing services like the logger.

        /// <summary>
        /// Returns the current logging service.
        /// </summary>
        public Components.ILoggingService LoggingService
        {
            get { return Services.GetService<Components.ILoggingService>(); }
        }

        /// <summary>
        /// Returns the current screen manager service.
        /// </summary>
        public Components.IScreenManagerService ScreenManager
        {
            get { return Services.GetService<Components.IScreenManagerService>(); }
        }

        /// <summary>
        /// Returns the current UI drawing service.
        /// </summary>
        public Components.IGraphicsService Graphics
        {
            get { return Services.GetService<Components.IGraphicsService>(); }
        }

        #endregion
    }
}