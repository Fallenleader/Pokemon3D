using System.Collections.Generic;
using System.Linq;
using Pokémon3D.DataModel.Json.GameMode.Map.Entities;
using Pokémon3D.GameModes.Maps.EntityComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Scene;

namespace Pokémon3D.GameModes.Maps
{
    /// <summary>
    /// The rendermode for an entity.
    /// </summary>
    enum EntityRenderMode
    {
        Primitive,
        Model
    }

    /// <summary>
    /// The cardinal directions an entity can face.
    /// </summary>
    enum EntityFaceDirection
    {
        North = 0,
        West = 1,
        South = 2,
        East = 3
    }

    /// <summary>
    /// Represents a functional part of a map.
    /// </summary>
    class Entity
    {
        public const string COMPONENT_NAME_STATIC = "isStatic";

        private SceneNode _sceneNode;

        private EntityModel _dataModel;
        private Dictionary<string, EntityComponent> _components;

        private Model _model = null;
        private Texture2D _texture;

        private float _cameraDistance = 0f;

        /// <summary>
        /// The absolute position of this entity in the world.
        /// </summary>
        public Vector3 Position
        {
            get { return _sceneNode.Position; }
            set
            {
                if (!IsStatic)
                    _sceneNode.Position = value;
            }
        }

        /// <summary>
        /// The scale of this entity, relative to <see cref="Vector3.One"/>.
        /// </summary>
        public Vector3 Scale
        {
            get { return _sceneNode.Scale; }
            set
            {
                if (!IsStatic)
                    _sceneNode.Scale = value;
            }
        }

        /// <summary>
        /// The rotation <see cref="Quaternion"/> of this entity.
        /// </summary>
        public Quaternion Rotation
        {
            get { return _sceneNode.Rotation; }
            set
            {
                if (!IsStatic)
                    _sceneNode.Rotation = value;
            }
        }

        /// <summary>
        /// Returns, if this entity is marked as static. When an entity is static, it cannot modify its position, rotation and scale.
        /// </summary>
        public bool IsStatic
        {
            get { return HasComponent(COMPONENT_NAME_STATIC); }
            set
            {
                if (value && !IsStatic)
                {
                    AddComponent(EntityComponentFactory.GetInstance().GetComponent(this, COMPONENT_NAME_STATIC));
                }
                else if (!value && IsStatic)
                {
                    RemoveComponent(COMPONENT_NAME_STATIC);
                }
            }
        }

        public Entity(EntityModel dataModel, Vector3 position)
        {
            _dataModel = dataModel;

            //_sceneNode = new SceneNode(); //TODO: Factory?

            Position = position;
            Scale = dataModel.Scale.GetVector3();
            //TODO: create rotation quaternion from rotation vector3 in data model.

            InitializeProperties();
        }

        void InitializeProperties()
        {
            // Loops over the data models of the entity components and creates actual instances.

            _components = new Dictionary<string, EntityComponent>();

            var factory = EntityComponentFactory.GetInstance();

            foreach (var compModel in _dataModel.Components)
            {
                if (!HasComponent(compModel.Name))
                {
                    var comp = factory.GetComponent(this, compModel);
                    _components.Add(compModel.Name.ToLowerInvariant(), comp);
                }
            }
        }

        public void AddComponent(EntityComponent component)
        {
            if (!HasComponent(component.Name))
                _components.Add(component.Name.ToLowerInvariant(), component);
        }

        public void RemoveComponent(string componentName)
        {
            if (HasComponent(componentName))
                _components.Remove(componentName.ToLowerInvariant());
        }

        /// <summary>
        /// Returns a component of this <see cref="Entity"/>.
        /// </summary>
        public EntityComponent GetComponent(string componentName)
        {
            if (HasComponent(componentName))
                return _components[componentName.ToLowerInvariant()];

            return null;
        }

        /// <summary>
        /// Returns a component of a specific type of this <see cref="Entity"/>.
        /// </summary>
        public T GetComponent<T>(string componentName) where T : EntityComponent
        {
            if (HasComponent<T>(componentName))
                return (T)_components[componentName.ToLowerInvariant()];

            return null;
        }

        /// <summary>
        /// Returns if this <see cref="Entity"/> has a component of a specific name.
        /// </summary>
        public bool HasComponent(string componentName)
        {
            return _components.Keys.Contains(componentName.ToLowerInvariant());
        }

        /// <summary>
        /// Returns if this <see cref="Entity"/> has a component of a specific type and name.
        /// </summary>
        public bool HasComponent<T>(string componentName) where T : EntityComponent
        {
            var component = GetComponent(componentName);
            return component != null && component.GetType() == typeof(T);
        }
    }
}
