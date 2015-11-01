using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokémon3D.DataModel.Json.GameMode.Map;

namespace Pokémon3D.GameModes.Maps
{
    class MapManager
    {
        private readonly MapModel[] _availableMaps;

        public MapManager(MapModel[] availableMaps)
        {
            _availableMaps = availableMaps;
        }

        public Map LoadMap(string mapName, Scene scene, ResourceManager resourceManager)
        {
            var mapModel = _availableMaps.Single(m => m.Name == mapName);
            return new Map(mapModel, scene, resourceManager);
        }
    }
}
