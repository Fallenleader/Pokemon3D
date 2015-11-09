using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// The data model for a region.
    /// </summary>
    [DataContract]
    public class RegionModel : JsonDataModel
    {
        /// <summary>
        /// Referenced in: <see cref="Map.MapModel.Region"/>.
        /// </summary>
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public string Name;

        [DataMember(Order = 2)]
        public ZoneModel[] Zones;
    }
}