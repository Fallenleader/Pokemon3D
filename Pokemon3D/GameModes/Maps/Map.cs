using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using System.Linq;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.DataModel.Json.GameMode.Map;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;
using Pokemon3D.GameModes.Maps.Generators;

namespace Pokemon3D.GameModes.Maps
{
    class Map
    {
        private readonly GameMode _gameMode;
        private readonly List<Entity> _allEntities = new List<Entity>();
        private readonly MapModel _mapModel;

        public Scene Scene { get; private set; }
        public ResourceManager ResourceManager { get; private set; }

        public Map(GameMode gameMode, MapModel mapModel, Scene scene, ResourceManager resourceManager)
        {
            Scene = scene;
            ResourceManager = resourceManager;
            _gameMode = gameMode;
            _mapModel = mapModel;

            if (_mapModel.Entities != null)
            {
                foreach (var entityDefinition in _mapModel.Entities)
                {
                    foreach (var entityPlacing in entityDefinition.Placing)
                    {
                        PlaceEntities(entityDefinition, entityPlacing, Vector3.Zero);
                    }
                }
            }
            if (_mapModel.Fragments != null)
            {
                foreach (var fragmentImport in _mapModel.Fragments)
                {
                    var fragmentModel = _gameMode.MapManager.GetMapFragment(fragmentImport.Id);

                    foreach (var position in fragmentImport.Positions)
                    {
                        Vector3 fragmentOffset = position.GetVector3();

                        foreach (var entityDefinition in fragmentModel.Entities)
                        {
                            foreach (var entityPlacing in entityDefinition.Placing)
                            {
                                PlaceEntities(entityDefinition, entityPlacing, fragmentOffset);
                            }
                        }
                    }
                }
            }
        }

        private void PlaceEntities(EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 offset)
        {
            var generator = EntityGeneratorSupplier.GetGenerator(entityDefinition.Entity.Generator);
            for (var x = 1.0f; x <= entityPlacing.Size.X; x += entityPlacing.Steps.X)
            {
                for (var y = 1.0f; y <= entityPlacing.Size.Y; y += entityPlacing.Steps.Y)
                {
                    for (var z = 1.0f; z <= entityPlacing.Size.Z; z += entityPlacing.Steps.Z)
                    {
                        var position = entityPlacing.Position.GetVector3() + new Vector3(x, y, z) + offset;

                        _allEntities.AddRange(generator.Generate(this, entityDefinition, entityPlacing, position));
                    }
                }
            }
        }

        public void Update(float elapsedTime)
        {
            _allEntities.ForEach(e => e.Update(elapsedTime));
        }
    }
}
