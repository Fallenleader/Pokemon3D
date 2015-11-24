using System;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Pokemon
{
    /// <summary>
    /// The data model for a Pokédex entry of a Pokémon.
    /// </summary>
    [DataContract]
    public class PokedexEntryModel : JsonDataModel<PokedexEntryModel>
    {
        /// <summary>
        /// The description text of the entry.
        /// </summary>
        [DataMember(Order = 0)]
        public string Text;

        /// <summary>
        /// The species of this Pokémon.
        /// </summary>
        [DataMember(Order = 1)]
        public string Species;

        /// <summary>
        /// The height of this Pokémon.
        /// </summary>
        [DataMember(Order = 2)]
        public double Height;

        /// <summary>
        /// The weight of this Pokémon.
        /// </summary>
        [DataMember(Order = 3)]
        public double Weight;

        /// <summary>
        /// The color associated with this Pokémon.
        /// </summary>
        [DataMember(Order = 4)]
        public ColorModel Color;

        /// <summary>
        /// The body style of this Pokémon. Defined BodyStyles are used.
        /// </summary>
        [DataMember(Order = 5)]
        public string BodyStyle;

        public override object Clone()
        {
            var clone = (PokedexEntryModel)MemberwiseClone();
            clone.Color = Color.CloneModel();
            return clone;
        }
    }
}
