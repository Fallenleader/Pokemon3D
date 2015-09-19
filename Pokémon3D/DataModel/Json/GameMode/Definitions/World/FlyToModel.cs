using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// A data model for the fly destination used by a map object.
    /// </summary>
    [DataContract]
    class FlyToModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public Vector3Model Position { get; private set; }
        
        [DataMember(Order = 1)]
        public string Mapfile { get; private set; }
    }
}
