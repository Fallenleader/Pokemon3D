using System.Collections.Generic;
using System.Linq;
using Pokémon3D.DataModel.Json.GameMode.Map.Entities;
using Pokémon3D.GameModes.Maps.EntityComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;

namespace Pokémon3D.GameModes.Maps
{
    /// <summary>
    /// Represents a functional part of a map.
    /// </summary>
    class Entity
    {
        public const string COMPONENT_NAME_STATIC = "isStatic";

        private readonly SceneNode _sceneNode;
        private readonly EntityModel _dataModel;
        private Dictionary<string, EntityComponent> _components;
        private readonly Map _map;

        private Scene Scene => _map.Scene;
        private ResourceManager Resources => _map.ResourceManager;

        public Entity(Map map, EntityModel dataModel, Vector3 position)
        {
            _map = map;
            _dataModel = dataModel;

            _sceneNode = Scene.CreateSceneNode();
            _sceneNode.Scale = dataModel.Scale.GetVector3();
            _sceneNode.Position = position;

            var eulerAngles = dataModel.Rotation.GetVector3();
            _sceneNode.Scale = dataModel.Scale.GetVector3();
            _sceneNode.Rotation = Quaternion.CreateFromYawPitchRoll(eulerAngles.Y, eulerAngles.X, eulerAngles.Z);
            
            var renderMode = _dataModel.RenderMode;
            if (renderMode.RenderMethod == RenderMethod.Primitive)
            {
                _sceneNode.Mesh = map.ResourceManager.GetMeshFromPrimitiveName(renderMode.PrimitiveModelId);
                _sceneNode.IsBillboard = dataModel.Components.Any(c => c.Id == "isBillboard");

                var texture = renderMode.Textures.First();

                var diffuseTexture = Resources.GetTexture2D(texture.Source);
                var material = new Material(diffuseTexture)
                {
                    Color = new Color(renderMode.Shading.GetVector3()),
                    CastShadow = false,
                    ReceiveShadow = false,
                    UseTransparency = _sceneNode.IsBillboard
                };

                if (texture.Rectangle != null)
                {
                    material.TexcoordOffset = diffuseTexture.GetTexcoordsFromPixelCoords(texture.Rectangle.X,
                        texture.Rectangle.Y);
                    material.TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(texture.Rectangle.Width,
                        texture.Rectangle.Height);
                }

                _sceneNode.Material = material;
            }
            else
            {
                //todo: model not yet supported.
            }

            InitializeProperties();
        }

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
        
        void InitializeProperties()
        {
            // Loops over the data models of the entity components and creates actual instances.

            _components = new Dictionary<string, EntityComponent>();

            var factory = EntityComponentFactory.GetInstance();

            foreach (var compModel in _dataModel.Components)
            {
                if (!HasComponent(compModel.Id))
                {
                    var comp = factory.GetComponent(this, compModel);
                    _components.Add(compModel.Id.ToLowerInvariant(), comp);
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
