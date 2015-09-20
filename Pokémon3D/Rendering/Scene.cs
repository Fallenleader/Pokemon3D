using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Pokémon3D.Rendering
{
    class Scene
    {
        private readonly List<SceneNode> _allNodes;

        public Scene()
        {
            _allNodes = new List<SceneNode>();
        }

        public SceneNode CreateSceneNode()
        {
            var sceneNode = new SceneNode();
            _allNodes.Add(sceneNode);
            return sceneNode;
        }

        public void Update(float elapsedTime)
        {
            foreach (var sceneNode in _allNodes)
            {
                sceneNode.Update();
            }
        }

        public void Draw(GraphicsDevice device)
        {
            foreach ( var sceneNode in _allNodes)
            {
                if (sceneNode.Mesh == null) continue;

                sceneNode.Mesh.Draw();
            }
        }
    }
}
