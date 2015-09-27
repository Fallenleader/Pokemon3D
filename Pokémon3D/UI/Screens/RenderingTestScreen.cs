using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokémon3D.GameCore;
using Pokémon3D.Rendering;

namespace Pokémon3D.UI.Screens
{
    class RenderingTestScreen : Screen
    {
        private Scene _scene;
        private Camera _camera;

        public void OnOpening()
        {
            _scene = new Scene(GameController.Instance);
            _scene.EnableShadows = false;
            _scene.LightDirection = new Vector3(0,-1,0);

            _camera = _scene.CreateCamera();
            _camera.Position = new Vector3(0.0f, 12.0f, 13.0f);
            _camera.RotateX(-MathHelper.PiOver4);
            
            var billboardSceneNode = _scene.CreateSceneNode();
            billboardSceneNode.IsBillboard = true;
            billboardSceneNode.Mesh = new Mesh(GameController.Instance.GraphicsDevice, Primitives.GenerateQuadForYBillboard());
            billboardSceneNode.Material = new Material(GameController.Instance.Content.Load<Texture2D>(ResourceNames.Textures.tileset1));
            billboardSceneNode.Material.CastShadow = false;
            billboardSceneNode.Position = new Vector3(0, 0, 3);
            billboardSceneNode.Scale = new Vector3(2, 4, 1);

            GeometryData[] quads = new GeometryData[32*32];

            //2,1
            var tsize = 1.0f / 16.0f;
            for(var x = 0; x < 32; x++)
            {
                for(var z = 0; z < 32; z++)
                {
                    quads[z*32+x] = Primitives.GenerateQuadXZ(new Vector3(-16.0f + x, 0.0f, -16.0f + z), texCoordStart: new Vector2(tsize * 2.0f, tsize), texCoordScale: new Vector2(tsize, tsize));
                }
            }

            var sceneNode = _scene.CreateSceneNode();
            sceneNode.Mesh = new Mesh(GameController.Instance.GraphicsDevice, Primitives.Merge(quads));
            sceneNode.Material = new Material(GameController.Instance.Content.Load<Texture2D>(ResourceNames.Textures.tileset1));
        }

        public void OnUpdate(GameTime gameTime)
        {
            var elapsed = gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            _scene.Update(elapsed);
        }

        public void OnDraw(GameTime gameTime)
        {
            _scene.Draw();
        }

        public void OnClosing()
        {
        }
    }
}
