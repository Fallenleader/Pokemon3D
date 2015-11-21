using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map.Entities
{
    [DataContract]
    public class EntityChildModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string ParentId;

        [DataMember(Order = 1)]
        public int Id;

        /////Overriding members/////

        [DataMember(Order = 2)]
        public Vector3Model Rotation;

        [DataMember(Order = 3)]
        public bool CardinalRotation;

        [DataMember(Order = 4)]
        public Vector3Model Scale;
    }
}
