using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions
{
    /// <summary>
    /// The data model for the badges list.
    /// </summary>
    [DataContract]
    class BadgesModel : JsonDataModel
    {
        /// <summary>
        /// The list of badges.
        /// </summary>
        [DataMember(Order = 0)]
        public BadgeModel[] Badges { get; private set; }
    }

    /// <summary>
    /// The data model for a badge.
    /// </summary>
    [DataContract]
    class BadgeModel : JsonDataModel
    {
        /// <summary>
        /// The name of the badge.
        /// </summary>
        /// <returns></returns>
        [DataMember(Order = 0)]
        public string Name { get; private set; }

        /// <summary>
        /// The description of the badge.
        /// </summary>
        [DataMember(Order = 1)]
        public string Description { get; private set; }

        /// <summary>
        /// The texture which is the badge's visual representation.
        /// </summary>
        [DataMember(Order = 2)]
        public TextureSourceModel Texture { get; private set; }

        /// <summary>
        /// The HM move this badge unlocks.
        /// </summary>
        [DataMember(Order = 3)]
        public string MoveUnlock { get; private set; }

        /// <summary>
        /// The region this badge belongs to.
        /// </summary>
        [DataMember(Order = 4)]
        public string Region { get; private set; }
    }
}