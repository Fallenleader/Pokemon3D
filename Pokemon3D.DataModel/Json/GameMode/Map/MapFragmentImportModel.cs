using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map
{
    [DataContract]
    public class MapFragmentImportModel
    {
        [DataMember(Order = 0)]
        public string Id { get; private set; }

        [DataMember(Order = 1)]
        public Vector3Model Position { get; private set; }
    }
}
