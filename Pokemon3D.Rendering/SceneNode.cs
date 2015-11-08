using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Part of a Scene with contains Transformation and Optional Rendering Attachments.
    /// SceneNodes can be arranged in a hierarchy to allow complex transformations.
    /// </summary>
    public class SceneNode
    {
        private readonly List<SceneNode> _childNodes;
        
        public SceneNode Parent { get; private set; }
        public ReadOnlyCollection<SceneNode> Children { get; private set; }
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }

        private Vector3 _rotationAxis;
        private Vector3 _position;
        private Vector3 _scale;
        private Quaternion _rotation;
        private bool _isDirty;
        private Matrix _localWorldMatrix;
        private Matrix _world;
        private Vector3 _globalPosition;
        private Vector3 _globalEulerAngles;

        internal SceneNode()
        {
            _childNodes = new List<SceneNode>();
            Children = _childNodes.AsReadOnly();
            _scale = Vector3.One;
            _rotation = Quaternion.Identity;
            Right = Vector3.Right;
            Up = Vector3.Up;
            Forward = new Vector3(0, 0, -1);
            SetDirty();
        }

        public Vector3 Scale
        {
            get
            {
                HandleIsDirty();
                return _scale;
            }
            set
            {
                _scale = value;
                SetDirty();
            }
        }

        public Quaternion Rotation
        {
            get
            {
                HandleIsDirty();
                return _rotation;
            }
            set
            {
                _rotation = value;
                SetDirty();
            }
        }

        public Vector3 EulerAngles
        {
            get
            {
                HandleIsDirty();
                return _rotationAxis;
            }
        }

        public Vector3 GlobalEulerAngles
        {
            get
            {
                HandleIsDirty();
                return _globalEulerAngles;
            }
            private set { _globalEulerAngles = value; }
        }

        public Vector3 Position
        {
            get
            {
                HandleIsDirty();
                return _position;
            }
            set
            {
                _position = value;
                SetDirty();
            }
        }

        public Vector3 GlobalPosition
        {
            get
            {
                HandleIsDirty();
                return _globalPosition;
            }
            private set { _globalPosition = value; }
        }

        public Vector3 Right { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Forward { get; private set; }
        public bool IsBillboard { get; set; }
        
        public void SetParent(SceneNode parent)
        {
            Parent?.RemoveChild(this);
            parent?.AddChild(this);
        }

        public void AddChild(SceneNode childElement)
        {
            childElement.Parent?.RemoveChild(childElement);
            _childNodes.Add(childElement);
            childElement.Parent = this;
        }

        public void RemoveChild(SceneNode childElement)
        {
            if (_childNodes.Remove(childElement))
            {
                childElement.Parent = null;
            }
        }

        public void Update()
        {
            if (IsBillboard) return;
            HandleIsDirty();
        }

        public void Translate(Vector3 translation)
        {
            Position += Right * translation.X + Up * translation.Y + Forward * translation.Z;
            SetDirty();
        }

        public void RotateX(float angle)
        {
            var matrix = Matrix.CreateFromAxisAngle(Right, angle);
            _rotationAxis.X += angle;
            Rotation = Quaternion.CreateFromYawPitchRoll(_rotationAxis.Y, _rotationAxis.X, _rotationAxis.Z);

            Up = Vector3.Transform(Up, matrix);
            Forward = Vector3.Transform(Forward, matrix);
            SetDirty();
        }

        public void RotateY(float angle)
        {
            var matrix = Matrix.CreateFromAxisAngle(Up, angle);
            _rotationAxis.Y += angle;
            Rotation = Quaternion.CreateFromYawPitchRoll(_rotationAxis.Y, _rotationAxis.X, _rotationAxis.Z);

            Right = Vector3.Transform(Right, matrix);
            Forward = Vector3.Transform(Forward, matrix);
            SetDirty();
        }

        public void RotateZ(float angle)
        {
            var matrix = Matrix.CreateFromAxisAngle(Forward, angle);
            _rotationAxis.Z += angle;
            Rotation = Quaternion.CreateFromYawPitchRoll(_rotationAxis.Y, _rotationAxis.X, _rotationAxis.Z);

            Up = Vector3.Transform(Up, matrix);
            Right = Vector3.Transform(Right, matrix);
            SetDirty();
        }

        protected void SetDirty()
        {
            _isDirty = true;
            _childNodes.ForEach(c => c.SetDirty());
        }

        protected virtual void HandleIsDirty()
        {
            if (!_isDirty) return;

            var localWorldMatrix = Matrix.CreateScale(_scale)*Matrix.CreateFromQuaternion(_rotation)*
                                   Matrix.CreateTranslation(_position);

            _world = Parent == null ? localWorldMatrix : Parent._world * localWorldMatrix;

            if (Parent != null)
            {
                GlobalPosition = new Vector3(_world.M41, _world.M42, _world.M43);
                GlobalEulerAngles = Parent.GlobalEulerAngles + _rotationAxis;
            }
            else
            {
                GlobalPosition = _position;
                GlobalEulerAngles = _rotationAxis;
            }
            

            _isDirty = false;
        }

        public Matrix GetWorldMatrix(Camera currentCamera)
        {
            return IsBillboard ? CalculateBillboardMatrix(currentCamera) : _world;
        }

        private Matrix CalculateBillboardMatrix(SceneNode currentCamera)
        {
            return Matrix.CreateScale(Scale)* Matrix.CreateRotationY(currentCamera.GlobalEulerAngles.Y) * Matrix.CreateTranslation(Position);
        }

        internal SceneNode Clone(bool cloneMesh)
        {
            var sceneNode = new SceneNode
            {
                Mesh = cloneMesh ? Mesh?.Clone() : Mesh,
                Material = Material.Clone(),
                Position = Position,
                IsBillboard = IsBillboard,
                Scale = Scale,
                Forward = Forward,
                Right = Right,
                Rotation = Rotation,
                Up = Up
            };
            sceneNode.SetParent(Parent);
            return sceneNode;
        }
    }
}
