using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Compositor;

namespace Pokemon3D.UI
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
        private readonly EffectParameter _texcoordOffset;
        private readonly EffectParameter _texcoordScale;
        private readonly EffectParameter _ambientLight;

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
            _texcoordOffset = _basicEffect.Parameters["TexcoordOffset"];
            _texcoordScale = _basicEffect.Parameters["TexcoordScale"];
            _ambientLight = _basicEffect.Parameters["AmbientLight"];
        }

        public Effect ShadowMapDebugEffect { get; }
        
        public void ActivateShadowDepthMapPass()
        {
            _basicEffect.CurrentTechnique = _shadowDepthTechnique;
        }

        public void ActivateLightingTechnique(bool unlit, bool receiveShadows)
        {
            if (unlit)
            {
                _basicEffect.CurrentTechnique = _billboardTechnique;
            }
            else
            {
                _basicEffect.CurrentTechnique = receiveShadows ? _defaultWithShadowsTechnique : _defaultTechnique;
            }
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

        public Vector2 TexcoordOffset
        {
            get { return _texcoordOffset.GetValueVector2(); }
            set { _texcoordOffset.SetValue(value); }
        }

        public Vector2 TexcoordScale
        {
            get { return _texcoordScale.GetValueVector2(); }
            set { _texcoordScale.SetValue(value); }
        }

        public EffectPassCollection CurrentTechniquePasses => _basicEffect.CurrentTechnique.Passes;

        public Vector4 AmbientLight
        {
            get { return _ambientLight.GetValueVector4(); }
            set { _ambientLight.SetValue(value);}
        }
    }
}
