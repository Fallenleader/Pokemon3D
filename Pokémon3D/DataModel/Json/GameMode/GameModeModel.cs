using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode
{
    [DataContract]
    class GameModeModel
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

    [DataContract]
    class GameModeStartConfigurationModel
    {
        [DataMember(Order = 0)]
        public string Map;

        [DataMember(Order = 1)]
        public string Script;
    }

    [DataContract]
    class GameRuleModel
    {
        [DataMember(Order = 0)]
        public string Name;

        [DataMember(Order = 1)]
        public string Value;
    }
}
