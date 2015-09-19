using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// A data model for an offset map.
    /// </summary>
    [DataContract]
    class OffsetMapModel
    {
        [DataMember(Order = 0)]
        public string MapFile { get; set; }

        [DataMember(Order = 1)]
        public Vector3Model Offset { get; set; }
    }
}
