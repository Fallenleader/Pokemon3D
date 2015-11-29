using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Animations;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.GameModes.Maps
{
    class Player : Entity
    {
        private readonly Animator _figureAnimator;
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
            _camera = scene.CreateCamera();
            _camera.SetParent(SceneNode);
            _camera.FarClipDistance = 50.0f;

            _camera.Skybox = new Skybox(Game)
            {
                Scale = 50,
                Texture = Game.Content.Load<Texture2D>(ResourceNames.Textures.skybox_texture)
            };

            Speed = 2.0f;
            RotationSpeed = MathHelper.PiOver4;

            SceneNode.Mesh = new Mesh(Game.GraphicsDevice, Primitives.GenerateQuadForYBillboard());
            var diffuseTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.DefaultGuy);
            SceneNode.Material = new Material
            {
                DiffuseTexture = diffuseTexture,
                UseTransparency = true,
                TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(32, 32),
                IsUnlit = true
            };
            SceneNode.Position = new Vector3(10, 1, 8);

            _figureAnimator = new Animator();
            _figureAnimator.AddAnimation("WalkForward", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 0),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 0),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 0),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 0),
            }, t => SceneNode.Material.TexcoordOffset = t, true));
            _figureAnimator.AddAnimation("WalkLeft", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 32),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 32),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 32),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 32),
            }, t => SceneNode.Material.TexcoordOffset = t, true));
            _figureAnimator.AddAnimation("WalkRight", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 96),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 96),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 96),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 96),
            }, t => SceneNode.Material.TexcoordOffset = t, true));
            _figureAnimator.AddAnimation("WalkBackward", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 64),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 64),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 64),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 64),
            }, t => SceneNode.Material.TexcoordOffset = t, true));

            MovementMode = PlayerMovementMode.ThirdPerson;
        }
        
        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);

            _figureAnimator.Update(elapsedTime);

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

        private void ActivateWalkingAnimation(string animationName)
        {
            if (_figureAnimator.CurrentAnimation == null || _figureAnimator.CurrentAnimationName != animationName)
            {
                _figureAnimator.SetAnimation(animationName);
            }
        }

        private void DeactivateWalkingAnimation()
        {
            if (_figureAnimator.CurrentAnimation != null)
            {
                _figureAnimator.Stop();
                SceneNode.Material.TexcoordOffset = Vector2.Zero;
            }
        }

        private void HandleGodModeMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            var speedFactor = Game.Keyboard.IsKeyDown(Keys.LeftShift) ? 2.0f : 1.0f;
            var step = Speed * elapsedTime * speedFactor;

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
            if (movementDirection.LengthSquared() > 0.0f)
            {
                SceneNode.Translate(Vector3.Normalize(movementDirection) * Speed * elapsedTime);

                if (movementDirection.X > 0.0f)
                {
                    ActivateWalkingAnimation("WalkRight");
                }
                else if (movementDirection.X < 0.0f)
                {
                    ActivateWalkingAnimation("WalkLeft");
                }
                else
                {
                    if (movementDirection.Z > 0.0f)
                    {
                        ActivateWalkingAnimation("WalkForward");
                    }
                    else
                    {
                        ActivateWalkingAnimation("WalkBackward");
                    }
                }
            }
            else
            {
                DeactivateWalkingAnimation();
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
                    SceneNode.IsActive = true;
                    _camera.SetParent(SceneNode);
                    _camera.EulerAngles = Vector3.Zero;
                    _camera.Position = new Vector3(0, 1, 0);
                    break;
                case PlayerMovementMode.ThirdPerson:
                    SceneNode.IsActive = true;
                    _camera.SetParent(SceneNode);
                    _camera.EulerAngles = Vector3.Zero;
                    _camera.Position = new Vector3(0, 1, 3);
                    break;
                case PlayerMovementMode.GodMode:
                    _camera.SetParent(null);
                    SceneNode.IsActive = false;
                    _camera.Position = SceneNode.GlobalPosition + new Vector3(0, 1, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
