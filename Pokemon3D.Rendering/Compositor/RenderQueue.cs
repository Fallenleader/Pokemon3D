using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    internal class RenderQueue : GameContextObject
    {
        private readonly Action<Material> _handleEffect;
        private readonly Func<IEnumerable<SceneNode>> _getSceneNodes;
        private readonly List<DrawableElement> _elementsToDraw = new List<DrawableElement>();
        private bool _isOptimized;

        protected SceneEffect SceneEffect { get; }
        public BlendState BlendState { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public bool SortNodesBackToFront { get; set; }
        public bool IsEnabled { get; set; }

        public RenderQueue(GameContext context, 
                           Action<Material> handleEffect,
                           Func<IEnumerable<SceneNode>> getSceneNodes,
                           SceneEffect sceneEffect) : base(context)
        {
            _handleEffect = handleEffect;
            _getSceneNodes = getSceneNodes;
            SceneEffect = sceneEffect;
            _isOptimized = false;
            IsEnabled = true;
        }

        public virtual void Draw(Camera camera, Light light, bool hasSceneNodesChanged)
        {
            GameContext.GraphicsDevice.BlendState = BlendState;
            GameContext.GraphicsDevice.DepthStencilState = DepthStencilState;
            GameContext.GraphicsDevice.RasterizerState = RasterizerState;

            HandleBatching(hasSceneNodesChanged);

            var nodes = SortNodesBackToFront ? _elementsToDraw.OrderByDescending(n => (camera.GlobalPosition - n.GlobalPosition).LengthSquared()).ToList()
                                             : _elementsToDraw;

            for (var i = 0; i < nodes.Count; i++)
            {
                var element = nodes[i];

                if (!IsValidForRendering(camera, element)) continue;
                _handleEffect(element.Material);
                DrawElement(camera, element);
            }
        }

        protected virtual bool IsValidForRendering(Camera camera, DrawableElement element)
        {
            return element.IsActive && camera.Frustum.Contains(element.BoundingBox) != ContainmentType.Disjoint;
        }

        private void HandleBatching(bool hasSceneNodesChanged)
        {
            if (_isOptimized && !hasSceneNodesChanged) return;

            var sceneNodes = _getSceneNodes().ToArray();

            _elementsToDraw.Clear();

            var staticNodes = new List<SceneNode>();
            var dynamicNodes = new List<SceneNode>();
            for (var i = 0; i < sceneNodes.Length; i++)
            {
                if (sceneNodes[i].IsStatic)
                {
                    staticNodes.Add(sceneNodes[i]);
                }
                else
                {
                    dynamicNodes.Add(sceneNodes[i]);
                }
            }

            Texture2D currentTexture = null;
            StaticMeshBatch currentBatch = null;
            for (var i = 0; i < staticNodes.Count; i++)
            {
                var node = sceneNodes[i];

                if (currentTexture != node.Material.DiffuseTexture || i == 0)
                {
                    currentTexture = node.Material.DiffuseTexture;
                    currentBatch?.Build();
                    currentBatch = new StaticMeshBatch(GameContext, node.Material);
                    _elementsToDraw.Add(currentBatch);
                }

                if (currentBatch != null)
                {
                    var success = currentBatch.AddBatch(node);
                    if (!success)
                    {
                        currentBatch.Build();
                        currentBatch = new StaticMeshBatch(GameContext, node.Material);
                        _elementsToDraw.Add(currentBatch);
                    }
                }
            }
            currentBatch?.Build();

            _elementsToDraw.AddRange(dynamicNodes);

            _isOptimized = true;
        }

        private void DrawElement(Camera camera, DrawableElement element)
        {
            DrawElement(camera, element, SceneEffect);
        }

        internal static void DrawElement(Camera camera, DrawableElement element, SceneEffect sceneEffect)
        {
            sceneEffect.World = element.GetWorldMatrix(camera);
            sceneEffect.DiffuseTexture = element.Material.DiffuseTexture;
            sceneEffect.TexcoordScale = element.Material.TexcoordScale;
            sceneEffect.TexcoordOffset = element.Material.TexcoordOffset;

            for (var i = 0; i < sceneEffect.CurrentTechniquePasses.Count; i++)
            {
                sceneEffect.CurrentTechniquePasses[i].Apply();
                element.Mesh.Draw();
            }
        }
    }
}
