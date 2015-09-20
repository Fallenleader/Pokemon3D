using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Pokemon
{
    /// <summary>
    /// The data model for a stat definition for a Pokémon.
    /// </summary>
    [DataContract]
    class PokemonStatSetModel : JsonDataModel
    {
        /// <summary>
        /// The Health Points stat.
        /// </summary>
        [DataMember(Order = 0)]
        public int HP { get; private set; }

        /// <summary>
        /// The Attack stat.
        /// </summary>
        [DataMember(Order = 1)]
        public int Atk { get; private set; }

        /// <summary>
        /// The Defense stat.
        /// </summary>
        [DataMember(Order = 2)]
        public int Def { get; private set; }

        /// <summary>
        /// The Special Attack stat.
        /// </summary>
        [DataMember(Order = 3)]
        public int SpAtk { get; private set; }

        /// <summary>
        /// The Special Defense stat.
        /// </summary>
        [DataMember(Order = 4)]
        public int SpDef { get; private set; }

        /// <summary>
        /// The Speed stat.
        /// </summary>
        [DataMember(Order = 5)]
        public int Speed { get; private set; }
    }
}
