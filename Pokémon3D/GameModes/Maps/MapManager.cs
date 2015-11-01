using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;

namespace Pokémon3D.GameModes.Maps
{
    class MapManager
    {
        Dictionary<string, MapPrototype> _prototypes;

        public MapManager()
        {
            _prototypes = new Dictionary<string, MapPrototype>();
        }

        public Map LoadMap(string mapName)
        {
            return new Map(scene, resourceManager);
        }
    }
}
