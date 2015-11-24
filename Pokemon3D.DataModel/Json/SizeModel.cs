using System;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json
{
    [DataContract]
    public class SizeModel : JsonDataModel<SizeModel>
    {
        [DataMember(Order = 1)]
        public int Width;

        [DataMember(Order = 2)]
        public int Height;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
