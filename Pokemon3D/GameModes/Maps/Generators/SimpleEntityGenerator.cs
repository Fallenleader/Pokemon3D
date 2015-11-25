using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;

namespace Pokemon3D.GameModes.Maps.Generators
{
    class SimpleEntityGenerator : Singleton<SimpleEntityGenerator>, EntityGenerator
    {
        public List<Entity> Generate(Map map, EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 position)
        {
            return new List<Entity>() { new Entity(map, entityDefinition.Entity, entityPlacing, position) };
        }
    }
}
