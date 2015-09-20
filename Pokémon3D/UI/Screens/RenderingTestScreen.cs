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
            _cube = _scene.CreateSceneNode();
            _cube.Material = new Material(_game.Content.Load<Texture2D>(ResourceNames.Textures.bricksSample));
            _cube.Mesh = new Mesh(_game.GraphicsDevice, Primitives.GenerateCubeData());
            _cube.Position = new Vector3(0.0f, 0.0f, -5.0f);
        }

        public void OnUpdate(object sender, GameFlowEventArgs e)
        {
            var elapsed = 1.0f / 60.0f;
            _cube.RotateX(MathHelper.PiOver4 * elapsed);
            //_cube.RotateY(MathHelper.Pi * elapsed);
            //_cube.RotateZ(MathHelper.Pi * elapsed);
            _scene.Update(elapsed);
        }

        public void OnDraw(object sender, GameFlowEventArgs e)
        {
            _scene.Draw();
        }
    }
}
