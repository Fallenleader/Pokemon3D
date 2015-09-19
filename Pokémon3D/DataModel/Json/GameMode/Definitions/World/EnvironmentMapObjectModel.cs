using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// A data model for a misc environment map object for decoration.
    /// </summary>
    [DataContract]
    sealed class EnvironmentMapObjectModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public Vector2Model Position { get; private set; }
        
        [DataMember(Order = 1)]
        public float Size { get; private set; }
        
        [DataMember(Order = 2)]
        public TextureSourceModel Texture { get; private set; }
    }
}
