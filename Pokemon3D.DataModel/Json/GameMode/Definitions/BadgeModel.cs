using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Definitions
{
    /// <summary>
    /// The data model for a badge.
    /// </summary>
    [DataContract]
    public class BadgeModel : JsonDataModel
    {
        /// <summary>
        /// The identification of the badge.
        /// </summary>
        [DataMember(Order = 0)]
        public string Id;

        /// <summary>
        /// The display name of the badge.
        /// </summary>
        [DataMember(Order = 1)]
        public string Name;

        /// <summary>
        /// The description of the badge.
        /// </summary>
        [DataMember(Order = 2)]
        public string Description;

        /// <summary>
        /// The texture which is the badge's visual representation.
        /// </summary>
        [DataMember(Order = 3)]
        public TextureSourceModel Texture;

        /// <summary>
        /// The region this badge belongs to.
        /// </summary>
        [DataMember(Order = 4)]
        public string Region;
    }
}