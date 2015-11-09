using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameCore
{
    [DataContract]
    class ConfigurationModel : JsonDataModel
    {
        public static ConfigurationModel Default
        {
            get
            {
                return new ConfigurationModel()
                {

                };
            }
        }
    }
}
