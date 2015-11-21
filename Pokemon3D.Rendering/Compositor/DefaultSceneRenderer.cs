using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    class DefaultSceneRenderer : GameContextObject, SceneRenderer
    {
        private readonly GraphicsDevice _device;
        private readonly SceneEffect _sceneEffect;
        private readonly RenderTarget2D _shadowMap;
        private RenderTargetBinding[] _oldBindings;

        private readonly List<SceneNode> _solidObjects = new List<SceneNode>();
        private readonly List<SceneNode> _transparentObjects = new List<SceneNode>();
        private readonly List<PostProcessingStep> _postProcessingSteps = new List<PostProcessingStep>();

        private RenderTarget2D _activeInputSource;
        private RenderTarget2D _activeRenderTarget;

        private readonly List<RenderQueue> _renderQueues; 

        public DefaultSceneRenderer(GameContext context, SceneEffect effect) : base(context)
        {
            _device = context.GraphicsDevice;
            _sceneEffect = effect;
            _shadowMap = new RenderTarget2D(context.GraphicsDevice, 1024, 1024, false, SurfaceFormat.Single, DepthFormat.Depth24);

            var width = context.ScreenBounds.Width;
            var height = context.ScreenBounds.Height;
            _activeInputSource = new RenderTarget2D(_device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
            _activeRenderTarget = new RenderTarget2D(_device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
            RenderStatistics = new RenderStatistics();

            _renderQueues = new List<RenderQueue>
            {
                new RenderQueue(context, HandleSolidObjects, GetSolidObjects, _sceneEffect)
                {
                    DepthStencilState = DepthStencilState.Default,
                    BlendState = BlendState.Opaque
                },
                new RenderQueue(context, HandleEffectTransparentObjects, GetTransparentObjects, _sceneEffect)
                {
                    DepthStencilState = DepthStencilState.DepthRead,
                    BlendState = BlendState.AlphaBlend,
                    SortNodesBackToFront = true
                }
            };
        }

        public Vector3 LightDirection { get; set; }
        public bool EnableShadows { get; set; }
        public bool EnablePostProcessing { get; set; }
        public RenderStatistics RenderStatistics { get; }

        private IEnumerable<SceneNode> GetTransparentObjects()
        {
            return _transparentObjects;
        }

        private IEnumerable<SceneNode> GetSolidObjects()
        {
            return _solidObjects;
        }
        
        public void AddPostProcessingStep(PostProcessingStep step)
        {
            step.Initialize(_sceneEffect.PostProcessingEffect);
            _postProcessingSteps.Add(step);
        }

        public void Draw(bool hasSceneNodesChanged, IList<SceneNode> allNodes, IList<Camera> cameras)
        {
            RenderStatistics.StartFrame();
            PreparePostProcessing();

            UpdateNodeLists(allNodes);

            foreach (var camera in cameras)
            {
                DrawSceneForCamera(camera);
            }

            DoPostProcessing();
            RenderStatistics.EndFrame();
        }

        private void HandleSolidObjects(Material material)
        {
            _sceneEffect.ActivateLightingTechnique(false);
        }

        private void HandleEffectTransparentObjects(Material material)
        {
            if (material.IsUnlit)
            {
                _sceneEffect.ActivateBillboardingTechnique();
            }
        }

        private void PreparePostProcessing()
        {
            if (!EnablePostProcessing || !_postProcessingSteps.Any()) return;

            _oldBindings = _device.GetRenderTargets();
            _device.SetRenderTarget(_activeRenderTarget);
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

                postProcessingStep.Process(GameContext, _activeInputSource, _activeRenderTarget);
            }

            _device.SetRenderTargets(_oldBindings);
            GameContext.SpriteBatch.Begin();
            GameContext.SpriteBatch.Draw(_activeRenderTarget, Vector2.Zero, Color.White);
            GameContext.SpriteBatch.End();
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

            foreach (var renderQueue in _renderQueues)
            {
                renderQueue.Draw(camera, RenderStatistics);
            }
        }

        private void UpdateNodeLists(IList<SceneNode> allNodes)
        {
            _solidObjects.Clear();
            _transparentObjects.Clear();

            for (var i = 0; i < allNodes.Count; i++)
            {
                var node = allNodes[i];
                if (node.Mesh == null || node.Material == null || !node.IsActive) continue;

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
