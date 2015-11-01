﻿using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// The data model for a zone inside a region.
    /// </summary>
    [DataContract]
    class ZoneModel : JsonDataModel
    {
        /// <summary>
        /// Referenced in: <see cref="Map.MapModel.Zone"/>.
        /// </summary>
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public string Name;

        [DataMember(Order = 2)]
        public WeatherModel[] Weather;
    }
}
