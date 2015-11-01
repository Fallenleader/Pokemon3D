using System.Collections.Generic;
using System.ComponentModel;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokémon3D.DataModel.Json.GameMode.Map;

namespace Pokémon3D.GameModes.Maps
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
                    var entity = new Entity(this, entityDefinition.Entity, entityPlacing.Position.GetVector3());
                    _allEntities.Add(entity);
                }   
            }
            
            _allEntities = new List<Entity>();
        } 

        public void Update(float elapsedTime)
        {
        }
    }
}
