using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// A field of entities, defined by a single entity definition.
    /// </summary>
    [DataContract]
    public class EntityFieldModel : JsonDataModel
    {
        [DataMember(Order = 1)]
        public EntityFieldPositionModel[] Placing;

        [DataMember(Order = 2)]
        public EntityModel Entity;
    }
}
