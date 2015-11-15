using System.Runtime.Serialization;

namespace Pokemon3D.DataModel.Json
{
    [DataContract]
    public class SizeModel : JsonDataModel
    {
        [DataMember(Order = 1)]
        public int Width;

        [DataMember(Order = 2)]
        public int Height;
    }
}
