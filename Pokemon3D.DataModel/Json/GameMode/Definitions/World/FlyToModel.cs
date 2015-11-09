using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// A data model for the fly destination used by a map object.
    /// </summary>
    [DataContract]
    public class FlyToModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public Vector3Model Position;

        [DataMember(Order = 1)]
        public string Mapfile;
    }
}
