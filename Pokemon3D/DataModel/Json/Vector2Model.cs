using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json
{
    /// <summary>
    /// The data model for a <see cref="Vector2"/> definition.
    /// </summary>
    [DataContract]
    class Vector2Model : JsonDataModel
    {
        /// <summary>
        /// The X coordinate of this vector.
        /// </summary>
        [DataMember(Order = 0)]
        public float X;

        /// <summary>
        /// The Y coordinate of this vector.
        /// </summary>
        [DataMember(Order = 1)]
        public float Y;

        /// <summary>
        /// Returns the corresponding <see cref="Vector2"/> to this model.
        /// </summary>
        public Vector2 GetVector2()
        {
            return new Vector2(X, Y);
        }
    }
}
