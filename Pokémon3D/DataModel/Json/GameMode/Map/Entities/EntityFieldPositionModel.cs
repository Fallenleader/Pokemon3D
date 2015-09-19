using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// The positioning of an entity field.
    /// </summary>
    [DataContract]
    class EntityFieldPositionModel
    {
        [DataMember(Order = 0)]
        public Vector3Model Position { get; set; }

        [DataMember(Order = 1)]
        public Vector3Model Size { get; set; }

        [DataMember(Order = 2)]
        public bool Fill { get; set; }

        [DataMember(Order = 3)]
        public Vector3Model Steps { get; set; }
    }
}
