using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.DataModel.Json.GameMode.Map;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;

namespace Pokemon3D.GameModes.Maps
{
    class Map
    {
        private readonly List<Entity> _allEntities = new List<Entity>();
        private readonly MapModel _mapModel;

        public Scene Scene { get; private set; }
        public ResourceManager ResourceManager { get; private set; }

        public Map(MapModel mapModel, Scene scene, ResourceManager resourceManager)
        {
            Scene = scene;
            ResourceManager = resourceManager;
            _mapModel = mapModel;

            foreach (var entityDefinition in _mapModel.Entities)
            {
                foreach (var entityPlacing in entityDefinition.Placing)
                {
                    PlaceEntities(entityDefinition, entityPlacing);
                }   
            }
        }

        private void PlaceEntities(EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing)
        {
            for (var x = 1.0f; x <= entityPlacing.Size.X; x += entityPlacing.Steps.X)
            {
                for (var y = 1.0f; y <= entityPlacing.Size.Y; y += entityPlacing.Steps.Y)
                {
                    for (var z = 1.0f; z <= entityPlacing.Size.Z; z += entityPlacing.Steps.Z)
                    {
                        var position = entityPlacing.Position.GetVector3() + new Vector3(x, y, z);
                        var entity = new Entity(this, entityDefinition.Entity, position);
                        _allEntities.Add(entity);
                    }
                }
            }

            
        }

        public void Update(float elapsedTime)
        {
        }
    }
}
