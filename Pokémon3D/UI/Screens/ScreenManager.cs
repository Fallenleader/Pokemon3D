using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pokémon3D.UI.Screens
{
    /// <summary>
    /// A component to manage open screens.
    /// </summary>
    class ScreenManager
    {
        private Screen _currentScreen;

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
            _currentScreen?.OnClosing();

            _currentScreen = newScreen;

            newScreen.OnOpening();
        }

        /// <summary>
        /// Draws the current screen.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            _currentScreen?.OnDraw(gameTime);
        }

        /// <summary>
        /// Updates the current screen.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            _currentScreen?.OnUpdate(gameTime);
        }
    }
}
