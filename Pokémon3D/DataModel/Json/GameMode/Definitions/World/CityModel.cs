using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

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
