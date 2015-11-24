using System;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// The positioning of an entity field.
    /// </summary>
    [DataContract]
    public class EntityFieldPositionModel : JsonDataModel<EntityFieldPositionModel>
    {
        [DataMember(Order = 0)]
        public Vector3Model Position;

        [DataMember(Order = 1)]
        public Vector3Model Size;

        [DataMember(Order = 2)]
        public bool Fill;

        [DataMember(Order = 3)]
        public Vector3Model Steps;

        [DataMember(Order = 4)]
        public Vector3Model Rotation;

        [DataMember(Order = 5)]
        public bool CardinalRotation;

        [DataMember(Order = 6)]
        public Vector3Model Scale;

        public override object Clone()
        {
            var clone = (EntityFieldPositionModel)MemberwiseClone();
            clone.Position = Position.CloneModel();
            clone.Size = Size.CloneModel();
            clone.Steps = Steps.CloneModel();
            clone.Rotation = Rotation.CloneModel();
            clone.Scale = Scale.CloneModel();
            return clone;
        }
    }
}
