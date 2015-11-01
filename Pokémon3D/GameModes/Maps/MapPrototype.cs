using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokémon3D.DataModel;
using Pokémon3D.DataModel.Json;
using Pokémon3D.DataModel.Json.GameMode.Map;

namespace Pokémon3D.GameModes.Maps
{
    class MapPrototype : IDataModelContainer
    {
        private MapModel _dataModel;

        public bool IsValid
        {
            get
            {
                return true;
            }
        }
    }
}
