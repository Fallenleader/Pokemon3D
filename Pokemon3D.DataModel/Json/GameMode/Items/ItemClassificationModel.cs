using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Items
{
    [DataContract]
    public class ItemClassificationModel : JsonDataModel
    {
        [DataMember]
        public bool IsBall;
        [DataMember]
        public bool IsBerry;
        [DataMember]
        public bool IsHealingItem;
        [DataMember]
        public bool IsMail;
        [DataMember]
        public bool IsMegastone;
        [DataMember]
        public bool IsPlate;
    }
}
