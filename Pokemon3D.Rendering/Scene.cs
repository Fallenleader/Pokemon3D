using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Representing a whole 3D Scene with all objects to display.
    /// </summary>
    public class Scene
    {
        private readonly List<SceneNode> _allNodes;
        private readonly List<Camera> _allCameras; 
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;
        private readonly SceneRenderer _sceneRenderer;

        public bool EnableShadows { get; set; }
        public Vector3 LightDirection { get; set; }

        public Scene(Game game, SceneEffect effect)
        {
            _device = game.GraphicsDevice;
            _sceneRenderer = new DefaultSceneRenderer(game, effect);
            _allNodes = new List<SceneNode>();
            _allCameras = new List<Camera>();
            LightDirection = new Vector3(1, -1, -1);
            _spriteBatch = new SpriteBatch(_device);
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

        public void Draw()
        {
            _sceneRenderer.Draw(_allNodes, _allCameras, LightDirection, EnableShadows);
        }
    }
}
