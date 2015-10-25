using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.GUI;
using Pokémon3D.GameCore;

namespace Pokémon3D.UI.Screens
{
    class RenderingTestScreen : GameContextObject, Screen
    {
        private Scene _scene;
        private Camera _camera;
        private GuiPanel _pauseMenuPanel;

        public void OnOpening()
        {
            _scene = new Scene(Game, new WindowsSceneEffect(Game.Content));
            _scene.Renderer.EnableShadows = false;
            _scene.Renderer.LightDirection = new Vector3(0, -1, 0);

            _camera = _scene.CreateCamera();
            _camera.Position = new Vector3(0.0f, 12.0f, 13.0f);
            _camera.RotateX(-MathHelper.PiOver4);
            
            var billboardSceneNode = _scene.CreateSceneNode();
            billboardSceneNode.IsBillboard = true;
            billboardSceneNode.Mesh = new Mesh(Game.GraphicsDevice, Primitives.GenerateQuadForYBillboard());
            billboardSceneNode.Material = new Material(Game.Content.Load<Texture2D>(ResourceNames.Textures.tileset1))
            {
                CastShadow = false,
                UseTransparency = true
            };
            billboardSceneNode.Position = new Vector3(-5, 0, -5);
            billboardSceneNode.Scale = new Vector3(2, 4, 1);

            for (var x = 0; x < 5; x++)
            {
                for(var z = 0; z < 5; z++)
                {
                    if (x == 0 && z == 0) continue;
                    var cloned = _scene.CloneNode(billboardSceneNode);
                    cloned.Position = new Vector3(x*2-5, 0, z * 2-5);
                }
            }

            var quads = new GeometryData[32*32];

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
            sceneNode.Mesh = new Mesh(Game.GraphicsDevice, Primitives.Merge(quads));
            sceneNode.Material = new Material(Game.Content.Load<Texture2D>(ResourceNames.Textures.tileset1));

            _pauseMenuPanel = new GuiPanel(Game)
            {
                IsEnabled = false
            };
            var root = Game.GuiSystem.CreateGuiHierarchyFromXml<GuiElement>("Content/GUI/PauseMenu.xml");
            _pauseMenuPanel.AddElement(root);
            root.FindGuiElementById<Button>("ContinueButton").Click += OnContinueButtonClick;
            root.FindGuiElementById<Button>("BackToMainMenuButton").Click += OnBackToMainMenuButtonClick;
        }

        private void OnBackToMainMenuButtonClick()
        {
            Game.ScreenManager.SetScreen(typeof(MainMenuScreen));
        }

        private void OnContinueButtonClick()
        {
            _pauseMenuPanel.IsEnabled = false;
        }

        public void OnUpdate(GameTime gameTime)
        {
            var elapsed = gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            _scene.Update(elapsed);
            _pauseMenuPanel.Update(elapsed);

            if (Game.Keyboard.IsKeyDownOnce(Keys.Escape))
            {
                _pauseMenuPanel.IsEnabled = !_pauseMenuPanel.IsEnabled;
            }
        }

        public void OnDraw(GameTime gameTime)
        {
            _scene.Draw();

            Game.SpriteBatch.Begin();
            _pauseMenuPanel.Draw();
            Game.SpriteBatch.End();
        }

        public void OnClosing()
        {
        }
    }
}
