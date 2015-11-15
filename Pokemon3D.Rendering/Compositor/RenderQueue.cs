using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Compositor
{
    internal class RenderQueue
    {
        private readonly GraphicsDevice _device;
        private readonly Action<SceneNode> _handleEffect;
        private readonly Func<IEnumerable<SceneNode>> _getSceneNodes;
        private readonly SceneEffect _sceneEffect;

        public BlendState BlendState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public bool SortNodesBackToFront { get; set; }

        public RenderQueue(GraphicsDevice device, Action<SceneNode> handleEffect, Func<IEnumerable<SceneNode>> getSceneNodes,  SceneEffect sceneEffect)
        {
            _device = device;
            _handleEffect = handleEffect;
            _getSceneNodes = getSceneNodes;
            _sceneEffect = sceneEffect;
        }

        public void Draw(Camera camera)
        {
            _device.BlendState = BlendState;
            _device.DepthStencilState = DepthStencilState;

            var nodes = SortNodesBackToFront ? _getSceneNodes().OrderByDescending(n => (camera.GlobalPosition - n.GlobalPosition).Length()) 
                                             : _getSceneNodes();

            foreach (var sceneNode in nodes)
            {
                var worldMatrix = sceneNode.GetWorldMatrix(camera);
                _handleEffect(sceneNode);
                DrawElement(worldMatrix, sceneNode);
            }
        }

        private void DrawElement(Matrix worldMatrix, DrawableElement element)
        {
            _sceneEffect.World = worldMatrix;
            _sceneEffect.DiffuseTexture = element.Material.DiffuseTexture;
            _sceneEffect.TexcoordScale = element.Material.TexcoordScale;
            _sceneEffect.TexcoordOffset = element.Material.TexcoordOffset;

            foreach (var pass in _sceneEffect.CurrentTechniquePasses)
            {
                pass.Apply();
                element.Mesh.Draw();
            }
        }
    }
}
