using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Definitions
{
    /// <summary>
    /// The data model for a Pokémon or move type in the game.
    /// </summary>
    [DataContract]
    class TypeModel : JsonDataModel
    {
        /// <summary>
        /// The name of the type.
        /// </summary>
        [DataMember(Order = 0)]
        public string Name;

        /// <summary>
        /// The color of this type.
        /// </summary>
        [DataMember(Order = 1)]
        public ColorModel Color;

        /// <summary>
        /// The type texture of this type.
        /// </summary>
        [DataMember(Order = 2)]
        public TextureSourceModel Texture;

        /// <summary>
        /// The list of types moves of this type are effective against.
        /// </summary>
        [DataMember(Order = 3)]
        public string[] Effective;

        /// <summary>
        /// The list of types moves of this type are not effective against.
        /// </summary>
        [DataMember(Order = 4)]
        public string[] Ineffective;

        /// <summary>
        /// The list of types moves of this type have no effect on.
        /// </summary>
        [DataMember(Order = 5)]
        public string[] Unaffected;
    }
}