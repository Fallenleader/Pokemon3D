using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokémon3D.UI.Screens
{
    /// <summary>
    /// A component to manage open screens.
    /// </summary>
    class ScreenManager
    {
        private Dictionary<Type, Screen> _screensByType = new Dictionary<Type, Screen>();

        public Screen CurrentScreen { get; private set; }

        public ScreenManager()
        {
            var screenTypes = typeof(Screen).Assembly.GetTypes().Where(t => typeof(Screen).IsAssignableFrom(t) && !t.IsAbstract);

            foreach(var screenType in screenTypes)
            {
                var screen = (Screen)Activator.CreateInstance(screenType);
                _screensByType.Add(screenType, screen);
            }
        }

        /// <summary>
        /// Sets the current screen to a new screen instance.
        /// </summary>
        public void SetScreen(Type screenType)
        {
            CurrentScreen?.OnClosing();
            CurrentScreen = _screensByType[screenType];
            CurrentScreen.OnOpening();
        }

        /// <summary>
        /// Draws the current screen.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            CurrentScreen?.OnDraw(gameTime);
        }

        /// <summary>
        /// Updates the current screen.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            CurrentScreen?.OnUpdate(gameTime);
        }
    }
}
