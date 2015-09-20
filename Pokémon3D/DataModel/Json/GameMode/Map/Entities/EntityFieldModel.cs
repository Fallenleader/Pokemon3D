using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// A field of entities, defined by a single entity definition.
    /// </summary>
    [DataContract]
    class EntityFieldModel
    {
        /// <summary>
        /// An option data tag, not used for anything. This can be used by map makers to store comments with their entities.
        /// </summary>
        [DataMember(Order = 0)]
        public string Tag ;

        [DataMember(Order = 1)]
        public EntityFieldPositionModel Placing ;

        [DataMember(Order = 2)]
        public EntityModel Entity ;
    }
}
