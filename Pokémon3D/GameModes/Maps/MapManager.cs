using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return new Map();
        }
    }
}
