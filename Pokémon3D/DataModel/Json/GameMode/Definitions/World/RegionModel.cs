using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

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
    /// The data model for a region.
    /// </summary>
    [DataContract]
    class RegionModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Name;
        
        [DataMember(Order = 1)]
        public ZoneModel[] Zones;
    }
}