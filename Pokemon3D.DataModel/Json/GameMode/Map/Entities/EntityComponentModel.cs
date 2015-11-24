using System;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// A model for an additional component assigned to an entity.
    /// </summary>
    [DataContract]
    public class EntityComponentModel : JsonDataModel<EntityComponentModel>
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public string Data;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
