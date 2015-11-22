using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using System.Linq;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.DataModel.Json.GameMode.Map;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;

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
                    var prototypeModel = GetEntityPrototypeModel(_mapModel.EntityPrototypes, entityDefinition.Entity.ParentId);
                    foreach (var entityPlacing in entityDefinition.Placing)
                    {
                        PlaceEntities(prototypeModel, entityDefinition, entityPlacing, Vector3.Zero);
                    }
                }
            }
            if (_mapModel.Fragments != null)
            {
                foreach (var fragmentImport in _mapModel.Fragments)
                {
                    var fragmentModel = _gameMode.MapManager.GetMapFragment(fragmentImport.Id);
                    Vector3 fragmentOffset = fragmentImport.Position.GetVector3();

                    foreach (var entityDefinition in fragmentModel.Entities)
                    {
                        var prototypeModel = GetEntityPrototypeModel(fragmentModel.EntityPrototypes, entityDefinition.Entity.ParentId);
                        foreach (var entityPlacing in entityDefinition.Placing)
                        {
                            PlaceEntities(prototypeModel, entityDefinition, entityPlacing, fragmentOffset);
                        }
                    }
                }
            }
        }

        private void PlaceEntities(EntityPrototypeModel prototypeModel, EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 offset)
        {
            if (prototypeModel != null)
            {
                for (var x = 1.0f; x <= entityPlacing.Size.X; x += entityPlacing.Steps.X)
                {
                    for (var y = 1.0f; y <= entityPlacing.Size.Y; y += entityPlacing.Steps.Y)
                    {
                        for (var z = 1.0f; z <= entityPlacing.Size.Z; z += entityPlacing.Steps.Z)
                        {
                            var position = entityPlacing.Position.GetVector3() + new Vector3(x, y, z) + offset;
                            var entity = new Entity(this, prototypeModel, entityDefinition.Entity, position);
                            _allEntities.Add(entity);
                        }
                    }
                }
            }
        }

        private EntityPrototypeModel GetEntityPrototypeModel(EntityPrototypeModel[] source, string prototypeId)
        {
            if (source != null)
            {
                var results = source.Where(x => x.PrototypeId == prototypeId);
                if (results.Count() > 0)
                    return results.ElementAt(0);
            }

            return null;
        }

        public void Update(float elapsedTime)
        {
        }
    }
}
