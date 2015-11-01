using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.Compositor
{
    class DefaultSceneRenderer : SceneRenderer
    {
        private readonly GameContext _gameContext;
        private readonly GraphicsDevice _device;
        private readonly SceneEffect _sceneEffect;
        private readonly RenderTarget2D _shadowMap;
        private readonly List<SceneNode> _solidObjects = new List<SceneNode>();
        private readonly List<SceneNode> _transparentObjects = new List<SceneNode>();
        private readonly List<PostProcessingStep> _postProcessingSteps = new List<PostProcessingStep>();

        private RenderTarget2D _activeInputSource;
        private RenderTarget2D _activeRenderTarget;

        public DefaultSceneRenderer(GameContext context, SceneEffect effect)
        {
            _gameContext = context;
            _device = context.GraphicsDevice;
            _sceneEffect = effect;
            _shadowMap = new RenderTarget2D(context.GraphicsDevice, 1024, 1024, false, SurfaceFormat.Single, DepthFormat.Depth24);

            var width = context.ScreenBounds.Width;
            var height = context.ScreenBounds.Height;
            _activeInputSource = new RenderTarget2D(_device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
            _activeRenderTarget = new RenderTarget2D(_device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
        }

        public Vector3 LightDirection { get; set; }
        public bool EnableShadows { get; set; }
        public bool EnablePostProcessing { get; set; }

        public void AddPostProcessingStep(PostProcessingStep step)
        {
            step.Initialize(_sceneEffect.PostProcessingEffect);
            _postProcessingSteps.Add(step);
        }

        private RenderTargetBinding[] _oldBindings;

        private void PreparePostProcessing()
        {
            if (!EnablePostProcessing || !_postProcessingSteps.Any()) return;

            _oldBindings = _device.GetRenderTargets();
            _device.SetRenderTarget(_activeRenderTarget);
        }

        public void Draw(IList<SceneNode> allNodes, IList<Camera> cameras)
        {
            PreparePostProcessing();

            UpdateNodeLists(allNodes);

            if (EnableShadows)
            {
                DrawShadowMap();
            }

            foreach (var camera in cameras)
            {
                DrawSceneForCamera(camera);
            }

            DoPostProcessing();
        }

        private void DoPostProcessing()
        {
            if (!EnablePostProcessing) return;

            foreach (var postProcessingStep in _postProcessingSteps)
            {
                var temp = _activeRenderTarget;
                _activeRenderTarget = _activeInputSource;
                _activeInputSource = temp;
                _device.SetRenderTarget(_activeRenderTarget);

                postProcessingStep.Process(_gameContext, _activeInputSource, _activeRenderTarget);
            }

            _device.SetRenderTargets(_oldBindings);
            _gameContext.SpriteBatch.Begin();
            _gameContext.SpriteBatch.Draw(_activeRenderTarget, Vector2.Zero, Color.White);
            _gameContext.SpriteBatch.End();
        }

        public void DrawDebugShadowMap(SpriteBatch spriteBatch, Rectangle target)
        {
            spriteBatch.Begin(effect: _sceneEffect.ShadowMapDebugEffect);
            spriteBatch.Draw(_shadowMap, target, Color.White);
            spriteBatch.End();
            _device.BlendState = BlendState.Opaque;
            _device.DepthStencilState = DepthStencilState.Default;
        }

        private void DrawSceneForCamera(Camera camera)
        {
            _device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);

            _sceneEffect.View = camera.ViewMatrix;
            _sceneEffect.Projection = camera.ProjectionMatrix;
            _sceneEffect.LightDirection = LightDirection;

            var lightView = BuildLightViewMatrix(LightDirection);

            DrawSolidObjects(camera, lightView);
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
                _sceneEffect.TexcoordScale = sceneNode.Material.TexcoordScale;
                _sceneEffect.TexcoordOffset = sceneNode.Material.TexcoordOffset;

                foreach (var pass in _sceneEffect.CurrentTechniquePasses)
                {
                    pass.Apply();
                    sceneNode.Mesh.Draw();
                }
            }
        }

        private void DrawSolidObjects(Camera camera, Matrix lightView)
        {
            _device.BlendState = BlendState.Opaque;
            foreach (var sceneNode in _solidObjects)
            {
                var worldMatrix = sceneNode.GetWorldMatrix(camera);
                if (EnableShadows && sceneNode.Material.ReceiveShadow)
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
                _sceneEffect.TexcoordScale = sceneNode.Material.TexcoordScale;
                _sceneEffect.TexcoordOffset = sceneNode.Material.TexcoordOffset;

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

        private void DrawShadowMap()
        {
            var oldRenderTargets = _device.GetRenderTargets();
            _device.SetRenderTarget(_shadowMap);
            _device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            var lightViewProjection = BuildLightViewMatrix(LightDirection);

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
