using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Pokémon3D.Input
{
    /// <summary>
    /// A class to control keyboard events.
    /// </summary>
    class Keyboard
    {
        private KeyboardState _oldState;
        private KeyboardState _newState;

        /// <summary>
        /// Updates the state of the handler.
        /// </summary>
        public void Update()
        {
            _oldState = _newState;
            _newState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }

        /// <summary>
        /// Checks if a specific key is being pressed.
        /// </summary>
        public bool IsKeyPressed(Keys key)
        {
            return _oldState.IsKeyUp(key) && _newState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if a specific key is being held down.
        /// </summary>
        public bool IsKeyDown(Keys key)
        {
            return _newState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns the array of pressed keys.
        /// </summary>
        public Keys[] GetPressedKeys()
        {
            return _newState.GetPressedKeys();
        }
    }
}