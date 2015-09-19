using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.i18n
{
    [DataContract]
    class LocalizationModel : JsonDataModel
    {
        [DataMember]
        public TokenModel[] Tokens { get; set; }
    }

    [DataContract]
    class TokenModel : JsonDataModel
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Val { get; set; }
    }
}
