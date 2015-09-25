using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.i18n
{
    [DataContract]
    class LocalizationModel : JsonDataModel
    {
        [DataMember]
        public TokenModel[] Tokens;
    }

    [DataContract]
    class TokenModel : JsonDataModel
    {
        [DataMember]
        public string Id;
        [DataMember]
        public string Val;
    }
}
