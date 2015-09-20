using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pokémon3D.UI.Screens
{
    /// <summary>
    /// A screen to represent a scene in the game.
    /// </summary>
    abstract class Screen
    {
        /// <summary>
        /// Occurs when the screen closes.
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        /// Occurs when the screen opens.
        /// </summary>
        public event EventHandler Opening;

        /// <summary>
        /// Occurs when this screen is getting drawn.
        /// </summary>
        public event GameFlowEventHandler Draw;

        /// <summary>
        /// Occurs when this screen is getting updated.
        /// </summary>
        public event GameFlowEventHandler Update;

        /// <summary>
        /// Event handler for game flow events.
        /// </summary>
        public delegate void GameFlowEventHandler(object sender, GameFlowEventArgs e);
           
        /// <summary>
        /// Raises the Draw event.
        /// </summary>
        public void OnDraw(GameTime gameTime)
        {
            if (Draw != null)
                Draw(this, new GameFlowEventArgs(gameTime));
        }

        /// <summary>
        /// Raises the Update event.
        /// </summary>
        public void OnUpdate(GameTime gameTime)
        {
            if (Update != null)
                Update(this, new GameFlowEventArgs(gameTime));
        }

        /// <summary>
        /// Raises the Closing event.
        /// </summary>
        public void OnClosing()
        {
            if (Closing != null)
                Closing(this, new EventArgs());
        }

        /// <summary>
        /// Raises the Opening event.
        /// </summary>
        public void OnOpening()
        {
            if (Opening != null)
                Opening(this, new EventArgs());
        }
    }

    /// <summary>
    /// Event Arguments for game flow events.
    /// </summary>
    class GameFlowEventArgs : EventArgs
    {
        /// <summary>
        /// A snapshot of the game time.
        /// </summary>
        public GameTime GameTime { get; private set; }

        public GameFlowEventArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }
}