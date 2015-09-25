using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// The model defining the environment of a map.
    /// </summary>
    [DataContract]
    class MapEnvironmentModel
    {
        [DataMember(Order = 0)]
        public string Name;

        [DataMember(Order = 1)]
        public int EnvironmentType;

        [DataMember(Order = 2)]
        public int WeatherType;

        [DataMember(Order = 3)]
        public int LightingType;

        /// <summary>
        /// If the map needs to get lit up by a Flash-like move.
        /// </summary>
        [DataMember(Order = 4)]
        public bool IsDark;

        [DataMember(Order = 5)]
        public string Terrain;

        [DataMember(Order = 6)]
        public string[] AllowedFieldMoves;

        [DataMember(Order = 7)]
        public MapPokemonModel PokemonData;

        /// <summary>
        /// Special rules for the map, like Safari Zone and Bug Catching Contest.
        /// </summary>
        [DataMember(Order = 8)]
        public string[] MapRules;

        /// <summary>
        /// The radio frequency that can be captured on this map.
        /// </summary>
        [DataMember(Order = 9)]
        public RangeModel RadioFrequency;
    }
}
