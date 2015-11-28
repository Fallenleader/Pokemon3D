using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering
{
    public class Skybox : GameContextObject
    {
        private readonly Mesh _skyBoxModel;

        internal SceneNode SceneNode { get; }

        public Texture2D Texture
        {
            get { return SceneNode.Material.DiffuseTexture; }
            set { SceneNode.Material.DiffuseTexture = value; }
        }

        public float Scale
        {
            get { return -SceneNode.Scale.X; }
            set { SceneNode.Scale = new Vector3(-value); }
        }

        public Skybox(GameContext gameContext) : base(gameContext)
        {
            SceneNode = new SceneNode();

            var height = 1.0f / 3.0f;
            var width = 0.25f;
            var coords = new[]
            {
                new Vector2(1.0f*width,1.0f*height), new Vector2(width, height),
                new Vector2(3.0f*width,1.0f*height), new Vector2(width, height),
                new Vector2(2.0f*width,1.0f*height), new Vector2(width, height),
                new Vector2(0.0f*width,1.0f*height), new Vector2(width, height),
                new Vector2(1.0f*width,0.0f*height), new Vector2(width, height),
                new Vector2(1.0f*width,2.0f*height), new Vector2(width, height),
            };
            
            _skyBoxModel = new Mesh(GameContext.GraphicsDevice, Primitives.GenerateCubeData(coords));
            SceneNode.Mesh = _skyBoxModel;
            SceneNode.Material = new Material(null)
            {
                CastShadow = false,
                ReceiveShadow = false,
                UseTransparency = false
            };
            Scale = 1.0f;
        }

        internal void Update(Camera camera)
        {
            SceneNode.Position = camera.GlobalPosition;
            SceneNode.Update();
        }
    }
}