using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.UI.Transitions;

namespace Pokemon3D.UI.Screens
{
    /// <summary>
    /// A component to manage open screens.
    /// </summary>
    class ScreenManager : GameObject
    {
        private readonly RenderTarget2D _sourceRenderTarget;
        private readonly RenderTarget2D _targetRenderTarget;
        private readonly Dictionary<Type, Screen> _screensByType;
        private readonly Dictionary<Type, ScreenTransition> _screenTransitionsByType;

        private bool _executingScreenTransition;
        private bool _quitGame;
        private ScreenTransition _currentTransition;

        public Screen CurrentScreen { get; private set; }

        public ScreenManager()
        {
            var clientBounds = Game.Window.ClientBounds;
            _sourceRenderTarget = new RenderTarget2D(Game.GraphicsDevice, clientBounds.Width, clientBounds.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            _targetRenderTarget = new RenderTarget2D(Game.GraphicsDevice, clientBounds.Width, clientBounds.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            _executingScreenTransition = false;
            _currentTransition = new BlendTransition();

            _screensByType = GetImplementationsOf<Screen>().ToDictionary(s => s, s => (Screen) Activator.CreateInstance(s));
            _screenTransitionsByType = GetImplementationsOf<ScreenTransition>().ToDictionary(s => s, s => (ScreenTransition)Activator.CreateInstance(s));
        }

        private static IEnumerable<Type> GetImplementationsOf<T>()
        {
            return typeof(T).Assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract);
        } 

        /// <summary>
        /// Sets the current screen to a new screen instance.
        /// </summary>
        public void SetScreen(Type screenType, Type transition = null, object enterInformation = null)
        {
            var oldScreen = CurrentScreen;

            CurrentScreen?.OnClosing();
            CurrentScreen = _screensByType[screenType];
            CurrentScreen.OnOpening(enterInformation);

            if (oldScreen != null && CurrentScreen != null && transition != null)
            {
                PrerenderSourceAndTargetAndMakeTransition(oldScreen, CurrentScreen, transition);
            }
        }

        public void NotifyQuitGame()
        {
            _quitGame = true;
        }

        private void PrerenderSourceAndTargetAndMakeTransition(Screen oldScreen, Screen currentScreen, Type transition)
        {
            _executingScreenTransition = true;
            var currentRenderTarget = Game.GraphicsDevice.GetRenderTargets();

            Game.GraphicsDevice.SetRenderTarget(_sourceRenderTarget);
            oldScreen.OnDraw(new GameTime());

            Game.GraphicsDevice.SetRenderTarget(_targetRenderTarget);
            currentScreen.OnUpdate(0);
            currentScreen.OnDraw(new GameTime());

            Game.GraphicsDevice.SetRenderTargets(currentRenderTarget);
            _currentTransition = _screenTransitionsByType[transition];
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
            var elapsedMilliSeconds = gameTime.ElapsedGameTime.Milliseconds*0.001f;

            if (_executingScreenTransition)
            {
                _currentTransition.Update(elapsedMilliSeconds);
                if (_currentTransition.IsFinished)
                {
                    _executingScreenTransition = false;
                }
            }
            else
            {
                CurrentScreen?.OnUpdate(elapsedMilliSeconds);
            }

            return !_quitGame;
        }
    }
}
