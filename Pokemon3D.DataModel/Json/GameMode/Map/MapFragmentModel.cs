using System;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Map
{
    [DataContract]
    public class MapFragmentModel : JsonDataModel<MapFragmentModel>
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 2)]
        public Entities.EntityFieldModel[] Entities;

        public override object Clone()
        {
            var clone = (MapFragmentModel)MemberwiseClone();
            clone.Entities = (Entities.EntityFieldModel[])Entities.Clone();
            return clone;
        }
    }
}
