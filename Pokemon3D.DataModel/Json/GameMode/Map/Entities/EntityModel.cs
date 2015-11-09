using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// The data model for an entity.
    /// </summary>
    [DataContract]
    public class EntityModel : JsonDataModel
    {
        /// <summary>
        /// The Id this Entity definition can be accessed with.
        /// </summary>
        [DataMember(Order = 0)]
        public int Id;

        [DataMember(Order = 1)]
        public Vector3Model Rotation;

        [DataMember(Order = 2)]
        public bool TakeFullRotation;

        [DataMember(Order = 3)]
        public Vector3Model Scale;

        [DataMember(Order = 4)]
        public EntityRenderModeModel RenderMode;

        [DataMember(Order = 5)]
        public bool Collision;

        [DataMember(Order = 6)]
        public bool IsFloor;

        [DataMember(Order = 7)]
        public EntityComponentModel[] Components;
    }
}
