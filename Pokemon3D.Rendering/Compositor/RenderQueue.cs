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
        private readonly SceneEffect _sceneEffect;

        private readonly List<DrawableElement> _elementsToDraw = new List<DrawableElement>();
        private bool _isOptimized;

        public BlendState BlendState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public bool SortNodesBackToFront { get; set; }

        public RenderQueue(GameContext context, 
                           Action<Material> handleEffect, 
                           Func<IEnumerable<SceneNode>> getSceneNodes,  
                           SceneEffect sceneEffect) : base(context)
        {
            _handleEffect = handleEffect;
            _getSceneNodes = getSceneNodes;
            _sceneEffect = sceneEffect;
            _isOptimized = false;
        }

        public void Draw(Camera camera, RenderStatistics renderStatistics)
        {
            GameContext.GraphicsDevice.BlendState = BlendState;
            GameContext.GraphicsDevice.DepthStencilState = DepthStencilState;

            HandleBatching();

            var nodes = SortNodesBackToFront ? _elementsToDraw.OrderByDescending(n => (camera.GlobalPosition - n.GlobalPosition).LengthSquared()).ToList()
                                             : _elementsToDraw;

            for (var i = 0; i < nodes.Count; i++)
            {
                var element = nodes[i];

                if (!element.IsActive) continue;
                if (camera.Frustum.Contains(element.BoundingBox) == ContainmentType.Disjoint) continue;
                _handleEffect(element.Material);
                DrawElement(camera, element, renderStatistics);
            }
        }

        private void HandleBatching()
        {
            if (_isOptimized) return;

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

                if (currentTexture != node.Material.DiffuseTexture)
                {
                    currentTexture = node.Material.DiffuseTexture;
                    currentBatch?.Build();
                    currentBatch = new StaticMeshBatch(GameContext, node.Material);
                    _elementsToDraw.Add(currentBatch);
                }
                currentBatch?.AddBatch(node);
            }
            currentBatch?.Build();

            _elementsToDraw.AddRange(dynamicNodes);

            _isOptimized = true;
        }

        private void DrawElement(Camera camera, DrawableElement element, RenderStatistics renderStatistics)
        {
            _sceneEffect.World = element.GetWorldMatrix(camera);
            _sceneEffect.DiffuseTexture = element.Material.DiffuseTexture;
            _sceneEffect.TexcoordScale = element.Material.TexcoordScale;
            _sceneEffect.TexcoordOffset = element.Material.TexcoordOffset;

            for (var i = 0; i < _sceneEffect.CurrentTechniquePasses.Count; i++)
            {
                _sceneEffect.CurrentTechniquePasses[i].Apply();
                element.Mesh.Draw();
                renderStatistics.DrawCalls++;
            }
        }
    }
}
