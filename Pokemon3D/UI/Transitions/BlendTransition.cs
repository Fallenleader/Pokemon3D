using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;

namespace Pokemon3D.UI.Transitions
{
    class BlendTransition : GameObject, ScreenTransition
    {
        private Texture2D _source;
        private Texture2D _target;
        private float _elapsedTime;
        private float _transitionTime;

        public void StartTransition(Texture2D source, Texture2D target)
        {
            _source = source;
            _target = target;
            IsFinished = false;
            _elapsedTime = 0.0f;
            _transitionTime = 1.0f;
        }

        public bool IsFinished { get; private set; }

        public void Update(float elapsedTimeSeconds)
        {
            if (IsFinished) return;
            _elapsedTime += elapsedTimeSeconds;

            if (_elapsedTime >= _transitionTime)
            {
                IsFinished = true;
                _elapsedTime = _transitionTime;
            }
        }

        public void Draw()
        {
            var alpha = _elapsedTime/_transitionTime;

            Game.SpriteBatch.Begin();

            Game.SpriteBatch.Draw(_target, Vector2.Zero, Color.White * alpha);
            Game.SpriteBatch.Draw(_source, Vector2.Zero, Color.White * (1.0f-alpha));

            Game.SpriteBatch.End();
        }
    }
}
