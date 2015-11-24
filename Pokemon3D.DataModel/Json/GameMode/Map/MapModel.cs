using System;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// A data model for a map.
    /// </summary>
    [DataContract]
    public class MapModel : JsonDataModel<MapModel>
    {
        /// <summary>
        /// Display name of the map.
        /// </summary>
        [DataMember(Order = 0)]
        public string Name;

        /// <summary>
        /// The identification of the map, which is used to reference the map.
        /// </summary>
        [DataMember(Order = 1)]
        public string Id;

        [DataMember(Order = 2)]
        public string Region;

        [DataMember(Order = 3)]
        public string Zone;

        [DataMember(Order = 4)]
        public string Song;

        [DataMember(Order = 5)]
        public string MapScript;

        [DataMember(Order = 6)]
        public string Environment;

        [DataMember(Order = 7)]
        public BattleMapDataModel BattleMapData;

        [DataMember(Order = 8)]
        public Entities.EntityFieldModel[] Entities;

        [DataMember(Order = 9)]
        public MapFragmentImportModel[] Fragments;

        [DataMember(Order = 10)]
        public OffsetMapModel[] OffsetMaps;

        public override object Clone()
        {
            var clone = (MapModel)MemberwiseClone();
            clone.BattleMapData = BattleMapData.CloneModel();
            clone.Entities = (Entities.EntityFieldModel[])Entities.Clone();
            clone.Fragments = (MapFragmentImportModel[])Fragments.Clone();
            clone.OffsetMaps = (OffsetMapModel[])OffsetMaps.Clone();
            return clone;
        }
    }
}
