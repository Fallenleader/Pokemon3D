using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Animations;
using Pokemon3D.Rendering.GUI;
using Pokémon3D.GameCore;

namespace Pokémon3D.UI.Screens
{
    class IntroScreen : GameContextObject, Screen
    {
        private Animator _logoAnimator;

        private Sprite _logoSprite;
        private Sprite _highlightSprite;

        public void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            Game.SpriteBatch.Begin();

            _logoSprite.Draw(Game.SpriteBatch);
            _highlightSprite.Draw(Game.SpriteBatch);
            Game.SpriteBatch.End();
        }

        public void OnUpdate(GameTime gameTime)
        {
            _logoAnimator.Update(gameTime.ElapsedGameTime.Milliseconds * 0.001f);
        }

        public void OnClosing()
        {
        }

        public void OnOpening()
        {
            _logoSprite = new Sprite(Game.Content.Load<Texture2D>(ResourceNames.Textures.SquareLogo_256px))
            {
                Position = new Vector2(Game.Window.ClientBounds.Width*0.5f, Game.Window.ClientBounds.Height*0.5f)
            };

            _highlightSprite = new Sprite(Game.Content.Load<Texture2D>(ResourceNames.Textures.highlight))
            {
                Position = _logoSprite.Position - _logoSprite.Size*0.5f,
                Alpha = 0.0f
            };

            _logoAnimator = new Animator();
            _logoAnimator.AddAnimation("TurningAlpha", Animation.CreateDelta(1.0f, OnUpdateTurningAlpha));
            _logoAnimator.AddAnimation("Wait", Animation.CreateWait(0.5f));
            _logoAnimator.AddAnimation("HighlightPass", Animation.CreateDelta(1.5f, OnUpdateHighlightPass));
            _logoAnimator.AddTransitionChain("TurningAlpha", "Wait", "HighlightPass");
            _logoAnimator.SetAnimation("TurningAlpha");
            _logoAnimator.AnimatorFinished += OnAnimatorFinished;
        }

        private void OnAnimatorFinished()
        {
            Game.ScreenManager.SetScreen(typeof(RenderingTestScreen));
        }

        private void OnUpdateHighlightPass(float delta)
        {
            if (delta <= 0.5f)
            {
                var highlightAlpha = MathHelper.SmoothStep(0.0f, 1.0f, delta*2.0f);
                _highlightSprite.Alpha = highlightAlpha;
                _highlightSprite.Scale = new Vector2( 1.0f + highlightAlpha);
            }
            else
            {
                var highlightAlpha = MathHelper.SmoothStep(0.0f, 1.0f, (delta - 0.5f) * 2.0f);
                _highlightSprite.Alpha = 1.0f - highlightAlpha;
                _highlightSprite.Scale = new Vector2(1.0f + 1.0f - highlightAlpha);
            }

            _highlightSprite.Position = _logoSprite.Position + new Vector2(0.0f, -_logoSprite.Height *0.5f) + new Vector2(_logoSprite.Width *0.5f, 0.0f) * delta;
        }

        private void OnUpdateTurningAlpha(float delta)
        {
            _logoSprite.Rotation = MathHelper.SmoothStep(0.0f, 1.0f, delta)*MathHelper.TwoPi;
            _logoSprite.Alpha = MathHelper.SmoothStep(0.0f, 1.0f, delta);
            _logoSprite.Scale = new Vector2( 0.5f + MathHelper.SmoothStep(0.0f, 1.0f, delta)*0.5f);
        }
    }
}
