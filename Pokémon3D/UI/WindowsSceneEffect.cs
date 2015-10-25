using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Compositor;

namespace Pokémon3D.UI
{
    class WindowsSceneEffect : SceneEffect
    {
        private readonly Effect _basicEffect;
        private readonly EffectTechnique _shadowDepthTechnique;
        private readonly EffectTechnique _defaultWithShadowsTechnique;
        private readonly EffectTechnique _billboardTechnique;
        private readonly EffectTechnique _defaultTechnique;

        private readonly EffectParameter _lightWorldViewProjection;
        private readonly EffectParameter _world;
        private readonly EffectParameter _view;
        private readonly EffectParameter _projection;
        private readonly EffectParameter _lightDirection;
        private readonly EffectParameter _shadowMap;
        private readonly EffectParameter _diffuseTexture;

        public WindowsSceneEffect(ContentManager content)
        {
            _basicEffect = content.Load<Effect>(ResourceNames.Effects.BasicEffect);
            PostProcessingEffect = content.Load<Effect>(ResourceNames.Effects.PostProcessing);

            _defaultTechnique = _basicEffect.Techniques["Default"];
            _shadowDepthTechnique = _basicEffect.Techniques["ShadowCaster"];
            _defaultWithShadowsTechnique = _basicEffect.Techniques["DefaultWithShadows"];
            _billboardTechnique = _basicEffect.Techniques["DefaultBillboard"];
            ShadowMapDebugEffect = content.Load<Effect>(ResourceNames.Effects.DebugShadowMap);

            _lightWorldViewProjection = _basicEffect.Parameters["LightWorldViewProjection"];
            _world = _basicEffect.Parameters["World"];
            _view = _basicEffect.Parameters["View"];
            _projection = _basicEffect.Parameters["Projection"];
            _lightDirection = _basicEffect.Parameters["LightDirection"];
            _shadowMap = _basicEffect.Parameters["ShadowMap"];
            _diffuseTexture = _basicEffect.Parameters["DiffuseTexture"];
        }

        public Effect ShadowMapDebugEffect { get; }
        
        public void ActivateShadowDepthMapPass()
        {
            _basicEffect.CurrentTechnique = _shadowDepthTechnique;
        }

        public void ActivateBillboardingTechnique()
        {
            _basicEffect.CurrentTechnique = _billboardTechnique;
        }

        public void ActivateLightingTechnique(bool withShadows)
        {
            _basicEffect.CurrentTechnique = withShadows ? _defaultWithShadowsTechnique : _defaultTechnique;
        }

        public Matrix LightWorldViewProjection
        {
            get { return _lightWorldViewProjection.GetValueMatrix(); }
            set { _lightWorldViewProjection.SetValue(value); }
        }

        public Matrix World
        {
            get { return _world.GetValueMatrix(); }
            set { _world.SetValue(value); }
        }

        public Matrix View
        {
            get { return _view.GetValueMatrix(); }
            set { _view.SetValue(value); }
        }

        public Matrix Projection
        {
            get { return _projection.GetValueMatrix(); }
            set { _projection.SetValue(value); }
        }

        public Vector3 LightDirection
        {
            get { return _lightDirection.GetValueVector3(); }
            set { _lightDirection.SetValue(value); }
        }

        public Texture2D ShadowMap
        {
            get { return _shadowMap.GetValueTexture2D(); }
            set { _shadowMap.SetValue(value); }
        }

        public Texture2D DiffuseTexture
        {
            get { return _diffuseTexture.GetValueTexture2D(); }
            set { _diffuseTexture.SetValue(value); }
        }

        public Effect PostProcessingEffect { get; }

        public IEnumerable<EffectPass> CurrentTechniquePasses
        {
            get { return _basicEffect.CurrentTechnique.Passes; }
        }
    }
}
