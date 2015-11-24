using System;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// The model defining the environment of a map.
    /// </summary>
    [DataContract]
    public class MapEnvironmentModel : JsonDataModel<MapEnvironmentModel>
    {
        /// <summary>
        /// Referenced in: <see cref="MapModel.Environment"/>.
        /// </summary>
        [DataMember(Order = 0)]
        public string Id;

        /// <summary>
        /// If the weather information in the Zone data model applied to the map will get applied.
        /// </summary>
        [DataMember(Order = 1)]
        public bool ApplyZoneData;

        /// <summary>
        /// If the map needs to get lit up by a Flash-like move.
        /// </summary>
        [DataMember(Order = 1)]
        public bool IsDark;

        [DataMember(Order = 2)]
        public string Terrain;

        [DataMember(Order = 3)]
        public string[] AllowedFieldMoves;

        [DataMember(Order = 4)]
        public MapPokemonModel PokemonData;

        /// <summary>
        /// Special rules for the map, like Safari Zone and Bug Catching Contest.
        /// </summary>
        [DataMember(Order = 5)]
        public string[] MapRules;

        /// <summary>
        /// The radio frequency that can be captured on this map.
        /// </summary>
        [DataMember(Order = 6)]
        public RangeModel RadioFrequency;

        public override object Clone()
        {
            var clone = (MapEnvironmentModel)MemberwiseClone();
            clone.AllowedFieldMoves = (string[])AllowedFieldMoves.Clone();
            clone.PokemonData = PokemonData.CloneModel();
            clone.MapRules = (string[])MapRules.Clone();
            clone.RadioFrequency = RadioFrequency.CloneModel();
            return clone;
        }
    }
}
