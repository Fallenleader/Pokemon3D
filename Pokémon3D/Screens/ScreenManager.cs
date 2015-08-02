using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pokémon3D.Screens
{
    /// <summary>
    /// A component to manage open screens.
    /// </summary>
    class ScreenManager : DrawableGameComponent, Components.IScreenManagerService
    {
        private Screen _currentScreen;

        /// <summary>
        /// Creates a new instance of the ScreenManager class.
        /// </summary
        public ScreenManager(Core.MainGame game) : base(game)
        {
            game.Services.AddService(typeof(Components.IScreenManagerService), this);
        }

        /// <summary>
        /// The currently active screen.
        /// </summary>
        public Screen CurrentScreen()
        {
            return _currentScreen;
        }

        /// <summary>
        /// Sets the current screen to a new screen instance.
        /// </summary>
        public void SetScreen(Screen newScreen)
        {
            if (_currentScreen != null)
                _currentScreen.OnClosing();

            _currentScreen = newScreen;
            newScreen.Game = (Core.MainGame)Game;

            newScreen.OnOpening();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (_currentScreen != null)
                _currentScreen.OnDraw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_currentScreen != null)
                _currentScreen.OnUpdate(gameTime);
        }
    }
}
