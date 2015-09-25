using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// A data model for a map.
    /// </summary>
    [DataContract]
    class MapModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Name;

        [DataMember(Order = 1)]
        public string Region;

        [DataMember(Order = 2)]
        public string Song;

        [DataMember(Order = 3)]
        public string MapScript;

        [DataMember(Order = 4)]
        public MapEnvironmentModel Environment;

        [DataMember(Order = 5)]
        public BattleMapDataModel BattleMapData;

        [DataMember(Order = 6)]
        public Entities.EntityFieldModel[] Entities;

        [DataMember(Order = 7)]
        public OffsetMapModel[] OffsetMaps;
    }
}