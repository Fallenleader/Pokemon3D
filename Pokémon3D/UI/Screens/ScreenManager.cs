using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Pokémon3D.GameCore;
using Pokémon3D.UI.Transitions;

namespace Pokémon3D.UI.Screens
{
    /// <summary>
    /// A component to manage open screens.
    /// </summary>
    class ScreenManager : GameContextObject
    {
        private readonly RenderTarget2D _sourceRenderTarget;
        private readonly RenderTarget2D _targetRenderTarget;
        private readonly Dictionary<Type, Screen> _screensByType = new Dictionary<Type, Screen>();

        private bool _executingScreenTransition;
        private bool _quitGame;
        private readonly ScreenTransition _currentTransition;

        public Screen CurrentScreen { get; private set; }

        public ScreenManager()
        {
            _sourceRenderTarget = new RenderTarget2D(Game.GraphicsDevice, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            _targetRenderTarget = new RenderTarget2D(Game.GraphicsDevice, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            _executingScreenTransition = false;
            _currentTransition = new BlendTransition();

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
            var oldScreen = CurrentScreen;

            CurrentScreen?.OnClosing();
            CurrentScreen = _screensByType[screenType];
            CurrentScreen.OnOpening();

            if (oldScreen != null && CurrentScreen != null)
            {
                PrerenderSourceAndTargetAndMakeTransition(oldScreen, CurrentScreen);
            }
        }

        public void NotifyQuitGame()
        {
            _quitGame = true;
        }

        private void PrerenderSourceAndTargetAndMakeTransition(Screen oldScreen, Screen currentScreen)
        {
            _executingScreenTransition = true;
            var currentRenderTarget = Game.GraphicsDevice.GetRenderTargets();

            Game.GraphicsDevice.SetRenderTarget(_sourceRenderTarget);
            oldScreen.OnDraw(new GameTime());

            Game.GraphicsDevice.SetRenderTarget(_targetRenderTarget);
            currentScreen.OnUpdate(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1.0/60.0d)));
            currentScreen.OnDraw(new GameTime());

            Game.GraphicsDevice.SetRenderTargets(currentRenderTarget);
            _currentTransition.StartTransition(_sourceRenderTarget, _targetRenderTarget);
        }

        /// <summary>
        /// Draws the current screen.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            if (_executingScreenTransition)
            {
                _currentTransition.Draw();
            }
            else
            {
                CurrentScreen?.OnDraw(gameTime);
            }
            
        }

        /// <summary>
        /// Updates the current screen.
        /// </summary>
        public bool Update(GameTime gameTime)
        {
            if (_executingScreenTransition)
            {
                _currentTransition.Update(gameTime.ElapsedGameTime.Milliseconds * 0.001f);
                if (_currentTransition.IsFinished)
                {
                    _executingScreenTransition = false;
                }
            }
            else
            {
                CurrentScreen?.OnUpdate(gameTime);
            }

            return !_quitGame;
        }
    }
}
