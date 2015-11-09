using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json
{
    /// <summary>
    /// The data model for a range.
    /// </summary>
    [DataContract]
    public class RangeModel : JsonDataModel
    {
        /// <summary>
        /// The lower bound of the range.
        /// </summary>
        [DataMember(Order = 0)]
        public double Min;

        /// <summary>
        /// The upper bound of the range.
        /// </summary>
        [DataMember(Order = 1)]
        public double Max;
    }
}
