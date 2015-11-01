using Pokémon3D.DataModel.Json.GameMode.Map.Entities;
using Pokémon3D.GameModes.Maps.EntityComponents.Components;

namespace Pokémon3D.GameModes.Maps.EntityComponents
{
    /// <summary>
    /// A helper struct to easily move construction parameters around.
    /// </summary>
    struct EntityComponentDataCreationStruct
    {
        public string Name;
        public string Data;
        public Entity Parent;
    }
    
    /// <summary>
    /// A singleton factory to create <see cref="EntityComponent"/> instances.
    /// </summary>
    class EntityComponentFactory
    {
        private static EntityComponentFactory _instance;

        private EntityComponentFactory() { }

        /// <summary>
        /// Returns the singleton instance.
        /// </summary>
        public static EntityComponentFactory GetInstance()
        {
            if (_instance == null)
                _instance = new EntityComponentFactory();

            return _instance;
        }

        /// <summary>
        /// Creates an empty <see cref="DataStorageEntityComponent"/> with the given name.
        /// </summary>
        public EntityComponent GetComponent(Entity parent, string name)
        {
            return new DataStorageEntityComponent(new EntityComponentDataCreationStruct()
            {
                Name = name,
                Parent = parent
            });
        }

        /// <summary>
        /// Creates a new instance of an <see cref="EntityComponent"/>.
        /// </summary>
        public EntityComponent GetComponent(Entity parent, EntityComponentModel dataModel)
        {
            EntityComponent comp;

            var parameters = new EntityComponentDataCreationStruct()
            {
                Parent = parent,
                Data = dataModel.Data,
                Name = dataModel.Id
            };

            switch (dataModel.Id.ToLowerInvariant())
            {
                default:
                    comp = new DataStorageEntityComponent(parameters);
                    break;
            }

            return comp;
        }
    }
}
