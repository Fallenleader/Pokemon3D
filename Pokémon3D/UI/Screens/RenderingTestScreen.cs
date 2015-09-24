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
        private SceneNode _cube;

        public void OnOpening()
        {
            _scene = new Scene(GameController.Instance);
            _scene.EnableShadows = true;
            _scene.LightDirection = new Vector3(1, -5, -1);

            _camera = _scene.CreateCamera();
            _camera.Position = new Vector3(0.0f, 12.0f, 13.0f);
            _camera.RotateX(-MathHelper.PiOver4);
            
            var cubeMesh = new Mesh(GameController.Instance.GraphicsDevice, Primitives.GenerateCubeData());
            var cubeMaterial = new Material(GameController.Instance.Content.Load<Texture2D>(ResourceNames.Textures.bricksSample));

            _cube = _scene.CreateSceneNode();
            _cube.Material = new Material(GameController.Instance.Content.Load<Texture2D>(ResourceNames.Textures.bricksSample) ){
                ReceiveShadow = false
            };
            _cube.Mesh = cubeMesh;
            _cube.Position = new Vector3(0.0f, 3.0f, 0.0f);

            GenerateCube(cubeMesh, cubeMaterial, new Vector3(15, 0.1f, 15), new Vector3(0, -0.05f, 0), Vector3.Zero);
            GenerateCube(cubeMesh, cubeMaterial, new Vector3(2, 2, 2), new Vector3(-3, 1, 5), new Vector3(0.0f, MathHelper.PiOver4, 0.0f));
            GenerateCube(cubeMesh, cubeMaterial, new Vector3(1, 4, 1), new Vector3(2, 2, -3), new Vector3(0.0f, 0.0f, 0.0f));
        }

        private void GenerateCube(Mesh mesh, Material material, Vector3 scale, Vector3 translation, Vector3 rotation)
        {
            var sceneNode = _scene.CreateSceneNode();
            sceneNode.Material = material;
            sceneNode.Mesh = mesh;
            sceneNode.Position = translation;
            sceneNode.Scale = scale;
            sceneNode.RotateX(rotation.X);
            sceneNode.RotateY(rotation.Y);
            sceneNode.RotateZ(rotation.Z);
        }

        public void OnUpdate(GameTime gameTime)
        {
            var elapsed = gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            _cube.RotateX(MathHelper.PiOver4 * elapsed);
            _cube.RotateY(MathHelper.Pi * elapsed);
            _cube.RotateZ(MathHelper.Pi * elapsed);
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
