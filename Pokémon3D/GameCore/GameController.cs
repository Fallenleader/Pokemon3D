using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokémon3D.GameCore
{
    /// <summary>
    /// Wraps around the MonoGame <see cref="Game"/> class.
    /// </summary>
    class GameController : Game
    {
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

        private GraphicsDeviceManager _graphicsDeviceManager;
        
        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get { return _graphicsDeviceManager; }
        }

        public GameController()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        protected override void Initialize()
        {
            base.Initialize();
            State.Initialize(this);

            //Set an inital screen:
            State.ScreenManager.SetScreen(new UI.Screens.RenderingTestScreen(this));
        }
        
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Update all components of the state that need to be updated here:
            State.ScreenManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Clear the back buffer.
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);

            //Draw all components of the state that need to be drawn here:
            State.ScreenManager.Draw(gameTime);

            base.Draw(gameTime);
        }
        
    }
}
