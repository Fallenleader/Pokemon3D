using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// A data model for a map.
    /// </summary>
    [DataContract]
    class MapModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        public string Region { get; set; }

        [DataMember(Order = 2)]
        public string Song { get; set; }

        [DataMember(Order = 3)]
        public MapEnvironmentModel Environment { get; set; }

        [DataMember(Order = 4)]
        public BattleMapDataModel BattleMapData { get; set; }

        [DataMember(Order = 5)]
        public Entities.EntityFieldModel[] Entities { get; set; }

        [DataMember(Order = 6)]
        public OffsetMapModel[] OffsetMaps { get; set; }
    }
}