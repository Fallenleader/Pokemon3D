using System;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Items.SpecialItems
{
    /// <summary>
    /// A data model for plate item definitions. These include a type.
    /// </summary>
    [DataContract]
    public class PlateModel : JsonDataModel<PlateModel>
    {
        /// <summary>
        /// The type (refer to <see cref="Definitions.TypeModel"/>).
        /// </summary>
        [DataMember]
        public string Type;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
