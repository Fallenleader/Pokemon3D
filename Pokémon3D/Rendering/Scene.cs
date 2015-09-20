using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Pokémon3D.Rendering
{
    class Scene
    {
        private readonly List<SceneNode> _allNodes;
        private readonly List<Camera> _allCameras; 
        private readonly GraphicsDevice _device;

        public Scene(GraphicsDevice device)
        {
            _device = device;
            _allNodes = new List<SceneNode>();
            _allCameras = new List<Camera>();
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

        public void Draw(GraphicsDevice device)
        {
            foreach (var camera in _allCameras)
            {
                DrawSceneForCamera(camera);
            }
        }

        private void DrawSceneForCamera(Camera camera)
        {
            foreach (var sceneNode in _allNodes)
            {
                if (sceneNode.Mesh == null) continue;

                sceneNode.Mesh.Draw();
            }
        }
    }
}
