using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// The data model for a zone inside a region.
    /// </summary>
    [DataContract]
    class ZoneModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Name;

        [DataMember(Order = 1)]
        public WeatherModel[] Weather;
    }
}
