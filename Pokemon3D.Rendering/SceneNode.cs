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
        private bool _isDirty;
        private Matrix _world;
        private Vector3 _globalPosition;
        private Vector3 _globalEulerAngles;
        private Vector3 _right;
        private Vector3 _up;
        private Vector3 _forward;
        private bool _isActive;

        internal SceneNode()
        {
            _isActive = true;
            _childNodes = new List<SceneNode>();
            Children = _childNodes.AsReadOnly();
            _scale = Vector3.One;
            Right = Vector3.Right;
            Up = Vector3.Up;
            Forward = Vector3.Forward;
            SetDirty();
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    _childNodes.ForEach(c => c.IsActive = _isActive);
                }
                
            }
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

        public Vector3 EulerAngles
        {
            get
            {
                HandleIsDirty();
                return _rotationAxis;
            }
            set
            {
                _rotationAxis = value;
                SetDirty();
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

        public Vector3 Right
        {
            get
            {
                HandleIsDirty();
                return _right;
            }
            private set { _right = value; }
        }

        public Vector3 Up
        {
            get
            {
                HandleIsDirty();
                return _up;
            }
            private set { _up = value; }
        }

        public Vector3 Forward
        {
            get
            {
                HandleIsDirty();
                return _forward;
            }
            private set { _forward = value; }
        }

        public bool IsBillboard { get; set; }
        
        public void SetParent(SceneNode parent)
        {
            Parent?.RemoveChild(this);
            parent?.AddChild(this);
            SetDirty();
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
            EulerAngles += new Vector3(angle, 0, 0);
            SetDirty();
        }

        public void RotateY(float angle)
        {
            EulerAngles += new Vector3(0, angle, 0);
            SetDirty();
        }

        public void RotateZ(float angle)
        {
            EulerAngles += new Vector3(0, 0, angle);
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

            _globalEulerAngles = Parent != null ? Parent.GlobalEulerAngles + _rotationAxis : _rotationAxis;

            var localWorldMatrix = Matrix.CreateScale(_scale)*Matrix.CreateFromYawPitchRoll(_rotationAxis.Y, _rotationAxis.X, _rotationAxis.Z) *
                                   Matrix.CreateTranslation(_position);

            Parent?.HandleIsDirty();
            _world = Parent == null ? localWorldMatrix :localWorldMatrix * Parent._world;

            if (Parent != null)
            {
                GlobalPosition = new Vector3(_world.M41, _world.M42, _world.M43);
            }
            else
            {
                GlobalPosition = _position;
            }

            var rotationMatrix = Matrix.CreateFromYawPitchRoll(_globalEulerAngles.Y, _globalEulerAngles.X, _globalEulerAngles.Z);
            _right = Vector3.TransformNormal(Vector3.Right, rotationMatrix);
            _up = Vector3.TransformNormal(Vector3.Up, rotationMatrix);
            _forward = Vector3.TransformNormal(Vector3.Forward, rotationMatrix);

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
                Up = Up,
                EulerAngles = EulerAngles
            };
            sceneNode.SetParent(Parent);
            return sceneNode;
        }
    }
}
