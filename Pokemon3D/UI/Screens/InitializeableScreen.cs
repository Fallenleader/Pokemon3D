using Microsoft.Xna.Framework;
using Pokemon3D.GameCore;

namespace Pokemon3D.UI.Screens
{
    abstract class InitializeableScreen : GameContextObject, Screen
    {
        private bool _isInitialized;

        protected abstract void OnInitialize();
        public abstract void OnDraw(GameTime gameTime);
        public abstract void OnUpdate(float elapsedTime);
        public abstract void OnClosing();

        public void OnOpening()
        {
            if (!_isInitialized) OnInitialize();
            _isInitialized = true;
        }
    }
}
