using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;
// ReSharper disable ForCanBeConvertedToForeach

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
        private readonly List<SceneNode> _shadowCastersObjects = new List<SceneNode>();
        private readonly List<PostProcessingStep> _postProcessingSteps = new List<PostProcessingStep>();

        private RenderTarget2D _activeInputSource;
        private RenderTarget2D _activeRenderTarget;

        private readonly List<RenderQueue> _renderQueues;
        private readonly RenderQueue _shadowCasterQueue;
        private bool _enableShadows;


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

            _shadowCasterQueue = new ShadowCastRenderQueue(context, HandleShadowCasterObjects, GetShadowCasterSceneNodes, _sceneEffect)
            {
                BlendState = BlendState.Opaque,
                DepthStencilState = DepthStencilState.Default,
                IsEnabled = true,
                SortNodesBackToFront = false,
                ShadowMap = _shadowMap
            };

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

        private IEnumerable<SceneNode> GetShadowCasterSceneNodes()
        {
            return _shadowCastersObjects;
        }

        public Vector3 LightDirection { get; set; }
        public bool EnablePostProcessing { get; set; }
        public RenderStatistics RenderStatistics { get; }

        public bool EnableShadows
        {
            get { return _enableShadows; }
            set
            {
                if (_enableShadows != value)
                {
                    _enableShadows = value;
                    _shadowCasterQueue.IsEnabled = value;
                }
            }
        }

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

            for (var i = 0; i < cameras.Count; i++)
            {
                DrawSceneForCamera(cameras[i]);
            }

            DoPostProcessing();
            RenderStatistics.EndFrame();
        }

        private void HandleSolidObjects(Material material)
        {
            _sceneEffect.ActivateLightingTechnique(false, false);
        }

        private void HandleEffectTransparentObjects(Material material)
        {
            _sceneEffect.ActivateLightingTechnique(material.IsUnlit, false);
        }

        private void HandleShadowCasterObjects(Material material)
        {
            _sceneEffect.ActivateShadowDepthMapPass();
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

            for (var i = 0; i <  _postProcessingSteps.Count; i++)
            {
                var temp = _activeRenderTarget;
                _activeRenderTarget = _activeInputSource;
                _activeInputSource = temp;
                _device.SetRenderTarget(_activeRenderTarget);

                _postProcessingSteps[i].Process(GameContext, _activeInputSource, _activeRenderTarget);
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
            if (EnableShadows)
            {
                _shadowCasterQueue.Draw(camera, LightDirection, RenderStatistics);
            }

            _device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);

            _sceneEffect.View = camera.ViewMatrix;
            _sceneEffect.Projection = camera.ProjectionMatrix;
            _sceneEffect.LightDirection = LightDirection;

            for(var i = 0; i < _renderQueues.Count; i++)
            {
                var renderQueue = _renderQueues[i];
                if (!renderQueue.IsEnabled) continue;
                renderQueue.Draw(camera, LightDirection, RenderStatistics);
            }
        }

        private void UpdateNodeLists(IList<SceneNode> allNodes)
        {
            _solidObjects.Clear();
            _transparentObjects.Clear();
            _shadowCastersObjects.Clear();

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

                if (node.Material.CastShadow && !node.Material.UseTransparency)
                {
                    _shadowCastersObjects.Add(node);
                }
            }
        }
    }
}
