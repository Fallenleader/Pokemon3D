using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Pokemon
{
    /// <summary>
    /// The data model for a stat definition for a Pokémon.
    /// </summary>
    [DataContract]
    public class PokemonStatSetModel : JsonDataModel
    {
        /// <summary>
        /// The Health Points stat.
        /// </summary>
        [DataMember(Order = 0)]
        public int HP;

        /// <summary>
        /// The Attack stat.
        /// </summary>
        [DataMember(Order = 1)]
        public int Atk;

        /// <summary>
        /// The Defense stat.
        /// </summary>
        [DataMember(Order = 2)]
        public int Def;

        /// <summary>
        /// The Special Attack stat.
        /// </summary>
        [DataMember(Order = 3)]
        public int SpAtk;

        /// <summary>
        /// The Special Defense stat.
        /// </summary>
        [DataMember(Order = 4)]
        public int SpDef;

        /// <summary>
        /// The Speed stat.
        /// </summary>
        [DataMember(Order = 5)]
        public int Speed;
    }
}
