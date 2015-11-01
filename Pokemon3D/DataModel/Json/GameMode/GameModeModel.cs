using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode
{
    [DataContract]
    class GameModeModel : JsonDataModel
    {
        // Basic information data:

        [DataMember(Order = 0)]
        public string Name;

        [DataMember(Order = 1)]
        public string Author;

        [DataMember(Order = 2)]
        public string Description;

        [DataMember(Order = 3)]
        public string Version;

        [DataMember(Order = 4)]
        public GameModeStartConfigurationModel StartConfiguration;

        [DataMember(Order = 5)]
        public GameRuleModel[] Gamerules;
    }
}
