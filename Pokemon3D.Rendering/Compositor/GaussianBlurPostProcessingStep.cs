using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.Compositor
{
    class GaussianBlurPostProcessingStep : PostProcessingStep
    {
        private Effect _effect;
        private EffectTechnique _gaussianTechnique;
        private EffectParameter _sourceMapParameter;

        public bool IsEnabled { get; set; }

        public void Initialize(Effect contextEffect)
        {
            _effect = contextEffect;
            _gaussianTechnique = _effect.Techniques["GaussianBlur"];
            _sourceMapParameter = _effect.Parameters["SourceMap"];
        }

        public void Process(GameContext gameContext, Texture2D source, RenderTarget2D target)
        {
            _effect.CurrentTechnique = _gaussianTechnique;
            _sourceMapParameter.SetValue(source);

            gameContext.SpriteBatch.Begin(effect: _effect);
            gameContext.SpriteBatch.Draw(source, Vector2.Zero, Color.White);
            gameContext.SpriteBatch.End();
        }
    }
}
