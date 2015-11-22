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
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public EntityRenderModeModel RenderMode;

        [DataMember(Order = 2)]
        public bool Collision;

        [DataMember(Order = 3)]
        public bool IsFloor;

        [DataMember(Order = 4)]
        public EntityComponentModel[] Components;
    }
}
