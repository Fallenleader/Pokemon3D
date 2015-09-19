using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// The battle map data for this map. Can be null at runtime.
    /// </summary>
    [DataContract]
    class BattleMapDataModel : JsonDataModel
    {
        [DataMember]
        public string BattleMapFile { get; set; }

        [DataMember]
        public Vector3Model CameraPosition { get; set; }
    }
}
