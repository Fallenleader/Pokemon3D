using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Rendering;

namespace Pokemon3D.GameModes.Maps
{
    class Player : Entity
    {
        private readonly Camera _camera;
        private PlayerMovementMode _movementMode;

        public PlayerMovementMode MovementMode
        {
            get { return _movementMode; }
            set
            {
                if (_movementMode != value)
                {
                    _movementMode = value;
                    OnMovementModeChanged();
                }
            }
        }
        
        public Player(Scene scene) : base(scene)
        {
            SceneNode.Position = new Vector3(0,0,0);

            _camera = scene.CreateCamera();
            _camera.SetParent(SceneNode);
            _camera.Position = new Vector3(6.0f, 2.0f, 14.0f);
        }

        public void Update(float elapsedTime)
        {
            var movementDirection = Vector3.Zero;
            if (Game.Keyboard.IsKeyDown(Keys.A))
            {
                movementDirection.X = -1.0f;
            }
            else if (Game.Keyboard.IsKeyDown(Keys.D))
            {
                movementDirection.X = 1.0f;
            }

            if (Game.Keyboard.IsKeyDown(Keys.W))
            {
                movementDirection.Z = 1.0f;
            }
            else if (Game.Keyboard.IsKeyDown(Keys.S))
            {
                movementDirection.Z = -1.0f;
            }

            if (movementDirection.LengthSquared() > 0.0f)
            {
                SceneNode.Translate(Vector3.Normalize(movementDirection) * 2.0f * elapsedTime);
            }

            if (Game.Keyboard.IsKeyDown(Keys.Left))
            {
                SceneNode.RotateY(MathHelper.PiOver4 * elapsedTime);
            }
            else if (Game.Keyboard.IsKeyDown(Keys.Right))
            {
                SceneNode.RotateY(-MathHelper.PiOver4 * elapsedTime);
            }
        }

        private void OnMovementModeChanged()
        {
            throw new NotImplementedException();
        }
    }
}
