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

        private readonly List<StaticMeshBatch> _staticBatches = new List<StaticMeshBatch>();
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

            var nodes = (SortNodesBackToFront ? _getSceneNodes().OrderByDescending(n => (camera.GlobalPosition - n.GlobalPosition).Length()) 
                                             : _getSceneNodes()).ToList();

            HandleBatching(nodes);

            foreach (var element in _elementsToDraw)
            {
                _handleEffect(element.Material);
                DrawElement(camera, element, renderStatistics);
            }
        }

        private void HandleBatching(IList<SceneNode>  sceneNodes)
        {
            if (_isOptimized) return;

            _staticBatches.Clear();
            _elementsToDraw.Clear();

            var staticNodes = new List<SceneNode>();
            var dynamicNodes = new List<SceneNode>();
            for (var i = 0; i < sceneNodes.Count; i++)
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

            foreach (var pass in _sceneEffect.CurrentTechniquePasses)
            {
                pass.Apply();
                element.Mesh.Draw();
                renderStatistics.DrawCalls++;
            }
        }
    }
}
