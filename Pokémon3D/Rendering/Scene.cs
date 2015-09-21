using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Pokémon3D.Rendering
{
    class Scene
    {
        private readonly List<SceneNode> _allNodes;
        private readonly List<Camera> _allCameras; 
        private readonly GraphicsDevice _device;
        private readonly Effect _basicEffect;
        private readonly RenderTarget2D _shadowMap;
        private SpriteBatch _spriteBatch;
        private Effect _shadowDepthDebugEffect;

        private EffectTechnique _shadowDepthTechnique;
        private EffectTechnique _defaultTechnique;
        private EffectTechnique _defaultWithShadowsTechnique;

        public bool EnableShadows { get; set; }
        public Vector3 LightDirection { get; set; }

        public Scene(Game game)
        {
            _device = game.GraphicsDevice;
            _allNodes = new List<SceneNode>();
            _allCameras = new List<Camera>();
            _basicEffect = game.Content.Load<Effect>(ResourceNames.Effects.BasicEffect);

            _shadowMap = new RenderTarget2D(game.GraphicsDevice, 1024, 1024,false, SurfaceFormat.Single, DepthFormat.Depth24);

            LightDirection = new Vector3(1, -1, -1);

            _defaultTechnique = _basicEffect.Techniques["Default"];
            _shadowDepthTechnique = _basicEffect.Techniques["ShadowCaster"];
            _defaultWithShadowsTechnique = _basicEffect.Techniques["DefaultWithShadows"];

            _spriteBatch = new SpriteBatch(_device);
            _shadowDepthDebugEffect = game.Content.Load<Effect>(ResourceNames.Effects.DebugShadowMap);
        }

        public SceneNode CreateSceneNode()
        {
            var sceneNode = new SceneNode();
            _allNodes.Add(sceneNode);
            return sceneNode;
        }

        public Camera CreateCamera()
        {
            var camera = new Camera(_device.Viewport);
            _allCameras.Add(camera);
            _allNodes.Add(camera);
            return camera;
        }

        public void Update(float elapsedTime)
        {
            foreach (var sceneNode in _allNodes)
            {
                sceneNode.Update();
            }
        }

        public void Draw()
        {
            if (EnableShadows) DrawShadowMap();

            foreach (var camera in _allCameras)
            {
                DrawSceneForCamera(camera);
            }

            //DrawShadowMapDebug();
        }

        private void DrawShadowMapDebug()
        {
            if (EnableShadows)
            {
                _spriteBatch.Begin(effect: _shadowDepthDebugEffect);
                _spriteBatch.Draw(_shadowMap, new Rectangle(0, 0, 128, 128), Color.White);
                _spriteBatch.End();
                _device.BlendState = BlendState.Opaque;
                _device.DepthStencilState = DepthStencilState.Default;
            }
        }

        private Matrix BuildLightViewMatrix()
        {
            LightDirection.Normalize();
            var lightProjection = Matrix.CreateOrthographic(21, 21, 1.0f, 60.0f);
            var lightView = Matrix.CreateLookAt(-LightDirection * 10.0f, Vector3.Zero, Vector3.Up);
            return lightView * lightProjection;
        }

        private void DrawShadowMap()
        {
            var oldRenderTargets = _device.GetRenderTargets();
            _device.SetRenderTarget(_shadowMap);
            _device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            var lightViewProjection = BuildLightViewMatrix();
            
            _basicEffect.CurrentTechnique = _shadowDepthTechnique;
            foreach (var sceneNode in _allNodes)
            {
                if (sceneNode.Mesh == null) continue;
                if (sceneNode.Material == null) throw new InvalidOperationException("Render Scene Node needs a material.");
                if (!sceneNode.Material.CastShadow) continue;

                _basicEffect.Parameters["LightWorldViewProjection"].SetValue(sceneNode.WorldMatrix * lightViewProjection);

                foreach (var pass in _basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    sceneNode.Mesh.Draw();
                }
            }
            _device.SetRenderTargets(oldRenderTargets);
        }

        private void DrawSceneForCamera(Camera camera)
        {
            _device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);

            _basicEffect.Parameters["View"].SetValue(camera.ViewMatrix);
            _basicEffect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            _basicEffect.Parameters["LightDirection"].SetValue(LightDirection);
            _basicEffect.CurrentTechnique = _defaultTechnique;

            var lightView = BuildLightViewMatrix();

            foreach (var sceneNode in _allNodes)
            {
                if (sceneNode.Mesh == null) continue;
                if (sceneNode.Material == null) throw new InvalidOperationException("Render Scene Node needs a material.");

                if (sceneNode.Material.ReceiveShadow)
                {
                    _basicEffect.CurrentTechnique = _defaultWithShadowsTechnique;
                    _basicEffect.Parameters["LightWorldViewProjection"].SetValue(sceneNode.WorldMatrix * lightView);
                    _basicEffect.Parameters["ShadowMap"].SetValue(_shadowMap);
                }
                else
                {
                    _basicEffect.CurrentTechnique = _defaultTechnique;
                }

                _basicEffect.Parameters["World"].SetValue(sceneNode.WorldMatrix);
                _basicEffect.Parameters["DiffuseTexture"].SetValue(sceneNode.Material.DiffuseTexture);

                foreach(var pass in _basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    sceneNode.Mesh.Draw();
                }
            }
        }
    }
}
