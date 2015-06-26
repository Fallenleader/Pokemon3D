#region File Information
/*
GameCore.cs

Pokémon 3D Open Source Version
Copyright © 2015 Nils Drescher. All rights reserved.
*/
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
#endregion

namespace Pokemon3D
{
    /// <summary>
    /// Controls the game's main workflow.
    /// </summary>
    public partial class GameCore : Game
    {
        #region Members

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        #endregion

        #region Properties

        /// <summary>
        /// The main spritebatch to draw textures.
        /// </summary>
        /// <returns></returns>
        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        /// <summary>
        /// The graphics device.
        /// </summary>
        /// <returns></returns>
        public GraphicsDeviceManager Graphics
        {
            get { return _graphics; }
        }

        /// <summary>
        /// The path to the game's folder.
        /// </summary>
        /// <returns></returns>
        public static string Gamepath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        #endregion

        /// <summary>
        /// Creates a new instance of the GameCore class.
        /// </summary>
        public GameCore()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Initialization code.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Load initial content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world, checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
