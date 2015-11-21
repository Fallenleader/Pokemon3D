using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Compositor;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Representing a whole 3D Scene with all objects to display.
    /// </summary>
    public class Scene : GameContextObject
    {
        private readonly List<SceneNode> _allNodes;
        private readonly List<Camera> _allCameras; 

        public SceneRenderer Renderer { get; }
        public bool HasSceneNodesChanged { get; private set; }

        public Scene(GameContext context, SceneEffect effect) : base(context)
        {
            Renderer = new DefaultSceneRenderer(context, effect) { LightDirection = new Vector3(1, -1, -1)};
            _allNodes = new List<SceneNode>();
            _allCameras = new List<Camera>();
        }

        public SceneNode CreateSceneNode()
        {
            HasSceneNodesChanged = true;
            var sceneNode = new SceneNode();
            _allNodes.Add(sceneNode);
            return sceneNode;
        }
        
        public void RemoveSceneNode(SceneNode node)
        { 
            HasSceneNodesChanged = true;
            _allNodes.Remove(node);
        }

        public Camera CreateCamera()
        {
            HasSceneNodesChanged = true;
            var camera = new Camera(GameContext.GraphicsDevice.Viewport);
            _allCameras.Add(camera);
            _allNodes.Add(camera);
            return camera;
        }

        public void Update(float elapsedTime)
        {
            foreach (var sceneNode in _allNodes.OrderBy(n => n.Parent != null))
            {
                sceneNode.Update();
            }
        }

        public void Draw()
        {
            Renderer.Draw(HasSceneNodesChanged, _allNodes, _allCameras);

#if DEBUG_RENDERING
            if (Renderer.EnableShadows) Renderer.DrawDebugShadowMap(GameContext.SpriteBatch, new Rectangle(0,0,128,128));
#endif
        }

        /// <summary>
        /// Clones a Scene node with its children and all attached Properties.
        /// Meshes will only be cloned when <see cref="cloneMeshs"/> is true.
        /// </summary>
        /// <param name="nodeToClone">Node to clone</param>
        /// <param name="cloneMeshs">Force cloning mesh data.</param>
        /// <returns>SceneNode cloned.</returns>
        public SceneNode CloneNode(SceneNode nodeToClone, bool cloneMeshs = false)
        {
            HasSceneNodesChanged = true;
            var cloned = nodeToClone.Clone(cloneMeshs);
            _allNodes.Add(cloned);
            CloneChildren(cloned, nodeToClone, cloneMeshs);
            return cloned;
        }

        private void CloneChildren(SceneNode parentCloned, SceneNode parentOriginal, bool cloneMeshs)
        {
            foreach (var childNode in parentOriginal.Children)
            {
                var clonedChild = childNode.Clone(cloneMeshs);
                _allNodes.Add(clonedChild);
                parentCloned.AddChild(clonedChild);

                CloneChildren(clonedChild, childNode, cloneMeshs);
            }
        }
    }
}
