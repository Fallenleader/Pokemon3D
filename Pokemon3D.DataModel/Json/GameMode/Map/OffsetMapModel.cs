using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// A data model for an offset map.
    /// </summary>
    [DataContract]
    public class OffsetMapModel : JsonDataModel<OffsetMapModel>
    {
        [DataMember(Order = 0)]
        public string MapFile;

        [DataMember(Order = 1)]
        public Vector3Model Offset;

        public override object Clone()
        {
            var clone = (OffsetMapModel)MemberwiseClone();
            clone.Offset = Offset.CloneModel();
            return clone;
        }
    }
}
