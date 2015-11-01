using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// The model defining the environment of a map.
    /// </summary>
    [DataContract]
    class MapEnvironmentModel : JsonDataModel
    {
        /// <summary>
        /// Referenced in: <see cref="MapModel.Environment"/>.
        /// </summary>
        [DataMember(Order = 0)]
        public string Id { get; }

        /// <summary>
        /// If the weather information in the Zone data model applied to the map will get applied.
        /// </summary>
        [DataMember(Order = 1)]
        public bool ApplyZoneData { get; }

        /// <summary>
        /// If the map needs to get lit up by a Flash-like move.
        /// </summary>
        [DataMember(Order = 1)]
        public bool IsDark { get; }

        [DataMember(Order = 2)]
        public string Terrain { get; }

        [DataMember(Order = 3)]
        public string[] AllowedFieldMoves { get; }

        [DataMember(Order = 4)]
        public MapPokemonModel PokemonData { get; }

        /// <summary>
        /// Special rules for the map, like Safari Zone and Bug Catching Contest.
        /// </summary>
        [DataMember(Order = 5)]
        public string[] MapRules { get; }

        /// <summary>
        /// The radio frequency that can be captured on this map.
        /// </summary>
        [DataMember(Order = 6)]
        public RangeModel RadioFrequency { get; }
    }
}
