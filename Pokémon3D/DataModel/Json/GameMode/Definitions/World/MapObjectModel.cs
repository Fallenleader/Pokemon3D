using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// A base data model for the map objects.
    /// </summary>
    [DataContract]
    abstract class MapObjectModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Name { get; private set; }
        
        [DataMember(Order = 1)]
        public Vector2Model Position { get; private set; }
        
        [DataMember(Order = 2)]
        public string[] Mapfiles { get; private set; }
        
        [DataMember(Order = 3)]
        public FlyToModel FlyTo { get; private set; }
    }
}
