using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;

namespace Pokemon3D.UI.Transitions
{
    class SlideTransition : GameContextObject, ScreenTransition
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
            var offset = (_elapsedTime/_transitionTime)*Game.ScreenBounds.Width;

            Game.SpriteBatch.Begin();

            Game.SpriteBatch.Draw(_source, new Vector2(-offset, 0.0f), Color.White);
            Game.SpriteBatch.Draw(_target, new Vector2(Game.ScreenBounds.Width-offset, 0.0f), Color.White);

            Game.SpriteBatch.End();
        }
    }
}
