using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions
{
    /// <summary>
    /// The data model for the Types data file.
    /// </summary>
    [DataContract]
    class TypesModel : JsonDataModel
    {
        /// <summary>
        /// The types enumeration of the file.
        /// </summary>
        [DataMember(Order = 0)]
        public TypeModel[] Types { get; private set; }
    }

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
        public string Name { get; private set; }

        /// <summary>
        /// The color of this type.
        /// </summary>
        [DataMember(Order = 1)]
        public ColorModel Color { get; private set; }

        /// <summary>
        /// The type texture of this type.
        /// </summary>
        [DataMember(Order = 2)]
        public TextureSourceModel Texture { get; private set; }

        /// <summary>
        /// The list of types moves of this type are effective against.
        /// </summary>
        [DataMember(Order = 3)]
        public string[] Effective { get; private set; }

        /// <summary>
        /// The list of types moves of this type are not effective against.
        /// </summary>
        [DataMember(Order = 4)]
        public string[] Ineffective { get; private set; }

        /// <summary>
        /// The list of types moves of this type have no effect on.
        /// </summary>
        [DataMember(Order = 5)]
        public string[] Unaffected { get; private set; }
    }
}