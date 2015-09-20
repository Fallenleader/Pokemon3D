using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.i18n
{
    [DataContract]
    class LocalizationModel : JsonDataModel
    {
        [DataMember]
        public TokenModel[] Tokens ;
    }

    [DataContract]
    class TokenModel : JsonDataModel
    {
        [DataMember]
        public string Id ;
        [DataMember]
        public string Val ;
    }
}
