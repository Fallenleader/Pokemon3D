using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions.World
{
    #region Enums

    /// <summary>
    /// The types of weather.
    /// </summary>
    public enum WeatherType
    {
        Clear,
        Rain,
        Snow,
        Underwater,
        Sunny,
        Fog,
        Thunderstorm,
        Sandstorm,
        Ash,
        Blizzard
    }

    #endregion

    /// <summary>
    /// The data model of the regions file.
    /// </summary>
    [DataContract]
    class RegionsModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public RegionModel[] Regions { get; private set; }
    }

    /// <summary>
    /// The data model for a region.
    /// </summary>
    [DataContract]
    class RegionModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Name { get; private set; }
        
        [DataMember(Order = 1)]
        public ZoneModel[] Zones { get; private set; }
    }
}