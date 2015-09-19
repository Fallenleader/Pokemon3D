using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// The data model for an entity.
    /// </summary>
    [DataContract]
    class EntityModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public int Id { get; set; }

        [DataMember(Order = 1)]
        public Vector3Model Rotation { get; set; }

        [DataMember(Order = 2)]
        public bool TakeFullRotation { get; set; }

        [DataMember(Order = 3)]
        public Vector3Model Scale { get; set; }

        [DataMember(Order = 4)]
        public EntityRenderModeModel RenderMode { get; set; }

        [DataMember(Order = 5)]
        public bool Collision { get; set; }

        [DataMember(Order = 6)]
        public bool IsFloor { get; set; }

        [DataMember(Order = 7)]
        public EntityComponentModel[] Components { get; set; }
    }
}
