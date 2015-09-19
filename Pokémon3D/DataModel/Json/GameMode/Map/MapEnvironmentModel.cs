using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// The model defining the environment of a map.
    /// </summary>
    [DataContract]
    class MapEnvironmentModel
    {
        [DataMember(Order = 0)]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        public int EnvironmentType { get; set; }

        [DataMember(Order = 2)]
        public int WeatherType { get; set; }

        [DataMember(Order = 3)]
        public int LightingType { get; set; }

        /// <summary>
        /// If the map needs to get lit up by a Flash-like move.
        /// </summary>
        [DataMember(Order = 4)]
        public bool IsDark { get; set; }

        [DataMember(Order = 5)]
        public string Terrain { get; set; }

        [DataMember(Order = 6)]
        public string[] AllowedFieldMoves { get; set; }

        [DataMember(Order = 7)]
        public MapPokemonModel PokemonData { get; set; }

        /// <summary>
        /// Special rules for the map, like Safari Zone and Bug Catching Contest.
        /// </summary>
        [DataMember(Order = 8)]
        public string[] MapRules { get; set; }

        /// <summary>
        /// The radio frequency that can be captured on this map.
        /// </summary>
        [DataMember(Order = 9)]
        public RangeModel RadioFrequency { get; set; }
    }
}
