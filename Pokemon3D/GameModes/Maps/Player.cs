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
        private MouseState _mouseState;

        public float Speed { get; set; }
        public float RotationSpeed { get; set; }

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
            Speed = 2.0f;
            RotationSpeed = MathHelper.PiOver4;
            MovementMode = PlayerMovementMode.GodMode;
        }

        public void Update(float elapsedTime)
        {
            var currentMouseState = Mouse.GetState();

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

            switch (MovementMode)
            {
                case PlayerMovementMode.FirstPerson:
                    HandleFirstPersonMovement(elapsedTime, currentMouseState, movementDirection);
                    break;
                case PlayerMovementMode.ThirdPerson:
                    HandleThirdPersonMovement(elapsedTime, currentMouseState, movementDirection);
                    break;
                case PlayerMovementMode.GodMode:
                    HandleGodModeMovement(elapsedTime, currentMouseState, movementDirection);
                    break;
            }

            _mouseState = currentMouseState;
        }

        private void HandleGodModeMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            var speedFactor = Game.Keyboard.IsKeyDown(Keys.LeftShift) ? 2.0f : 1.0f;
            var step = Speed*elapsedTime*speedFactor;

            if (_mouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                var differenceX = mouseState.X - _mouseState.X;
                var differenceY = mouseState.Y - _mouseState.Y;

                _camera.RotateX(-differenceY * 0.1f * elapsedTime);
                _camera.RotateY(-differenceX * 0.1f * elapsedTime);
            }

            if (movementDirection.LengthSquared() > 0.0f)
            {
                _camera.Translate(Vector3.Normalize(movementDirection) * step);
            }
            if (Game.Keyboard.IsKeyDown(Keys.Space))
            {
                _camera.Position += Vector3.UnitY * step;
            }
        }

        private void HandleThirdPersonMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
        }

        private void HandleFirstPersonMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            if (movementDirection.LengthSquared() > 0.0f)
            {
                SceneNode.Translate(Vector3.Normalize(movementDirection) * Speed * elapsedTime);
            }

            if (Game.Keyboard.IsKeyDown(Keys.Left))
            {
                SceneNode.RotateY(RotationSpeed * elapsedTime);
            }
            else if (Game.Keyboard.IsKeyDown(Keys.Right))
            {
                SceneNode.RotateY(-RotationSpeed * elapsedTime);
            }
        }

        private void OnMovementModeChanged()
        {
            switch (MovementMode)
            {
                case PlayerMovementMode.FirstPerson:
                    break;
                case PlayerMovementMode.ThirdPerson:
                    break;
                case PlayerMovementMode.GodMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
