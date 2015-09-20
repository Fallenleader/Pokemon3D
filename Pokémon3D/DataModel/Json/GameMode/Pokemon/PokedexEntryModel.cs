using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Pokemon
{
    /// <summary>
    /// The data model for a Pokédex entry of a Pokémon.
    /// </summary>
    [DataContract]
    class PokedexEntryModel : JsonDataModel
    {
        /// <summary>
        /// The description text of the entry.
        /// </summary>
        /// <returns></returns>
        [DataMember(Order = 0)]
        public string Text { get; private set; }

        /// <summary>
        /// The species of this Pokémon.
        /// </summary>
        /// <returns></returns>
        [DataMember(Order = 1)]
        public string Species { get; private set; }

        /// <summary>
        /// The height of this Pokémon.
        /// </summary>
        [DataMember(Order = 2)]
        public double Height { get; private set; }

        /// <summary>
        /// The weight of this Pokémon.
        /// </summary>
        [DataMember(Order = 3)]
        public double Weight { get; private set; }

        /// <summary>
        /// The color associated with this Pokémon.
        /// </summary>
        [DataMember(Order = 4)]
        public ColorModel Color { get; private set; }

        /// <summary>
        /// The body style of this Pokémon. Defined BodyStyles are used.
        /// </summary>
        [DataMember(Order = 5)]
        public string BodyStyle { get; private set; }
    }
}
