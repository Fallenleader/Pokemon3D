using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Pokémon3D.Rendering
{
    class SceneNode : IWorldDataContainer
    {
        private readonly List<SceneNode> _childNodes;
        private Vector3 _rotationAxis;

        public SceneNode Parent { get; private set; }
        public ReadOnlyCollection<SceneNode> Children { get; private set; }
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }

        private WorldCreator _worldCreator;

        private Vector3 _position;
        private Vector3 _scale;
        private Quaternion _rotation;

        public Vector3 Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                _worldCreator.SetDirty(WorldDirtyFlags.Scale);
            }
        }

        public Quaternion Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _worldCreator.SetDirty(WorldDirtyFlags.Rotation);
            }
        }

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _worldCreator.SetDirty(WorldDirtyFlags.Position);
            }
        }
        
        public Matrix World { get; private set; }

        public Vector3 Right { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Forward { get; private set; }
        
        public SceneNode()
        {
            _childNodes = new List<SceneNode>();
            Children = _childNodes.AsReadOnly();
            _worldCreator = new WorldCreator(this);
            _scale = Vector3.One;
            _rotation = Quaternion.Identity;
            Right = Vector3.Right;
            Up = Vector3.Up;
            Forward = Vector3.Forward;
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
            if (Parent == null)
                World = _worldCreator.GetWorldMatrix();
            else
                World = Parent.World * _worldCreator.GetWorldMatrix();
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
