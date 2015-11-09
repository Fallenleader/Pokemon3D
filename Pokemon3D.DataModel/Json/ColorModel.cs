using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json
{
    /// <summary>
    /// The data model for an RGB color.
    /// </summary>
    [DataContract]
    public class ColorModel : JsonDataModel
    {
        /// <summary>
        /// The red part of the color.
        /// </summary>
        [DataMember(Order = 0)]
        public byte Red;

        /// <summary>
        /// The green part of the color.
        /// </summary>
        [DataMember(Order = 1)]
        public byte Green;

        /// <summary>
        /// The blue part of the color.
        /// </summary>
        [DataMember(Order = 2)]
        public byte Blue;

        /// <summary>
        /// Returns the <see cref="Color"/> corresponding to this model.
        /// </summary>
        public Color GetColor()
        {
            return new Color(Red, Green, Blue);
        }
    }
}
