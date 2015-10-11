using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Animations;
using Pokemon3D.Rendering.GUI;
using Pokémon3D.GameCore;

namespace Pokémon3D.UI.Screens
{
    class IntroScreen : Screen
    {
        private Animator _logoAnimator;

        private Sprite _logoSprite;
        private Sprite _highlightSprite;

        public void OnDraw(GameTime gameTime)
        {
            var game = GameController.Instance;
            
            game.GraphicsDevice.Clear(Color.Black);
            game.SpriteBatch.Begin();

            _logoSprite.Draw(game.SpriteBatch);
            _highlightSprite.Draw(game.SpriteBatch);
            game.SpriteBatch.End();
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
            var game = GameController.Instance;

            _logoSprite = new Sprite(game.Content.Load<Texture2D>(ResourceNames.Textures.SquareLogo_256px))
            {
                Position = new Vector2(game.Window.ClientBounds.Width*0.5f, game.Window.ClientBounds.Height*0.5f)
            };

            _highlightSprite = new Sprite(game.Content.Load<Texture2D>(ResourceNames.Textures.highlight))
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
