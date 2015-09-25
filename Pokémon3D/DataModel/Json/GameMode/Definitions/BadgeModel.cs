using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

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
        public BadgeModel[] Badges;
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
        [DataMember(Order = 0)]
        public string Name;

        /// <summary>
        /// The description of the badge.
        /// </summary>
        [DataMember(Order = 1)]
        public string Description;

        /// <summary>
        /// The texture which is the badge's visual representation.
        /// </summary>
        [DataMember(Order = 2)]
        public TextureSourceModel Texture;

        /// <summary>
        /// The region this badge belongs to.
        /// </summary>
        [DataMember(Order = 3)]
        public string Region;
    }
}