using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// A data model for a misc environment map object for decoration.
    /// </summary>
    [DataContract]
    sealed public class EnvironmentMapObjectModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public Vector2Model Position;
        
        [DataMember(Order = 1)]
        public float Size;

        [DataMember(Order = 2)]
        public TextureSourceModel Texture;
    }
}
