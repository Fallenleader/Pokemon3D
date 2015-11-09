using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json
{
    /// <summary>
    /// Describes a texture source.
    /// </summary>
    [DataContract]
    public class TextureSourceModel : JsonDataModel
    {
        /// <summary>
        /// The source file for this texture.
        /// </summary>
        [DataMember(Order = 0)]
        public string Source;

        /// <summary>
        /// The rectangle cut out of the source texture.
        /// </summary>
        [DataMember(Order = 1)]
        public RectangleModel Rectangle;
    }
}
