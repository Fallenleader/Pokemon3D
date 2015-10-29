using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode
{
    [DataContract]
    class GameModeStartConfigurationModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Map;

        [DataMember(Order = 1)]
        public string Script;
    }
}
