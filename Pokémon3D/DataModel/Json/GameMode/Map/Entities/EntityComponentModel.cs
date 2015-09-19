using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// A model for an additional component assigned to an entity.
    /// </summary>
    [DataContract]
    class EntityComponentModel
    {
        [DataMember(Order = 0)]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        public string Data { get; set; }
    }
}
