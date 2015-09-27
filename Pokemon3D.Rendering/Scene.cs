using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Representing a whole 3D Scene with all objects to display.
    /// </summary>
    public class Scene
    {
        private readonly List<SceneNode> _allNodes;
        private readonly List<Camera> _allCameras; 
        private readonly GraphicsDevice _device;
        private readonly RenderTarget2D _shadowMap;
        private readonly SpriteBatch _spriteBatch;
        private readonly SceneEffect _sceneEffect;

        public bool EnableShadows { get; set; }
        public Vector3 LightDirection { get; set; }

        public Scene(Game game, SceneEffect effect)
        {
            _device = game.GraphicsDevice;
            _sceneEffect = effect;
            _allNodes = new List<SceneNode>();
            _allCameras = new List<Camera>();

            _shadowMap = new RenderTarget2D(game.GraphicsDevice, 1024, 1024,false, SurfaceFormat.Single, DepthFormat.Depth24);

            LightDirection = new Vector3(1, -1, -1);
            _spriteBatch = new SpriteBatch(_device);
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
                _spriteBatch.Begin(effect: _sceneEffect.ShadowMapDebugEffect);
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
            
            _sceneEffect.ActivateShadowDepthMapPass();
            foreach (var sceneNode in _allNodes)
            {
                if (sceneNode.Mesh == null) continue;
                if (sceneNode.Material == null) throw new InvalidOperationException("Render Scene Node needs a material.");
                if (!sceneNode.Material.CastShadow) continue;

                _sceneEffect.LightWorldViewProjection = sceneNode.GetWorldMatrix(null)*lightViewProjection;

                foreach (var pass in _sceneEffect.CurrentTechniquePasses)
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

            _sceneEffect.View = camera.ViewMatrix;
            _sceneEffect.Projection = camera.ProjectionMatrix;
            _sceneEffect.LightDirection = LightDirection;

            var lightView = BuildLightViewMatrix();

            foreach (var sceneNode in _allNodes.OrderBy(n => n.IsBillboard))
            {
                if (sceneNode.Mesh == null) continue;
                if (sceneNode.Material == null) throw new InvalidOperationException("Render Scene Node needs a material.");
                
                var worldMatrix = sceneNode.GetWorldMatrix(camera);
                if (sceneNode.IsBillboard)
                {
                    _device.BlendState = BlendState.AlphaBlend;
                    _sceneEffect.ActivateBillboardingTechnique();
                }
                else if(EnableShadows && sceneNode.Material.ReceiveShadow)
                {
                    _sceneEffect.ActivateLightingTechnique(true);
                    _sceneEffect.LightWorldViewProjection = worldMatrix*lightView;
                    _sceneEffect.ShadowMap = _shadowMap;
                }
                else 
                {
                    _sceneEffect.ActivateLightingTechnique(false);
                }

                _sceneEffect.World = worldMatrix;
                _sceneEffect.DiffuseTexture = sceneNode.Material.DiffuseTexture;

                foreach(var pass in _sceneEffect.CurrentTechniquePasses)
                {
                    pass.Apply();
                    sceneNode.Mesh.Draw();
                }

                _device.BlendState = BlendState.Opaque;
            }
        }
    }
}
