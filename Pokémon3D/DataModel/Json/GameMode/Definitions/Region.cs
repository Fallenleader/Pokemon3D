using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions
{
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

    /// <summary>
    /// The data model of the regions file.
    /// </summary>
    [DataContract]
    class RegionsModel : JsonDataModel
    {
        /// <summary>
        /// The regions definition of the GameMode.
        /// </summary>
        [DataMember(Order = 0)]
        public List<RegionModel> Regions { get; private set; }
    }

    /// <summary>
    /// The data model for a region.
    /// </summary>
    [DataContract]
    class RegionModel : JsonDataModel
    {
        /// <summary>
        /// The name of this region.
        /// </summary>
        [DataMember(Order = 0)]
        public string Name { get; private set; }

        /// <summary>
        /// The zones this region is split into.
        /// </summary>
        [DataMember(Order = 1)]
        public List<ZoneModel> Zones { get; private set; }
    }

    /// <summary>
    /// The data model for a zone inside a region.
    /// </summary>
    [DataContract]
    class ZoneModel : JsonDataModel
    {
        /// <summary>
        /// The name of the zone.
        /// </summary>
        [DataMember(Order = 0)]
        public string Name { get; private set; }

        /// <summary>
        /// The weather definition for this zone.
        /// </summary>
        [DataMember(Order = 1)]
        public List<WeatherModel> Weather { get; private set; }
    }

    /// <summary>
    /// The data model for a weather definition.
    /// </summary>
    [DataContract]
    class WeatherModel : JsonDataModel
    {
        [DataMember(Order = 0, Name = "WeatherType")]
        private string _weatherType;

        /// <summary>
        /// The type of weather.
        /// </summary>
        public WeatherType WeatherType
        {
            get { return ConvertStringToEnum<WeatherType>(_weatherType); }
        }

        /// <summary>
        /// The chance that the weather type will appear in the used object.
        /// </summary>
        [DataMember(Order = 1)]
        public int Chance { get; private set; }
    }
}