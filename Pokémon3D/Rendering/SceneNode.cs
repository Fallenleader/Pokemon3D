using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Pokémon3D.Rendering
{
    class SceneNode
    {
        private readonly List<SceneNode> _childNodes;
        private Vector3 _rotationAxis;

        public SceneNode Parent { get; private set; }
        public ReadOnlyCollection<SceneNode> Children { get; private set; }
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }

        private Vector3 _position;
        private Vector3 _scale;
        private Quaternion _rotation;
        private bool _isDirty;
        private Matrix _localWorldMatrix;
        private Matrix _world;

        public Vector3 Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                _isDirty = true;
            }
        }

        public Quaternion Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _isDirty = true;
            }
        }

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _isDirty = true;
            }
        }
        
        public Vector3 Right { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Forward { get; private set; }
        public bool IsBillboard { get; set; }
        
        public SceneNode()
        {
            _childNodes = new List<SceneNode>();
            Children = _childNodes.AsReadOnly();
            _scale = Vector3.One;
            _rotation = Quaternion.Identity;
            Right = Vector3.Right;
            Up = Vector3.Up;
            Forward = Vector3.Forward;
            _isDirty = true;
        }

        public void SetParent(SceneNode parent)
        {
            if (Parent != null) Parent.RemoveChild(this);
            parent.AddChild(this);
        }

        public void AddChild(SceneNode childElement)
        {
            if (childElement.Parent != null)
            {
                childElement.Parent.RemoveChild(childElement);
            }
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

        public virtual void Update()
        {
            if (IsBillboard) return;
            if (Parent == null)
            {
                _world = GetLocalWorldMatrix();
            }
            else
            {
                _world = Parent._world * GetLocalWorldMatrix();
            }
        }

        private Matrix GetLocalWorldMatrix()
        {
            if (_isDirty)
            {
                _localWorldMatrix = CalculateLocalWorldMatrix();
            }
            return _localWorldMatrix;
        }

        public Matrix GetWorldMatrix(Camera currentCamera)
        {
            return IsBillboard ? CalculateBillboardMatrix(currentCamera) : _world;
        }

        private Matrix CalculateBillboardMatrix(Camera currentCamera)
        {
            return Matrix.CreateScale(Scale) * Matrix.CreateConstrainedBillboard(Position, currentCamera.Position, Vector3.Up, null, null);
        }

        private Matrix CalculateLocalWorldMatrix()
        {
            return Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position);
        }

        public void Translate(Vector3 translation)
        {
            Position += Right * translation.X + Up * translation.Y + Forward * translation.Z;
        }

        public void RotateX(float angle)
        {
            var matrix = Matrix.CreateFromAxisAngle(Right, angle);
            _rotationAxis.X += angle;
            Rotation = Quaternion.CreateFromYawPitchRoll(_rotationAxis.Y, _rotationAxis.X, _rotationAxis.Z);

            Up = Vector3.Transform(Up, matrix);
            Forward = Vector3.Transform(Forward, matrix);
        }

        public void RotateY(float angle)
        {
            var matrix = Matrix.CreateFromAxisAngle(Up, angle);
            _rotationAxis.Y += angle;
            Rotation = Quaternion.CreateFromYawPitchRoll(_rotationAxis.Y, _rotationAxis.X, _rotationAxis.Z);

            Right = Vector3.Transform(Right, matrix);
            Forward = Vector3.Transform(Forward, matrix);
        }

        public void RotateZ(float angle)
        {
            var matrix = Matrix.CreateFromAxisAngle(Forward, angle);
            _rotationAxis.Z += angle;
            Rotation = Quaternion.CreateFromYawPitchRoll(_rotationAxis.Y, _rotationAxis.X, _rotationAxis.Z);

            Up = Vector3.Transform(Up, matrix);
            Right = Vector3.Transform(Right, matrix);
        }
    }
}
