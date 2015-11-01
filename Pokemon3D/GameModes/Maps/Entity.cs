using System.Collections.Generic;
using System.Linq;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;
using Pokemon3D.GameModes.Maps.EntityComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.GameModes.Maps
{
    /// <summary>
    /// Represents a functional part of a map.
    /// </summary>
    class Entity : GameContextObject
    {
        public const string COMPONENT_NAME_STATIC = "isStatic";

        protected Scene Scene { get; private set; }
        protected SceneNode SceneNode { get; private set; }

        private readonly EntityModel _dataModel;
        private readonly Dictionary<string, EntityComponent> _components = new Dictionary<string, EntityComponent>();
        private readonly Map _map;

        private ResourceManager Resources => _map.ResourceManager;

        public Entity(Map map, EntityModel dataModel, Vector3 position):
            this(map.Scene)
        {
            _map = map;
            _dataModel = dataModel;

            InitializeComponents();

            SceneNode.Scale = dataModel.Scale.GetVector3();
            SceneNode.Position = position;

            var eulerAngles = dataModel.Rotation.GetVector3();
            SceneNode.Scale = dataModel.Scale.GetVector3();
            SceneNode.Rotation = Quaternion.CreateFromYawPitchRoll(eulerAngles.Y, eulerAngles.X, eulerAngles.Z);
            
            var renderMode = _dataModel.RenderMode;
            if (renderMode.RenderMethod == RenderMethod.Primitive)
            {
                SceneNode.Mesh = map.ResourceManager.GetMeshFromPrimitiveName(renderMode.PrimitiveModelId);
                SceneNode.IsBillboard = HasComponent(EntityComponentFactory.COMPONENT_ID_BILLBOARD);

                var texture = renderMode.Textures.First();

                var diffuseTexture = Resources.GetTexture2D(texture.Source);
                var material = new Material(diffuseTexture)
                {
                    Color = new Color(renderMode.Shading.GetVector3()),
                    CastShadow = false,
                    ReceiveShadow = false,
                    UseTransparency = SceneNode.IsBillboard
                };

                if (texture.Rectangle != null)
                {
                    material.TexcoordOffset = diffuseTexture.GetTexcoordsFromPixelCoords(texture.Rectangle.X,
                        texture.Rectangle.Y);
                    material.TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(texture.Rectangle.Width,
                        texture.Rectangle.Height);
                }

                SceneNode.Material = material;
            }
            else
            {
                //todo: model not yet supported.
            }
        }

        public Entity(Scene scene)
        {
            Scene = scene;
            SceneNode = scene.CreateSceneNode();
        }

        /// <summary>
        /// The absolute position of this entity in the world.
        /// </summary>
        public Vector3 Position
        {
            get { return SceneNode.Position; }
            set
            {
                if (!IsStatic)
                    SceneNode.Position = value;
            }
        }

        /// <summary>
        /// The scale of this entity, relative to <see cref="Vector3.One"/>.
        /// </summary>
        public Vector3 Scale
        {
            get { return SceneNode.Scale; }
            set
            {
                if (!IsStatic)
                    SceneNode.Scale = value;
            }
        }

        /// <summary>
        /// The rotation <see cref="Quaternion"/> of this entity.
        /// </summary>
        public Quaternion Rotation
        {
            get { return SceneNode.Rotation; }
            set
            {
                if (!IsStatic)
                    SceneNode.Rotation = value;
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
        
        void InitializeComponents()
        {
            // Loops over the data models of the entity components and creates actual instances.

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
