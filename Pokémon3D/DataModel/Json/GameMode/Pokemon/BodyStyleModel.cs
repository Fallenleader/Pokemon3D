using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Pokémon3D.DataModel.Json.GameMode.Pokemon
{
    /// <summary>
    /// The data model for the BodyStyles data file.
    /// </summary>
    [DataContract]
    class BodyStylesModel : JsonDataModel
    {
        /// <summary>
        /// The defined body styles.
        /// </summary>
        [DataMember(Order = 0)]
        public BodyStyleModel[] BodyStyles;
    }

    /// <summary>
    /// The data model for a body style of a Pokémon.
    /// </summary>
    [DataContract]
    class BodyStyleModel : JsonDataModel
    {
        /// <summary>
        /// The name of this BodyStyle.
        /// </summary>
        [DataMember(Order = 0)]
        public string Name;

        /// <summary>
        /// The texture of this BodyStyle.
        /// </summary>
        [DataMember(Order = 1)]
        public TextureSourceModel Texture;
    }
}