using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokémon3D.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokémon3D.UI.Screens
{
    class RenderingTestScreen : Screen
    {
        private Game _game;
        private Scene _scene;
        private Camera _camera;
        private SceneNode _cube;

        public RenderingTestScreen(Game game)
        {
            Opening += OnOpening;
            Update += OnUpdate;
            Draw += OnDraw;
            _game = game;
        }

        public void OnOpening(object sender, EventArgs e)
        {
            _scene = new Scene(_game);

            _camera = _scene.CreateCamera();
            _camera.Position = new Vector3(0.0f, 12.0f, 13.0f);
            _camera.RotateX(-MathHelper.PiOver4);
            
            var cubeMesh = new Mesh(_game.GraphicsDevice, Primitives.GenerateCubeData());
            var cubeMaterial = new Material(_game.Content.Load<Texture2D>(ResourceNames.Textures.bricksSample));

            _cube = _scene.CreateSceneNode();
            _cube.Material = cubeMaterial;
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

        public void OnUpdate(object sender, GameFlowEventArgs e)
        {
            var elapsed = 1.0f / 60.0f;
            _cube.RotateX(MathHelper.PiOver4 * elapsed);
            _cube.RotateY(MathHelper.Pi * elapsed);
            _cube.RotateZ(MathHelper.Pi * elapsed);
            _scene.Update(elapsed);
        }

        public void OnDraw(object sender, GameFlowEventArgs e)
        {
            _scene.Draw();
        }
    }
}
