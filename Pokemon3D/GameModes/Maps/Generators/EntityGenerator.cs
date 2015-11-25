using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;

namespace Pokemon3D.GameModes.Maps.Generators
{
    /// <summary>
    /// Classes to generate entities based on patterns from map files.
    /// </summary>
    interface EntityGenerator
    {
        List<Entity> Generate(Map map, EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 position);
    }
}
