using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel;
using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.GameMode.Map;

namespace Pokemon3D.GameModes.Maps
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
