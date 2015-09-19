using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// The data model for a city object.
    /// </summary>
    [DataContract]
    class CityModel : MapObjectModel
    {
        #region CitySize

        [DataMember(Order = 4, Name = "CitySize")]
        private string _citySize;
        
        public CitySize CitySize
        {
            get { return ConvertStringToEnum<CitySize>(_citySize); }
        }

        #endregion
    }
}
