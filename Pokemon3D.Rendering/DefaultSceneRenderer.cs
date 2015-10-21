using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering
{
    class DefaultSceneRenderer : SceneRenderer
    {
        private readonly GraphicsDevice _device;
        private readonly SceneEffect _sceneEffect;
        private readonly RenderTarget2D _shadowMap;
        private readonly List<SceneNode> _solidObjects = new List<SceneNode>();
        private readonly List<SceneNode> _transparentObjects = new List<SceneNode>();

        public DefaultSceneRenderer(GameContext context, SceneEffect effect)
        {
            _device = context.GraphicsDevice;
            _sceneEffect = effect;
            _shadowMap = new RenderTarget2D(context.GraphicsDevice, 1024, 1024, false, SurfaceFormat.Single, DepthFormat.Depth24);
        }
        public void Draw(IList<SceneNode> allNodes, IList<Camera> cameras, Vector3 lightDirection, bool enableShadows)
        {
            UpdateNodeLists(allNodes);

            if (enableShadows)
            {
                DrawShadowMap(lightDirection);
            }

            foreach (var camera in cameras)
            {
                DrawSceneForCamera(camera, lightDirection, enableShadows);
            }
        }

        public void DrawDebugShadowMap(SpriteBatch spriteBatch, Rectangle target)
        {
            spriteBatch.Begin(effect: _sceneEffect.ShadowMapDebugEffect);
            spriteBatch.Draw(_shadowMap, target, Color.White);
            spriteBatch.End();
            _device.BlendState = BlendState.Opaque;
            _device.DepthStencilState = DepthStencilState.Default;
        }

        private void DrawSceneForCamera(Camera camera, Vector3 lightDirection, bool enableShadows)
        {
            _device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);

            _sceneEffect.View = camera.ViewMatrix;
            _sceneEffect.Projection = camera.ProjectionMatrix;
            _sceneEffect.LightDirection = lightDirection;

            var lightView = BuildLightViewMatrix(lightDirection);

            DrawSolidObjects(camera, lightView, enableShadows);
            DrawTransparentObjects(camera);
        }

        private void DrawTransparentObjects(Camera camera)
        {
            _device.BlendState = BlendState.AlphaBlend;
            foreach (var sceneNode in _transparentObjects)
            {
                var worldMatrix = sceneNode.GetWorldMatrix(camera);
                if (sceneNode.IsBillboard)
                {
                    _sceneEffect.ActivateBillboardingTechnique();
                }

                _sceneEffect.World = worldMatrix;
                _sceneEffect.DiffuseTexture = sceneNode.Material.DiffuseTexture;

                foreach (var pass in _sceneEffect.CurrentTechniquePasses)
                {
                    pass.Apply();
                    sceneNode.Mesh.Draw();
                }
            }
        }

        private void DrawSolidObjects(Camera camera, Matrix lightView, bool enableShadows)
        {
            _device.BlendState = BlendState.Opaque;
            foreach (var sceneNode in _solidObjects)
            {
                var worldMatrix = sceneNode.GetWorldMatrix(camera);
                if (enableShadows && sceneNode.Material.ReceiveShadow)
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

                foreach (var pass in _sceneEffect.CurrentTechniquePasses)
                {
                    pass.Apply();
                    sceneNode.Mesh.Draw();
                }
            }
        }

        private static Matrix BuildLightViewMatrix(Vector3 lightDirection)
        {
            lightDirection.Normalize();
            var lightProjection = Matrix.CreateOrthographic(21, 21, 1.0f, 60.0f);
            var lightView = Matrix.CreateLookAt(-lightDirection * 10.0f, Vector3.Zero, Vector3.Up);
            return lightView * lightProjection;
        }

        private void DrawShadowMap(Vector3 lightDirection)
        {
            var oldRenderTargets = _device.GetRenderTargets();
            _device.SetRenderTarget(_shadowMap);
            _device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            var lightViewProjection = BuildLightViewMatrix(lightDirection);

            _sceneEffect.ActivateShadowDepthMapPass();
            foreach (var sceneNode in _solidObjects)
            {
                if (!sceneNode.Material.CastShadow) continue;

                _sceneEffect.LightWorldViewProjection = sceneNode.GetWorldMatrix(null) * lightViewProjection;

                foreach (var pass in _sceneEffect.CurrentTechniquePasses)
                {
                    pass.Apply();
                    sceneNode.Mesh.Draw();
                }
            }
            _device.SetRenderTargets(oldRenderTargets);
        }

        private void UpdateNodeLists(IList<SceneNode> allNodes)
        {
            _solidObjects.Clear();
            _transparentObjects.Clear();

            for (var i = 0; i < allNodes.Count; i++)
            {
                var node = allNodes[i];
                if (node.Mesh == null || node.Material == null) continue;

                if (node.Material.UseTransparency)
                {
                    _transparentObjects.Add(node);
                }
                else
                {
                    _solidObjects.Add(node);
                }
            }
        }
    }
}
