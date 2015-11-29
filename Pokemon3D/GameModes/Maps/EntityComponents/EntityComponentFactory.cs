using Pokemon3D.Common;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;
using Pokemon3D.GameModes.Maps.EntityComponents.Components;

namespace Pokemon3D.GameModes.Maps.EntityComponents
{
    /// <summary>
    /// A singleton factory to create <see cref="EntityComponent"/> instances.
    /// </summary>
    class EntityComponentFactory : Singleton<EntityComponentFactory>
    {
        private EntityComponentFactory() { }
        
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
                case EntityComponent.IDs.Billboard:
                    comp = new BillboardEntityComponent(parameters);
                    break;
                case EntityComponent.IDs.Static:
                    comp = new StaticEntityComponent(parameters);
                    break;
                case EntityComponent.IDs.Floor:
                    comp = new FloorEntityComponent(parameters);
                    break;
                case EntityComponent.IDs.NoCollision:
                    comp = new NoCollisionEntityComponent(parameters);
                    break;
                case EntityComponent.IDs.AnimateTextures:
                    comp = new AnimateTexturesEntityComponent(parameters);
                    break;
                default:
                    comp = new DataStorageEntityComponent(parameters);
                    break;
            }

            return comp;
        }
    }
}
